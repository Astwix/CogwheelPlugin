using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using KAPITypes;

namespace Cogwheel_Plugin
{
    public partial class Form1 : Form
    {
        KompasObject kompas;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (kompas == null)
            {
                Type t = Type.GetTypeFromProgID("KOMPAS.Application.5");
                kompas = (KompasObject)Activator.CreateInstance(t);
            }

            if (kompas != null)
            {
                kompas.Visible = true;
                kompas.ActivateControllerAPI();
            }

            if (kompas != null)
            {
                // создание 3Д документа
                ksDocument3D doc = kompas.Document3D();
                doc.Create();
                ksPart part = doc.GetPart((short)Kompas6Constants3D.Part_Type.pTop_Part); // указатель на деталь 
                ksEntity planeXOY = part.GetDefaultEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeXOY); // определение плоскости XY 
                ksEntity sketch = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch); // создание переменной эскиза 
                ksSketchDefinition sd = sketch.GetDefinition(); // получение указателя на параметры эскиза 
                sd.SetPlane(planeXOY); // задание плоскости, на которой создается эскиз 
                sketch.Create(); // создается эскиз 
                ksDocument2D doc2d = sd.BeginEdit(); // режим редактирования эскиза 

                // Рассчитаем шестеренку 
                double r1 = 10; // внутренний радиус
                double r2 = 15; // внешний радиус
                double holeR = 3; //радиус прореизи 
                double width = 100; //толщина шестеренки
                int zubs = 10; //количество зубцов 
                double sector = 360 / (zubs * 4); // градусов на один сектор
                //double dotDelta = zubDelta / 2; // круговое расстояние между точками 
                double[] x = new double[zubs * 4];
                double[] y = new double[zubs * 4];
                double a = 0;
                //заполнение массива точек 
                for (int i = 0; i < zubs * 4; i = i + 4)
                {
                    x[i] = r1 * Math.Cos(a * (Math.PI / 180));
                    y[i] = r1 * Math.Sin(a * (Math.PI / 180));

                    a = a + sector;

                    x[i + 1] = r2 * Math.Cos(a * (Math.PI / 180));
                    y[i + 1] = r2 * Math.Sin(a * (Math.PI / 180));

                    a = a + sector;

                    x[i + 2] = r2 * Math.Cos(a * (Math.PI / 180));
                    y[i + 2] = r2 * Math.Sin(a * (Math.PI / 180));

                    a = a + sector;

                    x[i + 3] = r1 * Math.Cos(a * (Math.PI / 180));
                    y[i + 3] = r1 * Math.Sin(a * (Math.PI / 180));

                    a = a + sector;
                }
                // соединение точек отрезками 
                for (int i = 0; i < (zubs * 4) - 1; i++)
                {
                    doc2d.ksLineSeg(x[i], y[i], x[i + 1], y[i + 1], 1);
                }
                // соединение первой и последней точки 
                doc2d.ksLineSeg(x[0], y[0], x[(zubs * 4) - 1], y[(zubs * 4) - 1], 1);

                // центральная окружность -> вырез
                doc2d.ksCircle(0, 0, holeR, 1);

                sd.EndEdit();

                //выдавливание детали 
                ksEntity extrude = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_bossExtrusion);
                ksBossExtrusionDefinition extrDef = extrude.GetDefinition();
                extrDef.directionType = (short)Kompas6Constants3D.Direction_Type.dtMiddlePlane;
                extrDef.SetSketch(sketch);
                ksExtrusionParam extrudeParam = extrDef.ExtrusionParam();
                extrudeParam.depthNormal = width;
                extrude.Create();

                // первая доп. плоскость, первый эскиз и далее вырез
                ksEntity plane = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeOffset); //создаем переменную смещенной поверхности 
                ksPlaneOffsetDefinition pod = plane.GetDefinition(); // получаем указатель на её настройки 
                pod.SetPlane(planeXOY); // ХУ плоскость установим как исходную, чтобы отталкиватся от неё 
                pod.offset = width/2; //смещаемся на десять 
                plane.Create(); // создаем саму плоскость 
                ksEntity sketch2 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch); //создаем переменную эскиза 
                ksSketchDefinition sd2 = sketch2.GetDefinition(); //получим указатель на параметры эскиза 
                sd2.SetPlane(plane); //зададим плоскость на которой создаем эскиз 
                sketch2.Create(); // создаем эскизa 
                ksDocument2D doc2d2 = sd2.BeginEdit();
                //нарисуем на эскизе два круга 
                doc2d2.ksCircle(0, 0, (holeR + ((r1-holeR) * 0.2)), 1);
                doc2d2.ksCircle(0, 0, (holeR + ((r1-holeR) * 0.8)), 1);
                sd2.EndEdit();

                // вырез
                ksEntity cutExtrude = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_cutExtrusion);
                ksCutExtrusionDefinition cutextrDef = cutExtrude.GetDefinition();
                cutextrDef.directionType = (short)Kompas6Constants3D.Direction_Type.dtNormal;
                cutextrDef.SetSketch(sketch2);
                ksExtrusionParam cutExtrParam = cutextrDef.ExtrusionParam();
                cutExtrParam.depthNormal = (width / 3);
                cutExtrude.Create();

                // вторая доп. плоскость, второй эскиз и далее вырез
                ksEntity plane2 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_planeOffset); //создаем переменную смещенной поверхности 
                ksPlaneOffsetDefinition pod2 = plane2.GetDefinition(); // получаем указатель на её настройки 
                pod2.SetPlane(planeXOY); // ХУ плоскость установим как исходную, чтобы отталкиватся от неё 
                pod2.direction = false;
                pod2.offset = width / 2; //смещаемся на десять 
                plane2.Create(); // создаем саму плоскость 
                ksEntity sketch3 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_sketch); //создаем переменную эскиза 
                ksSketchDefinition sd3 = sketch3.GetDefinition(); //получим указатель на параметры эскиза 
                sd3.SetPlane(plane2); //зададим плоскость на которой создаем эскиз 
                sketch3.Create(); // создаем эскизa 
                ksDocument2D doc2d3 = sd3.BeginEdit();
                //нарисуем на эскизе два круга 
                doc2d3.ksCircle(0, 0, (holeR + ((r1 - holeR) * 0.2)), 1);
                doc2d3.ksCircle(0, 0, (holeR + ((r1 - holeR) * 0.8)), 1);
                sd3.EndEdit();

                // вырез
                ksEntity cutExtrude2 = part.NewEntity((short)Kompas6Constants3D.Obj3dType.o3d_cutExtrusion);
                ksCutExtrusionDefinition cutextrDef2 = cutExtrude2.GetDefinition();
                cutextrDef2.directionType = (short)Kompas6Constants3D.Direction_Type.dtReverse;
                cutextrDef2.SetSketch(sketch3);
                ksExtrusionParam cutExtrParam2 = cutextrDef2.ExtrusionParam();
                cutExtrParam2.depthReverse = (width / 3);
                cutExtrude2.Create();

            }
        }
    }
}
