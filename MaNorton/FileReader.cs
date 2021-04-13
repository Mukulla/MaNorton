﻿using System;
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
        static public string[] Data;
        static public bool LoadFile(string Path001)
        {
            Data = null;

            try
            {
                Data = File.ReadAllLines(@Path001);
                return true;
            }
            catch (NullReferenceException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Nothing read");
                Global.AddToLogFile(Path001, "Trying read file", "Nothing read");
                return false;
            }
            catch (FileNotFoundException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "File not found");
                Global.AddToLogFile(Path001, "Trying read file", "File not found");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Access Denied");
                Global.AddToLogFile(Path001, "Trying read file", "Access Denied");
                return false;
            }
            catch(Exception)
            {
                Global.Attention.AddContent(MyFunc.Set(2, 1), "Can not Read File");
                Global.AddToLogFile(Path001, "Trying read file", "Can not Read File");
                return false;
            }            
        }
    }
}
