using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;

namespace ExcelClone
{
    class Controller
    {
        private Controller()
        {
            SpreadsheetModel = new SpreadsheetModel(new CellCollection());
        }

        private SpreadsheetModel spreadsheetModel;
        public SpreadsheetModel SpreadsheetModel
        {
            get { return spreadsheetModel; }
            set { spreadsheetModel = value; }
        }

        protected void ClickNew() { }
        protected void ClickOpen() { }
        protected void ClickClose() { }
        protected void ClickSave() { }
        protected void ClickSaveAs() { }
        protected void ClickCut() { }
        protected void ClickCopy() { }
        protected void ClickPaste() { }
        protected void ClickFontAttributes() { }
        protected void ClickChart() { }
        protected void ClickInsertWorksheet() { }
        protected void ClickInsertFunction() { }

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
        