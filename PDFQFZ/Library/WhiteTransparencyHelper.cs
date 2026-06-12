using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace PDFQFZ.Library
{
    internal static class WhiteTransparencyHelper
    {
        public static Bitmap Apply(Bitmap src, int tolerance)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            Bitmap bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            bmp.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(src, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            try
            {
                int bytes = Math.Abs(data.Stride) * bmp.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(data.Scan0, buffer, 0, bytes);

                int threshold = 255 - ClampTolerance(tolerance);
                for (int i = 0; i < buffer.Length; i += 4)
                {
                    byte b = buffer[i];
                    byte g = buffer[i + 1];
                    byte r = buffer[i + 2];

                    if (r > threshold && g > threshold && b > threshold)
                    {
                        buffer[i + 3] = 0;
                    }
                }

                Marshal.Copy(buffer, 0, data.Scan0, bytes);
                return bmp;
            }
            finally
            {
                bmp.UnlockBits(data);
            }
        }

        public static int ClampTolerance(int tolerance)
        {
            if (tolerance < 0)
            {
                return 0;
            }

            if (tolerance > 50)
            {
                return 50;
            }

            return tolerance;
        }
    }
}
