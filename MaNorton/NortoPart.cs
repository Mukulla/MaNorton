using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;


namespace MaNorton
{
    class NortoPart
    {
        //Включить или выключить
        public bool Enabled = false;
        //Отключить отображение маркера
        public bool Active = false;

        //Переход по уровням директорий
        PathWalker MaWalker = new PathWalker();
        //Названия и типы содержимого отображаемой на текущий момент папки
        DirectoryContent CurrentContent = new DirectoryContent();
        //Основной маркер для выбора позиций
        Marker MainMarker = new Marker();
        //Заголовок с текущим путём
        ObjArray2D CurrentPath = new ObjArray2D();
        //Поле для помещения туда списка
        Content TableContent = new Content();
        //Размеры
        MyFunc.Geminus<int> ContentSizes;
        //Имя файла с записанной последней посещённой директорией
        string ConfigFile = "Config.nor";
        //Список полных имён выделенных файлов или папок
        List<string> ListFullNames = new List<string>();
        List<string> ListNames = new List<string>();
        //Список типов выделенных файлов или папок
        List<Global.Types> ListTypes = new List<Global.Types>();

        public void Init(MyFunc.Geminus<int> Koords001, int NumberPart001)
        {
            //Получаем декущий путь
            MaWalker.Init(ConfigFile, NumberPart001);
            //Указываем размеры относительно размеров глобального поля
            ContentSizes = MyFunc.Set(Global.Sizes.Primis / 2 - 3, (Global.Sizes.Secundus * 3) / 4 + 2);            
            //Общая рамка
            TableContent.Init(MyFunc.Set(Koords001.Primis, Koords001.Secundus + 5), ContentSizes);
            //Указываем горизонтальные границы для маркера, они будут всё время одинаковыми
            MainMarker.SetHorizontalLimites(0, Koords001.Primis + 1, Koords001.Primis + TableContent.GetHorLengthMarker(), Global.Sizes.Primis);
            //Подготовка заголовка с полным путём
            CurrentPath.Init(MyFunc.Set(Koords001.Primis, Koords001.Secundus + 2), 3, ContentSizes.Primis, MyFunc.Set(2, 1));
            CurrentPath.SetLabel(MyFunc.Set(1, 0), " Full Path ");
            //Подготовка списка директории и сопутствующих элементов
            PrepareDirectory();
        }
        public void PrepareDirectory()
        {
            //Получаем список элементов в директории по текущему пути
            CurrentContent.Load(MaWalker.GetPath());
            //Подготавливаем границы для маркера
            MainMarker.Init(0, TableContent.GetKoords().Secundus + 1, ContentSizes.Secundus - 2, CurrentContent.GetLength());

            Global.AddToLogFile(MaWalker.GetPath(), "New Directory Opened", "Success");
        }

        public void SetNewPath(string NewPath001)
        {
            MaWalker.SetNewPath(NewPath001);
        }
        public void HandleKeys()
        {
            if (!Active)
            {
                return;
            }
            Global.Attention.Show("");
            
            //Отмена выделений
            if (MaKeys.Get(ConsoleKey.Escape))
            {
                Global.ResetTextPainter();
                return;
            }

            //Переход по уровням директорий
            if (MaKeys.Get(ConsoleKey.Enter))
            {
                Global.ResetTextPainter();
                ListFullNames.Clear();
                ListNames.Clear();
                ListTypes.Clear();

                if ( CurrentContent.GetType( MainMarker.GetIndex() ) == Global.Types.LevelUp )
                {
                    MainMarker.SetDirection(true);
                    MaWalker.LevelUp();
                    PrepareDirectory();
                    return;
                }
                if ( CurrentContent.GetType( MainMarker.GetIndex() ) == Global.Types.Directory )
                {
                    MainMarker.SetDirection(false);
                    if( MaWalker.LevelDown( CurrentContent.GetName( MainMarker.GetIndex() ) ) )
                    {
                        PrepareDirectory();
                    }
                    return;
                }
            }

            //Переход на уровень выше
            if ( MaKeys.Get( ConsoleKey.Backspace ) )
            {
                Global.ResetTextPainter();

                MainMarker.SetDirection(true);
                MaWalker.LevelUp();
                PrepareDirectory();
                return;        
            }

            //Выделение объектов
            if ( MaKeys.Get( ConsoleKey.Spacebar ) )
            {
                Global.AddTextPainterKoords(MainMarker.GetIndex(), TableContent.GetKoords().Secundus + 1, MainMarker.GetHorizKoords());
                //ListFullNames.Add(MaWalker.GetPath() + '\\' + CurrentContent.GetName(MainMarker.GetIndex()));
                ListFullNames.Add( Path.Combine( MaWalker.GetPath(), CurrentContent.GetName( MainMarker.GetIndex() ) ) );
                ListNames.Add( CurrentContent.GetName( MainMarker.GetIndex() ) );
                ListTypes.Add(CurrentContent.GetType(MainMarker.GetIndex()));
                return;
            }

            MainMarker.HandleKeys();
        }
        public void Save()
        {
            MaWalker.SavePath(ConfigFile);
        }
        public void Reset()
        {
            Global.ResetTextPainter();
            ListFullNames.Clear();
            ListNames.Clear();
            ListTypes.Clear();
        }
        public void Show()
        {
            if(!Enabled)
            {
                return;
            }
            if(Active)
            {
                //Копируем маркер
                Global.AddMarkPainterKoords(MainMarker.GetVisIndex(), MainMarker.GetHorizKoords());
                //Обновляем границы для подсветки текста
                Global.AddTextVertBorders(MyFunc.Set(MainMarker.GetMin(), MainMarker.GetMax()));
            }            

            //Копируем элементы в глобальное поле
            //LabelCurrentPath.Show();
            CurrentPath.Show(MaWalker.GetPath());

            //VisibleContent.Show(ref CurrentContent.GetContent(), MainMarker.GetMin(), MainMarker.GetMax());
            TableContent.Show(ref CurrentContent.CurrentContent, MainMarker.GetMin(), MainMarker.GetMax());
            //Nintus.Show();
        }        
        public string GetFullPath()
        {
            return Path.Combine( MaWalker.GetPath(), CurrentContent.GetName(MainMarker.GetIndex()) );
        }
        public string GetCurrentPath()
        {
            return MaWalker.GetPath();
        }
        public string GetName()
        {
            return CurrentContent.GetName(MainMarker.GetIndex());
        }
        public Global.Types GetType()
        {
            return CurrentContent.GetType(MainMarker.GetIndex());
        }
        public string GetAttribute()
        {
            return CurrentContent.GetAttribute(MainMarker.GetIndex());
        }
        public ref List<string> GetFullNames()
        {
            return ref ListFullNames;
        }
        public ref List<string> GetNames()
        {
            return ref ListNames;
        }
        public ref List<Global.Types> GetTypes()
        {
            return ref ListTypes;
        }
    }
}
