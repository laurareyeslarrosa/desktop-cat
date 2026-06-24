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
    private int _clickCount = 0;

    public MainWindow()
    {
        InitializeComponent();
        
        // Cargar la imagen
        var image = this.FindControl<Image>("GatitoImagen");
        if (image != null)
        {
            try
            {
                var uri = new Uri(Constants.ImagePath);
                image.Source = new Bitmap(AssetLoader.Open(uri));
                Console.WriteLine("✅ Imagen cargada correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }

    private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        // Contar clicks
        _clickCount++;
        Console.WriteLine($"🖱️ Click #{_clickCount} en la ventana");
        
        // Actualizar el contador si existe
        var counter = this.FindControl<TextBlock>("ClickCounter");
        if (counter != null)
        {
            counter.Text = $"Clicks: {_clickCount}";
        }
        
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