using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;

namespace ExcelClone.DataIO
{
    public class DataIO
    {
        private List<SpreadsheetModel> book;

        public DataIO(String Filename)
        {
            book = new List<SpreadsheetModel>();
        }

        public bool AddSpreadsheet(SpreadsheetModel sheet)
        {
            book.Add(sheet);
            return true;
        }

        public bool SetBook(List<SpreadsheetModel> volume)
        {
            book = volume;
            return true;
        }

        public bool SaveBook()
        {
            if (book.Count == 0)
            {
                System.Console.WriteLine("Passed an empty book!  Feed me data");
                return false;
            }
            return WriteBook();
        }

        public bool LoadBook()
        {
            return ReadBook();
        }

        private bool WriteBook()
        {
            return true;
        }

        private bool ReadBook()
        {
            return true;
        }
    }
}
