using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxima_Distribuidores_VS
{
    class Imagenes
    {
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            Size dimensiones=RectangleSize(image.Size,new Size(width,height));
            var destRect = new System.Drawing.Rectangle(0, 0, dimensiones.Width, dimensiones.Height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private static Size RectangleSize(Size imageSize, Size rectangleSize)
        {
            double widthScale = 0, heighScale = 0;
            widthScale = (double)rectangleSize.Width / (double)imageSize.Width;
            heighScale = (double)rectangleSize.Height / (double)imageSize.Height;
            double scale = Math.Min(widthScale, heighScale);
            Size result = new Size((int)(imageSize.Width * scale), (int)(imageSize.Height * scale));
            return result;
        }
    }
}
