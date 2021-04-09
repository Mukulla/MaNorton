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
            //Сделать окно максимального размера
            Maximize();

            //Указание размеров глобального поля
            Global.Init(180, 54);
            //Создание командера
            FullCommander MaCommander = new FullCommander();

            while (!MaCommander.Ended)
            {
                //Отображение командера и его различных элементов
                MaCommander.Show();
                //Отображение глобального поля, куда скопированы все элементы в текущем кадре
                Global.Show();

                //Получение кода клавиши
                MaKeys.Set();
                //Обработка кодов
                MaCommander.HandleKeys();

                //Очистка главного поля
                Global.Clear();
                //Очистка экрана консоли
                Console.Clear();
            }
        }        
    }
}
