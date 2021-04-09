using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    //Три раздельных списка с с именем, типом, аттрибутом и размером
    class Content
    {
        //Основное поле с рамкой
        Global.Str_Array2D MainField = new Global.Str_Array2D();
        //Отельные поля с разным содержимым
        private Global.Str_Array2D[]  PartesOfContent = new Global.Str_Array2D[4];
        //Названия полей
        Global.Str_String[] Labels = new Global.Str_String[4];

        public void Init(MyFunc.Geminus<int> Koords001, MyFunc.Geminus<int> Sizes001)
        {
            //Создание общего поля
            MainField.Array = new char[Sizes001.Secundus, Sizes001.Primis];
            MainField.Koords = Koords001;
            Global.FillBroders(MainField.Array);
            
            //Размеры частей
            MyFunc.Geminus<int> PartSizes = MyFunc.Set(Sizes001.Primis / 6, Sizes001.Secundus - 2);

            PartesOfContent[0].Array = new char[PartSizes.Secundus, PartSizes.Primis * 3];
            PartesOfContent[0].Koords = MyFunc.Set(1, 1);

            PartesOfContent[1].Array = new char[PartSizes.Secundus, PartSizes.Primis];
            PartesOfContent[1].Koords = MyFunc.Set(Sizes001.Primis - 3 * PartSizes.Primis - 1, 1);

            PartesOfContent[2].Array = new char[PartSizes.Secundus, PartSizes.Primis];
            PartesOfContent[2].Koords = MyFunc.Set(Sizes001.Primis - 2 * PartSizes.Primis - 1, 1);

            PartesOfContent[3].Array = new char[PartSizes.Secundus, PartSizes.Primis];
            PartesOfContent[3].Koords = MyFunc.Set(Sizes001.Primis - PartSizes.Primis - 1, 1);
            
            //Названия полей
            Labels[0].SomeString = " Content ";
            Labels[1].SomeString = " Type ";
            Labels[2].SomeString = " Attribute ";
            Labels[3].SomeString = " Size ";
            
            //По левому краю
            Labels[0].Koords = MyFunc.Set(PartesOfContent[0].Koords.Primis, 0);
            
            //Выравнивание по центру всех кроме самого первого заголовка
            for (int i = 1; i < 4; ++i)
            {                
                int Aligner001 = MyFunc.AlignString(Labels[i].SomeString.Length, PartesOfContent[i].Array.GetLength(1), 1);
                Labels[i].Koords = MyFunc.Set(PartesOfContent[i].Koords.Primis + Aligner001, 0);
            }
            /*
            Labels[1].Koords = MyFunc.Set(PartesOfContent[1].Koords.Primis, 0);
            Labels[2].Koords = MyFunc.Set(PartesOfContent[2].Koords.Primis, 0);
            Labels[3].Koords = MyFunc.Set(PartesOfContent[3].Koords.Primis + Labels[2].SomeString.Length / 2, 0);*/

            //Запись заголовков с выравниванием
            MyFunc.CopyStringToArray(Labels[0].Koords, Labels[0].SomeString, MainField.Array, true);
            MyFunc.CopyStringToArray(Labels[1].Koords, Labels[1].SomeString, MainField.Array, true);
            MyFunc.CopyStringToArray(Labels[2].Koords, Labels[2].SomeString, MainField.Array, true);
            MyFunc.CopyStringToArray(Labels[3].Koords, Labels[3].SomeString, MainField.Array, true);
        }

        public void Show(ref List<Global.Str_Properties> NewEntaries001, int Min001, int Max001)
        {
            if (NewEntaries001.Any() && NewEntaries001 == null)
            {
                return;
            }

            MyFunc.FillArray(PartesOfContent[0].Array, ' ');
            MyFunc.FillArray(PartesOfContent[1].Array, ' ');
            MyFunc.FillArray(PartesOfContent[2].Array, ' ');
            MyFunc.FillArray(PartesOfContent[3].Array, ' ');

            MyFunc.Geminus<int> TempoPlacer = MyFunc.Set(0, 0);
            MyFunc.Geminus<int> SizePlacer = MyFunc.Set(0, 0);
            MyFunc.Geminus<int> AlignPlacer = MyFunc.Set(0, 0);
            string NameSize;

            for (int i = Min001; i < Max001; ++i)
            { 
                MyFunc.CopyStringToArray(ref TempoPlacer, NewEntaries001[i].Name, PartesOfContent[0].Array, true);
                
                AlignPlacer = TempoPlacer;
                AlignPlacer.Primis = MyFunc.AlignString(NewEntaries001[i].Type.ToString().Length, PartesOfContent[1].Array.GetLength(1), 1);
                if(NewEntaries001[i].Type != Global.Types.LevelUp)
                {
                    MyFunc.CopyStringToArray(ref AlignPlacer, NewEntaries001[i].Type.ToString(), PartesOfContent[1].Array, true);
                }
                AlignPlacer.Primis = MyFunc.AlignString(NewEntaries001[i].Attribute.Length, PartesOfContent[2].Array.GetLength(1), 1);
                MyFunc.CopyStringToArray(ref AlignPlacer, NewEntaries001[i].Attribute, PartesOfContent[2].Array, true);
                if(NewEntaries001[i].Size > 0)
                {
                    SizePlacer = TempoPlacer;
                    NameSize = NewEntaries001[i].Size.ToString();
                    //SizePlacer.Primis = TempoPlacer .Primis + PartesOfContent[3].Array.GetLength(1) - NameSize.Length;
                    SizePlacer.Primis = TempoPlacer.Primis + MyFunc.AlignString(NameSize.Length, PartesOfContent[3].Array.GetLength(1), 2);
                    MyFunc.CopyStringToArray(ref SizePlacer, NameSize, PartesOfContent[3].Array, true);
                }
                
                ++TempoPlacer.Secundus;
            }

            MyFunc.Copy(ref PartesOfContent[0].Koords, PartesOfContent[0].Array, MainField.Array);
            MyFunc.Copy(ref PartesOfContent[1].Koords, PartesOfContent[1].Array, MainField.Array);
            MyFunc.Copy(ref PartesOfContent[2].Koords, PartesOfContent[2].Array, MainField.Array);
            MyFunc.Copy(ref PartesOfContent[3].Koords, PartesOfContent[3].Array, MainField.Array);

            MyFunc.Copy(ref MainField.Koords, MainField.Array, Global.Field);
        }

        public int GetHorLengthMarker()
        {
            return PartesOfContent[0].Array.GetLength(1);
        }

        public MyFunc.Geminus<int> GetKoords()
        {
            return MainField.Koords;
        }
    }
}
