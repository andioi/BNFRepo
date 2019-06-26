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
using System.DirectoryServices;
using UOBISecurity;
using System.Text.RegularExpressions;

namespace MikroMnt.user
{
    public partial class UserMaintenance : System.Web.UI.Page
    {
        private DbConnection conn;
        private int dbtimeout;
        private string orderby, ConnString;

        #region static vars
        private static string Q_USERDATA = "SELECT USERID, SU_FULLNAME, GROUPID, SG_GRPNAME, SU_HPNUM, SU_EMAIL, " +
            "SU_LOGON, SU_REVOKE, SU_ACTIVE, SU_UPLINER, BRANCHID, AREAID, JenisUser, JenisUserDesc, SU_UPLINER2, SU_UPLINER3, SU_UPLINER4, SU_UPLINER5 FROM VW_SCALLUSER WHERE USERID = @1 ";
        private static string Q_MODULEUSER = "select userid, su_fullname, groupid, sg_grpname, su_logon, su_revoke, " +
            "modulename, moduleid, su_active, inpending, branchid, branchname, areaid, areaname, JenisUser, JenisUserDesc, SU_UPLINER2, SU_UPLINER3, SU_UPLINER4, SU_UPLINER5 from vw_moduleuser_branched ";
        private static string Q_RFMODULE = "select moduleid, modulename from rfmodule where active = '1' ";
        private static string Q_RFBRANCH = "select * from vw_rfbranch ";
        private static string Q_RFAREA = "select * from vw_rfarea ";
        private static string Q_LOGINPARAM = "select minpwdlength, maxpwdlength, def_pwd from loginparam ";
        private static string Q_MODULEGROUP = "select groupid, sg_grpname, moduleid, modulename, approval_group, usermntpage from vw_grpaccessmodule ";
        private static string Q_MODULEGROUPDDL = "select distinct groupid, sg_grpname from vw_grpaccessmodule ";
        private static string Q_UPLINER = "exec USP_USER_GETUPLINER ";
        private static string Q_SCCREDIT = "SELECT SC.SC_ID, SC.PRODUCTID, RP.PRODUCTDESC, SC.SC_SINGLE, " +
            "SC.SC_DOUBLE, SC.SC_DEV, NULL SC_LSO, SC.PROGRAMID, PR.PROGRAMDESC " +
            "FROM SCCREDIT SC LEFT JOIN RFPRODUCT RP ON SC.PRODUCTID=RP.PRODUCTID " +
            "LEFT JOIN RFPROGRAM PR ON PR.PROGRAMID = SC.PROGRAMID WHERE SC_ID = @1 ";
        private static string Q_MODULE_USER_ELOSME = "SELECT SU.SC_ID, SU.BRANCHID, RB.BRANCHNAME, SU.SC_UPLINER " +
            "FROM SCUSER SU LEFT JOIN RFBRANCH RB ON SU.BRANCHID = RB.BRANCHID " +
            "WHERE SU.SC_ID = @1 ";
        private static string Q_CEKUSER = "select (select 1 from scalluser where userid = @1) as existing, " +
            "(select 2 from pending_scalluser where userid = @1) as pending";

        private static string U_RESETLOGON = "UPDATE SCALLUSERFLAG SET SU_LOGON = 0 WHERE USERID = @1 ";
        private static string SP_DELETE = "exec SU_SCALLUSER_DELETE @1, @2, @3, @4, @5 ";
        private static string SP_UNDELETE = "exec SU_SCALLUSER_UNDELETE @1, @2, @3, @4, @5 ";
        private static string SP_USP_APPROVAL_LIMIT = "EXEC USP_APPROVAL_LIMIT @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12";
        private static string SP_INITPENDINGMODULEUSER = "EXEC SU_INITPENDINGMODULEUSER @1, @2 ";

        private static string SP_DELPENDINGMODULEUSER = "EXEC SU_DELPENDINGMODULEUSER @1, @2 ";

        private static string SP_USP_DELETE_PENDING = "EXEC USP_DELETE_PENDING @1, @2";
        private static string SP_SAVE = "exec SU_SCALLUSER_SAVE @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            ConnString = Session["ConnStringLogin"].ToString();
            if (!IsPostBack)
            {
                string user = (string)Session["UserID"];
                conn = new DbConnection(ConnString);

                uREF_BRANCH.fillRefList(Q_RFBRANCH, true);
                uREF_BRANCHID.fillRefList(Q_RFBRANCH, true);
                uREF_AREAID.fillRefList(Q_RFAREA, true);
                
                MyPage.fillRefList(DDL_RFMODULE, Q_RFMODULE + " and moduleid in (" + MaintainedModuleIDs + ")", null, dbtimeout, false, conn);
                ArrayList arrddl = new ArrayList();
                arrddl.Add(DDL_RFGROUP);
                arrddl.Add(DDL_GROUPID);
                string cond = " where moduleid in (" + MaintainedModuleIDs + ") ";
                MyPage.fillRefList(arrddl, Q_MODULEGROUPDDL + cond, null, dbtimeout, false, conn);

                orderby = "";
                ViewState["orderby"] = orderby;

                // dummy query to show the grid
                DatGrd.DataSource = conn.GetDataTable(Q_MODULEUSER + " WHERE 1 = 2 ", null, dbtimeout);
                try
                {
                    DatGrd.DataBind();
                }
                catch { }

                ClearEntries();
                conn.Dispose();
            }
            else
            {
                orderby = (string)ViewState["orderby"];
            }

            BTN_SAVE.Attributes.Add("onclick", "if(!cek_mandatory(document.form1)){return false;} else {simpan();};");
            //BTN_SAVE.Attributes.Add("onclick", "simpan();");
        }

        #region General Function
        private void SetEnable(bool mode)
        {
            TXT_USERID.Enabled = mode;
            TXT_SU_FULLNAME.Enabled = mode;
            TXT_SU_PWD.Enabled = mode;
            TXT_SU_HPNUM.Enabled = mode;
            TXT_SU_EMAIL.Enabled = mode;
            DDL_GROUPID.Enabled = mode;
            cb_revoke.Enabled = mode;
            uREF_BRANCHID.Enabled = mode;
            uREF_AREAID.Enabled = mode;
            cb_logon.Enabled = mode;
            cb_resetpwd.Enabled = mode;
            CHK_SU_ACTIVE.Enabled = mode;
            uREF_UPLINER.Enabled = mode;
            uREF_UPLINER2.Enabled = mode;
            uREF_UPLINER3.Enabled = mode;
            uREF_UPLINER4.Enabled = mode;
            uREF_UPLINER5.Enabled = mode;
            ddl_JenisUser.Enabled = mode;
        }

        private void ClearEntries()
        {
            TXT_USERID.Text = "";
            TXT_SU_FULLNAME.Text = "";
            TXT_SU_PWD.Text = "";
            TXT_SU_HPNUM.Text = "";
            TXT_SU_EMAIL.Text = "";
            lbl_jenisuser.Text = "";

            DDL_GROUPID.ClearSelection();
            uREF_UPLINER.ClearSelection();
            uREF_UPLINER2.ClearSelection();
            uREF_UPLINER3.ClearSelection();
            uREF_UPLINER4.ClearSelection();
            uREF_UPLINER5.ClearSelection();
            uREF_BRANCHID.ClearSelection();
            uREF_AREAID.ClearSelection();

            cb_revoke.Text = "(check for yes)";
            cb_revoke.Checked = false;
            cb_logon.Checked = false;
            cb_resetpwd.Checked = false;
            CHK_SU_ACTIVE.Checked = true;

            LBL_SAVEMODE.Text = "1";

            BTN_NEW_AD.Visible = true;
            BTN_NEW.Visible = true;
            BTN_SAVE.Visible = false;
            BTN_CANCEL.Visible = false;

            RBL_MODULE.Items.Clear();
            IFR_MODULE.Attributes.Remove("src");

            SetEnable(false);
            SetADMode(false);
        }

        private void ClearSearch()
        {
            try
            {
                DDL_RFMODULE.ClearSelection();
            }
            catch { }
            try
            {
                DDL_RFGROUP.ClearSelection();
            }
            catch { }
            try
            {
                uREF_BRANCH.ClearSelection();
            }
            catch { }
            TXT_SEARCH_USERID.Text = "";
            TXT_SEARCH_USERNAME.Text = "";
            TXT_SEARCH_UPLINER.Text = "";
            TXT_SEARCH_UPLINER2.Text = "";
            TXT_SEARCH_UPLINER3.Text = "";
        }

        private void SetADMode(bool mode)
        {
            if (mode == true)
            {
                TXT_SU_FULLNAME.ReadOnly = false;
                TXT_SU_FULLNAME.CssClass = "mandatory";

                //TXT_SU_FULLNAME.ReadOnly = true;
                //TXT_SU_FULLNAME.CssClass = "";

                TXT_SU_PWD.Enabled = false;
                TXT_SU_PWD.CssClass = "";
                TXT_VERIFYPWD.Enabled = false;
                TXT_VERIFYPWD.CssClass = "";
                //TXT_SU_EMAIL.ReadOnly = true;
                cb_resetpwd.Enabled = false;
                ddl_JenisUser.SelectedValue = "1";
                lbl_jenisuser.Text = "Active Directory";
                btn_cekAD.Visible = true;
            }
            else
            {
                TXT_SU_FULLNAME.ReadOnly = false;
                TXT_SU_FULLNAME.CssClass = "mandatory";
                TXT_SU_PWD.Enabled = true;
                TXT_SU_PWD.CssClass = "mandatory";
                TXT_VERIFYPWD.Enabled = true;
                TXT_VERIFYPWD.CssClass = "mandatory";
                //TXT_SU_EMAIL.ReadOnly = false;
                ddl_JenisUser.SelectedValue = "2";
                cb_resetpwd.Enabled = true;
                lbl_jenisuser.Text = "User Local";
                btn_cekAD.Visible = false;
            }
        }
        #endregion

        #region DataGrid
        #region binding
        private string SQLCondition()
        {
            string sqlCond = "";
            if (DDL_RFMODULE.SelectedValue != "")
            {
                sqlCond = " WHERE MODULEID = '" + DDL_RFMODULE.SelectedValue + "' ";
            }
            else
            {
                sqlCond = " WHERE MODULEID in (" + MaintainedModuleIDs + ") ";
            }

            if (DDL_RFGROUP.SelectedValue != "")
            {
                sqlCond += " AND GROUPID = '" + DDL_RFGROUP.SelectedValue + "' ";
            }

            if (uREF_BRANCH.SelectedValue != "")
            {
                sqlCond += " AND BRANCHID = '" + uREF_BRANCH.SelectedValue + "' ";
            }

            if (TXT_SEARCH_USERID.Text.Trim() != "")
            {
                sqlCond += " AND USERID like '%" + TXT_SEARCH_USERID.Text.Trim() + "%' ";
            }
            if (TXT_SEARCH_USERNAME.Text.Trim() != "")
            {
                sqlCond += " AND SU_FULLNAME like '%" + TXT_SEARCH_USERNAME.Text.Trim() + "%' ";
            }

            if (TXT_SEARCH_UPLINER.Text.Trim() != "")
            {
                sqlCond += " AND ISNULL(SU_UPLINER,'') + ' ' + ISNULL(SU_UNAME,'') like '%" + TXT_SEARCH_UPLINER.Text.Trim() + "%' ";
            }

            if (TXT_SEARCH_UPLINER2.Text.Trim() != "")
            {
                sqlCond += " AND ISNULL(SU_UPLINER2,'') + ' ' + ISNULL(SU_UNAME,'') like '%" + TXT_SEARCH_UPLINER2.Text.Trim() + "%' ";
            }

            if (TXT_SEARCH_UPLINER3.Text.Trim() != "")
            {
                sqlCond += " AND ISNULL(SU_UPLINER3,'') + ' ' + ISNULL(SU_UNAME,'') like '%" + TXT_SEARCH_UPLINER3.Text.Trim() + "%' ";
            }

            if (orderby != "")
                sqlCond += " ORDER BY " + orderby;

            return sqlCond;
        }

        private void BindData()
        {
            DatGrd.DataSource = conn.GetDataTable(Q_MODULEUSER + " " + SQLCondition(), null, dbtimeout);
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
                if (DatGrd.Items[i].Cells[10].Text == "0")	//su_active		-- user deleted
                {
                    lnk = (LinkButton)DatGrd.Items[i].Cells[11].FindControl("lnkDelete");
                    lnk.Visible = false;
                    lnk = (LinkButton)DatGrd.Items[i].Cells[11].FindControl("lnkEdit");
                    lnk.Visible = false;
                    DatGrd.Items[i].Cells[2].ForeColor = Color.Gray;		//userid
                    DatGrd.Items[i].Cells[3].ForeColor = Color.Gray;		//fullname
                }
                else if (DatGrd.Items[i].Cells[12].Text == "1")
                {
                    lnk = (LinkButton)DatGrd.Items[i].Cells[11].FindControl("lnkEdit");
                    lnk.Visible = false;
                    lnk = (LinkButton)DatGrd.Items[i].Cells[11].FindControl("lnkDelete");
                    lnk.Visible = false;
                    lnk = (LinkButton)DatGrd.Items[i].Cells[11].FindControl("lnkUndelete");
                    lnk.Visible = false;
                    DatGrd.Items[i].Cells[11].Text = "Waiting Approval!";
                }
                else
                {
                    lnk = (LinkButton)DatGrd.Items[i].Cells[11].FindControl("lnkUndelete");
                    lnk.Visible = false;
                }
            }
        }

        #endregion
        #endregion

        #region TableModule
        private void InitializeModule()
        {
            InitializeModule(false);
        }

        private void InitializeModule(bool init)
        {
            try
            {
                RBL_MODULE.Items.Clear();

                string cond = " where groupid = '" + DDL_GROUPID.SelectedValue +
                    "' and moduleid in (" + MaintainedModuleIDs + ") ";
                conn.ExecReader(Q_MODULEGROUP + cond, null, dbtimeout);
                int i = 0;
                while (conn.hasRow())
                {
                    string moduleid = conn.GetFieldValue("moduleid").Trim();
                    string src = conn.GetFieldValue("usermntpage").Trim();
                    RBL_MODULE.Items.Add(new ListItem(conn.GetFieldValue("modulename"), moduleid));
                    if (i == 0 && src != "")
                    {
                        if (src.IndexOf("?") > 0)
                            src += "&";
                        else
                            src += "?";
                        src += "moduleid=" + moduleid + "&moddesc=" + RBL_MODULE.Items[0].Text + "&grpid=" + DDL_GROUPID.SelectedValue + "&uid=" + TXT_USERID.Text + "&spv=" + uREF_UPLINER.SelectedValue;
                        IFR_MODULE.Attributes.Remove("src");
                        IFR_MODULE.Attributes.Add("src", src);
                        RBL_MODULE.SelectedIndex = 0;
                        i++;
                    }
                    if (init)
                    {
                        try
                        {
                            using (DbConnection modconn = new DbConnection(MNTTools.GetConnString(moduleid)))
                            {
                                object[] pardata = new object[2] { TXT_USERID.Text, moduleid };
                                modconn.ExecuteNonQuery(SP_INITPENDINGMODULEUSER, pardata, dbtimeout);
                            }
                        }
                        catch (Exception ex1)
                        {
                            Response.Write("<!-- " + ex1.Message.Replace("-->", "--)") + " -->\n");
                            MNTTools.LogError(this, (string)Session["UserID"], ex1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message.Replace("-->", "--)") + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
            }
        }

        #endregion

        #region Database Transaction

        private void DeletePendingUser(string userid)
        {
            try
            {
                object[] pardata = new object[2] { userid, "" };
                conn.ExecuteNonQuery(SP_USP_DELETE_PENDING, pardata, dbtimeout);
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message.Replace("-->", "--)") + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
            }
        }
        #endregion

        private void FillUpliner(string groupid, string branchid)
        {
            string qupliner = Q_UPLINER + " '" + groupid + "', '" + branchid + "'";
            conn.ExecReader(qupliner, null, dbtimeout);
            if (conn.hasRow())
            {
                uREF_UPLINER.fillRefList(qupliner, true);
                uREF_UPLINER2.fillRefList(qupliner, true);
                uREF_UPLINER3.fillRefList(qupliner, true);
                uREF_UPLINER4.fillRefList(qupliner, true);
                uREF_UPLINER5.fillRefList(qupliner, true);
            }
            else
            {
                uREF_UPLINER.Enabled = false;
                uREF_UPLINER2.Enabled = false;
                uREF_UPLINER3.Enabled = false;
                uREF_UPLINER4.Enabled = false;
                uREF_UPLINER5.Enabled = false;
            }
        }

        #region Property
        private string MaintainedModuleIDs
        {
            get
            {
                string ret;
                char[] sep = new char[2] { ',', ';' };
                string[] modids = ConfigurationSettings.AppSettings["MaintainedModuleIDs"].Split(sep);
                ret = "'" + modids[0] + "'";
                for (int i = 1; i < modids.Length; i++)
                    ret += ", '" + modids[i] + "'";
                return ret;
            }
        }
        #endregion

        protected void cb_logon_CheckedChanged(object sender, System.EventArgs e)
        {
            object[] paruser = new object[1] { TXT_USERID.Text };
            using (conn = new DbConnection(ConnString))
            {
                conn.ExecuteNonQuery(U_RESETLOGON, paruser, dbtimeout);
                BindData();
            }
            MyPage.popMessage(this, "Logon flag resetted!");
        }

        protected void BTN_SEARCH_Click(object sender, System.EventArgs e)
        {
            using (conn = new DbConnection(ConnString))
            {
                orderby = "";
                ViewState["orderby"] = orderby;
                DatGrd.CurrentPageIndex = 0;
                BindData();
                ClearEntries();
                //ClearSearch();
            }
        }

        protected void BTN_CLEAR_Click(object sender, System.EventArgs e)
        {
            ClearSearch();
        }

        protected void BTN_NEW_Click(object sender, System.EventArgs e)
        {
            BTN_NEW.Visible = false;
            BTN_SAVE.Visible = true;
            BTN_CANCEL.Visible = true;
            BTN_NEW_AD.Visible = false;
            CHK_SU_ACTIVE.Checked = true;

            SetEnable(true);
            SetADMode(false);
            TXT_USERID.ReadOnly = false;
            using (conn = new DbConnection(ConnString))
            {
                FillUpliner(DDL_GROUPID.SelectedValue, uREF_BRANCHID.SelectedValue);
            }

            pwdmsg.Value = "Leave password blank to use default password!";
            MyPage.SetFocus(this, BTN_CANCEL);
        }

        protected void BTN_SAVE_Click(object sender, System.EventArgs e)
        {
            if (TXT_USERID.Text == string.Empty || TXT_SU_FULLNAME.Text == string.Empty || DDL_GROUPID.SelectedValue == string.Empty)
            {
                LBL_RESULT.Text = "Mandatory field cannot blank... ";
                LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (TXT_SU_PWD.Text.Trim() != TXT_VERIFYPWD.Text.Trim())
            {
                LBL_RESULT.Text = "Password mismatch... ";
                LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (TXT_SU_EMAIL.Text != string.Empty && emailValidation(TXT_SU_EMAIL.Text)==false)
            {
                LBL_RESULT.Text = "Invalid email address... ";
                LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                return;
            }
            using (conn = new DbConnection(ConnString))
            {
                //Validasi
                if (LBL_SAVEMODE.Text == "1")			//insert new user
                {
                    object[] par = new object[1] { TXT_USERID.Text };
                    conn.ExecReader(Q_CEKUSER, par, dbtimeout);
                    if (conn.hasRow())
                    {
                        if (conn.GetFieldValue(0) == "1")
                        {
                            LBL_RESULT.Text = "UserID exists in existing system.";
                            LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                        else if (conn.GetFieldValue(1) == "2")
                        {
                            LBL_RESULT.Text = "UserID is in the pending list.";
                            LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }

                string password = "";
                conn.ExecReader(Q_LOGINPARAM, null, dbtimeout);
                if (conn.hasRow())
                {
                    if ((TXT_SU_PWD.Text.Trim().Length < (int)conn.GetNativeFieldValue(0)
                        && TXT_SU_PWD.Text.Trim().Length > (int)conn.GetNativeFieldValue(1)))
                    {
                        LBL_RESULT.Text = "Password must be between " + conn.GetFieldValue(0) + " and " + conn.GetFieldValue(1) + " characters!";
                        LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                }
                bool isnew = LBL_SAVEMODE.Text == "1";
                string revoke = "0", suActive = "0";
                if (cb_revoke.Checked)
                    revoke = "1";
                if (CHK_SU_ACTIVE.Checked)
                    suActive = "1";

                if (cb_resetpwd.Checked)
                    password = conn.GetFieldValue("def_pwd");
                else
                    password = TXT_SU_PWD.Text.Trim();

                if (isnew && password == "" & ddl_JenisUser.SelectedValue=="2")
                    password = conn.GetFieldValue("def_pwd");

                if (password != "")
                {	// blank means using old pwd
                    //password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                    Encryption.SimpleEncryption enc = new Encryption.SimpleEncryption();
                    password = enc.Encrypt(password, true);
                }

                try
                {
                    object[] pardata = new object[19] {TXT_USERID.Text, DDL_GROUPID.SelectedValue,
													TXT_SU_FULLNAME.Text, password, 
													TXT_SU_HPNUM.Text, TXT_SU_EMAIL.Text, 
													DBNull.Value, Session["UserID"], LBL_SAVEMODE.Text, 
													revoke, suActive, uREF_UPLINER.SelectedValue, uREF_BRANCHID.SelectedValue, 
                                                    uREF_AREAID.SelectedValue, ddl_JenisUser.SelectedValue, 
                                                    uREF_UPLINER2.SelectedValue, uREF_UPLINER3.SelectedValue,
                                                    uREF_UPLINER4.SelectedValue, uREF_UPLINER5.SelectedValue};
                    conn.ExecuteNonQuery(SP_SAVE, pardata, dbtimeout);

                    LBL_RESULT.Text = "Request Submitted! Awaiting Approval ... ";
                    LBL_RESULT.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        LBL_RESULT.Text = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        LBL_RESULT.Text = ex.Message;
                    LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                            
                DatGrd.CurrentPageIndex = 0;
                BindData();
                ClearEntries();
                ClearSearch();
            }
        }

        protected void BTN_CANCEL_Click(object sender, System.EventArgs e)
        {
            using (conn = new DbConnection(ConnString))
            {
                DeletePendingUser(TXT_USERID.Text);
                BindData();
                ClearEntries();
                ClearSearch();
            }
        }

        protected void DDL_GROUPID_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (TXT_USERID.Text.Trim() == "")
            {
                MyPage.popMessage(this, "Please enter UserID First.");
                DDL_GROUPID.SelectedIndex = 0;
                return;
            }
            using (conn = new DbConnection(ConnString))
            {
                if (LBL_SAVEMODE.Text == "1")			//insert new user
                {
                    object[] par = new object[1] { TXT_USERID.Text };
                    conn.ExecReader(Q_CEKUSER, par, dbtimeout);
                    if (conn.hasRow())
                    {
                        if (conn.GetFieldValue(0) == "1")
                        {
                            MyPage.popMessage(this, "UserID exists in existing system.");
                            DDL_GROUPID.SelectedIndex = 0;
                            return;
                        }
                        else if (conn.GetFieldValue(1) == "2")
                        {
                            MyPage.popMessage(this, "UserID is in the pending list.");
                            DDL_GROUPID.SelectedIndex = 0;
                            return;
                        }
                    }
                }
                TXT_USERID.ReadOnly = true;
                TXT_SU_FULLNAME.Text = hdn_nama.Value;
                TXT_SU_EMAIL.Text = hdn_email.Value;
                InitializeModule();
                FillUpliner(DDL_GROUPID.SelectedValue, uREF_BRANCHID.SelectedValue);

                MyPage.SetFocus(this, BTN_CANCEL);
            }
        }

        protected void RBL_MODULE_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            using (conn = new DbConnection(ConnString))
            {
                //InsertModuleUser(TXT_USERID.Text,RBL_MODULE.SelectedValue);

                string cond = " where groupid = '" + DDL_GROUPID.SelectedValue +
                    "' and moduleid = '" + RBL_MODULE.SelectedValue + "' ";
                conn.ExecReader(Q_MODULEGROUP + cond, null, dbtimeout);
                IFR_MODULE.Attributes.Remove("src");
                if (conn.hasRow() && conn.GetFieldValue("usermntpage").Trim() != "")
                {
                    string src = conn.GetFieldValue("usermntpage");
                    if (src.IndexOf("?") > 0)
                        src += "&";
                    else
                        src += "?";
                    src += "moduleid=" + RBL_MODULE.SelectedValue + "&moddesc=" + RBL_MODULE.SelectedItem.Text + "&grpid=" + DDL_GROUPID.SelectedValue + "&uid=" + TXT_USERID.Text + "&spv=" + uREF_UPLINER.SelectedValue;
                    IFR_MODULE.Attributes.Add("src", src);
                }
            }
            MyPage.SetFocus(this, BTN_SAVE);
        }

        protected void DatGrd_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            orderby = e.SortExpression;
            ViewState["orderby"] = orderby;
            conn = new DbConnection(ConnString);
            BindData();
            ClearEntries();
            ClearSearch();
            conn.Dispose();
        }

        protected void DatGrd_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DatGrd.CurrentPageIndex = e.NewPageIndex;
            conn = new DbConnection(ConnString);
            BindData();
            ClearEntries();
            //ClearSearch();
            conn.Dispose();
        }

        protected void DatGrd_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ClearEntries();
            conn = new DbConnection(ConnString);
            switch (e.CommandName)
            {
                case "edit":
                    BTN_NEW.Visible = false;
                    BTN_NEW_AD.Visible = false;
                    BTN_SAVE.Visible = true;
                    BTN_CANCEL.Visible = true;
                    LBL_SAVEMODE.Text = "0";
                    SetEnable(true);
                    TXT_USERID.ReadOnly = true;

                    object[] paruser = new object[1] { e.Item.Cells[2].Text };
                    conn.ExecReader(Q_USERDATA, paruser, dbtimeout);
                    if (conn.hasRow())
                    {
                        if (conn.GetFieldValue("SU_REVOKE") == "1")
                        {
                            cb_revoke.Checked = true;
                            cb_revoke.Text = "(clear to reset)";
                        }
                        else cb_revoke.Checked = false;

                        if (conn.GetFieldValue("SU_LOGON") == "1")
                            cb_logon.Checked = true;
                        else
                            cb_logon.Checked = false;

                        if (conn.GetFieldValue("SU_ACTIVE") == "1")
                            CHK_SU_ACTIVE.Checked = true;
                        else
                            CHK_SU_ACTIVE.Checked = false;

                        TXT_USERID.Text = conn.GetFieldValue("USERID");
                        TXT_SU_FULLNAME.Text = conn.GetFieldValue("SU_FULLNAME");
                        TXT_SU_HPNUM.Text = conn.GetFieldValue("SU_HPNUM");
                        TXT_SU_EMAIL.Text = conn.GetFieldValue("SU_EMAIL");
                        uREF_BRANCHID.SelectedValue = conn.GetFieldValue("BRANCHID");
                        uREF_AREAID.SelectedValue = conn.GetFieldValue("AREAID");
                        ddl_JenisUser.SelectedValue = conn.GetFieldValue("JenisUser");
                        if (conn.GetFieldValue("JenisUser") == "1")
                        {
                            //userAD tidak bisa reset password
                            SetADMode(true);
                            btn_cekAD.Visible = false;
                        }
                        else
                        {
                            SetADMode(false);
                        }
                        try
                        {
                            DDL_GROUPID.SelectedValue = conn.GetFieldValue("GROUPID");
                            string spv = conn.GetFieldValue("SU_UPLINER");
                            string spv2 = conn.GetFieldValue("SU_UPLINER2");
                            string spv3 = conn.GetFieldValue("SU_UPLINER3");
                            string spv4 = conn.GetFieldValue("SU_UPLINER4");
                            string spv5 = conn.GetFieldValue("SU_UPLINER5");
                            FillUpliner(DDL_GROUPID.SelectedValue, uREF_BRANCHID.SelectedValue);
                            try { uREF_UPLINER.SelectedValue = spv; } catch { }
                            try { uREF_UPLINER2.SelectedValue = spv2; } catch { }
                            try { uREF_UPLINER3.SelectedValue = spv3; } catch { }
                            try { uREF_UPLINER4.SelectedValue = spv4; } catch { }
                            try { uREF_UPLINER5.SelectedValue = spv5; } catch { }
                            InitializeModule(true);
                        }
                        catch (Exception ex)
                        {
                            MyPage.popMessage(this, "Error initializing group/module screen");
                            Response.Write("<!-- " + ex.Message.Replace("-->", "--)") + " -->\n");
                            MNTTools.LogError(this, (string)Session["UserID"], ex);
                        }
                    }

                    pwdmsg.Value = "Leave password blank to use old password!";
                    MyPage.SetFocus(this, BTN_CANCEL);

                    break;
                case "delete":
                    object[] pardel = new object[5]{e.Item.Cells[2].Text, e.Item.Cells[4].Text, 
													   "1", e.Item.Cells[3].Text, Session["USerID"]};
                    try
                    {
                        conn.ExecuteNonQuery(SP_DELETE, pardel, dbtimeout);
                        LBL_RESULT.Text = "Request Submitted! Awaiting Approval ... ";
                        LBL_RESULT.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("Last Query:") > 0)
                            LBL_RESULT.Text = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                        else
                            LBL_RESULT.Text = ex.Message;
                        LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                    }
                    break;
                case "undelete":
                    object[] parundel = new object[5]{e.Item.Cells[2].Text, e.Item.Cells[4].Text, 
														 "1", e.Item.Cells[3].Text, Session["UserID"]};
                    try
                    {
                        conn.ExecuteNonQuery(SP_UNDELETE, parundel, dbtimeout);
                        LBL_RESULT.Text = "Request Submitted! Awaiting Approval ... ";
                        LBL_RESULT.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("Last Query:") > 0)
                            LBL_RESULT.Text = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                        else
                            LBL_RESULT.Text = ex.Message;
                        LBL_RESULT.ForeColor = System.Drawing.Color.Red;
                    }
                    break;
            }
            BindData();
            conn.Dispose();
        }

        protected void BTN_NEW_AD_Click(object sender, System.EventArgs e)
        {
            BTN_NEW.Visible = false;
            BTN_NEW_AD.Visible = false;
            BTN_SAVE.Visible = true;
            BTN_CANCEL.Visible = true;
            CHK_SU_ACTIVE.Checked = true;

            SetEnable(true);
            SetADMode(true);
            TXT_USERID.ReadOnly = false;
            using (conn = new DbConnection(ConnString))
            {
                FillUpliner(DDL_GROUPID.SelectedValue, uREF_BRANCHID.SelectedValue);
            }
            pwdmsg.Value = "Leave password blank to use default password!";
            MyPage.SetFocus(this, BTN_CANCEL);
        }

        private bool CheckUserADExist(string samAccountName, string LDAPUser, string LDAPPwd)
        {
            string LDAPServer = ConfigurationSettings.AppSettings["LDAPServer"];
            //string LDAPUser = ConfigurationSettings.AppSettings["LDAPAdminUser"];
            //string LDAPPwd = ConfigurationSettings.AppSettings["LDAPAdminPwd"];

            //decrypt LDAP Password
            //Encryptor enc = new Encryptor();
            //ApplicationRegistryHandler reg = new ApplicationRegistryHandler();
            //string appName = ConfigurationSettings.AppSettings["appName"].ToString();
            //appName = appName.Replace(" ", "");
            //string uobikey = ConfigurationSettings.AppSettings["UOBIKey"].ToString();
            //string keyReg = reg.ReadFromRegistry("Software\\" + appName, "Key");

            //LDAPPwd = enc.Decrypt(LDAPPwd, keyReg, uobikey);

            DirectoryEntry root = new DirectoryEntry("LDAP://" + LDAPServer, LDAPUser, LDAPPwd, AuthenticationTypes.Secure);
            DirectorySearcher searcher = new DirectorySearcher(root);
            searcher.Filter = "(SAMAccountName=" + samAccountName + ")";
            SearchResult SResult = searcher.FindOne();
            if (SResult != null)
            {
                try
                {
                    TXT_SU_FULLNAME.Text = SResult.Properties["displayName"][0].ToString();
                    hdn_nama.Value = SResult.Properties["displayName"][0].ToString();
                }
                catch { }
                try
                {
                    TXT_SU_EMAIL.Text = SResult.Properties["mail"][0].ToString();
                    hdn_email.Value = SResult.Properties["mail"][0].ToString();
                }
                catch { }
                return true;
            }
            else
            {
                TXT_SU_FULLNAME.Text = "";
                hdn_nama.Value = "";
                TXT_SU_EMAIL.Text = "";
                hdn_email.Value = "";
                return false;
            }

            //TXT_SU_FULLNAME.Text = "ACHMAD AFANDI";
            //hdn_nama.Value = "ACHMAD AFANDI";
            //TXT_SU_EMAIL.Text = "achmad.afandi@yahoo.com";
            //hdn_email.Value = "achmad.afandi@yahoo.com";
            //return true;
        }

        protected bool emailValidation(string emailAddress)
        {
            bool result = true;
            if (emailAddress.Trim() != "")
            {
                string[] emailAddresses = emailAddress.Split(';');
                foreach (string email in emailAddresses)
                {
                    if (email.Trim() != "")
                    {
                        if (!Regex.IsMatch(email.Trim(), @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)\Z"))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }

    }
}
