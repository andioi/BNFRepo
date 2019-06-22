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

namespace DebtChecking.SLIK
{
    public partial class Collateral : DataPage
    {
        
        #region retrieve
        private void retrieve_debiturinfo(string key)
        {
            string sql = "select * from slik_vw_applicant where appid = @1";
            DataTable dt = conn.GetDataTable(sql, new object[] { key }, dbtimeout);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, reffnumber);
            string Q_NAME_LIST = "select appid, cust_name+' ('+status_app+')' cust_name from slik_applicant where " +
                "reffnumber = (select reffnumber from slik_applicant where appid = @1)";
            staticFramework.reff(ddl_appid, Q_NAME_LIST, new object[] { key }, conn, false);
            staticFramework.retrieve(dt, "appid", ddl_appid);
            staticFramework.retrieve(dt, status_app);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, pob_dob);
            staticFramework.retrieve(dt, ktp);
            staticFramework.retrieve(dt, genderdesc);
            staticFramework.retrieve(dt, full_ktpaddress);
            staticFramework.retrieve(dt, final_policy);
        }
        #endregion

        #region databinding

        private void gridbind_agunan(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable("select * from vw_ideb_agunan where appid = @1", par, dbtimeout);
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
                retrieve_debiturinfo(ddl_appid.SelectedValue);
            }
        }

        #region agunan
        protected void GridViewColl_Load(object sender, EventArgs e)
        {
                gridbind_agunan(appid.Value);
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
                string filename = appid.Value + "_" + USERID + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
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
