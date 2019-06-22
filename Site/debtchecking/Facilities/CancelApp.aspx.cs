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

using System.Collections.Specialized;
using DMS.Framework;

namespace DebtChecking.Facilities
{
    public partial class CancelApp : DataPage
    {
        #region static vars
        private static string Q_REF_CANCELTYPE = "select * from rftracklst where canceltrack = 1";
        private static string Q_REF_CANCELREASON = "select * from rfcancel";
        private static string Q_DATA = "select * from VW_DE_MAIN where ap_regno = @1 ";
        private static string Q_TRACK = "select f.ap_currtrcode + isnull(' - ' + l.tr_desc, '') as tr_desc " +
            "from appflag f join rftracklst l on l.tr_code = f.ap_currtrcode where f.ap_regno = @1";
        #endregion

        #region retrieve & save & delete & gridbind

        #region initial_reffrential_parameter
        private void initial_reffrential_parameter()
        {
            staticFramework.reff(df_TR_CODE, Q_REF_CANCELTYPE, null, conn);
            staticFramework.reff(df_CN_CODE, Q_REF_CANCELREASON, null, conn);
        }
        #endregion

        #region retrieve_data & schema
        private void retrieve_data()
        {
            object[] param = new object[1] { Request.QueryString["regno"] };
            conn.ExecReader(Q_DATA, param, dbtimeout);
            if (conn.hasRow())
            {
                staticFramework.retrieve(conn, dfc_CU_BORNDATE, "dfc_");
                staticFramework.retrieve(conn, dfc_CU_NAME, "dfc_");
                staticFramework.retrieve(conn, dfc_CU_HMPHN, "dfc_");
                staticFramework.retrieve(conn, dfc_CU_KTPNO, "dfc_");
                staticFramework.retrieve(conn, dfd_APPTYPEDESC, "dfd_");
                staticFramework.retrieve(conn, dfd_PROD_TYPEDESC, "dfd_");
                staticFramework.retrieve(conn, dfd_PRODCATDESC, "dfd_");
                staticFramework.retrieve(conn, dfd_PRODUCTDESC, "dfd_");
            }
            conn.ExecReader(Q_TRACK, param, dbtimeout);
            if (conn.hasRow())
            {
                staticFramework.retrieve(conn, df_TR_DESC, "df_");
            }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initial_reffrential_parameter();
                retrieve_data();
            }
        }

        protected void mainPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "cancel")
                {
                    object[] param = new object[] { Request.QueryString["regno"], USERID, df_TR_CODE.SelectedValue, df_CN_CODE.SelectedValue };
                    staticFramework.updatestatus(mainPanel, param, "USP_FAC_CANCELAPP", conn, "Application canceled.");
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                if (errmsg.IndexOf("Last Query") > 0)
                    errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                mainPanel.JSProperties["cp_alert"] = errmsg;
            }
        }
    }
}
