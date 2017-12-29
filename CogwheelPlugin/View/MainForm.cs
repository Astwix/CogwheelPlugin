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
    /// <summary>
    /// Главная форма программы
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Объект связи с КОМПАС
        /// </summary>
        private Model.KompasWrapper _kompasWrapper = new Model.KompasWrapper();
        
        /// <summary>
        /// Словарь, привязка textbox к label
        /// </summary>
        private Dictionary<TextBox, Label> _bindTextboxToLabel = 
            new Dictionary<TextBox, Label>();

        /// <summary>
        /// Главная форма
        /// </summary>
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
            ExtrudeTypeComboBox.SelectedIndex = 0;
            ExtrudeCountTextBox.Enabled = false;
            OuterRadiusTextBox.Select();
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            Model.Cogwheel cogwheel = null;
            try
            {
                double tempOuterRadius = Convert.ToDouble(OuterRadiusTextBox.Text);
                double tempInnerRadius = Convert.ToDouble(InnerRadiusTextBox.Text);
                double tempHoleRadius = Convert.ToDouble(HoleRadiusTextBox.Text);
                double tempDepth = Convert.ToDouble(DepthTextBox.Text);
                int tempCogs = Convert.ToInt32(CogsTextBox.Text);
                int tempExtrudeCount = Convert.ToInt32(ExtrudeCountTextBox.Text);
                ExtrudeType tempTypeOfExtrude = 
                    (ExtrudeType)ExtrudeTypeComboBox.SelectedIndex;
                cogwheel = new Model.Cogwheel(tempInnerRadius, 
                    tempOuterRadius, tempHoleRadius, tempDepth, 
                    tempCogs, tempTypeOfExtrude, tempExtrudeCount);
            }
            catch (CogwheelWrongOuterRadiusException exception)
            {
                ShowErrorMessage(OuterRadiusLabel, exception.Message);
            }
            catch (CogwheelWrongInnerRadiusException exception)
            {
                ShowErrorMessage(InnerRadiusLabel, exception.Message);
            }
            catch (CogwheelWrongHoleRadiusException exception)
            {
                ShowErrorMessage(HoleRadiusLabel, exception.Message);
            }
            catch (CogwheelWrongDepthException exception)
            {
                ShowErrorMessage(DepthLabel, exception.Message);
            }
            catch (CogwheelWrongCogsException exception)
            {
                ShowErrorMessage(CogsLabel, exception.Message);
            }
            catch (CogwheelWrongExtrudeCountException exception)
            {
                ShowErrorMessage(ExtrudeCountLabel, exception.Message);
            }
            catch (FormatException)
            {
                ShowErrorMessage(null, "Заданы не все параметры!");
            }

            if (cogwheel != null)
            {
                _kompasWrapper.StartKompas();
                _kompasWrapper.BuildCogwheel(cogwheel);
            }
        }

        /// <summary>
        /// Событие, проверяеющее, чтобы textbox содержал 
        /// максимум один знак разделения: точка или запятая
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Событие, проверяющее, чтобы textbox содержал 
        /// только цифры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IsNumberPressed(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsControl(e.KeyChar))
                && !(Char.IsDigit(e.KeyChar))
            )
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Вывод сообщения об ошибке и подсветка соответствующего label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="message"></param>
        private void ShowErrorMessage(Label label, string message)
        {
            if (label != null)
            {
                label.BackColor = Color.Pink;
            }
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, 
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        /// <summary>
        /// Проверка textbox на неверное значение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextboxValidation(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            if (textbox.Text == ""
                || textbox.Text == "."
                || textbox.Text == ",")
            {
                ShowErrorMessage(_bindTextboxToLabel[textbox], 
                    "Ошибка в значении параметра!");
                textbox.Focus();
            }
        }

        /// <summary>
        /// Возврат textbox исходного цвета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBackColor(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            _bindTextboxToLabel[textbox].BackColor = DefaultBackColor;
        }

        /// <summary>
        /// Возможность заполнения "количество вырезов"
        /// в соответствии с выбранным "тип выреза"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtrudeTypeComboBox_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (ExtrudeTypeComboBox.SelectedIndex == 0)
            {
                ExtrudeCountTextBox.Enabled = false;
            }
            else
            {
                ExtrudeCountTextBox.Enabled = true;
            }
        }
    }
}
