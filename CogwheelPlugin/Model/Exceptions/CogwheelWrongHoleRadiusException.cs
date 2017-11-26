using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    public class CogwheelWrongHoleRadiusException : ApplicationException
    {
        public CogwheelWrongHoleRadiusException(string message) :base(message)
        {
        }
    }
}
