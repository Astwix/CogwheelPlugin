using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CogwheelPlugin.Model.Exceptions;

namespace CogwheelPlugin
{
    public partial class MainForm : Form
    {
        private Model.KompasWrapper _kompasWrapper = new Model.KompasWrapper();
        private Dictionary<TextBox, Label> _bindTextboxToLabel = new Dictionary<TextBox, Label>();
        public MainForm()
        {
            InitializeComponent();
            _bindTextboxToLabel.Add(InnerRadiusTextBox, InnerRadiusLabel);
            _bindTextboxToLabel.Add(OuterRadiusTextBox, OuterRadiusLabel);
            _bindTextboxToLabel.Add(HoleRadiusTextBox, HoleRadiusLabel);
            _bindTextboxToLabel.Add(DepthTextBox, DepthLabel);
            _bindTextboxToLabel.Add(CogsTextBox, CogsLabel);
            _bindTextboxToLabel.Add(ExtrudeCountTextBox, ExtrudeCountLabel);
            InnerRadiusTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            OuterRadiusTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            HoleRadiusTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            DepthTextBox.KeyPress += new KeyPressEventHandler(IsNumberOrDotPressed);
            CogsTextBox.KeyPress += new KeyPressEventHandler(IsNumberPressed);
            ExtrudeCountTextBox.KeyPress += new KeyPressEventHandler(IsNumberPressed);
            OuterRadiusTextBox.Select();
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            Model.Cogwheel cw = null;
            try
            {
                double tempOuterRadius = Convert.ToDouble(OuterRadiusTextBox.Text);
                double tempInnerRadius = Convert.ToDouble(InnerRadiusTextBox.Text);
                double tempHoleRadius = Convert.ToDouble(HoleRadiusTextBox.Text);
                double tempDepth = Convert.ToDouble(DepthTextBox.Text);
                int tempCogs = Convert.ToInt32(CogsTextBox.Text);
                int extrudeCount = Convert.ToInt32(ExtrudeCountTextBox.Text);
                Model.ExtrudeType typeOfExtrude = (Model.ExtrudeType)ExtrudeTypeComboBox.SelectedIndex;
                cw = new Model.Cogwheel(tempInnerRadius, tempOuterRadius,
                    tempHoleRadius, tempDepth, tempCogs, typeOfExtrude, extrudeCount);
            }
            catch (CogwheelWrongOuterRadiusException ex)
            {
                ShowErrorMessage(OuterRadiusLabel, ex.Message);
            }
            catch (CogwheelWrongInnerRadiusException ex)
            {
                ShowErrorMessage(InnerRadiusLabel, ex.Message);
            }
            catch (CogwheelWrongHoleRadiusException ex)
            {
                ShowErrorMessage(HoleRadiusLabel, ex.Message);
            }
            catch (CogwheelWrongDepthException ex)
            {
                ShowErrorMessage(DepthLabel, ex.Message);
            }
            catch (CogwheelWrongCogsException ex)
            {
                ShowErrorMessage(CogsLabel, ex.Message);
            }
            catch (CogwheelWrongExtrudeCountException ex)
            {
                ShowErrorMessage(ExtrudeCountLabel, ex.Message);
            }
            catch (FormatException)
            {
                //этот случай обработан ифами выше!
            }

            if (cw != null)
            {
                _kompasWrapper.StartKompas();
                _kompasWrapper.BuildCogwheel(cw);
            }
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

        private void TextboxValidation(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == ""
                || textbox.Text == "."
                || textbox.Text == ",")
            {
                ShowErrorMessage(_bindTextboxToLabel[textbox], "Ошибка в значении параметра!");
                textbox.Focus();
            }
        }

        private void ResetBackColor(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            _bindTextboxToLabel[textbox].BackColor = DefaultBackColor;
        }
    }
}
