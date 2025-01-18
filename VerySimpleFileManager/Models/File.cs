namespace VerySimpleFileManager.Models;

public class File
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Path { get; set; }
}