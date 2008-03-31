using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using ExcelClone.Functions;

namespace ExcelClone
{
    class Controller
    {
        private Controller()
        {
            SpreadsheetModel = new SpreadsheetModel(new CellCollection());
            Parser = new Parser();
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

        public void ExecuteNew(object sender, EventArgs e)
        {
        }
        public void ExecuteOpen(object sender, EventArgs e)
        {
        }
        public void ExecuteClose(object sender, EventArgs e)
        {
        }
        public void ExecuteSave(object sender, EventArgs e)
        {
        }
        public void ExecuteSaveAs(object sender, EventArgs e)
        {
        }
        public void ExecuteCut(object sender, EventArgs e)
        {
        }
        public void ExecuteCopy(object sender, EventArgs e)
        {
        }
        public void ExecutePaste(object sender, EventArgs e)
        {
        }
        public void ExecuteChart(object sender, EventArgs e)
        {
        }
        public void ExecuteInsertWorksheet(object sender, EventArgs e)
        {
        }
        public void ExecuteInsertFunction(object sender, EventArgs e)
        {
        }
        public void ExecuteExit(object sender, EventArgs e)
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
