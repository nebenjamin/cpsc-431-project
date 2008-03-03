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

        public void ClickNew() { }
        public void ClickOpen() { }
        public void ClickClose() { }
        public void ClickSave() { }
        public void ClickSaveAs() { }
        public void ClickCut() { }
        public void ClickCopy() { }
        public void ClickPaste() { }
        public void ClickFontAttributes() { }
        public void ClickChart() { }
        public void ClickInsertWorksheet() { }
        public void ClickInsertFunction() { }

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
        