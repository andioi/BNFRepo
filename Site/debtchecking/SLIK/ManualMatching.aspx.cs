using System;
using System.Collections;
using System.Configuration;
using System.Data;
using DMS.Framework;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using DevExpress.Web;
using DMS.Tools;
using EvoPdf.HtmlToPdf;

namespace DebtChecking.Verification
{
    public partial class ManualMatching : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        private static string Q_NAME_LIST = "select * from VW_APPSTATUS_APP where reffnumber = @1";
        int i = 0;
        #endregion

        private void retrieve_debiturinfo(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_DEBITURINFO, par, dbtimeout);
            staticFramework.retrieve(dt, reffnumber);
            object[] par2 = new object[] { reffnumber.Value };
            staticFramework.reff(appid, Q_NAME_LIST, par2, conn);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, BORN_DATE);
            staticFramework.retrieve(dt, ktp_num);
            staticFramework.retrieve(dt, npwp);
            staticFramework.retrieve(dt, STATUS_APP);
            staticFramework.retrieve(dt, ALAMAT_DOM);
            staticFramework.retrieve(dt, ALAMAT_KTP);
            staticFramework.retrieve(dt, TELP_HP);
            staticFramework.retrieve(dt, ALAMAT_OFC);
            staticFramework.retrieve(dt, TLP_OFC);
            staticFramework.retrieve(dt, POLICYRES);
            staticFramework.retrieve(dt, addr_match);
            staticFramework.retrieve(dt, coyname_match);
            staticFramework.retrieve(dt, bizaddr_match);
            staticFramework.retrieve(dt, addr_match_qc);
            staticFramework.retrieve(dt, coyname_match_qc);
            staticFramework.retrieve(dt, bizaddr_match_qc);
            staticFramework.retrieve(dt, manualmatch_by);
            staticFramework.retrieve(dt, manualmatch_date);
            staticFramework.retrieve(dt, manualmatch_qc_by);
            staticFramework.retrieve(dt, manualmatch_qc_date);
            staticFramework.retrieve(dt, exportsif_by);
            staticFramework.retrieve(dt, exportsif_date);
            staticFramework.retrieve(dt, homephone_match);
            staticFramework.retrieve(dt, bizphone_match);
            staticFramework.retrieve(dt, cellphone_match);
            staticFramework.retrieve(dt, homephone_match_qc);
            staticFramework.retrieve(dt, bizphone_match_qc);
            staticFramework.retrieve(dt, cellphone_match_qc);
            staticFramework.retrieve(dt, NAME_ON_ID);
            exportfilename.Value = dt.Rows[0]["reffnumber"].ToString() + "_" + dt.Rows[0]["debitur_name"].ToString();
            flag_spv.Value = Session["flag_spv"].ToString();
            if(Request.QueryString["QC"]==null)
            {
                tbl_qc.Visible = false;
                tr_qc.Visible = false;
                if (dt.Rows[0]["manualmatch_by"].ToString()!=USERID && Session["flag_spv"].ToString() == "0")
                    btnsave.Attributes.Add("disabled", "disabled");
            } else
            {
                addr_match.Enabled = false;
                coyname_match.Enabled = false;
                bizaddr_match.Enabled = false;
                homephone_match.Enabled = false;
                bizphone_match.Enabled = false;
                cellphone_match.Enabled = false;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
                ViewData(Request.QueryString["regno"]);
                hd_appid.Value = Request.QueryString["regno"];
            }
            else
            {
                retrieve_debiturinfo(appid.SelectedValue);
                ViewData(appid.SelectedValue);
            }
        }

        private void ViewData(string key)
        {
            string src = "ManualReview.aspx?regno=" + Request.QueryString["regno"];
            if (Request.QueryString["QC"]!=null)
                src = "ManualReview.aspx?regno=" + Request.QueryString["regno"]+"&QC="+Request.QueryString["QC"];
            IFR_TEXT.Attributes.Add("src", src);
        }

    }
}
