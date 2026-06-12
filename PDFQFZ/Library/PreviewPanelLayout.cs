using System;

namespace PDFQFZ.Library
{
    internal static class PreviewPanelLayout
    {
        public static int GetCollapsedClientWidth(int previewLeft, int margin)
        {
            return Math.Max(1, previewLeft - margin);
        }

        public static bool ShouldShowPreviewPanel(int stampType, int seamStampType)
        {
            return stampType != 0 || seamStampType == 4;
        }
    }
}
