using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FunctionsTeamSandbox
{
    public class Parser
    {
        private string Base_Cell;
        private string Base_String;
        private Functions Fun_Class;

        public Parser()
        {
            Fun_Class = new Functions();
        }

        public string Parse(string Cell_String) //NOT COMPLETE YET
        {
            Console.WriteLine(Cell_String);

            Base_Cell = Cell_String.Substring(0, Cell_String.IndexOf(':') - 1);
            Base_String = Cell_String.Substring(Cell_String.IndexOf(':') + 1);
            Cell_String = Cell_String.Substring(Cell_String.IndexOf(':') + 1);

            Console.WriteLine(Cell_String);

            if (Cell_String.Length == 0)
                return Cell_String;
            if (Cell_String[0].ToString() != "=")
                return Cell_String;

            ArrayList Parts = Tokenize(Cell_String);
            PrintArrayList(Parts);

            string srtmp = "";
            if (Cell_String.Contains("+") || Cell_String.Contains("-") ||
                Cell_String.Contains("*") || Cell_String.Contains("/") ||
                Cell_String.Contains(":"))
            {
                ArrayList temp = Reformat(Parts);
                PrintArrayList(temp);

                //Store the Base_Cell and all References
                //Add in the ability to check if a Base_Cell is changed
                //And to update a cell if is it has reference to this cell

                srtmp = Breaker(temp);
            }
            else
            {

                //Store the Base_Cell and all References
                //Add in the ability to check if a Base_Cell is changed
                //And to update a cell if is it has reference to this cell

                srtmp = Breaker(Parts);
            }

            try
            {
                return Convert.ToDouble(srtmp).ToString();

            }
            catch
            {
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

        private ArrayList Reformat(ArrayList Parts) //NOT COMPLETE YET (Cell Reference,A1)
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
                        Console.WriteLine("ERROR"); //ERROR: INCORRECT INPUT STRING
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
                        Console.WriteLine("ERROR"); //ERROR: INCORRECT INPUT STRING
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
            /*for( int i=0; i<Parts.Count; i++ ) { //CELL REFERENCE
                if( Base_Cell == Parts[i] ) {
                    //ERROR: CIRCULAR REFERENCE
                }
                if( IsCellReference( Parts[i] ) ) {
                    If x->string is a Cell Reference {
                        //Anything?
                    }
                    Else {
                        Insert Reformat(Tokenize(x->string)) into x's position
                    }
                }
            }*/

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
                if (char.IsLetter(Reference, 0))
                {
                    Convert.ToInt32(Reference.Substring(1));
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void PrintArrayList(ArrayList temp)
        {
            for (int i = 0; i < temp.Count; i++)
                Console.Write(temp[i].ToString() + ".");

            Console.WriteLine("");
        }
    }
}
