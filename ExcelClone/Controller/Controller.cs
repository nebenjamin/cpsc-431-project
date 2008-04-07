using System;
using System.Collections.Generic;
using System.Text;
using ExcelClone.Core;
using ExcelClone.Functions;
using ExcelClone.Gui;

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
    }
    public void ExecuteCopy()
    {
    }
    public void ExecutePaste()
    {
    }
    public void ExecuteChart()
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
