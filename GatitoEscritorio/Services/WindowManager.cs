using Avalonia.Controls;
using System;

namespace GatitoEscritorio.Services;

public class WindowManager
{
    private readonly Window _window;

    public WindowManager(Window window)
    {
        _window = window;
    }

    public void CenterWindow()
    {
        var screen = _window.Screens.Primary;
        if (screen != null)
        {
            var x = (screen.Bounds.Width - _window.Width) / 2;
            var y = (screen.Bounds.Height - _window.Height) / 2;
            _window.Position = new PixelPoint((int)x, (int)y);
        }
    }

    public void MoveTo(int x, int y)
    {
        _window.Position = new PixelPoint(x, y);
    }
}
