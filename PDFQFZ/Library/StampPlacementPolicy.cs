namespace PDFQFZ.Library
{
    internal static class StampPlacementPolicy
    {
        public static bool ShouldRenderVisibleStampOnPage(int page, int excludedPage, int signaturePage, bool signatureEnabled)
        {
            if (excludedPage != 0 && page == excludedPage)
            {
                return false;
            }

            if (signatureEnabled && page == signaturePage)
            {
                return false;
            }

            return true;
        }
    }
}
