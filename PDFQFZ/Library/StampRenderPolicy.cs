namespace PDFQFZ.Library
{
    internal static class StampRenderPolicy
    {
        public static bool ShouldRenderVisibleStamp(int stampType)
        {
            return stampType != 0;
        }
    }
}
