using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class ObjArray1D
    {
        char[] SomeArray;
        MyFunc.Geminus<int> Koords;

        public void Init(MyFunc.Geminus<int> Koords001, string Name001, int NewSize001)
        {
            if(NewSize001 < 1)
            {
                NewSize001 = 1;
            }
            SomeArray = new char[NewSize001];
            MyFunc.FillArray(SomeArray, ' ');

            Koords = Koords001;

            MyFunc.CopyStringToArray(0, Name001, SomeArray);
        }
        public void NewName(string NewName001)
        {
            MyFunc.FillArray(SomeArray, ' ');
            MyFunc.CopyStringToArray(0, NewName001, SomeArray);
        }
        public void Clear()
        {
            MyFunc.FillArray(SomeArray, ' ');
        }

        public void Show()
        {
            MyFunc.Copy(ref Koords, SomeArray, Global.Field, true);
        }
    }
}
