using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public class CellCollection : ICellCollection
    {
        public CellCollection()
        {
            cells = new Dictionary<CellKey, Cell>();
        }

        #region ICellCollection Members

        ICell ICellCollection.this[int r, int c] { get { return this[r, c]; } }
        ICell ICellCollection.this[CellKey key] { get { return this[key]; } }

        private int rows;
        public int Rows { get { return rows; } }
        
        private int cols;
        public int Columns { get { return cols; } }

        #endregion

        private Dictionary<CellKey, Cell> cells;

        public Cell this[int r, int c] 
        { 
            get { return this[new CellKey(r, c)]; }
            set { this[new CellKey(r, c)] = value; }
        }

        public Cell this[CellKey key]
        {
            get 
            {
                if(cells.ContainsKey(key))
                    return cells[key];

                return null;
            }
            set
            {
                if (cells.ContainsKey(key))
                    cells[key] = value;
                else
                {
                    cells.Add(key, value);
                    if (key.C >= cols)
                        cols = key.C + 1;
                    if (key.R >= rows)
                        rows = key.R + 1;
                }
            }
        }

        public IEnumerable<CellKey> Keys { get { return cells.Keys; } }

    }

    public class CellKey
    {
        private int r, c;
        public int R { get { return r; } }
        public int C { get { return c; } }

        public CellKey(int r, int c)
        {
            this.r = r;
            this.c = c;
        }

        public override bool Equals(object obj)
        {
            CellKey other = obj as CellKey;

            if (other == null)
                return false;

            return R == other.R && C == other.C;
        }

        public override int GetHashCode()
        {
            return R << 7 + C;
        }
    }
}
