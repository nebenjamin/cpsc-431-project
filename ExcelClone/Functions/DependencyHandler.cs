using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Collections;

namespace ExcelClone.Functions
{
    class DependencyHandler
    {

        private DependencyList ReferencesTo;    //Heads:        Cells that contain formulas with references to other cells.
                                                //Dependents:   Cells referenced within the Head cell.

        private DependencyList ReferencedBy;    //Heads:        Cells that are referenced within another cell.
                                                //Dependents:   Cells that contain a reference to the Head cell.

        public DependencyHandler() {
            ReferencesTo = new DependencyList();
            ReferencedBy = new DependencyList();
        }

        /*Parser calls this function: Statement[0] should be the cell
                                      Statement[1]..[n] are references */
        public void NewStatement(ArrayList Statement) {
            string Head = (string)Statement[0];
            Statement.RemoveAt(0);

            List<string> Dependents = new List<string>();
            foreach (object Element in Statement) {
                if (IsCellReference((string)Element))
                    Dependents.Add((string)Element);
            }

            MaintainLists(Head, Dependents);
            
        }

        private void MaintainLists(string Head, List<string> Dependents) {
            /*Before updating the lists, check if any cell has been changed
              or needs to bee updated */

            /*Check if Head is in the ReferencesTo list.
              If it is, then its entry in ReferencesTo must be overriden,
              and it must be removed from any occurences in the ReferencedBy list */
            if (ReferencesTo.Contains(Head)) {

                //Go through the ReferencedBy list of each element that was previously referenced
                List<string> PreviousElements = ReferencesTo.DependentsOf(Head);
                foreach (string Element in PreviousElements) {
                    ReferencedBy.Remove(Element, Head);
                }

                //Clear the previous elements in the ReferencesTo list and rewrite it
                ReferencesTo.ChangeNode(Head, Dependents);
                AddReferencedBy(Head, Dependents);

                ReferencedBy.Clean();   //Clean all nodes that do not have a list
                ReferencesTo.Clean();
            }

            /*Check if Head is in the ReferencedBy list.
              If it is, then each cell in that list must be invalidated.    */
            if (ReferencedBy.Contains(Head)) {
                ReferencedBy.Invalidate(Head);
            }

            //If Head is not in either list.
            if (!ReferencedBy.Contains(Head) && !ReferencesTo.Contains(Head)) {
                AddReferencesTo(Head, Dependents);
                AddReferencedBy(Head, Dependents);
            }
        }

        /*Creates a new node or appends the dependents list to a pre-existing
          node in the ReferencesTo DependencyList 
          The head will be Head (the cell that contains the formula) and the
          list will be those cells that are referenced in the head cell     */
        private void AddReferencesTo(string Head, List<string> Dependents){
            ReferencesTo.Add(Head, Dependents);
        }

        /*Creates new nodes or appends the dependents list to pre-exiting nodes
          in the ReferencedBy DependencyList.
          Each item in the dependents list will be the head of a node, with the
          Head being a dependent of that item.                              */
        private void AddReferencedBy(string Head, List<string> Dependents){
            List<string> X = new List<string>();
            X.Add(Head);

            foreach (string Element in Dependents) {
                ReferencedBy.Add(Element, X);
            }
        }


        //--------------------------------------
        override public string ToString() {
            string RV = "";
            RV += "ReferencesTo:\n";
            RV += ReferencesTo;
            RV += '\n';
            RV += "ReferencedBy:\n";
            RV += ReferencedBy;
            RV += '\n';
            RV += "-----------------------------------";
            RV += '\n';
            return RV;
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
    }
}
