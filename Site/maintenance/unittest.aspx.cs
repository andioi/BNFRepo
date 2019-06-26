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
using UOBISecurity;

namespace MikroMnt
{
    public partial class unittest : System.Web.UI.Page
    {
        #region static vars
        private static string Q_LOGINSCR = "select top 1 LOGIN_SCR, DB_IP, DB_NAMA, DB_LOGINID, DB_LOGINPWD from RFMODULE where MODULEID = @1 ";
        private static string Q_USERDATA = "select USERID, GROUPID, SU_FULLNAME, BRANCHID " +
            "from VW_SESSIONLOS where USERID = @1 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string getConnStringLogin()
        {

            //Encryptor enc = new Encryptor();
            //ApplicationRegistryHandler reg = new ApplicationRegistryHandler();
            //string appName = ConfigurationSettings.AppSettings["appName"].ToString();
            //appName = appName.Replace(" ", "");
            //string uobikey = ConfigurationSettings.AppSettings["UOBIKey"].ToString();
            //string DBConfig = ConfigurationSettings.AppSettings["DBConfig"].ToString();
            //string keyReg = reg.ReadFromRegistry("Software\\" + appName, "Key");

            //string connDetail = enc.Decrypt(DBConfig, keyReg, uobikey);
            //string[] connDetails = connDetail.Split(';');
            //string dbhost = connDetails[1].Substring(connDetails[1].IndexOf("=") + 1);
            //string dbname = connDetails[2].Substring(connDetails[2].IndexOf("=") + 1);
            //string dbuser = connDetails[3].Substring(connDetails[3].IndexOf("=") + 1);
            //string dbpwd = connDetails[4].Substring(connDetails[4].IndexOf("=") + 1);
            //string connstring = "Data Source=" + dbhost + ";Initial Catalog=" + dbname + ";uid=" + dbuser + ";pwd=" + dbpwd + ";Pooling=true";

            string connstring = ConfigurationSettings.AppSettings["connString"].ToString();
            return connstring;

        }

        private string getConnStringModule()
        {

            //Encryptor enc = new Encryptor();
            //ApplicationRegistryHandler reg = new ApplicationRegistryHandler();
            //string appName = ConfigurationSettings.AppSettings["appName"].ToString();
            //appName = appName.Replace(" ", "");
            //string uobikey = ConfigurationSettings.AppSettings["UOBIKeyModule"].ToString();
            //string DBConfig = ConfigurationSettings.AppSettings["DBConfigModule"].ToString();
            //string keyReg = reg.ReadFromRegistry("Software\\" + appName, "Key");

            //string connDetail = enc.Decrypt(DBConfig, keyReg, uobikey);
            //string[] connDetails = connDetail.Split(';');
            //string dbhost = connDetails[1].Substring(connDetails[1].IndexOf("=") + 1);
            //string dbname = connDetails[2].Substring(connDetails[2].IndexOf("=") + 1);
            //string dbuser = connDetails[3].Substring(connDetails[3].IndexOf("=") + 1);
            //string dbpwd = connDetails[4].Substring(connDetails[4].IndexOf("=") + 1);
            //string connstring = "Data Source=" + dbhost + ";Initial Catalog=" + dbname + ";uid=" + dbuser + ";pwd=" + dbpwd + ";Pooling=true";

            string connstring = ConfigurationSettings.AppSettings["connStringModule"].ToString();
            return connstring;

        }

        private bool AddSession(string uid, string connstr, int timeout)
        {
            bool ret = false;
            using (DbConnection locconn = new DbConnection(getConnStringLogin()))			//load user data from loslogin for sysca maintenance user 
            {
                object[] user = new object[1] { uid };
                locconn.ExecReader(Q_USERDATA, user, timeout);
                if (locconn.hasRow())
                {
                    Session.Add("UserID", locconn.GetFieldValue("USERID"));
                    Session.Add("FullName", locconn.GetFieldValue("SU_FULLNAME"));
                    Session.Add("GroupID", locconn.GetFieldValue("GROUPID"));
                    Session.Add("ModuleID", ConfigurationSettings.AppSettings["ModuleID"]);
                    Session.Add("ConnString", connstr);
                    Session.Add("dbTimeOut", timeout);
                    Session.Add("dbBigTimeOut", int.Parse(ConfigurationSettings.AppSettings["dbBigTimeOut"]));
                    Session.Add("LoginTime", System.DateTime.Now);
                    Session.Add("BranchID", locconn.GetFieldValue("BRANCHID"));
                    Session.Add("ConnStringLogin", getConnStringLogin());
                    Session.Add("ConnString", getConnStringModule());
                    ret = true;
                }
            }
            return ret;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();

            int dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            DbConnection connESecurity = new DbConnection(getConnStringLogin());

            string nexturl = "";
            object[] parmodule = new object[1] { ConfigurationSettings.AppSettings["ModuleID"] };

            connESecurity.ExecReader(Q_LOGINSCR, parmodule, dbtimeout);
            if (connESecurity.hasRow())
            {
                string uid = TextBox1.Text,
                    dbip = connESecurity.GetFieldValue(1),
                    dbname = connESecurity.GetFieldValue(2),
                    dbuid = connESecurity.GetFieldValue(3),
                    dbpwd = connESecurity.GetFieldValue(4);

                if (AddSession(uid, "Data Source=" + dbip + ";Initial Catalog=" + dbname + ";uid=" + dbuid + ";pwd=" + dbpwd + ";Pooling=true", dbtimeout))
                {
                    FormsAuthentication.SetAuthCookie(uid, false);
                    nexturl = "main.html";
                }
            }


            connESecurity.Dispose();
            if (nexturl != "")
                Response.Redirect(nexturl, true);
            else
                MyPage.popMessage(this, "User not found");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect(TextBox2.Text);
        }
    }
}
