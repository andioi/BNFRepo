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

namespace MikroLogin
{
    public partial class index : System.Web.UI.Page
    {
        public static string decryptConnStr(string encryptedConnStr)
        {
            string connStr, encpwd, decpwd = "";
            int pos1, pos2;
            pos1 = encryptedConnStr.IndexOf("pwd=");
            pos2 = encryptedConnStr.IndexOf(";", pos1 + 4);
            encpwd = encryptedConnStr.Substring(pos1 + 4, pos2 - pos1 - 4);
            for (int i = 2; i < encpwd.Length; i++)
            {
                char chr = (char)(encpwd[i] - 2);
                decpwd += new string(chr, 1);
            }
            connStr = encryptedConnStr.Replace(encpwd, decpwd);
            return connStr;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = decryptConnStr(ConfigurationSettings.AppSettings["eSecurityConnectString"]);
            int dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            string ipclient = Request.UserHostAddress;
            int indexdot = ipclient.IndexOf(".",ipclient.IndexOf(".",ipclient.IndexOf(".", 0)+1)+1);
            string ipserver = ipclient.Substring(0,indexdot)+".3";
            
            using (DbConnection conn = new DbConnection(connectionString))
            {
                conn.ExecReader("SELECT TOP 1 LOGIN_SCR FROM RFMODULE", null, dbtimeout);
                string loginurl = "";
                if (conn.hasRow())
                {
                    loginurl = conn.GetFieldValue(0);
                }
                string redirecturl = loginurl.Replace("..", "");
                object[] par = new object[] { ipserver };
                conn.ExecReader("SELECT * FROM IPREDIRECT WHERE IPADDRESS = @1", par, dbtimeout);
                
                if (conn.hasRow())
                {
                    //dari cabang
                    Response.Redirect("http://"+ipserver + redirecturl);
                } else {
                    //dari KP
                    Response.Redirect(redirecturl);
                }
            }
            
        }
    }
}
