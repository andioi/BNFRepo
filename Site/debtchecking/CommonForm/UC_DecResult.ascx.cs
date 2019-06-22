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

using DMS.Tools;
using DMS.Framework;

namespace DebtChecking.CommonForm
{
    public partial class UC_DecResult : System.Web.UI.UserControl
    {
        public string Stage;
        public string regno;
        public Hashtable hashFieldFWsend = null;
        public DbConnection conn;
        public int dbtimeout;
        public bool ReRunOnStart = false;
        
        private void gridbind()
        {
            object[] param = new object[] { regno, Stage };
            GridViewDec.DataSource = conn.GetDataTable("EXEC USP_APPDECISIONRESULT @1,@2", param, (int)Session["DbTimeOut"]);
            GridViewDec.DataBind();
        }

        private void gridbinddetail(DevExpress.Web.ASPxGridView GridViewDecDtl)
        {
            object[] param = new object[] { GridViewDecDtl.GetMasterRowKeyValue() };
            GridViewDecDtl.DataSource = conn.GetDataTable("EXEC USP_APPDECISIONRESULTDETAIL @1", param, (int)Session["DbTimeOut"]);
            GridViewDecDtl.DataBind();
        }

        public void rundec(bool rerun)
        {
            LBL_MSG.Text = "";
            string retmsg = "";
            if (Request.QueryString["readonly"] != null)
                return;

            object[] param = new object[] { regno, Stage, rerun };
            DataTable dtRunDecSys = conn.GetDataTable(
                "EXEC SP_APPDECSYSRUN @1,@2,@3", param, dbtimeout);

            for (int i = 0; i < dtRunDecSys.Rows.Count; i++)
            {
                try
                {
                    DecSystem DecSys = new DecSystem(conn);
                    string DecId = dtRunDecSys.Rows[i]["DEC_ID"].ToString();
                    param = new object[] { regno, Stage };

                    string ResultID = DecSys.execute(
                        "AND [APP].[AP_REGNO]=@1", param,
                        DecId, Stage, hashFieldFWsend);
                    param = new object[] { regno, DecId, Stage, ResultID };
                    conn.ExecuteNonQuery(
                        "EXEC SP_APPDECSYSRES @1,@2,@3,@4", param, dbtimeout);
                }
                catch (Exception ex)
                {
                    ModuleSupport.LogError(this.Page, ex);
                    string errDecId = dtRunDecSys.Rows[i]["DEC_ID"].ToString();
                    retmsg += errDecId + ", ";
                }
            }
            if (retmsg != "")
                LBL_MSG.Text = "<font color=Red> Execution error on engine: " + retmsg.Substring(0, retmsg.Length - 2) + "</font>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void GridViewDec_Load(object sender, EventArgs e)
        {
            if (Stage != null && regno != null && conn != null)
            {
                if (Request.QueryString["readonly"] == null)
                    rundec(ReRunOnStart);
                gridbind();
                GridViewDec.SettingsDetail.ShowDetailRow = true;
            }
        }


        protected void GridViewDecDtl_Load(object sender, EventArgs e)
        {
            gridbinddetail((DevExpress.Web.ASPxGridView) sender);
        }

        protected void GridViewDec_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {

        }
    }
}