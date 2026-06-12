using System.Threading;

namespace PDFQFZ.Library
{
    internal sealed class PreviewOverlayRequestGate
    {
        private int currentRequestId;

        public int BeginNext()
        {
            return Interlocked.Increment(ref currentRequestId);
        }

        public bool IsCurrent(int requestId)
        {
            return Volatile.Read(ref currentRequestId) == requestId;
        }
    }
}
