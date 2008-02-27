using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;


using ExcelClone.Core;

namespace ExcelClone.DataIO
{
    public class DataIO
    {
        private List<SpreadsheetModel> book;
        private String filename;
        private XmlSerializer xmlSerializer;
        private XmlTextWriter xmlTextWriter;

        public DataIO(String Filename)
        {
            book = new List<SpreadsheetModel>();
            filename = Filename;
        }

        public bool AddSpreadsheet(SpreadsheetModel sheet)
        {
            book.Add(sheet);
            return true;
        }

        public bool SetBook(List<SpreadsheetModel> volume)
        {
            book = volume;
            return true;
        }

        public bool SaveBook()
        {
            if (book.Count == 0)
            {
                MessageBox.Show("Passed an empty book!  Feed me data", "I'm hungry");
                return false;
            }
            return WriteBook();
        }

        public bool LoadBook()
        {
            return ReadBook();
        }

        private bool WriteBook()
        {
            Cell temp = new Cell();
            //temp = book[0].Cells[0, 0];
            
            //int tempInt = 10;
            //MessageBox.Show("X: " + tempInt.ToString());

            //String filename = new String
            filename = "c:\\ExcelCloneFile.xml";
            
            XmlTextWriter writer = null;
            writer = new XmlTextWriter(filename, null);

            string fontTags = "";

            try
            {
                 writer.WriteStartDocument();
                
                 writer.WriteString("\n<template.extension>\n");
                 writer.WriteString("<author = 'Nathan Benjamin' />\n");
                 writer.WriteString("other metadata\n");
                 writer.WriteString("<sheet 1>\n");

                
                 int rows = book[0].Cells.Rows;
                 int cols = book[0].Cells.Columns;
                 writer.WriteString("<columns number = " + cols);
                 writer.WriteString(" />\n<rows number = " + rows + " />\n");

                 //Scan each column value, then go down a row and scan each column again.
                 for(int y = 1; y < cols; y++){
                     writer.WriteString("<column " + y + ">\n");
                     for(int x = 1; x < rows; x++){
                        temp = book[0].Cells[x,y];
                        
                        fontTags = "";
                        bool theTruth = temp.Formula.Equals(null);
                        String s = "";
                        int tempT = -1;
                        //Color tempC = new Color();
                        //tempC = temp.CellFormat.BackgroundBrush.BackColor;
                         
                        //s = temp.CellFormat.BackgroundBrush.GetType();
                       /* if (temp.CellFormat == null)
                        {//Leave the cell blank
                        }
                        else
                        {

                            if (temp.CellFormat.Font.Equals(null) == true)
                            {//ignore
                            }
                            else
                            {
                                 writer.WriteString("<row " + x + ">\n");
                                 if (tempFormat.Font.fontFamily() != "Times New Roman")
                                 {
                                     writer.WriteString("<text-font = " + tempFormat.Font.fontFamily.get() + ">");
                                     fontTags = fontTags + "</text-font>";
                                 }
                                 if (tempFormat.Font.emSize.get() != 12)
                                 {
                                     writer.WriteString("text-size = " << tempFormat.Font.emSize.get() << ">");
                                     fontTags = fontTags + "</text-size>";
                                 }
                                 if (tempFormat.Font.bold == true){
                                     writer.WriteString("<b>");
                                     fontTags = fontTags + "</b>";
                                 }
                                 if (tempFormat.Font.italics == true){
                                     writer.WriteString("<i>");
                                     fontTags = fontTags + "</i>";
                                 }
                                 if (tempFormat.Font.underlined == true){
                                     writer.WriteString("<u>");
                                     fontTags = fontTags + "</u>";
                                 }
                            }//end of Font check

                            
                            if (temp.CellFormat.FontBrush.Equals(null) == true)
                            {//ignore
                            }
                            else
                            {
                                if (tempFormat.FontBrush.color.get() != "#000000"){
                                   writer.WriteString("<text-color = " << tempFormat.FontBrush.Color.get() << ">");
                                   fontTags = fontTags + "</text-color>";
                                }
                                if (tempFormat.FontBrush.brushStyle.get() != null){
                                    writer.WriteString("<brush-style = " << tempFormat.FontBrush.BrushStyle.get() << ">");
                                    fontTags = fontTags + "</brush-style>";
                                }
                                if (tempFormat.FontBrush.backColor.get() != "#FFFFFF"){
                                    writer.WriteString("cell-bg-color = " << tempFormat.FontBrush.backColor.get() << ">");
                                    fontTags = fontTags + "</cell-bg-color>";
                                }
                            }
                        }//end of Format check
                         */
                /*if (temp.Formula.Equals(null) == true)
                {
                    if (temp.Formula.Value(null) == true)
                    {
                       }
                       else{//write the value
                           if(tempFormat == null){//we haven't printed out row yet
                           writer.WriteString("<row " << x << ">\n");
                           }
                           writer.WriteString("<content = number>\n" << tempCell.Value.get());
                           writer.WriteString("\n</content>\n");
                           writer.WriteString("</row>\n");
                       }
                   }
                   else{//write the formula
                       if(tempFormat == null){//we haven't printed out row yet
                             writer.WriteString("<row " << x << ">\n");
                         }
                             writer.WriteString("<content = formula>\n" << tempCell.Formula.get());
                             writer.WriteString("\n</content>\n");
                             writer.WriteString("</row>\n");
                        
                     }*/
                        

                        
                     }//end of row loop
                     writer.WriteString("</column>\n");
                 }//end of col loop*/

                 writer.WriteString("</sheet>\n");
                writer.WriteString("</template>\n");
                writer.Flush();
          
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: {0}", e.ToString());
            }
            finally
            {
                //if (writer != null)
                //{
                //    writer.Close();
                //}
            }
            return true;
        }

        private bool ReadBook()
        {
            return true;
        }
    }
}
