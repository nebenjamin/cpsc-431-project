using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using ExcelClone.Functions;
using ExcelClone.Gui;
using System.Windows.Forms;
using System.Collections;

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
                case CommandType.InsertGraph:
                    ExecuteInsertGraph();
                    break;
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
                SpreadsheetView.Instance.IsCut)
            {
                int smallestRowIndex = GetSmallestRowIndex(SpreadsheetView.Instance.SelectedCells);
                int smallestColumnIndex = GetSmallestColumnIndex(SpreadsheetView.Instance.SelectedCells);
                for (int r = 0; r < SpreadsheetView.Instance.ClipboardCells.Rows; r++)
                {
                    for (int c = 0; c < SpreadsheetView.Instance.ClipboardCells.Columns; c++)
                    {
                        if (smallestRowIndex + r < SpreadsheetView.Instance.ColumnCount && smallestColumnIndex + c < SpreadsheetView.Instance.RowCount)
                        SpreadsheetModel.Cells[smallestRowIndex + r, smallestColumnIndex + c] =
                            SpreadsheetView.Instance.ClipboardCells[r, c];
                        
                    }
                }
                SpreadsheetView.Instance.RefreshView();
                //need check for out of bounds
                SpreadsheetView.Instance.IsPaste = true;
                SpreadsheetView.Instance.IsCut = false;

            }
        }
        public void ExecuteInsertGraph()
        {
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
