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

namespace MikroMnt
{
    public partial class Footer : System.Web.UI.Page
    {
        private int dbtimeout;

        #region static vars
        private static string Q_USERDATA = "select SU_FULLNAME, SG_GRPNAME from VW_SESSIONLOS where USERID = @1 ";
        private static string SP_MONACTIVITY = "exec SU_MONITORACTIVITY @1, @2 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            DbConnection conn = new DbConnection(Session["ConnStringLogin"].ToString());
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbtimeout"]);
            if (!IsPostBack)
            {
                object[] user = new object[1] { Session["UserID"] };
                conn.ExecReader(Q_USERDATA, user, dbtimeout);
                if (conn.hasRow())
                {
                    string fullname = conn.GetFieldValue(0),
                        groupname = conn.GetFieldValue(1);
                    Label2.Text = fullname;
                    Label1.Text = System.DateTime.Now.ToShortTimeString() + " - " + System.DateTime.Now.ToLongDateString();
                    if (groupname != "")
                        Label5.Text = "( " + groupname + " )";
                }

                //DbConnection connModule = new DbConnection(Session["ConnStringModule"].ToString());
                //connModule.ExecReader("select top 1 * from RFSECURITYPARAM", null, dbtimeout);
                //if (connModule.hasRow())
                //{
                //    timeout_warning.Value = connModule.GetFieldValue("timeout_warning");
                //    timeout_logoff.Value = connModule.GetFieldValue("timeout_logoff");
                //}

                timeout_warning.Value = "0";
                timeout_logoff.Value = "0";
            }
            try
            {
                object[] par = new object[2] { Session["UserID"], Request.UserHostAddress };
                conn.ExecReader(SP_MONACTIVITY, par, dbtimeout);
                // force logout if activity invalid
                if (conn.hasRow())
                    if (conn.GetFieldValue(0) == "0")
                        post_cnt.Value = "18";
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message.Replace("-->", "--)") + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
            }
            conn.Dispose();
        }
    }
}
