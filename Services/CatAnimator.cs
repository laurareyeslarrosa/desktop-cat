using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
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
    private readonly Bitmap _front1;
    private readonly Bitmap _front2;
    private readonly Bitmap _back1;
    private readonly Bitmap _back2;
    private readonly Bitmap _sleep;
    private readonly Bitmap _groom;

    private readonly Random _random = new();

    private double _frameElapsedMs;
    private bool _showFirstFrame = true;
    private CatDirection? _direction;
    private string _state = "Idle";

    public CatAnimator(Image image)
    {
        _image = image;

        _idle = Load(Constants.IdlePath);
        _walk1 = Load(Constants.Walk1Path);
        _walk2 = Load(Constants.Walk2Path);
        _front1 = Load(Constants.WalkFront1Path);
        _front2 = Load(Constants.WalkFront2Path);
        _back1 = Load(Constants.WalkBack1Path);
        _back2 = Load(Constants.WalkBack2Path);
        _sleep = Load(Constants.SleepPath);
        _groom = Load(Constants.GroomPath);

        SetIdle();
    }

    private static Bitmap Load(string path)
    {
        return new Bitmap(AssetLoader.Open(new Uri(path)));
    }

    public void SetIdle()
    {
        _state = "Idle";
        _direction = null;
        _frameElapsedMs = 0;

        // La pose de repos nunca queda espejada.
        _image.RenderTransform = null;
        _image.Source = _idle;
    }

    public void SetWalking(CatDirection direction)
    {
        if (_state == "Walking" && _direction == direction)
            return;

        _state = "Walking";
        _direction = direction;
        _frameElapsedMs = 0;
        _showFirstFrame = true;

        // Espejar solamente el perfil lateral.
        _image.RenderTransform = direction == CatDirection.Left
            ? new ScaleTransform(-1, 1)
            : null;

        _image.RenderTransformOrigin = new RelativePoint(
            0.5, 0.5, RelativeUnit.Relative);

        SetFrame();
    }

    public void UpdateWalking(double elapsedMs)
    {
        if (_state != "Walking" || _direction == null)
            return;

        _frameElapsedMs += elapsedMs;

        if (_frameElapsedMs < Constants.WalkFrameDurationMs)
            return;

        _frameElapsedMs %= Constants.WalkFrameDurationMs;
        _showFirstFrame = !_showFirstFrame;

        SetFrame();
    }

    private void SetFrame()
    {
        if (_direction == null)
            return;

        _image.Source = _direction.Value switch
        {
            CatDirection.Left =>
                _showFirstFrame ? _walk1 : _walk2,

            CatDirection.Right =>
                _showFirstFrame ? _walk1 : _walk2,

            CatDirection.Front =>
                _showFirstFrame ? _front1 : _front2,

            CatDirection.Back =>
                _showFirstFrame ? _back1 : _back2,

            _ => _idle
        };
    }

    public void SetResting()
    {
        _state = "Resting";
        _direction = null;
        _frameElapsedMs = 0;

        _image.RenderTransform = null;

        // Elegir dormir o asearse al azar.
        _image.Source = _random.Next(2) == 0
            ? _sleep
            : _groom;
    }

    public void Dispose()
    {
        _idle.Dispose();
        _walk1.Dispose();
        _walk2.Dispose();
        _front1.Dispose();
        _front2.Dispose();
        _back1.Dispose();
        _back2.Dispose();
        _sleep.Dispose();
        _groom.Dispose();
    }
}
