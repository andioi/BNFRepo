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
using System.IO;
using DMS.Tools;
using DMS.Framework;

namespace DebtChecking.Facilities
{
    public partial class DupcheckResult : DataPage
    {
        #region static vars
        private string Q_VW_DUPCHECKRES = "exec SP_VW_DEDUPRESULT @1, @2";
        #endregion

        #region retrieve

        #endregion

        #region databinding
        private void gridbind_dup()
        {
            object[] par = new object[] { Request.QueryString["regno"], Request.QueryString["din"] };
            DataTable dt = conn.GetDataTable(Q_VW_DUPCHECKRES, par, dbtimeout);
            GridViewKREDIT.DataSource = dt;
            GridViewKREDIT.DataBind();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #region KREDIT
        protected void GridViewKREDIT_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_dup();
        }

        protected void GridViewKREDIT_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                gridbind_dup();
        }
        #endregion

    }
}
