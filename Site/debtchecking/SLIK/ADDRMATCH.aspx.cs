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
using DevExpress.Web;
using EvoPdf.HtmlToPdf;

namespace DebtChecking.SLIK
{
    public partial class ADDRMATCH : DataPage
    {
        #region static vars
        private string Q_VW_INFO = "select * from SLIK_VW_ADDRMATCH_INFO where appid = @1";
        #endregion

        #region retrieve
        private void initial_reffparameter()
        {
            //
        }

        private void retrieve_schema()
        {
            //

        }

        private void retrieve_data(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_INFO, par, dbtimeout);
            staticFramework.retrieve(dt, reffnumber);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, dob);
            staticFramework.retrieve(dt, ktp_num);
            staticFramework.retrieve(dt, home_addr_app);
            staticFramework.retrieve(dt, office_addr_app);
            staticFramework.retrieve(dt, home_phone_app);
            staticFramework.retrieve(dt, office_phone_app);
            staticFramework.retrieve(dt, mobilenum);
            staticFramework.retrieve(dt, ktp_addr_app);
            staticFramework.retrieve(dt, econ_addr_app);
            staticFramework.retrieve(dt, officename);

            par = new object[] { key };
            dt = conn.GetDataTable("select * from appverassignment where appid = @1", par, dbtimeout);
            staticFramework.retrieve(dt, home_match);
            staticFramework.retrieve(dt, office_match);
            staticFramework.retrieve(dt, coyname_match);
        }

        private void gridbind(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable("select * from VW_SLIK_ADDRESS where appid = @1", par, dbtimeout);
            GridViewAddr.DataSource = dt;
            GridViewAddr.DataBind();
            dt = conn.GetDataTable("select distinct tempatBekerja from VW_SLIK_ADDRESS where appid = @1", par, dbtimeout);
            GridViewComp.DataSource = dt;
            GridViewComp.DataBind();
        }

        private void save_data()
        {
            NameValueCollection Keys = new NameValueCollection();
            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
            NameValueCollection Fields = new NameValueCollection();
            staticFramework.saveNVC(Fields, office_match);
            staticFramework.saveNVC(Fields, home_match);
            staticFramework.saveNVC(Fields, coyname_match);
            staticFramework.saveNVC(Fields, "match_addr_by",USERID);
            Fields["match_addr_date"] = "GETDATE()";
            staticFramework.save(Fields, Keys, "appverassignment", conn);
            conn.ExecNonQuery("exec SLIK_SP_PVTYPE_UPDATE @1", new object[] { Request.QueryString["regno"] }, dbtimeout);
        }
        
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            //retrieve_schema();
            //initial_reffparameter();
            
            if (!IsPostBack)
            {
                retrieve_data(Request.QueryString["regno"]);
                gridbind(Request.QueryString["regno"]);
                if (Request.QueryString["bypasssession"] != null)
                {
                    btnprint.Visible = false;
                    btnpdf.Visible = false;
                }
            }
        }

        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter == "save")
            {
                try
                {
                    save_data();
                    mainPanel.JSProperties["cp_alert"] = "Data Saved.";
                }
                catch (Exception ex)
                {
                    string errmsg = "";
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        errmsg = ex.Message;
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
            else if (e.Parameter == "submit")
            {
                try
                {
                    save_data();
                    //submit
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "addr_match", "1");
                    staticFramework.save(Fields, Keys, "applicant", conn);
                    mainPanel.JSProperties["cp_alert"] = "Data Updated.";
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=TOBE|REV&passurl&mntitle=List to be review";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                }
                catch (Exception ex)
                {
                    string errmsg = "";
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        errmsg = ex.Message;
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
        }

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
