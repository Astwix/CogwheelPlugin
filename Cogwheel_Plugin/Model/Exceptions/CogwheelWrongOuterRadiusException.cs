using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogwheel_Plugin.Model.Exceptions
{
    class CogwheelWrongOuterRadiusException : ApplicationException
    {
        public CogwheelWrongOuterRadiusException(string message) :base(message)
        {
        }
    }
}
