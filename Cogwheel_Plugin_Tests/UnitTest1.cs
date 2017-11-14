using System;
using Cogwheel_Plugin;
using NUnit.Framework;

namespace Cogwheel_Plugin_Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [TestCase(80, 100, 10, 20, 30, TestName = "Максимальные параметры консруктора")]
        [TestCase(70, 120, 15, 25, 25, TestName = "Минимальные параметры консруктора")]
        [Test]
        public void TestPositiveCogwheelConstructor(double innerRadius, double outerRadius, double holeRadius, double depth, int cogs)
        {
            Assert.DoesNotThrow((() =>
            {
                new Cogwheel_Plugin.Model.Cogwheel(innerRadius, outerRadius, holeRadius, depth, cogs);
            }
            ));
        }

        [TestCase(110, 100, 10, 20, 30, typeof(Cogwheel_Plugin.Model.Exceptions.CogwheelWrongInnerRadiusException), TestName = "(Негативный)Внутренний радиус")]
        [Test]
        public void TestNegativeCogwheelConstructor(double innerRadius, double outerRadius, double holeRadius, double depth, int cogs, Type exceptionType)
        {
            Assert.That(() =>
            {
                new Cogwheel_Plugin.Model.Cogwheel(innerRadius, outerRadius, holeRadius, depth, cogs);
            }, Throws.TypeOf(exceptionType));
        }
    }
}
