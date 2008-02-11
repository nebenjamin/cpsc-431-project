namespace FunctionsTeamSandbox
{
    partial class Form1
    {
        public Cell A1 = new Cell();
        public Cell A2 = new Cell();
        public Cell B1 = new Cell();
        public Cell B2 = new Cell();

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
            A1_TB = new System.Windows.Forms.TextBox();
            A2_TB = new System.Windows.Forms.TextBox();
            B1_TB = new System.Windows.Forms.TextBox();
            B2_TB = new System.Windows.Forms.TextBox();
            //this.Func_TB = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            //this.label5 = new System.Windows.Forms.Label();
            //this.calculate = new System.Windows.Forms.Button();
            A1_Formula = new System.Windows.Forms.TextBox();
            A1_Value = new System.Windows.Forms.TextBox();
            A2_Value = new System.Windows.Forms.TextBox();
            A2_Formula = new System.Windows.Forms.TextBox();
            B1_Value = new System.Windows.Forms.TextBox();
            B1_Formula = new System.Windows.Forms.TextBox();
            B2_Value = new System.Windows.Forms.TextBox();
            B2_Formula = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // A1_TB
            // 
            A1_TB.Location = new System.Drawing.Point(47, 20);
            A1_TB.Name = "A1_TB";
            A1_TB.Size = new System.Drawing.Size(103, 20);
            A1_TB.TabIndex = 0;
            A1_TB.Enter += new System.EventHandler(this.A1_TB_Enter);
            A1_TB.Leave += new System.EventHandler(this.A1_TB_Leave);
            // 
            // A2_TB
            // 
            A2_TB.Location = new System.Drawing.Point(156, 20);
            A2_TB.Name = "A2_TB";
            A2_TB.Size = new System.Drawing.Size(103, 20);
            A2_TB.TabIndex = 2;
            A2_TB.Enter += new System.EventHandler(this.A2_TB_Enter);
            A2_TB.Leave += new System.EventHandler(this.A2_TB_Leave);
            // 
            // B1_TB
            // 
            B1_TB.Location = new System.Drawing.Point(47, 73);
            B1_TB.Name = "B1_TB";
            B1_TB.Size = new System.Drawing.Size(103, 20);
            B1_TB.TabIndex = 3;
            B1_TB.Enter += new System.EventHandler(this.B1_TB_Enter);
            B1_TB.Leave += new System.EventHandler(this.B1_TB_Leave);
            // 
            // B2_TB
            // 
            B2_TB.Location = new System.Drawing.Point(156, 73);
            B2_TB.Name = "B2_TB";
            B2_TB.Size = new System.Drawing.Size(103, 20);
            B2_TB.TabIndex = 4;
            B2_TB.Enter += new System.EventHandler(this.B2_TB_Enter);
            B2_TB.Leave += new System.EventHandler(this.B2_TB_Leave);
            // 
            // Func_TB
            // 
            /*this.Func_TB.Location = new System.Drawing.Point(156, 236);
            this.Func_TB.Name = "Func_TB";
            this.Func_TB.Size = new System.Drawing.Size(103, 20);
            this.Func_TB.TabIndex = 5;*/
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(27, 27);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(14, 13);
            label1.TabIndex = 6;
            label1.Text = "A";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(27, 80);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(14, 13);
            label2.TabIndex = 7;
            label2.Text = "B";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(92, 4);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(13, 13);
            label3.TabIndex = 8;
            label3.Text = "1";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(202, 4);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(13, 13);
            label4.TabIndex = 9;
            label4.Text = "2";
            // 
            // label5
            // 
            /*this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(102, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Function";*/
            // 
            // calculate
            // 
            /*this.calculate.Location = new System.Drawing.Point(37, 262);
            this.calculate.Name = "calculate";
            this.calculate.Size = new System.Drawing.Size(222, 52);
            this.calculate.TabIndex = 11;
            this.calculate.Text = "button1";
            this.calculate.UseVisualStyleBackColor = true;
            this.calculate.Click += new System.EventHandler(this.calculate_Click);*/
            // 
            // A1_Formula
            // 
            A1_Formula.Location = new System.Drawing.Point(47, 47);
            A1_Formula.Name = "A1_Formula";
            A1_Formula.ReadOnly = true;
            A1_Formula.Size = new System.Drawing.Size(45, 20);
            A1_Formula.TabIndex = 12;
            // 
            // A1_Value
            // 
            A1_Value.Location = new System.Drawing.Point(106, 47);
            A1_Value.Name = "A1_Value";
            A1_Value.ReadOnly = true;
            A1_Value.Size = new System.Drawing.Size(44, 20);
            A1_Value.TabIndex = 13;
            // 
            // A2_Value
            // 
            A2_Value.Location = new System.Drawing.Point(215, 46);
            A2_Value.Name = "A2_Value";
            A2_Value.ReadOnly = true;
            A2_Value.Size = new System.Drawing.Size(44, 20);
            A2_Value.TabIndex = 15;
            // 
            // A2_Formula
            // 
            A2_Formula.Location = new System.Drawing.Point(156, 46);
            A2_Formula.Name = "A2_Formula";
            A2_Formula.ReadOnly = true;
            A2_Formula.Size = new System.Drawing.Size(45, 20);
            A2_Formula.TabIndex = 14;
            // 
            // B1_Value
            // 
            B1_Value.Location = new System.Drawing.Point(106, 99);
            B1_Value.Name = "B1_Value";
            B1_Value.ReadOnly = true;
            B1_Value.Size = new System.Drawing.Size(44, 20);
            B1_Value.TabIndex = 17;
            // 
            // B1_Formula
            // 
            B1_Formula.Location = new System.Drawing.Point(47, 99);
            B1_Formula.Name = "B1_Formula";
            B1_Formula.ReadOnly = true;
            B1_Formula.Size = new System.Drawing.Size(45, 20);
            B1_Formula.TabIndex = 16;
            // 
            // B2_Value
            // 
            B2_Value.Location = new System.Drawing.Point(215, 99);
            B2_Value.Name = "B2_Value";
            B2_Value.ReadOnly = true;
            B2_Value.Size = new System.Drawing.Size(44, 20);
            B2_Value.TabIndex = 19;
            // 
            // B2_Formula
            // 
            B2_Formula.Location = new System.Drawing.Point(156, 99);
            B2_Formula.Name = "B2_Formula";
            B2_Formula.ReadOnly = true;
            B2_Formula.Size = new System.Drawing.Size(45, 20);
            B2_Formula.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 344);
            this.Controls.Add(B2_Value);
            this.Controls.Add(B2_Formula);
            this.Controls.Add(B1_Value);
            this.Controls.Add(B1_Formula);
            this.Controls.Add(A2_Value);
            this.Controls.Add(A2_Formula);
            this.Controls.Add(A1_Value);
            this.Controls.Add(A1_Formula);
            //this.Controls.Add(this.calculate);
            //this.Controls.Add(this.label5);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            //this.Controls.Add(this.Func_TB);
            this.Controls.Add(B2_TB);
            this.Controls.Add(B1_TB);
            this.Controls.Add(A2_TB);
            this.Controls.Add(A1_TB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private static System.Windows.Forms.TextBox A1_TB;
        private static System.Windows.Forms.TextBox B1_TB;
        private static System.Windows.Forms.TextBox A2_TB;
        private static System.Windows.Forms.TextBox B2_TB;
        //private System.Windows.Forms.TextBox Func_TB;
        private static System.Windows.Forms.Label label1;
        private static System.Windows.Forms.Label label2;
        private static System.Windows.Forms.Label label3;
        private static System.Windows.Forms.Label label4;
        //private System.Windows.Forms.Label label5;
        //private System.Windows.Forms.Button calculate;
        private static System.Windows.Forms.TextBox A1_Formula;
        private static System.Windows.Forms.TextBox A1_Value;
        private static System.Windows.Forms.TextBox A2_Value;
        private static System.Windows.Forms.TextBox A2_Formula;
        private static System.Windows.Forms.TextBox B1_Value;
        private static System.Windows.Forms.TextBox B1_Formula;
        private static System.Windows.Forms.TextBox B2_Value;
        private static System.Windows.Forms.TextBox B2_Formula;

        public static string A1Val   // the Name property
        {
            get { return A1_Formula.Text; }
            set { A1_Formula.Text = value; }
        }

        public static string B1Val   // the Name property
        {
            get { return B1_Formula.Text; }
            set { B1_Formula.Text = value; }
        }

        public static string A2Val   // the Name property
        {
            get { return A2_Formula.Text; }
            set { A2_Formula.Text = value; }
        }
      
        public static string B2Val   // the Name property
        {
            get { return B2_Formula.Text; }
            set { B2_Formula.Text = value; }
        }

        public static string getCellFormula(string cell_ref)
        {
            string toReturn;
            switch (cell_ref)
            {
                case "A1":
                    toReturn = A1Val;
                    break;
                case "A2":
                    toReturn = A2Val;
                    break;
                case "B1":
                    toReturn = B1Val;
                    break;
                case "B2":
                    toReturn = B2Val;
                    break;
                default:
                    toReturn = "Error";
                    break;
            }
            if (toReturn == "" || toReturn == null)
            {
                toReturn = "Error";
            }
            return toReturn;
        }
        /*public string Formula   // the Name property
        {
            get { return Func_TB.Text; }
            set { Func_TB.Text = value; }
        }*/
    }
}

