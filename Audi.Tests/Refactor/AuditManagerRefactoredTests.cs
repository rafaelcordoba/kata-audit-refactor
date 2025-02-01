using System;
using System.Collections.Generic;
using Audit.Original;
using Audit.Refactor;
using Moq;
using Xunit;

namespace Audi.Tests.Refactor;

public class AuditManagerRefactoredTests
{
    private const string AnyPath = "anyPath";
    private const string AnyVisitor = "anyVisitor";
    private readonly DateTime _timeOfVisit = new(1970, 1, 1);
    private readonly List<string> _existingLines = new() { "line1", "line2" };
    
    private readonly Mock<IFileSystem> _fileSystem = new();
    private readonly Mock<IAuditFileSelector> _fileSelector = new();
    private readonly Mock<IVisitorRecordFormatter> _visitorRecordFormatter = new();
    private readonly AuditManagerRefactored _sut;

    public AuditManagerRefactoredTests()
    {
        _sut = new AuditManagerRefactored(
            _fileSystem.Object,
            _fileSelector.Object,
            _visitorRecordFormatter.Object);
    }

    [Fact]
    public void AddRecord_Should_Write_New_Record_Accordingly()
    {
        // Arrange
        const string expectedTextToWrite = "expectedText";
        _fileSelector.Setup(x => x.GetPathToWrite())
            .Returns(AnyPath);
        _fileSystem.Setup(x => x.ReadAllLines(AnyPath))
            .Returns(_existingLines);
        _visitorRecordFormatter.Setup(x => x.GetTextToWrite(AnyVisitor, _timeOfVisit, _existingLines))
            .Returns(expectedTextToWrite);
        
        // Act
        _sut.AddRecord(AnyVisitor, _timeOfVisit);   
        
        // Assert
        _fileSelector.Verify(x => 
            x.GetPathToWrite(), Times.Once);
        _fileSystem.Verify(x => 
            x.ReadAllLines(AnyPath), Times.Once);
        _visitorRecordFormatter.Verify(x => 
            x.GetTextToWrite(AnyVisitor, _timeOfVisit, _existingLines), Times.Once);
        _fileSystem.Verify(x =>
            x.WriteAllText(AnyPath, expectedTextToWrite), Times.Once);
    }
}