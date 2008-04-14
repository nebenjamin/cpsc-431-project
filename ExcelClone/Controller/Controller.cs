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
        private Controller()
        {
            SpreadsheetModel = new SpreadsheetModel(new CellCollection());
            Parser = new Parser();
        }

        private Form mainForm;

        public Form MainForm
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

        private SpreadsheetModel spreadsheetModel;
        public SpreadsheetModel SpreadsheetModel
        {
            get { return spreadsheetModel; }
            set { spreadsheetModel = value; }
        }

        private Parser parser;
        public Parser Parser
        {
            get { return parser; }
            set { parser = value; }
        }

        public void ExecuteCommand(object sender, EventArgs e, CommandType command)
        {
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
                default:
                    break;
            }
        }

        public void ExecuteNew()
        {
        }
        public void ExecuteOpen()
        {
            DataIO.DataIO opener = new ExcelClone.DataIO.DataIO();
            opener.AddSpreadsheet(spreadsheetModel);
            spreadsheetModel = opener.LoadBook();
            SpreadsheetView.Instance.RefreshView();
        }
        public void ExecuteClose()
        {
        }
        public void ExecuteSave()
        {
            DataIO.DataIO saver = new ExcelClone.DataIO.DataIO();
            saver.AddSpreadsheet(spreadsheetModel);
            saver.SaveBook();
        }
        public void ExecuteSaveAs()
        {
        }
        public void ExecuteCut()
        {
            
            ExecuteCopy();
            foreach (DataGridViewCell cell in SpreadsheetView.Instance.SelectedCells)
            {
                cell.Value = "";
                if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                {
                    Cell c = SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
                    if (c != null)
                    {
                        c.Value = "";
                        c.Formula = "";
                    }
                }
            }
          
            SpreadsheetView.Instance.IsPaste = false;
            SpreadsheetView.Instance.IsCopy = false;
            SpreadsheetView.Instance.IsCut = true;
        }

        public void ExecuteCopy()
        {
            CellCollection cellCollection = new CellCollection();
            int smallestRowIndex = GetSmallestRowIndex(SpreadsheetView.Instance.SelectedCells);
            int smallestColumnIndex = GetSmallestColumnIndex(SpreadsheetView.Instance.SelectedCells);
            foreach (DataGridViewCell cell in SpreadsheetView.Instance.SelectedCells)
            {
                cellCollection[new CellKey(cell.RowIndex - smallestRowIndex, cell.ColumnIndex - smallestColumnIndex)] = (SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex] != null) ? (Cell)SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex].Clone() : new Cell();
            }
            SpreadsheetView.Instance.ClipboardCells = cellCollection;
            SpreadsheetView.Instance.IsCopy = true;
        }
        private int GetSmallestRowIndex(DataGridViewSelectedCellCollection cells)
        {
            int minRowIndex = SpreadsheetView.Instance.RowCount-1;
            foreach (DataGridViewCell cell in cells)
            {
                if (cell.RowIndex < minRowIndex)
                    minRowIndex = cell.RowIndex;
            }
            return minRowIndex;
        }
        private int GetSmallestColumnIndex(DataGridViewSelectedCellCollection cells)
        {
            int minColumnIndex = SpreadsheetView.Instance.ColumnCount-1;
            foreach (DataGridViewCell cell in cells)
            {
                if (cell.ColumnIndex < minColumnIndex)
                    minColumnIndex = cell.ColumnIndex;
            }
            return minColumnIndex;
        }
        public void ExecutePaste()
        {
            if (SpreadsheetView.Instance.SelectedCells.Count > 0 &&
                ((SpreadsheetView.Instance.IsCut && !SpreadsheetView.Instance.IsPaste) || 
                SpreadsheetView.Instance.IsCopy))
            {
                int smallestRowIndex = GetSmallestRowIndex(SpreadsheetView.Instance.SelectedCells);
                int smallestColumnIndex = GetSmallestColumnIndex(SpreadsheetView.Instance.SelectedCells);
                for (int r = 0; r < SpreadsheetView.Instance.ClipboardCells.Rows; r++)
                {
                    for (int c = 0; c < SpreadsheetView.Instance.ClipboardCells.Columns; c++)
                    {
                        if (smallestRowIndex + r < SpreadsheetView.Instance.RowCount && smallestColumnIndex + c < SpreadsheetView.Instance.ColumnCount)
                            SpreadsheetModel.Cells[smallestRowIndex + r, smallestColumnIndex + c] =
                            (Cell)(SpreadsheetView.Instance.ClipboardCells[r, c].Clone());
                        
                    }
                }
                SpreadsheetView.Instance.RefreshView();
                //need check for out of bounds
                SpreadsheetView.Instance.IsPaste = true;
                SpreadsheetView.Instance.IsCut = false;

            }
        }
        public void ExecuteInsertBarGraph()
        {
            Rectangle r = new Rectangle(0, 50, 400, 300);

            //Done by David, Caleb, & Scott
            int cellCount = Gui.SpreadsheetView.Instance.SelectedCells.Count;
            int max_col, min_col, max_row, min_row;
            max_col = max_row = 0;
            min_col = min_row = 51;
            for (int i = 0; i < cellCount; i++)
            {
                int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
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
                    int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                    int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                    if(Controller.Instance.SpreadsheetModel.Cells[rI, cI] != null)
                        data[rI - min_row][cI - min_col] = Controller.Instance.SpreadsheetModel.Cells[rI, cI].Value;
                }
                try
                {
                    Graphs.Graph gr = Graphs.bar_graph.Create_Bar_Graph(r, data);
                }catch(ArgumentException)
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
            int cellCount = Gui.SpreadsheetView.Instance.SelectedCells.Count;
            int max_col, min_col, max_row, min_row;
            max_col = max_row = 0;
            min_col = min_row = 51;
            for (int i = 0; i < cellCount; i++)
            {
                int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
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
                    int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                    int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                    if (Controller.Instance.SpreadsheetModel.Cells[rI, cI] != null)
                        data[rI - min_row][cI - min_col] = Controller.Instance.SpreadsheetModel.Cells[rI, cI].Value;
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
            int cellCount = Gui.SpreadsheetView.Instance.SelectedCells.Count;
            int max_col, min_col, max_row, min_row;
            max_col = max_row = 0;
            min_col = min_row = 51;
            for (int i = 0; i < cellCount; i++)
            {
                int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
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
                    int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                    int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                    if (Controller.Instance.SpreadsheetModel.Cells[rI, cI] != null)
                        data[rI - min_row][cI - min_col] = Controller.Instance.SpreadsheetModel.Cells[rI, cI].Value;
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
            int cellCount = Gui.SpreadsheetView.Instance.SelectedCells.Count;
            int max_col, min_col, max_row, min_row;
            max_col = max_row = 0;
            min_col = min_row = 51;
            for (int i = 0; i < cellCount; i++)
            {
                int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
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
                    int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                    int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                    if (Controller.Instance.SpreadsheetModel.Cells[rI, cI] != null)
                        data[rI - min_row][cI - min_col] = Controller.Instance.SpreadsheetModel.Cells[rI, cI].Value;
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
            int cellCount = Gui.SpreadsheetView.Instance.SelectedCells.Count;
            int max_col, min_col, max_row, min_row;
            max_col = max_row = 0;
            min_col = min_row = 51;
            for (int i = 0; i < cellCount; i++)
            {
                int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
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
                    int rI = Gui.SpreadsheetView.Instance.SelectedCells[i].RowIndex;
                    int cI = Gui.SpreadsheetView.Instance.SelectedCells[i].ColumnIndex;
                    if (Controller.Instance.SpreadsheetModel.Cells[rI, cI] != null)
                        data[rI - min_row][cI - min_col] = Controller.Instance.SpreadsheetModel.Cells[rI, cI].Value;
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
        }
        public void ExecuteInsertFunction(object sender, EventArgs e)
        {
        }
        public void ExecuteExit()
        {
            System.Windows.Forms.Application.Exit();
        }

        public void ExecuteFormatCells(string action)
        {
            FontStyle s = 0;
            
            
            
            foreach (DataGridViewCell cell in SpreadsheetView.Instance.SelectedCells)
            {
                if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                {                    
                    Cell c = SpreadsheetModel.Cells[cell.RowIndex, cell.ColumnIndex];
                    if (c != null)
                    {
                        if(s == 0) {
                            s = c.CellFormat.CellFont.Style;
                            switch(action) {
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
                            }
                        }

                        c.CellFormat.CellFont = new Font(c.CellFormat.CellFont.Name,
                                                         c.CellFormat.CellFont.Size,
                                                         s);
                        
                    }
                    
                }
                SpreadsheetView.Instance.RefreshCell(new CellKey(cell.RowIndex, cell.ColumnIndex));
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
