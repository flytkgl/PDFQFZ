using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class PreviewValueSanitizerTests
{
    [Fact]
    public void KeepDigitsOnly_RemovesNonDigits()
    {
        Assert.Equal("123", PreviewValueSanitizer.KeepDigitsOnly("1a-2b3"));
    }

    [Fact]
    public void KeepSignedInteger_KeepsLeadingMinusAndDigits()
    {
        Assert.Equal("-123", PreviewValueSanitizer.KeepSignedInteger("--1a2-3"));
    }

    [Fact]
    public void ClampInt_ReturnsFallbackForInvalidValue()
    {
        Assert.Equal(40, PreviewValueSanitizer.ClampInt("", 40, 0, 100));
    }

    [Theory]
    [InlineData("-5", 0)]
    [InlineData("150", 100)]
    [InlineData("25", 25)]
    public void ClampInt_ClampsIntoRange(string text, int expected)
    {
        Assert.Equal(expected, PreviewValueSanitizer.ClampInt(text, 40, 0, 100));
    }
}
