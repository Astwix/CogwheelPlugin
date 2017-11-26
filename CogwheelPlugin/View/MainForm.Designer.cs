namespace CogwheelPlugin
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BuildButton = new System.Windows.Forms.Button();
            this.OuterRadiusLabel = new System.Windows.Forms.Label();
            this.InnerRadiusLabel = new System.Windows.Forms.Label();
            this.HoleRadiusLabel = new System.Windows.Forms.Label();
            this.DepthLabel = new System.Windows.Forms.Label();
            this.CogsLabel = new System.Windows.Forms.Label();
            this.OuterRadiusTextBox = new System.Windows.Forms.TextBox();
            this.InnerRadiusTextBox = new System.Windows.Forms.TextBox();
            this.HoleRadiusTextBox = new System.Windows.Forms.TextBox();
            this.DepthTextBox = new System.Windows.Forms.TextBox();
            this.CogsTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BuildButton
            // 
            this.BuildButton.Location = new System.Drawing.Point(237, 179);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(100, 23);
            this.BuildButton.TabIndex = 11;
            this.BuildButton.Text = "Построить";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // OuterRadiusLabel
            // 
            this.OuterRadiusLabel.AutoSize = true;
            this.OuterRadiusLabel.Location = new System.Drawing.Point(6, 28);
            this.OuterRadiusLabel.Name = "OuterRadiusLabel";
            this.OuterRadiusLabel.Size = new System.Drawing.Size(112, 13);
            this.OuterRadiusLabel.TabIndex = 1;
            this.OuterRadiusLabel.Text = "Внешний радиус, мм";
            // 
            // InnerRadiusLabel
            // 
            this.InnerRadiusLabel.AutoSize = true;
            this.InnerRadiusLabel.Location = new System.Drawing.Point(6, 54);
            this.InnerRadiusLabel.Name = "InnerRadiusLabel";
            this.InnerRadiusLabel.Size = new System.Drawing.Size(126, 13);
            this.InnerRadiusLabel.TabIndex = 2;
            this.InnerRadiusLabel.Text = "Внутренний радиус, мм";
            // 
            // HoleRadiusLabel
            // 
            this.HoleRadiusLabel.AutoSize = true;
            this.HoleRadiusLabel.Location = new System.Drawing.Point(6, 80);
            this.HoleRadiusLabel.Name = "HoleRadiusLabel";
            this.HoleRadiusLabel.Size = new System.Drawing.Size(193, 13);
            this.HoleRadiusLabel.TabIndex = 3;
            this.HoleRadiusLabel.Text = "Радиус центрального отверстия, мм";
            // 
            // DepthLabel
            // 
            this.DepthLabel.AutoSize = true;
            this.DepthLabel.Location = new System.Drawing.Point(6, 106);
            this.DepthLabel.Name = "DepthLabel";
            this.DepthLabel.Size = new System.Drawing.Size(75, 13);
            this.DepthLabel.TabIndex = 4;
            this.DepthLabel.Text = "Толщина, мм";
            // 
            // CogsLabel
            // 
            this.CogsLabel.AutoSize = true;
            this.CogsLabel.Location = new System.Drawing.Point(18, 153);
            this.CogsLabel.Name = "CogsLabel";
            this.CogsLabel.Size = new System.Drawing.Size(104, 13);
            this.CogsLabel.TabIndex = 5;
            this.CogsLabel.Text = "Количество зубцов";
            // 
            // OuterRadiusTextBox
            // 
            this.OuterRadiusTextBox.Location = new System.Drawing.Point(225, 28);
            this.OuterRadiusTextBox.Name = "OuterRadiusTextBox";
            this.OuterRadiusTextBox.Size = new System.Drawing.Size(100, 20);
            this.OuterRadiusTextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.OuterRadiusTextBox, "[0,3; 100]");
            this.OuterRadiusTextBox.TextChanged += new System.EventHandler(this.ResetBackColor);
            this.OuterRadiusTextBox.Leave += new System.EventHandler(this.TextboxValidation);
            // 
            // InnerRadiusTextBox
            // 
            this.InnerRadiusTextBox.Location = new System.Drawing.Point(225, 54);
            this.InnerRadiusTextBox.Name = "InnerRadiusTextBox";
            this.InnerRadiusTextBox.Size = new System.Drawing.Size(100, 20);
            this.InnerRadiusTextBox.TabIndex = 7;
            this.toolTip1.SetToolTip(this.InnerRadiusTextBox, "[0,2; 100)");
            this.InnerRadiusTextBox.TextChanged += new System.EventHandler(this.ResetBackColor);
            this.InnerRadiusTextBox.Leave += new System.EventHandler(this.TextboxValidation);
            // 
            // HoleRadiusTextBox
            // 
            this.HoleRadiusTextBox.Location = new System.Drawing.Point(225, 80);
            this.HoleRadiusTextBox.Name = "HoleRadiusTextBox";
            this.HoleRadiusTextBox.Size = new System.Drawing.Size(100, 20);
            this.HoleRadiusTextBox.TabIndex = 8;
            this.toolTip1.SetToolTip(this.HoleRadiusTextBox, "[0,1; 30]");
            this.HoleRadiusTextBox.TextChanged += new System.EventHandler(this.ResetBackColor);
            this.HoleRadiusTextBox.Leave += new System.EventHandler(this.TextboxValidation);
            // 
            // DepthTextBox
            // 
            this.DepthTextBox.Location = new System.Drawing.Point(225, 106);
            this.DepthTextBox.Name = "DepthTextBox";
            this.DepthTextBox.Size = new System.Drawing.Size(100, 20);
            this.DepthTextBox.TabIndex = 9;
            this.toolTip1.SetToolTip(this.DepthTextBox, "[0,5; 70]");
            this.DepthTextBox.TextChanged += new System.EventHandler(this.ResetBackColor);
            this.DepthTextBox.Leave += new System.EventHandler(this.TextboxValidation);
            // 
            // CogsTextBox
            // 
            this.CogsTextBox.Location = new System.Drawing.Point(237, 153);
            this.CogsTextBox.Name = "CogsTextBox";
            this.CogsTextBox.Size = new System.Drawing.Size(100, 20);
            this.CogsTextBox.TabIndex = 10;
            this.toolTip1.SetToolTip(this.CogsTextBox, "[5; 30]\r\n");
            this.CogsTextBox.TextChanged += new System.EventHandler(this.ResetBackColor);
            this.CogsTextBox.Leave += new System.EventHandler(this.TextboxValidation);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OuterRadiusLabel);
            this.groupBox1.Controls.Add(this.DepthTextBox);
            this.groupBox1.Controls.Add(this.DepthLabel);
            this.groupBox1.Controls.Add(this.OuterRadiusTextBox);
            this.groupBox1.Controls.Add(this.HoleRadiusTextBox);
            this.groupBox1.Controls.Add(this.InnerRadiusTextBox);
            this.groupBox1.Controls.Add(this.HoleRadiusLabel);
            this.groupBox1.Controls.Add(this.InnerRadiusLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 135);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Размеры";
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Подсказка";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 213);
            this.Controls.Add(this.CogsTextBox);
            this.Controls.Add(this.CogsLabel);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Зубчатое колесо";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BuildButton;
        private System.Windows.Forms.Label OuterRadiusLabel;
        private System.Windows.Forms.Label InnerRadiusLabel;
        private System.Windows.Forms.Label HoleRadiusLabel;
        private System.Windows.Forms.Label DepthLabel;
        private System.Windows.Forms.Label CogsLabel;
        private System.Windows.Forms.TextBox OuterRadiusTextBox;
        private System.Windows.Forms.TextBox InnerRadiusTextBox;
        private System.Windows.Forms.TextBox HoleRadiusTextBox;
        private System.Windows.Forms.TextBox DepthTextBox;
        private System.Windows.Forms.TextBox CogsTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

