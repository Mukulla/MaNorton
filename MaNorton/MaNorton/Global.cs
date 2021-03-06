using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class Global
    {
        static List<int> MarkIndexes = new List<int>();
        static MyFunc.Quadrupla<int> PaintBackGrHorizKoords;

        static List<int> TextIndexes = new List<int>();
        static List<int> TextSortedIndexes = new List<int>();
        static MyFunc.Geminus<int> TextVertBorders;
        static int TextVertKoord;
        static MyFunc.Quadrupla<int> TextHorizontalKoords;

        static public ObjArray2D Attention = new ObjArray2D();

        static Str_String Label;
        static public MyFunc.Geminus<int> Sizes;
        static public char[,] Field;

        static string LogeFile = "Log.txt";

        public enum Types
        {
            LevelUp,
            Directory,
            File
        }
        public struct DPoint
        {
            public string Name;
            public Types Type;
        }

        public struct Str_Properties
        {
            public string Name;
            public Types Type;
            public string Attribute;
            public long Size;
        }

        public struct Str_String
        {
            public string SomeString;
            public MyFunc.Geminus<int> Koords;
        }
        public struct Str_Array1D
        {
            public char[] Array;
            public MyFunc.Geminus<int> Koords;
        }
        public struct Str_Array2D
        {
            public char[,] Array;
            public MyFunc.Geminus<int> Koords;
        }

        
        static public void Init(int Primis001, int Secundus001)
        {
            Sizes.Primis = Primis001;
            Sizes.Secundus = Secundus001;

            Field = new char[Sizes.Secundus, Sizes.Primis];
            MyFunc.FillArray(Field, ' ');
            Global.FillBroders(Field);

            Label.SomeString = " MaNorto ";
            Label.Koords = MyFunc.Set( ( Global.Sizes.Primis - Label.SomeString.Length ) / 2, 0);
            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);

            Attention.Init(MyFunc.Set(2, Global.Sizes.Secundus - 7), 3, Global.Sizes.Primis - 4, MyFunc.Set(2, 1));
            Attention.SetLabel(MyFunc.Set(2, 0), " System Messages ");
            Attention.Enabled = true;
        }
        static public void AddToLogFile(string TryingPath001, string NameError001, string Error001)
        {
            File.AppendAllText(Global.LogeFile, $"{DateTime.Now.ToString("HH:mm:ss")} {DateTime.Now.ToString("dd MMMM yyyy")} {NameError001}  {TryingPath001}  {Error001} {Environment.NewLine}");
        }
        static public void AddMarkPainterKoords(int VertIndex001, MyFunc.Quadrupla<int> HorizKoords001)
        {
            MarkIndexes.Add(VertIndex001);
            PaintBackGrHorizKoords = HorizKoords001;
        }

        static public void AddTextPainterKoords(int VertIndex001, int VertKoordField, MyFunc.Quadrupla<int> TextHorizontalKoords001)
        {
            //Проверяем на совпадение и если оно есть, то удаляем
            for (int i = 0; i < TextIndexes.Count; ++i)
            {
                if (TextIndexes[i] == VertIndex001)
                {
                    TextIndexes.RemoveAt(i);
                    Sort();
                    return;
                }
            }

            TextIndexes.Add(VertIndex001);
            TextVertKoord = VertKoordField;
            TextHorizontalKoords = TextHorizontalKoords001;

            Sort();
        }
        static public void AddTextVertBorders(MyFunc.Geminus<int> VertBorders001)
        {
            TextVertBorders = VertBorders001;
        }
        static public void DeleteTextPainter(int SomeIndex001)
        {
            if(!TextIndexes.Any() || TextIndexes == null)
            {
                return;
            }
            for (int i = 0; i < TextIndexes.Count; ++i)
            {
                if (TextIndexes[i] == SomeIndex001)
                {
                    TextIndexes.RemoveAt(i);
                }    
            }
        }
        static public void ResetTextPainter()
        {
            TextIndexes.Clear();
            TextSortedIndexes.Clear();
        }

        static public void Clear()
        {
            MyFunc.FillArray(Field, ' ');
            Global.FillBroders(Field);
            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field, true);

        }

        static public void Show()
        {
            //Attention.Show();

            int Index001 = 0;
            int Index002 = 0;
            //Вывод глобального поля
            for (int i = 0; i < Global.Sizes.Secundus; ++i)
            {
                //Закраска поля при условии совпадения с индексом и наличия элементов в списке
                if (Painter(ref i, ref Index001, ref Index002))
                {
                    continue;
                }
                Console.WriteLine(@MyFunc.GetString(i, Global.Field));
            }

            //Global.FillBroders(Field, '#');
            Attention.Clear();

            MarkIndexes.Clear();
        }
        static bool Painter(ref int CurrentIndex, ref int Index001, ref int Index002)
        {
            //Текущий видимый индекс
            int CurrentVisibleTextIndex;
            //Сравниваем итоговый индекс маркера с итератором
            //Закрашиваем область
            if (CheckList(ref MarkIndexes, Index001) && CheckList(ref TextSortedIndexes, Index002) )
            {
                //Вычисляем видимый индекс закрашиваемого слова
                CurrentVisibleTextIndex = TextSortedIndexes[Index002] - TextVertBorders.Primis + TextVertKoord;
                if (MarkIndexes[Index001] == CurrentVisibleTextIndex && CurrentVisibleTextIndex == CurrentIndex)
                {
                    ColorizeAll(CurrentVisibleTextIndex, PaintBackGrHorizKoords);
                    ++Index001;
                    ++Index002;
                    return true;
                }                
            }
            if (CheckList(ref MarkIndexes, Index001))
            //if (MarkIndexes.Any() && Index001 < MarkIndexes.Count && !WasPainted)
            {
                if (MarkIndexes[Index001] == CurrentIndex)
                {
                    ColorizePartLine(MarkIndexes[Index001], PaintBackGrHorizKoords);
                    ++Index001;
                    return true;
                }
            }
            if (CheckList(ref TextSortedIndexes, Index002))
            //if (TextIndexes.Any() && Index002 < TextIndexes.Count && !WasPainted)
            {
                if (MyFunc.IntraAream(TextSortedIndexes[Index002], TextVertBorders.Primis, TextVertBorders.Secundus - 1) == 0)
                {
                    //Вычисляем видимый индекс закрашиваемого слова
                    CurrentVisibleTextIndex = TextSortedIndexes[Index002] - TextVertBorders.Primis + TextVertKoord;
                    if (CurrentVisibleTextIndex == CurrentIndex)
                    {
                        ColorizeWord(CurrentVisibleTextIndex, TextHorizontalKoords);
                        ++Index002;
                        return true;
                    }
                }
            }
            return false;
        }
        static void ColorizePartLine(int SomeIndex001, MyFunc.Quadrupla<int> HorizKoods001)
        {
            //Colorize LEFT PART
            Console.Write(MyFunc.GetString(SomeIndex001, HorizKoods001.Primis, HorizKoods001.Secundus, Global.Field));

            //Colorize CENTER PART
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(MyFunc.GetString(SomeIndex001, HorizKoods001.Secundus, HorizKoods001.Tertium, Global.Field));
            Console.ResetColor();

            //Colorize RIGHT PART
            string Str001 = MyFunc.GetString(SomeIndex001, HorizKoods001.Tertium, HorizKoods001.Quartus, Global.Field);
            Console.Write(Str001);

            //Move To The Next Line
            Console.WriteLine();
        }

        static void ColorizeWord(int SomeIndex001, MyFunc.Quadrupla<int> HorizKoods001)
        {
            //Colorize LEFT PART
            Console.Write(MyFunc.GetString(SomeIndex001, HorizKoods001.Primis, HorizKoods001.Secundus, Global.Field));

            //Colorize CENTER PART
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(MyFunc.GetString(SomeIndex001, HorizKoods001.Secundus, HorizKoods001.Tertium, Global.Field));
            Console.ResetColor();

            //Colorize RIGHT PART
            string Str001 = MyFunc.GetString(SomeIndex001, HorizKoods001.Tertium, HorizKoods001.Quartus, Global.Field);
            Console.Write(Str001);

            //Move To The Next Line
            Console.WriteLine();
        }
        static void ColorizeAll(int SomeIndex001, MyFunc.Quadrupla<int> HorizKoods001)
        {
            //Colorize LEFT PART
            Console.Write(MyFunc.GetString(SomeIndex001, HorizKoods001.Primis, HorizKoods001.Secundus, Global.Field));

            //Colorize CENTER PART
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(MyFunc.GetString(SomeIndex001, HorizKoods001.Secundus, HorizKoods001.Tertium, Global.Field));
            Console.ResetColor();

            //Colorize RIGHT PART
            string Str001 = MyFunc.GetString(SomeIndex001, HorizKoods001.Tertium, HorizKoods001.Quartus, Global.Field);
            Console.Write(Str001);

            //Move To The Next Line
            Console.WriteLine();
        }
        static bool CheckList(ref List<int> SomeList001, int CurrentIndex001)
        {
            if (SomeList001.Any())
            {
                if (CurrentIndex001 < SomeList001.Count)
                {
                    return true;
                }
            }
            return false;
        }

        static void Sort()
        {
            TextSortedIndexes.Clear();
            var SortedID = from Value in TextIndexes
                           orderby Value
                           select Value;
            
            foreach (var Item in SortedID)
            {
                TextSortedIndexes.Add(Item);
            }
        }

        static public void FillBroders(char[,] SomeField)
        {
            if(SomeField == null)
            {
                return;
            }

            char[] HoriArray = new char[SomeField.GetLength(1)];
            char[] VertArray = new char[SomeField.GetLength(0)];
            MyFunc.FillArray(HoriArray, '═');
            MyFunc.FillArray(VertArray, '║');

            MyFunc.Copy(MyFunc.Set(0, 0), HoriArray, SomeField, true);
            MyFunc.Copy(MyFunc.Set(0, SomeField.GetLength(0) - 1), HoriArray, SomeField, true);

            MyFunc.Copy(MyFunc.Set(0, 0), VertArray, SomeField, false);
            MyFunc.Copy(MyFunc.Set(SomeField.GetLength(1) - 1, 0), VertArray, SomeField, false);

            SomeField[0, 0] = '╔';
            SomeField[0, SomeField.GetLength(1) - 1] = '╗';
            SomeField[SomeField.GetLength(0) - 1, SomeField.GetLength(1) - 1] = '╝';
            SomeField[SomeField.GetLength(0) - 1, 0] = '╚';
        }        
    }
}
