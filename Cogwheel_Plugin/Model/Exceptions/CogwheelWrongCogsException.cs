using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogwheel_Plugin.Model.Exceptions
{
    public class CogwheelWrongCogsException:ApplicationException
    {
        public CogwheelWrongCogsException(string message) :base(message)
        {
        }
    }
}
