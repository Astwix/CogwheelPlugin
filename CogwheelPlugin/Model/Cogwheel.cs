using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogwheelPlugin.Model
{
    public class Cogwheel
    {
        /// <summary>
        /// Внутренний радиус
        /// </summary>
        private double _innerRadius;

        /// <summary>
        /// Внешний радиус
        /// </summary>
        private double _outerRadius;

        /// <summary>
        /// Радиус прорези
        /// </summary>
        private double _holeRadius;

        /// <summary>
        /// Толщина шестеренки
        /// </summary>
        private double _depth;

        /// <summary>
        /// Количество зубцов
        /// </summary>
        private int _cogs;

        /// <summary>
        /// Тип выреза
        /// </summary>
        private ExtrudeType _typeOfExtrude;

        /// <summary>
        /// Количество вырезов
        /// </summary>
        private int _extrudeCount;

        /// <summary>
        /// Конструктор класса детали для пяти параметров
        /// </summary>
        /// <param name="innerRadius"></param>
        /// <param name="outerRadius"></param>
        /// <param name="holeRadius"></param>
        /// <param name="depth"></param>
        /// <param name="cogs"></param>
        public Cogwheel(double innerRadius, double outerRadius,
            double holeRadius, double depth, int cogs)
        {
            OuterRadius = outerRadius;
            InnerRadius = innerRadius;
            HoleRadius = holeRadius;
            Depth = depth;
            Cogs = cogs;
        }

        /// <summary>
        /// Конструктор класса детали для семи параметров
        /// </summary>
        /// <param name="innerRadius"></param>
        /// <param name="outerRadius"></param>
        /// <param name="holeRadius"></param>
        /// <param name="depth"></param>
        /// <param name="cogs"></param>
        /// <param name="extrudeType"></param>
        /// <param name="extrudeCount"></param>
        public Cogwheel(double innerRadius, double outerRadius,
            double holeRadius, double depth, int cogs,
            ExtrudeType extrudeType, int extrudeCount)
        {
            OuterRadius = outerRadius;
            InnerRadius = innerRadius;
            HoleRadius = holeRadius;
            Depth = depth;
            Cogs = cogs;
            TypeOfExtrude = extrudeType;
            ExtrudeCount = extrudeCount;
        }

        /// <summary>
        /// Свойство количества вырезов
        /// </summary>
        public int ExtrudeCount
        {
            get 
            { 
                return _extrudeCount; 
            }

            private set 
            {
                if (this.TypeOfExtrude == ExtrudeType.Classic)
                {
                    _extrudeCount = value;
                    return;
                }
                if (value > 7 && this.TypeOfExtrude == ExtrudeType.Circles)
                { 
                    throw new Exceptions.CogwheelWrongExtrudeCountException(
                        "Количество вырезов типа 'Круги' не может быть больше 7"); 
                }
                if ((value < 3)) 
                { 
                    throw new Exceptions.CogwheelWrongExtrudeCountException(
                        "Количество вырезов не может быть меньше 3."); 
                }
                if ((value > 30)) 
                { throw new Exceptions.CogwheelWrongExtrudeCountException(
                    "Количество вырезов не может быть больше 30."); 
                }
                _extrudeCount = value; 
            }
        }

        /// <summary>
        /// Свойство типа выреза
        /// </summary>
        public ExtrudeType TypeOfExtrude
        {
            get 
            {
                return _typeOfExtrude; 
            }

            private set 
            {
                _typeOfExtrude = value; 
            }
        }

        /// <summary>
        /// Внутренний радиус
        /// </summary>
        public double InnerRadius
        {
            get 
            {
                return _innerRadius;
            }

            private set
            {
                if (!IsValidDouble(value)) 
                { 
                    throw new Exceptions.CogwheelWrongInnerRadiusException(
                        "Заданный внутренний радиус - не вещественное число."); 
                }
                if (!(value >= 1)) 
                { 
                    throw new Exceptions.CogwheelWrongInnerRadiusException(
                        "Внутренний радиус не может быть меньше 1 мм."); 
                }
                if (!(value <= 95)) 
                { 
                    throw new Exceptions.CogwheelWrongInnerRadiusException(
                        "Внутренний радиус не может быть больше 95 мм."); 
                }
                if (!(value > HoleRadius)) 
                { 
                    throw new Exceptions.CogwheelWrongInnerRadiusException(
                        "Внутренний радиус не может быть меньше или равен радиусу" 
                        + "внутреннего отверстия."); 
                }
                if (!(value < OuterRadius)) 
                { 
                    throw new Exceptions.CogwheelWrongInnerRadiusException
                        ("Внутренний радиус не может быть больше или равен" 
                        + "внешнему радиусу."); 
                }
                _innerRadius = value;
            }
        }

        /// <summary>
        /// Внешний радиус
        /// </summary>
        public double OuterRadius
        {
            get 
            {
                return _outerRadius;
            }

            private set
            {
                if (!IsValidDouble(value)) 
                { 
                    throw new Exceptions.CogwheelWrongOuterRadiusException(
                        "Заданный внешний радиус - не вещественное число."); 
                }
                if (!(value >= 1.5)) 
                { 
                    throw new Exceptions.CogwheelWrongOuterRadiusException(
                        "Внешний радиус не может быть меньше 1,5 мм."); 
                }
                if (!(value <= 100)) 
                { throw new Exceptions.CogwheelWrongOuterRadiusException
                    ("Внешний радиус не может быть больше 100 мм."); 
                }
                if (!(value > InnerRadius)) 
                { throw new Exceptions.CogwheelWrongOuterRadiusException(
                    "Внешний радиус не может быть меньше или равен внутреннему радиусу."); 
                }
                _outerRadius = value;
            }
        }

        /// <summary>
        /// Радиус внутреннего отверстия
        /// </summary>
        public double HoleRadius
        {
            get 
            {
                return _holeRadius;
            }

            private set
            {
                if (!IsValidDouble(value)) 
                { 
                    throw new Exceptions.CogwheelWrongHoleRadiusException(
                    "Заданный радиус внутреннего отверстия - не вещественное число."); 
                }
                if (!(value >= 0.5)) 
                { 
                    throw new Exceptions.CogwheelWrongHoleRadiusException(
                        "Радиус внутреннего отверстия не может быть меньше 0,5 мм."); 
                }
                if (!(value <= 30)) 
                { 
                    throw new Exceptions.CogwheelWrongHoleRadiusException(
                        "Радиус внутреннего отверстия не может быть больше 30 мм."); 
                }
                if (!(value > 0)) 
                { 
                    throw new Exceptions.CogwheelWrongHoleRadiusException(
                        "Радиус внутреннего отверстия не может" +
                        " быть меньше или равен нулю."); 
                }
                if (!(value < InnerRadius)) 
                { 
                    throw new Exceptions.CogwheelWrongHoleRadiusException(
                        "Радиус внутреннего отверстия не может быть больше" 
                        + "или равен внутреннему радиусу."); 
                }
                _holeRadius = value;
            }
        }

        /// <summary>
        /// Толщина
        /// </summary>
        public double Depth
        {
            get 
            {
                return _depth;
            }

            private set
            {
                if (!IsValidDouble(value)) 
                { 
                    throw new Exceptions.CogwheelWrongDepthException(
                    "Заданная толщина - не вещественное число."); 
                }
                if (!(value >= 0.5)) 
                { 
                    throw new Exceptions.CogwheelWrongDepthException(
                    "Толщина не может быть меньше 0.5 мм."); 
                }
                if (!(value <= 70)) 
                { 
                    throw new Exceptions.CogwheelWrongDepthException(
                    "Толщина не может быть больше 70 мм."); 
                }
                if (!(value <= OuterRadius)) 
                { 
                    throw new Exceptions.CogwheelWrongDepthException(
                    "Толщина не может быть больше или равна внешнему радиусу."); 
                }
                _depth = value;
            }
        }

        /// <summary>
        /// Количество зубцов
        /// </summary>
        public int Cogs
        {
            get 
            {
                return _cogs;
            }

            private set
            {
                if (!IsValidInt(value)) 
                { 
                    throw new Exceptions.CogwheelWrongCogsException(
                        "Заданное число зубцов - не вещественное число."); 
                }
                if (!(value >= 5)) 
                { 
                    throw new Exceptions.CogwheelWrongCogsException(
                        "Число зубцов не может быть меньше 5."); 
                }
                if (!(value <= 30)) 
                { 
                    throw new Exceptions.CogwheelWrongCogsException(
                        "Число зубцов не может быть больше 30."); 
                }
                _cogs = value;
            }
        }

        /// <summary>
        /// Проверка на double константы
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsValidDouble(double value)
        {
            if (value < Double.MinValue
                || value > Double.MaxValue
                || Double.IsInfinity(value)
                || Double.IsNaN(value)
            )
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверка на int константы
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsValidInt(Int32 value)
        {
            if (value < Int32.MinValue
                || value > Int32.MaxValue)
            {
                return false;
            }
            return true;
        }
    }
}
