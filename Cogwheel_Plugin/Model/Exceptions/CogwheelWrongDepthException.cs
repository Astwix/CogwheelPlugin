using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogwheel_Plugin.Model.Exceptions
{
    public class CogwheelWrongDepthException : ApplicationException
    {
        public CogwheelWrongDepthException(string message) :base(message)
        {
        }
    }
}
