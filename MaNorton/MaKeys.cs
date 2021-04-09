using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;


namespace MaNorton
{
    //Обработчик нажатия клавиш, хранит код и передаёт его
    //Так же считывает текст
    class MaKeys
    {
        //Код клавиши
        static ConsoleKey CurrentKey;

        static bool TextReader = false;
        static MyFunc.Geminus<int> TextKoords = MyFunc.Set(0, 0);
        static string ReadedText = string.Empty;

        static public void Set()
        {
            //Прочитать текст
            if (TextReader)
            {
                Console.SetCursorPosition(TextKoords.Primis, TextKoords.Secundus);
                ReadedText = Console.ReadLine();
                TextReader = false;
                return;
            }
            //Прочитать код клавиши
            CurrentKey = Console.ReadKey().Key;
        }
        //Запустить чтение текста
        static public void ReadText(MyFunc.Geminus<int> TextKoords001, bool Start001)
        {
            TextKoords = TextKoords001;
            TextReader = Start001;
        }
        static public bool Get(ConsoleKey SomeKey001)
        {
            if (CurrentKey == SomeKey001)
            {
                return true;
            }
            return false;
        }
        
        static public string GetText()
        {
            return ReadedText;
        }
    }
}
