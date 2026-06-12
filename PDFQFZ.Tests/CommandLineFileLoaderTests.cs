using PDFQFZ.Library;
using Xunit;

namespace PDFQFZ.Tests;

public sealed class CommandLineFileLoaderTests
{
    [Fact]
    public void CollectExistingPdfFiles_FiltersNonPdfAndMissingFiles()
    {
        string root = Path.Combine(Path.GetTempPath(), "pdfqfz-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);

        try
        {
            string firstPdf = Path.Combine(root, "a.pdf");
            string secondPdf = Path.Combine(root, "b.PDF");
            string textFile = Path.Combine(root, "c.txt");
            string missingPdf = Path.Combine(root, "missing.pdf");
            File.WriteAllText(firstPdf, "x");
            File.WriteAllText(secondPdf, "x");
            File.WriteAllText(textFile, "x");

            var files = CommandLineFileLoader.CollectExistingPdfFiles(new[]
            {
                firstPdf,
                textFile,
                missingPdf,
                secondPdf
            });

            Assert.Equal(new[] { firstPdf, secondPdf }, files);
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }
}
