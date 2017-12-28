using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое, если параметр 
    /// "внешний радиус" задан неправильно.
    /// </summary>
    public class CogwheelWrongOuterRadiusException : ApplicationException
    {
        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message"></param>
        public CogwheelWrongOuterRadiusException(string message) :base(message)
        {
        }
    }
}
