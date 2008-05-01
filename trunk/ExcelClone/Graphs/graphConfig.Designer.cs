namespace ExcelClone.Graphs
{
    partial class graphConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonAddColor = new System.Windows.Forms.Button();
            this.buttonRemoveColor = new System.Windows.Forms.Button();
            this.listBoxGraphColors = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxRename = new System.Windows.Forms.TextBox();
            this.buttonRenameSeries = new System.Windows.Forms.Button();
            this.listBoxGraphSeries = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkDrawLegend = new System.Windows.Forms.CheckBox();
            this.checkDrawTitle = new System.Windows.Forms.CheckBox();
            this.GraphTitleBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(470, 421);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonAddColor);
            this.tabPage1.Controls.Add(this.buttonRemoveColor);
            this.tabPage1.Controls.Add(this.listBoxGraphColors);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBoxRename);
            this.tabPage1.Controls.Add(this.buttonRenameSeries);
            this.tabPage1.Controls.Add(this.listBoxGraphSeries);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.checkDrawLegend);
            this.tabPage1.Controls.Add(this.checkDrawTitle);
            this.tabPage1.Controls.Add(this.GraphTitleBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(462, 395);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonAddColor
            // 
            this.buttonAddColor.Location = new System.Drawing.Point(229, 165);
            this.buttonAddColor.Name = "buttonAddColor";
            this.buttonAddColor.Size = new System.Drawing.Size(111, 23);
            this.buttonAddColor.TabIndex = 11;
            this.buttonAddColor.Text = "Add Color";
            this.buttonAddColor.UseVisualStyleBackColor = true;
            this.buttonAddColor.Click += new System.EventHandler(this.buttonAddColor_Click);
            // 
            // buttonRemoveColor
            // 
            this.buttonRemoveColor.Location = new System.Drawing.Point(346, 165);
            this.buttonRemoveColor.Name = "buttonRemoveColor";
            this.buttonRemoveColor.Size = new System.Drawing.Size(90, 23);
            this.buttonRemoveColor.TabIndex = 10;
            this.buttonRemoveColor.Text = "Remove Color";
            this.buttonRemoveColor.UseVisualStyleBackColor = true;
            this.buttonRemoveColor.Click += new System.EventHandler(this.buttonRemoveColor_Click);
            // 
            // listBoxGraphColors
            // 
            this.listBoxGraphColors.FormattingEnabled = true;
            this.listBoxGraphColors.Location = new System.Drawing.Point(228, 64);
            this.listBoxGraphColors.Name = "listBoxGraphColors";
            this.listBoxGraphColors.Size = new System.Drawing.Size(208, 95);
            this.listBoxGraphColors.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(225, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Graph Series Colors";
            // 
            // textBoxRename
            // 
            this.textBoxRename.Location = new System.Drawing.Point(91, 168);
            this.textBoxRename.Name = "textBoxRename";
            this.textBoxRename.Size = new System.Drawing.Size(131, 20);
            this.textBoxRename.TabIndex = 7;
            // 
            // buttonRenameSeries
            // 
            this.buttonRenameSeries.Location = new System.Drawing.Point(12, 165);
            this.buttonRenameSeries.Name = "buttonRenameSeries";
            this.buttonRenameSeries.Size = new System.Drawing.Size(73, 23);
            this.buttonRenameSeries.TabIndex = 6;
            this.buttonRenameSeries.Text = "Rename Series";
            this.buttonRenameSeries.UseVisualStyleBackColor = true;
            this.buttonRenameSeries.Click += new System.EventHandler(this.buttonRenameSeries_Click);
            // 
            // listBoxGraphSeries
            // 
            this.listBoxGraphSeries.ColumnWidth = 100;
            this.listBoxGraphSeries.FormattingEnabled = true;
            this.listBoxGraphSeries.Location = new System.Drawing.Point(12, 64);
            this.listBoxGraphSeries.Name = "listBoxGraphSeries";
            this.listBoxGraphSeries.Size = new System.Drawing.Size(210, 95);
            this.listBoxGraphSeries.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Graph Series Names";
            // 
            // checkDrawLegend
            // 
            this.checkDrawLegend.AutoSize = true;
            this.checkDrawLegend.Location = new System.Drawing.Point(346, 23);
            this.checkDrawLegend.Name = "checkDrawLegend";
            this.checkDrawLegend.Size = new System.Drawing.Size(90, 17);
            this.checkDrawLegend.TabIndex = 3;
            this.checkDrawLegend.Text = "Draw Legend";
            this.checkDrawLegend.UseVisualStyleBackColor = true;
            this.checkDrawLegend.CheckedChanged += new System.EventHandler(this.checkDrawLegend_CheckedChanged);
            // 
            // checkDrawTitle
            // 
            this.checkDrawTitle.AutoSize = true;
            this.checkDrawTitle.Location = new System.Drawing.Point(266, 22);
            this.checkDrawTitle.Name = "checkDrawTitle";
            this.checkDrawTitle.Size = new System.Drawing.Size(74, 17);
            this.checkDrawTitle.TabIndex = 2;
            this.checkDrawTitle.Text = "Draw Title";
            this.checkDrawTitle.UseVisualStyleBackColor = true;
            this.checkDrawTitle.CheckedChanged += new System.EventHandler(this.checkDrawGraphTitle_CheckedChanged);
            // 
            // GraphTitleBox
            // 
            this.GraphTitleBox.Location = new System.Drawing.Point(9, 20);
            this.GraphTitleBox.Name = "GraphTitleBox";
            this.GraphTitleBox.Size = new System.Drawing.Size(250, 20);
            this.GraphTitleBox.TabIndex = 1;
            this.GraphTitleBox.Text = "Create Title Label";
            this.GraphTitleBox.TextChanged += new System.EventHandler(this.GraphTitleBox_TextChanged);
            this.GraphTitleBox.Click += new System.EventHandler(this.GraphTitleBox_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Graph Title";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(462, 395);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Specific Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(462, 395);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Preview";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(407, 439);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(326, 439);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // graphConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 474);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "graphConfig";
            this.Text = "Form2";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkDrawTitle;
        private System.Windows.Forms.TextBox GraphTitleBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkDrawLegend;
        private System.Windows.Forms.Button buttonRemoveColor;
        private System.Windows.Forms.ListBox listBoxGraphColors;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxRename;
        private System.Windows.Forms.Button buttonRenameSeries;
        private System.Windows.Forms.ListBox listBoxGraphSeries;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonAddColor;
        private System.Windows.Forms.ColorDialog colorDialog1;

    }
}