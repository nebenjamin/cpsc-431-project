using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace FunctionsTeamSandbox
{
    class DependencyList
    {
        List<DependencyNode> Data;

        public DependencyList(){
            Data = new List<DependencyNode>();
        }

        public void Add(string x, List<string> y) {
            if (IndexOf(x) == -1) {                         //if x is not already the head of a list, add it.                
                AddNode(x, y);
            }
            else {                                          //the string was already in the list
                Append(x, y);                
            }
        }

        public void ChangeNode(string x, List<string> y) {
 //           Data.ElementAt(IndexOf(x)).ChangeListTo(y);
            int i=0;
            int j = IndexOf(x);
            foreach (DependencyNode d in Data)
            {
                if (i == j)
                {
                    d.ChangeListTo(y);
                }
                i++;
            }
        }

        private void AddNode(string x, List<string> y){
            DependencyNode Node = new DependencyNode(x, y);
            Data.Add(Node);
        }

        //Searches for a Node with the head x and appends y to the list
        public void Append(string x, List<string> y) {
	    	int i = IndexOf(x);
			if (i == -1){
                AddNode(x, y);
                i = IndexOf(x);     //reset i
			}
//            Data.ElementAt(i).Append(y);
            int j = 0;

            foreach (DependencyNode d in Data)
            {
                if (i == j)
                {
                    d.Append(y);
                }
                j++;
            }
        }

        //Function to mark all cells in dependents list as invalid
        public void Invalidate(string x) {
	    	int i = IndexOf(x);
			if (i >= 0){
//				Data.ElementAt(i).Invalidate();
                int j = 0;
                
                foreach (DependencyNode d in Data)
                {
                    if (i == j)
                    {
                        d.Invalidate();
                    }
                    j++;
                }
		    }
        }

        public void Remove(string Reference, string Erase) {

//            DependencyNode X = Data.ElementAt(IndexOf(Reference));
            DependencyNode X = null;
            int k = 0;
            int j = IndexOf(Reference);
            foreach (DependencyNode d in Data)
            {
                if (k == j)
                {
                    X = d;
                }
                k++;
            }
            X.Remove(Erase);
        }

        public void Clean(){
            int Count = Data.Count;
            int i = 0;
            do{
                if (Data[i].IsEmpty()){
                    Data.Remove(Data[i]);
                    Count--;
                }
                else
                    i++;
            } while (i < Count);
        }

        public bool Contains(string Reference) {
            if (IndexOf(Reference) != -1)
                return true;
            else
                return false;
        }

        private int IndexOf(string Reference) {
//            for (int i = 0; i < Data.Count; i++)
//                if (Data.ElementAt(i).GetHead() == Reference)
//                    return i;
            int i = 0;
            foreach (DependencyNode d in Data)
            {
                if (d.GetHead() == Reference)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public List<string> DependentsOf(string Reference) {
            int i = IndexOf(Reference);
            if (i != -1)
            {
                int k=0;
                
                foreach (DependencyNode d in Data)
                {
                    if (k == i)
                    {
                        return d.ReturnDependents();
                    }
                    k++;
                }
                return null;
            }
                //return Data.ElementAt(i).ReturnDependents();
            else
                return null;
        }

        



        //---------------------------------------------------
        override public string ToString() {
            string RV = "";
            foreach (DependencyNode Element in Data)
                RV += Element;
            return RV;
        }
    }
}
