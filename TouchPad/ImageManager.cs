using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TouchPad
{
    public class ImageManager
    {
        private static object staticLock = new object();

        private Dictionary<string, BitmapImage> images 
            = new Dictionary<string, BitmapImage>();

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
    }
}
