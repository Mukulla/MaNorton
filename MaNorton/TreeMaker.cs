using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    //Создание дерева каталогов, рекурсивное
    class TreeMaker
    {
        //Список с деревом
        static public List<string> CurrentTree = new List<string>();
        static string Spacer = "   ";
        static string TopDownRight = "├──";
        static string TopDown = "│  ";
        static string DownRight = "└──";
        static public void MakeTree(string SomePath001)
        {
            try
            {
                string[] TryDir = Directory.GetDirectories(SomePath001);
            }
            catch (UnauthorizedAccessException)
            {
                //Global.Attention.Show("Access Denied");
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                Global.AddToLogFile(SomePath001, "Trying to make Tree", "Access Denied");
                return;
            }
            catch(Exception)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not make Tree");
                Global.AddToLogFile(SomePath001, "Trying to make Tree", "Can not make Tree");
                return;
            }

            try
            {
                CurrentTree.Clear();
                ShowDirRec(SomePath001, "");
            }
            catch (UnauthorizedAccessException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                Global.AddToLogFile(SomePath001, "Trying to make Tree", "Access Denied");
                return;
            }
            catch (StackOverflowException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Reached Limit Folders");
                Global.AddToLogFile(SomePath001, "Trying to make Tree", "Reached Limit Folders");
                return;
            }
            catch(Exception)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not make Tree");
                Global.AddToLogFile(SomePath001, "Trying to make Tree", "Can not make Tree");
                return;
            }            
        }

        static void ShowDirRec(string SomePath, string LastShifter)
        {
            string Shifter;
            DirectoryInfo CurrentDirectory = new DirectoryInfo(SomePath);
            DirectoryInfo[] CurrentArrayDir = CurrentDirectory.GetDirectories();
            FileInfo[] ArrayFiles = CurrentDirectory.GetFiles();

            for (int i = 0; i < CurrentArrayDir.Length; ++i)
            {
                DirectoryInfo LevelDownDir = new DirectoryInfo(CurrentArrayDir[i].FullName);
                DirectoryInfo[] LevelDownArrayDir = LevelDownDir.GetDirectories();
                FileInfo[] LevelDownArrayFiles = LevelDownDir.GetFiles();

                if ( i == CurrentArrayDir.Length - 1 )
                {
                    if(ArrayFiles.Length > 1)
                    {
                        Shifter = LastShifter + TopDownRight;
                        CurrentTree.Add(Shifter + CurrentArrayDir[i].Name);
                        Shifter = LastShifter + TopDown;
                        ShowDirRec(CurrentArrayDir[i].FullName, Shifter);
                        continue;
                    }
                    Shifter = LastShifter + DownRight;
                    CurrentTree.Add(Shifter + CurrentArrayDir[i].Name);
                    Shifter = LastShifter + Spacer;
                    ShowDirRec(CurrentArrayDir[i].FullName, Shifter);
                    continue;
                }
                if( i < CurrentArrayDir.Length)
                {
                    
                    if(LevelDownArrayDir.Length > 0 || LevelDownArrayFiles.Length > 0)
                    {
                        Shifter = LastShifter + TopDownRight;
                        CurrentTree.Add(Shifter + CurrentArrayDir[i].Name);
                        Shifter = LastShifter + TopDown;
                        ShowDirRec(CurrentArrayDir[i].FullName, Shifter);
                        continue;
                    }

                    Shifter = LastShifter + TopDownRight;
                    CurrentTree.Add(Shifter + CurrentArrayDir[i].Name);
                    Shifter = LastShifter + TopDown;
                    ShowDirRec(CurrentArrayDir[i].FullName, Shifter);
                    continue;
                }
                if (i == CurrentArrayDir.Length - 1 && ArrayFiles.Length > 0)
                {
                    Shifter = LastShifter + TopDownRight;
                    CurrentTree.Add(Shifter + CurrentArrayDir[i].Name);
                    Shifter = LastShifter + TopDown;
                    ShowDirRec(CurrentArrayDir[i].FullName, Shifter);
                    continue;
                }
            }

            foreach (var Item in ArrayFiles)
            {
                if (Item == ArrayFiles.Last())
                {
                    Shifter = LastShifter + DownRight;
                    CurrentTree.Add(Shifter + Item.Name);
                }
                else
                {
                    Shifter = LastShifter + TopDownRight;
                    CurrentTree.Add(Shifter + Item.Name);
                }
            }
        }
    }
}
