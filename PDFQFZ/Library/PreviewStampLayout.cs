using System;
using System.Drawing;

namespace PDFQFZ.Library
{
    internal static class PreviewStampLayout
    {
        public static Size CalculateOverlaySize(
            float configuredWidthMm,
            float imageDpi,
            int imagePixelWidth,
            int imagePixelHeight,
            float previewWidth,
            float pdfWidth,
            int fallbackSquareSize)
        {
            if (configuredWidthMm <= 0 ||
                imagePixelWidth <= 0 ||
                imagePixelHeight <= 0 ||
                previewWidth <= 0 ||
                pdfWidth <= 0)
            {
                return new Size(fallbackSquareSize, fallbackSquareSize);
            }

            float stampWidthPoints = configuredWidthMm * 72f / 25.4f;
            float previewScale = previewWidth / pdfWidth;
            int width = Math.Max(1, (int)(stampWidthPoints * previewScale));
            int height = Math.Max(1, (int)(width * imagePixelHeight / (float)imagePixelWidth));
            return new Size(width, height);
        }
    }
}
