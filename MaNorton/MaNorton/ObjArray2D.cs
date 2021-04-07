using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class ObjArray2D
    {
        public bool Enabled = false;
        Global.Str_String Label;

        MyFunc.Geminus<int> Koords;
        MyFunc.Geminus<int> ContentKoords;
        string CurrentContent;
        char[,] Field;

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

        public void SetLabel(MyFunc.Geminus<int> Koords001, string NewLabel001)
        {
            Label.SomeString = NewLabel001;
            Label.Koords = Koords001;
        }
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
        public void AddContent(MyFunc.Geminus<int> StartKoords, string Content001)
        {
            Clear();

            CurrentContent = Content001;
            ContentKoords = StartKoords;
            MyFunc.CopyStringToArray(StartKoords, CurrentContent, Field, true);
        }
        /*
        public void Show(ref List<Global.DPoint> NewEntaries001, int Min001, int Max001)
        {
            if (NewEntaries001.Any() && NewEntaries001 == null)
            {
                return;
            }

            MyFunc.FillArray(Field, ' ');

            MyFunc.Geminus<int> TempoPlacer = ContentKoords;

            for (int i = Min001; i < Max001; ++i)
            {
                MyFunc.CopyStringToArray(ref TempoPlacer, NewEntaries001[i].Name, Field, true);
                ++TempoPlacer.Secundus;
            }
            
            Global.FillBroders(Field);
            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);

            MyFunc.Copy(ref Koords, Field, Global.Field);
        }*/
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
        /*
        public void CopyToGlobal()
        {
            MyFunc.Copy(ref Koords, Field, Global.Field);
        }*/
    }
}
