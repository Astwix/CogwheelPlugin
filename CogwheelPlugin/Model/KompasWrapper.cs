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
    class KompasWrapper
    {
        private KompasObject _kompas = null;

        public void StartKompas()
        {
            if (_kompas == null)
            {
                Type t = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompas = (KompasObject)Activator.CreateInstance(t);
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
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        Type t = Type.GetTypeFromProgID("KOMPAS.Application.5");
                        _kompas = (KompasObject)Activator.CreateInstance(t);
                        if (tried > 3)
                        {
                            retry = false;
                        }
                    }
                }
                _kompas.ActivateControllerAPI();
            }

            if (_kompas == null) throw new Exception("Нет связи с Kompas3D.");
        }

        public void BuildCogwheel(Cogwheel cogwheel)
        {
            if (_kompas == null) throw new Exception("Не возможно построить деталь. Нет связи с Kompas3D.");

            double sector = 360.0 / (cogwheel.Cogs * 4); // градусов на один сектор
            double[] xArray = new double[cogwheel.Cogs * 4];
            double[] yArray = new double[cogwheel.Cogs * 4];
            double a = 0;
            //заполнение массива точек 
            for (int i = 0; i < cogwheel.Cogs * 4; i = i + 4)
            {
                xArray[i] = cogwheel.InnerRadius * Math.Cos(a * (Math.PI / 180));
                yArray[i] = cogwheel.InnerRadius * Math.Sin(a * (Math.PI / 180));

                a = a + sector;

                xArray[i + 1] = cogwheel.OuterRadius * Math.Cos(a * (Math.PI / 180));
                yArray[i + 1] = cogwheel.OuterRadius * Math.Sin(a * (Math.PI / 180));

                a = a + sector;

                xArray[i + 2] = cogwheel.OuterRadius * Math.Cos(a * (Math.PI / 180));
                yArray[i + 2] = cogwheel.OuterRadius * Math.Sin(a * (Math.PI / 180));

                a = a + sector;

                xArray[i + 3] = cogwheel.InnerRadius * Math.Cos(a * (Math.PI / 180));
                yArray[i + 3] = cogwheel.InnerRadius * Math.Sin(a * (Math.PI / 180));

                a = a + sector;
            }

            // создание 3Д документа
            ksDocument3D doc = _kompas.Document3D();
            doc.Create();
            ksPart part = doc.GetPart((short)Kompas6Constants3D.Part_Type.pTop_Part); // указатель на деталь 
            ksEntity planeXOY = part.GetDefaultEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeXOY); // определение плоскости XY 
            ksEntity sketch = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch); // создание переменной эскиза 
            ksSketchDefinition sd = sketch.GetDefinition(); // получение указателя на параметры эскиза 
            sd.SetPlane(planeXOY); // задание плоскости, на которой создается эскиз 
            sketch.Create(); // создается эскиз 
            ksDocument2D doc2d = sd.BeginEdit(); // режим редактирования эскиза 

            // соединение точек отрезками 
            for (int i = 0; i < (cogwheel.Cogs * 4) - 1; i++)
            {
                doc2d.ksLineSeg(xArray[i], yArray[i], xArray[i + 1], yArray[i + 1], 1);
            }
            // соединение первой и последней точки 
            doc2d.ksLineSeg(xArray[0], yArray[0], xArray[(cogwheel.Cogs * 4) - 1], yArray[(cogwheel.Cogs * 4) - 1], 1);

            // центральная окружность -> вырез
            doc2d.ksCircle(0, 0, cogwheel.HoleRadius, 1);

            // добавление вырезов
            switch (cogwheel.TypeOfExtrude)
            {
                case ExtrudeType.Classic:
                    break;
                // круги
                case ExtrudeType.Circles:
                    {
                        double crugsDelta = ((360.0 / cogwheel.ExtrudeCount) * Math.PI) / 180;
                        double l = cogwheel.HoleRadius + (cogwheel.InnerRadius - cogwheel.HoleRadius) / 2;
                        for (int i = 0; i < cogwheel.ExtrudeCount; ++i)
                        {
                            double xc = l * Math.Cos(i * crugsDelta);
                            double yc = l * Math.Sin(i * crugsDelta);
                            doc2d.ksCircle(xc, yc, (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.3)) / 2, 1);
                        }
                    }
                    break;
                // "лодочки"
                case ExtrudeType.Boats:
                    {
                        double boatLength = (360 * 0.8) / cogwheel.ExtrudeCount;
                        double needleLength = (360 * 0.2) / cogwheel.ExtrudeCount;
                        double b = 0;
                        for (int i = 0; i < cogwheel.ExtrudeCount; ++i)
                        {
                            double x1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Cos(b * (Math.PI / 180));
                            double y1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Sin(b * (Math.PI / 180));
                            b += boatLength / 2;
                            double xc1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Cos(b * (Math.PI / 180));
                            double yc1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Sin(b * (Math.PI / 180));
                            double xc2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Cos(b * (Math.PI / 180));
                            double yc2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Sin(b * (Math.PI / 180));
                            b += boatLength / 2;
                            double x2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Cos(b * (Math.PI / 180));
                            double y2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Sin(b * (Math.PI / 180));
                            doc2d.ksBezier(0, 1);
                            doc2d.ksPoint(x1, y1, 1);
                            doc2d.ksPoint(xc1, yc1, 1);
                            doc2d.ksPoint(x2, y2, 1);
                            doc2d.ksEndObj();
                            doc2d.ksBezier(0, 1);
                            doc2d.ksPoint(x1, y1, 1);
                            doc2d.ksPoint(xc2, yc2, 1);
                            doc2d.ksPoint(x2, y2, 1);
                            doc2d.ksEndObj();
                            b += needleLength;
                        }
                    }
                    break;
                // спицеобразный
                case ExtrudeType.Needles:
                    {
                        double extrudeLength = (360 * 0.8) / cogwheel.ExtrudeCount;
                        double needleLength = (360 * 0.2) / cogwheel.ExtrudeCount;
                        double b = 0;
                        for (int i = 0; i < cogwheel.ExtrudeCount; ++i)
                        {
                            double x1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Cos(b * (Math.PI / 180));
                            double y1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Sin(b * (Math.PI / 180));
                            double x2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Cos(b * (Math.PI / 180));
                            double y2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Sin(b * (Math.PI / 180));
                            b += extrudeLength / 2;
                            double xc1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Cos(b * (Math.PI / 180));
                            double yc1 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Sin(b * (Math.PI / 180));
                            double xc2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Cos(b * (Math.PI / 180));
                            double yc2 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Sin(b * (Math.PI / 180));
                            b += extrudeLength / 2;
                            double x3 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Cos(b * (Math.PI / 180));
                            double y3 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)) * Math.Sin(b * (Math.PI / 180));
                            double x4 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Cos(b * (Math.PI / 180));
                            double y4 = (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)) * Math.Sin(b * (Math.PI / 180));
                            doc2d.ksBezier(0, 1);
                            doc2d.ksPoint(x1, y1, 1);
                            doc2d.ksPoint(xc1, yc1, 1);
                            doc2d.ksPoint(x3, y3, 1);
                            doc2d.ksEndObj();
                            doc2d.ksBezier(0, 1);
                            doc2d.ksPoint(x2, y2, 1);
                            doc2d.ksPoint(xc2, yc2, 1);
                            doc2d.ksPoint(x4, y4, 1);
                            doc2d.ksEndObj();
                            doc2d.ksLineSeg(x1, y1, x2, y2, 1);
                            doc2d.ksLineSeg(x3, y3, x4, y4, 1);
                            b += needleLength;
                        }
                    }
                    break;
            }

            sd.EndEdit();

            //выдавливание детали 
            ksEntity extrude = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_bossExtrusion);
            ksBossExtrusionDefinition extrDef = extrude.GetDefinition();
            extrDef.directionType = (short)Kompas6Constants3D.Direction_Type.dtMiddlePlane;
            extrDef.SetSketch(sketch);
            ksExtrusionParam extrudeParam = extrDef.ExtrusionParam();
            extrudeParam.depthNormal = cogwheel.Depth;
            extrude.Create();

            if(cogwheel.TypeOfExtrude == ExtrudeType.Classic)
            {
                // первая доп. плоскость, первый эскиз и далее вырез
                ksEntity plane = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeOffset); //создаем переменную смещенной поверхности 
                ksPlaneOffsetDefinition pod = plane.GetDefinition(); // получаем указатель на её настройки 
                pod.SetPlane(planeXOY); // ХУ плоскость установим как исходную, чтобы отталкиватся от неё 
                pod.offset = cogwheel.Depth / 2; //смещаемся на десять 
                    plane.Create(); // создаем саму плоскость 
                ksEntity sketch2 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch); //создаем переменную эскиза 
                ksSketchDefinition sd2 = sketch2.GetDefinition(); //получим указатель на параметры эскиза 
                sd2.SetPlane(plane); //зададим плоскость на которой создаем эскиз 
                sketch2.Create(); // создаем эскизa 
                ksDocument2D doc2d2 = sd2.BeginEdit();
                //нарисуем на эскизе два круга 
                doc2d2.ksCircle(0, 0, (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)), 1);
                doc2d2.ksCircle(0, 0, (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)), 1);
                sd2.EndEdit();

                // вырез
                ksEntity cutExtrude = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_cutExtrusion);
                ksCutExtrusionDefinition cutextrDef = cutExtrude.GetDefinition();
                cutextrDef.directionType = (short)Kompas6Constants3D.Direction_Type.dtNormal;
                cutextrDef.SetSketch(sketch2);
                ksExtrusionParam cutExtrParam = cutextrDef.ExtrusionParam();
                cutExtrParam.depthNormal = (cogwheel.Depth / 3);
                cutExtrude.Create();

                // вторая доп. плоскость, второй эскиз и далее вырез
                ksEntity plane2 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeOffset); //создаем переменную смещенной поверхности 
                ksPlaneOffsetDefinition pod2 = plane2.GetDefinition(); // получаем указатель на её настройки 
                pod2.SetPlane(planeXOY); // ХУ плоскость установим как исходную, чтобы отталкиватся от неё 
                pod2.direction = false;
                pod2.offset = cogwheel.Depth / 2; //смещаемся на десять 
                plane2.Create(); // создаем саму плоскость 
                ksEntity sketch3 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch); //создаем переменную эскиза 
                ksSketchDefinition sd3 = sketch3.GetDefinition(); //получим указатель на параметры эскиза 
                sd3.SetPlane(plane2); //зададим плоскость на которой создаем эскиз 
                sketch3.Create(); // создаем эскизa 
                ksDocument2D doc2d3 = sd3.BeginEdit();
                //нарисуем на эскизе два круга 
                doc2d3.ksCircle(0, 0, (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.2)), 1);
                doc2d3.ksCircle(0, 0, (cogwheel.HoleRadius + ((cogwheel.InnerRadius - cogwheel.HoleRadius) * 0.8)), 1);
                sd3.EndEdit();

                // вырез
                ksEntity cutExtrude2 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_cutExtrusion);
                ksCutExtrusionDefinition cutextrDef2 = cutExtrude2.GetDefinition();
                cutextrDef2.directionType = (short)Kompas6Constants3D.Direction_Type.dtReverse;
                cutextrDef2.SetSketch(sketch3);
                ksExtrusionParam cutExtrParam2 = cutextrDef2.ExtrusionParam();
                cutExtrParam2.depthReverse = (cogwheel.Depth / 3);
                cutExtrude2.Create();
            }
        }
    }
}
