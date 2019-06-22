using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DMS.Tools;

namespace ePayroll_v2.Login
{
    public partial class Logout : System.Web.UI.Page
    {
        #region static vars
        private DbConnection conn;
        private string connstring;
        private int dbtimeout;
        private static string U_UPD_USERFLAG = "update scuserflag set su_logon = 0 where userid = @1";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            connstring = (string)Session["ConnString"];
            dbtimeout = (int)Session["DbTimeOut"];
            string url = "Login.aspx";
            using (conn = new DbConnection(connstring))
            {
                object[] paruser = new object[1] { Session["UserID"] };
                try
                {
                    conn.ExecuteNonQuery(U_UPD_USERFLAG, paruser, dbtimeout);
                }
                catch { }
            }

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            if (Request.QueryString.Keys.Count != 0)
            {
                switch (Request.QueryString[0])
                {
                    case "login":
                        url += "?login";
                        break;
                    case "menu":
                        url += "?menu=0";
                        break;
                    default:
                        break;
                }
            }
            //Response.Redirect(url.Trim(), true);
            Response.Write("<html><head><title>Logout</title>");
            Response.Write("<script language='JavaScript'>window.location='" + url + "';</script>");
            Response.Write("</head></html>");
            Response.End();
        }
    }
}
