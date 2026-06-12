using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class StampRenderPolicyTests
{
    [Fact]
    public void ShouldRenderVisibleStamp_ReturnsFalseWhenNoStampSelectedEvenIfSignatureIsEnabled()
    {
        Assert.False(StampRenderPolicy.ShouldRenderVisibleStamp(0));
    }

    [Fact]
    public void ShouldRenderVisibleStamp_ReturnsTrueWhenStampPagesAreEnabled()
    {
        Assert.True(StampRenderPolicy.ShouldRenderVisibleStamp(1));
        Assert.True(StampRenderPolicy.ShouldRenderVisibleStamp(2));
        Assert.True(StampRenderPolicy.ShouldRenderVisibleStamp(3));
        Assert.True(StampRenderPolicy.ShouldRenderVisibleStamp(4));
    }
}
