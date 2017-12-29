using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое, если параметр "число зубцов" задан неправильно.
    /// </summary>
    public class CogwheelWrongCogsException : ApplicationException
    {
        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message"></param>
        public CogwheelWrongCogsException(string message) : base(message)
        {
        }
    }
}
