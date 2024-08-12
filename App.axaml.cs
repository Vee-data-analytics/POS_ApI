using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using System;

namespace POSApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            Console.WriteLine("App Initialize called.");
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            Console.WriteLine("OnFrameworkInitializationCompleted called.");
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LoginWindow();
                desktop.MainWindow.Opened += (s, e) => Console.WriteLine("Login window opened.");
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
