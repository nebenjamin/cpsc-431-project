using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FunctionParser
{
    class Functions
    {
        private ArrayList Function_List = new ArrayList();

        public ArrayList Get_Functions()
        {
            return Function_List;
        }

        public Functions()
        {
            Function_List.Add("ADD");
            Function_List.Add("SUB");
            Function_List.Add("MUL");
            Function_List.Add("DIV");
            Function_List.Add("AVG"); //5
            Function_List.Add("MAX");
            Function_List.Add("MIN");
            Function_List.Add("COUNT");
            Function_List.Add("ABS");
            Function_List.Add("LOG10"); //10
            Function_List.Add("LOG");
            Function_List.Add("LN");
            Function_List.Add("ROUND");
            Function_List.Add("COS");
            Function_List.Add("SIN"); //15
            Function_List.Add("TAN");
            Function_List.Add("POW");
            Function_List.Add("SQRT");
            Function_List.Add("DEGREES");
            Function_List.Add("RADIANS"); //20
            Function_List.Add("PI");
            //Median
            //Random (0<=x<=1)
            //Round Down
            //Round UP
            //To Upper Case
            //To Lower Case
        }

        public string CallFunction(ArrayList Cell_String)
        {
            try
            {
                switch (Cell_String[0].ToString().ToUpper())
                {
                    case "ADD":
                        return Add(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "SUB":
                        return Subtract(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "MUL":
                        return Multiply(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "DIV":
                        return Divide(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "AVG":
                        return Average(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "MIN":
                        return Minimum(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "MAX":
                        return Maximum(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "COUNT":
                        return Count(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "ABS":
                        return Absolute(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "LOG10":
                        return Log10(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "LOG":
                        return Log(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "LN":
                        return LN(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "ROUND":
                        return Round(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "COS":
                        return Cos(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "SIN":
                        return Sin(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "TAN":
                        return Tan(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "POW":
                        return Power(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "SQRT":
                        return SquareRoot(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "DEGREES":
                        return Degrees(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "RADIANS":
                        return Radians(Cell_String.GetRange(1, Cell_String.Count - 1));
                    case "PI":
                        return PI();
                    default:
                        string temp = "";
                        for (int i = 0; i < Cell_String.Count; i++)
                            temp += Cell_String[i].ToString();

                        return temp;
                }
            }
            catch
            {
                string temp = "";
                for (int i = 0; i < Cell_String.Count; i++)
                    temp += Cell_String[i].ToString();

                return temp;
            }
        }

        public bool IsFunction(string Function)
        {
            //Check string against list of available functions
            if (Function_List.Contains(Function.ToUpper()))
                return true;
            else
                return false;
        }

        private string Add(ArrayList Arguments) //Adds Argument1 and Argument2
        {
            double total = 0;
            for (int i = 1; i < Arguments.Count; i = i + 2)
                total += Convert.ToDouble(Arguments[i]);

            return total.ToString();
        }

        private string Subtract(ArrayList Arguments) //Subtracts Argument2 from Argument1
        {
            double total = Convert.ToDouble(Arguments[1]);
            for (int i = 3; i < Arguments.Count; i = i + 2)
                total -= Convert.ToDouble(Arguments[i]);

            return total.ToString();
        }

        private string Multiply(ArrayList Arguments) //Multiplies Argument1 by Argument2
        {
            double total = Convert.ToDouble(Arguments[1]);
            for (int i = 3; i < Arguments.Count; i = i + 2)
                total *= Convert.ToDouble(Arguments[i]);

            return total.ToString();
        }

        private string Divide(ArrayList Arguments) //Divides Argument1 by Argument2
        {
            double total = Convert.ToDouble(Arguments[1]);
            for (int i = 3; i < Arguments.Count; i = i + 2)
                total /= Convert.ToDouble(Arguments[i]);

            return total.ToString();
        }

        private string Average(ArrayList Arguments) //Finds the average out of all arguments passed
        {
            int count = 0;
            double total = 0;
            for (int i = 1; i < Arguments.Count; i = i + 2)
            {
                count++;
                total += Convert.ToDouble(Arguments[i]);
            }
            total /= count;

            return total.ToString();
        }

        private string Minimum(ArrayList Arguments) //Finds the minimum number out of all arguments passed
        {
            double total = Convert.ToDouble(Arguments[1]);
            for (int i = 3; i < Arguments.Count; i = i + 2)
                if (Convert.ToDouble(Arguments[i]) < total)
                    total = Convert.ToDouble(Arguments[i]);

            return total.ToString();
        }

        private string Maximum(ArrayList Arguments) //Finds the maximum number out of all arguments passed
        {
            double total = Convert.ToDouble(Arguments[1]);
            for (int i = 3; i < Arguments.Count; i = i + 2)
                if (Convert.ToDouble(Arguments[i]) > total)
                    total = Convert.ToDouble(Arguments[i]);

            return total.ToString();
        }

        private string Count(ArrayList Arguments) //Counts the number of arguments
        {
            double total = 0;
            for (int i = 1; i < Arguments.Count; i = i + 2)
                total++;

            return total.ToString();
        }

        private string Absolute(ArrayList Arguments) //Absolute value of Argument1
        {
            return Math.Abs(Convert.ToDouble(Arguments[1])).ToString();
        }

        private string Log10(ArrayList Arguments) //Log of Argument1 to base 10
        {
            return Math.Log10(Convert.ToDouble(Arguments[1])).ToString();
        }

        private string Log(ArrayList Arguments) //Log of Argument1 to base of Argument2
        {
            return Math.Log(Convert.ToDouble(Arguments[1]), Convert.ToDouble(Arguments[3])).ToString();
        }

        private string LN(ArrayList Arguments) //Log of Argument1 to base E
        {
            return Math.Log(Convert.ToDouble(Arguments[1]), Math.E).ToString();
        }

        private string Round(ArrayList Arguments) //Round Argument1 to Argument2's number of decimals
        {
            return Math.Round(Convert.ToDouble(Arguments[1]), Convert.ToInt32(Arguments[3])).ToString();
        }

        private string Cos(ArrayList Arguments) //Uses angle in radians
        {
            return Math.Cos(Convert.ToDouble(Arguments[1])).ToString();
        }

        private string Sin(ArrayList Arguments) //Uses angle in radians
        {
            return Math.Sin(Convert.ToDouble(Arguments[1])).ToString();
        }

        private string Tan(ArrayList Arguments) //Uses angle in radians
        {
            return Math.Tan(Convert.ToDouble(Arguments[1])).ToString();
        }

        private string Power(ArrayList Arguments) //Argument1 raised to Argument2
        {
            return Math.Pow(Convert.ToDouble(Arguments[1]), Convert.ToDouble(Arguments[3])).ToString();
        }

        private string SquareRoot(ArrayList Arguments) //Square root of Argument1
        {
            return Math.Sqrt(Convert.ToDouble(Arguments[1])).ToString();
        }

        private string Degrees(ArrayList Arguments) //Uses Argument1 to change Degrees to Radians
        {
            return Convert.ToString(Convert.ToDouble(Arguments[1]) / 180 * Math.PI);
        }

        private string Radians(ArrayList Arguments) //Uses Argument1 to change Radians to Degrees
        {
            return Convert.ToString(Convert.ToDouble(Arguments[1]) * 180 / Math.PI);
        }

        private string PI() //Uses Argument1 to change Radians to Degrees
        {
            return Math.PI.ToString();
        }
    }
}
