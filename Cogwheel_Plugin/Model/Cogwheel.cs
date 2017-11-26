using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogwheel_Plugin.Model
{
    public class Cogwheel
    {
        private double _innerRadius; // внутренний радиус
        private double _outerRadius; // внешний радиус
        private double _holeRadius; //радиус прореизи 
        private double _depth; //толщина шестеренки
        private int _cogs; //количество зубцов

        public Cogwheel(double innerRadius, double outerRadius, double holeRadius, double depth, int cogs)
        {
            OuterRadius = outerRadius;
            InnerRadius = innerRadius;
            HoleRadius = holeRadius;
            Depth = depth;
            Cogs = cogs;
        }

        public double InnerRadius
        {
            get { return _innerRadius;}
            private set
            {
                if (!IsValidDouble(value)) { throw new Exceptions.CogwheelWrongInnerRadiusException("Заданный внутренний радиус - не вещественное число."); }
                if (!(value >= 1)) { throw new Exceptions.CogwheelWrongInnerRadiusException("Внутренний радиус не может быть меньше 1 мм."); }
                if (!(value <= 95)) { throw new Exceptions.CogwheelWrongInnerRadiusException("Внутренний радиус не может быть больше 95 мм."); }
                if (!(value > HoleRadius)) { throw new Exceptions.CogwheelWrongInnerRadiusException("Внутренний радиус не может быть меньше или равен радиусу внутреннего отверстия."); }
                if (!(value < OuterRadius)) { throw new Exceptions.CogwheelWrongInnerRadiusException("Внутренний радиус не может быть больше или равен внешнему радиусу."); }
                _innerRadius = value;
            }
        }
        public double OuterRadius
        {
            get { return _outerRadius;}
            private set
            {
                if (!IsValidDouble(value)) { throw new Exceptions.CogwheelWrongOuterRadiusException("Заданный внешний радиус - не вещественное число."); }
                if (!(value >= 1.5)) { throw new Exceptions.CogwheelWrongOuterRadiusException("Внешний радиус не может быть меньше 1,5 мм."); }
                if (!(value <= 100)) { throw new Exceptions.CogwheelWrongOuterRadiusException("Внешний радиус не может быть больше 100 мм."); }
                if (!(value > InnerRadius)) { throw new Exceptions.CogwheelWrongOuterRadiusException("Внешний радиус не может быть меньше или равен внутреннему радиусу."); }
                _outerRadius = value;
            }
        }
        public double HoleRadius
        {
            get { return _holeRadius;}
            private set
            {
                if (!IsValidDouble(value)) { throw new Exceptions.CogwheelWrongHoleRadiusException("Заданный радиус внутреннего отверстия - не вещественное число."); }
                if (!(value >= 0.5)) { throw new Exceptions.CogwheelWrongHoleRadiusException("Радиус внутреннего отверстия не может быть меньше 0,5 мм."); }
                if (!(value <= 30)) { throw new Exceptions.CogwheelWrongHoleRadiusException("Радиус внутреннего отверстия не может быть больше 30 мм."); }
                if (!(value > 0)) { throw new Exceptions.CogwheelWrongHoleRadiusException("Радиус внутреннего отверстия не может быть меньше или равен нулю."); }
                if (!(value < InnerRadius)) { throw new Exceptions.CogwheelWrongHoleRadiusException("Радиус внутреннего отверстия не может быть больше или равен внутреннему радиусу."); }
                _holeRadius = value;
            }
        }
        public double Depth
        {
            get { return _depth;}
            private set
            {
                if (!IsValidDouble(value)) { throw new Exceptions.CogwheelWrongDepthException("Заданная толщина - не вещественное число."); }
                if (!(value >= 0.5)) { throw new Exceptions.CogwheelWrongDepthException("Толщина не может быть меньше 0.5 мм."); }
                if (!(value <= 70)) { throw new Exceptions.CogwheelWrongDepthException("Толщина не может быть больше 70 мм."); }
                if (!(value <= OuterRadius)) { throw new Exceptions.CogwheelWrongDepthException("Толщина не может быть больше или равна внешнему радиусу."); }
                _depth = value;
            }
        }
        public int Cogs
        {
            get { return _cogs;}
            private set
            {
                if (!IsValidInt(value)) { throw new Exceptions.CogwheelWrongCogsException("Заданное число зубцов - не вещественное число."); }
                if (!(value >= 5)) { throw new Exceptions.CogwheelWrongCogsException("Число зубцов не может быть меньше 5."); }
                if (!(value <= 30)) { throw new Exceptions.CogwheelWrongCogsException("Число зубцов не может быть больше 30."); }
                _cogs = value;
            }
        }

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

        private bool IsValidInt(Int32 value)
        {
            if (value < Int32.MinValue
                || value > Int32.MaxValue
            )
            {
                return false;
            }

            return true;
        }
    }
}
