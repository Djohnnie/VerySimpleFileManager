using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Threading;
using VerySimpleFileManager.Helpers;
using VerySimpleFileManager.Models;
using Wpf.Ui.Controls;

namespace VerySimpleFileManager.ViewModels.Pages;

public partial class FileBrowserViewModel : ObservableObject, IDisposable
{
    private readonly DispatcherTimer _fileScanPollingTimer = new();
    private readonly DispatcherTimer _directoryScanPollingTimer = new();
    private readonly FileIndexerHelper _fileIndexerHelper;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    [ObservableProperty]
    private string _temp = string.Empty;

    [ObservableProperty]
    private string _fileScanPollingMessage = "Bestanden worden doorzocht...";

    [ObservableProperty]
    private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;

    [ObservableProperty]
    private ObservableCollection<FileBrowserFolder> _folders = new();

    [ObservableProperty]
    private FileBrowserFolder _selectedFolder;
    private FileBrowserFolder _previousSelectedFolder;

    [ObservableProperty]
    public partial ObservableCollection<FileBrowserFile> Files { get; set; }

    public FileBrowserViewModel(FileIndexerHelper fileIndexerHelper)
    {
        _fileScanPollingTimer.Interval = TimeSpan.FromSeconds(10);
        _fileScanPollingTimer.Tick += OnFileScanPollingTimerTick;
        _fileScanPollingTimer.Start();

        _directoryScanPollingTimer.Interval = TimeSpan.FromMilliseconds(100);
        _directoryScanPollingTimer.Tick += OnDirectoryScanPollingTimerTick;
        _directoryScanPollingTimer.Start();

        _fileIndexerHelper = fileIndexerHelper;
    }

    private void OnDirectoryScanPollingTimerTick(object sender, EventArgs e)
    {
        if (SelectedFolder != _previousSelectedFolder)
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            }

            _previousSelectedFolder = SelectedFolder;
            SyncFilesToFolder(_cancellationTokenSource.Token);
        }
    }

    private void OnFileScanPollingTimerTick(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Temp))
        {
            var drive = _fileIndexerHelper.Drives.FirstOrDefault(x => x.Name == Temp);
            if (drive != null)
            {
                FileScanPollingMessage = drive.CountFiles();

                if (drive.IsIndexed)
                {
                    InfoBarSeverity = InfoBarSeverity.Success;
                }

                SyncDriveToFolders(drive);
            }
        }
    }

    private void SyncFilesToFolder(CancellationToken cancellationToken)
    {
        var files = SelectedFolder.Files.ToList();
        Files = new ObservableCollection<FileBrowserFile>(files);

        _ = Parallel.ForEachAsync(files, cancellationToken, async (file, token) =>
        {
            string[] extensions = ["jpg", "jpeg", "png", "gif", "bmp"];
            if (extensions.Contains(file.Name.ToLower().Split('.').Last()))
            {
                try
                {
                    using var image = await SixLabors.ImageSharp.Image.LoadAsync(file.Path);
                    image.Mutate(x => x.Resize(100, 100));
                    using MemoryStream memStream = new MemoryStream();
                    await image.SaveAsync(memStream, new JpegEncoder { Quality = 75 });
                    file.Bitmap = memStream.ToArray();
                }
                catch { }
            }
        });
    }

    private void SyncDriveToFolders(Drive drive)
    {
        foreach (var subFolder in drive.Folders.ToList())
        {
            var existingFileBrowserFolder = Folders.FirstOrDefault(x => x.Path == subFolder.Path);
            if (existingFileBrowserFolder == null)
            {
                existingFileBrowserFolder = new FileBrowserFolder
                {
                    Name = subFolder.Name,
                    Path = subFolder.Path,
                    Files = new ObservableCollection<FileBrowserFile>(subFolder.Files.Select(x => new FileBrowserFile
                    {
                        Name = x.Name,
                        Path = x.Path
                    }))
                };

                Folders.Add(existingFileBrowserFolder);
            }

            SyncFoldersToFolders(subFolder, existingFileBrowserFolder);
        }
    }

    private void SyncFoldersToFolders(Folder folder, FileBrowserFolder destination)
    {
        foreach (var subFolder in folder.Folders.ToList())
        {
            var existingFileBrowserFolder = destination.Folders.FirstOrDefault(x => x.Path == subFolder.Path);
            if (existingFileBrowserFolder == null)
            {
                var files = subFolder.Files.ToList().Select(x => new FileBrowserFile
                {
                    Name = x.Name,
                    Path = x.Path
                });

                existingFileBrowserFolder = new FileBrowserFolder
                {
                    Name = subFolder.Name,
                    Path = subFolder.Path,
                    Files = new ObservableCollection<FileBrowserFile>(files)
                };

                destination.Folders.Add(existingFileBrowserFolder);
            }

            SyncFoldersToFolders(subFolder, existingFileBrowserFolder);
        }
    }

    public void Dispose()
    {
        _fileScanPollingTimer.Stop();
        _directoryScanPollingTimer.Stop();

        _fileCache.Clear();
    }
}

public partial class FileBrowserFolder : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _path;

    [ObservableProperty]
    private ObservableCollection<FileBrowserFolder> _folders = new();

    [ObservableProperty]
    public partial ObservableCollection<FileBrowserFile> Files { get; set; }
}

public partial class FileBrowserFile : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial string Path { get; set; }

    [ObservableProperty]
    public partial byte[] Bitmap { get; set; }
}