using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyLib;

namespace MaNorton
{
    class Scrollar
    {
        MyLib.Stricto BorderMin = new Stricto();
        MyLib.Stricto BorderMax = new Stricto();

        int Step = 0;

        public void Init(int LowerLimit001, int UpperLimit001, int VisibleHeight001, int Step001)
        {
            if (UpperLimit001 <= VisibleHeight001)
            {
                BorderMin.SetMinMax(0, 0);
                BorderMax.SetMinMax(UpperLimit001, UpperLimit001);
            }
            else
            {
                BorderMin.SetMinMax(0, UpperLimit001 - VisibleHeight001);
                BorderMax.SetMinMax(VisibleHeight001, UpperLimit001);               
            }
            //BorderMin.SetMinMax(0, UpperLimit001 - VisibleHeight001);
            //BorderMax.SetMinMax(UpperLimit001, VisibleHeight001);

            Step = Step001;
        }

        public void OffSet(int Direction001)
        {
            switch (Direction001)
            {
                //Home
                case -3:
                    BorderMin.Minimalize();
                    BorderMax.Minimalize();
                    break;
                //PageUp
                case -2:
                    BorderMin.Do(Step, 1);
                    BorderMax.Do(Step, 1);
                    break;
                //Сместить вверх
                case -1:
                    BorderMin.Do(1, 1);
                    BorderMax.Do(1, 1);
                    break;
                //Сместить вниз
                case 1:
                    BorderMin.Do(1, 0);
                    BorderMax.Do(1, 0);
                    break;
                //PageDown
                case 2:
                    BorderMin.Do(Step, 0);
                    BorderMax.Do(Step, 0);
                    break;
                //End
                case 3:
                    BorderMin.Maximalize();
                    BorderMax.Maximalize();
                    break;
            }           
        }
        public int GetStep()
        {
            return Step;
        }
        public int GetMin()
        {
            return BorderMin.Get();
        }
        public int GetMax()
        {
            return BorderMax.Get();
        }
    }
}
