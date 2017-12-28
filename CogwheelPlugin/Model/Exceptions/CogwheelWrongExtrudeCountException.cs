using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое, если параметр 
    /// "количество вырезов" задан неправильно.
    /// </summary>
    public class CogwheelWrongExtrudeCountException : ApplicationException
    {
        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message"></param>
        public CogwheelWrongExtrudeCountException(string message) : base(message)
        {
        }
    }
}
