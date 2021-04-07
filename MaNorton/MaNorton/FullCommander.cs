using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class FullCommander
    {
        public bool Ended = false;

        int CurrentState = 0;

        NortoPart[] Partes = new NortoPart[2];
        bool[] MemActivity = new bool[2];
        MyLib.Clausa1D NumberPart = new Clausa1D();
        string[] Buttons = new string[10];
        Global.Str_Array2D ButtonField;

        ObjArray2D Helper = new ObjArray2D();
        MaShouwer MaShouwar = new MaShouwer();
        ObjArray2D NameEnterer = new ObjArray2D();
        bool SetNewDirectory = false;
        bool SetNewFile = false;

        BaseWindows Confirmer = new BaseWindows();
        BaseWindows DiskSelecter = new BaseWindows();

        public void Init()
        {
            Partes[0] = new NortoPart();
            Partes[1] = new NortoPart();

            Partes[0].Active = true;
            Partes[1].Active = false;
            Partes[0].Enabled = true;
            Partes[1].Enabled = true;

            Partes[0].Init(MyFunc.Set(2, 0), 0);
            Partes[1].Init(MyFunc.Set(Global.Sizes.Primis / 2 + 1, 0), 1);
            NumberPart.SetMinMax(0, 1);
            NumberPart.Set(0);

            MakeButtons();
            MakeHelper();
            MaShouwar.Init(MyFunc.Set(0, 0), Global.Sizes.Secundus, Global.Sizes.Primis);

            MyFunc.Geminus<int> TempoKoords;
            TempoKoords.Primis = MyFunc.AlignString(78, Global.Sizes.Primis, 1);
            TempoKoords.Secundus = MyFunc.AlignString(3, Global.Sizes.Secundus, 1);
            NameEnterer.Init(TempoKoords, 3, 78, MyFunc.Set(0, 0));
            //NameEnterer.Enabled = true;

            Confirmer.Init(0);
            DiskSelecter.Init(1);
            DiskSelecter.SetLabel(" Select The Disk ", 1);
        }

        public void HandleKeys()
        {
            switch (CurrentState)
            {
                case 0:
                    MainKeys();
                    break;
                case 1:
                    if (MaKeys.Get(ConsoleKey.F1))
                    {
                        Helper.Enabled = false;
                        ChangeActivity(1);
                        CurrentState = 0;
                        return;
                    }
                    if (MaKeys.Get(ConsoleKey.Escape))
                    {
                        Helper.Enabled = false;
                        ChangeActivity(1);
                        CurrentState = 0;
                        return;
                    }
                    break;
                case 2:
                    switch (DiskSelecter.HandleKeys())
                    {
                        case -1:
                            //Partes[NumberPart.Get()].Reset();
                            ChangeActivity(1);
                            CurrentState = 0;
                            break;
                        case 0:
                            break;
                        case 1:
                            Partes[NumberPart.Get()].SetNewPath(DiskSelecter.GetNewPath());
                            Partes[NumberPart.Get()].PrepareDirectory();
                            ChangeActivity(1);
                            CurrentState = 0;
                            break;
                    }
                    break;
                case 3:
                    if(!MaShouwar.HandleKeys())
                    {
                        ChangeActivity(1);
                        CurrentState = 0;
                    }
                    break;
                case 4:
                    break;
                case 5:
                    switch (Confirmer.HandleKeys())
                    {
                        case -1:
                            Partes[NumberPart.Get()].Reset();
                            ChangeActivity(1);
                            CurrentState = 0;
                            break;
                        case 0:
                            break;
                        case 1:
                            Copy();
                            ChangeActivity(1);
                            CurrentState = 0;
                            break;
                    }
                    break;
                case 6:
                    if (MakeFile())
                    {
                        Partes[NumberPart.Get()].PrepareDirectory();
                        ChangeActivity(1);
                        CurrentState = 0;
                    }                    
                    break;
                case 7:
                    if (MakeDir())
                    {
                        Partes[NumberPart.Get()].PrepareDirectory();
                        ChangeActivity(1);
                        CurrentState = 0;
                    }
                    break;
                case 8:
                    switch (Confirmer.HandleKeys())
                    {
                        case -1:
                            Partes[NumberPart.Get()].Reset();
                            ChangeActivity(1);
                            CurrentState = 0;
                            break;
                        case 0:
                            break;
                        case 1:
                            Delete();
                            ChangeActivity(1);
                            CurrentState = 0;
                            break;
                    }
                    break;
                case 9:
                    if (!MaShouwar.HandleKeys())
                    {
                        ChangeActivity(1);
                        CurrentState = 0;
                    }
                    break;
                //case 10:
                    //Partes[0].Save();
                    //Partes[1].Save();
                    //Ended = true;
                   //break;
            }
        }
        void MainKeys()
        {
            if (MaKeys.Get(ConsoleKey.Tab))
            {
                Partes[NumberPart.Get()].Reset();
                Partes[0].Active = !Partes[0].Active;
                Partes[1].Active = !Partes[1].Active;
                NumberPart.Do(1, 1);
                return;
            }


            if (MaKeys.Get(ConsoleKey.F1))
            {
                CurrentState = 1;
                Helper.Enabled = true;
                ChangeActivity(0);
                return;
            }
            if (MaKeys.Get(ConsoleKey.F2))
            {
                CurrentState = 2;
                ChangeActivity(0);

                DiskSelecter.Enabled = true;
                return;
            }
            if (MaKeys.Get(ConsoleKey.F3))
            {
                MakeFileReader();
                return;
            }
            if (MaKeys.Get(ConsoleKey.F5))
            {
                CurrentState = 5;
                ChangeActivity(0);

                Confirmer.Enabled = true;
                Confirmer.SetLabel(" Do you Realy want to COPY? ", 1);
                return;
            }
            if (MaKeys.Get(ConsoleKey.F6))
            {
                CurrentState = 6;
                ChangeActivity(0);

                NameEnterer.SetLabel(MyFunc.Set(2, 0), " Enter a name for the new File ");
                SetNewFile = true;
                NameEnterer.Enabled = true;
                MaKeys.ReadText(MyFunc.Set(NameEnterer.GetKoords().Primis + 2, NameEnterer.GetKoords().Secundus + 1), true);
                return;
            }
            if (MaKeys.Get(ConsoleKey.F7))
            {
                CurrentState = 7;
                ChangeActivity(0);

                NameEnterer.SetLabel(MyFunc.Set(2, 0), " Enter a name for the new Directory ");
                SetNewDirectory = true;
                NameEnterer.Enabled = true;
                MaKeys.ReadText(MyFunc.Set(NameEnterer.GetKoords().Primis + 2, NameEnterer.GetKoords().Secundus + 1), true);
            }
            if (MaKeys.Get(ConsoleKey.F8))
            {
                CurrentState = 8;
                ChangeActivity(0);
                
                Confirmer.Enabled = true;
                Confirmer.SetLabel(" Do you Realy want to DELETE? ", 1);
                return;
            }
            if (MaKeys.Get(ConsoleKey.F9))
            {
                MakeTreeViewer();
                return;
            }
            if (MaKeys.Get(ConsoleKey.F10))
            {
                Partes[0].Save();
                Partes[1].Save();
                Ended = true;
                //CurrentState = 10;
                //ChangeActivity(0);
                return;
            }

            Partes[0].HandleKeys();
            Partes[1].HandleKeys();
        }

        public void Show()
        {
            Partes[0].Show();
            Partes[1].Show();

            MyFunc.Copy(ref ButtonField.Koords, ButtonField.Array, Global.Field);

            Global.Attention.Show();

            Helper.Show();
            MaShouwar.Show();

            NameEnterer.Show();

            Confirmer.Show();
            DiskSelecter.Show();
        }

        void ChangeActivity(int Number001)
        {
            //Запоминаем или восстанавливаем активность
            switch (Number001)
            {
                case 0:
                    MemActivity[0] = Partes[0].Active;
                    MemActivity[1] = Partes[1].Active;

                    Partes[0].Active = false;
                    Partes[1].Active = false;
                    break;
                case 1:
                    Partes[0].Active = MemActivity[0];
                    Partes[1].Active = MemActivity[1];
                    break;
            }
        }

        void MakeButtons()
        {
            Buttons[0] = "F1 Help";
            Buttons[1] = "F2 Disk";
            Buttons[2] = "F3 Read";
            Buttons[3] = "F4";
            Buttons[4] = "F5 Copy";
            Buttons[5] = "F6 Make File";
            Buttons[6] = "F7 Make Dir";
            Buttons[7] = "F8 Delete";
            Buttons[8] = "F9 Tree";
            Buttons[9] = "F10 Exit";

            ButtonField.Array = new char[3, Global.Sizes.Primis - 4];
            ButtonField.Koords = MyFunc.Set(2, Global.Sizes.Secundus - 4);

            int Step = ButtonField.Array.GetLength(1) / 10;
            int ToCenter;

            MyFunc.Geminus<int> TempoKoords = MyFunc.Set(2, 1);
            for (int i = 0; i < 10; ++i)
            {
                ToCenter = (Step - Buttons[i].Length) / 2;
                TempoKoords.Primis = 2 + i * Step + ToCenter;

                MyFunc.CopyStringToArray(TempoKoords, Buttons[i], ButtonField.Array, true);
            }

            Global.FillBroders(ButtonField.Array);
            MyFunc.CopyStringToArray(MyFunc.Set(2, 0), " Buttons ", ButtonField.Array, true);
            MyFunc.Copy(ref ButtonField.Koords, ButtonField.Array, Global.Field);
        }
        void MakeHelper()
        {
            List<string> HelpContent = new List<string>();
            HelpContent.Add("Up and Down Arrows, PageUp and PageDown, Home and End - To navigate in current Directory");
            HelpContent.Add("BackSpace - To Up Level Directory");
            HelpContent.Add("SpaceBar - Select files or directories");
            HelpContent.Add("Escape - Close Helper Or Cancel Selected Files");
            HelpContent.Add("F1 - Open Helper");
            HelpContent.Add("F2 - Change Disk in current part of MaNorto");
            HelpContent.Add("F3 - Open some file in Reader");
            HelpContent.Add("F4");
            HelpContent.Add("F5 - Copy selected files or directories from Current Part to Another");
            HelpContent.Add("F6 - Make file");
            HelpContent.Add("F7 - Make Directory");
            HelpContent.Add("F8 - Delete selected files or directories");
            HelpContent.Add("F9 - Show Tree current active directoy");
            HelpContent.Add("F10 - Exit");

            MyFunc.Geminus<int> TempoKoords = MyFunc.Set((Global.Sizes.Primis - HelpContent[0].Length) / 2, (Global.Sizes.Secundus - HelpContent.Count) / 8);
            Helper.Init(TempoKoords, HelpContent.Count + 2, HelpContent[0].Length + 2, MyFunc.Set(1, 1));
            Helper.SetLabel(MyFunc.Set(2, 0), " Helper ");
            Helper.AddContent(MyFunc.Set(1, 1), ref HelpContent);
        }
        void MakeFileReader()
        {
            //Если маркер находится на файле
            if (Partes[NumberPart.Get()].GetType() == Global.Types.File)
            {
                //MaShouwar.Init(MyFunc.Set(0, 0), Global.Sizes.Secundus, Global.Sizes.Primis);
                //Грузим данные
                FileReader NewReader = new FileReader();
                NewReader.LoadFile(Partes[NumberPart.Get()].GetFullPath());
                if (NewReader.Data != null)
                {
                    //Записываем
                    MaShouwar.SetLabel(Partes[NumberPart.Get()].GetName());
                    MaShouwar.SetData(ref NewReader.Data);
                    MaShouwar.Enabled = true;

                    CurrentState = 3;
                    ChangeActivity(0);
                }
            }
        }
        void MakeTreeViewer()
        {
            if(Partes[NumberPart.Get()].GetAttribute() == "Hidden")
            {
                return;
            }
            //Если маркер находится на папке
            if (Partes[NumberPart.Get()].GetType() == Global.Types.Directory)
            {
                TreeMaker NewTree001 = new TreeMaker();
                NewTree001.MakeTree(Partes[NumberPart.Get()].GetFullPath());
                MaShouwar.SetLabel(Partes[NumberPart.Get()].GetFullPath());
                MaShouwar.SetData(ref NewTree001.CurrentTree);
                MaShouwar.Enabled = true;

                CurrentState = 9;
                ChangeActivity(0);
                return;
            }
            if (Partes[NumberPart.Get()].GetType() == Global.Types.LevelUp)
            {
                TreeMaker NewTree001 = new TreeMaker();
                NewTree001.MakeTree(Partes[NumberPart.Get()].GetCurrentPath());
                MaShouwar.SetLabel(Partes[NumberPart.Get()].GetCurrentPath());
                MaShouwar.SetData(ref NewTree001.CurrentTree);
                MaShouwar.Enabled = true;

                CurrentState = 9;
                ChangeActivity(0);
                return;
            }
        }

        bool MakeFile()
        {
            if (SetNewFile)
            {
                string Tempo = Partes[NumberPart.Get()].GetCurrentPath();
                Tempo += '\\' + MaKeys.GetText();

                try
                {
                    FileStream NewFile = File.Create(@Tempo);
                    Global.AddToLogFile(@Tempo, "Maked File", "Success");
                    Global.Attention.AddContent(MyFunc.Set(2, 1), $"Successfully Maked File {@Tempo}");
                }
                catch (UnauthorizedAccessException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                    Global.AddToLogFile(@Tempo, "Trying to make File", "Access Denied");
                    return false;
                }
                catch (Exception)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not make File");
                    Global.AddToLogFile(@Tempo, "Trying to make File", "Can not make File");
                    return false;
                }

                SetNewFile = false;
                NameEnterer.Enabled = false;
                return true;
            }
            return false;
        }

        bool MakeDir()
        {
            if (SetNewDirectory)
            {
                string Tempo = Partes[NumberPart.Get()].GetCurrentPath();
                Tempo += '\\' + MaKeys.GetText();

                try
                {
                    DirectoryInfo NewDir = Directory.CreateDirectory(@Tempo);
                    Global.AddToLogFile(@Tempo, "Maked Directory", "Success");
                    Global.Attention.AddContent(MyFunc.Set(2, 1), $"Successfully Maked Directory {@Tempo}");
                }
                catch (UnauthorizedAccessException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                    Global.AddToLogFile(@Tempo, "Trying to make Directory", "Access Denied");
                    return false;
                }
                catch (Exception)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not make Directory");
                    Global.AddToLogFile(@Tempo, "Trying to make Directory", "Can not make Directory");
                    return false;
                }

                SetNewDirectory = false;
                NameEnterer.Enabled = false;
                return true;
            }
            return false;
        }
        void Delete()
        {
            List<string> TempoList = Partes[NumberPart.Get()].GetFullNames();
            if (TempoList.Count > 0)
            {
                List<Global.Types> TempoTypes = Partes[NumberPart.Get()].GetTypes();
                for (int i = 0; i < TempoList.Count; ++i)
                {
                    DeleteUnit(TempoList[i], TempoTypes[i]);
                }
                Partes[NumberPart.Get()].PrepareDirectory();
                Partes[NumberPart.Get()].Reset();
                return;
            }

            DeleteUnit(Partes[NumberPart.Get()].GetFullPath(), Partes[NumberPart.Get()].GetType());
            Partes[NumberPart.Get()].PrepareDirectory();
            Partes[NumberPart.Get()].Reset();
        }
        void DeleteUnit(string CurrentFullName, Global.Types CurrentType)
        {
            if (CurrentType == Global.Types.File)
            {
                try
                {
                    File.Delete(CurrentFullName);
                    Global.AddToLogFile(CurrentFullName, "Deleted File", "Success");
                    Global.Attention.AddContent(MyFunc.Set(2, 1), $"Successfully Deleted File {CurrentFullName}");
                }
                catch (UnauthorizedAccessException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                    Global.AddToLogFile(CurrentFullName, "Trying to Delete File", "Access Denied");
                }
                catch (Exception)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not Delete File");
                    Global.AddToLogFile(CurrentFullName, "Trying to make Delete File", "Can not make Delete File");
                }
                return;
            }
            if (CurrentType == Global.Types.Directory)
            {
                try
                {
                    Directory.Delete(CurrentFullName, true);
                    Global.AddToLogFile(CurrentFullName, "Deleted Directory", "Success");
                    Global.Attention.AddContent(MyFunc.Set(2, 1), $"Successfully Deleted Directory {CurrentFullName}");
                }
                catch (UnauthorizedAccessException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                    Global.AddToLogFile(CurrentFullName, "Trying to Delete Directory", "Access Denied");
                }
                catch (Exception)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not Delete Directory");
                    Global.AddToLogFile(CurrentFullName, "Trying to make Delete Directory", "Can not make Delete Directory");
                }
                return;
            }
        }
        void Copy()
        {
            MyLib.Clausa1D TempoSwitcher = new Clausa1D();
            TempoSwitcher.SetMinMax(0, 1);
            TempoSwitcher.Set(NumberPart.Get());
            TempoSwitcher.Do(1, 0);

            List<string> Names = Partes[NumberPart.Get()].GetNames();
            string From = Partes[NumberPart.Get()].GetCurrentPath();
            string To = Partes[TempoSwitcher.Get()].GetCurrentPath();

            if (Names.Count > 0)
            {
                List<Global.Types> FromTypes = Partes[NumberPart.Get()].GetTypes();
                for (int i = 0; i < Names.Count; ++i)
                {
                    DoCopy(From, To, Names[i], FromTypes[i]);
                }

                Partes[NumberPart.Get()].PrepareDirectory();
                Partes[NumberPart.Get()].Reset();
                Partes[TempoSwitcher.Get()].PrepareDirectory();
                return;
            }

            Global.Types CurrentType = Partes[NumberPart.Get()].GetType();
            string CurrentName = Partes[NumberPart.Get()].GetName();
            DoCopy(From, To, CurrentName, CurrentType);

            Partes[NumberPart.Get()].PrepareDirectory();
            Partes[NumberPart.Get()].Reset();
            Partes[TempoSwitcher.Get()].PrepareDirectory();
        }
        void DoCopy(string FromPath, string ToPath, string Name, Global.Types CurrentType)
        {
            string SourcePath = Path.Combine(FromPath, Name);
            string DestinationPath = Path.Combine(ToPath, Name);

            if (CurrentType == Global.Types.File)
            {
                try
                {
                    File.Copy(SourcePath, DestinationPath, true);
                    Global.AddToLogFile($"From {FromPath} To {ToPath}", $"Copied File {Name}", "Success");
                    Global.Attention.AddContent(MyFunc.Set(2, 1), $"Successfully Copied File {SourcePath} To {ToPath}");
                }
                catch (UnauthorizedAccessException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                    Global.AddToLogFile(SourcePath, "Trying to Copy File", "Access Denied");
                }
                catch (Exception)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not Copy File");
                    Global.AddToLogFile(SourcePath, "Trying to Copy File", "Can not Copy File");
                }
                return;
            }
            if (CurrentType == Global.Types.Directory)
            {
                try
                {
                    CopyDirectory(FromPath, ToPath, Name);
                    Global.AddToLogFile($"From {FromPath} To {ToPath}", $"Copied Directory {Name}", "Success");
                    Global.Attention.AddContent(MyFunc.Set(2, 1), $"Successfully Copied Directory From {SourcePath} To {ToPath}");
                }
                catch (UnauthorizedAccessException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                    Global.AddToLogFile(SourcePath, "Trying to Copy Directory", "Access Denied");
                }
                catch (Exception)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not Copy Directory");
                    Global.AddToLogFile(SourcePath, "Trying to Copy Directory", "Can not Copy Directory");
                }
                return;
            }
        }
        void CopyDirectory(string FromPath, string ToPath, string Name)
        {
            Directory.CreateDirectory(Path.Combine(ToPath, Name));

            DirectoryInfo FromDir = new DirectoryInfo(Path.Combine(FromPath, Name));
            DirectoryInfo ToDir = new DirectoryInfo(Path.Combine(ToPath, Name));

            FileInfo[] CurrentFiles = FromDir.GetFiles();
            foreach (FileInfo File in CurrentFiles)
            {
                File.CopyTo(Path.Combine(ToDir.FullName, File.Name));
            }

            DirectoryInfo[] CurrentDirs = FromDir.GetDirectories();
            foreach (DirectoryInfo Directory in CurrentDirs)
            {
                CopyDirectory(FromDir.FullName, ToDir.FullName, Directory.Name);
            }
        }
    }
}