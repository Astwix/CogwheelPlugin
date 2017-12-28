using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое, если параметр 
    /// "внутренний радиус отверстия" задан неправильно.
    /// </summary>
    public class CogwheelWrongHoleRadiusException : ApplicationException
    {
        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message"></param>
        public CogwheelWrongHoleRadiusException(string message) :base(message)
        {
        }
    }
}
