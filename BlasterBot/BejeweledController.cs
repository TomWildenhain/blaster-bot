using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BlasterBot
{
    static class BejeweledController
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public static bool positionBejeweled()
        {
            Process p = findBejeweled();
            if (p != null)
            {
                IntPtr windowHandle = p.MainWindowHandle;
                MoveWindow(windowHandle, 647, 2, 1254, 713, true);
                return true;
            }
            else
            {
                return false;
            }
        }
        private static Process findBejeweled()
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if (p.ProcessName.IndexOf("popcapgame1") > -1)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
