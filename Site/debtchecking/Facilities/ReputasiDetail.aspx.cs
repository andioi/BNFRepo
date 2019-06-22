using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using DMS.Tools;
using DMS.Framework;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Diagnostics;
using EvoPdf.HtmlToPdf;
using Excel = Microsoft.Office.Interop.Excel;

namespace DebtChecking.Facilities
{
    public partial class ReputasiDetail : DataPage
    {
        #region static vars
        DataSet dsRep = null;
        #endregion

        #region initial
        protected void initial_reffrential_parameter()
        {
            //staticFramework.reff(product, "select * FROM rfproduct_calc", null, conn);
        }

        private void retrieve_schema()
        {
            //DataTable dt = conn.GetDataTable("select top 0 * from appfincal", null, dbtimeout);
            //staticFramework.retrieveschema(dt, reffnumber);
           
        }

        public bool isActive
        {
            get { return !(Session["ApprovalGroup"].ToString() != "1"); }
        }
        #endregion

        #region retrieve

        private void retrieve_data(string key)
        {
            DataTable dt = conn.GetDataTable("select appid, cust_name from SLIK_APPLICANT where reffnumber = @1 order by appid", new object[] { Request.QueryString["reffnum"] }, dbtimeout);
            for (int i=0; i < dt.Rows.Count;i++)
            {
                UserControl uc = (UserControl)Page.LoadControl("../CommonForm/UC_ReputasiDetail.ascx");
                System.Reflection.PropertyInfo[] info = uc.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo item in info)
                {
                    if (item.CanWrite)
                    {
                        switch (item.Name)
                        {
                            case "appid":
                                item.SetValue(uc, dt.Rows[i][0].ToString(), null);
                                break;
                            case "cust_name":
                                item.SetValue(uc, dt.Rows[i][1].ToString(), null);
                                break;
                        }
                    }
                    divUC.Controls.Add(uc);
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_data(Request.QueryString["reffnum"]);
                urlframe.Value = HttpContext.Current.Request.Url.AbsoluteUri;
                if (Request.QueryString["bypasssession"] == "1")
                {
                    btnexcel.Visible = false;
                    btnpdf.Visible = false;
                }
            }
        }

        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter == "export_excel")
            {
                try
                {
                    export_excel(Request.QueryString["reffnum"]);
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
        }

        #region export_excel
        private void export_excel(string reffnumber)
        {
            string msg = "";
            try
            {
                #region start excel
                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBookReputasi;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheetLaporan;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheetReputasi;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheetTenor;

                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBookReputasi = xlApp.Workbooks.Add(misValue);
                #endregion

                #region content excel

                #region looping sheet
                //looping sheet
                DataTable dtsheet;

                dtsheet = conn.GetDataTable("exec SP_VW_SHEET_LIST @1", new object[] { reffnumber }, dbtimeout);

                for (int j = 0; j < dtsheet.Rows.Count; j++)
                {
                    string appid = dtsheet.Rows[j][0].ToString();

                    #region sheet tenor
                    //sheet tenor
                    string sheetTenorName = dtsheet.Rows[j][2].ToString();
                    xlWorkSheetTenor = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBookReputasi.Worksheets.Add(misValue, misValue, misValue, misValue);
                    xlWorkSheetTenor.Name = sheetTenorName;
                    //xlWorkSheetTenor.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                    //xlWorkSheetTenor.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    //xlWorkSheetTenor.PageSetup.LeftMargin = 0.1;
                    //xlWorkSheetTenor.PageSetup.RightMargin = 0.1;
                    //xlWorkSheetTenor.PageSetup.TopMargin = 0.1;
                    //xlWorkSheetTenor.PageSetup.BottomMargin = 0.1;
                    //xlWorkSheetTenor.PageSetup.HeaderMargin = 0.1;
                    //xlWorkSheetTenor.PageSetup.FooterMargin = 0.1;

                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 1]).EntireColumn.ColumnWidth = 3;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 2]).EntireColumn.ColumnWidth = 20;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 3]).EntireColumn.ColumnWidth = 20;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 4]).EntireColumn.ColumnWidth = 20;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 5]).EntireColumn.ColumnWidth = 12;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 6]).EntireColumn.ColumnWidth = 14;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 7]).EntireColumn.ColumnWidth = 12;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 8]).EntireColumn.ColumnWidth = 10;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 9]).EntireColumn.ColumnWidth = 14;
                    ((Excel.Range)xlWorkSheetTenor.Cells[1, 10]).EntireColumn.ColumnWidth = 10;

                    //get sheet content
                    object[] paramTenor = { reffnumber, appid };
                    DataTable dtTenor = conn.GetDataTable("EXEC sp_vw_excel_tenor @1,@2", paramTenor, dbtimeout);

                    //row dttenor
                    for (int i = 0; i <= dtTenor.Rows.Count - 1; i++)
                    {
                        int startRow = int.Parse(dtTenor.Rows[i][0].ToString());
                        int startColumn = int.Parse(dtTenor.Rows[i][1].ToString());
                        int EndRow = int.Parse(dtTenor.Rows[i][2].ToString());
                        int EndColumn = int.Parse(dtTenor.Rows[i][3].ToString());

                        if (EndRow == 0 || EndColumn == 0)
                        {
                            EndRow = startRow;
                            EndColumn = startColumn;
                        }

                        bool isMerge = bool.Parse(dtTenor.Rows[i][4].ToString());
                        string bgColor = dtTenor.Rows[i][5].ToString();
                        bool isbold = bool.Parse(dtTenor.Rows[i][6].ToString());

                        string valueType = dtTenor.Rows[i][7].ToString();
                        string valueColumn = dtTenor.Rows[i][8].ToString();
                        float valueColumn_numeric = float.Parse(dtTenor.Rows[i][9].ToString());
                        string datatype = dtTenor.Rows[i][10].ToString();

                        //Excel.Range range = (Excel.Range)xlWorkSheetReputasi.Cells[i + 2, j + 1];                        
                        Excel.Range range = (Excel.Range)xlWorkSheetTenor.Range[xlWorkSheetTenor.Cells[startRow, startColumn], xlWorkSheetTenor.Cells[EndRow, EndColumn]];

                        //range.ColumnWidth = 7;

                        if (startColumn == 2)
                        {
                            range.Cells.WrapText = true;
                        }
                        else
                        {
                            range.Cells.WrapText = false;
                        }

                        if (valueType.Trim() == "int" || valueType.Trim() == "formula_int" || valueType.Trim() == "formula_int_border")
                        {
                            range.NumberFormat = "0";
                        }
                        else if (valueType.Trim() == "formula_decimal" || valueType.Trim() == "formula_decimal_border")
                        {
                            //range.NumberFormat = "#,##0.00";
                            range.NumberFormat = "0.00%";
                        }
                        else if (valueType.Trim() == "date" || valueType.Trim() == "date_border")
                        {
                            range.NumberFormat = "mm/dd/yyyy";
                        }
                        else if (datatype == "string" && valueType.Trim() != "formula")
                        {
                            range.NumberFormat = "@";
                        }
                        else if (datatype == "numeric" || valueType.Trim() == "formula")
                        {
                            range.NumberFormat = "Rp #,##0.00";
                        }

                        //if (valueType.Trim() == "text_center")
                        //{
                        //    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignJustify;
                        //    range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                        //}

                        if (isMerge)
                        {
                            range.Merge();
                        }

                        //if (valueType.Trim().Contains("border"))
                        //{
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 1d;
                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 1d;
                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 1d;
                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 1d;                            
                        //}

                        //bgColor isbold
                        //bgColor                     
                        if (bgColor == "Old Green")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                        }
                        else if (bgColor == "Pink")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Moccasin);
                        }
                        else if (bgColor == "Yellow")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                        else if (bgColor == "Young Green")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Honeydew);
                        }
                        else if (bgColor == "blue")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);
                        }
                        else
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        }

                        ////isbold
                        //if (isbold)
                        //{
                        //    range.Font.Bold = true;
                        //}

                        if (valueType.Trim() == "formula" || valueType.Trim() == "formula_int" || valueType.Trim() == "formula_decimal" || valueType.Trim() == "formula_decimal_border" || valueType.Trim() == "formula_int_border")
                        {
                            string formula = valueColumn;
                            range.Formula = formula;

                            range.Calculate();
                        }
                        else
                        {
                            if (valueType.Trim() == "int")
                            {
                                int[] intArray = new int[] { int.Parse(valueColumn_numeric.ToString()) };
                                range.Value = intArray;
                            }
                            else if (datatype == "numeric")
                            {
                                float[] floatArray = new float[] { valueColumn_numeric };
                                range.Value = floatArray;
                            }
                            else
                            {
                                valueColumn = valueColumn.TrimEnd(new char[] { '\r', '\n' });
                                string[] stringArray = new string[] { valueColumn };
                                range.Value = stringArray;
                            }
                        }
                    }
                    //end row dttenor

                    //end sheet tenor
                    #endregion

                    #region sheet reputasi
                    //sheet reputasi
                    string sheetReputasiName = dtsheet.Rows[j][1].ToString();
                    xlWorkSheetReputasi = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBookReputasi.Worksheets.Add(misValue, misValue, misValue, misValue);
                    xlWorkSheetReputasi.Name = sheetReputasiName;
                    //xlWorkSheetReputasi.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                    //xlWorkSheetReputasi.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    //xlWorkSheetReputasi.PageSetup.LeftMargin = 0.1;
                    //xlWorkSheetReputasi.PageSetup.RightMargin = 0.1;
                    //xlWorkSheetReputasi.PageSetup.TopMargin = 0.1;
                    //xlWorkSheetReputasi.PageSetup.BottomMargin = 0.1;
                    //xlWorkSheetReputasi.PageSetup.HeaderMargin = 0.1;
                    //xlWorkSheetReputasi.PageSetup.FooterMargin = 0.1;

                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 1]).EntireColumn.ColumnWidth = 3;
                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 2]).EntireColumn.ColumnWidth = 20;
                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 3]).EntireColumn.ColumnWidth = 20;
                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 4]).EntireColumn.ColumnWidth = 10;
                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 5]).EntireColumn.ColumnWidth = 10;
                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 6]).EntireColumn.ColumnWidth = 12;
                    ((Excel.Range)xlWorkSheetReputasi.Cells[1, 7]).EntireColumn.ColumnWidth = 10;

                    //get sheet content
                    object[] paramReputasi = { reffnumber, appid };
                    DataTable dtReputasi = conn.GetDataTable("EXEC sp_vw_excel_reputasi @1,@2", paramReputasi, dbtimeout);

                    //row dtreputasi
                    for (int i = 0; i <= dtReputasi.Rows.Count - 1; i++)
                    {
                        int startRow = int.Parse(dtReputasi.Rows[i][0].ToString());
                        int startColumn = int.Parse(dtReputasi.Rows[i][1].ToString());
                        int EndRow = int.Parse(dtReputasi.Rows[i][2].ToString());
                        int EndColumn = int.Parse(dtReputasi.Rows[i][3].ToString());

                        if (EndRow == 0 || EndColumn == 0)
                        {
                            EndRow = startRow;
                            EndColumn = startColumn;
                        }

                        bool isMerge = bool.Parse(dtReputasi.Rows[i][4].ToString());
                        string bgColor = dtReputasi.Rows[i][5].ToString();
                        bool isbold = bool.Parse(dtReputasi.Rows[i][6].ToString());

                        string valueType = dtReputasi.Rows[i][7].ToString();
                        string valueColumn = dtReputasi.Rows[i][8].ToString();
                        float valueColumn_numeric = float.Parse(dtReputasi.Rows[i][9].ToString());
                        string datatype = dtReputasi.Rows[i][10].ToString();

                        //Excel.Range range = (Excel.Range)xlWorkSheetReputasi.Cells[i + 2, j + 1];                        
                        Excel.Range range = (Excel.Range)xlWorkSheetReputasi.Range[xlWorkSheetReputasi.Cells[startRow, startColumn], xlWorkSheetReputasi.Cells[EndRow, EndColumn]];

                        //range.ColumnWidth = 7;
                        if (startColumn == 2)
                        {
                            range.Cells.WrapText = true;
                        }
                        else
                        {
                            range.Cells.WrapText = false;
                        }

                        if (valueType.Trim() == "int" || valueType.Trim() == "formula_int" || valueType.Trim() == "formula_int_border")
                        {
                            range.NumberFormat = "0";
                        }
                        else if (valueType.Trim() == "formula_decimal" || valueType.Trim() == "formula_decimal_border")
                        {
                            //range.NumberFormat = "#,##0.00";
                            range.NumberFormat = "0.00%";
                        }
                        else if (valueType.Trim() == "date" || valueType.Trim() == "date_border")
                        {
                            range.NumberFormat = "mm/dd/yyyy";
                        }
                        else if (datatype == "string" && valueType.Trim() != "formula")
                        {
                            range.NumberFormat = "@";
                        }
                        else if (datatype == "numeric" || valueType.Trim() == "formula")
                        {
                            range.NumberFormat = "Rp #,##0.00";
                        }

                        if (valueType.Trim() == "text_center")
                        {
                            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignJustify;
                            range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignJustify;
                        }

                        if (isMerge)
                        {
                            range.Merge();
                        }

                        //if (valueType.Trim().Contains("border"))
                        //{
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        //    range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 1d;
                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 1d;
                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 1d;
                        //    //range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 1d;
                        //}

                        //bgColor                     
                        if (bgColor == "Old Green")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                        }
                        else if (bgColor == "Pink")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Moccasin);
                        }
                        else if (bgColor == "Yellow")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        }
                        else if (bgColor == "Young Green")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Honeydew);
                        }
                        else if (bgColor == "blue")
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);
                        }
                        else
                        {
                            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        }

                        ////isbold
                        //if (isbold)
                        //{
                        //    range.Font.Bold = true;
                        //}

                        if (valueType.Trim() == "formula" || valueType.Trim() == "formula_int" || valueType.Trim() == "formula_decimal" || valueType.Trim() == "formula_decimal_border" || valueType.Trim() == "formula_int_border")
                        {
                            string formula = valueColumn;
                            range.Formula = formula;

                            range.Calculate();
                        }
                        else
                        {
                            if (valueType.Trim() == "int")
                            {
                                int[] intArray = new int[] { int.Parse(valueColumn_numeric.ToString()) };
                                range.Value = intArray;
                            }
                            else if (datatype == "numeric")
                            {
                                float[] floatArray = new float[] { valueColumn_numeric };
                                range.Value = floatArray;
                            }
                            else
                            {
                                valueColumn = valueColumn.TrimEnd(new char[] { '\r', '\n' });
                                string[] stringArray = new string[] { valueColumn };
                                range.Value = stringArray;
                            }
                        }
                    }
                    //end row dtreputasi

                    //sheet reputasi
                    #endregion
                }
                //end looping sheet
                #endregion

                #region sheet laporan
                //laporan
                string sheetlaporanName = "Laporan";
                xlWorkSheetLaporan = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBookReputasi.Worksheets.Add(misValue, misValue, misValue, misValue);
                xlWorkSheetLaporan.Name = sheetlaporanName;
                //xlWorkSheetLaporan.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
                //xlWorkSheetLaporan.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                //xlWorkSheetLaporan.PageSetup.LeftMargin = 0.1;
                //xlWorkSheetLaporan.PageSetup.RightMargin = 0.1;
                //xlWorkSheetLaporan.PageSetup.TopMargin = 0.1;
                //xlWorkSheetLaporan.PageSetup.BottomMargin = 0.1;
                //xlWorkSheetLaporan.PageSetup.HeaderMargin = 0.1;
                //xlWorkSheetLaporan.PageSetup.FooterMargin = 0.1;

                //get sheet content
                object[] paramLaporan = { reffnumber };
                DataTable dtLaporan = conn.GetDataTable("EXEC sp_vw_excel_laporan @1", paramLaporan, dbtimeout);

                //row dtLaporan
                for (int i = 0; i <= dtLaporan.Rows.Count - 1; i++)
                {
                    int startRow = int.Parse(dtLaporan.Rows[i][0].ToString());
                    int startColumn = int.Parse(dtLaporan.Rows[i][1].ToString());
                    int EndRow = int.Parse(dtLaporan.Rows[i][2].ToString());
                    int EndColumn = int.Parse(dtLaporan.Rows[i][3].ToString());

                    if (EndRow == 0 || EndColumn == 0)
                    {
                        EndRow = startRow;
                        EndColumn = startColumn;
                    }

                    bool isMerge = bool.Parse(dtLaporan.Rows[i][4].ToString());
                    string bgColor = dtLaporan.Rows[i][5].ToString();
                    bool isbold = bool.Parse(dtLaporan.Rows[i][6].ToString());

                    string valueType = dtLaporan.Rows[i][7].ToString();
                    string valueColumn = dtLaporan.Rows[i][8].ToString();
                    float valueColumn_numeric = float.Parse(dtLaporan.Rows[i][9].ToString());
                    string datatype = dtLaporan.Rows[i][10].ToString();

                    //Excel.Range range = (Excel.Range)xlWorkSheetReputasi.Cells[i + 2, j + 1];                        
                    Excel.Range range = (Excel.Range)xlWorkSheetLaporan.Range[xlWorkSheetLaporan.Cells[startRow, startColumn], xlWorkSheetLaporan.Cells[EndRow, EndColumn]];
                    range.ColumnWidth = 7;
                    //range.Cells.WrapText = false;

                    if (valueType.Trim() == "int" || valueType.Trim() == "formula_int" || valueType.Trim() == "formula_int_border")
                    {
                        range.NumberFormat = "0";
                    }
                    else if (valueType.Trim() == "formula_decimal" || valueType.Trim() == "formula_decimal_border")
                    {
                        //range.NumberFormat = "#,##0.00";
                        range.NumberFormat = "0.00%";
                    }
                    else if (valueType.Trim() == "date" || valueType.Trim() == "date_border")
                    {
                        range.NumberFormat = "mm/dd/yyyy";
                    }
                    else if (datatype == "string" && valueType.Trim() != "formula")
                    {
                        range.NumberFormat = "@";
                    }
                    else if (datatype == "numeric" || valueType.Trim() == "formula")
                    {
                        range.NumberFormat = "Rp #,##0.00";
                    }

                    if (isMerge)
                    {
                        range.Merge();
                    }

                    if (startColumn == 3)
                    {
                        range.Cells.WrapText = true;
                    }

                    //if (valueType.Trim() == "text_center")
                    //{
                    //    range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    //    range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    //}

                    if (valueType.Trim() == "image")
                    {
                        string sFile = "logoreport.jpg";
                        string imagString = AppDomain.CurrentDomain.BaseDirectory + "\\image\\" + sFile;
                        float Left = (float)((double)range.Left) - 40;
                        float Top = (float)((double)range.Top) + 5;
                        const float ImageSizeX = 140;
                        const float ImageSizeY = 50;
                        xlWorkSheetLaporan.Shapes.AddPicture(imagString, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSizeX, ImageSizeY);
                    }

                    //if (valueType.Trim().Contains("border"))
                    //{
                    //    range.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //    range.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //    range.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    //    range.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;

                    //    //range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = 1d;
                    //    //range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = 1d;
                    //    //range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = 1d;
                    //    //range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = 1d;                        
                    //}

                    //bgColor                     
                    if (bgColor == "Old Green")
                    {
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                    }
                    else if (bgColor == "Pink")
                    {
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Moccasin);
                    }
                    else if (bgColor == "Yellow")
                    {
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    }
                    else if (bgColor == "Young Green")
                    {
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Honeydew);
                    }
                    else if (bgColor == "blue")
                    {
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);
                    }
                    else
                    {
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                    }

                    ////isbold
                    //if (isbold)
                    //{
                    //    range.Font.Bold = true;
                    //}

                    if (valueType.Trim() == "formula" || valueType.Trim() == "formula_int" || valueType.Trim() == "formula_decimal" || valueType.Trim() == "formula_decimal_border" || valueType.Trim() == "formula_int_border")
                    {
                        string formula = valueColumn;
                        range.Formula = formula;

                        range.Calculate();
                    }
                    else
                    {

                        if (valueType.Trim() == "int")
                        {
                            int[] intArray = new int[] { int.Parse(valueColumn_numeric.ToString()) };
                            range.Value = intArray;
                        }
                        else if (datatype == "numeric")
                        {
                            float[] floatArray = new float[] { valueColumn_numeric };
                            range.Value = floatArray;
                        }
                        else
                        {
                            valueColumn = valueColumn.TrimEnd(new char[] { '\r', '\n' });
                            string[] stringArray = new string[] { valueColumn };
                            range.Value = stringArray;
                        }
                    }
                }
                //end row dtLaporan

                //end laporan
                #endregion

                #endregion

                #region end excel

                string filename = reffnumber + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string flnm = AppDomain.CurrentDomain.BaseDirectory + "Download\\excel\\" + filename;

                msg = flnm;

                xlApp.DisplayAlerts = false;
                xlWorkBookReputasi.CheckCompatibility = false;
                xlWorkBookReputasi.DoNotPromptForConvert = true;

                xlWorkBookReputasi.SaveAs(flnm, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBookReputasi.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkBookReputasi);
                releaseObject(xlApp);

                mainPanel.JSProperties["cp_redirect"] = "../Download/excel/" + filename;
                //mainPanel.JSProperties["cp_target"] = "_blank";

                //mainPanel.JSProperties["cp_alert"] = "Export Excel Success " + msg;
                #endregion
            }
            catch (Exception ex)
            {
                mainPanel.JSProperties["cp_alert"] = ex.Message;
            }

        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                string Msg = "Exception Occured while releasing object " + ex.ToString();
                mainPanel.JSProperties["cp_alert"] = Msg;
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion

        protected void pdfPanel_Callback(object source, CallbackEventArgsBase e)
        {
            try
            {
                string DownloadPath = Server.MapPath("../Download/pdf/");

                PdfConverter pdfConverter = new PdfConverter();
                // set the license key
                pdfConverter.LicenseKey = "Vn1ndmVldmdkdmV4Y3ZlZ3hnZHhvb29v";
                // save the PDF bytes in a file on disk
                string url = e.Parameter + "&bypasssession=1";
                conn.ExecReader("select cust_name from appreputasi where reffnumber = @1",
                    new object[] { Request.QueryString["reffnum"] }, dbtimeout);
                string filename = "";
                if (conn.hasRow()) filename = "ReputasiDetail_" + conn.GetFieldValue(0).ToString() + "_" + USERID + ".pdf";
                string fullfilename = DownloadPath + filename;
                pdfConverter.SavePdfFromUrlToFile(url, fullfilename);
                pdfPanel.JSProperties["cp_redirect"] = "../Download/pdf/" + filename;
                pdfPanel.JSProperties["cp_target"] = "_blank";
                //Response.Write("<script>window.open('../Download/pdf/" + filename + "');</script>");
            }
            catch (Exception ex)
            {
                pdfPanel.JSProperties["cp_alert"] = ex.Message;
            }
        }
    }
}

