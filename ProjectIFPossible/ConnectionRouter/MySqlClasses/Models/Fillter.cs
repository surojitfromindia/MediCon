using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses.Models
{
    class Fillter
    {
        public Fillter(int fillterLow, int filterHigh, bool isActive)
        {

            if (filterHigh < fillterLow)
            {
                int dummy = fillterLow;
                fillterLow = filterHigh;
                filterHigh = dummy;
            }
            LowValue = fillterLow;
            HighValue = filterHigh;
            IsActive = isActive;
        }

        public bool IsActive { get; }
        public int LowValue { get; }
        public int HighValue { get; }
    }
}
