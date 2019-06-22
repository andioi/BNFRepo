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
    public partial class UC_EarmarkInfo : System.Web.UI.UserControl
    {
        #region Connection & class variables
        private int dbtimeout;
        private DbConnection conn;

        #region static vars
        private static string Q_EARMARKINFO = "select * from vw_gen_earmarkinfo where ap_regno = @1 ";
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
            conn.ExecReader(Q_EARMARKINFO, par, dbtimeout);
            if (conn.hasRow())
            {
                t1.InnerText = "Informasi Earmarking " + conn.GetFieldValue("mitraname");
                l12.InnerText = ((double)conn.GetNativeFieldValue("plafon")).ToString("###,##0.00");
                l21.InnerText = ((double)conn.GetNativeFieldValue("plafonbooked")).ToString("###,##0.00");
                l22.InnerText = ((double)conn.GetNativeFieldValue("plafonapproved")).ToString("###,##0.00");
                l23.InnerText = ((double)conn.GetNativeFieldValue("plafonpipeline")).ToString("###,##0.00");
                if (conn.GetFieldValue("msg").Trim() != "")
                {
                    trmsg.Visible = true;
                    msg.InnerText = conn.GetFieldValue("msg");
                }
            }
            else
                tbl.Visible = false;
        }

        private void FillFields()
        {
            if (t1.InnerText == "")
                t1.InnerHtml = "&nbsp;";
            if (l12.InnerText == "")
                l12.InnerHtml = "&nbsp;";
            if (l21.InnerText == "")
                l21.InnerHtml = "&nbsp;";
            if (l22.InnerText == "")
                l22.InnerHtml = "&nbsp;";
            if (l23.InnerText == "")
                l23.InnerHtml = "&nbsp;";
        }
    }
}