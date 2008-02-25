using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionsTeamSandbox
{
    public class Cell
    {
        private string Cell_Formula;
        private string Cell_Value;

        public string Formula
        {
            get { return Cell_Formula; }
            set { Cell_Formula = value; }
        }

        public string Value
        {
            get { return Cell_Value; }
            set { Cell_Value = value; }
        }
    }
}
