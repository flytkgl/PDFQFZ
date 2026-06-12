using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class PreviewStampLayoutTests
{
    [Fact]
    public void CalculateOverlaySize_ScalesUsingConfiguredWidthAndAspectRatio()
    {
        var size = PreviewStampLayout.CalculateOverlaySize(
            configuredWidthMm: 40,
            imageDpi: 300,
            imagePixelWidth: 600,
            imagePixelHeight: 300,
            previewWidth: 500,
            pdfWidth: 1000,
            fallbackSquareSize: 72);

        Assert.Equal(56, size.Width);
        Assert.Equal(28, size.Height);
    }

    [Fact]
    public void CalculateOverlaySize_FallsBackWhenInputsAreInvalid()
    {
        var size = PreviewStampLayout.CalculateOverlaySize(
            configuredWidthMm: 0,
            imageDpi: 0,
            imagePixelWidth: 0,
            imagePixelHeight: 0,
            previewWidth: 0,
            pdfWidth: 0,
            fallbackSquareSize: 72);

        Assert.Equal(72, size.Width);
        Assert.Equal(72, size.Height);
    }
}
