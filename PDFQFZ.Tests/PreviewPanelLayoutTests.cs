using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class PreviewPanelLayoutTests
{
    [Fact]
    public void GetCollapsedClientWidth_HidesPreviewControlsStartingAtPreviewLeft()
    {
        int collapsedWidth = PreviewPanelLayout.GetCollapsedClientWidth(previewLeft: 629, margin: 8);

        Assert.Equal(621, collapsedWidth);
    }

    [Fact]
    public void ShouldShowPreviewPanel_ReturnsTrueOnlyWhenStampPreviewIsNeeded()
    {
        Assert.False(PreviewPanelLayout.ShouldShowPreviewPanel(0, 1));
        Assert.True(PreviewPanelLayout.ShouldShowPreviewPanel(1, 1));
        Assert.True(PreviewPanelLayout.ShouldShowPreviewPanel(0, 4));
    }
}
