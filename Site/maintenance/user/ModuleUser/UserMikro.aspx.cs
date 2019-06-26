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

namespace MikroMnt.user.ModuleUser
{
    public partial class UserMikro : System.Web.UI.Page
    {
        private DbConnection conn, modconn;
        private int dbtimeout;
        private string ConnString, ModConnString;
        private string moduleid, groupid;

        #region static vars
        private static string Q_MODULEGROUP = "select groupid, sg_grpname, moduleid, modulename, approval_group, usermntpage from vw_grpaccessmodule ";
        private static string Q_USERDATA = "EXEC SU_SCUSER_PENDINGMODULEUSER @1, @2 ";
        private static string SP_SAVEUSER = "EXEC SU_SCUSER_SAVEPENDINGMODULEUSER @1, @2, @3 ";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            moduleid = Request.QueryString["moduleid"];
            groupid = Request.QueryString["grpid"];
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            ConnString = Session["ConnStringLogin"].ToString();
            ModConnString = MNTTools.GetConnString(moduleid);
            if (!IsPostBack)
            {
                using (modconn = new DbConnection(ModConnString))
                {
                    //ViewData();
                }
            }
            else
            {
                if (Request.Form["sta"] == "save")
                {
                    using (modconn = new DbConnection(ModConnString))
                    {
                        SaveData();
                    }
                }
            }
        }

        private void ViewData()
        {
            object[] paruser = new object[2] { Request.QueryString["uid"], moduleid };
            modconn.ExecReader(Q_USERDATA, paruser, dbtimeout);
            CBL_SEGMEN.Items.Clear();
            while (modconn.hasRow())
            {
                ListItem li = new ListItem();
                li.Value = modconn.GetFieldValue("segmen_id");
                li.Text = modconn.GetFieldValue("segmen_desc");
                li.Selected = (bool)modconn.GetNativeFieldValue("status");
                CBL_SEGMEN.Items.Add(li);
            }
        }

        private void SaveData()
        {
            try
            {

                for (int i = 0; i <= CBL_SEGMEN.Items.Count - 1; i++)
                {
                    object[] p = new object[] { Request.QueryString["uid"], CBL_SEGMEN.Items[i].Value, CBL_SEGMEN.Items[i].Selected };

                    modconn.ExecuteNonQuery(SP_SAVEUSER, p, dbtimeout);
                }

                //MNTTools.InsertModuleUser(Request.QueryString["uid"], moduleid);
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message.Replace("-->", "--)") + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
            }
        }
    }
}
