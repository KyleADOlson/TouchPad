using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInputLib;
using WindowsInputLib.Native;

namespace KyleOlson.TouchPad
{
    public static class KeyHelper
    {
        public static VirtualKeyCode [] GetAllKeys()
        {
            return (VirtualKeyCode [])Enum.GetValues(typeof(VirtualKeyCode));
        }

        public static string Name<T>(this T ob) where T : System.Enum
        {
            return Enum.GetName(typeof(T), ob);
        }

        public static string KeyText(this VirtualKeyCode key)
        {


            int val = (int)key;


            string basename = key.Name();
            

            return basename;
        }
    }
}
