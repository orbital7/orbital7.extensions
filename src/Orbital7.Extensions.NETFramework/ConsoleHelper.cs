using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Orbital7.Extensions.NETFramework
{
    public static class ConsoleHelper
    {
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint nPosition, uint wFlags);

        internal const UInt32 SC_CLOSE = 0xF060;
        internal const UInt32 MF_GRAYED = 0x00000001;
        internal const uint MF_BYCOMMAND = 0x00000000;

        public static void DisableCloseButtonFromRunningConsole()
        {
            IntPtr hMenu = Process.GetCurrentProcess().MainWindowHandle;
            IntPtr hSystemMenu = GetSystemMenu(hMenu, false);
            EnableMenuItem(hSystemMenu, SC_CLOSE, MF_GRAYED);
            RemoveMenu(hSystemMenu, SC_CLOSE, MF_BYCOMMAND); 
        }

        public static bool GetBooleanArg(string value)
        {
            if (value.ToUpper().Equals("TRUE"))
                return true;
            else
                return false;
        }
    }
}
