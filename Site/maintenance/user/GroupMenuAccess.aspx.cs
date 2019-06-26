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

using System.Xml;
using System.IO;
using My.Tools;

namespace MikroMnt.user
{
    public partial class GroupMenuAccess : System.Web.UI.Page
    {
        private DbConnection conn;
        private int dbtimeout;
        private string dbconnstr;

        #region static vars
        private static string Q_GRPACCESSMODULE = "select distinct moduleid, modulename from vw_grpaccessmodule ";
        private static string Q_MODULEMENU = "select menuid, menudesc from vw_modulemenux where typeid = @1 ";
        private static string Q_MENULIST = "select menuid, menudesc from vw_menuxlist where typeid = @1 and menuparent = @2 ";
        private static string Q_GRPACCESSMENU = "select menuid from vw_grpmenux where groupid = @1 and typeid = @2 ";
        private static string Q_GRPACCESSMENU_PENDING = "select menuid from vw_pending_grpmenux where groupid = @1 and typeid = @2 ";
        private static string U_DELGRPACCESSMENU = "delete from pending_grpmenux " +
            "where groupid = @1 and typeid = @2 ";
        private static string SP_INSGRPACCESSMENU = "exec USP_GRPACCESSMENUX_SAVE @1, @2, @3 ";
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            dbconnstr = Session["ConnStringLogin"].ToString();
            conn = new DbConnection(dbconnstr);
            base.OnLoad(e);
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
            int rowNumber = 0;

            conn.ExecReader(Q_GRPACCESSMODULE, null, dbtimeout);
            while (conn.hasRow())
            {
                HyperLink t = new HyperLink();
                t.Text = conn.GetFieldValue("modulename");
                t.Font.Bold = true;
                t.NavigateUrl = "GroupMenuAccess.aspx?GroupID=" + Request.QueryString["GroupID"] + "&ModuleID=" + conn.GetFieldValue("moduleid") + 
                    "&ModuleName=" + conn.GetFieldValue("modulename")+ "&type=" + Request.QueryString["type"];
                if (Request.QueryString["Readonly"] != null) t.NavigateUrl += "&Readonly=1";
                PlaceHolder1.Controls.Add(t);
                PlaceHolder1.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));
            }

            TBL_MENU.Rows.Add(new TableRow());
            TBL_MENU.Rows[rowNumber].Cells.Add(new TableCell());
            try
            {
                if (Request.QueryString["ModuleName"].Length != 0)
                    TBL_MENU.Rows[rowNumber].Cells[0].Text = Request.QueryString["ModuleName"];
            }
            catch
            {
                TBL_MENU.Rows[rowNumber].Cells[0].Text = "- CHOOSE A MODULE -";
            }

            TBL_MENU.Rows[rowNumber].Cells[0].CssClass = "H1";
            rowNumber++;

            object[] parmod = new object[1] { Request.QueryString["ModuleID"] };
            DataTable dtMenu = conn.GetDataTable(Q_MODULEMENU, parmod, dbtimeout);

            for (int i = 0; i < dtMenu.Rows.Count; i++)
            {
                TBL_MENU.Rows.Add(new TableRow());
                TBL_MENU.Rows[rowNumber].Cells.Add(new TableCell());
                TBL_MENU.Rows[rowNumber].Cells[0].Text = dtMenu.Rows[i]["menudesc"].ToString();
                TBL_MENU.Rows[rowNumber].Cells[0].Font.Bold = true;
                CheckBoxList chkList = new CheckBoxList();
                chkList.ID = "chkList_" + i.ToString();
                rowNumber++;

                TBL_MENU.Rows.Add(new TableRow());
                TBL_MENU.Rows[rowNumber].Cells.Add(new TableCell());
                TBL_MENU.Rows[rowNumber].Cells[0].Controls.Add(chkList);

                rowNumber++;

                if (!IsPostBack)
                {
                    fillChildList(Request.QueryString["ModuleID"], dtMenu.Rows[i]["menuid"].ToString(), chkList, "");
                }

                TBL_MENU.Rows.Add(new TableRow());
                TBL_MENU.Rows[rowNumber].Cells.Add(new TableCell());
                TBL_MENU.Rows[rowNumber].Cells[0].Text = "&nbsp;";
                rowNumber++;
            }
            ViewData(Request.QueryString["type"]);
            if (Request.QueryString["Readonly"] != null)
                BTN_SAVE.Visible = false;
        }

        private void fillChildList(string typeid, string menuid, ListControl lc, string parentdesc)
        {
                object[] parsub = new object[2] { typeid, menuid };
                DataTable dtSubMenu = conn.GetDataTable(Q_MENULIST, parsub, dbtimeout);
                for (int i = 0; i < dtSubMenu.Rows.Count; i++)
                {
                    string desc = parentdesc + dtSubMenu.Rows[i]["menudesc"].ToString(),
                        id = dtSubMenu.Rows[i]["menuid"].ToString();
                    lc.Items.Add(new ListItem(desc, id));
                    fillChildList(typeid, id, lc, desc + "  >  ");
                }
        }

        private void ViewData(string type = "")
        {
            object[] parmod = new object[2] { Request.QueryString["GroupID"], Request.QueryString["ModuleID"] };
            if (type == "pending")
            {
                conn.ExecReader(Q_GRPACCESSMENU_PENDING, parmod, dbtimeout);
                if (!conn.hasRow())
                    conn.ExecReader(Q_GRPACCESSMENU, parmod, dbtimeout);
            }
            else
                conn.ExecReader(Q_GRPACCESSMENU, parmod, dbtimeout);

            while (conn.hasRow())
            {
                for (int k = 0; k < TBL_MENU.Rows.Count; k++)
                {
                    CheckBoxList temp = null;
                    try
                    {
                        temp = (CheckBoxList)TBL_MENU.Rows[k].Cells[0].Controls[0];
                    }
                    catch { continue; }
                    for (int l = 0; l < temp.Items.Count; l++)
                    {
                        if (temp.Items[l].Value == conn.GetFieldValue(0))
                            temp.Items[l].Selected = true;
                    }
                }
            }
        }

        private void UpdateMenuAccess()
        {
            object[] parmod = new object[2] { Request.QueryString["GroupID"], Request.QueryString["ModuleID"] };
            conn.ExecuteNonQuery(U_DELGRPACCESSMENU, parmod, dbtimeout);

            // insert new selected ones
            for (int k = 0; k < TBL_MENU.Rows.Count; k++)
            {
                CheckBoxList cbTemp = null;
                try
                {
                    cbTemp = (CheckBoxList)TBL_MENU.Rows[k].Cells[0].Controls[0];
                }
                catch { continue; }

                for (int j = 0; j < cbTemp.Items.Count; j++)
                {
                    if (cbTemp.Items[j].Selected)
                    {
                        object[] parmenu = new object[3] { Request.QueryString["GroupID"], Request.QueryString["ModuleID"], cbTemp.Items[j].Value };
                        conn.ExecuteNonQuery(SP_INSGRPACCESSMENU, parmenu, dbtimeout);
                    }
                }
            }
        }

        protected void BTN_SAVE_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateMenuAccess();
                ViewData("pending");
                MyPage.popMessage(this, "Menu Access submitted! Awaiting approval..\nPlease click save again on group level...");
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
                MyPage.popMessage(this, "Failed...");
            }
        }

    }
}
