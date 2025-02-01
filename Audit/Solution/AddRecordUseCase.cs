namespace Audit.Solution;

public class AddRecordUseCase
{
    private readonly string _directoryName;
    private readonly AuditManagerSolution _auditManager;
    private readonly Persister _persister;

    public AddRecordUseCase(string directoryName, int maxEntriesPerFile)
    {
        _directoryName = directoryName;
        _auditManager = new AuditManagerSolution(maxEntriesPerFile);
        _persister = new Persister();
    }
    
    public void Handle(string visitorName, DateTime timeOfVisit)
    {
        FileContent[] files = _persister.ReadDirectory(_directoryName);
        FileUpdate update = _auditManager.AddRecord(files, visitorName, timeOfVisit);

        _persister.ApplyUpdate(_directoryName, update);
    }
}