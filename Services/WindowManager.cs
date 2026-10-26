using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DesktopCat.Services;

public enum CatDirection
{
    Left,
    Right,
    Front,
    Back
}

public class WindowManager
{
    private readonly Window _window;
    private readonly Stopwatch _clock = Stopwatch.StartNew();
    private bool _isDragging;
    private Point _dragStart;
    private PixelPoint _windowStart;
    private Point _lastCursor;
    private bool _hasLastCursor;
    private long _lastCursorMovementMs;

    [StructLayout(LayoutKind.Sequential)]
    private struct NativePoint
    {
        public int X;
        public int Y;
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out NativePoint point);

    public WindowManager(Window window)
    {
        _window = window;
    }

    public bool IsDragging => _isDragging;

    public CatDirection CurrentDirection { get; private set; }
        = CatDirection.Left;

    public long CursorIdleMs =>
        _clock.ElapsedMilliseconds - _lastCursorMovementMs;

    public void BeginDrag(PointerEventArgs e)
    {
        _isDragging = true;
        _dragStart = e.GetPosition(_window);
        _windowStart = _window.Position;
    }

    public void Drag(PointerEventArgs e)
    {
        if (!_isDragging)
            return;

        var current = e.GetPosition(_window);

        var dx = current.X - _dragStart.X;
        var dy = current.Y - _dragStart.Y;

        _window.Position = new PixelPoint(
            _windowStart.X + (int)Math.Round(dx),
            _windowStart.Y + (int)Math.Round(dy));
    }

    public void EndDrag()
    {
        _isDragging = false;
        _hasLastCursor = false;
    }

    public bool FollowCursor(double elapsedSeconds)
    {
        if (_isDragging)
            return false;

        if (!GetCursorPos(out var native))
            return false;

        var cursor = new Point(native.X, native.Y);
        var now = _clock.ElapsedMilliseconds;

        if (!_hasLastCursor ||
            Distance(cursor, _lastCursor) >=
            Constants.CursorMovementThreshold)
        {
            _lastCursor = cursor;
            _hasLastCursor = true;
            _lastCursorMovementMs = now;
        }

        var targetX = native.X + Constants.CursorOffsetX;
        var targetY = native.Y + Constants.CursorOffsetY;

        var current = _window.Position;
        var dx = targetX - current.X;
        var dy = targetY - current.Y;

        var distance = Math.Sqrt(dx * dx + dy * dy);

        if (distance <= Constants.StopDistance)
            return true;

        // Elegir la orientación según el eje predominante.
        if (Math.Abs(dx) > Math.Abs(dy))
        {
            CurrentDirection = dx < 0
                ? CatDirection.Left
                : CatDirection.Right;
        }
        else
        {
            CurrentDirection = dy < 0
                ? CatDirection.Back
                : CatDirection.Front;
        }

        // Evitar saltos grandes si el sistema se ralentiza.
        elapsedSeconds = Math.Clamp(elapsedSeconds, 0, 0.05);

        var step = Math.Min(
            Constants.FollowSpeed * elapsedSeconds,
            distance - Constants.StopDistance);

        _window.Position = new PixelPoint(
            (int)Math.Round(current.X + dx / distance * step),
            (int)Math.Round(current.Y + dy / distance * step));

        return false;
    }

     private static double Distance(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }

    public void CenterWindow()
    {
        var screen = _window.Screens.Primary;

        if (screen == null)
            return;

        var x = screen.Bounds.X +
                (screen.Bounds.Width - _window.Width) / 2;

        var y = screen.Bounds.Y +
                (screen.Bounds.Height - _window.Height) / 2;

        _window.Position = new PixelPoint(
            (int)Math.Round(x),
            (int)Math.Round(y));
    }

    public void MoveTo(int x, int y)
    {
        _window.Position = new PixelPoint(x, y);
    }
}
