using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KyleOlson.TouchPad
{
    static class ActiveWindowHelper
    {
        public static string Title
        {
            get
            {

                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                IntPtr handle = GetForegroundWindow();

                if (GetWindowText(handle, Buff, nChars) > 0)
                {
                    return Buff.ToString();
                }
                return null;
            }
        }

        public static string ClassName
        {
            get
            {

                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                IntPtr handle = GetForegroundWindow();

                if (GetClassName(handle, Buff, nChars) > 0)
                {
                    return Buff.ToString();
                }
                return null;

            }
        }

        public static string ImageName
        {
            get
            {
                const int nChars = 256;
                StringBuilder Buff = new StringBuilder(nChars);
                IntPtr handle = GetForegroundWindow();

                IntPtr phandle = GetProcessHandleFromHwnd(handle);

                if (GetProcessImageFileName(phandle, Buff, nChars) > 0)
                {
                    return Buff.ToString();
                }
                return null;

            }
        }



        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        [DllImport("Oleacc.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr GetProcessHandleFromHwnd(IntPtr hWnd);


        [DllImport("psapi.dll")]
        static extern uint GetProcessImageFileName(
            IntPtr hProcess,
            [Out] StringBuilder lpImageFileName,
            [In] [MarshalAs(UnmanagedType.U4)] int nSize
        );

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;
    }
}
