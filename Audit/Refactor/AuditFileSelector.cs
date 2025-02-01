using Audit.Original;

namespace Audit.Refactor;

public class AuditFileSelector : IAuditFileSelector
{
    private readonly IFileSystem _fileSystem;
    private readonly int _maxEntriesPerFile;
    private readonly string _directoryName;

    public AuditFileSelector(IFileSystem fileSystem, int maxEntriesPerFile, string directoryName)
    {
        _fileSystem = fileSystem;
        _maxEntriesPerFile = maxEntriesPerFile;
        _directoryName = directoryName;
    }

    public string GetPathToWrite()
    {
        const string prefix = "audit_"; 
        var filePaths = _fileSystem.GetFiles(_directoryName);
        (int index, string path)[] sorted = SortByIndex(filePaths);

        if (sorted.Length == 0)
        {
            return Path.Combine(_directoryName, $"{prefix}1.txt");
        }
            
        var (lastFileIndex, lastFilePath) = sorted.Last();
            
        List<string> lines = _fileSystem.ReadAllLines(lastFilePath).ToList();

        if (lines.Count < _maxEntriesPerFile)
        {
            return lastFilePath;
        }
            
        var newIndex = lastFileIndex + 1;
        var newName = $"{prefix}{newIndex}.txt";
        return Path.Combine(_directoryName, newName);
    }
    
    private static (int index, string path)[] SortByIndex(string[] filePaths)
        => filePaths
            .OrderBy(x => x)
            .Select((path, index) => (index + 1, path))
            .ToArray();
}