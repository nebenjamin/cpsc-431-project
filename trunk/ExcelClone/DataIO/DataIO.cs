using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using ExcelClone.Core;

namespace ExcelClone.DataIO
{
    public class DataIO
    {
        private List<SpreadsheetModel> book;
        private String filename;
        private XmlSerializer xmlSerializer;
        private XmlTextWriter xmlTextWriter;

        public DataIO(String Filename)
        {
            book = new List<SpreadsheetModel>();
            filename = Filename;
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
                MessageBox.Show("Passed an empty book!  Feed me data", "I'm hungry");
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
