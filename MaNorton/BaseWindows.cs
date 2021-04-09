using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    //Общие окна - подтверждение, выбор диска
    class BaseWindows
    {
        //Включить или выключить
        public bool Enabled = false;
        //Тип и номер созданного окна
        int NumberWindow = 0;
        //Заголовок
        Global.Str_String Label;
        //Поле с содержимым
        Global.Str_Array2D Field;
        //Горизонтальные координаты закрашенной области выбора
        MyFunc.Quadrupla<int>[] HorizKoords;
        //Вертикальные координаты выделенной области
        int CurrentIndex = 0;
        //Замкнуный номер выбранного диска или ответа
        MyLib.Clausa1D Indexer = new Clausa1D();
        //Найденные диски
        DriveInfo[] Drives;
        //Путь с новым диском
        string NewPath = string.Empty;

        public void Init(int Number001)
        {
            NumberWindow = Number001;
            switch (Number001)
            {
                case 0:
                    MakeOkCancel();
                    break;
                case 1:
                    MakeDiskSelecter();
                    break;
            }
        }
        void MakeOkCancel()
        {
            MyFunc.Geminus<int> Sizes = MyFunc.Set(36, 5);
            Field.Array = new char[Sizes.Secundus, Sizes.Primis];
            Field.Koords = MyFunc.Set(MyFunc.AlignString(Sizes.Primis, Global.Sizes.Primis, 1), MyFunc.AlignString(Sizes.Secundus, Global.Sizes.Secundus, 1));

            MyFunc.FillArray(Field.Array, ' ');

            string[] Names = { "YES", "NO" };
            MyFunc.CopyStringToArray(MyFunc.Set(MyFunc.AlignString(Names[0].Length, Sizes.Primis / 2, 1), 2), Names[0], Field.Array, true);
            MyFunc.CopyStringToArray(MyFunc.Set(MyFunc.AlignString(Names[1].Length, Sizes.Primis / 2, 1) + 18, 2), Names[1], Field.Array, true);

            Label.SomeString = string.Empty;
            Label.Koords = MyFunc.Set(2, 0);

            Global.FillBroders(Field.Array);
            //MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field.Array, true);

            HorizKoords = new MyFunc.Quadrupla<int>[2];
            HorizKoords[0] = MyFunc.Set(0, Field.Koords.Primis + 2, Field.Koords.Primis + Sizes.Primis / 2 - 1, Global.Sizes.Primis);
            HorizKoords[1] = MyFunc.Set(0, Field.Koords.Primis + Sizes.Primis / 2 + 1, Field.Koords.Primis + Sizes.Primis - 2, Global.Sizes.Primis);

            CurrentIndex = Field.Koords.Secundus + 2;

            Indexer.SetMinMax(0, 1);
            Indexer.Set(0);
        }

        void MakeDiskSelecter()
        {
            int Step = 7;

            Drives = DriveInfo.GetDrives();

            MyFunc.Geminus<int> Sizes = MyFunc.Set(Drives.Length * Step + 2, 5);
            Field.Array = new char[Sizes.Secundus, Sizes.Primis];
            Field.Koords = MyFunc.Set(MyFunc.AlignString(Sizes.Primis, Global.Sizes.Primis, 1), Global.Sizes.Secundus / 4);

            MyFunc.FillArray(Field.Array, ' ');

            Label.SomeString = string.Empty;
            Label.Koords = MyFunc.Set(2, 0);

            Global.FillBroders(Field.Array);

            HorizKoords = new MyFunc.Quadrupla<int>[Drives.Length]; 
            string TempoName = string.Empty;
            for (int i = 0; i < Drives.Length; ++i)
            {
                HorizKoords[i] = MyFunc.Set(0, i * Step + Field.Koords.Primis + 2, ( i + 1 ) * Step + Field.Koords.Primis, Global.Sizes.Primis);

                TempoName += Drives[i].Name[0];
                MyFunc.CopyStringToArray(MyFunc.Set( i * Step + 4,  2), TempoName, Field.Array, true);
                TempoName = string.Empty;
            }

            CurrentIndex = Field.Koords.Secundus + 2;

            Indexer.SetMinMax(0, Drives.Length - 1);
            Indexer.Set(0);
        }

        public int HandleKeys()
        {
            switch (NumberWindow)
            {
                case 0:
                    return OKCANCELKeys();
                case 1:
                    return DiskSelecterKeys();
            }
            return 0;
        }
        int OKCANCELKeys()
        { 
            if (!Enabled)
            {
                return 0;
            }

            if (MaKeys.Get(ConsoleKey.Escape))
            {
                Enabled = false;
                return -1;
            }

            if (MaKeys.Get(ConsoleKey.LeftArrow))
            {
                Indexer.Do(1, 1);
                return 0;
            }
            if (MaKeys.Get(ConsoleKey.RightArrow))
            {
                Indexer.Do(1, 0);
                return 0;
            }

            if (MaKeys.Get(ConsoleKey.Enter))
            {
                if (Indexer.Get() == 0)
                {
                    Enabled = false;
                    return 1;
                }
                if (Indexer.Get() == 1)
                {
                    Enabled = false;
                    return -1;
                }
            }

            return 0;
        }
        
        int DiskSelecterKeys()
        {
            if (!Enabled)
            {
                return 0;
            }

            if (MaKeys.Get(ConsoleKey.Escape))
            {
                Enabled = false;
                return -1;
            }

            if (MaKeys.Get(ConsoleKey.LeftArrow))
            {
                Indexer.Do(1, 1);
                return 0;
            }
            if (MaKeys.Get(ConsoleKey.RightArrow))
            {
                Indexer.Do(1, 0);
                return 0;
            }

            if (MaKeys.Get(ConsoleKey.Enter))
            {
                NewPath = Drives[Indexer.Get()].Name;
                Enabled = false;
                return 1;
            }

            return 0;
        }

        public void SetLabel(string NewLabel001, int Align)
        {
            Label.SomeString = NewLabel001;
            switch (Align)
            {
                case 0:
                    Label.Koords = MyFunc.Set(1, 0);
                    break;
                case 1:
                    Label.Koords = MyFunc.Set(MyFunc.AlignString(NewLabel001.Length, Field.Array.GetLength(1), 1), 0);
                    break;
                case 2:
                    Label.Koords = MyFunc.Set(MyFunc.AlignString(NewLabel001.Length, Field.Array.GetLength(1), 2) - 1, 0);
                    break;
            }

            MyFunc.CopyStringToArray(Label.Koords, Label.SomeString, Field.Array, true);
        }

        public string GetNewPath()
        {
            return NewPath;
        }

        public void Show()
        {
            if (!Enabled)
            {
                return;
            }
            //MyFunc.FillArray(Field, ' ');

            Global.AddMarkPainterKoords(CurrentIndex, HorizKoords[Indexer.Get()]);

            MyFunc.Copy(ref Field.Koords, Field.Array, Global.Field);
        }
    }
}
