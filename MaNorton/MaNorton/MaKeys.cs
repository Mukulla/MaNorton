using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;


namespace MaNorton
{
    class MaKeys
    {
        //Код клавиши
        static ConsoleKey CurrentKey;

        static bool TextReader = false;
        static MyFunc.Geminus<int> TextKoords = MyFunc.Set(0, 0);
        static string ReadedText = "";

        static public void Set()
        {
            if (TextReader)
            {
                Console.SetCursorPosition(TextKoords.Primis, TextKoords.Secundus);
                ReadedText = Console.ReadLine();
                TextReader = false;
                return;
            }
            CurrentKey = Console.ReadKey().Key;
        }
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
