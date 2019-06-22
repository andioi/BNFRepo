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

namespace DebtChecking
{
    public partial class Footer : System.Web.UI.Page
    {
        private static string Q_MONITOR = "exec SU_MONITORACTIVITY @1, @2 ";
        protected void Page_Load(object sender, EventArgs e)
        {
            int dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            DbConnection connESecurity = new DbConnection(Session["ConnStringLogin"].ToString());
            if (!IsPostBack)
            {
                Label2.Text = (string)Session["FullName"];
                Label1.Text = System.DateTime.Now.ToShortTimeString() + " - " + System.DateTime.Now.ToLongDateString();
                Label5.Text = (string)Session["GroupName"];

                DbConnection connModule = new DbConnection(Session["ConnStringModule"].ToString());
                connModule.ExecReader("select top 1 * from RFSECURITYPARAM", null, dbtimeout);
                if (connModule.hasRow())
                {
                    timeout_warning.Value = connModule.GetFieldValue("timeout_warning");
                    timeout_logoff.Value = connModule.GetFieldValue("timeout_logoff");
                }

                //timeout_warning.Value = "0";
                //timeout_logoff.Value = "0";
            }
            try
            {
                object[] par = new object[] { Session["UserID"], Request.UserHostAddress };
                connESecurity.ExecReader(Q_MONITOR, par, dbtimeout);
                // force logout if activity invalid
                if (!connESecurity.hasRow() || connESecurity.GetFieldValue(0) == "0")
                {
                    Response.Write("<script for=window event=onload language='JavaScript'>window.parent.execScript('logout_now()');</script>");
                }
            }
            catch { }
        }
    }
}
