using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Runtime.InteropServices;

namespace DesktopCat.Services;

public class WindowManager
{
    private readonly Window _window;
    private bool _isDragging;
    private Point _dragStart;
    private PixelPoint _windowStart;

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
    }

    public bool FollowCursor(double elapsedSeconds)
    {
        if (_isDragging)
            return false;

        if (!GetCursorPos(out var cursor))
            return false;

        var targetX = cursor.X + Constants.CursorOffsetX;
        var targetY = cursor.Y + Constants.CursorOffsetY;

        var current = _window.Position;

        var dx = targetX - current.X;
        var dy = targetY - current.Y;

        var distance = Math.Sqrt(dx * dx + dy * dy);

        if (distance <= Constants.StopDistance)
            return true;

        // Evita saltos si la aplicación se pausa brevemente.
        elapsedSeconds = Math.Clamp(elapsedSeconds, 0, 0.05);

        var step = Math.Min(
            Constants.FollowSpeed * elapsedSeconds,
            distance - Constants.StopDistance);

        var nextX = current.X + dx / distance * step;
        var nextY = current.Y + dy / distance * step;

        _window.Position = new PixelPoint(
            (int)Math.Round(nextX),
            (int)Math.Round(nextY));

        return false;
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
