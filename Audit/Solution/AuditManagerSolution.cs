namespace Audit.Solution;

public class AuditManagerSolution
{
    private readonly int _maxEntriesPerFile;

    private const string FirstFileName = "audit_1.txt";

    public AuditManagerSolution(int maxEntriesPerFile)
        => _maxEntriesPerFile = maxEntriesPerFile;

    public FileUpdate AddRecord(
        FileContent[] files,
        string visitorName,
        DateTime timeOfVisit)
    {
        var sorted = SortByIndex(files);
        var newRecord = NewRecord(visitorName, timeOfVisit);

        return sorted switch
        {
            {Length: 0} => CreateFirstFile(newRecord),
            _ => CreateNewFileOrUpdate(sorted.Last(), newRecord)
        };
    }

    private static FileUpdate CreateFirstFile(string newRecord)
        => new(FirstFileName, newRecord);

    private FileUpdate CreateNewFileOrUpdate((int, FileContent) currentFile, string newRecord)
    {
        var (fileIndex, fileContent) = currentFile;
        return fileContent.Lines.Length < _maxEntriesPerFile
            ? AppendToExistingFile(fileContent, newRecord)
            : CreateANewFile(fileIndex, newRecord);
    }

    private static FileUpdate AppendToExistingFile(FileContent currentFile, string newRecord)
    {
        List<string> lines = currentFile.Lines.ToList();
        lines.Add(newRecord);
        string newContent = string.Join(Environment.NewLine, lines);

        return new FileUpdate(currentFile.FileName, newContent);
    }

    private static FileUpdate CreateANewFile(int currentFileIndex, string newRecord)
    {
        int newIndex = currentFileIndex + 1;
        string newName = $"audit_{newIndex}.txt";

        return new FileUpdate(newName, newRecord);
    }

    private static string NewRecord(string visitorName, DateTime timeOfVisit)
        => visitorName + ';' + timeOfVisit.ToString("yyyy-MM-dd HH:mm:ss");

    private static (int index, FileContent)[] SortByIndex(FileContent[] files)
        => files
            .OrderBy(x => x.FileName)
            .AsEnumerable()
            .Select((content, index) => (index + 1, content))
            .ToArray();
}