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
            int i = 0;
            int currentRow, currentColumn;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message, "Error");
            }


            XmlNodeList sheetList = doc.GetElementsByTagName("sheet");

            // Traverse the List of Sheets
            foreach (XmlNode node in sheetList)
            {
                CellCollection cells = new CellCollection();
                XmlElement sheetElement = (XmlElement)node;
                String sheetName = "";

                if (sheetElement.HasAttributes)
                {
                    if (sheetElement.Attributes[0].Name == "name")
                        sheetName = sheetElement.Attributes["name"].InnerText;
                }

                XmlNodeList columnList = sheetElement.GetElementsByTagName("column");

                // Traverse the List of Columns
                foreach (XmlNode node2 in columnList)
                {
                    XmlElement columnElement = (XmlElement)node2;
                    String columnNumber = "";

                    if (columnElement.HasAttributes)
                    {
                        if (columnElement.Attributes[0].Name == "num")
                            columnNumber = columnElement.Attributes["num"].InnerText;
                    }

                    currentColumn = Int32.Parse(columnNumber);

                    XmlNodeList rowList = columnElement.GetElementsByTagName("row");

                    // Traverse the List of Rows
                    foreach (XmlNode node3 in rowList)
                    {
                        XmlElement rowElement = (XmlElement)node3;
                        String rowNumber = "";

                        if (rowElement.HasAttributes)
                        {
                            if (rowElement.Attributes[0].Name == "num")
                                rowNumber = rowElement.Attributes["num"].InnerText;
                        }

                        currentRow = Int32.Parse(rowNumber);

                        if (rowElement.GetElementsByTagName("content").Count != 0)
                        {
                            String formula = rowElement.GetElementsByTagName("content")[0].InnerText;
                            Cell cell = new Cell(formula);
                            cells[currentRow, currentColumn] = cell;
                        }

                    }
                }

                // Add current CellCollection to book
                book.Add(new SpreadsheetModel(cells));
            }

            return true;
        }
    }
}
