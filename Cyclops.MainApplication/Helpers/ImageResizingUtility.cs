using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Cyclops.MainApplication
{
    public static class ImageResizingUtility
    {

        /// <summary>
        /// Change size of an image
        /// </summary>
        public static Image ResizeImage(this Image imgToResize, int width, int height)
        {
            var b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.Default;
            g.DrawImage(imgToResize, 0, 0, width, height);
            g.Dispose();

            return b;
        }

        private static Image ResizeImageInProportion(this Image image, int width, int height)
        {
            int actualWidth = image.Width, actualHeight = image.Height, newWidth = 1, newHeight = 1;

            if (width > 0)
            {
                newWidth = width;
                newHeight = (int) (actualHeight/(actualWidth/(float) width));
            }
            else
            {
                newHeight = height;
                newWidth = (int) (actualWidth/(actualHeight/(float) height));
            }
            return image.ResizeImage(newWidth, newHeight);
        }

        public static Image ResizeImageInProportionByWidth(this Image image, int width)
        {
            return ResizeImageInProportion(image, width, -1);
        }

        public static Image ResizeImageInProportionByHeight(this Image image, int heigth)
        {
            return ResizeImageInProportion(image, -1, heigth);
        }

        public static Image ResizeImageInProportionIfLarge(this Image image, int maxSize)
        {
            if ((image.Width > image.Height) && image.Width > maxSize)
                return ResizeImageInProportionByWidth(image, maxSize);
            if ((image.Height > image.Width) && image.Height > maxSize)
                return ResizeImageInProportionByHeight(image, maxSize);
            return image;
        }
    }
}
