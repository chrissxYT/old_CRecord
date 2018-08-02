using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CRecord
{
    public static class Util
    {
        /// <summary>
        /// A static RNG-Generator.
        /// </summary>
        public static Random Random { get; } = new Random();

        /// <summary>
        /// Take a screenshot as bitmap.
        /// </summary>
        /// <returns>The screen as a bitmap</returns>
        public static Bitmap Screenshot()
        {
            var bounds = Screen.PrimaryScreen.Bounds;

            var bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format24bppRgb);
            
            Graphics.FromImage(bmp).CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);

            return bmp;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
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

        /// <summary>
        /// Appends the second given string to the first given string with a comma and space as a seperator.
        /// </summary>
        /// <param name="s">The string to append to</param>
        /// <param name="stringToAppend">The string to append to the other string</param>
        /// <returns>The new full string</returns>
        public static string AppendWithComma(this string s, string stringToAppend)
        {
            if (s == null || s == "")
                s = stringToAppend;
            else
                s += ", " + stringToAppend;
            return s;
        }

        /// <summary>
        /// Appends one string to the other with a ", " between them if the bool is true
        /// </summary>
        /// <param name="s">The string to append the other one to</param>
        /// <param name="appendToString">The string to append to the other one</param>
        /// <param name="append">A bool to specify if the string should be appended</param>
        /// <returns></returns>
        public static string AppendWithCommaIf(this string s, string appendToString, bool append)
        {
            if (append)
                return s.AppendWithComma(appendToString);
            else
                return s;
        }

        /// <summary>
        /// Creates a new temp path and returns it.
        /// </summary>
        public static string Temp_Path
        {
            get
            {
                var s = "C:\\Users\\";
                while (Directory.Exists(s))
                    s = Path.GetTempPath()+"\\"+Random.Next()+"\\";
                Directory.CreateDirectory(s);
                return s;
            }
        }

        /// <summary>
        /// Converts the bytes into their int value.
        /// </summary>
        /// <param name="bytes">The 4 bytes that are the int</param>
        /// <returns>The converted int</returns>
        public static int ToInt32(this byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Converts the int into its bytes.
        /// </summary>
        /// <param name="i">The int to convert</param>
        /// <returns>The bytes of the int</returns>
        public static byte[] ToByteArray(this int i)
        {
            return BitConverter.GetBytes(i);
        }
    }
}
