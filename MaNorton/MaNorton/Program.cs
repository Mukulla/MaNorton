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
    }
}
