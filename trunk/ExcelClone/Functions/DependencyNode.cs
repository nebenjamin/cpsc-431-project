using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace ExcelClone.Functions
{
    class DependencyNode
    {
        string Head;
        List<string> Dependents;
        private TextWriter OutFile;


        public DependencyNode(string NewHead, List<string> NewDependents, TextWriter Out) {
            Head = NewHead;
            Dependents = new List<string>(NewDependents);
            OutFile = Out;
        }


        public string GetHead() {
            return Head;
        }

		//Append List y onto the end of the Dependents list
		public void Append(List<string> y){
            Dependents.AddRange(y);
		}

        //Change the Dependents list to the list y from the previous list.
        public void ChangeListTo(List<string> y) {          
            Dependents = y;
        }
        
        //Invalidate all items in Dependents
		public void Invalidate(){
            /*OutFile.Write("The following must be updated: ");
            foreach (string s in Dependents)
                OutFile.Write(s + " ");
            OutFile.WriteLine();*/
            foreach (string s in Dependents)
            {
                ArrayList RC = BreakReference(s);
                SpreadsheetControl.Instance.InvalidateCell((int)RC[1], (int)RC[0]);
            }
		}

        //Return the list of dependents
        public List<string> ReturnDependents() {
            return Dependents;
        }

        public bool IsEmpty() {
            if (Dependents.Count == 0)
                return true;
            else
                return false;
        }

        public bool Remove(string Reference) {
            return Dependents.Remove(Reference);
        }


        //------------------------------------------
        override public string ToString() {
            string RV = "";
            RV += Head + ": ";

            foreach (string Element in Dependents) {
                RV += Element;
                RV += " ";
            }
            RV += '\n';

            return RV;
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
                        temp.Add((((int)Reference[0]) - ((int)'A')) * 10 + (((int)Reference[1]) - ((int)'A')));
                        temp.Add(Convert.ToInt32(Reference.Substring(2)));
                        return temp;
                    }
                }
                if (char.IsLetter(Reference, 0))
                {
                    if (Convert.ToInt32(Reference.Substring(1)) >= 0)
                    {
                        temp.Add(((int)Reference[0]) - ((int)'A'));
                        temp.Add(Convert.ToInt32(Reference.Substring(1)));
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


    }
}
