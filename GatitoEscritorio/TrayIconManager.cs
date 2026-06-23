using Avalonia.Controls;
using Avalonia.Threading;
using System;

namespace GatitoEscritorio;

public class TrayIconManager
{
    private readonly Window _mainWindow;
    private readonly TrayIcon _trayIcon;
    private bool _isWindowVisible;

    public TrayIconManager(Window mainWindow)
    {
        _mainWindow = mainWindow;
        _isWindowVisible = true;

        // Crear el icono de bandeja
        _trayIcon = new TrayIcon
        {
            Icon = new WindowIcon(new Uri("avares://GatitoEscritorio/Assets/Images/gatito.png")),
            ToolTipText = "Gatito de Vic"
        };

        // Crear el menú contextual
        var menu = new NativeMenu();
        
        var showItem = new NativeMenuItem("Mostrar");
        showItem.Click += (s, e) => ShowWindow();
        menu.Add(showItem);

        var hideItem = new NativeMenuItem("Ocultar");
        hideItem.Click += (s, e) => HideWindow();
        menu.Add(hideItem);

        menu.Add(new NativeMenuItemSeparator());

        var messageItem = new NativeMenuItem("Enviar mensaje");
        messageItem.Click += (s, e) => ShowMessage();
        menu.Add(messageItem);

        menu.Add(new NativeMenuItemSeparator());

        var quitItem = new NativeMenuItem("Salir");
        quitItem.Click += (s, e) => QuitApplication();
        menu.Add(quitItem);

        _trayIcon.Menu = menu;

        // Manejar clicks en el icono
        _trayIcon.Clicked += OnTrayIconClicked;

        // Manejar evento de cierre de ventana
        _mainWindow.Closing += (s, e) =>
        {
            // No cerrar realmente, solo ocultar
            e.Cancel = true;
            HideWindow();
        };
    }

    private void OnTrayIconClicked(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (_isWindowVisible)
                HideWindow();
            else
                ShowWindow();
        });
    }

    private void ShowWindow()
    {
        Dispatcher.UIThread.Post(() =>
        {
            _mainWindow.Show();
            _mainWindow.Activate();
            _mainWindow.WindowState = WindowState.Normal;
            _isWindowVisible = true;
        });
    }

    private void HideWindow()
    {
        Dispatcher.UIThread.Post(() =>
        {
            _mainWindow.Hide();
            _isWindowVisible = false;
        });
    }

    private void ShowMessage()
    {
        Dispatcher.UIThread.Post(() =>
        {
            _trayIcon.ShowBalloonTip("¡Hola!", "Haz click en el icono para ocultar/mostrar", 
                BalloonIcon.Info);
        });
    }

    private void QuitApplication()
    {
        // Limpiar recursos
        _trayIcon.Dispose();
        Environment.Exit(0);
    }

    public void Initialize()
    {
        // El TrayIcon se mostrará automáticamente al crearse
    }
}
