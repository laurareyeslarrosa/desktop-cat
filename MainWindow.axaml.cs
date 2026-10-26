
using Avalonia;
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
    private bool _wasWalking;

    public MainWindow()
    {
        InitializeComponent();

        _windowManager = new WindowManager(this);

        var catImage = this.FindControl<Image>("CatImage");

        if (catImage != null)
            _animator = new CatAnimator(catImage);

        _timer.Interval =
            TimeSpan.FromMilliseconds(Constants.UpdateIntervalMs);

        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        var now = _clock.ElapsedMilliseconds;

        var elapsedMs = now - _previousTickMs;
        _previousTickMs = now;

        if (_windowManager.IsDragging || _animator == null)
            return;

        var arrived = _windowManager.FollowCursor(
            elapsedMs / 1000.0);

        var isWalking = !arrived;

        if (isWalking)
        {
            _animator.UpdateWalking(elapsedMs);
        }
        else if (_wasWalking)
        {
            _animator.SetIdle();
        }

        _wasWalking = isWalking;
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
    }

    protected override void OnClosed(EventArgs e)
    {
        _timer.Stop();
        _timer.Tick -= OnTimerTick;

        _animator?.Dispose();

        base.OnClosed(e);
    }
}
