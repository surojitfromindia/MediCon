using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
    class NonRegisterNameException : Exception
    {
        public override string StackTrace => 
            "No Such Record Found in New Medicine Entry Table or Medicine is Zero (0)";
    }
}
