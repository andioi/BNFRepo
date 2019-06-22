using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.Tools;
using DMS.Framework;
using DevExpress.Spreadsheet;
using System.Data;
using System.IO;

namespace DebtChecking.SLIK
{
    public partial class BatchApproval : DataPage
    {
        string batchId;
        string status;
        string tableName;
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            batchId = Request.QueryString["batchId"];
            status = Request.QueryString["sts"];
            tableName = Request.QueryString["tbl"];
            if (!Page.IsPostBack)
            {               
                if (status == "1")
                {
                    
                    //grpBtnSend.Visible = true;
                    grpBtnApprove.Visible = false;
                    btnExporter.Visible = true;
                }
                else if(status == "2")
                {
                    //grpBtnSend.Visible = false;
                    grpBtnApprove.Visible = true;
                    btnExporter.Visible = false;
                }
            }            
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            
            ReportSys.gridBind(grid, "select * from "+ tableName+" where batchid = '" + batchId + "'", null, null, conn);            
        }

        protected void mainPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            { 
                if (e.Parameter == "a:")
                {
                    conn.ExecNonQuery("exec SP_SEND_TO_CBAS_DESKTOP @1, @2", new object[] { batchId, USERID }, dbtimeout);
                    string msg = "Batch data has been submit";
                    mainPanel.JSProperties["cp_alert"] = msg;
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BATCH|APPR";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                }
                else if (e.Parameter == "e:")
                {
                    conn.ExecReader("select tablename from CBAS_UPLOAD_BATCH where batchId = @1 ", new object[] { batchId }, dbtimeout);
                    if (conn.hasRow())
                    {
                        dt = conn.GetDataTable("select * from " + conn.GetFieldValue(0).ToString() + " where batchid = @1", new object[] { batchId }, dbtimeout);
                        Workbook wb = new Workbook();
                        wb.Worksheets[0].Import(dt, true, 0, 0);

                        wb.SaveDocument(MapPath("~/Upload/Report/uploadResult.xlsx"), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                        mainPanel.JSProperties["cp_export"] = "../Upload/Report/uploadResult.xlsx";
                    }
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                if (errmsg.IndexOf("Last Query") > 0)
                    errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                mainPanel.JSProperties["cp_alert"] = errmsg;
            }
        }


        protected void PNL_REMARK_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                conn.ExecNonQuery("exec SP_BATCH_REJECT @1, @2, @3", new object[] { batchId, detAN_MESSAGE.Text, USERID }, dbtimeout);
                string msg = "Batch data has been rejected";
                PNL_REMARK.JSProperties["cp_alert"] = msg;
                PNL_REMARK.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BATCH|APPR";
                PNL_REMARK.JSProperties["cp_target"] = "_parent";
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                if (errmsg.IndexOf("Last Query") > 0)
                    errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                PNL_REMARK.JSProperties["cp_alert"] = errmsg;
            }
        }
      
    }
}