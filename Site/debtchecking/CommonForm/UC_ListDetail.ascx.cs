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
using DevExpress.Web;
using System.IO;

namespace DebtChecking.List
{
    public partial class UC_ListDetail : System.Web.UI.UserControl
    {
        protected DbConnection conn;
        protected int dbtimeout;
        protected string USERID, GROUPID;
        
        private static string svrpathurl = "../Upload/List";
        private static string strfiletmpl = "../Template";
        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            dbtimeout = (int)Session["DbTimeOut"];
            conn = new DbConnection((string)Session["ConnString"]);
            USERID = (string)Session["UserID"];
            GROUPID = (string)Session["GroupID"];
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            try
            {
                conn.Dispose();
            }
            catch { }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["li"] != null && Request.QueryString["li"].Trim() != "")
            {
                if (!IsPostBack)
                {
                    string strSQL = "", title = "";
                    string det1desc = "", det2desc = "";
                    string chkCmdText = "";
                    bool allowExport = false;
                    ListSys.gridInit(grid, Request.QueryString["li"], ref title, ref strSQL, conn, ref det1desc, ref det2desc, ref chkCmdText, ref allowExport);
                    TitleHeader.Text = title;
                    ViewState["strSQL"] = strSQL;
                    ViewState["det1desc"] = det1desc;
                    ViewState["det2desc"] = det2desc;
                    ViewState["chkCmdText"] = chkCmdText;
                    ViewState["allowExport"] = allowExport;
                };
                ListSys.gridLoad(grid, (string)ViewState["det1desc"], (string)ViewState["det2desc"]);
                if (ViewState["chkCmdText"] != null && (string)ViewState["chkCmdText"] != "")
                {
                    grid.Columns[1].Visible = true;
                    btnSave.Visible = true;
                    btnSave.Text = (string)ViewState["chkCmdText"];
                }
                if ((bool)ViewState["allowExport"])
                {
                    btnExport.Visible = true;
                }
            }
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["li"] != null && Request.QueryString["li"].Trim() != "")
                ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
            grid.Settings.ShowFooter = false;
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
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
            try
            {
                HyperLink hlDetail2 = grid.FindRowCellTemplateControl(e.VisibleIndex, grid.Columns["ListDetail2"] as GridViewDataColumn, "hlDetail") as HyperLink;
                string ListDetail2 = grid.GetRowValues(e.VisibleIndex, "ListDetail2").ToString();
                if (ListDetail2.StartsWith("javascript"))
                {
                    hlDetail2.NavigateUrl = "javascript:void(0)";
                    hlDetail2.Attributes.Add("onclick", ListDetail2);
                }
                else
                {
                    hlDetail2.NavigateUrl = ListDetail2;
                }
            }
            catch { }
            try
            {
                Table tblReason = grid.FindRowCellTemplateControl(e.VisibleIndex, grid.Columns["ListReasonQry"] as GridViewDataColumn, "tblReason") as Table;
                string ListReasonQry = grid.GetRowValues(e.VisibleIndex, "ListReasonQry").ToString();
                if (ListReasonQry != null && ListReasonQry.Trim() != "")
                {
                    DataTable dtReason = conn.GetDataTable(ListReasonQry, null, dbtimeout);
                    for (int i = 0; i < dtReason.Rows.Count; i++)
                    {
                        TableRow r = new TableRow();
                        TableCell c = new TableCell();
                        c.Wrap = false;
                        c.Text = dtReason.Rows[i][0].ToString();
                        r.Cells.Add(c);
                        tblReason.Rows.Add(r);
                    }
                }
            }
            catch { }
        }

        protected void grid_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (!grid.Columns[1].Visible)
                grid.Columns[0].Visible = (grid.FilterExpression != "");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<object> keyValues = grid.GetSelectedFieldValues(new string[] { grid.KeyFieldName });
            string ListChkBoxCmd = "";
            foreach (object key in keyValues)
            {
                ListChkBoxCmd = grid.GetRowValuesByKeyValue(key, "ListChkBoxCmd").ToString();
                conn.ExecNonQuery(ListChkBoxCmd, null, dbtimeout);
            }
            grid.Selection.UnselectAll();
            if (ListChkBoxCmd.IndexOf("USP_RESTORE_DATA") > 0)
                Response.Write("<script>alert('Restore Data Success.')</script>");
            ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string strFileName = Session["userid"] + "_" + DateTime.Now.ToString("yyMMddhhmmss");
                string strFilePath = Server.MapPath(svrpathurl) + "\\" + strFileName + ".xls";
                System.IO.FileStream fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                gridExport.WriteXls(fs);
                fs.Dispose();

                Response.Write("<script for=window event=onload language='JavaScript'>");
                Response.Write("window.open('" + svrpathurl + "/" + strFileName + ".xls" + "');");
                Response.Write("</script>");
            } catch(Exception ex)
            {
                MyPage.popMessage(Page, ex.Message);
            }
        }
    }
}