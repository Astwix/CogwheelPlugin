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
                ksSketchDefinition sd = sketch.GetDefinition(); // получение указатель на параметры эскиза 
                sd.SetPlane(planeXOY); // задание плоскости, на которой создается эскиз 
                sketch.Create(); // создается эскиз 
                ksDocument2D doc2d = sd.BeginEdit(); // режим редактирования эскиза 

                // Рассчитаем шестеренку 
                double r1 = 10;
                double r2 = 15;
                int zubs = 15; //количество зубцов 
                double zubDelta = 360 / zubs; // круговое расстояние между вершинами зубцов 
                double dotDelta = zubDelta / 2; // круговое расстояние между точками 
                double[] x = new double[zubs * 2];
                double[] y = new double[zubs * 2];
                //заполнение массива точек 
                for (int i = 0; i < zubs * 2; i++)
                {
                    if (i % 2 == 0)
                    {
                        x[i] = r2 * Math.Cos((i * dotDelta) * (Math.PI / 180));
                        y[i] = r2 * Math.Sin((i * dotDelta) * (Math.PI / 180));
                    }
                    else
                    {
                        x[i] = r1 * Math.Cos((i * dotDelta) * (Math.PI / 180));
                        y[i] = r1 * Math.Sin((i * dotDelta) * (Math.PI / 180));
                    }
                }
                // соединение точек отрезками 
                int k = 0;
                for (int i = 0; i < (zubs * 2) - 1; i++)
                {
                    doc2d.ksLineSeg(x[i], y[i], x[i + 1], y[i + 1], 1);
                    k++;
                }
                // соединение первой и последней точки 
                doc2d.ksLineSeg(x[0], y[0], x[(zubs * 2) - 1], y[(zubs * 2) - 1], 1);

                sd.EndEdit();
            }
        }
    }
}
