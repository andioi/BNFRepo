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
    public partial class SLIKCreditSummary : DataPage
    {

        DataSet dataset = null;

        #region static vars
        private string Q_POLICY_DETAIL = "select * from vw_policy_result_detail where idikredit_id = @1";
        private string Q_POLICYPASS_DETAIL = "select * from vw_policy_resultpass_detail where idikredit_id = @1";
        #endregion

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
            //staticFramework.retrieve(dt, npwp);
            //staticFramework.retrieve(dt, gender);
            //staticFramework.retrieve(dt, mother_name);
            //staticFramework.retrieve(dt, full_ktpaddress);
            staticFramework.retrieve(dt, genderdesc);
            //staticFramework.retrieve(dt, full_officeaddress);
            //staticFramework.retrieve(dt, full_econaddress);
            staticFramework.retrieve(dt, final_policy);
        }

        private void cekrules()
        {
            conn.ExecReader("select approval_group from scallgroup where groupid = @1", new object[] { GROUPID }, dbtimeout);
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
            }
        }

        private void retrieve_data_kredit(string key)
        {
            DataTable dt = conn.GetDataTable("select * from vw_ideb_kredit where fasilitasid = @1", new object[] { key }, dbtimeout);
            dataset = new DataSet();
            dataset.Tables.Add(dt);
        }

        private void retrieve_data_history(string key)
        {
            DataTable dt = conn.GetDataTable("select * from vw_ideb_kredit_history where fasilitasid = @1", new object[] { key }, dbtimeout);
            dataset = new DataSet();
            dataset.Tables.Add(dt);
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
            DataTable dt = conn.GetDataTable("select * from vw_ideb_kredit where appid = @1", par, dbtimeout);
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

        #region CALLBACK PANEL
        protected void PNL_KREDIT_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                retrieve_data_kredit(e.Parameter.Substring(2));
            }
        }

        protected void PNL_HISTORY_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                retrieve_data_history(e.Parameter.Substring(2));
            }
        }

        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter=="r:")
            {
                runcreditpolicy(appid.Value);
                retrieve_debiturinfo(appid.Value);
                gridbind_kredit(appid.Value);
                mainPanel.JSProperties["cp_alert"] = "Policy has been recalculated.";
            }

        }
        #endregion

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
