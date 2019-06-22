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

namespace DebtChecking.Facilities
{
    public partial class ApprovalSLIK : DataPage
    {
        #region retreive & save

        protected void initial_reffrential_parameter()
        {
            //
        }

        private void retreive_schema()
        {
            //
        }

        private void retrieve_data()
        {
            DataTable dt = conn.GetDataTable("select * from vw_apprequest where requestid = @1",
                new object[] { Request.QueryString["requestid"] }, dbtimeout);
            staticFramework.retrieve(dt, requestid);
            staticFramework.retrieve(dt, inputby);
            staticFramework.retrieve(dt, reqdate);
            staticFramework.retrieve(dt, productdesc);
            staticFramework.retrieve(dt, purposedesc);
            staticFramework.retrieve(dt, branchname);
            staticFramework.retrieve(dt, cust_type);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, dob);
            staticFramework.retrieve(dt, ktp);
            staticFramework.retrieve(dt, pob);
            staticFramework.retrieve(dt, npwp);
            staticFramework.retrieve(dt, homeaddress);
            staticFramework.retrieve(dt, phonenumber);
            staticFramework.retrieve(dt, status_wl);

            if (cust_type.Text == "IND")
            {
                staticFramework.retrieve(dt, gender_desc);
                staticFramework.retrieve(dt, mother_name);
            }
            else
            {
                tr_gender.Visible = false;
                tr_mother_name.Visible = false;
            }
        }

        private void gridbind_suppl()
        {
            DataTable dt = conn.GetDataTable("select *, rel_desc as relation from apprequestsupp left join rfrelationbic " +
                "on rel_code = status_app where requestid = @1 order by seq",
                new object[] { requestid.Text }, dbtimeout);
            GridViewSuppl.DataSource = dt;
            GridViewSuppl.DataBind();
        }

        protected void gridbindnotes()
        {
            DataTable dt = conn.GetDataTable("select * from vw_apprequesttrack where requestid = @1 order by seq desc",
                new object[] { Request.QueryString["requestid"] }, dbtimeout);
            if (dt.Rows.Count > 0)
            {
                GRID_NOTES.DataSource = dt;
                GRID_NOTES.DataBind();
            }
            else { GRID_NOTES.Visible = false; }
        }

        private void gridbind_blacklist()
        {
            DataTable dt = conn.GetDataTable("select * from vw_blacklist_result where requestid = @1 order by blacklistid",
                new object[] { requestid.Text }, dbtimeout);

            if (dt.Rows.Count > 0)
            {
                GridViewBlacklist.DataSource = dt;
                GridViewBlacklist.DataBind();
            }
            else
            {
                tr_blheader.Visible = false;
                tr_blacklist.Visible = false;
                GridViewBlacklist.Visible = false;
            }
        }

        private void gridbind_whitelist()
        {
            DataTable dt = conn.GetDataTable("select * from vw_whitelist_result where requestid = @1 order by whitelistid",
                new object[] { requestid.Text }, dbtimeout);

            if (dt.Rows.Count > 0)
            {
                GridViewWhitelist.DataSource = dt;
                GridViewWhitelist.DataBind();
            }
            else
            {
                tr_wlheader.Visible = false;
                tr_whitelist.Visible = false;
                GridViewWhitelist.Visible = false;
            }
        }

        private void savedata()
        {
            //
        }
        #endregion

        #region Additional Function
        DataSet ds = null;

        public string FormatedValue(object value)
        {
            string FormatType = null;
            if (value is Int32 || value is Int64 || value is float || value is double || value is decimal)
                FormatType = "n0";
            if (value is DateTime)
                FormatType = "dd MMM yyyy HH:mm:ss";
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
                object value = ds.Tables[tbl].Rows[0][FieldName];
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
                if (ds.Tables[tbl].Rows.Count == 0)
                    return "";
                object value = ds.Tables[tbl].Rows[0][FieldName];
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
            DataTable dt = ds.Tables[tbl];

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
                retrieve_data();
                gridbind_suppl();
                gridbindnotes();
                gridbind_blacklist();
            }
        }

        #region callback
        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "a")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "BMA", "GCC", "APV", Session["UserID"], null };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    mainPanel.JSProperties["cp_alert"] = "Data permintaan SLIK checking berhasil diapprove.";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|BM&passurl&mntitle=Approval BM&li=L|BIC|BM";
                }
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

        protected void PanelSID_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "r")
                    btn_confirm.Attributes["onclick"] = "callback(PanelSID,'reject')";
                else if (e.Parameter == "v")
                    btn_confirm.Attributes["onclick"] = "callback(PanelSID,'reverse')";
                if (e.Parameter == "reject")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "BMA", "RJC", "RJC", Session["UserID"], comment.Text };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    PanelSID.JSProperties["cp_alert"] = "Data permintaan SLIK checking berhasil direject.";
                    PanelSID.JSProperties["cp_target"] = "_parent";
                    PanelSID.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|BM&passurl&mntitle=Approval BM&li=L|BIC|BM";
                }
                else if (e.Parameter == "reverse")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "BMA", "DRF", "RVS", Session["UserID"], comment.Text };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    PanelSID.JSProperties["cp_alert"] = "Data permintaan SLIK checking berhasil direverse.";
                    PanelSID.JSProperties["cp_target"] = "_parent";
                    PanelSID.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|BM&passurl&mntitle=Approval BM&li=L|BIC|BM";
                }
            }
            catch (Exception ex)
            {
                string errmsg = "";
                if (ex.Message.IndexOf("Last Query:") > 0)
                    errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                else
                    errmsg = ex.Message;
                PanelSID.JSProperties["cp_alert"] = errmsg;
            }
        }

        protected void GridViewSuppl_Load(object sender, EventArgs e)
        {
            gridbind_suppl();
            if (Request.QueryString["readonly"] != null)
            {
                btn_back.Visible = false;
                btn_apprv.Visible = false;
                btn_reject.Visible = false;
            }
        }

        protected void GridViewBlacklist_Load(object sender, EventArgs e)
        {
            gridbind_blacklist();
        }

        protected void GridViewWhitelist_Load(object sender, EventArgs e)
        {
            gridbind_whitelist();
        }
        #endregion

    }
}