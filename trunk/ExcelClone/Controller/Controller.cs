using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using ExcelClone.Functions;
using ExcelClone.Gui;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace ExcelClone
{
  class Controller
  {
    private DataIO.DataIO saveload;
    private Controller()
    {
      Parser = new Parser();
      isCut = false;
      isPaste = false;
      isCopy = false;
    }

    private Window mainForm;

    public Window MainForm
    {
      get
      {
        return mainForm;
      }
      set
      {
        mainForm = value;
      }
    }

    #region Clipboard

    private CellCollection clipboardCells;
    private bool isCut;
    private bool isPaste;
    private bool isCopy;

    #endregion

    private Parser parser;
    public Parser Parser
    {
      get { return parser; }
      set { parser = value; }
    }
    private SpreadsheetUserControl ActiveWS = null;
      public List<SpreadsheetModel> GetAllSpreadsheetModels()
      {
          System.Windows.Forms.TabControl.TabPageCollection pages = mainForm.WorksheetsTabControl.TabPages;
          List<SpreadsheetModel> spreadsheetModels = new List<SpreadsheetModel>();
          for (int i = 0; i < pages.Count; i++)
          {
              spreadsheetModels.Add(((SpreadsheetUserControl)pages[i].Controls[0]).Spreadsheet.SpreadsheetModel);
          }
          return spreadsheetModels;
      }
      public void ExecuteCommand(object sender, EventArgs e, CommandType command)
    {
        if (command != CommandType.InsertWorksheet)
        {
            try
            {
                ActiveWS = (SpreadsheetUserControl)mainForm.WorksheetsTabControl.SelectedTab.Controls[0];
            }
            catch (Exception exc)
            {
            }
        }
      switch (command)
      {
        case CommandType.Exit:
          ExecuteExit();
          break;
        case CommandType.Open:
          ExecuteOpen();
          break;
        case CommandType.Save:
          ExecuteSave();
          break;
        case CommandType.SaveAs:
          ExecuteSaveAs();
          break;
        case CommandType.Cut:
          ExecuteCut();
          break;
        case CommandType.Copy:
          ExecuteCopy();
          break;
        case CommandType.Paste:
          ExecutePaste();
          break;
        case CommandType.InsertBarGraph:
          ExecuteInsertBarGraph();
          break;
        case CommandType.InsertColumnGraph:
          ExecuteInsertColumnGraph();
          break;
        case CommandType.InsertLineGraph:
          ExecuteInsertLineGraph();
          break;
        case CommandType.InsertPieGraph:
          ExecuteInsertPieGraph();
          break;
        case CommandType.InsertScatterGraph:
          ExecuteInsertScatterGraph();
          break;
        case CommandType.FormatCells:
          ExecuteFormatCells((sender as ToolStripButton).Name);
          break;
        case CommandType.InsertWorksheet:
          ExecuteInsertWorksheet();
          break;
        case CommandType.SelectTextColor:
          ExecuteSelectTextColor((e as ColorEventArgs).Color);
          break;
        case CommandType.SelectCellColor:
          ExecuteSelectCellColor((e as ColorEventArgs).Color);
          break;
        case CommandType.ChangeFont:
          ExecuteChangeFont((e as FontEventArgs));
          break;
          case CommandType.DeleteActiveWS:
              ExecuteDeleteActiveWS();
              break;
        default:
          break;
      }
    }

      private void ExecuteDeleteActiveWS()
      {
          TabPage activeTab = mainForm.WorksheetsTabControl.SelectedTab;
          try
          {
              mainForm.WorksheetsTabControl.TabPages.Remove(activeTab);
          }
          catch (Exception e) { }
          
      }

    public void ExecuteNew()
    {
    }
    public void ExecuteOpen()
    {
      saveload = new ExcelClone.DataIO.DataIO();
      ActiveWS.Spreadsheet.EndEdit();
      //saveload = new ExcelClone.DataIO.DataIO(ActiveWS);
      //saveload.AddSpreadsheet(ActiveWS.Spreadsheet.SpreadsheetModel);
      List<SpreadsheetModel> lister = saveload.LoadBook();
      mainForm.WorksheetsTabControl.TabPages.Clear();
      foreach (SpreadsheetModel sm in lister)
      {
          ExecuteInsertWorksheet(sm);
      }
      /**/
      //Create sheets from lister
      /**/
      //ActiveWS.Spreadsheet.SpreadsheetModel = lister[0];
      //ActiveWS.Spreadsheet.RefreshView();
    }
    public void ExecuteClose()
    {
    }
    public void ExecuteSave()
    {
      saveload = new ExcelClone.DataIO.DataIO();
      ActiveWS.Spreadsheet.EndEdit();
      //saveload = new ExcelClone.DataIO.DataIO(ActiveWS);
      saveload.AddSpreadsheet(ActiveWS.Spreadsheet.SpreadsheetModel);
      saveload.SaveBook(false);
    }
    public void ExecuteSaveAs()
    {
      saveload = new ExcelClone.DataIO.DataIO();
      ActiveWS.Spreadsheet.EndEdit();
      //saveload = new ExcelClone.DataIO.DataIO(ActiveWS);
      saveload.AddSpreadsheet(ActiveWS.Spreadsheet.SpreadsheetModel);
      saveload.SaveBook(true);
    }
    public void ExecuteCut()
    {

      ExecuteCopy();
      foreach (DataGridViewCell cell in ActiveWS.Spreadsheet.SelectedCells)
      {
        cell.Value = "";
        if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
        {
          Cell c = ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
          if (c != null)
          {
            c.Value = "";
            c.Formula = "";
          }
        }
      }

      isPaste = false;
      isCopy = false;
      isCut = true;
    }

    public void ExecuteCopy()
    {
      CellCollection cellCollection = new CellCollection();
      int smallestRowIndex = GetSmallestRowIndex(ActiveWS.Spreadsheet.SelectedCells);
      int smallestColumnIndex = GetSmallestColumnIndex(ActiveWS.Spreadsheet.SelectedCells);
      foreach (DataGridViewCell cell in ActiveWS.Spreadsheet.SelectedCells)
      {
        cellCollection[new CellKey(cell.RowIndex - smallestRowIndex, cell.ColumnIndex - smallestColumnIndex)] = (ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex] != null) ? (Cell)ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex].Clone() : new Cell();
      }
      clipboardCells = cellCollection;
      isCopy = true;
    }
    private int GetSmallestRowIndex(DataGridViewSelectedCellCollection cells)
    {
      int minRowIndex = ActiveWS.Spreadsheet.RowCount - 1;
      foreach (DataGridViewCell cell in cells)
      {
        if (cell.RowIndex < minRowIndex)
          minRowIndex = cell.RowIndex;
      }
      return minRowIndex;
    }
    private int GetSmallestColumnIndex(DataGridViewSelectedCellCollection cells)
    {
      int minColumnIndex = ActiveWS.Spreadsheet.ColumnCount - 1;
      foreach (DataGridViewCell cell in cells)
      {
        if (cell.ColumnIndex < minColumnIndex)
          minColumnIndex = cell.ColumnIndex;
      }
      return minColumnIndex;
    }
    public void ExecutePaste()
    {
      if (ActiveWS.Spreadsheet.SelectedCells.Count > 0 &&
          ((isCut && !isPaste) ||
          isCopy))
      {
        int smallestRowIndex = GetSmallestRowIndex(ActiveWS.Spreadsheet.SelectedCells);
        int smallestColumnIndex = GetSmallestColumnIndex(ActiveWS.Spreadsheet.SelectedCells);
        for (int r = 0; r < clipboardCells.Rows; r++)
        {
          for (int c = 0; c < clipboardCells.Columns; c++)
          {
            if (smallestRowIndex + r < ActiveWS.Spreadsheet.RowCount && smallestColumnIndex + c < ActiveWS.Spreadsheet.ColumnCount)
              ActiveWS.Spreadsheet.SpreadsheetModel.Cells[smallestRowIndex + r, smallestColumnIndex + c] =
              (Cell)(clipboardCells[r, c].Clone());

          }
        }
        ActiveWS.Spreadsheet.RefreshView();
        //need check for out of bounds
        isPaste = true;
        isCut = false;

      }
    }
    public void ExecuteInsertBarGraph()
    {
      Rectangle r = new Rectangle(0, 50, 400, 300);

      //Done by David, Caleb, & Scott
      int cellCount = ActiveWS.Spreadsheet.SelectedCells.Count;
      int max_col, min_col, max_row, min_row;
      max_col = max_row = 0;
      min_col = min_row = 51;
      for (int i = 0; i < cellCount; i++)
      {
        int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
        int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
        if (rI < min_row)
          min_row = rI;
        if (rI > max_row)
          max_row = rI;
        if (cI < min_col)
          min_col = cI;
        if (cI > max_col)
          max_col = cI;
      }
      int colCount = max_col - min_col + 1;
      int rowCount = max_row - min_row + 1;

      if (colCount >= 1 && rowCount >= 1)
      {
        string[][] data = new string[rowCount][];
        for (int i = 0; i < data.Length; i++)
          data[i] = new string[colCount];
        for (int i = 0; i < cellCount; i++)
        {
          int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
          int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
          if (Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI] != null)
            data[rI - min_row][cI - min_col] = Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI].Value;
        }
        try
        {
          Graphs.Graph gr = Graphs.bar_graph.Create_Bar_Graph(r, data);
        }
        catch (ArgumentException)
        {
          MessageBox.Show("Invalid Data");
        }
      }
      else
      {
        MessageBox.Show("Incorrect Number of Columns");
      }
    }
    public void ExecuteInsertColumnGraph()
    {
      Rectangle r = new Rectangle(0, 50, 400, 300);

      //Done by David, Caleb, & Scott
      int cellCount = ActiveWS.Spreadsheet.SelectedCells.Count;
      int max_col, min_col, max_row, min_row;
      max_col = max_row = 0;
      min_col = min_row = 51;
      for (int i = 0; i < cellCount; i++)
      {
        int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
        int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
        if (rI < min_row)
          min_row = rI;
        if (rI > max_row)
          max_row = rI;
        if (cI < min_col)
          min_col = cI;
        if (cI > max_col)
          max_col = cI;
      }
      int colCount = max_col - min_col + 1;
      int rowCount = max_row - min_row + 1;

      if (colCount >= 1 && rowCount >= 1)
      {
        string[][] data = new string[rowCount][];
        for (int i = 0; i < data.Length; i++)
          data[i] = new string[colCount];
        for (int i = 0; i < cellCount; i++)
        {
          int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
          int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
          if (Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI] != null)
            data[rI - min_row][cI - min_col] = Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI].Value;
        }
        try
        {
          Graphs.Graph gr = Graphs.column_graph.Create_Column_Graph(r, data);
        }
        catch (ArgumentException)
        {
          MessageBox.Show("Invalid Data");
        }
      }
      else
      {
        MessageBox.Show("Incorrect Number of Columns");
      }
    }
    public void ExecuteInsertLineGraph()
    {
      Rectangle r = new Rectangle(0, 50, 400, 300);

      //Done by David, Caleb, & Scott
      int cellCount = ActiveWS.Spreadsheet.SelectedCells.Count;
      int max_col, min_col, max_row, min_row;
      max_col = max_row = 0;
      min_col = min_row = 51;
      for (int i = 0; i < cellCount; i++)
      {
        int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
        int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
        if (rI < min_row)
          min_row = rI;
        if (rI > max_row)
          max_row = rI;
        if (cI < min_col)
          min_col = cI;
        if (cI > max_col)
          max_col = cI;
      }
      int colCount = max_col - min_col + 1;
      int rowCount = max_row - min_row + 1;

      if (colCount >= 2 && rowCount >= 1)
      {
        string[][] data = new string[rowCount][];
        for (int i = 0; i < data.Length; i++)
          data[i] = new string[colCount];
        for (int i = 0; i < cellCount; i++)
        {
          int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
          int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
          if (Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI] != null)
            data[rI - min_row][cI - min_col] = Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI].Value;
        }
        try
        {
          Graphs.Graph gr = Graphs.line_graph.Create_Line_Graph(r, data);
        }
        catch (ArgumentException)
        {
          MessageBox.Show("Invalid Data");
        }
      }
      else
      {
        MessageBox.Show("Incorrect Number of Columns");
      }
    }
    public void ExecuteInsertPieGraph()
    {
      Rectangle r = new Rectangle(0, 50, 400, 300);

      //Done by David, Caleb, & Scott
      int cellCount = ActiveWS.Spreadsheet.SelectedCells.Count;
      int max_col, min_col, max_row, min_row;
      max_col = max_row = 0;
      min_col = min_row = 51;
      for (int i = 0; i < cellCount; i++)
      {
        int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
        int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
        if (rI < min_row)
          min_row = rI;
        if (rI > max_row)
          max_row = rI;
        if (cI < min_col)
          min_col = cI;
        if (cI > max_col)
          max_col = cI;
      }
      int colCount = max_col - min_col + 1;
      int rowCount = max_row - min_row + 1;

      if (colCount == 1 && rowCount >= 1)
      {
        string[][] data = new string[rowCount][];
        for (int i = 0; i < data.Length; i++)
          data[i] = new string[colCount];
        for (int i = 0; i < cellCount; i++)
        {
          int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
          int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
          if (Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI] != null)
            data[rI - min_row][cI - min_col] = Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI].Value;
        }
        try
        {
          Graphs.Graph gr = Graphs.pie_graph.Create_Pie_Graph(r, data);
        }
        catch (ArgumentException)
        {
          MessageBox.Show("Invalid Data");
        }
      }
      else
      {
        MessageBox.Show("Incorrect Number of Columns");
      }
    }
    public void ExecuteInsertScatterGraph()
    {
      Rectangle r = new Rectangle(0, 50, 400, 300);

      //Done by David, Caleb, & Scott
      int cellCount = ActiveWS.Spreadsheet.SelectedCells.Count;
      int max_col, min_col, max_row, min_row;
      max_col = max_row = 0;
      min_col = min_row = 51;
      for (int i = 0; i < cellCount; i++)
      {
        int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
        int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
        if (rI < min_row)
          min_row = rI;
        if (rI > max_row)
          max_row = rI;
        if (cI < min_col)
          min_col = cI;
        if (cI > max_col)
          max_col = cI;
      }
      int colCount = max_col - min_col + 1;
      int rowCount = max_row - min_row + 1;

      if (colCount >= 2 && rowCount >= 1)
      {
        string[][] data = new string[rowCount][];
        for (int i = 0; i < data.Length; i++)
          data[i] = new string[colCount];
        for (int i = 0; i < cellCount; i++)
        {
          int rI = ActiveWS.Spreadsheet.SelectedCells[i].RowIndex;
          int cI = ActiveWS.Spreadsheet.SelectedCells[i].ColumnIndex;
          if (Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI] != null)
            data[rI - min_row][cI - min_col] = Controller.Instance.ActiveWS.Spreadsheet.SpreadsheetModel.Cells[rI, cI].Value;
        }
        try
        {
          Graphs.Graph gr = Graphs.scatter_graph.Create_Scatter_Graph(r, data);
        }
        catch (ArgumentException e)
        {
          MessageBox.Show("Invalid Data");
        }
      }
      else
      {
        MessageBox.Show("Incorrect Number of Columns");
      }
    }
    public void ExecuteInsertWorksheet()
    {
      System.Windows.Forms.TabPage newWSPage = new TabPage("Worksheet " +
          (MainForm.WorksheetsTabControl.Controls.Count + 1));

      MainForm.WorksheetsTabControl.Controls.Add(newWSPage);
      newWSPage.Location = new System.Drawing.Point(4, 22);
      newWSPage.Padding = new System.Windows.Forms.Padding(3);
      newWSPage.Size = mainForm.Size;
      newWSPage.TabIndex = 0;
      newWSPage.UseVisualStyleBackColor = true;
      SpreadsheetUserControl ws = new SpreadsheetUserControl();
      ws.Spreadsheet.KeyDown += new KeyEventHandler(SpreadsheetView_KeyDown);
      ws.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      ws.Location = new System.Drawing.Point(0, 0);
      ws.Name = "spreadsheetUserControl" +
          (MainForm.WorksheetsTabControl.Controls.Count + 1);
      ws.Size = newWSPage.Size;
      newWSPage.Controls.Add(ws);
      ws.Spreadsheet.RefreshView();
    }
      public void ExecuteInsertWorksheet(SpreadsheetModel spreadsheetModel)
      {
          System.Windows.Forms.TabPage newWSPage = new TabPage("Worksheet " +
              (MainForm.WorksheetsTabControl.Controls.Count + 1));

          MainForm.WorksheetsTabControl.Controls.Add(newWSPage);
          newWSPage.Location = new System.Drawing.Point(4, 22);
          newWSPage.Padding = new System.Windows.Forms.Padding(3);
          newWSPage.Size = mainForm.Size;
          newWSPage.TabIndex = 0;
          newWSPage.UseVisualStyleBackColor = true;
          SpreadsheetUserControl ws = new SpreadsheetUserControl();
          ws.Spreadsheet.SpreadsheetModel = spreadsheetModel;
          ws.Spreadsheet.KeyDown += new KeyEventHandler(SpreadsheetView_KeyDown);
          ws.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          ws.Location = new System.Drawing.Point(0, 0);
          ws.Name = "spreadsheetUserControl" +
              (MainForm.WorksheetsTabControl.Controls.Count + 1);
          ws.Size = newWSPage.Size;
          newWSPage.Controls.Add(ws);
          ws.Spreadsheet.RefreshView();
      }
    public void ExecuteInsertFunction(object sender, EventArgs e)
    {
    }
    public void ExecuteExit()
    {
      System.Windows.Forms.Application.Exit();
    }


    public void ExecuteSheetChange()
    {

    }

    public void ExecuteSelectTextColor(Color textColor)
    {
      foreach (DataGridViewCell cell in ActiveWS.Spreadsheet.SelectedCells)
      {
        if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
        {
          Cell c = ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
          if (c == null)
          {
            c = new Cell();
            ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex] = c;
          }
          c.CellFormat.TextColor = textColor;

        }
        ActiveWS.Spreadsheet.RefreshCell(new CellKey(cell.RowIndex, cell.ColumnIndex));
      }
    }

    public void ExecuteSelectCellColor(Color cellColor)
    {
      foreach (DataGridViewCell cell in ActiveWS.Spreadsheet.SelectedCells)
      {
        if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
        {
          Cell c = ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
          if (c == null)
          {
            c = new Cell();
            ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex] = c;
          }
          c.CellFormat.CellColor = cellColor;

        }
        ActiveWS.Spreadsheet.RefreshCell(new CellKey(cell.RowIndex, cell.ColumnIndex));
      }
    }

    public void ExecuteFormatCells(string action)
    {
      FontStyle s = 0;
      float sizeChange = 0;


      foreach (DataGridViewCell cell in ActiveWS.Spreadsheet.SelectedCells)
      {
        Console.WriteLine("here");
        if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
        {
          Cell c = ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
          if (c == null)
          {
            c = new Cell();
            ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex] = c;
          }

          s = c.CellFormat.CellFont.Style;

          switch (action)
          {
            case "bold":
              if ((s & FontStyle.Bold) == FontStyle.Bold)
                s = s & ~FontStyle.Bold;
              else
                s |= FontStyle.Bold;
              break;
            case "italic":
              if ((s & FontStyle.Italic) == FontStyle.Italic)
                s = s & ~FontStyle.Italic;
              else
                s |= FontStyle.Italic;
              break;
            case "underline":
              if ((s & FontStyle.Underline) == FontStyle.Underline)
                s = s & ~FontStyle.Underline;
              else
                s |= FontStyle.Underline;
              break;
            case "increaseFont":
              sizeChange++;
              break;
            case "decreaseFont":
              sizeChange--;
              break;
          }


          c.CellFormat.CellFont = new Font(c.CellFormat.CellFont.Name,
                                           c.CellFormat.CellFont.Size + sizeChange,
                                           s);



        }
        ActiveWS.Spreadsheet.RefreshCell(new CellKey(cell.RowIndex, cell.ColumnIndex));
      }

    }

    private void ExecuteChangeFont(FontEventArgs e)
    {
      Console.WriteLine("here");
      foreach (DataGridViewCell cell in ActiveWS.Spreadsheet.SelectedCells)
      {
        if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
        {
          Cell c = ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
          if (c == null)
          {
            c = new Cell();
            ActiveWS.Spreadsheet.SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex] = c;
          }
          c.CellFormat.CellFont = new Font((e.FamilyName == null) ? c.CellFormat.CellFont.Name : e.FamilyName,
                                           (e.Size == -1) ? c.CellFormat.CellFont.Size : e.Size,
                                           c.CellFormat.CellFont.Style);

        }
        ActiveWS.Spreadsheet.RefreshCell(new CellKey(cell.RowIndex, cell.ColumnIndex));
      }
    }

    private void SpreadsheetView_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Control && e.KeyCode == Keys.C)
      {
        ExecuteCommand(sender, e, CommandType.Copy);
        e.Handled = true;
      }
      else if (e.Control && e.KeyCode == Keys.X)
      {
        ExecuteCommand(sender, e, CommandType.Cut);
        e.Handled = true;
      }
      else if (e.Control && e.KeyCode == Keys.V)
      {
        ExecuteCommand(sender, e, CommandType.Paste);
        e.Handled = true;
      }
    }
    public static Controller Instance
    {
      get
      {
        return ControllerCreator.ControllerInstance;
      }

    }
    private sealed class ControllerCreator
    {
      private static readonly Controller _instance = new Controller();
      public static Controller ControllerInstance
      {
        get
        {
          return _instance;
        }
      }
    }
  }
}
