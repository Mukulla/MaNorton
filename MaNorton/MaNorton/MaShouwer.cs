using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class MaShouwer
    {
        public bool Enabled = false;       

        Scrollar Scroller001 = new Scrollar();

        ObjArray2D MainField = new ObjArray2D();
        ObjArray2D HelpField = new ObjArray2D();
        
        Global.Str_Array2D VisibleField;
        Global.Str_Array1D ImageBorder;

        MyFunc.Geminus<int> Pages = MyFunc.Set(1, 1);

        List<string> Data = new List<string>();

        public void Init(MyFunc.Geminus<int> Koords001, int i001, int j001)
        {
            MainField.Init(Koords001, i001, j001, MyFunc.Set(0, 0));
            MainField.Enabled = true;
            VisibleField.Koords = MyFunc.Set(2, 1);
            VisibleField.Array = new char[i001 - 6, j001-2];

            //Label.SomeString = FileName001;
            //Label.Koords = MyFunc.Set(( j001 - FileName001.Length ) / 2, 0);

            ImageBorder.Array = new char[i001 + j001];
            ImageBorder.Koords = MyFunc.Set(0, 0);

            List<string> Helper = new List<string>();
            Helper.Add("Up and Down Arrows, PageUp and PageDown, Home and End - Navigate");
            Helper.Add("Escape - Close reader");

            //MainField.AddContent(MyFunc.Set(3, i001 - 3), ref Helper);

            HelpField.Init(MyFunc.Set(1, i001 - 5), 4, j001 - 2, MyFunc.Set(1, 1));
            HelpField.AddContent(MyFunc.Set(1,1), ref Helper);
            HelpField.Enabled = true;
        }

        public void SetLabel(string FileName001)
        {
            string NewName = ' ' + FileName001 + ' ';
            MainField.SetLabel(MyFunc.Set(MyFunc.AlignString(NewName.Length, VisibleField.Array.GetLength(1), 1), 0), NewName);
            //Label.SomeString = FileName001;
            //Label.Koords = MyFunc.Set((VisibleField.Array.GetLength(1) - FileName001.Length) / 2, 0);
        }
        public void SetData(ref List<string> SomeData001)
        {
            if (SomeData001 == null)
            {
                return;
            }

            Data = SomeData001;
            
            Scroller001.Init(0, Data.Count, VisibleField.Array.GetLength(0), VisibleField.Array.GetLength(0) * 2 / 3);

            Pages.Secundus = Data.Count / VisibleField.Array.GetLength(0);
            if(Pages.Secundus == 0)
            {
                Pages.Secundus = 1;
            }
            Pages.Primis = Scroller001.GetMin() / VisibleField.Array.GetLength(0);
        }
        public bool HandleKeys()
        {
            if(!Enabled)
            {
                return false;
            }

            if (MaKeys.Get(ConsoleKey.Escape))
            {
                Enabled = false;
                if(Data != null)
                {
                    Data.Clear();
                }
                return false;
            }

            //Смещение маркера вверх или вниз
            if (MaKeys.Get(ConsoleKey.DownArrow))
            {
                Scroller001.OffSet(1);
                return true;
            }
            if (MaKeys.Get(ConsoleKey.UpArrow))
            {
                Scroller001.OffSet(-1);
                return true; ;
            }
            
            //Постраничное смещение
            if (MaKeys.Get(ConsoleKey.PageDown))
            {
                Scroller001.OffSet(2);
                return true; ;
            }
            if (MaKeys.Get(ConsoleKey.PageUp))
            {
                Scroller001.OffSet(-2);
                return true; ;
            }

            //Перемещение в начало или конец            
            if (MaKeys.Get(ConsoleKey.End))
            {
                Scroller001.OffSet(3);
                return true; ;
            }
            if (MaKeys.Get(ConsoleKey.Home))
            {
                Scroller001.OffSet(-3);
            }
            return true;
        }
        public void Show()
        {
            if (!Enabled)
            {
                return;
            }

            if(Data == null)
            {
                return;
            }
            MyFunc.FillArray(VisibleField.Array, ' ');
            //Global.FillBroders(Field);
            //MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);

            Pages.Primis = Scroller001.GetMin() / VisibleField.Array.GetLength(0) + 1;
            string TempoString = $" Current Page {Pages.Primis} of {Pages.Secundus} ";
            HelpField.SetLabel(MyFunc.Set(MyFunc.AlignString(TempoString.Length, VisibleField.Array.GetLength(1), 1), 0), TempoString);

            MyFunc.Geminus<int> TempoKoords = MyFunc.Set(0, 0);
            for (int i = Scroller001.GetMin(); i < Scroller001.GetMax(); ++i)
            {
                
                MyFunc.CopyStringToArray(ref TempoKoords, Data[i], VisibleField.Array, true);
                ++TempoKoords.Secundus;
            }

            //Global.FillBroders(VisibleField.Array);
            //MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, VisibleField.Array, true);

            MyFunc.Copy(ref VisibleField.Koords, VisibleField.Array, MainField.GetField());
            MainField.Show();

            HelpField.Show();
            //MyFunc.Copy(ref VisibleField.Koords, VisibleField.Array, Global.Field);
        }
    }
}
