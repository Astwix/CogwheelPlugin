using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cogwheel_Plugin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InnerRadiusTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            OuterRadiusTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            HoleRadiusTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            DepthTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            CogsTextBox.KeyPress += new KeyPressEventHandler(IsNumberPressed);
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if (OuterRadiusTextBox.Text == "" || OuterRadiusTextBox.Text == "." || OuterRadiusTextBox.Text == ",")
            {
                OuterRadiusLabel.BackColor = Color.Red;
                MessageBox.Show("Не верно задан внешний радиус!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            if (InnerRadiusTextBox.Text == "" || InnerRadiusTextBox.Text == "." || InnerRadiusTextBox.Text == ",")
            {
                InnerRadiusLabel.BackColor = Color.Red;
                MessageBox.Show("Не верно задан внутренний радиус!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            if (HoleRadiusTextBox.Text == "" || HoleRadiusTextBox.Text == "." || HoleRadiusTextBox.Text == ",")
            {
                HoleRadiusLabel.BackColor = Color.Red;
                MessageBox.Show("Не верно задан радиус отверстия!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            if (DepthTextBox.Text == "" || DepthTextBox.Text == "." || DepthTextBox.Text == ",")
            {
                DepthLabel.BackColor = Color.Red;
                MessageBox.Show("Не верно задана толщина!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            if (CogsTextBox.Text == "")
            {
                CogsLabel.BackColor = Color.Red;
                MessageBox.Show("Не верно задано количество зубцов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            Model.Cogwheel cw = null;
            try
            {
                double tempOuterRadius = Convert.ToDouble(OuterRadiusTextBox.Text);
                double tempInnerRadius = Convert.ToDouble(InnerRadiusTextBox.Text);
                double tempHoleRadius = Convert.ToDouble(HoleRadiusTextBox.Text);
                double tempDepth = Convert.ToDouble(DepthTextBox.Text);
                int tempCogs = Convert.ToInt32(CogsTextBox.Text);
                cw = new Model.Cogwheel(tempInnerRadius, tempOuterRadius, tempHoleRadius, tempDepth, tempCogs);
            }
            catch (Model.Exceptions.CogwheelWrongOuterRadiusException ex)
            {
                OuterRadiusLabel.BackColor = Color.Red;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch (Model.Exceptions.CogwheelWrongInnerRadiusException ex)
            {
                InnerRadiusLabel.BackColor = Color.Red;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch (Model.Exceptions.CogwheelWrongHoleRadiusException ex)
            {
                HoleRadiusLabel.BackColor = Color.Red;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch (Model.Exceptions.CogwheelWrongDepthException ex)
            {
                DepthLabel.BackColor = Color.Red;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch (Model.Exceptions.CogwheelWrongCogsException ex)
            {
                CogsLabel.BackColor = Color.Red;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            catch (System.FormatException)
            {
                //этот случай обработан ифами выше!
            }

            if (cw != null)
            {
                Model.KompasWrapper kompasWrapper = new Model.KompasWrapper();
                bool retry = true;
                short tried = 0;
                while (retry)
                {
                    try
                    {
                        tried++;
                        kompasWrapper.StartKompas();
                        kompasWrapper.BuildCogwheel(cw);
                        retry = false;
                    }
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        kompasWrapper.ResetKompasObject();
                        if (tried > 3)
                        {
                            retry = false;
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
        }

        private void OuterRadiusTextBox_TextChanged(object sender, EventArgs e)
        {
            OuterRadiusLabel.BackColor = Control.DefaultBackColor;
        }

        private void InnerRadiusTextBox_TextChanged(object sender, EventArgs e)
        {
            InnerRadiusLabel.BackColor = Control.DefaultBackColor;
        }

        private void HoleRadiusTextBox_TextChanged(object sender, EventArgs e)
        {
            HoleRadiusLabel.BackColor = Control.DefaultBackColor;
        }

        private void DepthTextBox_TextChanged(object sender, EventArgs e)
        {
            DepthLabel.BackColor = Control.DefaultBackColor;
        }

        private void CogsTextBox_TextChanged(object sender, EventArgs e)
        {
            CogsLabel.BackColor = Control.DefaultBackColor;
        }

        public void IsNumberOrDotPressed(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsControl(e.KeyChar))
                && !(Char.IsDigit(e.KeyChar))
                && !((e.KeyChar == '.') 
                && (((TextBox)sender).Text.IndexOf(".") == -1) 
                && (((TextBox)sender).Text.IndexOf(",") == -1))
                && !((e.KeyChar == ',') 
                && (((TextBox)sender).Text.IndexOf(",") == -1) 
                && (((TextBox)sender).Text.IndexOf(".") == -1))
            )
            {
                e.Handled = true;
            }
        }

        public void IsNumberPressed(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsControl(e.KeyChar))
                && !(Char.IsDigit(e.KeyChar))
            )
            {
                e.Handled = true;
            }
        }

        private void ShowErrorMessage(Label label, string message)
        { 
            
        }
    }
}
