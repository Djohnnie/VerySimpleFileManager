using System.Windows.Threading;
using VerySimpleFileManager.Helpers;
using Wpf.Ui.Controls;

namespace VerySimpleFileManager.ViewModels.Pages;

public partial class FileBrowserViewModel : ObservableObject
{
    private readonly DispatcherTimer _fileScanPollingTimer = new();
    private readonly FileIndexerHelper _fileIndexerHelper;

    [ObservableProperty]
    private string _temp = string.Empty;

    [ObservableProperty]
    private string _fileScanPollingMessage = "Bestanden worden doorzocht...";

    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;

    public FileBrowserViewModel(FileIndexerHelper fileIndexerHelper)
    {
        _fileScanPollingTimer.Interval = TimeSpan.FromSeconds(1);
        _fileScanPollingTimer.Tick += OnFileScanPollingTimerTick;
        _fileScanPollingTimer.Start();

        _fileIndexerHelper = fileIndexerHelper;
    }

    private void OnFileScanPollingTimerTick(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_temp))
        {
            var drive = _fileIndexerHelper.Drives.FirstOrDefault(x => x.Name == _temp);
            var count = drive.Where(x => x.Name.EndsWith("jpg")).Count;
            FileScanPollingMessage = $"{(count == 0 ? "Geen" : count)} foto's gevonden...";

            if (drive.IsIndexed)
            {
                InfoBarSeverity = InfoBarSeverity.Success;
            }
        }
    }
}