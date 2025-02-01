namespace Audit.Refactor;

public interface IAuditFileSelector
{
    string GetPathToWrite(int maxEntriesPerFile, string directoryName);
}