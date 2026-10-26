using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace DesktopCat.Services;

public class CatAnimator : IDisposable
{
    private readonly Image _image;

    private readonly Bitmap _idle;
    private readonly Bitmap _walk1;
    private readonly Bitmap _walk2;
    private readonly Bitmap _sleep;
    private readonly Bitmap _groom;

    private bool _showWalk1;
    private double _frameElapsedMs;

    public CatAnimator(Image image)
    {
        _image = image;

        _idle = LoadBitmap(Constants.IdlePath);
        _walk1 = LoadBitmap(Constants.Walk1Path);
        _walk2 = LoadBitmap(Constants.Walk2Path);
        _sleep = LoadBitmap(Constants.SleepPath);
        _groom = LoadBitmap(Constants.GroomPath);

        SetIdle();
    }

    private static Bitmap LoadBitmap(string path)
    {
        return new Bitmap(
            AssetLoader.Open(new Uri(path)));
    }

    public void SetIdle()
    {
        _frameElapsedMs = 0;
        _showWalk1 = false;
        _image.Source = _idle;
    }

    public void UpdateWalking(double elapsedMs)
    {
        _frameElapsedMs += elapsedMs;

        if (_frameElapsedMs < Constants.WalkFrameDurationMs)
            return;

        _frameElapsedMs %= Constants.WalkFrameDurationMs;

        _showWalk1 = !_showWalk1;
        _image.Source = _showWalk1 ? _walk1 : _walk2;
    }

    public void Dispose()
    {
        _idle.Dispose();
        _walk1.Dispose();
        _walk2.Dispose();
        _sleep.Dispose();
        _groom.Dispose();
    }
}