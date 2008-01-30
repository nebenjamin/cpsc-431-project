using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FunctionsTeamSandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.Text = "Functions Team Sandbox";
            InitializeComponent();
        }

        private void calculate_Click(object sender, EventArgs e)
        {
            Formula = evaluateFunction(Formula);
        }

        string evaluateFunction(string s)
        {
            try
            {
                return Convert.ToString(Convert.ToInt32(A1, 10) + Convert.ToInt32(A2, 10) + Convert.ToInt32(B1, 10) + Convert.ToInt32(B2, 10));
            }
            catch (System.ArgumentException e)
            {
                return "#ARGUMENT";
            }
            catch (System.FormatException e)
            {
                return "#FORMAT";
            }
            catch (System.OverflowException e)
            {
                return "#OVERFLOW";
            }
        }
        
    }
}