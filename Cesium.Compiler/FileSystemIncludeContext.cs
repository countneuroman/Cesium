using Cesium.Preprocessor;

namespace Cesium.Compiler;

public class FileSystemIncludeContext : IIncludeContext
{
    private readonly string _stdLibDirectory;
    private readonly string _currentDirectory;
    private readonly List<string> _guardedIncludedFiles = new();

    public FileSystemIncludeContext(string stdLibDirectory, string currentDirectory)
    {
        _stdLibDirectory = stdLibDirectory;
        _currentDirectory = currentDirectory;
    }

    public string LookUpAngleBracedIncludeFile(string filePath)
    {
        var path = Path.Combine(_stdLibDirectory, filePath);
        return path;
    }

    public string LookUpQuotedIncludeFile(string filePath)
    {
        var path = Path.Combine(_currentDirectory, filePath);
        if (File.Exists(path))
            return Path.GetFullPath(path);

        path = Path.Combine(_stdLibDirectory, filePath);
        return Path.GetFullPath(path);
    }

    public TextReader OpenFileStream(string filePath) => new StreamReader(filePath);

    public bool ShouldIncludeFile(string filePath)
    {
        return !_guardedIncludedFiles.Contains(filePath);
    }

    public void RegisterGuardedFileInclude(string filePath)
    {
        _guardedIncludedFiles.Add(filePath);
    }
}
