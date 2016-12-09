using System;
using System.IO;
using System.Runtime.InteropServices;
using Shared.Database;
using Shared.Util;
using Shared.Util.Configuration;

namespace Shared
{
    /// <summary>
    /// General methods needed by all servers.
    /// </summary>
    public abstract class ServerMain
    {
        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;

        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        };

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();


        [DllImport("user32")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int cx, int cy, int flags);

        [DllImport("user32")]
        static extern bool GetClientRect(IntPtr hWnd, ref RECT rect);

        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }

        public void GetWindowPosition(out int x, out int y, out int width, out int height)
        {
            RECT rect = new RECT();
            GetClientRect(Handle, ref rect);
            x = rect.top;
            y = rect.left;
            width = rect.right - rect.left;
            height = rect.bottom - rect.top;
        }

        public static IntPtr Handle
        {
            get
            {
                //Initialize();
                return GetConsoleWindow();
            }
        }

        /// <summary>
        /// Tries to find aura root folder and changes the working directory to it.
        /// Exits if not successful.
        /// </summary>
        protected static void NavigateToRoot()
        {
            // Go back max 2 folders, the bins should be in [aura]/bin/(Debug|Release)
            for (int i = 0; i < 3; ++i)
            {
                if (Directory.Exists("system"))
                    return;

                Directory.SetCurrentDirectory("..");
            }

            Log.Error("Unable to find root directory.");
            ConsoleUtil.Exit(1);
        }

        /// <summary>
        /// Tries to call conf's load method, exits on error.
        /// </summary>
        protected static void LoadConf(BaseConf conf)
        {
            Log.Info("Reading configuration...");

            try
            {
                conf.Load();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Unable to read configuration. ({0})", ex.Message);
                ConsoleUtil.Exit(1);
            }
        }

        /// <summary>
        /// Tries to initialize database with the information from conf,
        /// exits on error.
        /// </summary>
        protected static void InitDatabase(BaseDatabase db, BaseConf conf)
        {
            Log.Info("Initializing database...");

            try
            {
                db.Init(conf.Database.Host, conf.Database.Port, conf.Database.User, conf.Database.Pass, conf.Database.Db);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to open database connection. ({0})", ex.Message);
                ConsoleUtil.Exit(1);
            }
        }
    }
}
