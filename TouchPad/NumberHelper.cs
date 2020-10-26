using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KyleOlson.TouchPad
{
    public static class NumberHelper
    {
        public static bool InRange(this int check, int begin, int length)
        {
            return (check >= begin && check < begin + length);


        }
            
    }
}
