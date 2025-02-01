namespace Audit.Refactor;

public interface IVisitorRecordFormatter
{
    string GetTextToWrite(string visitorName, DateTime timeOfVisit, List<string> existingLines);
}