namespace Audit.Refactor;

public interface IAuditFileSelector
{
    string GetPathToWrite();
}