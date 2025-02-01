using System.Collections.Generic;
using Audit.Original;
using Audit.Refactor;
using FluentAssertions;
using Moq;
using Xunit;

namespace Audi.Tests.Refactor;

public class AuditFileSelectorTests
{
    private const string DirectoryName = "audits";
    private const string Prefix = "audit_";
    
    private readonly Mock<IFileSystem> _fileSystem;
    private readonly AuditFileSelector _sut;

    public AuditFileSelectorTests()
    {
        _fileSystem = new Mock<IFileSystem>();
        _sut = new AuditFileSelector(_fileSystem.Object);
    }

    [Fact]
    public void Without_Prior_Entries_Return_Audit_1_File()
    {
        _fileSystem
            .Setup(x => x.GetFiles(DirectoryName))
            .Returns(System.Array.Empty<string>());

        var pathToWrite = _sut.GetPathToWrite(3, DirectoryName);
        
        pathToWrite.Should().Be(GetFilePath(1));
    }

    [Fact]
    public void With_Less_Than_Max_Entries_On_Last_File_Return_Last_File()
    {
        _fileSystem
            .Setup(x => x.GetFiles(DirectoryName))
            .Returns(new[]
            {
                GetFilePath(1),
                GetFilePath(2),
            });
        
        _fileSystem
            .Setup(x => x.ReadAllLines(GetFilePath(2)))
            .Returns(new List<string>
            {
                "1",
                "2"
            });

        var pathToWrite = _sut.GetPathToWrite(3, DirectoryName);
        
        pathToWrite.Should().Be(GetFilePath(2));
    }
    
    [Fact]
    public void With_Exact_Max_Entries_On_Last_File_Return_New_File()
    {
        _fileSystem
            .Setup(x => x.GetFiles(DirectoryName))
            .Returns(new[]
            {
                GetFilePath(1),
                GetFilePath(2),
            });
        
        _fileSystem
            .Setup(x => x.ReadAllLines(GetFilePath(2)))
            .Returns(new List<string>
            {
                "1",
                "2",
                "3"
            });

        var pathToWrite = _sut.GetPathToWrite(3, DirectoryName);
        
        pathToWrite.Should().Be(GetFilePath(3));
    }
    
    [Fact]
    public void With_Unsorted_Files_And_Less_Than_Max_Entries_On_Last_File_Return_Last_File()
    {
        _fileSystem
            .Setup(x => x.GetFiles(DirectoryName))
            .Returns(new[]
            {
                GetFilePath(2),
                GetFilePath(1)
            });
        
        _fileSystem
            .Setup(x => x.ReadAllLines(GetFilePath(2)))
            .Returns(new List<string>
            {
                "1",
                "2"
            });

        var pathToWrite = _sut.GetPathToWrite(3, DirectoryName);
        
        pathToWrite.Should().Be(GetFilePath(2));
    }

    private static string GetFilePath(int fileNumber) => $"{DirectoryName}/{Prefix}{fileNumber}.txt";
}