using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter
{
    class AccessConnection : DataBaseConnectionBase
    {
        public override void PrinLn()
        {
            Console.WriteLine("Access");
        }
    }
}
