using System.Diagnostics;

namespace VerySimpleFileManagerDriveDetector;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void pollingTimer_Tick(object sender, EventArgs e)
    {
        var processName = "VerySimpleFileManager";
        var running = Process.GetProcessesByName(processName);

        if (running.Length == 0)
        {
            var drives = DriveInfo.GetDrives().Where(drive => drive.IsReady);

            foreach (var drive in drives)
            {
                var pathToExe = $"{drive.RootDirectory}VerySimpleFileManager\\{processName}.exe";
                if (File.Exists(pathToExe))
                {
                    Process.Start(pathToExe, drive.Name);
                }
            }
        }
    }
}