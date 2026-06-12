using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class StampPlacementPolicyTests
{
    [Fact]
    public void ShouldRenderVisibleStampOnPage_SkipsSignaturePageWhenDigitalSignatureIsEnabled()
    {
        Assert.False(StampPlacementPolicy.ShouldRenderVisibleStampOnPage(
            page: 5,
            excludedPage: 0,
            signaturePage: 5,
            signatureEnabled: true));
    }

    [Fact]
    public void ShouldRenderVisibleStampOnPage_RendersNormalPagesWhenDigitalSignatureIsEnabled()
    {
        Assert.True(StampPlacementPolicy.ShouldRenderVisibleStampOnPage(
            page: 4,
            excludedPage: 0,
            signaturePage: 5,
            signatureEnabled: true));
    }

    [Fact]
    public void ShouldRenderVisibleStampOnPage_RespectsExcludedPageWithoutSignature()
    {
        Assert.False(StampPlacementPolicy.ShouldRenderVisibleStampOnPage(
            page: 1,
            excludedPage: 1,
            signaturePage: 5,
            signatureEnabled: false));
    }
}
