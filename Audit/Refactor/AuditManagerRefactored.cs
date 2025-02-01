using Audit.Original;

namespace Audit.Refactor
{
    public class AuditManagerRefactored
    {
        private readonly int _maxEntriesPerFile;
        private readonly string _directoryName;
        private readonly IFileSystem _fileSystem;
        private readonly IAuditFileSelector _auditFileSelector;
        private readonly IVisitorRecordFormatter _visitorRecordFormatter;

        public AuditManagerRefactored(
            int maxEntriesPerFile, 
            string directoryName,
            IFileSystem fileSystem, 
            IAuditFileSelector auditFileSelector, 
            IVisitorRecordFormatter visitorRecordFormatter)
        {
            _maxEntriesPerFile = maxEntriesPerFile;
            _directoryName = directoryName;
            _fileSystem = fileSystem;
            _auditFileSelector = auditFileSelector;
            _visitorRecordFormatter = visitorRecordFormatter;
        }
    
        public void AddRecord(string visitorName, DateTime timeOfVisit)
        {
            var pathToWrite = _auditFileSelector.GetPathToWrite(_maxEntriesPerFile, _directoryName);
            List<string> lines = _fileSystem.ReadAllLines(pathToWrite).ToList();
            var textToWrite = _visitorRecordFormatter.GetTextToWrite(visitorName, timeOfVisit, lines);
            _fileSystem.WriteAllText(pathToWrite, textToWrite);
        }
    }
}
