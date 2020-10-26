using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KyleOlson.TouchPad
{
    public static class ButtonStyler
    {
        public static void Style(Button b, ButtonDescription desc, bool reload = false)
        {
            b.FontSize = desc.FontSize;
            b.Foreground = new SolidColorBrush(desc.FontColor.ToColor());
            b.FontFamily = new FontFamily(desc.FontFamily);
            b.FontWeight = desc.FontWeight.FontWeight();
            TextBlock tb = new TextBlock();
            tb.Text = desc.Text;
            tb.TextAlignment = TextAlignment.Center;

            b.Content = tb;
            

            if (desc.Image != null && desc.Image.Length > 0)
            {
                if (File.Exists(desc.Image))
                {

                    BitmapImage img = ImageManager.Instance.Get(desc.Image, reload);
                    b.Background = new ImageBrush(img);
                }
            }

        }

    }
}
