﻿using System;
using Cogwheel_Plugin.Model.Exceptions;
using NUnit.Framework;

namespace Cogwheel_Plugin_Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [TestCase(95, 100, 30, 70, 30, TestName = "(Позитивный)Максимальные параметры консруктора")]
        [TestCase(1, 1.5, 0.5, 0.5, 5, TestName = "(Позитивный)Минимальные параметры консруктора")]
        [TestCase(45.5, 50.5, 15.5, 35.5, 20, TestName = "(Позитивный)Средние параметры консруктора")]
        [Test]
        public void TestPositiveCogwheelConstructor(double innerRadius, double outerRadius, double holeRadius, double depth, int cogs)
        {
            Assert.DoesNotThrow((() =>
            {
                new Cogwheel_Plugin.Model.Cogwheel(innerRadius, outerRadius, holeRadius, depth, cogs);
            }
            ));
        }

        [TestCase(0.5, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус меньше")]
        [TestCase(96, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус больше")]
        [TestCase(double.NaN, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус не число")]
        [TestCase(double.MinValue, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус double.minvalue")]
        [TestCase(double.MaxValue, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус double.maxvalue")]
        [TestCase(double.NegativeInfinity, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус double.negativeinfinity")]
        [TestCase(double.PositiveInfinity, 100, 30, 70, 30, typeof(CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус double.positiveinfinity")]

        [TestCase(95, 1, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус меньше")]
        [TestCase(95, 101, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус больше")]
        [TestCase(95, double.NaN, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус не число")]
        [TestCase(95, double.MinValue, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус double.minvalue")]
        [TestCase(95, double.MaxValue, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус double.maxvalue")]
        [TestCase(95, double.NegativeInfinity, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус double.negativeinfinity")]
        [TestCase(95, double.PositiveInfinity, 30, 70, 30, typeof(CogwheelWrongOuterRadiusException), TestName = "(Негативный)Внешний радиус double.positiveinfinity")]

        [TestCase(95, 100, 0.3, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия меньше")]
        [TestCase(95, 100, 31, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия больше")]
        [TestCase(95, 100, double.NaN, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия не число")]
        [TestCase(95, 100, double.MinValue, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия double.minvalue")]
        [TestCase(95, 100, double.MaxValue, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия double.maxvalue")]
        [TestCase(95, 100, double.NegativeInfinity, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия double.negativeinfinity")]
        [TestCase(95, 100, double.PositiveInfinity, 70, 30, typeof(CogwheelWrongHoleRadiusException), TestName = "(Негативный)Радиус внутреннего отверстия double.positiveinfinity")]

        [TestCase(95, 100, 30, 0.3, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина меньше")]
        [TestCase(95, 100, 30, 71, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина больше")]
        [TestCase(95, 100, 30, double.NaN, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина не число")]
        [TestCase(95, 100, 30, double.MinValue, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина double.minvalue")]
        [TestCase(95, 100, 30, double.MaxValue, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина double.maxvalue")]
        [TestCase(95, 100, 30, double.NegativeInfinity, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина double.negativeinfinity")]
        [TestCase(95, 100, 30, double.PositiveInfinity, 30, typeof(CogwheelWrongDepthException), TestName = "(Негативный)Толщина double.positiveinfinity")]

        [TestCase(95, 100, 30, 70, 4, typeof(CogwheelWrongCogsException), TestName = "(Негативный)Количество зубцов меньше")]
        [TestCase(95, 100, 30, 70, 31, typeof(CogwheelWrongCogsException), TestName = "(Негативный)Количество зубцов меньше")]
        [TestCase(95, 100, 30, 70, int.MinValue, typeof(CogwheelWrongCogsException), TestName = "(Негативный)Количество зубцов int.minvalue")]
        [TestCase(95, 100, 30, 70, int.MaxValue, typeof(CogwheelWrongCogsException), TestName = "(Негативный)Количество зубцов int.maxvalue")]
        [Test]
        public void TestNegativeCogwheelConstructor(double innerRadius, double outerRadius, double holeRadius, double depth, int cogs, Type exceptionType)
        {
            Assert.That(() =>
            {
                new Cogwheel_Plugin.Model.Cogwheel(innerRadius, outerRadius, holeRadius, depth, cogs);
            }, Throws.TypeOf(exceptionType));
        }

        [TestCase(95, TestName = "(Позитивный)Получение внутреннего радиуса")]
        [Test]
        public void TestInnerRadiusGet(double value)
        {
            Cogwheel_Plugin.Model.Cogwheel cw = new Cogwheel_Plugin.Model.Cogwheel(95, 100, 30, 70, 30);
            Assert.AreEqual(value, cw.InnerRadius);
        }

        [TestCase(100, TestName = "(Позитивный)Получение внешнего радиуса")]
        [Test]
        public void TestOuterRadiusGet(double value)
        {
            Cogwheel_Plugin.Model.Cogwheel cw = new Cogwheel_Plugin.Model.Cogwheel(95, 100, 30, 70, 30);
            Assert.AreEqual(value, cw.OuterRadius);
        }

        [TestCase(30, TestName = "(Позитивный)Получение внутреннего радиуса отверстия")]
        [Test]
        public void TestHoleRadiusGet(double value)
        {
            Cogwheel_Plugin.Model.Cogwheel cw = new Cogwheel_Plugin.Model.Cogwheel(95, 100, 30, 70, 30);
            Assert.AreEqual(value, cw.HoleRadius);
        }

        [TestCase(70, TestName = "(Позитивный)Получение толщины")]
        [Test]
        public void TestDepthGet(double value)
        {
            Cogwheel_Plugin.Model.Cogwheel cw = new Cogwheel_Plugin.Model.Cogwheel(95, 100, 30, 70, 30);
            Assert.AreEqual(value, cw.Depth);
        }

        [TestCase(30, TestName = "(Позитивный)Получение количества зубцов")]
        [Test]
        public void TestCogsGet(double value)
        {
            Cogwheel_Plugin.Model.Cogwheel cw = new Cogwheel_Plugin.Model.Cogwheel(95, 100, 30, 70, 30);
            Assert.AreEqual(value, cw.Cogs);
        }
    }
}
