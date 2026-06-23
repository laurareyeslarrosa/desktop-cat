using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace GatitoEscritorio;

public partial class App : Application
{
    private TrayIconManager? _trayManager;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();
            desktop.MainWindow = mainWindow;
            
            // Inicializar el sistema de bandeja
            _trayManager = new TrayIconManager(mainWindow);
            _trayManager.Initialize();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
