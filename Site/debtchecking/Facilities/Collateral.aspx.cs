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
using EvoPdf.HtmlToPdf;
using DevExpress.Web;

namespace DebtChecking.Facilities
{
    public partial class Collateral : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        private string Q_VW_SID_AGUNAN = "SELECT * FROM VW_SID_AGUNAN WHERE APPID = @1";
        private static string Q_NAME_LIST = "select * from VW_APPSTATUS_APP where reffnumber = @1";
        #endregion

        #region retrieve
        private void retrieve_debiturinfo(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_DEBITURINFO, par, dbtimeout);
            staticFramework.retrieve(dt, reffnumber);
            object[] par2 = new object[] { reffnumber.Value };
            staticFramework.reff(appid, Q_NAME_LIST, par2, conn);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, BORN_DATE);
            staticFramework.retrieve(dt, STATUS_APP);
            staticFramework.retrieve(dt, KTP_NUM);
            staticFramework.retrieve(dt, ALAMAT_DOM);
            staticFramework.retrieve(dt, TELP_HP);
        }
        #endregion

        #region databinding

        private void gridbind_agunan(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_AGUNAN, par, dbtimeout);
            GridViewColl.DataSource = dt;
            GridViewColl.DataBind();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
                if (Request.QueryString["bypasssession"] != null)
                {
                    btnprint.Visible = false;
                    btnpdf.Visible = false;
                }
            }
            else
            {
                retrieve_debiturinfo(appid.SelectedValue);
            }
        }

        #region agunan
        protected void GridViewColl_Load(object sender, EventArgs e)
        {
                gridbind_agunan(appid.SelectedValue);
        }
        #endregion

        protected void pdfPanel_Callback(object source, CallbackEventArgsBase e)
        {
            try
            {
                string DownloadPath = Server.MapPath("../Download/pdf/");

                PdfConverter pdfConverter = new PdfConverter();
                // set the license key
                pdfConverter.LicenseKey = "Vn1ndmVldmdkdmV4Y3ZlZ3hnZHhvb29v";
                // save the PDF bytes in a file on disk
                string url = Request.Url.ToString() + "&bypasssession=1";
                string filename = Request.QueryString["regno"] + "_" + USERID + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
                string fullfilename = DownloadPath + filename;
                pdfConverter.SavePdfFromUrlToFile(url, fullfilename);
                pdfPanel.JSProperties["cp_redirect"] = "../Download/pdf/" + filename;
                pdfPanel.JSProperties["cp_target"] = "_blank";
                //Response.Write("<script>window.open('../Download/pdf/" + filename + "');</script>");
            }
            catch (Exception ex)
            {
                pdfPanel.JSProperties["cp_alert"] = ex.Message;
            }
        }

    }
}
