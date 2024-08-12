using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace POSApp
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Application starting...");
                BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
            }
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            Console.WriteLine("Building Avalonia app...");
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .AfterSetup(AfterSetupCallback);
        }

        private static void AfterSetupCallback(AppBuilder builder)
        {
            Console.WriteLine("Avalonia setup completed.");
        }
    }
}