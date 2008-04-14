using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;

//NATHAN SULLIVAN- to serialize graphs
//using OpenTK.OpenGL;


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

   public DataIO()
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
     /***** TEST CODE *****/

     book.Add(new SpreadsheetModel(new CellCollection()));

     /***** END TEST CODE *****/

     SaveFileDialog saver = new SaveFileDialog();
     saver.AddExtension = true;
     saver.CheckPathExists = true;
     saver.DefaultExt = ".xml";
     saver.Filter = ".xml Files|*.xml";
     saver.InitialDirectory = "c:\\documents and settings\\" + Environment.UserName + "\\desktop\\";
     saver.ShowDialog();
     filename = saver.FileName;

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
         MessageBox.Show("Error: (" + e.GetType().ToString() + "): " +
                         e.Message + Environment.NewLine +
                         "Debug Data: " + e.ToString(), "Error");
       }
     }
     return false;
   }

   public SpreadsheetModel LoadBook()
   {
     OpenFileDialog opener = new OpenFileDialog();
     opener.AddExtension = true;
     opener.CheckFileExists = true;
     opener.CheckPathExists = true;
     opener.DefaultExt = ".xml";
     opener.Filter = ".xml Files|*.xml";
     opener.InitialDirectory = "c:\\documents and settings\\" + Environment.UserName + "\\desktop\\";

     if (opener.ShowDialog() == DialogResult.OK)
     {
       filename = opener.FileName;

       if (ReadBook())
       {
         return book[0];
       }
     }
     return new SpreadsheetModel(new CellCollection());
   }

     private bool WriteBook()
     {
         Cell theCell = new Cell();
         Cell defaultCell = new Cell();

         Cell tempCell = new Cell();

         /**** TEST Code*/

         /*CellCollection cc = new CellCollection();
         FontFamily blahFamily = new FontFamily("Arial");
         Font blahFont = new Font(blahFamily, 13);
         CellFormat blahFormat = new CellFormat(blahFont,Color.Orange, Color.Green);
         Font f = new Font(FontFamily.GenericSansSerif, 12.0F, FontStyle.Bold);
         f = new Font(f, f.Style | FontStyle.Italic);
         CellFormat blahFormat2 = new CellFormat(f, Color.Black, Color.Black);
         //blahFormat.CellColor = Color.Yellow;
         //blahFormat.TextColor = Color.Purple;
         //tempCell.CellFormat = blahFormat;

         cc[0, 0] = new Cell("= SUM()", blahFormat);
         cc[0, 0].Value = "42";
         cc[0, 1] = new Cell("", tempCell.CellFormat);
         cc[0, 2] = new Cell("", tempCell.CellFormat);
         cc[1, 0] = new Cell("", tempCell.CellFormat);
         cc[1, 0].Value = "blah";
         cc[1, 1] = new Cell("= AVG()", tempCell.CellFormat);
         cc[1, 1].Value = "47";
         cc[1, 2] = new Cell("", blahFormat2);
         cc[1, 2].Value = "99";
         cc[2, 0] = new Cell("", tempCell.CellFormat);
         cc[2, 0].Value = "YourFather";
         cc[2, 1] = new Cell("", tempCell.CellFormat);
         cc[2, 2] = new Cell("", tempCell.CellFormat);

         book[0] = new SpreadsheetModel(cc);
         //Cell theCell = new Cell();
       */
         //END TEST CODE

         double tempDouble = new double();
         bool blank = false;
         fileStream = new FileStream(filename, FileMode.Create);
         textWriter = new XmlTextWriter(fileStream, Encoding.Unicode);

         try
         {

             textWriter.WriteStartElement(filename.Substring(filename.LastIndexOf("\\") + 1,
                                          filename.LastIndexOf(".") - filename.LastIndexOf("\\") - 1));
             textWriter.WriteStartElement("sheet");
             textWriter.WriteStartElement("author");
             textWriter.WriteAttributeString("name", "Nathan Benjamin");
             textWriter.WriteEndElement();

             textWriter.WriteString(Environment.NewLine + "other metadata" + Environment.NewLine);

             int bookRows = book[0].Cells.Rows;
             int bookColumns = book[0].Cells.Columns;

             for (int column = 0; column < bookColumns; column++)
             {
                 textWriter.WriteStartElement("column");
                 textWriter.WriteAttributeString("index", column.ToString());


                 for (int row = 0; row < bookRows; row++)
                 {
                     theCell = book[0].Cells[row, column];
                     /*IS CELL DEFAULT CHECK*/
                     blank = false;
                     if (theCell == null || theCell.Value == null) { blank = true; }
                     else if (theCell.Value.Equals("")) { blank = true; }
                     /*CELL FORMAT needs to be instantiated*/
                     //if (theCell.CellFormat.IsDefault == false) { blank = true; }

                     if (blank == true)
                     {//skip writing the row, go on to the next cell.
                     }
                     else
                     {
                         textWriter.WriteStartElement("row");
                         textWriter.WriteAttributeString("index", row.ToString());

                         if (theCell != null)
                         {
                             /*CONTENT TYPE CHECK*/
                             textWriter.WriteStartElement("content");
                             if (theCell.Formula.Contains("="))
                             {
                                 textWriter.WriteAttributeString("type", "Formula");
                             }
                             else
                             {
                                 try
                                 {
                                     tempDouble = double.Parse(theCell.Value);
                                     textWriter.WriteAttributeString("type", "Number");
                                 }
                                 catch (System.FormatException)
                                 {
                                     textWriter.WriteAttributeString("type", "Text");
                                 }
                             }

                             textWriter.WriteAttributeString("CellFormat", theCell.CellFormat.serialize());


                             /*IF THE CELLFORMAT IS DEFAULT, WE WRITE NOTHING ABOUT CELL FORMAT
                             if(theCell.CellFormat.IsDefault){//skip the cell
                             }
                             else{
                                 //Each Format attribute is compared to the default cell. If that .Equals method returns false
                                 //Then theCell has an attribute different from defaultCell, and that attribute must be written.
                                 //CellColor
                                 if (theCell.CellFormat.CellColor.Equals(defaultCell.CellFormat.CellColor) == false)
                                     textWriter.WriteAttributeString("CellColor", theCell.CellFormat.CellColor.ToString());
                                 //TextColor
                                 if (theCell.CellFormat.TextColor.Equals(defaultCell.CellFormat.TextColor) == false)
                                     textWriter.WriteAttributeString("TextColor", theCell.CellFormat.TextColor.ToString());
                                 //Bold
                                 if (theCell.CellFormat.CellFont.Bold.Equals(defaultCell.CellFormat.CellFont.Bold) == false)
                                     textWriter.WriteAttributeString("Bold", "true");
                                 //Italics
                                 if (theCell.CellFormat.CellFont.Italic.Equals(defaultCell.CellFormat.CellFont.Italic) == false)
                                     textWriter.WriteAttributeString("Italic", "true");
                                 //Underline
                                 if (theCell.CellFormat.CellFont.Underline.Equals(defaultCell.CellFormat.CellFont.Underline) == false)
                                     textWriter.WriteAttributeString("Underline", "true");
                                 //FontFamily
                                 if (theCell.CellFormat.CellFont.FontFamily.Equals(defaultCell.CellFormat.CellFont.FontFamily) == false)
                                     textWriter.WriteAttributeString("FontFamily", theCell.CellFormat.CellFont.FontFamily.ToString());
                                 //Size
                                 if (theCell.CellFormat.CellFont.Size.Equals(defaultCell.CellFormat.CellFont.Size) == false)
                                     textWriter.WriteAttributeString("Size", theCell.CellFormat.CellFont.Size.ToString());
                             }//end of Format Check
                             */


                             //FORMULA CHECK
                             if (theCell.Formula.Equals("") || theCell.Formula.Equals(null))
                             {} //ignore
                             else{
                                 textWriter.WriteElementString("Formula", theCell.Formula.ToString());
                             }
                             //VALUE CHECK
                             if (theCell.Value.Equals("") || theCell.Value.Equals(null))
                             { } //ignore
                             else
                             {
                                 textWriter.WriteElementString("Value", theCell.Value.ToString());
                             }
                             textWriter.WriteEndElement();//end of content type element?
                         }//end of theCell != null if
                         /*****Where do all these other write elements correspond to?*/

                         textWriter.WriteEndElement();
                     }//end of blank cell check if/else
                     
                 }//end of row loop
                 textWriter.WriteEndElement();
             }//end of column loop
             textWriter.WriteEndElement();
             textWriter.WriteEndElement();
             textWriter.Flush();

             //NATHAN SULLIVAN:  Test code for graphs
             /*foreach(Control c in Controller.Instance.MainForm.Controls)
             {
                 if( c is Graphs.GraphControl )  //try to serialize all graph controls
                 {
                     ((Graphs.GraphControl)c).WriteXml(textWriter);
                 }
             }*/
         }
         catch (Exception e)
         {
             MessageBox.Show("Error: (" + e.GetType().ToString() + "): " +
                             e.Message + Environment.NewLine +
                             "Debug Data: " + e.ToString(), "Error");
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
         int currentRow, currentColumn;

         XmlDocument doc = new XmlDocument();

         try
         {
             doc.Load(filename);

             XmlNodeList sheetList = doc.GetElementsByTagName("sheet");

             // Traverse the List of Sheets
             foreach (XmlNode sheet in sheetList)
             {
                 CellCollection cells = new CellCollection();
                 XmlElement sheetElement = (XmlElement)sheet;
                 String sheetName = "";

                 if (sheetElement.HasAttributes)
                 {
                     if (sheetElement.Attributes[0].Name == "name")
                         sheetName = sheetElement.Attributes["name"].InnerText;
                 }

                 XmlNodeList columnList = sheetElement.GetElementsByTagName("column");

                 // Traverse the List of Columns
                 foreach (XmlNode column in columnList)
                 {
                     XmlElement columnElement = (XmlElement)column;
                     String columnNumber = "";

                     if (columnElement.HasAttributes)
                     {
                         if (columnElement.Attributes[0].Name == "index")
                             columnNumber = columnElement.Attributes["index"].InnerText;
                     }

                     currentColumn = Int32.Parse(columnNumber);

                     XmlNodeList rowList = columnElement.GetElementsByTagName("row");

                     // Traverse the List of Rows
                     foreach (XmlNode row in rowList)
                     {
                         XmlElement rowElement = (XmlElement)row;
                         String rowNumber = "";

                         if (rowElement.HasAttributes)
                         {
                             if (rowElement.Attributes[0].Name == "index")
                                 rowNumber = rowElement.Attributes["index"].InnerText;
                         }

                         currentRow = Int32.Parse(rowNumber);

                         for (int j = 0; j < rowElement.Attributes.Count; j++)
                         {
                             //MessageBox.Show("Row: " + rowNumber + " Column: " + columnNumber + "\n");

                             if (rowElement.GetElementsByTagName("content").Count != 0)
                             {
                                 XmlElement contentElement = (XmlElement)rowElement.GetElementsByTagName("content")[0];
                                 Cell cell = new Cell();

                                 if (contentElement.HasAttribute("CellFormat"))
                                 {
                                     String settings = contentElement.GetAttribute("CellFormat");
                                     cell.CellFormat = CellFormatFactory.createCellFormat(settings);
                                 }

                                 if (contentElement.GetElementsByTagName("Formula").Count != 0)
                                 {
                                     String formula = contentElement.GetElementsByTagName("Formula")[0].InnerText;
                                     cell.Formula = formula;
                                 }

                                 if (contentElement.GetElementsByTagName("Value").Count != 0)
                                 {
                                     String value = contentElement.GetElementsByTagName("Value")[0].InnerText;
                                     cell.Value = value;
                                 }

                                 cells[currentRow, currentColumn] = cell;
                             }
                         }

                     }
                 }

                 // Add current CellCollection to book
                 book[0] = new SpreadsheetModel(cells);
             }
         }
         catch (Exception e)
         {
             MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message + Environment.NewLine + e.ToString(), "Error");
             return false;
         }
         return true;
     }

 }
}