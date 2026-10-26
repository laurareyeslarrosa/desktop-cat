using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using DesktopCat.Services;
using System;
using System.Diagnostics;

namespace DesktopCat;

public partial class MainWindow : Window
{
    private readonly WindowManager _windowManager;
    private readonly DispatcherTimer _timer = new();
    private readonly Stopwatch _clock = Stopwatch.StartNew();

    private CatAnimator? _animator;

    private long _previousTickMs;
    private long? _arrivedAtMs;

    private bool _wasWalking;
    private bool _isResting;

    public MainWindow()
    {
        InitializeComponent();

        _windowManager = new WindowManager(this);

        var catImage = this.FindControl<Image>("CatImage");

        if (catImage != null)
            _animator = new CatAnimator(catImage);

        _previousTickMs = _clock.ElapsedMilliseconds;

        _timer.Interval =
            TimeSpan.FromMilliseconds(Constants.UpdateIntervalMs);

        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (_animator == null || _windowManager.IsDragging)
            return;

        var now = _clock.ElapsedMilliseconds;
        var elapsedMs = now - _previousTickMs;
        _previousTickMs = now;

        var arrived = _windowManager.FollowCursor(
            elapsedMs / 1000.0);

        if (!arrived)
        {
            // Sigue al cursor: caminar y animar.
            _arrivedAtMs = null;
            _isResting = false;
            _wasWalking = true;

            _animator.SetWalking(
                _windowManager.CurrentDirection);

            _animator.UpdateWalking(elapsedMs);
            return;
        }

        // Llegó al objetivo.
        if (_arrivedAtMs == null || _wasWalking)
        {
            _arrivedAtMs = now;
            _animator.SetIdle();
        }

        _wasWalking = false;

        // Si el cursor sigue moviéndose cerca de Bellota,
        // reiniciar el tiempo de inactividad.
        if (_windowManager.CursorIdleMs < 100)
        {
            _arrivedAtMs = now;
            _isResting = false;
            _animator.SetIdle();
            return;
        }

        // Después de 2 segundos, elegir dormir o asearse.
        if (!_isResting &&
            now - _arrivedAtMs.Value >= Constants.IdleBeforeRestMs)
        {
            _animator.SetResting();
            _isResting = true;
        }
    }

    private void Window_PointerPressed(
        object? sender,
        PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            return;

        _windowManager.BeginDrag(e);
        e.Pointer.Capture(this);
    }

    private void Window_PointerMoved(
        object? sender,
        PointerEventArgs e)
    {
        _windowManager.Drag(e);
    }

    private void Window_PointerReleased(
        object? sender,
        PointerReleasedEventArgs e)
    {
        _windowManager.EndDrag();
        e.Pointer.Capture(null);

        _arrivedAtMs = null;
        _isResting = false;
        _wasWalking = false;

        _animator?.SetIdle();
    }

    protected override void OnClosed(EventArgs e)
    {
        _timer.Stop();
        _timer.Tick -= OnTimerTick;

        _animator?.Dispose();

        base.OnClosed(e);
    }
}
