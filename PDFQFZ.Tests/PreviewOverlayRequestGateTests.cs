using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class PreviewOverlayRequestGateTests
{
    [Fact]
    public void BeginNext_MakesOnlyLatestRequestCurrent()
    {
        var gate = new PreviewOverlayRequestGate();

        int first = gate.BeginNext();
        int second = gate.BeginNext();

        Assert.False(gate.IsCurrent(first));
        Assert.True(gate.IsCurrent(second));
    }
}
