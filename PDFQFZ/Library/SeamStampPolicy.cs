namespace PDFQFZ.Library
{
    internal static class SeamStampPolicy
    {
        public static bool ShouldSkipForSinglePage(int pageCount, int seamStampType)
        {
            return pageCount == 1 && seamStampType != 1;
        }
    }
}
