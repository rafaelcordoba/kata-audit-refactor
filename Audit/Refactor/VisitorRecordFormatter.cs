namespace Audit.Refactor;

public class VisitorRecordFormatter : IVisitorRecordFormatter
{
    private readonly string _timeOfVisitFormat;

    public VisitorRecordFormatter(string timeOfVisitFormat)
    {
        _timeOfVisitFormat = timeOfVisitFormat;
    }
    
    public string GetTextToWrite(string visitorName, DateTime timeOfVisit, List<string> existingLines)
    {
        var newRecord = visitorName + ';' + timeOfVisit.ToString(_timeOfVisitFormat);
        if (existingLines.Count <= 0)
        {
            return newRecord;
        }

        existingLines.Add(newRecord);
        return string.Join(Environment.NewLine, existingLines);
    }
}