using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Encountify.Services
{
    class ImageCreator
    {
        public static byte[] GetDefaultImage()
        {
            string defaultImageLocation = "Encountify.default_image.png";
            byte[] buffer;
            Assembly assembly = typeof(ImageCreator).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(defaultImageLocation))
            {
                long length = stream.Length;
                buffer = new byte[length];
                stream.Read(buffer, 0, (int)length);
            }

            return buffer;
        }

        public static ImageSource GetDefaultImageStream()
        {
            byte[] imageBlob = GetDefaultImage();
            try
            {
                if (imageBlob is null)
                    return null;

                var imageByteArray = imageBlob;

                return ImageSource.FromStream(() => new MemoryStream(imageByteArray));
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
