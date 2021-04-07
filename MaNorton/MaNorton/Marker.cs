using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class Marker
    {
        //Вертикальный индекс элемента в полном списке
        MyLib.Stricto VertIndex = new Stricto();
        //Вертикальный индекс элемента в видимом списке
        MyLib.Stricto VisIndex = new Stricto();
        //Прошлые координаты видимого и невидимого индексов
        //MyFunc.Geminus<int> LastIndex = MyFunc.Set(0, 0);
        //Прошлые координаты верхней и нижней границ отображения
        //MyFunc.Geminus<int> LastBorders = MyFunc.Set(0, 0);

        //Список координатами и границами предъидущего уровня
        List<MyFunc.Quadrupla<int>> LastIndexes = new List<MyFunc.Quadrupla<int>>();
        //Направление погружения
        bool IsUp = false;
        //Шаг смещения для постраничного вывода
        int VertStep = 0;

        //Границы копируемой области в массив отображения
        MyLib.Stricto BorderUp = new Stricto();
        MyLib.Stricto BorderDown = new Stricto();

        //Горизонтальные границы для закраски
        MyLib.Stricto BorderLeft = new Stricto();
        MyLib.Stricto BorderRight = new Stricto();

        public void Init(int Top001, int FieldKoordSecundus, int FieldHeigth, int ListLength)
        {
            VertIndex.SetMinMax(Top001, ListLength - 1);
            VertIndex.Set(0);
            if (VertIndex.GetMax() <= FieldHeigth)
            {
                VisIndex.SetMinMax(FieldKoordSecundus, FieldKoordSecundus + VertIndex.GetMax());

                BorderUp.SetMinMax(0, 0);
                BorderDown.SetMinMax(ListLength, ListLength);
            }
            else
            {
                VisIndex.SetMinMax(FieldKoordSecundus, FieldKoordSecundus + FieldHeigth - 1);

                BorderUp.SetMinMax(Top001, ListLength - FieldHeigth);
                BorderDown.SetMinMax(FieldHeigth, ListLength);
            }
            VisIndex.Set(0);

            BorderUp.Set(0);
            BorderDown.Set(0);

            VertStep = FieldHeigth / 2;

            if (IsUp && LastIndexes.Any())
            //if (IsUp)
            { 
                MyFunc.Quadrupla<int> Tempo = LastIndexes.Last();

                VertIndex.Set(Tempo.Primis);
                VisIndex.Set(Tempo.Secundus);

                BorderUp.Set(Tempo.Tertium);
                BorderDown.Set(Tempo.Quartus);

                LastIndexes.RemoveAt(LastIndexes.Count - 1);
                IsUp = false;
            }
        }

        public void SetHorizontalLimites(int Left001, int Min001, int Max001, int Right001)
        {
            BorderLeft.SetMinMax(Left001, Right001 - 1);
            BorderLeft.Set(Min001);

            BorderRight.SetMinMax(0, Right001);
            BorderRight.Set(Max001);
        }
        public void HandleKeys()
        {
            //Смещение маркера вверх или вниз
            if (MaKeys.Get(ConsoleKey.DownArrow))
            {
                VertIndex.Do(1, 0);
                VisIndex.Do(1, 0);

                if (VisIndex.IsMax())
                {
                    BorderUp.Do(1, 0);
                    BorderDown.Do(1, 0);
                }
                return;
            }
            if (MaKeys.Get(ConsoleKey.UpArrow))
            {
                VertIndex.Do(1, 1);
                VisIndex.Do(1, 1);
                if (VisIndex.IsMin())
                {
                    BorderUp.Do(1, 1);
                    BorderDown.Do(1, 1);
                }
                return;
            }

            //Постраничное смещение
            if (MaKeys.Get(ConsoleKey.PageDown))
            {
                VertIndex.Do(VertStep, 0);
                VisIndex.Do(VertStep, 0);

                BorderUp.Do(VertStep, 0);
                BorderDown.Do(VertStep, 0);
                return;
            }
            if (MaKeys.Get(ConsoleKey.PageUp))
            {
                VertIndex.Do(VertStep, 1);
                VisIndex.Do(VertStep, 1);

                BorderUp.Do(VertStep, 1);
                BorderDown.Do(VertStep, 1);
                return;
            }

            //Перемещение в начало или конец списка            
            if (MaKeys.Get(ConsoleKey.End))
            {
                VertIndex.Maximalize();
                VisIndex.Maximalize();
                BorderUp.Maximalize();
                BorderDown.Maximalize();
                return;
            }
            if (MaKeys.Get(ConsoleKey.Home))
            {
                VertIndex.Minimalize();
                VisIndex.Minimalize();
                BorderUp.Minimalize();
                BorderDown.Minimalize();
            }
        }
        /*
        public void OffSet(int Direction)
        {
            switch (Direction)
            {
                //Home
                case -3:
                    VertIndex.Minimalize();
                    VisIndex.Minimalize();
                    BorderUp.Minimalize();
                    BorderDown.Minimalize();
                    break;
                //PageUp
                case -2:
                    VertIndex.Do(VertStep, 1);
                    VisIndex.Do(VertStep, 1);

                    BorderUp.Do(VertStep, 1);
                    BorderDown.Do(VertStep, 1);
                    break;
                //Сместить вверх
                case -1:
                    VertIndex.Do(1, 1);
                    VisIndex.Do(1, 1);
                    if (VisIndex.IsMin())
                    {
                        BorderUp.Do(1, 1);
                        BorderDown.Do(1, 1);
                    }
                    break;
                //Сместить вниз
                case 1:
                    VertIndex.Do(1, 0);
                    VisIndex.Do(1, 0);

                    if (VisIndex.IsMax())
                    {
                        BorderUp.Do(1, 0);
                        BorderDown.Do(1, 0);
                    }
                    break;
                //PageDown
                case 2:
                    VertIndex.Do(VertStep, 0);
                    VisIndex.Do(VertStep, 0);

                    BorderUp.Do(VertStep, 0);
                    BorderDown.Do(VertStep, 0);
                    break;
                //End
                case 3:
                    VertIndex.Maximalize();
                    VisIndex.Maximalize();
                    BorderUp.Maximalize();
                    BorderDown.Maximalize();
                    break;
            }
        }*/

        public void SetDirection(bool State001)
        {
            IsUp = State001;
            if(!IsUp)
            {
                LastIndexes.Add(MyFunc.Set(VertIndex.Get(), VisIndex.Get(), BorderUp.Get(), BorderDown.Get()));
            }
        }

        public int GetVisIndex()
        {
            return VisIndex.Get();
        }
        public int GetIndex()
        {
            VertIndex.Set(BorderUp.Get() + VisIndex.Get() - VisIndex.GetMin());
            return VertIndex.Get();
        }

        public int GetMin()
        {
            return BorderUp.Get();
        }

        public int GetMax()
        {
            return BorderDown.Get();
        }
        public MyFunc.Quadrupla<int> GetHorizKoords()
        {
            return MyFunc.Set(BorderLeft.GetMin(), BorderLeft.Get(), BorderRight.Get(), BorderRight.GetMax());
        }
        /*
          
        public void Colorize()
        {
            //Colorize LEFT PART
            Console.Write(MyFunc.GetString(VisIndex.Get(), BorderLeft.GetMin(), BorderLeft.Get(), Global.Field));

            //Colorize CENTER PART
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(MyFunc.GetString(VisIndex.Get(), BorderLeft.Get(), BorderRight.Get(), Global.Field));
            Console.ResetColor();

            //Colorize RIGHT PART
            string Str001 = MyFunc.GetString(VisIndex.Get(), BorderRight.Get(), BorderRight.GetMax(), Global.Field);
            Console.Write(Str001);

            //Move To The Next Line
            Console.WriteLine();
        }
        
        public void ColorizeMarkar()
        {
            //TotalIndex = VerticalIndex + GlobalKoords.Secundus;
            //Colorize LEFT PART
            Console.Write(MyFunc.GetString(TotalIndex, HorizontalLimiters.Primis, HorizontalLimiters.Secundus, SomeField));

            //Colorize CENTER PART
            Console.BackgroundColor = SomeColor;
            Console.Write(MyFunc.GetString(TotalIndex, HorizontalLimiters.Secundus, HorizontalLimiters.Tertium, SomeField));
            Console.ResetColor();

            //Colorize RIGHT PART
            string Str001 = MyFunc.GetString(TotalIndex, HorizontalLimiters.Tertium, HorizontalLimiters.Quartus, SomeField);
            Console.Write(Str001);

            //Move To The Next Line
            Console.WriteLine();
        }

        public int Get(int Number)
        {
            if(Number == 1)
            {
                return TotalIndex;
            }
            if (Number == 2)
            {
                return VerticalLimites.Secundus;
            }
            if (Number == 3)
            {
                return VerticalLimites.Tertium;
            }
            return VerticalIndex;
        }*/
    }
}
