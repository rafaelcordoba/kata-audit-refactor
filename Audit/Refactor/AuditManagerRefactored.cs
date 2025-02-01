using Audit.Original;

namespace Audit.Refactor
{
    public class AuditManagerRefactored
    {
        private readonly IFileSystem _fileSystem;
        private readonly IAuditFileSelector _auditFileSelector;
        private readonly IVisitorRecordFormatter _visitorRecordFormatter;

        public AuditManagerRefactored(
            IFileSystem fileSystem, 
            IAuditFileSelector auditFileSelector, 
            IVisitorRecordFormatter visitorRecordFormatter)
        {
            _fileSystem = fileSystem;
            _auditFileSelector = auditFileSelector;
            _visitorRecordFormatter = visitorRecordFormatter;
        }
    
        public void AddRecord(string visitorName, DateTime timeOfVisit)
        {
            var pathToWrite = _auditFileSelector.GetPathToWrite();
            List<string> lines = _fileSystem.ReadAllLines(pathToWrite).ToList();
            var textToWrite = _visitorRecordFormatter.GetTextToWrite(visitorName, timeOfVisit, lines);
            _fileSystem.WriteAllText(pathToWrite, textToWrite);
        }
    }
}
