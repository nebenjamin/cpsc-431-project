using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using ExcelClone.Gui;

namespace ExcelClone.Functions
{
    public class Parser
    {
        private string Base_Cell;
        private Functions Fun_Class = new Functions();
        private TextWriter OutFile;
        private DependencyHandler Dependencies;
        private SpreadsheetUserControl activeWS = null;
        public Parser() 
        {
            //OutFile = new StreamWriter("Output "+System.DateTime.Now.ToString().Replace(':','.').Replace('/','.')+".txt");
            Dependencies = new DependencyHandler(OutFile);
        }

        public string Parse(SpreadsheetUserControl activeWS,string Cell_String)
        {
            this.activeWS = activeWS; //Assign the current worksheet to reference
            string Original_Cell_String = Cell_String.Substring(Cell_String.IndexOf(':') + 1); //Saves the original cell function

            Cell_String = Cell_String.ToUpper().Replace(" ", ""); //Removes all empty spaces

            Base_Cell = Cell_String.Substring(0, Cell_String.IndexOf(':')); //Sets the base cell that is being referenced
            {//Resets the cell's error state
                ArrayList atemp = BreakReference(Base_Cell);
                int r = (int)atemp[0];
                int c = (int)atemp[1];
                if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                {
                    activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Error = false;
                    activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].ErrorString = "";
                }
            }
            Cell_String = Cell_String.Substring(Cell_String.IndexOf(':') + 1); //removes the base cell reference
            //OutFile.WriteLine("---------- " + Base_Cell + ":" + Cell_String);
            //OutFile.WriteLine(Cell_String);

            ArrayList Send = new ArrayList();
            Send.Add(Base_Cell);

            if (Cell_String.Length == 0)
            {
                Cell_String = " ";
            }
            if (Cell_String[0].ToString() != "=")
            {
                Dependencies.NewStatement(Send); //Sends the data to the dependency list
                //OutFile.WriteLine("Cell is a string");
                return Original_Cell_String;
            }

            ArrayList Parts = Tokenize(Cell_String);
            PrintArrayList(Parts);

            //Store the Base_Cell and all References
            Send = ReformatForDL(new ArrayList(Parts));
            Send.Insert(0, Base_Cell);
            Dependencies.NewStatement(Send);

            //Store the Base_Cell and all References
            //Add in the ability to check if a Base_Cell is changed
            //And to update a cell if is it has reference to this cell

            ArrayList temp = Reformat(Parts);
            PrintArrayList(temp);

            string strtmp = Breaker(temp);

            //OutFile.WriteLine("----- " + strtmp);
            //Form1.Step("----- " + strtmp);
            //OutFile.Flush();
            try
            {
                return Convert.ToDouble(strtmp).ToString();

            }
            catch
            {
                //OutFile.WriteLine("ERROR - FORMATING ERROR");
                //Form1.Step("ERROR: FORMATING ERROR");
                //OutFile.Flush();
                ArrayList atemp = BreakReference(Base_Cell);
                int r = (int)atemp[0];
                int c = (int)atemp[1];
                if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                {
                    activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Error = true;
                    activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].ErrorString = "ERROR - FORMATING ERROR";
                }
                //ERROR: FORMATING ERROR
                return Original_Cell_String;
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

            #region BREAK APART EACH PART OF THE ORIGINAL STRING
            for (int i = 1; i < Cell_String.Length; i++)
            {
                for (int j = i; j < Cell_String.Length; j++)
                {
                    if (Cell_String[j].Equals('*') || Cell_String[j].Equals('/') || Cell_String[j].Equals('+') ||
                        Cell_String[j].Equals('-') || Cell_String[j].Equals('(') || Cell_String[j].Equals(')') ||
                        Cell_String[j].Equals(',') || Cell_String[j].Equals('~'))
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
            #endregion

            #region REMOVE ALL EMPTY ELEMENTS IN THE ARRAYLIST
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].ToString().Equals(""))
                {
                    temp.RemoveAt(i);
                    i--;
                }
            }
            #endregion

            #region CHECK FOR NEGATIVE VALUES (-)
            bool first = true;
            if (first)
            {
                temp.Insert(0, "=");
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].ToString().Equals("-"))
                    {
                        if (i + 1 >= temp.Count || i - 1 < 0) break;
                        if (IsCellReference(temp[i + 1].ToString())) //Cell Refrence
                        {
                            if (temp[i - 1].ToString().Equals("=") || temp[i - 1].ToString().Equals("+") ||
                                temp[i - 1].ToString().Equals("-") || temp[i - 1].ToString().Equals("*") ||
                                temp[i - 1].ToString().Equals("/") || temp[i - 1].ToString().Equals("(") ||
                                temp[i - 1].ToString().Equals(","))
                            {
                                temp.RemoveAt(i);
                                temp.Insert(i, "*");
                                temp.Insert(i, "-1");
                            }
                        }
                        else if (IsNumber(temp[i + 1].ToString())) //Number
                        {
                            if (temp[i - 1].ToString().Equals("=") || temp[i - 1].ToString().Equals("+") ||
                                temp[i - 1].ToString().Equals("-") || temp[i - 1].ToString().Equals("*") ||
                                temp[i - 1].ToString().Equals("/") || temp[i - 1].ToString().Equals("(") ||
                                temp[i - 1].ToString().Equals(","))
                            {
                                temp.RemoveAt(i);
                                temp[i] = Convert.ToString(Convert.ToInt32(temp[i].ToString()) * -1);
                                i--;
                            }
                        }
                        else if (Fun_Class.IsFunction(temp[i + 1].ToString())) //Function
                        {
                            if (temp[i - 1].ToString().Equals("=") || temp[i - 1].ToString().Equals("+") ||
                                temp[i - 1].ToString().Equals("-") || temp[i - 1].ToString().Equals("*") ||
                                temp[i - 1].ToString().Equals("/") || temp[i - 1].ToString().Equals("(") ||
                                temp[i - 1].ToString().Equals(","))
                            {
                                temp.RemoveAt(i);
                                temp.Insert(i, "*");
                                temp.Insert(i, "-1");
                            }
                        }
                        else //String
                        {
                            if (temp[i - 1].ToString().Equals("=") || temp[i - 1].ToString().Equals("+") ||
                                temp[i - 1].ToString().Equals("-") || temp[i - 1].ToString().Equals("*") ||
                                temp[i - 1].ToString().Equals("/") || temp[i - 1].ToString().Equals("(") ||
                                temp[i - 1].ToString().Equals(","))
                            {
                                temp.RemoveAt(i);
                                temp.Insert(i, "*");
                                temp.Insert(i, "-1");
                            }
                        }
                    }
                }
                temp.RemoveAt(0);
            }
            #endregion
            #region CHECK FOR NEGATIVE VALUES (~)
            if (first)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].ToString().Equals("~"))
                    {
                        if (i + 1 >= temp.Count) break;
                        if (IsCellReference(temp[i + 1].ToString())) //Cell Refrence
                        {
                            temp.RemoveAt(i);
                            temp.Insert(i, "*");
                            temp.Insert(i, "-1");
                        }
                        else if (IsNumber(temp[i + 1].ToString())) //Number
                        {
                            temp.RemoveAt(i);
                            temp[i] = Convert.ToString(Convert.ToInt32(temp[i].ToString()) * -1);
                            i--;
                        }
                        else if (Fun_Class.IsFunction(temp[i + 1].ToString())) //Function
                        {
                            temp.RemoveAt(i);
                            temp.Insert(i, "*");
                            temp.Insert(i, "-1");
                        }
                        else //String
                        {
                            temp.RemoveAt(i);
                            temp.Insert(i, "*");
                            temp.Insert(i, "-1");
                        }
                    }
                }
            }
            #endregion

            return temp;
        }

        private ArrayList ReformatForDL(ArrayList Parts) {

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

            #region DECREMENT ALL CELL REFERENCES
            for (int i = 0; i < Parts.Count; i++)
            {
                if (IsCellReference(Parts[i].ToString()))
                    Parts[i] = DecrementCellReference(Parts[i].ToString());
            }
            #endregion

            return Parts;
        }

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

                                Parts.Insert(start + 1, ")"); //Parts.Insert(start + i, ")");
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
                        //OutFile.WriteLine("=ERROR - INCORRECT INPUT STRING (/,*)"); //ERROR: INCORRECT INPUT STRING
                        //Form1.Step("ERROR: INCORRECT INPUT STRING");
                        ArrayList atemp = BreakReference(Base_Cell);
                        int r = (int)atemp[0];
                        int c = (int)atemp[1];
                        if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                        {
                            activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Error = true;
                            activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].ErrorString = "ERROR - INCORRECT INPUT STRING  (/,*)";
                        }
                        return Tokenize("=ERROR - INCORRECT INPUT STRING (/,*)");//return Tokenize(Base_String);
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
                        //OutFile.WriteLine("=ERROR - INCORRECT INPUT STRING  (+,-)"); //ERROR: INCORRECT INPUT STRING
                        //Form1.Step("ERROR: INCORRECT INPUT STRING");
                        ArrayList atemp = BreakReference(Base_Cell);
                        int r = (int)atemp[0];
                        int c = (int)atemp[1];
                        if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                        {
                            activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Error = true;
                            activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].ErrorString = "ERROR - INCORRECT INPUT STRING  (+,-)";
                        }
                        return Tokenize("=ERROR - INCORRECT INPUT STRING  (+,-)");//return Tokenize(Base_String);
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

            #region DECREMENT ALL CELL REFERENCES
            for (int i = 0; i < Parts.Count; i++)
            {
                if (IsCellReference(Parts[i].ToString()))
                    Parts[i] = DecrementCellReference(Parts[i].ToString());
            }
            #endregion

            #region EXPAND OUT ALL CELL REFERENCES
            for (int i = 0; i < Parts.Count; i++)
            { //CELL REFERENCE
                for(int j=0; j<Parts.Count; j++)
                    if ((Base_Cell.CompareTo(Parts[j].ToString().ToUpper()) == 0) || (Parts[j].ToString().ToUpper().Contains("ERROR")))
                    {
                        ArrayList atemp = BreakReference(Base_Cell);
                        int r = (int)atemp[0];
                        int c = (int)atemp[1];
                        if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                        {
                            activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Error = true;
                            activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].ErrorString = "ERROR - CIRCULAR REFERENCE";
                        }
                        return Tokenize("=ERROR - CIRCULAR REFERENCE");
                    }

                for(int j=0; j<Parts.Count; j++) {
                    if(IsCellReference(Parts[j].ToString())) {
                        ArrayList atemp = BreakReference(Parts[j].ToString());
                        int r = (int)atemp[0];
                        int c = (int)atemp[1];
                        if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                            if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Error)
                            {
                                activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].ErrorString = "ERROR - POSSIBLE CIRCULAR REFERENCE";
                                return Tokenize("=ERROR - POSSIBLE CIRCULAR REFERENCE");
                            }
                    }
                }

                //if (Base_Cell.CompareTo(Parts[i].ToString().ToUpper()) == 0)
                //{
                //    //OutFile.WriteLine("=ERROR - CIRCULAR REFERENCE");
                //    //Form1.Step("ERROR: CIRCULAR REFERENCE");
                //    //ERROR: CIRCULAR REFERENCE
                //    return Tokenize("=ERROR - CIRCULAR REFERENCE");//return Parts;
                //}
                if (IsCellReference(Parts[i].ToString()))
                {
                    //OutFile.WriteLine("Cell Reference");
                    //Form1.Step("Cell Reference");
                    //Console.WriteLine("BaseCell= " + Base_Cell);
                    //Console.WriteLine("CellReference= " + Parts[i].ToString());

                    string cell_ref = Parts[i].ToString().ToUpper();

                    Parts.RemoveAt(i);
                    //string temp = "=1+2";//temp will equal the output of the UI's function
                    //FIXME
                    ArrayList atemp = BreakReference(cell_ref);
                    int r = (int)atemp[0];
                    int c = (int)atemp[1];
                    string temp;
                    if (activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c] != null)
                        temp = activeWS.Spreadsheet.SpreadsheetModel.Cells[r, c].Formula.ToUpper();// Form1.getCellFormula(cell_ref);
                    else
                        temp = "NULL";
                    if (temp.Length <= 0) temp = "NULL";

                    //OutFile.WriteLine(cell_ref + " -> " + temp);
                    //Form1.Step(cell_ref + " -> " + temp);

                    if (temp[0] == '=')
                    {
                        //Cell has a formula
                        Parts.InsertRange(i, Reformat(Tokenize(temp)));

                        //OutFile.WriteLine("Cell has a formula");
                        //Form1.Step("Cell has a formula");
                    }
                    else
                    {
                        try
                        {
                            //Cell has a value
                            Parts.Insert(i, Convert.ToDouble(temp));

                            //OutFile.WriteLine("Cell has a number");
                            //Form1.Step("Cell has a number");
                        }
                        catch
                        {
                            Parts.Insert(i, "NULL");

                            //OutFile.WriteLine("Cell has a string");
                            //Form1.Step("Cell has a string");
                            //ERROR: Cell has a string
                            //return Parts;
                        }
                    }
                }
            }
            #endregion

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
                    if (Convert.ToInt32(Reference.Substring(2)) >= 0)
                        return true;
                }
                if (char.IsLetter(Reference, 0))
                {
                    if (Convert.ToInt32(Reference.Substring(1)) >= 0)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private string DecrementCellReference(string BaseCell)
        {
            try
            {
                if (char.IsLetter(BaseCell, 0) && char.IsLetter(BaseCell, 1))
                {
                    if (Convert.ToInt32(BaseCell.Substring(2)) > 0)
                    {
                        return BaseCell[0] + BaseCell[1] + (Convert.ToInt32(BaseCell.Substring(2)) - 1).ToString();
                    }
                }
                if (char.IsLetter(BaseCell, 0))
                {
                    if (Convert.ToInt32(BaseCell.Substring(1)) > 0)
                    {
                        return BaseCell[0] + (Convert.ToInt32(BaseCell.Substring(1)) - 1).ToString();
                    }
                }
                return BaseCell;
            }
            catch
            {
                return BaseCell;
            }
        }

        private ArrayList BreakReference(string Reference)
        {
            ArrayList temp = new ArrayList();
            try
            {
                if (char.IsLetter(Reference, 0) && char.IsLetter(Reference, 1))
                {
                    if (Convert.ToInt32(Reference.Substring(2)) >= 0)
                    {
                        temp.Add(Convert.ToInt32(Reference.Substring(2)));
                        temp.Add((((int)Reference[0]) - ((int)'A')) * 10 + (((int)Reference[1]) - ((int)'A')));
                        return temp;
                    }
                }
                if (char.IsLetter(Reference, 0))
                {
                    if (Convert.ToInt32(Reference.Substring(1)) >= 0)
                    {
                        temp.Add(Convert.ToInt32(Reference.Substring(1)));
                        temp.Add(((int)Reference[0]) - ((int)'A'));
                        return temp;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void PrintArrayList(ArrayList temp)
        {
            string stemp = "";
            for (int i = 0; i < temp.Count; i++)
            {
                //OutFile.Write(temp[i].ToString() + ".");
                stemp += temp[i].ToString() + ".";
            }

            //OutFile.WriteLine("");
            //Form1.Step(stemp);
        }
    }
}
