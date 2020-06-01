using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TouchPad
{
    public static class StringHelper
    {
        public static bool LegalRegex(this string s)
        {
            try
            {

                Regex x = new Regex(s);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
