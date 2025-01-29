namespace VerySimpleFileManager.Models;

public class Drive
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Label { get; set; }
    public bool IsIndexed { get; set; }

    public List<Folder> Folders { get; set; } = new List<Folder>();
    public List<File> Files { get; set; } = new List<File>();

    public string CountFiles()
    {
        var photoCount = CountPhotoFiles();
        var videoCount = CountVideoFiles();
        var textFinish = IsIndexed ? "!" : "...";
        return $"{(photoCount == 0 ? "Geen" : $"{photoCount:N0}")} foto's en {(videoCount == 0 ? "geen" : $"{videoCount:N0}")} video's gevonden{textFinish}";
    }

    public int CountPhotoFiles()
    {
        string[] extensions = ["jpg", "jpeg", "png", "gif", "bmp"];
        return Where(x => extensions.Contains(x.Name.Split('.').Last())).Count;
    }

    public int CountVideoFiles()
    {
        string[] extensions = ["mp4", "avi", "mkv", "mov", "wmv"];
        return Where(x => extensions.Contains(x.Name.Split('.').Last())).Count;
    }

    private List<File> Where(Func<File, bool> predicate)
    {
        var results = new List<File>();

        results.AddRange(Files.ToArray().Where(predicate));

        foreach (var folder in Folders.ToArray())
        {
            results.AddRange(Where(folder, predicate));
        }

        return results;
    }

    private List<File> Where(Folder folder, Func<File, bool> predicate)
    {
        var results = new List<File>();

        results.AddRange(folder.Files.ToArray().Where(predicate));

        foreach (var subFolder in folder.Folders.ToArray())
        {
            results.AddRange(Where(subFolder, predicate));
        }

        return results;
    }
}