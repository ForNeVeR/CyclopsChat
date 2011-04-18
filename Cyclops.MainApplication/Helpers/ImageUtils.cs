using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Cyclops.MainApplication.Helpers
{
    /// <summary>
    /// Set of extension methods for System.Drawing.Image
    /// </summary>
    public static class ImageUtils
    {
        /// <summary>
        /// Convert image to Base64 string
        /// </summary>
        public static string ImageToBase64(this Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        /// <summary>
        /// Convert image back from base64 string
        /// </summary>
        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0,
                                      imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        /// <summary>
        /// Cut part of image
        /// </summary>
        public static Image CropImage(this Image img, int width, int height, int x, int y)
        {
            var bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(new Rectangle(x, y, width, height), bmpImage.PixelFormat);
            return bmpCrop;
        }

        /// <summary>
        /// Change size of image
        /// </summary>
        public static Image ResizeImage(this Image imgToResize, int width, int height)
        {
            var b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, width, height);
            g.Dispose();

            return b;
        }

        /// <summary>
        /// Rotate image by angle
        /// </summary>
        public static Image RotateImage(this Image image, float angle)
        {
            var returnBitmap = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(returnBitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);
            g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
            g.DrawImage(image, new Point(0, 0));
            g.Dispose();
            return returnBitmap;
        }

        /// <summary>
        /// Convert System.Drawing.Image into wpf BitmapImage
        /// </summary>
        public static BitmapImage ToBitmapImage(this Image image)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
    }
}
