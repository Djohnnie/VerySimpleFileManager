namespace VerySimpleFileManager.Models;

public class Drive
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Label { get; set; }
    public bool IsIndexed { get; set; }

    public List<Folder> Folders { get; set; } = new List<Folder>();
    public List<File> Files { get; set; } = new List<File>();

    public List<File> Where(Func<File, bool> predicate)
    {
        var results = new List<File>();

        results.AddRange(Files.ToArray().Where(predicate));

        foreach (var folder in Folders.ToArray())
        {
            results.AddRange(Where(folder, predicate));
        }

        return results;
    }

    public List<File> Where(Folder folder, Func<File, bool> predicate)
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