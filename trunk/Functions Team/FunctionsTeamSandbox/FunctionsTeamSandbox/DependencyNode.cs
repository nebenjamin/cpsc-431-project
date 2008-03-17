using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FunctionsTeamSandbox
{
    class DependencyNode
    {
        string Head;
        List<string> Dependents;

        public DependencyNode(string NewHead, List<string> NewDependents) {
            Head = NewHead;
            Dependents = new List<string>(NewDependents);
        }


        public string GetHead() {
            return Head;
        }

		//Append List y onto the end of the Dependents list
		public void Append(List<string> y){
            Dependents.AddRange(y);
		}

        public void ChangeListTo(List<string> y) {          
            Dependents = y;
        }
        
        //Invalidate all items in Dependents
		public void Invalidate(){
            Console.Write("The following must be updated: ");
            foreach (string s in Dependents)
                Console.Write(s + " ");
            Console.WriteLine();
		}

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
    }
}
