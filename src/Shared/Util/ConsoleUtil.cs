using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Shared.Util
{
    public class ConsoleUtil
    {
        private const string TitlePrefix = "DCNC: ";

        private static readonly string[] Logo =
        {
            @" /$$$$$$$   /$$$$$$  /$$   /$$  /$$$$$$ ",
            @"| $$__  $$ /$$__  $$| $$$ | $$ /$$__  $$",
            @"| $$  \ $$| $$  \__/| $$$$| $$| $$  \__/",
            @"| $$  | $$| $$      | $$ $$ $$| $$      ",
            @"| $$  | $$| $$      | $$  $$$$| $$      ",
            @"| $$  | $$| $$    $$| $$\  $$$| $$    $$",
            @"| $$$$$$$/|  $$$$$$/| $$ \  $$|  $$$$$$/",
            @"|_______/  \______/ |__/  \__/ \______/ "
        };

        private static readonly string[] Credits =
        {
            @"Copyright (c) 2017 GigaToni",
            @"For problems & support: https://github.com/exmex/DCNC/issues",
            @"Also visit our discord channel: https://discord.gg/GnW6xxf",
            @"Special Thanks to amPerl"
        };

        /// <summary>
        ///     Gets a value indicating whether the current process is running
        ///     in user interactive mode.
        /// </summary>
        /// <remarks>
        ///     Custom property wrapping Environment.UserInteractive, with special
        ///     behavior for Mono, which currently doesn't support that property.
        /// </remarks>
        /// <returns></returns>
        public static bool UserInteractive
        {
            get
            {
#if __MonoCS__ // "In" is CStreamReader when running normally
// (TextReader on Windows) and SynchronizedReader
// when running in background.
                return (Console.In is System.IO.StreamReader);
#else
                return Environment.UserInteractive;
#endif
            }
        }

        /// <summary>
        ///     Writes logo and credits to Console.
        /// </summary>
        /// <param name="consoleTitle">The title of the console window</param>
        /// <param name="color">Color of the logo.</param>
        public static void WriteHeader(string consoleTitle, ConsoleColor color)
        {
            Console.Title = TitlePrefix + consoleTitle;

            Console.WriteLine();

            Console.ForegroundColor = color;
            WriteLinesCentered(Logo);

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;
            WriteLinesCentered(Credits);

            Console.ResetColor();
            WriteSeperator();
        }

        /// <summary>
        ///     Writes seperator in form of 80 underscores to Console.
        /// </summary>
        public static void WriteSeperator()
        {
            Console.WriteLine("".PadLeft(Console.WindowWidth, '_'));
        }

        /// <summary>
        ///     Writes lines to Console, centering them as a group.
        /// </summary>
        /// <param name="lines"></param>
        private static void WriteLinesCentered(string[] lines)
        {
            var longestLine = lines.Max(a => a.Length);
            foreach (var line in lines)
                WriteLineCentered(line, longestLine);
        }

        /// <summary>
        ///     Writes line to Console, centering it either with the string's
        ///     length or the given length as reference.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="referenceLength">Set to greater than 0, to use it as reference length, to align a text group.</param>
        private static void WriteLineCentered(string line, int referenceLength = -1)
        {
            if (referenceLength < 0)
                referenceLength = line.Length;

            Console.WriteLine(line.PadLeft(line.Length + Console.WindowWidth / 2 - referenceLength / 2));
        }

        /// <summary>
        ///     Prefixes window title with an asterisk.
        /// </summary>
        public static void LoadingTitle()
        {
            if (!Console.Title.StartsWith("* "))
                Console.Title = "* " + Console.Title;
        }

        /// <summary>
        ///     Removes asterisks and spaces that were prepended to the window title.
        /// </summary>
        public static void RunningTitle()
        {
            Console.Title = Console.Title.TrimStart('*', ' ');
        }

        /// <summary>
        ///     Waits for the return key, and closes the application afterwards.
        /// </summary>
        /// <param name="exitCode"></param>
        /// <param name="wait"></param>
        public static void Exit(int exitCode, bool wait = true)
        {
            if (wait && UserInteractive)
            {
                Log.Info("Press Enter to exit.");
                Console.ReadLine();
            }
            Log.Info("Exiting...");
            Environment.Exit(exitCode);
        }

        /// <summary>
        ///     Returns whether the application runs with admin rights or not.
        /// </summary>
        public static bool CheckAdmin()
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        
        /// <summary>
        ///     Returns arguments parsed from line.
        /// </summary>
        /// <remarks>
        ///     Matches words and multiple words in quotation.
        /// </remarks>
        /// <example>
        ///     arg0 arg1 arg2 -- 3 args: "arg0", "arg1", and "arg2"
        ///     arg0 arg1 "arg2 arg3" -- 3 args: "arg0", "arg1", and "arg2 arg3"
        /// </example>
        public static IList<string> ParseLine(string line)
        {
            var args = new List<string>();
            var quote = false;
            for (int i = 0, n = 0; i <= line.Length; ++i)
            {
                if ((i == line.Length || line[i] == ' ') && !quote)
                {
                    if (i - n > 0)
                        args.Add(line.Substring(n, i - n).Trim(' ', '"'));

                    n = i + 1;
                    continue;
                }

                if (line[i] == '"')
                    quote = !quote;
            }

            return args;
        }
    }
}