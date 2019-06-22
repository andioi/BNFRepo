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


namespace DebtChecking.Facilities
{
    public partial class InqStatus : MasterPage
    {
        #region static vars
        private static string Q_TRCURR = "SELECT * FROM VW_APPFLAG WHERE AP_REGNO = @1 ";
        private static string Q_TRHIST = "SELECT * FROM VW_TRACKHISTORY WHERE AP_REGNO = @1 ORDER BY TR_DATE DESC";
        #endregion

        #region binddata
        protected void gridbindtr()
        {
            object[] param = new object[] { Request.QueryString["regno"] };
            gridTr.DataSource = conn.GetDataTable(Q_TRCURR, param, dbtimeout);
            gridTr.DataBind();
        }

        protected void gridbindtrhist()
        {
            object[] param = new object[] { Request.QueryString["regno"] };
            gridTrHist.DataSource = conn.GetDataTable(Q_TRHIST, param, dbtimeout);
            gridTrHist.DataBind();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void gridTr_Load(object sender, EventArgs e)
        {
            gridbindtr();
        }

        protected void gridTrHist_Load(object sender, EventArgs e)
        {
            gridbindtrhist();
        }
    }
}
