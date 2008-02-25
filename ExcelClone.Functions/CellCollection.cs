using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FunctionsTeamSandbox
{
    class CellCollection
    {
        private ArrayList Cells;
        private int NumberOfCells;

        public CellCollection(int NumOfCells)
        {
            NumberOfCells = NumOfCells;

            for (int i = 0; i < NumberOfCells; i++)
            {
                for (int j = 0; j < NumberOfCells; j++)
                {
                    Cells.Add(new Cell());
                }
            }
        }

        public string GetCellFormula(string CellReference)
        {
            CellReference.ToUpper();

            if (char.IsLetter(CellReference[0]) && char.IsLetter(CellReference[1]))
            {
                try
                {
                    int t = 'A';
                    int t1 = CellReference[0] - t;
                    int t2 = CellReference[1] - t;

                    int t3 = ((t1 + t2) * NumberOfCells) + Convert.ToInt32(CellReference.Substring(2)) - 1;

                    return ((Cell)Cells[t3]).Formula;
                }
                catch
                {
                    return "";
                }
            }
            if (char.IsLetter(CellReference[0]))
            {
                try
                {
                    int t = 'A';
                    int t1 = CellReference[0] - t;

                    int t3 = (t1 * NumberOfCells) + Convert.ToInt32(CellReference.Substring(1)) - 1;

                    return ((Cell)Cells[t3]).Formula;
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
    }
}
