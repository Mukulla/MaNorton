using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using MyLib;


namespace MaNorton
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        private static void Maximize()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }
        static void Main(string[] args)
        {
            Maximize();

            Global.Init(180, 54);
            FullCommander MaCommander = new FullCommander();
            MaCommander.Init();

            while (!MaCommander.Ended)
            {
                MaCommander.Show();
                Global.Show();

                MaKeys.Set();
                MaCommander.HandleKeys();

                Global.Clear();
                Console.Clear();
            }
        }
        /*
        static void TestColors()
        {
            string SomeString = "dnhkrth";
            Console.Write("   ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(SomeString);
            Console.Write("   ");
            Console.ResetColor();
            Console.WriteLine();



            string letters = "ogofdgreo";
            var o = letters.IndexOf('o');
            Console.Write(letters.Substring(0, o));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(letters[o]);
            Console.ResetColor();
            Console.WriteLine(letters.Substring(o + 1));

            // Get an array with the values of ConsoleColor enumeration members.
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            // Save the current background and foreground colors.
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;

            // Display all foreground colors except the one that matches the background.
            Console.WriteLine("All the foreground colors except {0}, the background color:",
                              currentBackground);
            foreach (var color in colors)
            {
                if (color == currentBackground) continue;

                Console.ForegroundColor = color;
                Console.WriteLine("   The foreground color is {0}.", color);
            }
            Console.WriteLine();
            // Restore the foreground color.
            Console.ForegroundColor = currentForeground;

            // Display each background color except the one that matches the current foreground color.
            Console.WriteLine("All the background colors except {0}, the foreground color:",
                              currentForeground);
            foreach (var color in colors)
            {
                if (color == currentForeground) continue;

                Console.BackgroundColor = color;
                Console.WriteLine("   The background color is {0}.", color);
            }

            // Restore the original console colors.
            Console.ResetColor();
            Console.WriteLine("\nOriginal colors restored...");
        }*/
    }
}
