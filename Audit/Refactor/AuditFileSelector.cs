using Audit.Original;

namespace Audit.Refactor;

public class AuditFileSelector : IAuditFileSelector
{
    private readonly IFileSystem _fileSystem;

    public AuditFileSelector(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public string GetPathToWrite(int maxEntriesPerFile, string directoryName)
    {
        const string prefix = "audit_"; 
        var filePaths = _fileSystem.GetFiles(directoryName);
        (int index, string path)[] sorted = SortByIndex(filePaths);

        if (sorted.Length == 0)
        {
            return Path.Combine(directoryName, $"{prefix}1.txt");
        }
            
        var (lastFileIndex, lastFilePath) = sorted.Last();
            
        List<string> lines = _fileSystem.ReadAllLines(lastFilePath).ToList();

        if (lines.Count < maxEntriesPerFile)
        {
            return lastFilePath;
        }
            
        var newIndex = lastFileIndex + 1;
        var newName = $"{prefix}{newIndex}.txt";
        return Path.Combine(directoryName, newName);
    }
    
    private static (int index, string path)[] SortByIndex(string[] filePaths)
        => filePaths
            .OrderBy(x => x)
            .Select((path, index) => (index + 1, path))
            .ToArray();
}