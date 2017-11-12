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
        private Model.KompasWrapper _kompasWrapper = new Model.KompasWrapper();
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
                ShowErrorMessage(OuterRadiusLabel, "Не верно задан внешний радиус!");
            }

            if (InnerRadiusTextBox.Text == "" || InnerRadiusTextBox.Text == "." || InnerRadiusTextBox.Text == ",")
            {
                ShowErrorMessage(InnerRadiusLabel, "Не верно задан внутренний радиус!");
            }

            if (HoleRadiusTextBox.Text == "" || HoleRadiusTextBox.Text == "." || HoleRadiusTextBox.Text == ",")
            {
                ShowErrorMessage(HoleRadiusLabel, "Не верно задан радиус отверстия!");
            }

            if (DepthTextBox.Text == "" || DepthTextBox.Text == "." || DepthTextBox.Text == ",")
            {
                ShowErrorMessage(DepthLabel, "Не верно задана толщина!");
            }

            if (CogsTextBox.Text == "")
            {
                ShowErrorMessage(CogsLabel, "Не верно задано количество зубцов!");
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
                ShowErrorMessage(OuterRadiusLabel, ex.Message);
            }
            catch (Model.Exceptions.CogwheelWrongInnerRadiusException ex)
            {
                ShowErrorMessage(InnerRadiusLabel, ex.Message);
            }
            catch (Model.Exceptions.CogwheelWrongHoleRadiusException ex)
            {
                ShowErrorMessage(HoleRadiusLabel, ex.Message);
            }
            catch (Model.Exceptions.CogwheelWrongDepthException ex)
            {
                ShowErrorMessage(DepthLabel, ex.Message);
            }
            catch (Model.Exceptions.CogwheelWrongCogsException ex)
            {
                ShowErrorMessage(CogsLabel, ex.Message);
            }
            catch (System.FormatException)
            {
                //этот случай обработан ифами выше!
            }

            if (cw != null)
            {
                _kompasWrapper.StartKompas();
                _kompasWrapper.BuildCogwheel(cw);
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
            label.BackColor = Color.Pink;
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
    }
}
