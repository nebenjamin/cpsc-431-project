using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
 
namespace FunctionsTeamSandbox
{
    public class Parser
    {
        private string Base_Cell;
        private string Base_String;
        private Functions Fun_Class = new Functions();
        private TextWriter OutFile;
        private DependencyHandler Dependencies = new DependencyHandler();

        public Parser() 
        {
            OutFile = new StreamWriter("Output "+System.DateTime.Now.ToString().Replace(':','.').Replace('/','.')+".txt");
        }

        /* NOT COMPLETE YET (References Table Implementation)
         */
        public string Parse(string Cell_String)
        {
            Cell_String = Cell_String.ToUpper().Replace(" ","");
            OutFile.WriteLine("---------- " + Cell_String);
            Form1.Step("---------- " + Cell_String);

            Base_Cell = Cell_String.Substring(0, Cell_String.IndexOf(':'));
            Base_String = Cell_String.Substring(Cell_String.IndexOf(':') + 1);
            Cell_String = Cell_String.Substring(Cell_String.IndexOf(':') + 1);

            OutFile.WriteLine(Cell_String);

            ArrayList Send = new ArrayList();
            Send.Add(Base_Cell);

            if (Cell_String.Length == 0)
            {
                OutFile.WriteLine("Cell is empty");
                Form1.Step("Cell is empty");
                return Cell_String;
            }
            if (Cell_String[0].ToString() != "=")
            {
                //Still need to call a DependencyHandler function
                Dependencies.NewStatement(Send);
                OutFile.WriteLine("Cell is a string");
                Form1.Step("Cell is a string");
                return Cell_String;
            }

            ArrayList Parts = Tokenize(Cell_String);
            PrintArrayList(Parts);

            Send = new ArrayList(Parts);
            Send.Insert(0, Base_Cell);
            //Store the Base_Cell and all References
            Dependencies.NewStatement(Send);
            //Add in the ability to check if a Base_Cell is changed
            //And to update a cell if is it has reference to this cell

            ArrayList temp = Reformat(Parts);
            PrintArrayList(temp);

            string strtmp = Breaker(temp);

            OutFile.WriteLine("-----" + strtmp);
            Form1.Step("-----" + strtmp);
            OutFile.Flush();
            try
            {
                return Convert.ToDouble(strtmp).ToString();

            }
            catch
            {
                OutFile.WriteLine("ERROR: FORMATING ERROR");
                Form1.Step("ERROR: FORMATING ERROR");
                OutFile.Flush();
                //ERROR: FORMATING ERROR
                return Base_String;
            }
        }

        private string Breaker(ArrayList Cell_String)
        {
            for (int i = 1; i < Cell_String.Count; i++)
            {
                if (Fun_Class.IsFunction(Cell_String[i].ToString()))
                {
                    int found = 1;
                    int start = i + 1;

                    while (found != 0)
                    {
                        start++;
                        if (Cell_String[start].ToString().Equals("("))
                            found++;
                        if (Cell_String[start].ToString().Equals(")"))
                            found--;
                    }
                    ArrayList atemp = new ArrayList(Cell_String.GetRange(i, start - i + 1));
                    Cell_String.RemoveRange(i, start - i + 1);
                    Cell_String.Insert(i, Breaker(atemp));
                }
            }

            //PrintArrayList(Cell_String);
            //Console.WriteLine(Cell_String[0].ToString());

            return Fun_Class.CallFunction(Cell_String);
        }

        private ArrayList Tokenize(string Cell_String)
        {
            ArrayList temp = new ArrayList();

            for (int i = 1; i < Cell_String.Length; i++)
            {
                for (int j = i; j < Cell_String.Length; j++)
                {
                    if (Cell_String[j].Equals('*') || Cell_String[j].Equals('/') || Cell_String[j].Equals('+') ||
                        Cell_String[j].Equals('-') || Cell_String[j].Equals('(') || Cell_String[j].Equals(')') ||
                        Cell_String[j].Equals(','))
                    {
                        temp.Add(Cell_String.Substring(i, j - i));
                        temp.Add(Cell_String.Substring(j, 1));
                        i = j;
                        break;
                    }
                    if (j == Cell_String.Length - 1)
                    {
                        temp.Add(Cell_String.Substring(i, j - i + 1));
                        i = j + 1;
                        break;
                    }
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].ToString().Equals(""))
                {
                    temp.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].ToString().Equals("-"))
                {
                    if ((i - 1 == -1) && char.IsNumber(temp[i + 1].ToString(), 0))
                    {
                        temp.RemoveAt(i);
                        temp[i] = Convert.ToString(Convert.ToInt32(temp[i].ToString()) * -1);
                        i--;
                    }
                    else
                    {
                        if (!IsNumber(temp[i + 1].ToString()) && !IsNumber(temp[i + 2].ToString()))
                        {
                            temp.RemoveAt(i);
                            temp.Insert(i, "*");
                            temp.Insert(i, "-1");
                        }
                        else
                        {
                            if ((temp[i - 1].ToString().Equals("(") || temp[i - 1].ToString().Equals("*") ||
                                temp[i - 1].ToString().Equals("/") || temp[i - 1].ToString().Equals("+") ||
                                temp[i - 1].ToString().Equals("-") || temp[i - 1].ToString().Equals("=") ||
                                temp[i - 1].ToString().Equals(","))
                                && char.IsNumber(temp[i + 1].ToString(), 0))
                            {
                                temp.RemoveAt(i);
                                temp[i] = Convert.ToString(Convert.ToInt32(temp[i].ToString()) * -1);
                                i--;
                            }
                        }
                    }
                }
            }

            return temp;
        }

        /* NOT COMPLETE YET (Cell Reference,A1) 
         * THIS NEED TO BE SOLVED BY UI! 
         * The UI team needs to setup a mathod that takes a Cell Reference as a string and return the formula.
         */
        private ArrayList Reformat(ArrayList Parts)
        {
            #region SECTION REMOVER ()
            for (int i = 0; i < Parts.Count; i++)
            {
                int second = i - 1;
                if (second == -1)
                    second = 0;
                if (Parts[i].ToString().Equals("(") && !Fun_Class.IsFunction(Parts[second].ToString()) && !IsNumber(Parts[second].ToString()))
                {
                    //PrintArrayList(Parts);

                    int found = 1;
                    int start = i;

                    while (found != 0)
                    {
                        start++;
                        if (Parts[start].ToString().Equals("("))
                            found++;
                        if (Parts[start].ToString().Equals(")"))
                            found--;
                    }
                    ArrayList atemp = new ArrayList(Parts.GetRange(i + 1, start - i + 1 - 2));
                    Parts.RemoveRange(i, start - i + 1);
                    Parts.InsertRange(i, Reformat(atemp));
                }
            }
            #endregion
            #region SHORTCUT REMOVER * and /
            for (int i = 0; i < Parts.Count; i++)
            {
                string function;
                if (Parts[i].ToString().Equals("*") || Parts[i].ToString().Equals("/"))
                {
                    //PrintArrayList(Parts);

                    if (Parts[i].ToString().Equals("*"))
                        function = "MUL";
                    else
                        function = "DIV";

                    try
                    {
                        int first = i - 1, second = i + 2;
                        if ((i + 2) >= Parts.Count)
                            second--;
                        if (Parts[first].ToString().Equals(")") && !Parts[second].ToString().Equals("("))
                        {
                            int found = 1;
                            int start = i - 1;

                            while (found != 0)
                            {
                                start--;
                                if (Parts[start].ToString().Equals(")"))
                                    found++;
                                if (Parts[start].ToString().Equals("("))
                                    found--;
                            }

                            Parts.Insert(i, ")");
                            Parts.Insert(i, Parts[i + 2]);
                            Parts.Insert(i, ",");
                            Parts.InsertRange(i, Parts.GetRange(start - 1, i - start + 1));
                            Parts.Insert(i, "(");
                            Parts.Insert(i, function);

                            Parts.RemoveAt(i + (i - start + 1) + 5);
                            Parts.RemoveAt(i + (i - start + 1) + 5);
                            Parts.RemoveRange(start - 1, i - start + 1);
                        }
                        else
                        {
                            if (!Parts[first].ToString().Equals(")") && Parts[second].ToString().Equals("("))
                            {
                                int found = 1;
                                int start = i + 2;

                                while (found != 0)
                                {
                                    start++;
                                    if (Parts[start].ToString().Equals("("))
                                        found++;
                                    if (Parts[start].ToString().Equals(")"))
                                        found--;
                                }

                                Parts.Insert(start + i, ")");
                                Parts.Insert(i + 1, ",");
                                Parts.Insert(i + 1, Parts[i - 1]);
                                Parts.Insert(i + 1, "(");
                                Parts.Insert(i + 1, function);

                                Parts.RemoveAt(i - 1);
                                Parts.RemoveAt(i - 1);
                            }
                            else
                            {
                                if (!Parts[first].ToString().Equals(")") && !Parts[second].ToString().Equals("("))
                                {
                                    Parts.Insert(i, ")");
                                    Parts.Insert(i, Parts[i + 2]);
                                    Parts.Insert(i, ",");
                                    Parts.Insert(i, Parts[i - 1]);
                                    Parts.Insert(i, "(");
                                    Parts.Insert(i, function);

                                    Parts.RemoveAt(i + 6);
                                    Parts.RemoveAt(i + 6);
                                    Parts.RemoveAt(i - 1);
                                }
                                else
                                {
                                    if (Parts[first].ToString().Equals(")") && Parts[second].ToString().Equals("("))
                                    {
                                        int found = 1;
                                        int start = i + 2;

                                        while (found != 0)
                                        {
                                            start++;
                                            if (Parts[start].ToString().Equals("("))
                                                found++;
                                            if (Parts[start].ToString().Equals(")"))
                                                found--;
                                        }

                                        Parts.Insert(start + 1, ")");
                                        Parts[i] = ",";

                                        found = 1;
                                        start = i - 1;

                                        while (found != 0)
                                        {
                                            start--;
                                            if (Parts[start].ToString().Equals(")"))
                                                found++;
                                            if (Parts[start].ToString().Equals("("))
                                                found--;
                                        }

                                        Parts.Insert(start - 1, "(");
                                        Parts.Insert(start - 1, function);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        OutFile.WriteLine("ERROR: INCORRECT INPUT STRING"); //ERROR: INCORRECT INPUT STRING
                        Form1.Step("ERROR: INCORRECT INPUT STRING");
                        return Tokenize(Base_String);
                    }
                }
            }
            #endregion
            #region SHORTCUT REMOVER + and -
            for (int i = 0; i < Parts.Count; i++)
            { //SHORTCUT REMOVER
                string function;
                if (Parts[i].ToString().Equals("+") || Parts[i].ToString().Equals("-"))
                {
                    //PrintArrayList(Parts);

                    if (Parts[i].ToString().Equals("+"))
                        function = "ADD";
                    else
                        function = "SUB";

                    try
                    {
                        int first = i - 1, second = i + 2;
                        if ((i + 2) >= Parts.Count)
                            second--;
                        if (Parts[first].ToString().Equals(")") && !Parts[second].ToString().Equals("("))
                        {
                            int found = 1;
                            int start = i - 1;

                            while (found != 0)
                            {
                                start--;
                                if (Parts[start].ToString().Equals(")"))
                                    found++;
                                if (Parts[start].ToString().Equals("("))
                                    found--;
                            }

                            Parts.Insert(i, ")");
                            Parts.Insert(i, Parts[i + 2]);
                            Parts.Insert(i, ",");
                            Parts.InsertRange(i, Parts.GetRange(start - 1, i - start + 1));
                            Parts.Insert(i, "(");
                            Parts.Insert(i, function);

                            Parts.RemoveAt(i + (i - start + 1) + 5);
                            Parts.RemoveAt(i + (i - start + 1) + 5);
                            Parts.RemoveRange(start - 1, i - start + 1);
                        }
                        else
                        {
                            if (!Parts[first].ToString().Equals(")") && Parts[second].ToString().Equals("("))
                            {
                                int found = 1;
                                int start = i + 2;

                                while (found != 0)
                                {
                                    start++;
                                    if (Parts[start].ToString().Equals("("))
                                        found++;
                                    if (Parts[start].ToString().Equals(")"))
                                        found--;
                                }

                                Parts.Insert(start + i, ")");
                                Parts.Insert(i + 1, ",");
                                Parts.Insert(i + 1, Parts[i - 1]);
                                Parts.Insert(i + 1, "(");
                                Parts.Insert(i + 1, function);

                                Parts.RemoveAt(i - 1);
                                Parts.RemoveAt(i - 1);
                            }
                            else
                            {
                                if (!Parts[first].ToString().Equals(")") && !Parts[second].ToString().Equals("("))
                                {
                                    Parts.Insert(i, ")");
                                    Parts.Insert(i, Parts[i + 2]);
                                    Parts.Insert(i, ",");
                                    Parts.Insert(i, Parts[i - 1]);
                                    Parts.Insert(i, "(");
                                    Parts.Insert(i, function);

                                    Parts.RemoveAt(i + 6);
                                    Parts.RemoveAt(i + 6);
                                    Parts.RemoveAt(i - 1);
                                }
                                else
                                {
                                    if (Parts[first].ToString().Equals(")") && Parts[second].ToString().Equals("("))
                                    {
                                        int found = 1;
                                        int start = i + 2;

                                        while (found != 0)
                                        {
                                            start++;
                                            if (Parts[start].ToString().Equals("("))
                                                found++;
                                            if (Parts[start].ToString().Equals(")"))
                                                found--;
                                        }

                                        Parts.Insert(start + 1, ")");
                                        Parts[i] = ",";

                                        found = 1;
                                        start = i - 1;

                                        while (found != 0)
                                        {
                                            start--;
                                            if (Parts[start].ToString().Equals(")"))
                                                found++;
                                            if (Parts[start].ToString().Equals("("))
                                                found--;
                                        }

                                        Parts.Insert(start - 1, "(");
                                        Parts.Insert(start - 1, function);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        OutFile.WriteLine("ERROR: INCORRECT INPUT STRING"); //ERROR: INCORRECT INPUT STRING
                        Form1.Step("ERROR: INCORRECT INPUT STRING");
                        return Tokenize(Base_String);
                    }
                }
            }
            #endregion
            #region SHORTCUT REMOVER :
            for (int i = 0; i < Parts.Count; i++)
            { //SHORTCUT REMOVER 1
                if (Parts[i].ToString().Contains(":"))
                {
                    //PrintArrayList(Parts);

                    char c = Convert.ToChar(Parts[i].ToString().Substring(0, 1));
                    int t1 = Convert.ToInt32(Parts[i].ToString().Split(':')[0].Substring(1));
                    int t2 = Convert.ToInt32(Parts[i].ToString().Split(':')[1].Substring(1));

                    Parts.RemoveAt(i);
                    for (int j = t2; j >= t1; j--)
                    {
                        Parts.Insert(i, c + j.ToString());
                        if (j != t1)
                            Parts.Insert(i, ",");
                    }
                }
            }
            #endregion
            for (int i = 0; i < Parts.Count; i++)
            { //CELL REFERENCE
                if (Base_Cell.CompareTo(Parts[i].ToString().ToUpper()) == 0)
                {
                    OutFile.WriteLine("ERROR: CIRCULAR REFERENCE");
                    Form1.Step("ERROR: CIRCULAR REFERENCE");
                    //ERROR: CIRCULAR REFERENCE
                    return Parts;
                }
                if (IsCellReference(Parts[i].ToString()))
                {
                    OutFile.WriteLine("Cell Reference");
                    Form1.Step("Cell Reference");
                    //Console.WriteLine("BaseCell= " + Base_Cell);
                    //Console.WriteLine("CellReference= " + Parts[i].ToString());

                    string cell_ref = Parts[i].ToString().ToUpper();

                    Parts.RemoveAt(i);
                    //string temp = "=1+2";//temp will equal the output of the UI's function
                    string temp = Form1.getCellFormula(cell_ref);

                    OutFile.WriteLine(cell_ref + " -> " + temp);
                    Form1.Step(cell_ref + " -> " + temp);

                    if (temp[0] == '=')
                    {
                        //Cell has a formula
                        Parts.InsertRange(i, Reformat(Tokenize(temp)));

                        OutFile.WriteLine("Cell has a formula");
                        Form1.Step("Cell has a formula");
                    }
                    else
                    {
                        try
                        {
                            //Cell has a value
                            Parts.Insert(i, Convert.ToDouble(temp));

                            OutFile.WriteLine("Cell has a number");
                            Form1.Step("Cell has a number");
                        }
                        catch
                        {
                            OutFile.WriteLine("Cell has a string");
                            Form1.Step("Cell has a string");
                            //ERROR: Cell has a string
                            return Parts;
                        }
                    }
                }
            }
            
            return Parts;
        }

        private bool IsNumber(string input)
        {
            try
            {
                Convert.ToDouble(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsCellReference(string Reference)
        {
            try
            {
                if (char.IsLetter(Reference, 0) && char.IsLetter(Reference, 1))
                {
                    if (Convert.ToInt32(Reference.Substring(2)) > 0)
                        return true;
                }
                if (char.IsLetter(Reference, 0))
                {
                    if (Convert.ToInt32(Reference.Substring(1)) > 0)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void PrintArrayList(ArrayList temp)
        {
            string stemp = "";
            for (int i = 0; i < temp.Count; i++)
            {
                OutFile.Write(temp[i].ToString() + ".");
                stemp += temp[i].ToString() + ".";
            }

            OutFile.WriteLine("");
            Form1.Step(stemp);
        }
    }
}
