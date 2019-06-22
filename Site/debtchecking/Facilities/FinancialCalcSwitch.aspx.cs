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
using System.Collections.Specialized;

namespace DebtChecking.Facilities
{
    public partial class FinancialCalcSwitch : DataPage
    {
        private static string Q_PAGEURL = "Select FINCAL_URL From rfproduct Where productid = (select productid from slik_applicant where appid = @1) ";

        protected void Page_Load(object sender, EventArgs e)
        {
            loadUrlDetailUsaha();
        }

        private void loadUrlDetailUsaha()
        {
            object[] param = new object[] { Request.QueryString["regno"] };
            DataTable dt = conn.GetDataTable(Q_PAGEURL,param, dbtimeout, true, true);
            if (dt.Rows.Count == 0) return;
            staticFramework.retrieve(dt, FINCAL_URL);

            if (FINCAL_URL.Value.Trim() != "")
            {
                FINCAL_URL.Value = FINCAL_URL.Value.Trim() + "?regno=" + Request.QueryString["regno"];
                if (Request.QueryString["readonly"] != null)
                    FINCAL_URL.Value = FINCAL_URL.Value.Trim() + "&readonly=" + Request.QueryString["readonly"];
            }
        }
    }
}
