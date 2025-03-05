using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Cyclops.MainApplication;

internal static class ImageUtils
{

    public static byte[] ImageToByte(Image img)
    {
        var converter = new ImageConverter();
        return (byte[])converter.ConvertTo(img, typeof(byte[])); //sometimes it threws an exception (i did'nt solve it yet :) i.e. on avatar of dotnet@conference.jabber.ru/Finn
    }

    /// <summary>
    /// Convert System.Drawing.Image into wpf BitmapImage
    /// </summary>
    public static BitmapImage ToBitmapImage(this byte[] image)
    {
        var bi = new BitmapImage();
        bi.BeginInit();
        var ms = new MemoryStream();
        Image.FromStream(new MemoryStream(image)).Save(ms, ImageFormat.Bmp);
        ms.Seek(0, SeekOrigin.Begin);
        bi.StreamSource = ms;
        bi.EndInit();
        RenderOptions.SetBitmapScalingMode(bi, BitmapScalingMode.LowQuality);
        bi.Freeze();
        return bi;
    }
}
