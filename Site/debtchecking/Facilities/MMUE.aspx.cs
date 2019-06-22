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
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using EvoPdf.HtmlToPdf;

namespace DebtChecking.Facilities
{
    public partial class MMUE : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE reffnumber in (select reffnumber from slik_applicant where APPID = @1)";
        private static string Q_NAME_LIST = "select * from VW_APPSTATUS_APP where reffnumber = @1";
        DataSet dsMMUE = null;
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
                object value = dsMMUE.Tables[tbl].Rows[0][FieldName];
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
                if (dsMMUE.Tables[tbl].Rows.Count == 0)
                    return "";
                object value = dsMMUE.Tables[tbl].Rows[0][FieldName];
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
            DataTable dt = dsMMUE.Tables[tbl];

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
                DataTable dt = conn.GetDataTable("EXEC SP_MMUE @1", new object[] { Request.QueryString["regno"] }, dbtimeout);
                DataTable dt2 = conn.GetDataTable("EXEC SP_MMUE_CC_UPTO6MONTH @1", new object[] { Request.QueryString["regno"] }, dbtimeout);
                DataTable dt3 = conn.GetDataTable("EXEC SP_MMUE_CC_UPPER6MONTH @1", new object[] { Request.QueryString["regno"] }, dbtimeout);
                dsMMUE = new DataSet();
                dsMMUE.Tables.Add(dt);
                dsMMUE.Tables.Add(dt2);
                dsMMUE.Tables.Add(dt3);

                GRID_1.DataSource = dsMMUE.Tables[1];
                GRID_1.DataBind();
                GRID_2.DataSource = dsMMUE.Tables[2];
                GRID_2.DataBind();

                if (Request.QueryString["bypasssession"] != null)
                {
                    btnprint.Visible = false;
                    btnpdf.Visible = false;
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

