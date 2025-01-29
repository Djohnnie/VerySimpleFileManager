using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;
using VerySimpleFileManager.Helpers;
using Wpf.Ui;

namespace VerySimpleFileManager.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly DispatcherTimer _fileScanPollingTimer = new();
    private readonly FileIndexerHelper _fileIndexerHelper;

    [ObservableProperty]
    private ObservableCollection<DashboardDrive> _drives = new();

    public DashboardViewModel(
        INavigationService navigationService,
        FileIndexerHelper fileIndexerHelper)
    {
        _navigationService = navigationService;

        _fileScanPollingTimer.Interval = TimeSpan.FromSeconds(1);
        _fileScanPollingTimer.Tick += OnFileScanPollingTimerTick;
        _fileScanPollingTimer.Start();

        _fileIndexerHelper = fileIndexerHelper;
    }

    private void OnFileScanPollingTimerTick(object sender, EventArgs e)
    {
        foreach (var drive in _fileIndexerHelper.Drives)
        {
            var mappedDrive = Drives.SingleOrDefault(x => x.Name == drive.Name);

            if (mappedDrive is null)
            {
                Drives.Add(new DashboardDrive(Navigate, drive.Name, $"{drive.Name} ({drive.Label})", drive.CountFiles(), drive.IsIndexed));
            }
            else
            {
                mappedDrive.Description = drive.CountFiles();
                mappedDrive.IsIndexed = drive.IsIndexed;
            }
        }
    }

    private void Navigate(string driveName)
    {
        _navigationService.Navigate(driveName);
    }
}

public partial class DashboardDrive : ObservableObject
{
    private readonly Action<string> _navigate;

    public DashboardDrive(Action<string> navigate, string name, string label, string description, bool isIndexed)
    {
        _navigate = navigate;

        _name = name;
        _label = label;
        _description = description;
        _isIndexed = isIndexed;
    }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _label;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private bool _isIndexed;

    [RelayCommand]
    private void OnClick(object sender)
    {
        if (_navigate != null)
        {
            _navigate(_name);
        }
    }
}