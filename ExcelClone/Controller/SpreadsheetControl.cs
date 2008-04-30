using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ExcelClone.Core;
using ExcelClone.Gui;

namespace ExcelClone
{
	public class SpreadsheetControl : ISpreadsheetControl
	{
		private Queue<CellKey> InvalidCells;

		public static SpreadsheetControl Instance
		{
			get { return SpreadsheetControlCreator.CreatorInstance; }
		}

		private sealed class SpreadsheetControlCreator
		{
			private static readonly SpreadsheetControl _instance = new SpreadsheetControl();

			public static SpreadsheetControl CreatorInstance
			{
				get { return _instance; }
			}
		}

		public SpreadsheetControl()
		{
			InvalidCells = new Queue<CellKey>();
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

		#region ISpreadsheetControl Members

		//enqueue invalid cell
		public void InvalidateCell(CellKey key)
		{
			InvalidCells.Enqueue(key);
		}

		//call on cell changes
		public void CellChanged(CellKey key)
		{
			foreach (System.Windows.Forms.Control c in mainForm.WorksheetsTabControl.SelectedTab.Controls)
			{
				if (c is SpreadsheetUserControl)
				{
					SpreadsheetUserControl ActiveWS = (SpreadsheetUserControl)c;
					CellKey lastUpdated = null;
					string val = null;
					InvalidateCell(key);
					while (InvalidCells.Count > 0)
					{
						key = InvalidCells.Dequeue();

						if (lastUpdated == key)
						{
							//circular Reference
						}
						//val == null or value in new cell
						//val = parse(key, m.Cells[key].Value);
						if (val != null)
						{
							UpdateCellValue(key, val);
							lastUpdated = key;
						}
						val = Controller.Instance.Parser.Parse(ActiveWS, SpreadsheetView.MakeColumnLabel(key.C) + key.R + ":" + ActiveWS.Spreadsheet.SpreadsheetModel.Cells[key].Formula);
						UpdateCellValue(key, val);
						//updateCellValue(key, parse(key, m.Cells[key].value)); call the parse function for this cell key
					}
				}
			}
		}

		public void InvalidateCell(int r, int c)
		{
			this.InvalidateCell(new CellKey(r, c));
		}

		public void UpdateCellValue(CellKey key, string value)
		{
			foreach (System.Windows.Forms.Control c in mainForm.WorksheetsTabControl.SelectedTab.Controls)
			{
				if (c is SpreadsheetUserControl)
				{
					SpreadsheetUserControl ActiveWS = (SpreadsheetUserControl)c;
					ActiveWS.Spreadsheet.SpreadsheetModel.Cells[key].Value = value;
					ActiveWS.Spreadsheet.RefreshCell(key);
				}
			}
		}

		public void UpdateCellFormula(CellKey key, string formula)
		{
			foreach (System.Windows.Forms.Control c in mainForm.WorksheetsTabControl.SelectedTab.Controls)
			{
				if (c is SpreadsheetUserControl)
				{
					SpreadsheetUserControl ActiveWS = (SpreadsheetUserControl)c;
					ActiveWS.Spreadsheet.SpreadsheetModel.Cells[key].Formula = formula;
					ActiveWS.Spreadsheet.RefreshCell(key);
				}
			}
		}

		public void ClearCell(CellKey key)
		{
			foreach (System.Windows.Forms.Control c in mainForm.WorksheetsTabControl.SelectedTab.Controls)
			{
				if (c is SpreadsheetUserControl)
				{
					SpreadsheetUserControl ActiveWS = (SpreadsheetUserControl)c;
					ActiveWS.Spreadsheet.SpreadsheetModel.Cells[key].Formula = null;
					ActiveWS.Spreadsheet.SpreadsheetModel.Cells[key].Value = "";
					//repaint
				}
			}
		}

		public void LoadSheet(ICellCollection c)
		{
			foreach (System.Windows.Forms.Control ctr in mainForm.WorksheetsTabControl.SelectedTab.Controls)
			{
				if (ctr is SpreadsheetUserControl)
				{
					SpreadsheetUserControl ActiveWS = (SpreadsheetUserControl)ctr;
					ActiveWS.Spreadsheet.SpreadsheetModel.Cells = (CellCollection)c;
					//change  view!     
				}
			}
		}

		#endregion
	}
}
