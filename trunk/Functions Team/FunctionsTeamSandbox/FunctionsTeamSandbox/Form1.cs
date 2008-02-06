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
        Parser Excel_Parser = new Parser();
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
            /*try
            {
                return Convert.ToString(Convert.ToInt32(A1, 10) + Convert.ToInt32(A2, 10) + Convert.ToInt32(B1, 10) + Convert.ToInt32(B2, 10));
            }
            catch// (System.ArgumentException e)
            {
                return "#ARGUMENT";
            }
            catch (System.FormatException e)
            {
                return "#FORMAT";
            }
            catch (System.OverflowException e)
            {*/
                return "#OVERFLOW";
            //}
        }

        private void A1_TB_Enter(object sender, EventArgs e)
        {
            A1_TB.Text = A1.Formula;
        }

        private void A1_TB_Leave(object sender, EventArgs e)
        {
            A1_Formula.Text = A1_TB.Text;
            A1.Formula = A1_Formula.Text;

            A1_Value.Text = Excel_Parser.Parse("A1:" + A1_Formula.Text);
            A1.Value = A1_Value.Text;
            A1_TB.Text = A1.Value;
        }

        private void A2_TB_Enter(object sender, EventArgs e)
        {
            A2_TB.Text = A2.Formula;
        }

        private void A2_TB_Leave(object sender, EventArgs e)
        {
            A2_Formula.Text = A2_TB.Text;
            A2.Formula = A2_Formula.Text;

            A2_Value.Text = Excel_Parser.Parse("A2:" + A2_Formula.Text);
            A2.Value = A2_Value.Text;
            A2_TB.Text = A2.Value;
        }

        private void B1_TB_Enter(object sender, EventArgs e)
        {
            B1_TB.Text = B1.Formula;
        }

        private void B1_TB_Leave(object sender, EventArgs e)
        {
            B1_Formula.Text = B1_TB.Text;
            B1.Formula = B1_Formula.Text;

            B1_Value.Text = Excel_Parser.Parse("B1:" + B1_Formula.Text);
            B1.Value = B1_Value.Text;
            B1_TB.Text = B1.Value;
        }

        private void B2_TB_Enter(object sender, EventArgs e)
        {
            B2_TB.Text = B2.Formula;
        }

        private void B2_TB_Leave(object sender, EventArgs e)
        {
            B2_Formula.Text = B2_TB.Text;
            B2.Formula = B2_Formula.Text;

            B2_Value.Text = Excel_Parser.Parse("B2:" + B2_Formula.Text);
            B2.Value = B2_Value.Text;
            B2_TB.Text = B2.Value;
        }
    }
}