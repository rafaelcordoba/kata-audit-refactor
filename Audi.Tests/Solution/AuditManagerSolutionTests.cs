using System;
using Audit.Solution;
using FluentAssertions;
using Xunit;

namespace Audi.Tests.Solution;

public class AuditManagerSolutionTests
{
    [Fact]
    public void A_new_file_is_created_when_the_current_file_overflows()
    {
        var sut = new AuditManagerSolution(3);
        var existingFiles = new FileContent[]
        {
            new("audit_2.txt",
                new[]
                {
                    "Peter;2019-04-06 16:30:00",
                    "Jane;2019-04-06 16:40:00",
                    "Jack;2019-04-06 17:00:00"
                }),
            new("audit_1.txt", Array.Empty<string>()),
        };
        
        var fileUpdate = sut.AddRecord(existingFiles, "Alice", DateTime.Parse("2019-04-06T18:00:00"));
        
        fileUpdate.Should().Be(new FileUpdate("audit_3.txt", "Alice;2019-04-06 18:00:00"));
    }
    
    [Fact]
    public void A_new_file_is_created_when_no_prior_audits_exist()
    {
        var sut = new AuditManagerSolution(3);
        var existingFiles = Array.Empty<FileContent>();
        
        var fileUpdate = sut.AddRecord(existingFiles, "Alice", DateTime.Parse("2019-04-06T18:00:00"));
        
        fileUpdate.Should().Be(new FileUpdate("audit_1.txt", "Alice;2019-04-06 18:00:00"));
    }
    
    [Fact]
    public void An_update_to_last_file_happens_when_max_entries_was_not_reached()
    {
        var sut = new AuditManagerSolution(3);
        var existingFiles = new FileContent[]
        {
            new("audit_1.txt",
                new[]
                {
                    "Peter;2019-04-06 16:30:00",
                    "Jane;2019-04-06 16:40:00"
                })
        };
        
        var fileUpdate = sut.AddRecord(existingFiles, "Alice", DateTime.Parse("2019-04-06T18:00:00"));
        
        fileUpdate.Should().Be(new FileUpdate("audit_1.txt", "Peter;2019-04-06 16:30:00\n" +
                                                             "Jane;2019-04-06 16:40:00\n" +
                                                             "Alice;2019-04-06 18:00:00"));
    }
}