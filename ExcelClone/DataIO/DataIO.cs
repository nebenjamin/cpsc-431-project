using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;


using ExcelClone.Core;

namespace ExcelClone.DataIO
{
	public class DataIO
	{
		public const string extension = ".xml";

		private string filename;

    private Cell defaultCell;
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
            /***** TEST CODE *****/

            book.Add(new SpreadsheetModel(new CellCollection()));

            /***** END TEST CODE *****/
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
            Cell theCell = new Cell();

            fileStream = new FileStream(filename, FileMode.Create);
            textWriter = new XmlTextWriter(fileStream, Encoding.Unicode);

            string startTags = "";
            string endTags = "";

            /*try
            {
                textWriter.WriteStartElement(filename.Substring(filename.LastIndexOf("\\") + 1,
                                                                filename.LastIndexOf(".") - filename.LastIndexOf("\\") - 1));
                textWriter.WriteString(Environment.NewLine);

                textWriter.WriteStartElement("sheet");
                textWriter.WriteString(Environment.NewLine);

                /*if (book[0].Author.Equals("")) textWriter.WriteElementString("author = " + Environment.UserName, "");
                else*//*
                textWriter.WriteStartElement("author", "Nathan Benjamin");
                textWriter.WriteEndElement();
                textWriter.WriteString(Environment.NewLine + "other metadata" + Environment.NewLine);


                int rows = book[0].Cells.Rows;
                int cols = book[0].Cells.Columns;

                textWriter.WriteStartElement("columns", cols.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteString(Environment.NewLine);

                textWriter.WriteStartElement("rows", rows.ToString());
                textWriter.WriteEndElement();
                textWriter.WriteString(Environment.NewLine);

                //Scan each column value, then go down a row and scan each column again.
                for (int y = 1; y < cols; y++)
                {
                    textWriter.WriteElementString("column ", y.ToString());
                    textWriter.WriteString(Environment.NewLine);
                    for (int x = 1; x < rows; x++)
                    {
                        theCell = book[0].Cells[x, y];

                        startTags = "";
                        endTags = "";
                        bool formulaBool = theCell.Formula.Equals("");
                        bool valueBool = theCell.Value.Equals("");
                        bool formatBool = theCell.CellFormat.Equals(null);
                        String s = "";
                        Color tempC = new Color();

                        //tempC.ToString();
                        //theCell.CellFormat.BackgroundBrush.Color.Equals(Color.Red);
                        /*LETS FIRST DEAL WITH FORMATTING*//*
                        if (formatBool == true)
                        {//change nothing
                        }
                        else
                        {

                            if (theCell.CellFormat.Font.Equals(null) == true)
                            {//ignore
                            }
                            else
                            {
                                //textWriter.WriteElementString("<row " + x + ">\n", "");
                                if (theCell.CellFormat.Font.FontFamily.ToString() != "Times New Roman")
                                {
                                    textWriter.WriteElementString(theCell.CellFormat.Font.FontFamily.GetType().ToString(), theCell.CellFormat.Font.FontFamily.ToString());
                                    //textWriter.WriteString("<text-font = " + theCell.CellFormat.Font.FontFamily.ToString() + ">");
                                    //fontTags = fontTags + "</text-font>";
                                }
                                if (theCell.CellFormat.Font.EmSize.Equals(12))
                                { }//default, do nothing
                                else
                                {
                                    textWriter.WriteElementString(theCell.CellFormat.Font.EmSize.GetType().ToString(), theCell.CellFormat.Font.EmSize.ToString());
                                    textWriter.WriteEndElement();

                                }
                                if (theCell.CellFormat.Font.Bold.Equals(true))
                                {
                                    startTags = startTags + "<b>";
                                    endTags = endTags + "</b>";
                                }
                                if (theCell.CellFormat.Font.Italics.Equals(true))
                                {
                                    startTags = startTags + "<i>";
                                    endTags = endTags + "</i>";
                                }
                                if (theCell.CellFormat.Font.Underlined.Equals(true))
                                {
                                    startTags = startTags + "<u>";
                                    endTags = endTags + "</u>";
                                }
                            }//end of Font check

                            if (theCell.CellFormat.FontBrush.Equals(null))
                            {//ignore
                            }
                            else
                            {
                                /* if (theCell.CellFormat.FontBrush.color.get() != "#000000")
                                 {
                                     textWriter.WriteString("<text-color = " << theCell.CellFormat.FontBrush.Color.get() << ">");
									fontTags = fontTags + "</text-color>";
								 }
                                 if (theCell.CellFormat.FontBrush.brushStyle.get() != null)
                                 {
									 textWriter.WriteString("<brush-style = " << theCell.CellFormat.FontBrush.BrushStyle.get() << ">");
									 fontTags = fontTags + "</brush-style>";
								 }
								 if (theCell.CellFormat.FontBrush.backColor.get() != "#FFFFFF"){
									 textWriter.WriteString("cell-bg-color = " << theCell.CellFormat.FontBrush.backColor.get() << ">");
									 fontTags = fontTags + "</cell-bg-color>";
								 }*/
                       //     }
                     //   }//end of Format check

                        /*if (temp.Formula.Equals(null) == true)
                        {
                            if (temp.Formula.Value(null) == true)
                            {
                               }
                               else{//write the value
                                   if(tempFormat == null){//we haven't printed out row yet
                                   textWriter.WriteString("<row " << x << ">\n");
                                   }
                                   textWriter.WriteString("<content = number>\n" << tempCell.Value.get());
                                   textWriter.WriteString("\n</content>\n");
                                   textWriter.WriteString("</row>\n");
                               }
                           }
                           else{//write the formula
                               if(tempFormat == null){//we haven't printed out row yet
                                     textWriter.WriteString("<row " << x << ">\n");
                                 }
                                     textWriter.WriteString("<content = formula>\n" << tempCell.Formula.get());
                                     textWriter.WriteString("\n</content>\n");
                                     textWriter.WriteString("</row>\n");
                        
                             }*/

  /*                  }//end of row loop
                    textWriter.WriteEndElement();
                    textWriter.WriteString(Environment.NewLine);
                }//end of col loop*/
/*
                textWriter.WriteEndElement();
                textWriter.WriteString(Environment.NewLine);
                textWriter.Flush();

            }
            catch (Exception e)
            {
                MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message, "Error");
            }
            finally
            {
                if (textWriter != null)
                {
                    textWriter.Close();
                    textWriter = null;
                }
            }*/
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
