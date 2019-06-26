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

using System.Drawing;
using My.Tools;

namespace MikroMnt.user
{
    public partial class GroupMaintenance : System.Web.UI.Page
    {
        private int dbtimeout;
        private DbConnection conn;
        private string orderby;

        #region static vars
        private static string Q_RFMODULE = "select moduleid, modulename from rfmodule where active = '1' ";
        private static string Q_GROUP = "select * from VW_SCALLGROUP ";
        private static string Q_MODULE = "select * from rfmodule where moduleid = @1 ";
        private static string SP_DELETE = "exec SU_SCALLGROUP_DELETE @1, @2, @3, @4, @5 ";
        private static string SP_SAVE = "exec SU_SCALLGROUP_SAVE @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12 ";
        private static string SP_SAVEMENUXML = "exec USP_GRPMENUALL @1, @2, @3 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);

            if (!IsPostBack)
            {
                conn = new DbConnection(Session["ConnStringLogin"].ToString());

                MyPage.fillRefList(DDL_MODULEID, Q_RFMODULE + " and moduleid in (" + MaintainedModuleIDs + ")", null, dbtimeout, false, conn);
                conn.ExecReader(Q_RFMODULE + " and moduleid in (" + MaintainedModuleIDs + ")", null, dbtimeout);
                while (conn.hasRow())
                {
                    CHK_MODULEID.Items.Add(new ListItem(conn.GetFieldValue(1), conn.GetFieldValue(0)));
                }
                string cond = " order by sg_grpname ";
                MyPage.fillRefList(DDL_SG_GRPUPLINER, Q_GROUP + cond, null, dbtimeout, true, conn);

                ClearEntries();

                orderby = "";
                ViewState["orderby"] = orderby;

                conn.Dispose();
            }
            else
            {
                orderby = (string)ViewState["orderby"];
            }

            LBL_RESULT.Text = "";
            BTN_SAVE.Attributes.Add("onclick", "if(!cek_mandatory(document.form1)){return false;} else {simpan();};");
            IFR_MODULE.Attributes.Add("onload", "resizeFrame()");
        }

        #region General Function
        private void EnableFields(bool mode)
        {
            TXT_GROUPID.Enabled = mode;
            TXT_SG_GRPNAME.Enabled = mode;
            CHK_MODULEID.Enabled = mode;
            CHK_SG_APPRSTA.Enabled = mode;
            MEMBEROF_AD.Enabled = mode;
            CHK_SG_CALCULATOR.Enabled = mode;
            FLAG_SUPERVISOR.Enabled = mode;
            SG_ROLEDESC.Enabled = mode;
            lnk_menu.Enabled = mode;
        }

        private void ClearEntries()
        {
            TXT_GROUPID.Text = "";
            TXT_SG_GRPNAME.Text = "";
            SG_ROLEDESC.Text = "";

            for (int i = 0; i < CHK_MODULEID.Items.Count; i++)
                CHK_MODULEID.Items[i].Selected = false;

            DDL_SG_GRPUPLINER.SelectedValue = "";

            CHK_SG_APPRSTA.Checked = false;
            CHK_SG_CALCULATOR.Checked = false;
            FLAG_SUPERVISOR.Checked = false;
            LBL_SAVEMODE.Text = "1";

            BTN_NEW.Visible = true;
            BTN_CANCEL.Visible = false;
            BTN_SAVE.Visible = false;

            RBL_MODULE.Items.Clear();
            IFR_MODULE.Attributes.Remove("src");

            EnableFields(false);
            TXT_GROUPID.ReadOnly = false;
        }

        private bool CHK_MODULEID_Selected(string moduleid)
        {
            try
            {
                return CHK_MODULEID.Items.FindByValue(moduleid).Selected;
            }
            catch { }
            return false;
        }

        #endregion

        #region DataGrid
        private string SQLCondition()
        {
            string sqlCond = "";
            //if (DDL_MODULEID.SelectedValue != "")
            //{
            //    sqlCond = " WHERE MODULEID = '" + DDL_MODULEID.SelectedValue + "' ";
            //}
            //else
            //{
            //    sqlCond = " WHERE MODULEID in (" + MaintainedModuleIDs + ") ";
            //}
            if (TXT_FINDGROUPID.Text.Trim() != "")
            {
                sqlCond = " WHERE GROUPID like '%" + TXT_FINDGROUPID.Text + "%' ";
            }
            else
            {
                sqlCond = " WHERE 1=1 ";
            }
            if (TXT_FINDGROUP.Text.Trim() != "")
            {
                sqlCond += " AND SG_GRPNAME like '%" + TXT_FINDGROUP.Text.Trim() + "%' ";
            }

            if (TXT_FINDUPLINER.Text.Trim() != "")
            {
                sqlCond += " AND (SG_GRPUNAME like '%" + TXT_FINDUPLINER.Text.Trim() +
                    "%' or SG_GRPUPLINER = '" + TXT_FINDUPLINER.Text.Trim() + "') ";
            }

            if (orderby != "")
                sqlCond += " ORDER BY " + orderby;

            return sqlCond;
        }

        private void BindData()
        {
            DatGrd.DataSource = conn.GetDataTable(Q_GROUP + " " + SQLCondition(), null, dbtimeout);
            try
            {
                DatGrd.DataBind();
            }
            catch
            {
                DatGrd.CurrentPageIndex = 0;
                DatGrd.DataBind();
            }

            LinkButton lnk;
            for (int i = 0; i < DatGrd.Items.Count; i++)
            {
                if (DatGrd.Items[i].Cells[6].Text == "0")	//sg_active		-- group deleted
                {
                    lnk = (LinkButton)DatGrd.Items[i].Cells[7].FindControl("lnk_menu");
                    lnk.Visible = false;
                    lnk = (LinkButton)DatGrd.Items[i].Cells[7].FindControl("lnk_delete");
                    lnk.Visible = false;
                    lnk = (LinkButton)DatGrd.Items[i].Cells[7].FindControl("lnk_edit");
                    lnk.Visible = false;
                    DatGrd.Items[i].Cells[0].ForeColor = Color.Gray;
                    DatGrd.Items[i].Cells[1].ForeColor = Color.Gray;
                    DatGrd.Items[i].Cells[2].ForeColor = Color.Gray;
                    DatGrd.Items[i].Cells[3].ForeColor = Color.Gray;
                    DatGrd.Items[i].Cells[4].ForeColor = Color.Gray;
                    DatGrd.Items[i].Cells[5].ForeColor = Color.Gray;
                }
            }
        }
        #endregion

        #region Property
        private string MaintainedModuleIDs
        {
            get
            {
                string ret = (string)ViewState["MaintainedModuleIDs"];
                if ((ret == null) || (ret == ""))
                {
                    char[] sep = new char[2] { ',', ';' };
                    string[] modids = ConfigurationSettings.AppSettings["MaintainedModuleIDs"].Split(sep);
                    ret = "'" + modids[0] + "'";
                    for (int i = 1; i < modids.Length; i++)
                        ret += ", '" + modids[i] + "'";
                    ViewState["MaintainedModuleIDs"] = ret;
                }
                return ret;
            }
        }
        #endregion

        protected void BTN_SEARCH_Click(object sender, EventArgs e)
        {
            using (conn = new DbConnection(Session["ConnStringLogin"].ToString()))
            {
                orderby = "";
                ViewState["orderby"] = orderby;
                DatGrd.CurrentPageIndex = 0;
                BindData();
            }
        }

        protected void BTN_CLEARSEARCH_Click(object sender, EventArgs e)
        {

        }

        protected void BTN_NEW_Click(object sender, EventArgs e)
        {
            BTN_SAVE.Visible = true;
            BTN_CANCEL.Visible = true;
            BTN_NEW.Visible = false;
            EnableFields(true);

            LBL_SAVEMODE.Text = "1";

            MyPage.SetFocus(this, TXT_GROUPID);
        }

        protected void BTN_SAVE_Click(object sender, EventArgs e)
        {
            string moduleID = "";

            for (int i = 0; i < CHK_MODULEID.Items.Count; i++)
            {
                if (CHK_MODULEID.Items[i].Selected == true)
                    moduleID = moduleID + CHK_MODULEID.Items[i].Value + ";";
            }

            conn = new DbConnection(Session["ConnStringLogin"].ToString());
            if (TXT_GROUPID.Text == String.Empty)
            {
                LBL_RESULT.Text = "Group ID is required... ";
                LBL_RESULT.ForeColor = System.Drawing.Color.Red;
            }
            else if (TXT_SG_GRPNAME.Text == String.Empty)
            {
                LBL_RESULT.Text = "Group Name is required... ";
                LBL_RESULT.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                try
                {
                    string tmpApprovalGroup = "0";
                    string saveCalculator = "0";
                    string flagSupervisor = "0";
                    if (CHK_SG_APPRSTA.Checked) tmpApprovalGroup = "1";
                    if (CHK_SG_CALCULATOR.Checked) saveCalculator = "1";
                    if (FLAG_SUPERVISOR.Checked) flagSupervisor = "1";
                    object[] pardata = new object[12] {TXT_GROUPID.Text, TXT_SG_GRPNAME.Text, DDL_SG_GRPUPLINER.SelectedValue,
                                                     moduleID, LBL_SAVEMODE.Text, Session["UserID"],null,tmpApprovalGroup, MEMBEROF_AD.Text,
                                                     saveCalculator, flagSupervisor, SG_ROLEDESC.Text };
                    conn.ExecuteNonQuery(SP_SAVE, pardata, dbtimeout);

                    ClearEntries();

                    LBL_RESULT.Text = "Request Submitted! Awaiting Approval ... ";
                    LBL_RESULT.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {
                    ClearEntries();
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        LBL_RESULT.Text = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        LBL_RESULT.Text = ex.Message;
                    LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                }

                conn.Dispose();
            }
        }

        protected void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            ClearEntries();
        }

        protected void DatGrd_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            conn = new DbConnection(Session["ConnStringLogin"].ToString());
            ClearEntries();
            switch (e.CommandName)
            {
                case "menuAccess":
                    Response.Write("<script language='javascript'>window.open('GroupMenuAccess.aspx?GroupID=" + e.Item.Cells[0].Text + "&ModuleID=61','MenuAccess','status=no,scrollbars=yes,width=500,height=400');</script>");
                    break;
                case "edit":
                    EnableFields(true);
                    BTN_NEW.Visible = false;
                    BTN_CANCEL.Visible = true;
                    BTN_SAVE.Visible = true;

                    LBL_SAVEMODE.Text = "0";

                    TXT_GROUPID.ReadOnly = true;
                    TXT_GROUPID.Text = e.Item.Cells[0].Text;
                    TXT_SG_GRPNAME.Text = e.Item.Cells[1].Text;
                    
                    
                    CHK_SG_APPRSTA.Checked = false;
                    CHK_SG_CALCULATOR.Checked = false;
                    FLAG_SUPERVISOR.Checked = false;
                    if (e.Item.Cells[3].Text == "True")
                        CHK_SG_APPRSTA.Checked = true;
                    if (e.Item.Cells[4].Text == "True")
                        CHK_SG_CALCULATOR.Checked = true;
                    if (e.Item.Cells[5].Text == "True")
                        FLAG_SUPERVISOR.Checked = true;

                    conn.ExecReader("select moduleid from VW_GRPACCESSMODULE where groupid = '" + TXT_GROUPID.Text + "'", null, dbtimeout);
                    while (conn.hasRow())
                        try
                        {
                            CHK_MODULEID.Items.FindByValue(conn.GetFieldValue("moduleid")).Selected = true;
                        }
                        catch { }
                    //MEMBEROF_AD.Text = conn.GetFieldValue("memberof_ad").ToString();
                    SG_ROLEDESC.Text = e.Item.Cells[2].Text;
                    try { DDL_SG_GRPUPLINER.SelectedValue = e.Item.Cells[8].Text; } catch { }
                    try { CHK_MODULEID_SelectedIndexChanged(null, null); }
                    catch { }
                    MyPage.SetFocus(this, BTN_CANCEL);
                    break;
                case "delete":
                    object[] pardel = new object[5] { e.Item.Cells[0].Text, e.Item.Cells[1].Text, "2", "1", Session["UserID"] };
                    try
                    {
                        conn.ExecuteNonQuery(SP_DELETE, pardel, dbtimeout);
                        LBL_RESULT.Text = "Request Submitted! Awaiting Approval ... ";
                        LBL_RESULT.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        ClearEntries();
                        if (ex.Message.IndexOf("Last Query:") > 0)
                            LBL_RESULT.Text = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                        else
                            LBL_RESULT.Text = ex.Message;
                        LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                    }
                    break;
            }
            conn.Dispose();
        }

        protected void DatGrd_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DatGrd.CurrentPageIndex = e.NewPageIndex;
            conn = new DbConnection(Session["ConnStringLogin"].ToString());
            BindData();
            conn.Dispose();
        }

        protected void DatGrd_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            orderby = e.SortExpression;
            ViewState["orderby"] = orderby;
            conn = new DbConnection(Session["ConnStringLogin"].ToString());
            BindData();
            conn.Dispose();
        }

        protected void CHK_MODULEID_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (conn = new DbConnection(Session["ConnStringLogin"].ToString()))
            {
                string cond = " and moduleid in (''";
                for (int i = 0; i < CHK_MODULEID.Items.Count; i++)
                {
                    if (CHK_MODULEID.Items[i].Selected)
                    {
                        cond += ", '" + CHK_MODULEID.Items[i].Value + "'";
                    }
                }
                cond += ")";
                MyPage.fillRefList(RBL_MODULE.Items, Q_RFMODULE + cond, conn);
                RBL_MODULE.Items.RemoveAt(0);
                try { RBL_MODULE.SelectedIndex = 0; RBL_MODULE_SelectedIndexChanged(null, null); }
                catch { }
            }
            MyPage.SetFocus(this, BTN_SAVE);
        }

        protected void RBL_MODULE_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (conn = new DbConnection(Session["ConnStringLogin"].ToString()))
            {
                //InsertModuleUser(TXT_USERID.Text,RBL_MODULE.SelectedValue);

                object[] par = new object[] { RBL_MODULE.SelectedValue };
                conn.ExecReader(Q_MODULE, par, dbtimeout);
                IFR_MODULE.Attributes.Remove("src");
                if (conn.hasRow() && conn.GetFieldValue("grpmntpage").Trim() != "")
                {
                    string src = conn.GetFieldValue("grpmntpage");
                    if (src.IndexOf("?") > 0)
                        src += "&";
                    else
                        src += "?";
                    src += "moduleid=" + RBL_MODULE.SelectedValue + "&moddesc=" + RBL_MODULE.SelectedItem.Text + "&gid=" + TXT_GROUPID.Text;
                    IFR_MODULE.Attributes.Add("src", src);
                }
            }
            MyPage.SetFocus(this, BTN_SAVE);
        }
    }
}
