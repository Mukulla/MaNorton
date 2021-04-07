using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class PathWalker
    {
        string CurrentPath = "";
        string FilePath = "";
        int CurrentPart = 0;

        string Attention = "";

        public void Init(string FilePath001, int NumberOfPart)
        {
            FilePath = FilePath001;
            CurrentPart = NumberOfPart;

            string[] FileContent;

            if (File.Exists(FilePath001))
            {
                FileContent = File.ReadAllLines(FilePath001);

                if (FileContent.Length != 2 || !CheckDirectories(FileContent))
                {
                    MakeNewFile();
                    return;
                }
                CurrentPath = FileContent[CurrentPart];
            }
            else
            {
                MakeNewFile();
            }
        }
        public void SetNewPath(string NewPath001)
        {
            if (PathTryier(NewPath001))
            {
                CurrentPath = NewPath001;
            }
        }
        void MakeNewFile()
        {
            DriveInfo[] Drives = DriveInfo.GetDrives();
            File.WriteAllText(FilePath, Drives[0].Name + Environment.NewLine + Drives[0].Name);

            CurrentPath = Drives[0].Name;
        }
        bool CheckDirectories(string[] ArrayOfPaths001)
        {
            foreach (var Item in ArrayOfPaths001)
            {
                if (!Directory.Exists(Item))
                {
                    return false;
                }
            }
            return true;
        }
        public void SavePath(string SomePath001)
        {
            switch (CurrentPart)
            {
                case 0:
                    File.WriteAllText(SomePath001, CurrentPath + Environment.NewLine);
                    break;
                case 1:
                    File.AppendAllText(SomePath001, CurrentPath);
                    break;
            }
        }
        public void LevelUp()
        {
            DirectoryInfo UpperDirectory = Directory.GetParent(CurrentPath);
            if (UpperDirectory != null)
            {
                CurrentPath = UpperDirectory.FullName;
            }
        }

        public bool LevelDown(string NameDir)
        {
            string NewPath = Path.Combine(CurrentPath, NameDir);

            if (PathTryier(NewPath))
            {
                CurrentPath = NewPath;
                return true;
            }
            return false;
        }

        public string GetPath()
        {
            return CurrentPath;
        }
        public string GetAtteintion()
        {
            return Attention;
        }

        bool PathTryier(string SomePath001)
        {
            try
            {
                string[] TryDir = Directory.GetDirectories(SomePath001);
            }
            catch (UnauthorizedAccessException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                Global.AddToLogFile(SomePath001, "Trying to open a directory", "Access Denied");
                return false;
            }
            catch (Exception)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not open Directory");
                Global.AddToLogFile(SomePath001, "Trying to open a directory", "Can not open Directory");
                return false;
            }
            return true;
        }
    }
}
