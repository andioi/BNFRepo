using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.Office.Interop.Excel;
using DMS.Tools;
using DMS.Framework;

namespace DMS.Interface
{
    public class oExcel
    {
        public Application excelApp = null;
        public Workbook excelWorkbook = null;
        public Sheets excelSheets = null;
        public Worksheet excelWorksheet = null;
        public Worksheet excelWorksheet2 = null;
        private static object x_missing = Type.Missing;

        private static object x_visible = false;
        private static object x_false = false;
        private static object x_true = true;

        private bool x_app_visible = false;
        private bool x_app_display_alerts = false;
        private System.Globalization.CultureInfo oci;
        private object x_filename;
        #region OPEN WORKBOOK VARIABLES
        private object x_update_links = 0;
        //private object x_read_only = x_true;
        private object x_format = 1;
        private object x_password = x_missing;
        private object x_write_res_password = x_missing;
        private object x_ignore_read_only_recommend = x_true;
        private object x_origin = x_missing;
        private object x_delimiter = x_missing;
        private object x_editable = x_false;
        private object x_notify = x_false;
        private object x_converter = x_missing;
        private object x_add_to_mru = x_false;
        private object x_local = x_false;
        private object x_corrupt_load = x_false;
        #endregion

        #region CLOSE WORKBOOK VARIABLES
        private object x_save_changes = x_false;
        private object x_route_workbook = x_false;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Excel Object Constructor.
        /// </summary>
        public oExcel()
        {
            this.startExcel();
        }

        /// <summary>
        /// Excel Object Constructor
        /// visible is a parameter, either TRUE or FALSE, of type object.
        /// Visible parameter, true for visible, false for non-visible
        public oExcel(bool visible, bool display_alert)
        {
            this.x_app_visible = visible;
            this.x_app_display_alerts = display_alert;
            this.startExcel();
        }
        #endregion

        #region START EXCEL
        private void startExcel()
        {
            oci = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            if (this.excelApp == null)
            {
                this.excelApp = new ApplicationClass();
            }

            // Make Excel Visible
            this.excelApp.Visible = this.x_app_visible;
            this.excelApp.DisplayAlerts = this.x_app_display_alerts;
        }
        #endregion

        #region STOP EXCEL
        public void stopExcel()
        {
            try
            {
                excelApp.Workbooks.Close();
            }
            catch { };
            if (excelApp != null)
                excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            if (this.excelApp != null)
            {
                System.Diagnostics.Process[] pProcess;
                pProcess = System.Diagnostics.Process.GetProcessesByName("Excel");
                pProcess[0].Kill();
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = oci;
        }
        #endregion

        #region OPEN FILE FOR EXCEL
        public void OpenFile(string fileName, string password)
        {
            OpenFile(fileName, password, true);
        }
        public void OpenFile(string fileName, string password, bool x_read_only)
        {
            x_filename = fileName;

            if (password.Length > 0)
            {
                x_password = password;
            }

            // Open a workbook in Excel
            this.excelWorkbook = this.excelApp.Workbooks.Open(
                    fileName, x_update_links, x_read_only,
                    x_format, x_password,
                    x_write_res_password,
                    x_ignore_read_only_recommend, x_origin,
                    x_delimiter, x_editable, x_notify,
                    x_converter, x_add_to_mru, x_missing, x_missing);
            excelSheets = excelWorkbook.Worksheets;
        }
        #endregion

        #region CLOSE FILE FOR EXCEL
        public void CloseFile()
        {
            excelWorkbook.Close(x_save_changes,
                x_filename, x_route_workbook);
        }
        #endregion

        #region SAVEAS FILE FOR EXCEL
        public void CloseFile(string SaveAs)
        {

            excelWorkbook.Close(true,
                SaveAs, x_route_workbook);
        }
        #endregion

    }

    public static class iExcel
    {
        private struct oField
        {
            public int Row, Column;
            public string FieldName;
        }

        private static int dbtimeout = 600;
        private static string FixupPath(string FilePath)
        {
            if (FilePath == null)
                return FilePath;
            FilePath = FilePath.Replace("/", "\\");

            if (FilePath.StartsWith("~\\") || FilePath.StartsWith("..\\"))
            {
                FilePath = FilePath.Replace("~\\", "");
                FilePath = FilePath.Replace("..\\", "");
                FilePath = HttpContext.Current.Request.PhysicalApplicationPath + FilePath;
            }
            return FilePath;
        }

        private static string getcellnm(int row, int col)
        {
            return getcellnm(row, col, false, false); 
        }

        private static string getcellnm(int row, int col, bool absrow, bool abscol)
        {
            string abscol_txt = (abscol) ? "$" : "";
            string absrow_txt = (absrow) ? "$" : "";
 
            int colnm1 = ((int) 'A' + (col-1) / 26) - 1 ;
            string colnm = (char)((int) 'A' + (col-1) % 26)+"";
            if(colnm1 >= (int)'A')
                colnm = (char)colnm1 + colnm;
            return abscol_txt + colnm + absrow_txt + (row).ToString();
        }

        public static string generate_excel_report(ref oExcel objExcel,System.Data.DataTable dtdata, string ExcelFileTmpl, string ExcelFileSaveAs, object sheetnm, string pwd)
        {
            DataView dv = new DataView(dtdata);
            return generate_excel_report(ref objExcel, dv, ExcelFileTmpl, ExcelFileSaveAs, sheetnm, pwd);
        
        }

        private static void generate_excel_report(oExcel objExcel, DataSet ds, DataView dv, System.Collections.Generic.List<oField>[] objFields, System.Collections.Generic.List<Range> objPrints, int tbl )
        {
            for (int row = dv.Count - 1; row >= 0 ; row--)
            {
                if (row != dv.Count - 1)
                {
                    Range objCopy = (Range)objExcel.excelWorksheet2.get_Range(ds.Tables[tbl * 2].Rows[0][0].ToString(), ds.Tables[tbl * 2].Rows[0][1].ToString());
                    objCopy.Copy(Type.Missing);
                    Range objPaste = (Range)objExcel.excelWorksheet.get_Range(ds.Tables[tbl * 2].Rows[0][0].ToString(), Type.Missing);
                    objPaste.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
                }

                if (tbl == 0)
                {
                    Range objPrint = (Range)objExcel.excelWorksheet.get_Range(ds.Tables[tbl * 2].Rows[0][1].ToString(), Type.Missing);
                    objPrints.Add(objPrint);
                }

                foreach (oField objField in objFields[tbl])
                {
                    Range objCell = (Range)objExcel.excelWorksheet.Cells[objField.Row, objField.Column];
                    if (objCell.Value2!=null && objCell.Value2.ToString().IndexOf("@1") > 0)
                    {
                        string[] FieldNames = objField.FieldName.Split(',');
                        for (int i = 1; i <= FieldNames.Length; i++)
                        {
                            string FieldName = FieldNames[i - 1].Trim();
                            objCell.Value2 = objCell.Value2.ToString().Replace("@" + i.ToString(), dv[row][FieldName].ToString().Trim());
                        }
                    }
                    else
                    {
                        if (dv[row][objField.FieldName] is System.String)
                            objCell.Value2 = dv[row][objField.FieldName].ToString().Trim();
                        else
                            objCell.Value2 = dv[row][objField.FieldName];
                    }
                }
                if (tbl + 1 < ds.Tables.Count / 2)
                {
                    System.Data.DataTable dtrel = ds.Tables[(tbl + 1) * 2];
                    string filter = "";
                    string sort = "";
                    for (int i = 2; i < dtrel.Columns.Count; i++)
                    {
                        if (dtrel.Rows[0][i].ToString().ToUpper().StartsWith("ORDER BY"))
                            sort = dtrel.Rows[0][i].ToString();
                        else
                        {
                            if (filter != "") filter += " AND ";
                            filter += dtrel.Rows[0][i].ToString() + " = " + staticFramework.toSql(dv[row][dtrel.Rows[0][i].ToString()]);
                        }
                    }

                    DataView dvchild = new DataView(ds.Tables[(tbl + 1) * 2 + 1], filter, sort, DataViewRowState.OriginalRows);
                    generate_excel_report(objExcel, ds, dvchild, objFields, objPrints, tbl + 1);
                }
            }
        }

        public static string generate_excel_report(ref oExcel objExcel, System.Data.DataSet ds, string ExcelFileTmpl, string ExcelFileSaveAs, object sheetnm, string pwd)
        {
            ExcelFileTmpl = FixupPath(ExcelFileTmpl);
            ExcelFileSaveAs = FixupPath(ExcelFileSaveAs);

            if (objExcel == null)
            {
                objExcel = new oExcel();
                if (ExcelFileTmpl == ExcelFileSaveAs)
                    objExcel.OpenFile(ExcelFileTmpl, "", false);
                else
                    objExcel.OpenFile(ExcelFileTmpl, "");
            }
            try
            {

                objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[sheetnm];
                int TblCount = ds.Tables.Count / 2;
                System.Collections.Generic.List<oField>[] objFields = new System.Collections.Generic.List<oField>[TblCount];
                for (int i = 0; i < TblCount; i++)
                {
                    objFields[i] = new System.Collections.Generic.List<oField>();
                }

                int CmtCount = objExcel.excelWorksheet.Comments.Count;
                for (int i = 0; i < CmtCount; i++)
                {
                    Range objComment = (Range)objExcel.excelWorksheet.Comments[i + 1].Parent;
                    oField objField = new oField();
                    objField.Row = objComment.Row;
                    objField.Column = objComment.Column;
                    objField.FieldName = objExcel.excelWorksheet.Comments[i + 1].Shape.TextFrame.Characters(Type.Missing, Type.Missing).Text.Trim().Replace("\n", "");
                    for (int j = TblCount - 1; j >= 0 ; j--)
                    {
                        Range startRange = (Range)objExcel.excelWorksheet.get_Range(ds.Tables[j * 2].Rows[0][0].ToString(), Type.Missing);
                        Range endRange = (Range)objExcel.excelWorksheet.get_Range(ds.Tables[j * 2].Rows[0][1].ToString(), Type.Missing);
                        if (objField.Row >= startRange.Row && objField.Row <= endRange.Row &&
                            objField.Column >= startRange.Column && objField.Column <= endRange.Column)
                        {
                            objFields[j].Add(objField);
                            break;
                        }
                    }
                }

                for (int i = 0; i < CmtCount; i++)
                    objExcel.excelWorksheet.Comments[1].Delete();

                
                DataView dv = new DataView(ds.Tables[1], "", "", DataViewRowState.OriginalRows);
                System.Collections.Generic.List<Range> objPrints = new System.Collections.Generic.List<Range>();

                objExcel.excelWorksheet.Copy(objExcel.excelWorksheet, Type.Missing);
                objExcel.excelWorksheet2 = (Worksheet)objExcel.excelSheets[objExcel.excelWorksheet.Name + " (2)"];
                generate_excel_report(objExcel, ds, dv, objFields, objPrints, 0);
                objExcel.excelWorksheet2.Delete();
                foreach (Range objPrint in objPrints)
                {
                    Range objPrintBreak = (Range)objExcel.excelWorksheet.Cells[objPrint.Row + 1, objPrint.Column + 1];
                    objPrintBreak.PageBreak = (int)XlPageBreak.xlPageBreakManual;
                }
                //((Range)objExcel.excelWorksheet.Cells[((rowlast - rowstart) + 1) * row + rowstart, collast + 1]).PageBreak = (int)XlPageBreak.xlPageBreakManual;
                

                
            }
            catch (Exception e)
            {
                objExcel.CloseFile();
                objExcel.stopExcel();
                return e.Message;
            }

            if (ExcelFileSaveAs != null)
            {
                if (pwd != null && pwd != "")
                {
                    for (int j = 1; j <= objExcel.excelSheets.Count; j++)
                    {
                        objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[j];
                        objExcel.excelWorksheet.Protect(pwd,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing);
                    }
                    objExcel.excelWorkbook.Protect(pwd, true, false);
                }
                objExcel.CloseFile(ExcelFileSaveAs);
                objExcel.stopExcel();
            }
            return "";
        }

        public static string generate_excel_report(ref DMS.Interface.oExcel objExcel, System.Data.DataView dv, string ExcelFileTmpl, string ExcelFileSaveAs, object sheetnm, string pwd)
        {
            ExcelFileTmpl = FixupPath(ExcelFileTmpl);
            ExcelFileSaveAs = FixupPath(ExcelFileSaveAs);

            if (objExcel == null)
            {
                objExcel = new oExcel();
                if (ExcelFileTmpl == ExcelFileSaveAs)
                    objExcel.OpenFile(ExcelFileTmpl, "", false);
                else
                    objExcel.OpenFile(ExcelFileTmpl, "");
            }
            try
            {

                objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[sheetnm];
                if (dv.Count == 0)
                    objExcel.excelWorksheet.Delete();
                else
                {
                    System.Collections.Generic.List<oField> objFields = new System.Collections.Generic.List<oField>();
                    int CmtCount = objExcel.excelWorksheet.Comments.Count;

                    for (int i = 0; i < CmtCount; i++)
                    {
                        Range objCell = (Range)objExcel.excelWorksheet.Comments[i + 1].Parent;
                        oField objField = new oField();
                        objField.Row = objCell.Row;
                        objField.Column = objCell.Column;
                        objField.FieldName = objExcel.excelWorksheet.Comments[i + 1].Shape.TextFrame.Characters(Type.Missing, Type.Missing).Text.Trim().Replace("\n","");
                        objFields.Add(objField);
                    }

                    for (int i = 0; i < CmtCount; i++)
                        objExcel.excelWorksheet.Comments[1].Delete();

                    int rowstart = 1;
                    int colstart = 1;
                    int rowlast = objExcel.excelWorksheet.UsedRange.Rows.Count;
                    int collast = objExcel.excelWorksheet.UsedRange.Columns.Count;



                    for (int row = 0; row < dv.Count - 1; row++)
                    {
                        Range objCopy = (Range)objExcel.excelWorksheet.get_Range(getcellnm(rowstart, colstart), getcellnm(rowlast, collast));
                        objCopy.Copy(Type.Missing);
                        Range objPaste = (Range)objExcel.excelWorksheet.get_Range(getcellnm(rowstart, colstart), Type.Missing);
                        objPaste.Insert(XlInsertShiftDirection.xlShiftDown, Type.Missing);
                    }
                    for (int row = 1; row <= dv.Count; row++)
                        ((Range)objExcel.excelWorksheet.Cells[((rowlast - rowstart) + 1) * row + rowstart, collast + 1]).PageBreak = (int)XlPageBreak.xlPageBreakManual;

                    for (int row = 0; row < dv.Count; row++)
                    {
                        foreach (oField objField in objFields)
                        {
                            Range objCell = (Range)objExcel.excelWorksheet.Cells[row * rowlast + objField.Row, objField.Column];
                            if (objCell.Value2!=null && objCell.Value2.ToString().IndexOf("@1") > 0)
                            {
                                string[] FieldNames = objField.FieldName.Split(',');
                                for (int i = 1; i <= FieldNames.Length; i++)
                                {
                                    string FieldName = FieldNames[i - 1].Trim();
                                    objCell.Value2 = objCell.Value2.ToString().Replace("@" + i.ToString(), dv[row][FieldName].ToString().Trim());
                                }
                            }
                            else
                            {
                                if (dv[row][objField.FieldName] is System.String)
                                    objCell.Value2 = dv[row][objField.FieldName].ToString().Trim();
                                else
                                    objCell.Value2 = dv[row][objField.FieldName];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objExcel.CloseFile();
                objExcel.stopExcel();
                return e.Message;
            }
        
            if (ExcelFileSaveAs != null)
            {
                if (pwd != null && pwd != "")
                {
                    for (int j = 1; j <= objExcel.excelSheets.Count; j++)
                    {
                        objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[j];
                        objExcel.excelWorksheet.Protect(pwd,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing);
                    }
                    objExcel.excelWorkbook.Protect(pwd, true, false);
                }
                objExcel.CloseFile(ExcelFileSaveAs);
                objExcel.stopExcel();
            }
            return "";
        }

        public static string upload_excel(string ExcelFile, string savequery, DbConnection conn, int rowstart, int colstart, int colend, string pwd)
        {
            ExcelFile = FixupPath(ExcelFile);

            oExcel objExcel = new oExcel();
            objExcel.OpenFile(ExcelFile, "");
            

            try
            {
                if(pwd!=null || pwd!="")
                    objExcel.excelWorkbook.Unprotect(pwd);
                
                int Field = colend - colstart + 1;
                objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[1];
                object[] objparam = new object[Field];
                for(int row = rowstart;((Range)objExcel.excelWorksheet.Cells[row, colstart]).Value2 != null; row++)
                {
                    for (int Column = 0; Column < Field; Column++)
                    {
                        Range objCell = (Range)objExcel.excelWorksheet.Cells[row, Column + colstart];
                        objparam[Column] = objCell.Value2;
                        try
                        {
                            switch (objCell.Validation.Type)
                            {
                                case 3:
                                    objparam[Column] = objparam[Column].ToString().Substring(0, objparam[Column].ToString().IndexOf(" - ")).Trim();
                                    break;
                                case 4:
                                    objparam[Column] = DateTime.FromOADate((double)objparam[Column]);
                                    break;
                            }
                        }
                        catch
                        {
                        };
                        
                    }
                    conn.ExecuteNonQuery(savequery, objparam, dbtimeout);
                } 
            }
            catch(Exception e)
            {
                objExcel.CloseFile();
                objExcel.stopExcel();
                return e.Message;
            }
            objExcel.CloseFile();
            objExcel.stopExcel();
            
            return "";
        }

        public static string generate_excel(ref oExcel objExcel, System.Data.DataSet dtset, string ExcelFileTmpl, string ExcelFileSaveAs, string pwd)
        {
            System.Data.DataTable dtdata = dtset.Tables[0];

            int rowstart = 5, colreffstart = 210;
            int colstart = 1, colend = dtdata.Columns.Count;

            ExcelFileTmpl = FixupPath(ExcelFileTmpl);
            ExcelFileSaveAs = FixupPath(ExcelFileSaveAs);

            if (objExcel == null)
            {
                objExcel = new oExcel();
                if (ExcelFileTmpl == ExcelFileSaveAs)
                    objExcel.OpenFile(ExcelFileTmpl, "", false);
                else
                    objExcel.OpenFile(ExcelFileTmpl, "");
            }
            try
            {

                if (dtdata.Rows.Count == 0)
                    throw new Exception("no data");

                objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[1];
                if (dtset.Tables.Count > 1)
                {
                    System.Data.DataTable dtreffcol = dtset.Tables[1];
                    for (int i = 0; i < dtset.Tables.Count-2; i++)
                    {
                        System.Data.DataTable dtreff = dtset.Tables[i+2];
                        for (int j = 0; j < dtreff.Rows.Count; j++)
                        {
                            Range objCell = (Range)objExcel.excelWorksheet.Cells[j + 2, colreffstart + i];
                            objCell.Value2 = dtreff.Rows[j][0].ToString() + " - " + dtreff.Rows[j][1].ToString();
                        }
                        Range objList = (Range)objExcel.excelWorksheet.get_Range(dtreffcol.Rows[0][i].ToString(), Type.Missing);
                        if (objList.Validation != null)
                            objList.Validation.Delete();
                        objList.Validation.Add(
                            XlDVType.xlValidateList,
                            XlDVAlertStyle.xlValidAlertStop,
                            XlFormatConditionOperator.xlBetween,
                            "=" + getcellnm(1, colreffstart + i, true, true) + ":" + getcellnm(dtreff.Rows.Count + 1, colreffstart + i, true, true),
                            Type.Missing);
                    }
                }

                Range objCopy = (Range)objExcel.excelWorksheet.get_Range(getcellnm(rowstart, colstart), getcellnm(rowstart, colend));
                Range objPaste = (Range)objExcel.excelWorksheet.get_Range(getcellnm(rowstart, colstart), getcellnm(rowstart + dtdata.Rows.Count - 1, colend));
                objCopy.Copy(objPaste);

                for (int row = 0; row < dtdata.Rows.Count; row++)
                {

                    for (int Column = 0; Column < dtdata.Columns.Count; Column++)
                    {
                        if (dtdata.Rows[row][Column] is System.DBNull) 
                            continue;
                        Range objCell = (Range)objExcel.excelWorksheet.Cells[row + rowstart, Column + colstart];
                        objCell.Value2 = dtdata.Rows[row][Column];
                    }
                }
            }
            catch (Exception e)
            {
                objExcel.CloseFile();
                objExcel.stopExcel();
                return e.Message;
            }

            if (ExcelFileSaveAs != null)
            {
                if (pwd != null && pwd != "")
                {
                    for (int j = 1; j <= objExcel.excelSheets.Count; j++)
                    {
                        objExcel.excelWorksheet = (Worksheet)objExcel.excelSheets[j];
                        objExcel.excelWorksheet.Protect(pwd,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing);
                    }
                    objExcel.excelWorkbook.Protect(pwd, true, false);
                }
                objExcel.CloseFile(ExcelFileSaveAs);
                objExcel.stopExcel();
            }
            return "";
        }


    }
}
