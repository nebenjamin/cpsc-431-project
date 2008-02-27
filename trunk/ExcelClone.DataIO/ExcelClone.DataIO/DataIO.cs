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
				//if (book.Count == 0) throw new MissingMemberException("Passed an empty book!  Feed me data");
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
			Cell temp = new Cell();

			fileStream = new FileStream(filename, FileMode.Create);
			textWriter = new XmlTextWriter(fileStream, Encoding.Unicode);

			string fontTags = "";

			try
			{

				/************  HEY SCOTT!!!!!!!!!! ************/
				//This is how Nathan and Chris want the file formmating to be. Talk to us
				//if you have any questions

				textWriter.WriteStartElement(filename.Substring(filename.LastIndexOf("\\") + 1));
				textWriter.WriteString(nl);
				
				textWriter.WriteStartElement("sheet");
				textWriter.WriteString(nl);

				/*if (book[0].Author.Equals("")) textWriter.WriteElementString("author = " + Environment.UserName, "");
				else*/ textWriter.WriteElementString("author = " + "Nathan Benjamin", "");
				textWriter.WriteString(nl + "other metadata" + nl);
				textWriter.WriteEndElement();
				
				textWriter.WriteString(nl);

				/**********************************************/


				int rows = book[0].Cells.Rows;
				int cols = book[0].Cells.Columns;
				textWriter.WriteString("<columns number = " + cols);
				textWriter.WriteString(" />\n<rows number = " + rows + " />\n");

				//Scan each column value, then go down a row and scan each column again.
				for (int y = 1; y < cols; y++)
				{
					textWriter.WriteString("<column " + y + ">\n");
					for (int x = 1; x < rows; x++)
					{
						temp = book[0].Cells[x, y];

						fontTags = "";
						bool theTruth = temp.Formula.Equals(null);
						String s = "";
						int tempT = -1;
						//Color tempC = new Color();
						//tempC = temp.CellFormat.BackgroundBrush.BackColor;

						//s = temp.CellFormat.BackgroundBrush.GetType();
						/* if (temp.CellFormat == null)
						 {//Leave the cell blank
						 }
						 else
						 {

							 if (temp.CellFormat.Font.Equals(null) == true)
							 {//ignore
							 }
							 else
							 {
								  textWriter.WriteString("<row " + x + ">\n");
								  if (tempFormat.Font.fontFamily() != "Times New Roman")
								  {
									  textWriter.WriteString("<text-font = " + tempFormat.Font.fontFamily.get() + ">");
									  fontTags = fontTags + "</text-font>";
								  }
								  if (tempFormat.Font.emSize.get() != 12)
								  {
									  textWriter.WriteString("text-size = " << tempFormat.Font.emSize.get() << ">");
									  fontTags = fontTags + "</text-size>";
								  }
								  if (tempFormat.Font.bold == true){
									  textWriter.WriteString("<b>");
									  fontTags = fontTags + "</b>";
								  }
								  if (tempFormat.Font.italics == true){
									  textWriter.WriteString("<i>");
									  fontTags = fontTags + "</i>";
								  }
								  if (tempFormat.Font.underlined == true){
									  textWriter.WriteString("<u>");
									  fontTags = fontTags + "</u>";
								  }
							 }//end of Font check

                            
							 if (temp.CellFormat.FontBrush.Equals(null) == true)
							 {//ignore
							 }
							 else
							 {
								 if (tempFormat.FontBrush.color.get() != "#000000"){
									textWriter.WriteString("<text-color = " << tempFormat.FontBrush.Color.get() << ">");
									fontTags = fontTags + "</text-color>";
								 }
								 if (tempFormat.FontBrush.brushStyle.get() != null){
									 textWriter.WriteString("<brush-style = " << tempFormat.FontBrush.BrushStyle.get() << ">");
									 fontTags = fontTags + "</brush-style>";
								 }
								 if (tempFormat.FontBrush.backColor.get() != "#FFFFFF"){
									 textWriter.WriteString("cell-bg-color = " << tempFormat.FontBrush.backColor.get() << ">");
									 fontTags = fontTags + "</cell-bg-color>";
								 }
							 }
						 }//end of Format check
						  */
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



					}//end of row loop
					textWriter.WriteString("</column>\n");
				}//end of col loop*/

				textWriter.WriteString("</sheet>\n");
				textWriter.WriteString("</template>\n");
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
			}
			return true;
		}

		private bool ReadBook()
		{
			return true;
		}
	}
}
