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
using DevExpress.Web;
using DMS.Framework;

namespace DebtChecking.CommonForm
{
    public partial class ListSend : DataPage
    {
        static string Q_FILLOFFICER = "exec USP_FAC_REASG_GETOFC";
        static string SP_ASSIGN = "exec sp_update_request @1,@2,@3,@4,@5,@6";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["li"] != null && Request.QueryString["li"].Trim() != "")
            {
                if (!IsPostBack)
                {
                    object[] par = new object[] {Request.QueryString["sts"], Session["ModuleID"], USERID};
                    staticFramework.reff(ddl_Officer, Q_FILLOFFICER, par, conn);

                    string strSQL = "", title = "";
                    ListSys.gridInit(grid, Request.QueryString["li"], ref title, ref strSQL, conn);
                    TitleHeader.Text = title;
                    ViewState["strSQL"] = strSQL;
                };
                ListSys.gridLoad(grid);
            }
            btnSave.Attributes.Add("onclick", "if (!cek_mandatory(document.form1)) return false; ");
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["li"] != null && Request.QueryString["li"].Trim() != "")
                ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }

        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                HyperLink hlDetail = grid.FindRowCellTemplateControl(e.VisibleIndex, grid.Columns["ListDetail"] as GridViewDataColumn, "hlDetail") as HyperLink;
                string ListDetail = grid.GetRowValues(e.VisibleIndex, "ListDetail").ToString();
                if (ListDetail.StartsWith("javascript"))
                {
                    hlDetail.NavigateUrl = "javascript:void(0)";
                    hlDetail.Attributes.Add("onclick", ListDetail);
                }
                else
                {
                    hlDetail.NavigateUrl = ListDetail;
                }
            }
            catch { }
        }

        protected void grid_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<object> keyValues = grid.GetSelectedFieldValues(new string[] { grid.KeyFieldName });
            try
            {
                foreach (object key in keyValues)
                {
                    string nextstatus = "APV";
                    if (Request.QueryString["sts"] == "BMA") nextstatus = "GCC";
                    object[] par = new object[] { key, Request.QueryString["sts"], nextstatus, ddl_Officer.SelectedValue, USERID, null };
                    conn.ExecNonQuery(SP_ASSIGN, par, dbtimeout);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                if (errmsg.IndexOf("Last Query") > 0)
                    errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                DMS.Tools.MyPage.popMessage(this, errmsg);
            }
            grid.Selection.UnselectAll();
            ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }
    }
}
