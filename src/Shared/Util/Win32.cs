using System;
using System.Runtime.InteropServices;

namespace Shared.Util
{
    public static class Win32
    {
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_NOACTIVATE = 0x10;

        public static IntPtr Handle => GetConsoleWindow();

        [DllImport("kernel32")]
        private static extern IntPtr GetConsoleWindow();


        [DllImport("user32")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);

        [DllImport("user32")]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT rect);

        /// <summary>
        ///     Sets the console window location and size in pixels
        /// </summary>
        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        public static void GetWindowPosition(out int x, out int y, out int width, out int height)
        {
            var rect = new RECT();
            GetClientRect(Handle, ref rect);
            x = rect.top;
            y = rect.left;
            width = rect.right - rect.left;
            height = rect.bottom - rect.top;
        }

        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}