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

namespace DebtChecking.Facilities
{
    public partial class CreditSummary : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        private string Q_VW_SID_KREDIT = "SELECT * FROM VW_SID_KREDIT WHERE APPID = @1 order by REKENING_AKTIF desc";
        private string Q_VW_SID_KREDITDETAIL = "SELECT * FROM VW_SID_KREDITDETAIL WHERE IDIKREDIT_ID = @1";
        private string Q_VW_SID_KREDITKOLEK = "SELECT * FROM VW_SID_KREDITKOLEK WHERE IDIKREDIT_ID = @1";
        private string Q_POLICY_DETAIL = "select * from vw_policy_result_detail where idikredit_id = @1";
        private string Q_POLICYPASS_DETAIL = "select * from vw_policy_resultpass_detail where idikredit_id = @1";
        private static string Q_NAME_LIST = "select * from VW_APPSTATUS_APP where reffnumber = @1";
        #endregion

        private void save_idikredit()
        {
            object[] par = new object[] { KREDIT_ID.Value };
            conn.ExecNonQuery("exec SP_INCLUDE_IN_CALC @1", par, dbtimeout);
        }

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
            staticFramework.retrieve(dt, POLICYRES);
            staticFramework.retrieve(dt, ALAMAT_DOM);
            staticFramework.retrieve(dt, KTP_NUM);
            staticFramework.retrieve(dt, productid);
            tr_recalculate2.Visible = false;
            
        }

        private void retrieve_data_kredit(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_KREDITDETAIL, par, dbtimeout);
            staticFramework.retrieve(dt, BANK_NAME);
            staticFramework.retrieve(dt, AKAD_AWAL);
            staticFramework.retrieve(dt, JATUH_TEMPO);
            staticFramework.retrieve(dt, SEKTOR_EKONOMI);
            staticFramework.retrieve(dt, JENIS_PENGGUNAAN);
            staticFramework.retrieve(dt, NO_REKENING);
            staticFramework.retrieve(dt, REKENING_AKTIF);
            staticFramework.retrieve(dt, PLAFON);
            staticFramework.retrieve(dt, BAKI_DEBET);
            staticFramework.retrieve(dt, PERCENT_BUNGA);
            staticFramework.retrieve(dt, SEBAB_MACET);
            staticFramework.retrieve(dt, TGL_UPDATE);
            staticFramework.retrieve(dt, PELAPOR);
            staticFramework.retrieve(dt, SIFAT);
            staticFramework.retrieve(dt, KONDISI);
            staticFramework.retrieve(dt, TGL_KONDISI);
            staticFramework.retrieve(dt, exclude_calc);
            staticFramework.retrieve(dt, KREDIT_ID);
            staticFramework.retrieve(dt, TUNGGAKAN_POKOK);
            staticFramework.retrieve(dt, BUNGA_ON);
            staticFramework.retrieve(dt, BUNGA_OFF);
            staticFramework.retrieve(dt, FREK);
            staticFramework.retrieve(dt, TGL_MACET);
            staticFramework.retrieve(dt, DESKRIPSI_KONDISI);
            if (exclude_calc.Text.Trim() == "1")
            { BTN_SAVE1.Visible = true; }
            else { BTN_SAVE1.Visible = false; }

            if (Session["ApprovalGroup"].ToString() != "1")
            {
                exclude_calc.Enabled = false;
                BTN_SAVE1.Disabled = true;
            }
        }

        private void runcreditpolicy(string key)
        {
            object[] par = new object[] { key };
            conn.ExecReader("select isnull(creditpolicy,0) from slik_applicant where appid = @1", par, dbtimeout);
            bool creditpolicy = false;
            if (conn.hasRow()) { creditpolicy = bool.Parse(conn.GetFieldValue(0).ToString()); }
            if (!creditpolicy)
            {
                string sql = "select idikredit_id from idi_kredit where debiturinfo_id in (select debiturinfo_id from idi_debitur_info where appid = @1 and selected = '1')";
                DataTable dt = conn.GetDataTable(sql, par, dbtimeout);
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DecSystem d = new DecSystem(conn);
                    string idikredit_id = dt.Rows[i][0].ToString();
                    par = new object[] { idikredit_id, "DE" };

                    string ResultID = d.execute("AND [IDI_KREDIT].[IDIKREDIT_ID]=@1", par,"POLICY", "DE", null);
                    par = new object[] { idikredit_id, "POLICY", "DE", ResultID };
                    conn.ExecuteNonQuery("EXEC SP_APPDECSYSRES @1,@2,@3,@4", par, dbtimeout);

                    ResultID = d.execute("AND [IDI_KREDIT].[IDIKREDIT_ID]=@1", par, "PASSPOLICY", "DE", null);
                    par = new object[] { idikredit_id, "PASSPOLICY", "DE", ResultID };
                    conn.ExecuteNonQuery("EXEC SP_APPDECSYSRES @1,@2,@3,@4", par, dbtimeout);
                }
                par = new object[] { key };
                conn.ExecuteNonQuery("update applicant set creditpolicy = '1' where appid = @1", par, dbtimeout);
            }
        }
        #endregion

        #region databinding
        private void gridbind_kredit(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_KREDIT, par, dbtimeout);
            GridViewKREDIT.DataSource = dt;
            GridViewKREDIT.DataBind();
        }

        private void gridbind_kolek(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_KREDITKOLEK, par, dbtimeout);
            GridViewKolek.DataSource = dt;
            GridViewKolek.DataBind();
        }

        private void gridbind_policy(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_POLICY_DETAIL, par, dbtimeout);
            GridPolicy.DataSource = dt;
            GridPolicy.DataBind();
        }

        private void gridbind_policy2(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_POLICYPASS_DETAIL, par, dbtimeout);
            GridPolicy2.DataSource = dt;
            GridPolicy2.DataBind();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                runcreditpolicy(Request.QueryString["regno"]);
                retrieve_debiturinfo(Request.QueryString["regno"]);
                gridbind_kredit(Request.QueryString["regno"]);

                if (Request.QueryString["bypasssession"] != null)
                {
                    btnprint.Visible = false;
                    btnpdf.Visible = false;
                    Button5.Visible = false;
                }
            }
        }

        #region KREDIT
        protected void PNL_KREDIT_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                retrieve_data_kredit(e.Parameter.Substring(2));
                gridbind_policy(e.Parameter.Substring(2));
                gridbind_policy2(e.Parameter.Substring(2));
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                save_idikredit();
            }
        }

        protected void GridViewKolek_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_kolek(IDIKREDIT_ID.Value.ToString());
        }

        protected void GridViewKolek_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gridbind_kolek(IDIKREDIT_ID.Value.ToString());
        }

        protected void panelKolektabilitas_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                gridbind_kolek(e.Parameter.Substring(2));
                IDIKREDIT_ID.Value = e.Parameter.Substring(2);
            }
        }

        #endregion

        #region mainpanel callback
        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter == "s:")
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, appid);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "creditpolicy", "0");
                    staticFramework.save(Fields, Keys, "applicant", conn);
                    runcreditpolicy(appid.SelectedValue);
                    retrieve_debiturinfo(appid.SelectedValue);
                    gridbind_kredit(appid.SelectedValue);
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
            else if (e.Parameter=="r:")
            {
                runcreditpolicy(appid.SelectedValue);
                retrieve_debiturinfo(appid.SelectedValue);
                gridbind_kredit(appid.SelectedValue);
            }

        }
        #endregion

        protected void GridViewKREDIT_Load(object sender, EventArgs e)
        {
            gridbind_kredit(appid.SelectedValue);
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
