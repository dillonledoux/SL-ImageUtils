using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace SL_ImageUtils
{
    public class ImageUtils
    {
        /// <summary>
        /// Performs high quality resize a supplied image to the specified dimensions
        /// </summary>
        /// <param name="image">The image to be resized</param>
        /// <param name="width">The number of horizontal pixels</param>
        /// <param name="height">The number of vertical pixels</param>
        /// <returns>Resized image via a Bitmap object</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRectangle = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// Compresses an image and saves the image to file
        /// </summary>
        /// <param name="bmpImage">The image to be compressed and saved</param>
        /// <param name="savePath">The save destination of the image</param>
        /// <param name="qualityLevel">Quality level of the output between 0 and 100 with zero being maximum compression and 100 being lossless</param>
        public static void CompressAndSave(Bitmap bmpImage, string savePath, long qualityLevel)
        {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, qualityLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bmpImage.Save(savePath, jpgEncoder, myEncoderParameters);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
