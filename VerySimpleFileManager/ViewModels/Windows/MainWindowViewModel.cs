using System.Collections.ObjectModel;
using System.IO;
using VerySimpleFileManager.Helpers;
using VerySimpleFileManager.Models;
using Wpf.Ui.Controls;

namespace VerySimpleFileManager.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _applicationTitle = "Eenvoudige fotobeheer";

    [ObservableProperty]
    private ObservableCollection<object> _menuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Start",
            ToolTip = "Start",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
            TargetPageType = typeof(Views.Pages.DashboardPage)
        },
    };

    [ObservableProperty]
    private ObservableCollection<object> _footerMenuItems = new()
    {
        new NavigationViewItem()
        {
            Content = "Instellingen",
            ToolTip = "Instellingen",
            Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
            TargetPageType = typeof(Views.Pages.SettingsPage)
        }
    };

    [ObservableProperty]
    private ObservableCollection<MenuItem> _trayMenuItems = new()
    {
        new MenuItem { Header = "Start", Tag = "tray_home" }
    };

    public MainWindowViewModel(FileIndexerHelper fileIndexerHelper)
    {
        var drives = DriveInfo.GetDrives();
        foreach (var drive in drives)
        {
            if (drive.IsReady)
            {
                var fileIndexerDrive = fileIndexerHelper.Drives.FirstOrDefault(d => d.Name == drive.Name);

                if (fileIndexerDrive == null)
                {
                    fileIndexerHelper.Drives.Add(new Drive()
                    {
                        Name = drive.Name,
                        Label = drive.VolumeLabel,
                        IsIndexed = false
                    });
                }

                var description = $"{drive.Name} ({drive.VolumeLabel})";

                var navigationItem = new NavigationViewItem()
                {
                    Content = description,
                    ToolTip = description,
                    Icon = new SymbolIcon { Symbol = SymbolRegular.UsbStick24 },
                    TargetPageType = typeof(Views.Pages.FileBrowserPage),
                    TargetPageTag = drive.Name,
                };

                _menuItems.Add(navigationItem);
            }
        }
    }
}