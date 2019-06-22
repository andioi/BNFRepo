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
    public partial class CustomCreditSummary : DataPage
    {

        DataSet dataset = null;

        #region static vars
        private string Q_POLICY_DETAIL = "select * from vw_policy_result_detail where idikredit_id = @1";
        private string Q_POLICYPASS_DETAIL = "select * from vw_policy_resultpass_detail where idikredit_id = @1";
        private static string svrpathurl = "../Upload/List";
        #endregion

        #region retrieve
        private void retrieve_debiturinfo(string key)
        {
            string sql = "exec sp_credit_summary @1";
            DataTable dt = conn.GetDataTable(sql, new object[] { key }, dbtimeout);
            dataset = new DataSet();
            dataset.Tables.Add(dt);
            staticFramework.retrieve(dt, "reffnumber", appid);

            string Q_NAME_LIST = "select appid, cust_name+' ('+status_app+')' cust_name from slik_applicant where "+
                "reffnumber = (select reffnumber from slik_applicant where appid = @1)";
            staticFramework.reff(ddl_appid, Q_NAME_LIST, new object[] { key }, conn, false);
            staticFramework.retrieve(dt, "reffnumber", ddl_appid);
            //staticFramework.retrieve(dt, cust_name);
            //staticFramework.retrieve(dt, idnumber);
            //staticFramework.retrieve(dt, request_date);
            //staticFramework.retrieve(dt, request_time);
            //staticFramework.retrieve(dt, reffnumber);
            //staticFramework.retrieve(dt, worst_collect);
            //staticFramework.retrieve(dt, worst_collect_date);
            //staticFramework.retrieve(dt, total_plafon_credit_active);
            //staticFramework.retrieve(dt, total_os_credit_active);
            //staticFramework.retrieve(dt, total_plafon_lc_active);
            //staticFramework.retrieve(dt, total_os_lc_active);
            //staticFramework.retrieve(dt, total_plafon_guarantee_active);
            //staticFramework.retrieve(dt, total_os_guarantee_active);
            //staticFramework.retrieve(dt, total_plafon_securities_active);
            //staticFramework.retrieve(dt, total_os_securities_active);
            //staticFramework.retrieve(dt, total_plafon_other_active);
            //staticFramework.retrieve(dt, total_os_other_active);
            //staticFramework.retrieve(dt, total_plafon_wc_active);
            //staticFramework.retrieve(dt, total_os_wc_active);
            //staticFramework.retrieve(dt, total_unused_wc_active);
            //staticFramework.retrieve(dt, total_plafon_invest_active);
            //staticFramework.retrieve(dt, total_os_invest_active);
            //staticFramework.retrieve(dt, total_unused_invest_active);
            //staticFramework.retrieve(dt, total_plafon_cf_active);
            //staticFramework.retrieve(dt, total_os_cf_active);
            //staticFramework.retrieve(dt, total_unused_cf_active);
            //staticFramework.retrieve(dt, total_plafon_all_fac_active);
            //staticFramework.retrieve(dt, total_os_all_fac_active);


        }

        private void cekrules()
        {
            /*conn.ExecReader("select approval_group from scallgroup where groupid = @1", new object[] { GROUPID }, dbtimeout);
            if (conn.hasRow())
            {
                if (conn.GetFieldValue(0).ToString() == "1")
                {
                    Button1.Disabled = false;
                }
                else
                {
                    Button1.Disabled = true;
                }
            }*/
        }


        private void runcreditpolicy(string key)
        {
            object[] par = new object[] { key };
            conn.ExecNonQuery("exec slik_clearPolicy @1", par, dbtimeout);
            string sql = "select * from slik_vw_creditpolicy where appid = @1 ";
            DataTable dt = conn.GetDataTable(sql, par, dbtimeout);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DecSystem d = new DecSystem(conn);
                string resultid = dt.Rows[i]["fasilitasid"].ToString();
                par = new object[] { resultid, "SLIK" };
                try
                {
                    string ResultID = d.execute("AND [slik_ideb_kredit].[fasilitasid]=@1", par, "POLICY", "SLIK", null);
                    par = new object[] { resultid, "POLICY", "SLIK", ResultID };
                    conn.ExecuteNonQuery("EXEC SP_APPDECSYSRES @1,@2,@3,@4", par, dbtimeout);
                }
                catch { }
            }
            par = new object[] { key };
            conn.ExecNonQuery("exec slik_updFinalPolicy @1", par, dbtimeout);
        }
        #endregion

        #region databinding
        private void gridbind_kredit(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable("select * from vw_creditsummary where appid = @1", par, dbtimeout);
            GridViewKREDIT.DataSource = dt;
            GridViewKREDIT.DataBind();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //runcreditpolicy(Request.QueryString["regno"]);
                retrieve_debiturinfo(Request.QueryString["regno"]);
                //gridbind_kredit(appid.Value);

                if (Request.QueryString["bypasssession"] != null)
                {
                    btnprint.Visible = false;
                    btnpdf.Visible = false;
                }
                cekrules();
            }
            else
            {
                retrieve_debiturinfo(ddl_appid.SelectedValue);
                //gridbind_kredit(appid.Value);
            }
        }

        protected void GridViewKREDIT_Load(object sender, EventArgs e)
        {
            gridbind_kredit(appid.Value);
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
                string filename = ddl_appid.SelectedValue + "_" + USERID + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string strFileName = "Summ_"+Session["userid"] + "_" + DateTime.Now.ToString("yyMMddhhmmss");
                string strFilePath = Server.MapPath(svrpathurl) + "\\" + strFileName + ".xls";
                System.IO.FileStream fs = new System.IO.FileStream(strFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                gridExport.WriteXls(fs);
                fs.Dispose();

                Response.Write("<script for=window event=onload language='JavaScript'>");
                Response.Write("window.open('" + svrpathurl + "/" + strFileName + ".xls" + "');");
                Response.Write("</script>");
            }
            catch (Exception ex)
            {
                MyPage.popMessage(Page, ex.Message);
            }
        }

        #region Additional Function
        public string FormatedValue(object value)
        {
            string FormatType = null;
            if (value is Int32 || value is Int64 || value is float || value is double || value is decimal)
                FormatType = "n0";
            if (value is DateTime)
                FormatType = "dd MMMM yyyy";
            return FormatedValue(value, FormatType);
        }
        public string FormatedValue(object value, string FormatType)
        {
            if (value == DBNull.Value)
                value = "";
            if (FormatType != null)
            {
                if (value is Int32)
                    value = ((Int32)value).ToString(FormatType);
                else if (value is Int64)
                    value = ((Int64)value).ToString(FormatType);
                else if (value is float)
                    value = ((float)value).ToString(FormatType);
                else if (value is double)
                    value = ((double)value).ToString(FormatType);
                else if (value is decimal)
                    value = ((decimal)value).ToString(FormatType);
                else if (value is DateTime)
                    value = ((DateTime)value).ToString(FormatType);
            }
            return value.ToString();
        }

        public string DS(int tbl, string FieldName)
        {
            try
            {
                object value = dataset.Tables[tbl].Rows[0][FieldName];
                return FormatedValue(value);
            }
            catch
            {
                return "";
            }
        }

        public string DS(int tbl, string FieldName, string FormatType)
        {
            try
            {
                if (dataset.Tables[tbl].Rows.Count == 0)
                    return "";
                object value = dataset.Tables[tbl].Rows[0][FieldName];
                return FormatedValue(value, FormatType);
            }
            catch
            {
                return "";
            }
        }

        public string DS_SUM(int tbl, string FieldName, string sumtype)
        {
            return DS_SUM(tbl, FieldName, sumtype, null);
        }
        public string DS_SUM(int tbl, string FieldName, string sumtype, string FormatType)
        {
            DataTable dt = dataset.Tables[tbl];

            double value = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (sumtype)
                {
                    case "SUM":
                        if (dt.Rows[i][FieldName] != DBNull.Value)
                            value += double.Parse(dt.Rows[i][FieldName].ToString());
                        break;
                    case "CNT":
                        value += 1;
                        break;
                    case "AVG":
                        if (dt.Rows[i][FieldName] != DBNull.Value)
                            value += Convert.ToSingle(dt.Rows[i][FieldName]);
                        break;

                }
            }
            if (sumtype == "AVG" && value != 0)
                value = value / dt.Rows.Count;

            if (value != 0)
            {
                if (FormatType == null)
                    return FormatedValue(value);
                else
                    return FormatedValue(value, FormatType);
            }
            else return "";
        }
        #endregion

    }
}
