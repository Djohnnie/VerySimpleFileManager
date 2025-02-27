﻿using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VerySimpleFileManager.Helpers;
using VerySimpleFileManager.Services;
using VerySimpleFileManager.ViewModels.Pages;
using VerySimpleFileManager.ViewModels.Windows;
using VerySimpleFileManager.Views.Pages;
using VerySimpleFileManager.Views.Windows;
using VerySimpleFileManager.Workers;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace VerySimpleFileManager;

public partial class App
{
    private static readonly IHost _host = Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
        .ConfigureServices((context, services) =>
        {
            services.AddNavigationViewPageProvider();

            services.AddHostedService<ApplicationHostService>();
            services.AddHostedService<FileIndexerWorker>();

            // Theme manipulation
            services.AddSingleton<IThemeService, ThemeService>();

            // TaskBar manipulation
            services.AddSingleton<ITaskBarService, TaskBarService>();

            // Service containing navigation, same as INavigationWindow... but without window
            services.AddSingleton<INavigationService, NavigationService>();

            // Main window with navigation
            services.AddSingleton<INavigationWindow, MainWindow>();
            services.AddSingleton<MainWindowViewModel>();

            services.AddSingleton<DashboardPage>();
            services.AddSingleton<DashboardViewModel>();

            services.AddTransient<FileBrowserPage>();
            services.AddTransient<FileBrowserViewModel>();

            services.AddSingleton<SettingsPage>();
            services.AddSingleton<SettingsViewModel>();


            services.AddSingleton<CommandLineArgumentHelper>();
            services.AddSingleton<FileIndexerHelper>();
        }).Build();

    public static IServiceProvider Services
    {
        get { return _host.Services; }
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}