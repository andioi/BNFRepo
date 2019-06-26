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

namespace MikroMnt.Parameter
{
    public partial class ParamSet : System.Web.UI.Page
    {
        DbConnection conn = null;
        int dbtimeout = 600;

        #region static vars
        private static string Q_PARAMLISTMAKER = "select * from VW_PARAMSET_MAKER where SETID = @1 and GROUPID = @2 order by PARAMPOS";
        private static string Q_PARAMLISTAPPRV = "select * from VW_PARAMSET_APPRV where SETID = @1 and GROUPID = @2 order by PARAMPOS";
        #endregion

        #region init page
        protected override void OnLoad(EventArgs e)
        {
            string modid = "61";
            if (Request.QueryString["moduleid"] != null && Request.QueryString["moduleid"].Trim() != "")
                modid = Request.QueryString["moduleid"];
            conn = new DbConnection(MNTTools.GetConnString(modid));
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            try
            {
                conn.Dispose();
            }
            catch { }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LBL_TITLE.Text = Request.QueryString["title"];

                dataView.ColumnCount = 5;
                dataView.RowPerPage = 5;
                if (Request.QueryString["col"] != null)
                    try
                    {
                        dataView.ColumnCount = int.Parse(Request.QueryString["col"]);
                    }
                    catch { }
                if (Request.QueryString["row"] != null)
                    try
                    {
                        dataView.RowPerPage = int.Parse(Request.QueryString["row"]);
                    }
                    catch { }

                if (Session["BackURL"] != null)
                    Session.Remove("BackURL");
                Session.Add("BackURL", Request.RawUrl);
            }
        }

        private void binddata()
        {
            object[] par = new object[] { Request.QueryString["set"], Session["GroupID"] };
            string Q_PARAMLIST = Q_PARAMLISTMAKER;
            if(Request.QueryString["ismaker"]=="0")
                Q_PARAMLIST = Q_PARAMLISTAPPRV;
            dataView.DataSource = conn.GetDataTable(Q_PARAMLIST, par, dbtimeout);
            dataView.DataBind();
        }

        protected void dataView_Load(object sender, EventArgs e)
        {
            binddata();
        }
    }
}
