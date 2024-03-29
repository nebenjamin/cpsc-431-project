using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone
{
    public enum CommandType
    {
        About,
        AssignFormula,
        Close,
        Copy,
        Cut,
        DeleteActiveWS,
        Exit,
        InsertWorksheet,
        InsertBarGraph,
        InsertLineGraph,
        InsertPieGraph,
        InsertScatterGraph,
        InsertColumnGraph,
        InsertFunction,
        New,
        Open,
        Paste,
        Save,
        SaveAs,
        FormatCells,
        SelectTextColor,
        SelectCellColor,
        ChangeFont
    }
}
