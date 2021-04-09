using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class FileReader
    {
        public List<string> Data = new List<string>();
        public void LoadFile(string Path001)
        {
            string[] FileContent = null;

            try
            {
                FileContent = File.ReadAllLines(@Path001);
            }
            catch (NullReferenceException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Nothing read");
                Global.AddToLogFile(Path001, "Trying read file", "Nothing read");
                return;
            }
            catch (FileNotFoundException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "File not found");
                Global.AddToLogFile(Path001, "Trying read file", "File not found");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                Global.AddToLogFile(Path001, "Trying read file", "Access Denied");
                return;
            }
            catch(Exception)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not Read File");
                Global.AddToLogFile(Path001, "Trying read file", "Can not Read File");
                return;
            }
            
            Data = new List<string>();
            foreach (var Item in FileContent)
            {
                try
                {
                    Data.Add(Item);
                    //FileContent = File.ReadAllLines(Path001);
                }
                catch (NullReferenceException)
                {
                    Global.Attention.AddContent(MyFunc.Set(2, 1), "Nothing read");
                    Global.AddToLogFile(Path001, "Trying read file", "Nothing read");
                }
                
            }
        }
        public void Clear()
        {            
            Data.Clear();
        }
    }
}
