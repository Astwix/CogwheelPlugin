using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogwheel_Plugin.Model.Exceptions
{
    class CogwheelWrongInnerRadiusException : ApplicationException
    {
        public CogwheelWrongInnerRadiusException(string message) :base(message)
        {
        }
    }
}
