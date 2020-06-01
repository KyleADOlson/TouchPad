using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TouchPad
{
    public static class Utils
    {

        public static Color ToColor(this uint source)
        {

            byte[] bytes = BitConverter.GetBytes(source);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

    }
}
