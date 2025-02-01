using System;
using System.Collections.Generic;
using Audit.Refactor;
using FluentAssertions;
using Xunit;

namespace Audi.Tests.Refactor;

public class VisitorRecordFormatterTests
{
    private readonly VisitorRecordFormatter _sut = new("yyyy-MM-dd HH:mm:ss");

    [Fact]
    public void Without_Prior_Existing_Lines_Return_Only_New_Record_Line()
    {
        var textToWrite = _sut.GetTextToWrite(
                "Rafael", 
                new DateTime(2025, 10, 10, 10, 10, 10), 
                new List<string>());
        
        textToWrite.Should().Be("Rafael;2025-10-10 10:10:10");
    }
    
    [Fact]
    public void With_Prior_Existing_Lines_Return_Record_Separate_By_New_Line()
    {
        var textToWrite = _sut.GetTextToWrite(
            "Rafael", 
            new DateTime(2025, 10, 10, 10, 10, 10), 
            new List<string>
            {
                "Peter;2019-04-06 16:30:00",
                "Jane;2019-04-06 16:40:00"
            });
        
        textToWrite.Should().Be("Peter;2019-04-06 16:30:00\n" +
                               "Jane;2019-04-06 16:40:00\n" +
                               "Rafael;2025-10-10 10:10:10");
    }
}