namespace FunctionsTeamSandbox
{
    partial class Form1
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
            this.A1_TB = new System.Windows.Forms.TextBox();
            this.A2_TB = new System.Windows.Forms.TextBox();
            this.B1_TB = new System.Windows.Forms.TextBox();
            this.B2_TB = new System.Windows.Forms.TextBox();
            this.Func_TB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.calculate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // A1_TB
            // 
            this.A1_TB.Location = new System.Drawing.Point(47, 20);
            this.A1_TB.Name = "A1_TB";
            this.A1_TB.Size = new System.Drawing.Size(103, 20);
            this.A1_TB.TabIndex = 0;
            // 
            // A2_TB
            // 
            this.A2_TB.Location = new System.Drawing.Point(156, 20);
            this.A2_TB.Name = "A2_TB";
            this.A2_TB.Size = new System.Drawing.Size(103, 20);
            this.A2_TB.TabIndex = 2;
            // 
            // B1_TB
            // 
            this.B1_TB.Location = new System.Drawing.Point(47, 46);
            this.B1_TB.Name = "B1_TB";
            this.B1_TB.Size = new System.Drawing.Size(103, 20);
            this.B1_TB.TabIndex = 3;
            // 
            // B2_TB
            // 
            this.B2_TB.Location = new System.Drawing.Point(156, 46);
            this.B2_TB.Name = "B2_TB";
            this.B2_TB.Size = new System.Drawing.Size(103, 20);
            this.B2_TB.TabIndex = 4;
            // 
            // Func_TB
            // 
            this.Func_TB.Location = new System.Drawing.Point(156, 72);
            this.Func_TB.Name = "Func_TB";
            this.Func_TB.Size = new System.Drawing.Size(103, 20);
            this.Func_TB.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "B";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(199, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(90, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Function";
            // 
            // calculate
            // 
            this.calculate.Location = new System.Drawing.Point(36, 120);
            this.calculate.Name = "calculate";
            this.calculate.Size = new System.Drawing.Size(222, 52);
            this.calculate.TabIndex = 11;
            this.calculate.Text = "button1";
            this.calculate.UseVisualStyleBackColor = true;
            this.calculate.Click += new System.EventHandler(this.calculate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 186);
            this.Controls.Add(this.calculate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Func_TB);
            this.Controls.Add(this.B2_TB);
            this.Controls.Add(this.B1_TB);
            this.Controls.Add(this.A2_TB);
            this.Controls.Add(this.A1_TB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox A1_TB;
        private System.Windows.Forms.TextBox B1_TB;
        private System.Windows.Forms.TextBox A2_TB;
        private System.Windows.Forms.TextBox B2_TB;
        private System.Windows.Forms.TextBox Func_TB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button calculate;
        
        public string A1   // the Name property
        {
            get { return A1_TB.Text; }
            set { A1_TB.Text = value; }
        }

        public string B1   // the Name property
        {
            get { return B1_TB.Text; }
            set { B1_TB.Text = value; }
        }

        public string A2   // the Name property
        {
            get { return A2_TB.Text; }
            set { A2_TB.Text = value; }
        }
      
        public string B2   // the Name property
        {
            get { return B2_TB.Text; }
            set { B2_TB.Text = value; }
        }

        public string Formula   // the Name property
        {
            get { return Func_TB.Text; }
            set { Func_TB.Text = value; }
        }
    }
}

