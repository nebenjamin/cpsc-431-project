using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using ExcelClone.Core;

namespace ExcelClone.DataIO
{
    public class DataIO
    {
		public const string extension = ".dump";

		private string filename;
		private string nl = Environment.NewLine;

		private List<SpreadsheetModel> book;
		private Stream fileStream;
		private XmlSerializer serializer;
		private XmlTextWriter textWriter;

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
			try
			{
				if (book.Count == 0) throw new MissingMemberException("Passed an empty book!  Feed me data");
				return WriteBook();
			}
			catch (Exception e)
			{
				if (e is System.Security.SecurityException)
				{
					MessageBox.Show("I cannot write a file. Please run me with FileIO permissions.", "Error");
				}
				else
				{
					MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message, "Error");
				}
			}
			return false;
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
