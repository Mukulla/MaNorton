using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class Nortoneiro
    {
        //Вертикальный индекс элемента в полном списке
        MyLib.Stricto VertIndex = new Stricto();
        //Вертикальный индекс элемента в видимом списке
        MyLib.Stricto VisIndex = new Stricto();
        //Прошлые координаты видимого и невидимого индексов
        MyFunc.Geminus<int> LastIndex = MyFunc.Set(0, 0);
        //Прошлые координаты верхней и нижней границ отображения
        MyFunc.Geminus<int> LastBorders = MyFunc.Set(0, 0);

        List<MyFunc.Quadrupla<int>> LastIndexes = new List<MyFunc.Quadrupla<int>>();
        //Направление погружения
        bool IsUp = false;
        int VertStep = 0;

        //Границы копируемой области в массив отображения
        MyLib.Stricto BorderUp = new Stricto();
        MyLib.Stricto BorderDown = new Stricto();

        //Горизонтальные границы для закраски
        MyLib.Stricto Left = new Stricto();
        MyLib.Stricto Right = new Stricto();

        //Код клавиши
        ConsoleKey SomeKey;

        //Поле для помещения туда списка
        ObjArray2D VisibleContent = new ObjArray2D();
        MyFunc.Geminus<int> ContentSizes;

        //Основной маркер для выбора позиций
        Marker MainMarker = new Marker();

        //Названия и типы содержимого отображаемой на текущий момент папки
        List<Global.DPoint> DirEntare;

        //string TestPath = @"C:\Git\Lessons\Lesson002";
        //string CurrentPath = @"C:\Git\Lessons";
        string CurrentPath = @"C:\Windows";
        //string TestPath = @"C:";

        PathWalker MaWalker = new PathWalker();

        string NewPath = "";
        //Global.ObjString Nintus;
        ObjArray1D Nintus = new ObjArray1D();

        ObjArray1D LabelCurrentPath = new ObjArray1D();

        char Slasher = '\\';
        string LevelUpper = "...";

        int WidthPlacer = 29;

        int HorizontalCollumn = 2;
        MyFunc.Geminus<int>[] AncPoints;

        public Nortoneiro()
        {
            MaWalker.Init(@"C:\Windows", 0);

            ContentSizes = MyFunc.Set( Global.Sizes.Primis / 6, ( Global.Sizes.Secundus * 2 ) / 3 );

            AncPoints = new MyFunc.Geminus<int>[4];
            AncPoints[0] = MyFunc.Set(HorizontalCollumn, 3);
            AncPoints[1] = MyFunc.Set(HorizontalCollumn, 5);
            AncPoints[2] = MyFunc.Set(HorizontalCollumn, AncPoints[1].Secundus + ContentSizes.Secundus + 2);
            AncPoints[3] = MyFunc.Set(HorizontalCollumn, Global.Sizes.Secundus - 2);

            //Console.BackgroundColor = ConsoleColor.DarkGray;
            //Console.ForegroundColor = ConsoleColor.Gray;

            LabelCurrentPath.Init(AncPoints[0], CurrentPath, WidthPlacer);

            VisibleContent.Init(AncPoints[1], ContentSizes.Secundus, WidthPlacer, MyFunc.Set(1, 1));
            /*
            Placer.Field = new char[Global.Sizes.Secundus - 12, WidthPlacer];            
            Placer.Koords = AncPoints[1];*/
            VertStep = ContentSizes.Secundus / 2;

            Nintus.Init(AncPoints[2], "", WidthPlacer);
            /*
            Nintus.SomeArray = new char[Placer.Field.GetLength(1)];
            //Nintus.SomeString = "";
            Nintus.Koords = AncPoints[2];*/

            MainMarker.SetHorizontalLimites(0, HorizontalCollumn, HorizontalCollumn + ContentSizes.Primis - 1, Global.Sizes.Primis);


            Left.SetMinMax(0, Global.Sizes.Primis - 1);
            Left.Set(HorizontalCollumn);

            Right.SetMinMax(0, Global.Sizes.Primis);
            Right.Set(HorizontalCollumn + ContentSizes.Primis - 1);

            NewPath = CurrentPath; 
        }
        public void PrepareDirectory()
        {
            /*
            try
            {
                GetEntariesDir(ref DirEntare, MaWalker.GetPath());                
                CurrentPath = NewPath;
            }
            catch (UnauthorizedAccessException)
            {
                Nintus.NewName("Access Denied");
                //MyFunc.CopyStringToArray(0, "Access Denied", Nintus.SomeArray);
                //Nintus.SomeString = "Access Denied";
                //MyFunc.Copy(ref Nintus.Koords, Nintus.SomeArray, Global.Field, true);
                //MyFunc.CopyStringToArray(ref Nintus.Koords, Nintus.SomeString, Global.Field, true);

                //Console.WriteLine("Access Denied");
            }*/

            GetEntariesDir(ref DirEntare, MaWalker.GetPath());
            LabelCurrentPath.NewName(MaWalker.GetPath());
            
            MainMarker.Init(0, AncPoints[1].Secundus, ContentSizes.Secundus, DirEntare.Count);

            /*
            MainMarker.Reset();
            
            MainMarker.SetVerticalLImites(0, 0, Placer.Field.GetLength(0), DirEntare.Count);
            MainMarker.SetHorizontalLimites(0, Placer.Koords.Primis, Placer.Koords.Primis + Placer.Field.GetLength(1), Global.Sizes.Primis);
            MainMarker.SetGlobalKoords(Placer.Koords.Primis, Placer.Koords.Secundus);*/

            

            VertIndex.SetMinMax(0, DirEntare.Count - 1);
            VertIndex.Set(0);
            if(VertIndex.GetMax() <= ContentSizes.Secundus)
            {
                VisIndex.SetMinMax(AncPoints[1].Secundus, AncPoints[1].Secundus + VertIndex.GetMax());
                
                BorderUp.SetMinMax(0, 0);
                BorderDown.SetMinMax(DirEntare.Count, DirEntare.Count);
            }
            else
            {
                VisIndex.SetMinMax(AncPoints[1].Secundus, AncPoints[1].Secundus + ContentSizes.Secundus - 1);
                
                BorderUp.SetMinMax(0, DirEntare.Count - ContentSizes.Secundus);
                BorderDown.SetMinMax(ContentSizes.Secundus, DirEntare.Count);                
            }            
            VisIndex.Set(0);

            BorderUp.Set(0);
            BorderDown.Set(0);
            
            if(IsUp && LastIndexes.Any())
            {
                MyFunc.Quadrupla<int> Tempo = LastIndexes.Last();
                
                VertIndex.Set(Tempo.Primis);
                VisIndex.Set(Tempo.Secundus);

                BorderUp.Set(Tempo.Primis);
                BorderDown.Set(Tempo.Secundus);

                LastIndexes.RemoveAt(LastIndexes.Count - 1);
                /*
                VertIndex.Set(LastIndex.Primis);
                VisIndex.Set(LastIndex.Secundus);

                BorderUp.Set(LastBorders.Primis);
                BorderDown.Set(LastBorders.Secundus);
                */
                IsUp = false;
            }

            //VisibleContent.NewContent(ref DirEntare, MainMarker.GetMin(), MainMarker.GetMax());
            //VisibleContent.NewContent(ref DirEntare, BorderUp.Get(), BorderDown.Get());
        }
        void GetEntariesDir(ref List<Global.DPoint> SomeEntaries, string SomePath001)
        {
            Global.DPoint TempoPoint;

            string[] CurrentDir = Directory.GetDirectories(SomePath001);
            string[] ArrayFiles = Directory.GetFiles(SomePath001);
            
            SomeEntaries = new List<Global.DPoint>();

            //Если количество слешей больше одного,
            //добавляем многоточие в начало
            string[] Tempo = SomePath001.Split(Slasher);
            if (Tempo.Length > 1 && Tempo[Tempo.Length - 1] != "")
            {
                //Отмечаем, что это переход на уровень выше
                TempoPoint.Name = LevelUpper;
                TempoPoint.Type = Global.Types.LevelUp;

                SomeEntaries.Add(TempoPoint);
            }
            //Записываем имена директорий
            for (int i = 0; i < CurrentDir.Length; ++i)
            {               
                DirectoryInfo SomeDir = new DirectoryInfo(CurrentDir[i]);
                //Отмечаем, что это директория
                TempoPoint.Name = SomeDir.Name;
                TempoPoint.Type = Global.Types.Directory;

                SomeEntaries.Add(TempoPoint);
            }
            //Записываем имена файлов в поле отображения
            for (int i = 0; i < ArrayFiles.Length; ++i)
            {                
                string FileName = Path.GetFileName(ArrayFiles[i]);
                //Отмечаем, что это файл
                TempoPoint.Name = FileName;
                TempoPoint.Type = Global.Types.File;

                SomeEntaries.Add(TempoPoint);
            }
        }
        /*
        void CopyDirToPlacer(ref List<Global.DPoint> SomeEntaries, Global.Object SomeObject001)
        {
            MyFunc.FillArray(SomeObject001.Field, ' ');            

            MyFunc.Geminus<int> TempoPlacer = MyFunc.Set(0, 0);
            //int Limit = SomeObject001.Field.GetLength(0);

            for (int i = BorderUp.Get(); i < BorderDown.Get(); ++i)
            //for (int i = 0; i < SomeEntaries.Count; ++i)
            //for (int i = MainMarker.Get(2); i < MainMarker.Get(3); ++i)
            {
                //if(i == Limit)
                //{
                //    break;
                //}
                MyFunc.CopyStringToArray(ref TempoPlacer, SomeEntaries[i].Name, SomeObject001.Field, true);
                ++TempoPlacer.Secundus;
            }
            
            //Копируем поле отображения в основное поле со смещением
            MyFunc.Copy(ref SomeObject001.Koords, SomeObject001.Field, Global.Field);            
        }*/

        public void HandleKeys()
        {
            SomeKey = Console.ReadKey().Key;

            //Выход из программы
            if (SomeKey == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }

            if (SomeKey == ConsoleKey.Enter)
            {
                int CurrentIndex = BorderUp.Get() + VisIndex.Get() - VisIndex.GetMin();
                VertIndex.Set(CurrentIndex);
                if (DirEntare[MainMarker.GetIndex()].Type == Global.Types.LevelUp)
                //if (DirEntare[VertIndex.Get()].Type == Global.Types.LevelUp)
                {
                    IsUp = true;

                    //NewPath = NewLevelUPPath(CurrentPath);

                    MaWalker.LevelUp();

                    PrepareDirectory();
                    return;
                }
                if (DirEntare[MainMarker.GetIndex()].Type == Global.Types.Directory)
                //if (DirEntare[VertIndex.Get()].Type == Global.Types.Directory)
                {
                    LastIndexes.Add(MyFunc.Set(VertIndex.Get(), VisIndex.Get(), BorderUp.Get(), BorderDown.Get()));
                    LastIndex.Primis = VertIndex.Get();
                    LastIndex.Secundus = VisIndex.Get();

                    LastBorders.Primis = BorderUp.Get();
                    LastBorders.Secundus = BorderDown.Get();

                    //NewPath = CurrentPath + Slasher + DirEntare[VertIndex.Get()].Name;
                    //NewPath = NewLevelDownPath(CurrentPath, DirEntare[VertIndex.Get()].Name);

                    MaWalker.LevelDown(DirEntare[MainMarker.GetIndex()].Name);
                    //MaWalker.LevelDown(DirEntare[VertIndex.Get()].Name);
                    Nintus.NewName(MaWalker.GetAtteintion());

                    PrepareDirectory();
                    return;
                }                
            }

            //Смещение маркера вверх или вниз
            if (SomeKey == ConsoleKey.DownArrow)
            {
                /*
                VertIndex.Do(1, 0);
                VisIndex.Do(1, 0);

                if(VisIndex.IsMax())
                {
                    BorderUp.Do(1, 0);
                    BorderDown.Do(1, 0);
                }*/

                MainMarker.OffSet(1);
                return;
            }
            if (SomeKey == ConsoleKey.UpArrow)
            {/*
                VertIndex.Do(1, 1);
                VisIndex.Do(1, 1);
                if (VisIndex.IsMin())
                {
                    BorderUp.Do(1, 1);
                    BorderDown.Do(1, 1);
                }*/

                MainMarker.OffSet(-1);
                return;
            }
            
            if (SomeKey == ConsoleKey.PageDown)
            {/*
                VertIndex.Do(VertStep, 0);
                VisIndex.Do(VertStep, 0);

                BorderUp.Do(VertStep, 0);
                BorderDown.Do(VertStep, 0);
                */
                MainMarker.OffSet(2);
                return;
            }
            if (SomeKey == ConsoleKey.PageUp)
            {/*
                VertIndex.Do(VertStep, 1);
                VisIndex.Do(VertStep, 1);

                BorderUp.Do(VertStep, 1);
                BorderDown.Do(VertStep, 1);
                */
                MainMarker.OffSet(-2);
                return;
            }

            //Перемещение в начало или конец списка            
            if (SomeKey == ConsoleKey.End)
            {/*
                VertIndex.Maximalize();
                VisIndex.Maximalize();
                BorderUp.Maximalize();
                BorderDown.Maximalize();
                */
                MainMarker.OffSet(3);
                return;
            }
            if (SomeKey == ConsoleKey.Home)
            {/*
                VertIndex.Minimalize();
                VisIndex.Minimalize();
                BorderUp.Minimalize();
                BorderDown.Minimalize();
                */
                MainMarker.OffSet(-3);
                return;
            }
        }
        /*
        string NewLevelUPPath(string SomePath001)
        {
            string[] PartesPath = SomePath001.Split(Slasher);
            
            string NewPath = "";
            
            if(PartesPath.Length < 3)
            {
                NewPath += PartesPath[0];
                NewPath += Slasher;
                return NewPath;
            }            
            
            for (int i = 0; i < PartesPath.Length - 1; ++i)
            {                
                NewPath += PartesPath[i];
                if(i != PartesPath.Length - 2)
                {
                    NewPath += Slasher;
                }                
            }

            return NewPath;
        }

        string NewLevelDownPath(string SomePath001, string NameDir)
        {
            string[] PartesPath = SomePath001.Split(Slasher);

            string NewPath = "";

            if (PartesPath.Length < 3 && PartesPath[1] == "")
            {
                NewPath += PartesPath[0];
                NewPath += Slasher;
                NewPath += NameDir;
                return NewPath;
            }

            foreach (var Item in PartesPath)
            {
                NewPath += Item;
                NewPath += Slasher;                
            }

            NewPath += NameDir;

            return NewPath;
        }*/

        public void Show()
        {
            //CopyDirToPlacer(ref DirEntare, Placer);
            
            LabelCurrentPath.Show();
            //VisibleContent.Show(ref DirEntare, MainMarker.GetMin(), MainMarker.GetMax());            
            Nintus.Show();

            //MyFunc.Copy(ref LabelCurrentPath.Koords, LabelCurrentPath.SomeArray, Global.Field, true);

            for (int i = 0; i < Global.Sizes.Secundus; ++i)
            {
                //Сравниваем итоговый индекс маркера с итератором
                if(i == MainMarker.GetVisIndex())
                //if(i == VisIndex.Get())
                //if(i == MainMarker.Get(1))
                {
                    //MainMarker.Colorize();
                    //Colorize();
                    //ColoriseSubString(ref MarkerKoords, Placer, ConsoleColor.DarkBlue);
                    continue;
                }
                Console.WriteLine(MyFunc.GetString(i, Global.Field));
            }
        }
        public void Colorize()
        {
            //TotalIndex = VerticalIndex + GlobalKoords.Secundus;
            //Colorize LEFT PART
            Console.Write(MyFunc.GetString(VisIndex.Get(), Left.GetMin(), Left.Get(), Global.Field));

            //Colorize CENTER PART
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(MyFunc.GetString(VisIndex.Get(), Left.Get(), Right.Get(), Global.Field));
            Console.ResetColor();

            //Colorize RIGHT PART
            string Str001 = MyFunc.GetString(VisIndex.Get(), Right.Get(), Right.GetMax(), Global.Field);
            Console.Write(Str001);

            //Move To The Next Line
            Console.WriteLine();
        }
        /*
        void ColoriseSubString(ref MyFunc.Geminus<int> SomeKoords, Str_Object SomePlace, ConsoleColor SomeColor)
        {
            MyFunc.Geminus<int> MinMax = MyFunc.Set(0, SomePlace.Koords.Primis);
            Console.Write(MyFunc.GetString(SomeKoords.Secundus, ref MinMax, MainField));

            Console.BackgroundColor = SomeColor;
            MinMax = MyFunc.Set(SomePlace.Koords.Primis, SomePlace.Koords.Primis + SomePlace.Field.GetLength(1));
            Console.Write(MyFunc.GetString(SomeKoords.Secundus, ref MinMax, MainField));
            Console.ResetColor();

            MinMax = MyFunc.Set(SomePlace.Koords.Primis + SomePlace.Field.GetLength(1), MainField.GetLength(1));
            Console.Write(MyFunc.GetString(SomeKoords.Secundus, ref MinMax, MainField));
            Console.WriteLine();
        }*/
    }
}
