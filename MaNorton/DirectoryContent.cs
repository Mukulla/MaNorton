using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    //Загрузка и передача содержимого указанной папки
    class DirectoryContent
    {
        //Список с именем, типом, аттрибутом и размером
        public List<Global.Str_Properties> CurrentContent;

        string LevelUpper = "...";
        char Slasher = '\\';
        
        //Загрузка в один общий список
        public void Load(string SomePath001)
        {
            Global.Str_Properties TempoPoint;

            DirectoryInfo CurrentDirectory = new DirectoryInfo(SomePath001);
            DirectoryInfo[] ArrayDir = CurrentDirectory.GetDirectories();
            FileInfo[] ArrayFiles = CurrentDirectory.GetFiles();

            CurrentContent = new List<Global.Str_Properties>();

            //Если количество слешей больше одного,
            //добавляем многоточие в начало
            string[] Tempo = SomePath001.Split(Slasher);
            if (Tempo.Length > 1 && Tempo[Tempo.Length - 1] != "")
            {
                //Отмечаем, что это переход на уровень выше
                TempoPoint.Name = LevelUpper;
                TempoPoint.Type = Global.Types.LevelUp;
                TempoPoint.Attribute = "";
                TempoPoint.Size = 0;

                CurrentContent.Add(TempoPoint);
            }
            //Записываем имена директорий
            for (int i = 0; i < ArrayDir.Length; ++i)
            {
                //Отмечаем, что это директория
                TempoPoint.Name = ArrayDir[i].Name;
                TempoPoint.Type = Global.Types.Directory;
                TempoPoint.Attribute = "";
                TempoPoint.Size = 0;
                if(ArrayDir[i].Attributes.HasFlag(FileAttributes.Hidden))
                //if(ArrayDir[i].Attributes == )
                {
                    TempoPoint.Attribute = "Hidden";
                }

                CurrentContent.Add(TempoPoint);
            }
            //Записываем имена файлов в поле отображения
            for (int i = 0; i < ArrayFiles.Length; ++i)
            {
                //Отмечаем, что это файл
                TempoPoint.Name = ArrayFiles[i].Name;
                TempoPoint.Type = Global.Types.File;
                TempoPoint.Attribute = "";
                TempoPoint.Size = ArrayFiles[i].Length;
                if (ArrayFiles[i].Attributes.HasFlag(FileAttributes.Hidden))
                {
                    TempoPoint.Attribute = "Hidden";
                }

                CurrentContent.Add(TempoPoint);
            }
        }

        public string GetName(int Index)
        {
            MyFunc.CheckLimitataAream(ref Index, 0, CurrentContent.Count - 1);
            return CurrentContent[Index].Name;
        }
        public string GetAttribute(int Index)
        {
            MyFunc.CheckLimitataAream(ref Index, 0, CurrentContent.Count - 1);
            return CurrentContent[Index].Attribute;
        }
        public Global.Types GetType(int Index)
        {
            MyFunc.CheckLimitataAream(ref Index, 0, CurrentContent.Count - 1);
            return CurrentContent[Index].Type;
        }
        public int GetLength()
        {
            return CurrentContent.Count;
        }
        
    }
}
