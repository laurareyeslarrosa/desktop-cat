
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using System;

namespace DesktopCat;

public partial class App : Application
{
    private TrayIconManager? _trayManager;
    private WindowNotificationManager? _notificationManager;

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
            
            // Inicializar notificaciones
            _notificationManager = new WindowNotificationManager(mainWindow)
            {
                Position = NotificationPosition.BottomRight,
                MaxItems = 5
            };
            
            // Inicializar Tray Icon
            _trayManager = new TrayIconManager(mainWindow, _notificationManager);
            _trayManager.Initialize();
        }

        base.OnFrameworkInitializationCompleted();
    }
}