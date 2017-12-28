using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model.Exceptions
{
    /// <summary>
    /// Исключение, выбрасываемое, если параметр 
    /// "толщина" задан неправильно.
    /// </summary>
    public class CogwheelWrongDepthException : ApplicationException
    {
        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message"></param>
        public CogwheelWrongDepthException(string message) :base(message)
        {
        }
    }
}
