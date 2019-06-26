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

namespace MikroMnt.user
{
    public partial class GroupApproval : System.Web.UI.Page
    {
        private DbConnection conn;
        private int dbtimeout;

        #region static vars
        private static string Q_PENDINGDATA = "select GROUPID, SG_GRPNAME, SG_GRPUPLINER, SG_GRPUNAME, " +
            "MODULEIDS, MODULENAMES, CH_STA, STATUS, REQUESTBY from VW_PENDING_SCALLGROUP WHERE REQUESTBY <> @1 ";
        private static string U_DELPENDING = "DELETE FROM PENDING_SCALLGROUP WHERE GROUPID = @1 ";
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
                    if (DG_REQUEST.Items[i].Cells[5].Text == "2")
                        MNTTools.RemoveGroup(DG_REQUEST.Items[i].Cells[0].Text, (string)Session["UserID"]);
                    else if (DG_REQUEST.Items[i].Cells[5].Text == "0" || DG_REQUEST.Items[i].Cells[5].Text == "1")
                    {
                        if (DG_REQUEST.Items[i].Cells[7].Text == Session["USERID"].ToString())
                            flag_sameuser = true;
                        else
                            MNTTools.ApproveGroup(DG_REQUEST.Items[i].Cells[0].Text, DG_REQUEST.Items[i].Cells[3].Text, (string)Session["UserID"]);
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
                if (result == true)
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
    }
}
