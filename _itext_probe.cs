using System;
using System.Drawing;
using System.Drawing.Imaging;

class Program
{
    static void Main()
    {
        using (var bmp = new Bitmap(600, 300))
        {
            bmp.SetResolution(300, 300);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Red);
            }

            var img = iTextSharp.text.Image.GetInstance(bmp, ImageFormat.Png);
            Console.WriteLine("bmp dpi={0}, px={1}x{2}", bmp.HorizontalResolution, bmp.Width, bmp.Height);
            Console.WriteLine("itext width={0}, height={1}, plainWidth={2}, plainHeight={3}, scaledWidth={4}, scaledHeight={5}", img.Width, img.Height, img.PlainWidth, img.PlainHeight, img.ScaledWidth, img.ScaledHeight);
        }
    }
}
