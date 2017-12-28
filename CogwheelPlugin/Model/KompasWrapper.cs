using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using KAPITypes;

namespace CogwheelPlugin.Model
{
    /// <summary>
    /// Класс для взаимодействия с КОМПАС
    /// </summary>
    public class KompasWrapper
    {
        /// <summary>
        /// Объект КОМПАС API
        /// </summary>
        private KompasObject _kompas = null;

        /// <summary>
        /// Запуск КОМПАС, если он не запущен
        /// </summary>
        public void StartKompas()
        {
            if (_kompas == null)
            {
                Type kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompas = (KompasObject)Activator.CreateInstance(kompasType);
            }

            if (_kompas != null)
            {
                bool retry = true;
                short tried = 0;
                while (retry)
                {
                    try
                    {
                        tried++;
                        _kompas.Visible = true;
                        retry = false;
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        Type kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                        _kompas = (KompasObject)Activator.CreateInstance(kompasType);
                        if (tried > 3)
                        {
                            retry = false;
                        }
                    }
                }
                _kompas.ActivateControllerAPI();
            }
        }

        /// <summary>
        /// Построение зубчатого колеса в КОМПАС
        /// </summary>
        /// <param name="cogwheel"></param>
        public void BuildCogwheel(Cogwheel cogwheel)
        {
            if (_kompas == null) 
            { 
                throw new Exception(
                    "Невозможно построить деталь. Нет связи с КОМПАС 3D."); 
            }

            // Расчет количества градусов на один сектор (зуб)
            double sector = 360.0 / (cogwheel.Cogs * 4); 
            
            // Расчет точек детали

            double[] xArray = new double[cogwheel.Cogs * 4];
            double[] yArray = new double[cogwheel.Cogs * 4];
            double degree = 0;
            for (int i = 0; i < cogwheel.Cogs * 4; i = i + 4)
            {
                xArray[i] = cogwheel.InnerRadius * Math.Cos(degree * (Math.PI / 180));
                yArray[i] = cogwheel.InnerRadius * Math.Sin(degree * (Math.PI / 180));
                degree = degree + sector;

                for (int j = i + 1; j < i + 3; ++j)
                {
                    xArray[j] = cogwheel.OuterRadius * Math.Cos(degree * (Math.PI / 180));
                    yArray[j] = cogwheel.OuterRadius * Math.Sin(degree * (Math.PI / 180));
                    degree = degree + sector;
                }

                xArray[i + 3] = cogwheel.InnerRadius * Math.Cos(degree * (Math.PI / 180));
                yArray[i + 3] = cogwheel.InnerRadius * Math.Sin(degree * (Math.PI / 180));
                degree = degree + sector;
            }

            // Создание 3Д документа
            
            ksDocument3D document3D = _kompas.Document3D();
            document3D.Create();
            
            // Указатель на деталь 
            ksPart part = document3D.GetPart((short)Kompas6Constants3D.Part_Type.pTop_Part);
            // Определение плоскости XY
            ksEntity planeXOY = 
                part.GetDefaultEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeXOY);
            // Создание переменной эскиза 
            ksEntity sketch = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch);
            // Получение указателя на параметры эскиза 
            ksSketchDefinition sketchDefinition = sketch.GetDefinition();
            // Задание плоскости, на которой создается эскиз 
            sketchDefinition.SetPlane(planeXOY);
            // Создание эскиза 
            sketch.Create(); 
            // Переход в режим редактирования эскиза
            ksDocument2D document2D = sketchDefinition.BeginEdit();  

            // Соединение точек отрезками 

            for (int i = 0; i < (cogwheel.Cogs * 4) - 1; i++)
            {
                document2D.ksLineSeg(xArray[i], yArray[i], xArray[i + 1], yArray[i + 1], 1);
            }

            // Соединение первой и последней точки 
            document2D.ksLineSeg(xArray[0], yArray[0], xArray[(cogwheel.Cogs * 4) - 1], 
                yArray[(cogwheel.Cogs * 4) - 1], 1);
            // Задание центральной окружности
            document2D.ksCircle(0, 0, cogwheel.HoleRadius, 1);

            // Заранее расчитаем некоторые значения, которые далее часто используются

            double radiusDelta = cogwheel.InnerRadius - cogwheel.HoleRadius;
            double outerRadiusDot = cogwheel.HoleRadius + (radiusDelta * 0.8);
            double innerRadiusDot = cogwheel.HoleRadius + (radiusDelta * 0.2);
            double degreeToRadian = Math.PI / 180;

            // Добавление вырезов
            
            switch (cogwheel.TypeOfExtrude)
            {
                case ExtrudeType.Classic:
                    // Не обрабарывается здесь, т.к. имеет другую логику
                    break;

                case ExtrudeType.Circles:
                    {
                        double circlesDegreeDelta = ((360.0 / cogwheel.ExtrudeCount)
                        * Math.PI) / 180;
                        double distanceToCenter = cogwheel.HoleRadius
                            + (cogwheel.InnerRadius - cogwheel.HoleRadius) / 2;
                        for (int i = 0; i < cogwheel.ExtrudeCount; ++i)
                        {
                            double xCenter = distanceToCenter * Math.Cos(i * circlesDegreeDelta);
                            double yCenter = distanceToCenter * Math.Sin(i * circlesDegreeDelta);
                            document2D.ksCircle(xCenter, yCenter, (cogwheel.HoleRadius
                                + (radiusDelta * 0.3)) / 2, 1);
                        }
                    }
                    break;

                case ExtrudeType.Boats:
                    {
                        double boatLength = (360 * 0.8) / cogwheel.ExtrudeCount;
                        double needleLength = (360 * 0.2) / cogwheel.ExtrudeCount;
                        double degreePointer = 0;

                        for (int i = 0; i < cogwheel.ExtrudeCount; ++i)
                        {
                            double x1 = outerRadiusDot * Math.Cos(degreePointer * degreeToRadian);
                            double y1 = outerRadiusDot * Math.Sin(degreePointer * degreeToRadian);
                            degreePointer += boatLength / 2;
                            double xc1 = innerRadiusDot * Math.Cos(degreePointer * degreeToRadian);
                            double yc1 = innerRadiusDot * Math.Sin(degreePointer * degreeToRadian);
                            double xc2 = outerRadiusDot * Math.Cos(degreePointer * degreeToRadian);
                            double yc2 = outerRadiusDot * Math.Sin(degreePointer * degreeToRadian);
                            degreePointer += boatLength / 2;
                            double x2 = outerRadiusDot * Math.Cos(degreePointer * degreeToRadian);
                            double y2 = outerRadiusDot * Math.Sin(degreePointer * degreeToRadian);
                            Draw3DotBezier(document2D, x1, y1, xc1, yc1, x2, y2);
                            Draw3DotBezier(document2D, x1, y1, xc2, yc2, x2, y2);
                            degreePointer += needleLength;
                        }
                    }
                    break;
                
                case ExtrudeType.Needles:
                    {
                        double extrudeLength = (360 * 0.8) / cogwheel.ExtrudeCount;
                        double needleLength = (360 * 0.2) / cogwheel.ExtrudeCount;
                        double degreePointer = 0;
                        for (int i = 0; i < cogwheel.ExtrudeCount; ++i)
                        {
                            double[] line1X = new double[3];
                            double[] line1Y = new double[3];
                            double[] line2X = new double[3];
                            double[] line2Y = new double[3];
                            for (int j = 0; j < 3; ++j)
                            {
                                line1X[j] = innerRadiusDot * Math.Cos(degreePointer * degreeToRadian);
                                line1Y[j] = innerRadiusDot * Math.Sin(degreePointer * degreeToRadian);
                                line2X[j] = outerRadiusDot * Math.Cos(degreePointer * degreeToRadian);
                                line2Y[j] = outerRadiusDot * Math.Sin(degreePointer * degreeToRadian);
                                degreePointer += extrudeLength / 2;
                            }
                            degreePointer -= extrudeLength / 2;
                            Draw3DotBezier(document2D, 
                                line1X[0], line1Y[0], 
                                line1X[1], line1Y[1], 
                                line1X[2], line1Y[2]);
                            Draw3DotBezier(document2D, 
                                line2X[0], line2Y[0], 
                                line2X[1], line2Y[1], 
                                line2X[2], line2Y[2]);
                            document2D.ksLineSeg(line1X[0], line1Y[0], line2X[0], line2Y[0], 1);
                            document2D.ksLineSeg(line1X[2], line1Y[2], line2X[2], line2Y[2], 1);
                            degreePointer += needleLength;
                        }
                    }
                    break;
            }
            sketchDefinition.EndEdit();

            // Выдавливание детали 
            
            ksEntity extrude = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_bossExtrusion);
            ksBossExtrusionDefinition extrudeDefinition = extrude.GetDefinition();
            extrudeDefinition.directionType = (short)Kompas6Constants3D.Direction_Type.dtMiddlePlane;
            extrudeDefinition.SetSketch(sketch);
            ksExtrusionParam extrudeParam = extrudeDefinition.ExtrusionParam();
            extrudeParam.depthNormal = cogwheel.Depth;
            extrude.Create();

            // Выдавливание классического выреза (углубления)

            if (cogwheel.TypeOfExtrude == ExtrudeType.Classic)
            {
                // Создание дополнителных плоскостей с обоих сторон зубчатоо колеса
                ksEntity planeTop = CreateOffsetPlane(planeXOY, part, cogwheel.Depth / 2, true);
                ksEntity planeBottom = CreateOffsetPlane(planeXOY, part, cogwheel.Depth / 2, false);

                // Создание эскизов классическоо выреза (два круга) на плоскостях
                ksEntity sketchTop = DrawClassicExtrude(part, planeTop, 
                    innerRadiusDot, outerRadiusDot);
                ksEntity sketchBottom = DrawClassicExtrude(part, planeBottom, 
                    innerRadiusDot, outerRadiusDot);

                // Выдавливание эскизов
                ExecuteCutExtrude(part, sketchTop, cogwheel.Depth / 3, false);
                ExecuteCutExtrude(part, sketchBottom, cogwheel.Depth / 3, true);
            }
        }

        /// <summary>
        /// Нарисовать кривую Безье по трем точкам. Требует режим редактирования эскиза.
        /// </summary>
        /// <param name="document2D"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        private void Draw3DotBezier(ksDocument2D document2D, 
            double x1, double y1, double x2, double y2, double x3, double y3)
        {
            document2D.ksBezier(0, 1);
            document2D.ksPoint(x1, y1, 1);
            document2D.ksPoint(x2, y2, 1);
            document2D.ksPoint(x3, y3, 1);
            document2D.ksEndObj();
        }

        /// <summary>
        /// Создать смещённую плоскость
        /// </summary>
        /// <param name="basePlane"></param>
        /// <param name="part"></param>
        /// <param name="offset"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private ksEntity CreateOffsetPlane(ksEntity basePlane, ksPart part, 
            double offset, bool direction)
        {
            // Создание переменной смещенной поверхности 
            ksEntity plane = 
                part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeOffset);
            // Получение указателя на её настройки
            ksPlaneOffsetDefinition planeOffsetDefinition = plane.GetDefinition();
            // Установка исходной плоскости 
            planeOffsetDefinition.SetPlane(basePlane);
            // Смещение на половину толщины
            planeOffsetDefinition.offset = offset;
            planeOffsetDefinition.direction = direction;
            plane.Create();
            return plane;
        }

        /// <summary>
        /// Сделать вырез
        /// </summary>
        /// <param name="part"></param>
        /// <param name="sketch"></param>
        /// <param name="depth"></param>
        /// <param name="reverse"></param>
        private void ExecuteCutExtrude(ksPart part, ksEntity sketch, 
            double depth, bool reverse)
        {
            // Создание переменной выреза 
            ksEntity cutExtrude = 
                part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_cutExtrusion);
            // Получение указателя на его настройки
            ksCutExtrusionDefinition cutextrDefinition = cutExtrude.GetDefinition();
            if (reverse)
            {
                cutextrDefinition.directionType = 
                    (short)Kompas6Constants3D.Direction_Type.dtReverse;
            }
            else
            {
                cutextrDefinition.directionType = 
                    (short)Kompas6Constants3D.Direction_Type.dtNormal;
            }
            // Установка плоскости
            cutextrDefinition.SetSketch(sketch);
            ksExtrusionParam cutExtrudeParam = cutextrDefinition.ExtrusionParam();
            if (reverse)
            {
                cutExtrudeParam.depthReverse = depth;
            }
            else
            {
                cutExtrudeParam.depthNormal = depth;
            }
            cutExtrude.Create();
        }
        
        /// <summary>
        /// Создать эскиз классического выреза (два круга)
        /// </summary>
        /// <param name="part"></param>
        /// <param name="plane"></param>
        /// <param name="innerRadius"></param>
        /// <param name="outerRadius"></param>
        /// <returns></returns>
        private ksEntity DrawClassicExtrude(ksPart part, ksEntity plane, 
            double innerRadius, double outerRadius)
        {
            // Создание переменной эскиза 
            ksEntity sketch = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch);
            // Получение указателя на параметры эскиза 
            ksSketchDefinition sketchDefinition = sketch.GetDefinition();
            // Задание плоскости, на которой создастся эскиз 
            sketchDefinition.SetPlane(plane);
            sketch.Create();
            ksDocument2D document2D = sketchDefinition.BeginEdit();
            document2D.ksCircle(0, 0, innerRadius, 1);
            document2D.ksCircle(0, 0, outerRadius, 1);
            sketchDefinition.EndEdit();
            return sketch;
        }
    }
}
