using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Core
{
    public class CellCollection : ICellCollection
    {
        public CellCollection()
        {
            cells = new Dictionary<Key, Cell>();
        }

        #region ICellCollection Members

        ICell ICellCollection.this[int r, int c] { get { return this[r, c]; } }

        private int rows;
        public int Rows { get { return rows; } }
        
        private int cols;
        public int Columns { get { return cols; } }

        #endregion

        private Dictionary<Key, Cell> cells;
        public Cell this[int r, int c] 
        { 
            get { return cells[new Key(r, c)]; }
            set
            {
                Key key = new Key(r,c);
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

        private class Key
        {
            private int r, c;
            public int R { get { return r; } }
            public int C { get { return c; } }

            public Key(int r, int c)
            {
                this.r = r;
                this.c = c;
            }

            public override bool Equals(object obj)
            {
                Key other = obj as Key;

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
}
