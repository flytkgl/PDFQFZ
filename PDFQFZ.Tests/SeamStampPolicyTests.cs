using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class SeamStampPolicyTests
{
    [Fact]
    public void ShouldSkipForSinglePage_ReturnsTrueWhenSeamStampIsEnabled()
    {
        Assert.True(SeamStampPolicy.ShouldSkipForSinglePage(1, 0));
        Assert.True(SeamStampPolicy.ShouldSkipForSinglePage(1, 2));
        Assert.True(SeamStampPolicy.ShouldSkipForSinglePage(1, 3));
        Assert.True(SeamStampPolicy.ShouldSkipForSinglePage(1, 4));
    }

    [Fact]
    public void ShouldSkipForSinglePage_ReturnsFalseWhenSeamStampIsDisabledOrMultiplePages()
    {
        Assert.False(SeamStampPolicy.ShouldSkipForSinglePage(1, 1));
        Assert.False(SeamStampPolicy.ShouldSkipForSinglePage(2, 0));
    }
}
