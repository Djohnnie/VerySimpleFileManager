namespace VerySimpleFileManager.Models;

public class Folder
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Path { get; set; }

    public List<Folder> Folders { get; set; } = new List<Folder>();
    public List<File> Files { get; set; } = new List<File>();
}