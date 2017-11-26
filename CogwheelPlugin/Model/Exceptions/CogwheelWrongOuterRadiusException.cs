using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    public class CogwheelWrongOuterRadiusException : ApplicationException
    {
        public CogwheelWrongOuterRadiusException(string message) :base(message)
        {
        }
    }
}
