using System.Drawing;
using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class WhiteTransparencyHelperTests
{
    [Fact]
    public void ClampTolerance_ClampsToSupportedRange()
    {
        Assert.Equal(0, WhiteTransparencyHelper.ClampTolerance(-1));
        Assert.Equal(20, WhiteTransparencyHelper.ClampTolerance(20));
        Assert.Equal(50, WhiteTransparencyHelper.ClampTolerance(99));
    }

    [Fact]
    public void Apply_MakesNearWhitePixelsTransparent()
    {
        using var source = new Bitmap(2, 1);
        source.SetPixel(0, 0, Color.FromArgb(255, 250, 250, 250));
        source.SetPixel(1, 0, Color.FromArgb(255, 200, 200, 200));

        using Bitmap result = WhiteTransparencyHelper.Apply(source, 20);

        Assert.Equal(0, result.GetPixel(0, 0).A);
        Assert.Equal(255, result.GetPixel(1, 0).A);
    }

    [Fact]
    public void Apply_PreservesPixelScaleForHighDpiImages()
    {
        using var source = new Bitmap(10, 1);
        source.SetResolution(300, 300);
        for (int x = 0; x < source.Width; x++)
        {
            source.SetPixel(x, 0, Color.Black);
        }

        using Bitmap result = WhiteTransparencyHelper.Apply(source, 0);

        Assert.Equal(300f, result.HorizontalResolution);
        Assert.Equal(300f, result.VerticalResolution);
        Assert.Equal(255, result.GetPixel(9, 0).A);
    }
}
