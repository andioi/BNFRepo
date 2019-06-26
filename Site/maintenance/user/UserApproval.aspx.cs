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
using My.Tools;
using My.Framework;

namespace MikroMnt.user
{
    public partial class UserApproval : System.Web.UI.Page
    {
        private DbConnection conn;
        private int dbtimeout;

        #region static vars
        private static string Q_PENDINGDATA = "SELECT USERID, GROUPID, SG_GRPNAME, SU_FULLNAME, SU_PWD, " +
            "SU_HPNUM, SU_EMAIL, SU_UPLINER, SU_NIP, SU_REGISTERDATE, SU_REGISTERBY, SU_APPROVEBY, " +
            "SU_PWDEXPDATE, SU_ACTIVE, SU_REVOKE, CH_STA, STATUS " +
            "FROM VW_PENDING_SCALLUSER WHERE SU_REGISTERBY <> @1";
        //private static string U_DELPENDING = "DELETE FROM PENDING_SCALLUSER WHERE USERID = @1; DELETE FROM PENDING_SCCREDIT WHERE SC_ID=@1 ";
        private static string U_DELPENDING = "DELETE FROM PENDING_SCALLUSER WHERE USERID = @1 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);

            if (!IsPostBack)
            {
                conn = new DbConnection(Session["ConnStringLogin"].ToString());
                BindData();
                conn.Dispose();
            }
        }

        private void BindData()
        {
            DG_REQUEST.DataSource = conn.GetDataTable(Q_PENDINGDATA, new object[] { Session["UserID"] }, dbtimeout);
            DG_REQUEST.DataBind();
        }

        private bool ProcessPending()
        {
            bool flag_sameuser = false;
            for (int i = 0; i < DG_REQUEST.Items.Count; i++)
            {
                RadioButton rdA = (RadioButton)DG_REQUEST.Items[i].FindControl("RDO_APPROVE"),
                    rdR = (RadioButton)DG_REQUEST.Items[i].FindControl("RDO_REJECT");
                
                if (rdA.Checked == true)
                {
                    if (DG_REQUEST.Items[i].Cells[5].Text == Session["USERID"].ToString())
                        flag_sameuser = true;
                    else
                    {
                        if (DG_REQUEST.Items[i].Cells[3].Text == "2")
                            MNTTools.RemoveUser(DG_REQUEST.Items[i].Cells[0].Text, DG_REQUEST.Items[i].Cells[9].Text, (string)Session["UserID"]);
                        if (DG_REQUEST.Items[i].Cells[3].Text == "3")
                            MNTTools.RestoreUser(DG_REQUEST.Items[i].Cells[0].Text, DG_REQUEST.Items[i].Cells[9].Text, (string)Session["UserID"]);
                        else if (DG_REQUEST.Items[i].Cells[3].Text == "0" || DG_REQUEST.Items[i].Cells[3].Text == "1")
                            MNTTools.ApproveUser(DG_REQUEST.Items[i].Cells[0].Text, DG_REQUEST.Items[i].Cells[9].Text, (string)Session["UserID"]);
                    }
                }
                else if (rdR.Checked == true)
                {
                    object[] parreject = new object[1] { DG_REQUEST.Items[i].Cells[0].Text };
                    conn.ExecuteNonQuery(U_DELPENDING, parreject, dbtimeout);
                }
            }
            return !flag_sameuser;
        }

        protected void BTN_SUBMIT_Click(object sender, EventArgs e)
        {
            conn = new DbConnection(Session["ConnStringLogin"].ToString());
            try
            {
                bool result = ProcessPending();
                if (result==true)
                    MyPage.popMessage(this, "Update Complete...");
                else
                    MyPage.popMessage(this, "Update failed, Checker maker must be done by different ID ...");
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
                MyPage.popMessage(this, "Update Failed...");
            }
            BindData();
            conn.Dispose();
        }

        protected void panel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            conn = new DbConnection(Session["ConnStringLogin"].ToString());
            if (e.Parameter.StartsWith("r:"))
            {
                string userid = e.Parameter.Substring(2);
                DataTable dt = conn.GetDataTable("select * from vw_scalluser where userid = @1", new object[] { userid }, dbtimeout);
                staticFramework.retrieve(dt, "USERID", bf_userid);
                staticFramework.retrieve(dt, "SU_FULLNAME", bf_username);
                staticFramework.retrieve(dt, "SG_GRPNAME", bf_group);
                staticFramework.retrieve(dt, "su_email", bf_email);
                staticFramework.retrieve(dt, "su_hpnum", bf_hp);
                staticFramework.retrieve(dt, "branchname", bf_cabang);
                staticFramework.retrieve(dt, "SU_UPLINER", bf_upliner1);
                staticFramework.retrieve(dt, "SU_UPLINER2", bf_upliner2);
                staticFramework.retrieve(dt, "SU_UPLINER3", bf_upliner3);
                staticFramework.retrieve(dt, "SU_UPLINER4", bf_upliner4);
                staticFramework.retrieve(dt, "SU_UPLINER5", bf_upliner5);
                staticFramework.retrieve(dt, "STATUS_AKTIF", bf_status);
                staticFramework.retrieve(dt, "LOCKED", bf_locked);

                DataTable dt2 = conn.GetDataTable("select * from vw_pending_scalluser where userid = @1", new object[] { userid }, dbtimeout);
                staticFramework.retrieve(dt2, "USERID", af_userid);
                staticFramework.retrieve(dt2, "SU_FULLNAME", af_username);
                staticFramework.retrieve(dt2, "SG_GRPNAME", af_group);
                staticFramework.retrieve(dt2, "su_email", af_email);
                staticFramework.retrieve(dt2, "su_hpnum", af_hp);
                staticFramework.retrieve(dt2, "branchname", af_cabang);
                staticFramework.retrieve(dt2, "SU_UPLINER", af_upliner1);
                staticFramework.retrieve(dt2, "SU_UPLINER2", af_upliner2);
                staticFramework.retrieve(dt2, "SU_UPLINER3", af_upliner3);
                staticFramework.retrieve(dt2, "SU_UPLINER4", af_upliner4);
                staticFramework.retrieve(dt2, "SU_UPLINER5", af_upliner5);
                staticFramework.retrieve(dt2, "STATUS_AKTIF", af_status);
                staticFramework.retrieve(dt2, "LOCKED", af_locked);
                staticFramework.retrieve(dt2, "STATUS", ch_type);
            }
            conn.Dispose();
        }
    }
}
