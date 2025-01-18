using System.IO;
using VerySimpleFileManager.Models;

namespace VerySimpleFileManager.Helpers;

public class FileIndexerHelper
{
    public List<Drive> Drives { get; set; } = [];

    internal async Task ProcessDrive(Drive drive)
    {
        DriveInfo driveInfo = new DriveInfo(drive.Name);

        foreach (var file in driveInfo.RootDirectory.GetFiles())
        {
            await ProcessFile(file, drive);
        }

        foreach (var folder in driveInfo.RootDirectory.GetDirectories())
        {
            await ProcessFolder(folder, drive);
        }

        drive.IsIndexed = true;
    }

    private async Task ProcessFolder(DirectoryInfo directoryInfo, Drive drive)
    {
        try
        {
            await Task.Delay(1);

            var subFolder = new Folder
            {
                Name = directoryInfo.Name,
                Path = directoryInfo.FullName
            };

            drive.Folders.Add(subFolder);

            foreach (var file in directoryInfo.GetFiles())
            {
                await ProcessFile(file, subFolder);
            }

            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                await ProcessFolder(subDirectory, subFolder);
            }
        }
        catch (Exception ex)
        {
            // NOP
        }
    }

    private async Task ProcessFolder(DirectoryInfo directoryInfo, Folder folder)
    {
        try
        {
            await Task.Delay(1);

            var subFolder = new Folder
            {
                Name = directoryInfo.Name,
                Path = directoryInfo.FullName
            };

            folder.Folders.Add(subFolder);

            foreach (var file in directoryInfo.GetFiles())
            {
                await ProcessFile(file, subFolder);
            }

            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                await ProcessFolder(subDirectory, subFolder);
            }
        }
        catch (Exception ex)
        {
            // NOP
        }
    }

    private async Task ProcessFile(FileInfo fileInfo, Folder folder)
    {
        try
        {
            folder.Files.Add(new Models.File
            {
                Name = fileInfo.Name,
                Path = fileInfo.FullName
            });
        }
        catch (Exception ex)
        {
            // NOP
        }
    }

    private async Task ProcessFile(FileInfo fileInfo, Drive drive)
    {
        try
        {
            drive.Files.Add(new Models.File
            {
                Name = fileInfo.Name,
                Path = fileInfo.FullName
            });
        }
        catch (Exception ex)
        {
            // NOP
        }
    }
}