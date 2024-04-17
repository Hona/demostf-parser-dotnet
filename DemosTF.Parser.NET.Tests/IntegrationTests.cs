using System.Reflection;
using System.Runtime.InteropServices;
using FluentAssertions;

namespace DemosTF.Parser.NET.Unit.Tests;

public class IntegrationTests
{
    [Fact]
    public void DllLoaded_AnalyzeDemo_Successfully()
    {
        var methodInfo = typeof(TfDemoParser).GetMethod("analyze_demo", BindingFlags.NonPublic | BindingFlags.Static);

        methodInfo.Should().NotBeNull();
        
        Marshal.Prelink(methodInfo!);
    }
    
    [Fact]
    public void DllLoaded_FreeString_Successfully()
    {
        var methodInfo = typeof(TfDemoParser).GetMethod("free_string", BindingFlags.NonPublic | BindingFlags.Static);

        methodInfo.Should().NotBeNull();
        
        Marshal.Prelink(methodInfo!);
    }
    
    [Fact]
    public void AnalyzeDemo_WithStvFilePath_ReturnsValidData()
    {
        // Arrange
        var filePath = Path.Join(Environment.CurrentDirectory, "auto-20140701-233440-jump_propel_fixed_v3.dem");
        
        // Act
        var response = TfDemoParser.AnalyzeDemo(filePath);
        
        // Assert
        response.Should().NotBeNull();
        response!.Chat.Should().NotBeNullOrEmpty();
        response.Chat.Any(x => !string.IsNullOrWhiteSpace(x.Text)).Should().BeTrue("the demo contains chat messages");
        response.Chat.Should().HaveCount(406, "the demo has 406 chat messages");

        response.Users.Should().NotBeNullOrEmpty();
        response.Deaths.Should().NotBeNullOrEmpty();
    }
}