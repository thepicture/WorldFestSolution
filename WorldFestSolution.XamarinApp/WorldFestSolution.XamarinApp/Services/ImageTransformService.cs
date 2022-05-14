using Android.Graphics;
using System.IO;

namespace WorldFestSolution.XamarinApp.Services
{
    public class ImageTransformService : IImageTransformService
    {
        /// <summary>
        /// Transforms the given image and changes its quality.
        /// <see="https://stackoverflow.com/a/66562122">Source of the implementation</see>
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
            BitmapFactory.Options options = new BitmapFactory.Options();
            Bitmap originalImage = BitmapFactory
                .DecodeByteArray(imageData,
                                 0,
                                 imageData.Length,
                                 options);
            options.InBitmap = originalImage;
            int originalHeight = originalImage.Height;
            int originalWidth = originalImage.Width;


            float newHeight;
            float newWidth;
            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage,
                                                            (int)newWidth,
                                                            (int)newHeight,
                                                            true);

            originalImage.Recycle();

            using (MemoryStream ms = new MemoryStream())
            {
                if (resizedImage.Compress(Bitmap.CompressFormat.Jpeg,
                                          quality,
                                          ms))
                {
                    resizedImage.Recycle();
                }

                return ms.ToArray();
            }
        }
    }
}
