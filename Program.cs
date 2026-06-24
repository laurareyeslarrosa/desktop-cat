/*
using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace DesktopCat;

class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
*/

using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace DesktopCat;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Console.WriteLine("🚀 Iniciando programa...");
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
            Console.WriteLine("✅ Programa finalizado correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}