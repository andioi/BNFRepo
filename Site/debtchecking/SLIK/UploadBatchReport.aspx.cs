using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.Tools;
using DMS.Framework;
using DevExpress.Spreadsheet;

namespace DebtChecking.SLIK
{
    public partial class UploadBatchReport : DataPage
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
            //dt = conn.GetDataTable("select * from " + tableName + " where batchid = '" + batchId + "' and isnull(msg,'') != ''", null, dbtimeout);
            conn.ExecReader("select top 1 1 from " + tableName + " where batchid = '" + batchId + "' and isnull([message],'') = 'success'", null, dbtimeout);
            if (conn.hasRow())
            {
                btnSubmit.Visible = true;
                //btnCancel.Visible = true;
            }
            else
            {
                btnSubmit.Visible = false;
                //btnCancel.Visible = false;
            }
        }

        protected void mainPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "e:")
                {
                    dt = conn.GetDataTable(@"select * from vw_export_batch_upload where batchid = @1", new object[] { batchId }, dbtimeout);
                    Workbook wb = new Workbook();
                    wb.Worksheets[0].Import(dt, true, 0, 0);

                    wb.SaveDocument(MapPath("~/Upload/Report/uploadResult.xlsx"), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    mainPanel.JSProperties["cp_export"] = "../Upload/Report/uploadResult.xlsx";
                }
                else if (e.Parameter == "s:")
                {
                    conn.ExecNonQuery("exec SP_SEND_TO_CBAS_DESKTOP @1, @2", new object[] { batchId, USERID }, dbtimeout);
                    string msg = "Batch request telah berhasil disubmit";
                    mainPanel.JSProperties["cp_alert"] = msg;
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BATCH|DRFT";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                }
                else if (e.Parameter == "c:")
                {
                    conn.ExecNonQuery("update CBAS_UPLOAD_BATCH set current_track = '9', current_track_by = NULL where batchid = @1", new object[] { batchId }, dbtimeout);
                    conn.ExecNonQuery("insert into CBAS_UPLOAD_BATCH_HISTORY values (@1, '9',@2, getdate(),'Cancel by uploader')", new object[] { batchId,  USERID }, dbtimeout);
                    string msg = "Batch request ideb telah dibatalkan";
                    mainPanel.JSProperties["cp_alert"] = msg;
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BATCH|DRFT";
                    mainPanel.JSProperties["cp_target"] = "_parent";
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

        protected void grid_Load(object sender, EventArgs e)
        {
            ReportSys.gridBind(grid, "select * from " + tableName + " where batchid = '" + batchId + "' ", null, null, conn);
        }
    }
}