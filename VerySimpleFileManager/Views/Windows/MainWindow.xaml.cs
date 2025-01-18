using VerySimpleFileManager.Helpers;
using VerySimpleFileManager.ViewModels.Windows;
using VerySimpleFileManager.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace VerySimpleFileManager.Views.Windows;

public partial class MainWindow : INavigationWindow
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(
        MainWindowViewModel viewModel,
        CommandLineArgumentHelper commandLineArgumentHelper,
        IPageService pageService,
        INavigationService navigationService
    )
    {
        ViewModel = viewModel;
        DataContext = this;

        SystemThemeWatcher.Watch(this);

        InitializeComponent();
        SetPageService(pageService);

        RootNavigation.Navigated += RootNavigation_Navigated;

        navigationService.SetNavigationControl(RootNavigation);

        Task.Run(async () =>
        {
            await Task.Delay(1000);
            Dispatcher.Invoke(() =>
            {
                WindowState = WindowState.Maximized;

                if (commandLineArgumentHelper.Arguments.Count == 1)
                {
                    var drive = commandLineArgumentHelper.Arguments[0];
                    navigationService.Navigate(drive);
                }
            });
        });
    }

    private void RootNavigation_Navigated(NavigationView sender, NavigatedEventArgs args)
    {
        if (args.Page is FileBrowserPage fileBrowserPage)
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Dispatcher.Invoke(() =>
                {
                    fileBrowserPage.SetPageTag(RootNavigation.SelectedItem.TargetPageTag);
                });
            });
        }
    }

    #region INavigationWindow methods

    public INavigationView GetNavigation() => RootNavigation;

    public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

    public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();

    #endregion INavigationWindow methods

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        // Make sure that closing this window will begin the process of closing the application.
        Application.Current.Shutdown();
    }

    INavigationView INavigationWindow.GetNavigation()
    {
        throw new NotImplementedException();
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}