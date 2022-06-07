using System;
using System.Drawing;
using System.IO;

namespace WorldFestSolution.ImportApp
{
    public class ImageTransformService : IImageTransformService
    {
        /// <summary>
        /// Transforms the given image and changes its quality.
        /// <see="https://stackoverflow.com/a/18455151">Source of the implementation</see>
        /// </summary>
        /// <param name="imageData">The image input bytes.</param>
        /// <param name="width">A new width.</param>
        /// <param name="height">A new height.</param>
        /// <param name="quality">A quality of the resized image.</param>
        /// <returns>The new transformed image with the given quality.</returns>
        public byte[] Transform(byte[] imageData,
                                float width,
                                float height,
                                int quality)
        {
            using (var ms = new MemoryStream(imageData))
            {
                var image = Image.FromStream(ms);

                var ratioX = (double)width / image.Width;
                var ratioY = (double)height / image.Height;

                var ratio = Math.Min(ratioX, ratioY);

                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);

                var newImage = new Bitmap(newWidth, newHeight);

                Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

                Bitmap bmp = new Bitmap(newImage);

                ImageConverter converter = new ImageConverter();

                return (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            }
        }
    }
}
