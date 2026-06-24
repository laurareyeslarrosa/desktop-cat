using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace DesktopCat;

public partial class MainWindow : Window
{
    private Point _dragStartPosition;
    private bool _isDragging;

    public MainWindow()
    {
        InitializeComponent();
        
        // Cargar la imagen desde los recursos
        var image = this.FindControl<Image>("GatitoImagen");
        if (image != null)
        {
            try
            {
                var uri = new Uri("avares://Assets/Images/gatito.png");
                image.Source = new Bitmap(AssetLoader.Open(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando imagen: {ex.Message}");
            }
        }
    }

    private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDragging = true;
            _dragStartPosition = e.GetPosition(this);
        }
    }

    private void Window_PointerMoved(object sender, PointerEventArgs e)
    {
        if (_isDragging)
        {
            var currentPosition = e.GetPosition(this);
            var delta = currentPosition - _dragStartPosition;
            
            // Mover la ventana
            var newPosition = this.Position + new PixelPoint((int)delta.X, (int)delta.Y);
            this.Position = newPosition;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        _isDragging = false;
        base.OnPointerReleased(e);
    }
}
