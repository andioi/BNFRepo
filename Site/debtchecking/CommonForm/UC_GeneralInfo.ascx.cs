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

namespace DebtChecking.CommonForm
{
    public partial class UC_GeneralInfo : System.Web.UI.UserControl
    {
        #region Connection & class variables
        private int dbtimeout;
        private DbConnection conn;

        #region static vars
        private static string Q_APP_GENINFO = "select * from vw_app_geninfo where ap_regno = @1 ";
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dbtimeout = (int)Session["dbTimeOut"];
                using (conn = new DbConnection((string)Session["ConnString"]))
                {
                    ViewData();
                    FillFields();
                }
            }
        }

        private void ViewData()
        {
            object[] par = new object[1] { Request.QueryString["regno"] };
            l1.InnerText = Request.QueryString["regno"];
            conn.ExecReader(Q_APP_GENINFO, par, dbtimeout);
            if (conn.hasRow())
            {
                l2.InnerText = ((DateTime)conn.GetNativeFieldValue("ap_recvdate")).ToString("d MMMM yyyy");
                l3.InnerText = conn.GetFieldValue("bookingbranchname");
                l4.InnerText = conn.GetFieldValue("apptypedesc");

                l5.InnerText = conn.GetFieldValue("pr_desc");
                l6.InnerText = conn.GetFieldValue("ch_desc");

                lA.InnerText = conn.GetFieldValue("cu_fullnm");
                try
                {
                    lB.InnerText = ((DateTime)conn.GetNativeFieldValue("cu_borndate")).ToString("d MMMM yyyy");
                }
                catch { }
                lC.InnerText = conn.GetFieldValue("cu_ktpno");
            }
        }

        private void FillFields()
        {
            if (l1.InnerText == "")
                l1.InnerHtml = "&nbsp;";
            if (l2.InnerText == "")
                l2.InnerHtml = "&nbsp;";
            if (l3.InnerText == "")
                l3.InnerHtml = "&nbsp;";
            if (l4.InnerText == "")
                l4.InnerHtml = "&nbsp;";
            if (l5.InnerText == "")
                l5.InnerHtml = "&nbsp;";
            if (l6.InnerText == "")
                l6.InnerHtml = "&nbsp;";
            if (lA.InnerText == "")
                lA.InnerHtml = "&nbsp;";
            if (lB.InnerText == "")
                lB.InnerHtml = "&nbsp;";
            if (lC.InnerText == "")
                lC.InnerHtml = "&nbsp;";
        }
    }
}