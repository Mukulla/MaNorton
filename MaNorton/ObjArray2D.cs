using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    //Некое поле с рамкой, заголовком, загружаемым содержимым
    class ObjArray2D
    {
        //Включено или выключено
        public bool Enabled = false;
        //Заголовок
        Global.Str_String Label;

        //Глобальные координаты
        MyFunc.Geminus<int> Koords;
        //Стартовые координаты содержимого
        MyFunc.Geminus<int> ContentKoords;
        //Содержимое
        string CurrentContent;
        //Само поле
        char[,] Field;

        //Создание
        public void Init(MyFunc.Geminus<int> Koords001, int i001, int j001, MyFunc.Geminus<int> ContentKoords001)
        {
            if (i001 < 1)
            {
                i001 = 1;
            }
            if (j001 < 1)
            {
                j001 = 1;
            }
            Field = new char[i001, j001];
            MyFunc.FillArray(Field, ' ');

            Koords = Koords001;
            ContentKoords = ContentKoords001;
        }
        //Добавление заголовока
        public void SetLabel(MyFunc.Geminus<int> Koords001, string NewLabel001)
        {
            Label.SomeString = NewLabel001;
            Label.Koords = Koords001;
        }
        //Добавление сдержимого в виде вертикального списка
        public void AddContent(MyFunc.Geminus<int> StartKoords, ref List<string> Content)
        {
            if (Content.Any() && Content == null)
            {
                return;
            }

            ContentKoords = StartKoords;
            foreach (var Item in Content)
            {
                MyFunc.CopyStringToArray(StartKoords, Item, Field, true);
                ++StartKoords.Secundus;
            }
        }
        //Добавление сдержимого в виде массива строк
        public void AddContent(MyFunc.Geminus<int> StartKoords, string[] Content)
        {
            if (Content == null)
            {
                return;
            }

            ContentKoords = StartKoords;
            foreach (var Item in Content)
            {
                MyFunc.CopyStringToArray(StartKoords, Item, Field, true);
                ++StartKoords.Secundus;
            }
        }

        //Добавление сдержимого в виде строки с очисткой
        public void AddContent(MyFunc.Geminus<int> StartKoords, string Content001)
        {
            Clear();

            CurrentContent = Content001;
            ContentKoords = StartKoords;
            MyFunc.CopyStringToArray(StartKoords, CurrentContent, Field, true);
        }
        //Отображение
        public void Show()
        {
            if(!Enabled)
            {
                return;
            }
            //MyFunc.FillArray(Field, ' ');

            Global.FillBroders(Field);
            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);

            MyFunc.Copy(ref Koords, Field, Global.Field);
        }
        public void Show(string SomeString001)
        {
            if (Enabled)
            {
                return;
            }
            MyFunc.FillArray(Field, ' ');
            MyFunc.CopyStringToArray(ContentKoords, SomeString001, Field, true);
            
            Global.FillBroders(Field);
            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);

            MyFunc.Copy(ref Koords, Field, Global.Field);
        }
        
        public void Clear()
        {
            MyFunc.FillArray(Field, ' ');
            Global.FillBroders(Field);
            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);
        }

        public int GetWidth()
        {
            return Field.GetLength(1);
        }
        public int GetHeight()
        {
            return Field.GetLength(0);
        }
        public MyFunc.Geminus<int> GetKoords()
        {
            return Koords;
        }
        public char[,] GetField()
        {
            return Field;
        }
    }
}
