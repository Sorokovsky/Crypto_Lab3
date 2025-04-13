namespace Lab3.Core;

public class FilesService
{
    private readonly string _folder;

    public FilesService(string folder = "files")
    {
        _folder = folder;
        if (Directory.Exists(_folder) is false) Directory.CreateDirectory(_folder);
    }

    public void Write(string fileName, string content, bool prepare = true)
    {
        var path = GetPath(fileName, prepare);
        File.WriteAllText(path, content);
    }

    public string Read(string fileName, bool prepare = true)
    {
        var path = GetPath(fileName, prepare);
        return File.ReadAllText(path);
    }

    public bool Exists(string fileName, bool prepare = true)
    {
        return File.Exists(GetPath(fileName, prepare));
    }

    public string GetPath(string fileName, bool prepare = true)
    {
        return prepare ? Path.Combine(_folder, $"{fileName}.txt") : fileName;
    }
}