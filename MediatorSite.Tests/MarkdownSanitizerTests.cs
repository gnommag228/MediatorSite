
using Xunit;
using MediatorSite.Utilities;

namespace MediatorSite.Tests;

public class MarkdownSanitizerTests
{
    [Fact]
    public void Sanitize_EscapesAsterisk()
    {
        string input = "test*";
        string result = MarkdownSanitizer.Sanitize(input);
        Assert.Equal("test\\*", result);
    }
    [Fact]
    public void Sanitize_EscapesUnderscore()
    {
        string input = "test_";
        string result = MarkdownSanitizer.Sanitize(input);
        Assert.Equal("test\\_", result);
    }
    [Fact]
    public void Sanitize_EscapesBacktick()
    {
        string input = "test`";
        string result = MarkdownSanitizer.Sanitize(input);
        Assert.Equal("test\\`", result);
    }






}