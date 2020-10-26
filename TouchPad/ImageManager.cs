using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Media;

namespace KyleOlson.TouchPad
{
    public class ImageManager
    {
        private static object staticLock = new object();

        private Dictionary<string, BitmapImage> images 
            = new Dictionary<string, BitmapImage>();
        private Dictionary<Size, Dictionary<string, byte[]>> streamDeckImages
            = new Dictionary<Size, Dictionary<string, byte[]>>();

        private static ImageManager instance;
        public static ImageManager Instance
        {
            get
            {
                lock (staticLock)
                {

                    if (instance == null)
                    {
                        instance = new ImageManager();
                    }
                }

                return instance;
                
            }
        }

        private ImageManager()
        {

        }

        public  BitmapImage Get(string path, bool reload = false)
        {
            BitmapImage image = null;
            if (reload || !images.TryGetValue(path, out image))
            {
                try
                {
                    if (File.Exists(path))
                    {
                        image = new BitmapImage(new Uri(path));
                        images[path] = image;
                    }
                }
                catch
                {

                }
            }
            return image;
        }
        public byte[] GetStreamDeckImage(string path, int width, int height)
        {
            return GetStreamDeckImage(path,  new Size(width,  height));
        }
        public byte [] GetStreamDeckImage(string path, Size sz)
        {
            if (!streamDeckImages.TryGetValue(sz, out Dictionary<string, byte[]> dictionary))
            {
                dictionary = new Dictionary<string, byte[]>();
                streamDeckImages[sz] = dictionary;
            }

            if (!dictionary.TryGetValue(path, out byte [] sdBytes))
            {
                sdBytes = CreateStreamDeckImage(path, sz);
            }
            return sdBytes;
            
        }



        private byte [] CreateStreamDeckImage(string path, Size sz)
        {
            BitmapImage img = Get(path);
            if (img == null)
            {
                return null;
            }

            double width = (double)sz.Width;
            double height = (double)sz.Height;


            double xScale = width / img.Width;
            double yScale = height / img.Height;


            Transform tf = new ScaleTransform(xScale, yScale);
            BitmapSource usemap = new TransformedBitmap(img, tf);

            if (usemap.Format != PixelFormats.Bgr24)
            {
                usemap = new FormatConvertedBitmap(usemap, PixelFormats.Bgr24, null, 0.0);
            }
            int size = sz.Width * sz.Height * 3;
            byte[] data = new byte[size];
            usemap.CopyPixels(new System.Windows.Int32Rect(0, 0, sz.Width, sz.Height), data, 3 * sz.Width, 0);

            return data;
        }
    }
}
