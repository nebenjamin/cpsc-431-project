using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using ExcelClone.Gui;

//NATHAN SULLIVAN- to serialize graphs
using OpenTK.OpenGL;
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

    public SpreadsheetModel GetSpreadsheet(int pagenumber)
    {
      if (book != null)
      {
        return book[pagenumber];
      }
      else
      {
        return new SpreadsheetModel(new CellCollection());
      }
    }

    public bool SetBook(List<SpreadsheetModel> volume)
    {
      book = volume;
      return true;
    }

    public List<SpreadsheetModel> GetBook()
    {
      if (book != null)
      {
        return book;
      }
      else
      {
        book.Add(new SpreadsheetModel(new CellCollection()));
        return book;
      }
    }

    public bool SaveBook(bool saveAs)
    {
      try
      {
        if (filename == null || saveAs)
        {
          SaveFileDialog saver = new SaveFileDialog();
          saver.AddExtension = true;
          saver.CheckPathExists = true;
          saver.DefaultExt = ".xml";
          saver.Filter = ".xml Files|*.xml";
          saver.InitialDirectory = "c:\\documents and settings\\" + Environment.UserName + "\\desktop\\";
          if (saver.ShowDialog() == DialogResult.OK)
          {
            filename = saver.FileName;

            if (book.Count == 0) throw new MissingMemberException("Passed an empty book!  Feed me data");
            return WriteBook();
          }
        }
        else
        {
          return WriteBook();
        }
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

    public List<SpreadsheetModel> LoadBook()
    {
      OpenFileDialog opener = new OpenFileDialog();
      opener.AddExtension = true;
      opener.CheckFileExists = true;
      opener.CheckPathExists = true;
      opener.DefaultExt = ".xml";
      opener.Filter = ".xml Files|*.xml";
      opener.InitialDirectory = "c:\\documents and settings\\" + Environment.UserName + "\\desktop\\";
      DialogResult result = opener.ShowDialog();
      if (result == DialogResult.OK)
      {
        filename = opener.FileName;
        book.Clear();
        if (ReadBook())
        {
          return book;
        }
      }
      else if (result == DialogResult.Cancel)
      {
        return book;
      }
      book = new List<SpreadsheetModel>();
      book.Add(new SpreadsheetModel(new CellCollection()));
      return book;
    }

    private bool WriteBook()
    {
      int i = 0;
      Cell theCell = new Cell();
      Cell defaultCell = new Cell();

      double tempDouble = new double();
      bool blank = false;
      fileStream = new FileStream(filename, FileMode.Create);
      textWriter = new XmlTextWriter(fileStream, Encoding.Unicode);

      try
      {
        textWriter.WriteStartElement(filename.Substring(filename.LastIndexOf("\\") + 1,
                                     filename.LastIndexOf(".") - filename.LastIndexOf("\\") - 1));

        for (int bookIndex = 0; bookIndex < book.Count; bookIndex++)
        {
          textWriter.WriteStartElement("sheet");
          textWriter.WriteAttributeString("index", bookIndex.ToString());

          int bookRows = book[bookIndex].Cells.Rows;
          int bookColumns = book[bookIndex].Cells.Columns;

          for (int column = 0; column < bookColumns; column++)
          {
            textWriter.WriteStartElement("column");
            textWriter.WriteAttributeString("index", column.ToString());
            //textWriter.WriteAttributeString("width", sv[column, 0].Size.Width.ToString());
            //sv[0,0].Size.Height;
            for (int row = 0; row < bookRows; row++)
            {
              theCell = book[bookIndex].Cells[row, column];



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
                //textWriter.WriteAttributeString("height", sv[0, row].Size.Height.ToString());

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
                    catch (System.FormatException e)
                    {
                      textWriter.WriteAttributeString("type", "Text");
                    }
                  }

                  textWriter.WriteAttributeString("CellFormat", theCell.CellFormat.serialize());

                  //FORMULA CHECK
                  if (theCell.Formula.Equals("") || theCell.Formula.Equals(null))
                  { } //ignore
                  else
                  {
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
          //NATHAN SULLIVAN:  Test code for graphs
          System.Windows.Forms.TabControl.TabPageCollection tabs = Controller.Instance.GetMainTabPageCollection();


          foreach (Control c in tabs[i].Controls)
          {
            if (c is Graphs.GraphControl)  //try to serialize all graph controls
            {
              textWriter.WriteStartElement("graph");
              ((Graphs.GraphControl)c).WriteXml(textWriter);
              textWriter.WriteEndElement();
            }
          }

          textWriter.WriteEndElement();
          ++i;
        }
        textWriter.WriteEndElement();
        textWriter.Flush();
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
      int i = 0;
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
          /*XmlNodeList graphs = doc.GetElementsByTagName("graph");
          XmlTextReader textReader;

          foreach (XmlNode graph in graphs)
          {
            try
            {
              textReader = new XmlTextReader(new StringReader(graph.InnerXml));
              textReader.Read();
              Graphs.GraphControl g = new ExcelClone.Graphs.GraphControl();
              g.ReadXml(textReader);

              //Controller.Instance.MainForm.WorksheetsTabControl.SelectedTab.Controls.Add(gc);
              //gc.BringToFront();
              
              tabs[i].Controls.Add(g);
              g.BringToFront();
              tabs[i].Refresh();
              
              
              //Controller.Instance.MainForm.Controls.Add(g);
              //g.BringToFront();
              /*foreach (Control c in tabs[i].Controls)
              {
                if (c is Graphs.GraphControl)
                  c.BringToFront();
              }*/
          /*
     }
     catch (Exception e)
     {
       MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message + Environment.NewLine + e.ToString(), "Error");
     }
   }*/

          // Add current CellCollection to book
          book.Add(new SpreadsheetModel(cells));
          ++i;
        }
      }
      catch (Exception e)
      {
        MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message + Environment.NewLine + e.ToString(), "Error");
        return false;
      }
      return true;
    }

    public bool ReadGraphs()
    {
      XmlDocument doc = new XmlDocument();
      if(filename == null) return true;
      doc.Load(filename);
      System.Windows.Forms.TabControl.TabPageCollection tabs = Controller.Instance.GetMainTabPageCollection();

      XmlNodeList sheetList = doc.GetElementsByTagName("sheet");

      // Traverse the List of Sheets
      foreach (XmlNode sheet in sheetList)
      {
        XmlNodeList graphs = doc.GetElementsByTagName("graph");
        XmlTextReader textReader;

        int i = 0;
        foreach (XmlNode graph in graphs)
        {
          try
          {
            textReader = new XmlTextReader(new StringReader(graph.InnerXml));
            textReader.Read();
            Graphs.GraphControl g = new ExcelClone.Graphs.GraphControl();
            g.ReadXml(textReader);

            //Controller.Instance.MainForm.WorksheetsTabControl.SelectedTab.Controls.Add(gc);
            //gc.BringToFront();

            tabs[i].Controls.Add(g);
            g.BringToFront();
            tabs[i].Refresh();


            //Controller.Instance.MainForm.Controls.Add(g);
            //g.BringToFront();
            /*foreach (Control c in tabs[i].Controls)
            {
              if (c is Graphs.GraphControl)
                c.BringToFront();
            }*/
          }
          catch (Exception e)
          {
            MessageBox.Show("Error: (" + e.GetType().ToString() + "): " + e.Message + Environment.NewLine + e.ToString(), "Error");
          }
        }
        ++i;
      }
      return true;
    }

  }
}