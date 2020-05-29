using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible
{
  public  class SettingCollection
    {
        public int AUTOLOG { get;} = 10;

        public SettingCollection(int autoLogOut)
        {
            this.AUTOLOG = autoLogOut;
        }
    }
}
