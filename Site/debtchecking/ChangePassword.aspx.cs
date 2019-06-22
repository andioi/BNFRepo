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
    public partial class ChangePassword : System.Web.UI.Page
    {
        #region static vars
        private static string Q_OLDPWD = "select SU_PWD from SCALLUSER where USERID = @1 ";
        private static string Q_VALIDATEPOLICY = "select dbo.isPwdValid (@1, @2, @3) ";
        private static string Q_MODULEDB = "SELECT DB_IP, DB_NAMA, DB_LOGINID, DB_LOGINPWD " +
            "FROM SCALLUSER U JOIN GRPACCESSMODULE G ON G.GROUPID = U.GROUPID " +
            "JOIN RFMODULE M ON M.MODULEID = G.MODULEID " +
            "WHERE U.USERID = @1 ";
        private static string SP_USRPWDALL = "exec SU_SCALLUSERPASSWORD @1, @2 ";
        private static string SP_USRPWD = "exec SU_SCUSERPASSWORD @1, @2 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            int dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            DbConnection conn = new DbConnection(Session["ConnStringLogin"].ToString());
            conn.ExecReader("select jenisuser from scalluser where userid=@1", new object[] { Session["USERID"] }, dbtimeout);
            if (conn.hasRow())
            {
                if (conn.GetFieldValue(0) != null)
                {
                    if (conn.GetFieldValue(0).ToString() == "1")
                    {
                        MyPage.popMessage(this, "User Active Directory cannot changes password here. Please contact IT Admin..");
                        TXT_OLD.Enabled = false;
                        TXT_NEW.Enabled = false;
                        TXT_VERIFY.Enabled = false;
                        BTN_CHANGE.Enabled = false;
                    }
                }
            }
        }

        private void Clear()
        {
            TXT_OLD.Text = "";
            TXT_NEW.Text = "";
            TXT_VERIFY.Text = "";
            MyPage.SetFocus(this, TXT_OLD);
        }

        protected void BTN_CHANGE_Click(object sender, EventArgs e)
        {
            if (TXT_NEW.Text.Trim().Equals(""))
            {
                LBL_MESSAGE.Text = "New password cannot be blank!";
                Clear();
                return;
            }

            if (TXT_NEW.Text.Trim() != TXT_VERIFY.Text.Trim())
            {
                LBL_MESSAGE.Text = "Password mismatch!";
                Clear();
                return;
            }

            int dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            DbConnection conn = new DbConnection(Session["ConnStringLogin"].ToString());

            string newPassword = "", oldPassword = "", dbPassword = "";
            object[] user = new object[1] { Session["UserID"] };
            Encryption.SimpleEncryption enc = new Encryption.SimpleEncryption();

            if (TXT_OLD.Enabled)
            {
                //oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TXT_OLD.Text.Trim(), "sha1");
                oldPassword = enc.Encrypt(TXT_OLD.Text.Trim(), true);
                conn.ExecReader(Q_OLDPWD, user, dbtimeout);
                if (conn.hasRow())
                    dbPassword = conn.GetFieldValue(0);
            }

            if (!TXT_OLD.Enabled || oldPassword == dbPassword)
            {
                //newPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(TXT_NEW.Text, "sha1");
                newPassword = enc.Encrypt(TXT_NEW.Text, true);
                object[] parnew = new object[3] { Session["UserID"], TXT_NEW.Text, newPassword };
                conn.ExecReader(Q_VALIDATEPOLICY, parnew, dbtimeout);
                if (conn.hasRow())
                    if (conn.GetFieldValue(0) == "")
                    {
                        parnew = new object[2] { Session["UserID"].ToString(), newPassword };
                        conn.ExecuteNonQuery(SP_USRPWDALL, parnew, dbtimeout);

                        conn.ExecReader(Q_MODULEDB, user, dbtimeout);
                        while (conn.hasRow())
                        {
                            string connectionString = "Data Source=" + conn.GetFieldValue(0) +		//dbip
                                ";Initial Catalog=" + conn.GetFieldValue(1) +						//dbnama
                                ";uid=" + conn.GetFieldValue(2) +									//db_loginid
                                ";pwd=" + conn.GetFieldValue(3) + ";Pooling=true";					//db_loginpwd
                            using (DbConnection lclConn = new DbConnection(connectionString))
                            {
                                lclConn.ExecuteNonQuery(SP_USRPWD, parnew, dbtimeout);
                            }
                        }

                        LBL_MESSAGE.Text = "";
                        Clear();
                        Response.Write("<script for=window event=onload language=javascript>\n" +
                            "alert('Password Updated!');\nform1.IMG_BACK.click();</script>");
                    }
                    else
                    {
                        LBL_MESSAGE.Text = conn.GetFieldValue(0);
                        Clear();
                    }
            }
            else
            {
                LBL_MESSAGE.Text = "Old Password invalid!";
                Clear();
            }
        }
    }
}
