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
    public partial class ApprovalSLIK2 : DataPage
    {

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

        #region retreive & save

        protected void initial_reffrential_parameter()
        {
            staticFramework.reff(purpose, "select * FROM rftujuanbic", null, conn);
            staticFramework.reff(branchid, "select * FROM rfbranch", null, conn);
            staticFramework.reff(productid, "select * FROM rfproduct", null, conn);
            staticFramework.reff(status_app, "select * FROM rfrelationbic", null, conn);
        }

        private void retreive_schema()
        {
            DataTable dt = conn.GetDataTable("select top 0 * from apprequest", null, dbtimeout);
            staticFramework.retrieveschema(dt, productid);
            staticFramework.retrieveschema(dt, purpose);
            staticFramework.retrieveschema(dt, branchid);
            staticFramework.retrieveschema(dt, cust_name);
            staticFramework.retrieveschema(dt, ktp);
            staticFramework.retrieveschema(dt, pob);
            staticFramework.retrieveschema(dt, npwp);
            staticFramework.retrieveschema(dt, homeaddress);
            staticFramework.retrieveschema(dt, phonenumber);
            staticFramework.retrieveschema(dt, gender);
            staticFramework.retrieveschema(dt, mother_name);
        }

        private void retrieve_data()
        {
            DataTable dt = conn.GetDataTable("select * from vw_apprequest where requestid = @1",
                new object[] { Request.QueryString["requestid"] }, dbtimeout);
            staticFramework.retrieve(dt, requestid);
            staticFramework.retrieve(dt, productid);
            staticFramework.retrieve(dt, purpose);
            staticFramework.retrieve(dt, branchid);
            staticFramework.retrieve(dt, cust_type);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, dob);
            staticFramework.retrieve(dt, ktp);
            staticFramework.retrieve(dt, pob);
            staticFramework.retrieve(dt, npwp);
            staticFramework.retrieve(dt, homeaddress);
            staticFramework.retrieve(dt, phonenumber);
            //staticFramework.retrieve(dt, cid);
            staticFramework.retrieve(dt, status_wl);
            staticFramework.retrieve(dt, gender);
            staticFramework.retrieve(dt, mother_name);

            if (cust_type.SelectedValue == "PSH")
            {
                tr_gender.Style["display"] = "none";
                tr_mother_name.Style["display"] = "none";
                npwp.CssClass = "mandatory";
                gender.CssClass = "";
            }
            else
            {
                tr_gender.Style["display"] = "";
                tr_mother_name.Style["display"] = "";
                npwp.CssClass = "";
                gender.CssClass = "mandatory";
            }
        }

        private void retrieve_data_suppl(string key)
        {
            DataTable dt = conn.GetDataTable("select * from apprequestsupp where requestid = @1 and seq = @2",
                new object[] { Request.QueryString["requestid"], key }, dbtimeout);
            staticFramework.retrieve(dt, seq);
            staticFramework.retrieve(dt, supp_cust_type, "supp_");
            staticFramework.retrieve(dt, supp_cust_name, "supp_");
            staticFramework.retrieve(dt, status_app);
            staticFramework.retrieve(dt, supp_dob, "supp_");
            staticFramework.retrieve(dt, supp_ktp, "supp_");
            staticFramework.retrieve(dt, supp_pob, "supp_");
            staticFramework.retrieve(dt, supp_npwp, "supp_");
            staticFramework.retrieve(dt, supp_homeaddress, "supp_");
            staticFramework.retrieve(dt, supp_phonenumber, "supp_");

            staticFramework.retrieve(dt, supp_gender, "supp_");
            staticFramework.retrieve(dt, supp_mother_name, "supp_");

            if (supp_cust_type.SelectedValue == "PSH")
            {
                tr_supp_gender.Style["display"] = "none";
                tr_supp_mother_name.Style["display"] = "none";
                supp_npwp.CssClass = "mandatory";
                supp_gender.CssClass = "";
            }
            else
            {
                tr_supp_gender.Style["display"] = "";
                tr_supp_mother_name.Style["display"] = "";
                supp_npwp.CssClass = "";
                supp_gender.CssClass = "mandatory";
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

        private string genreqid()
        {
            string reqid = "";
            conn.ExecReader("exec sp_gen_requestid", null, dbtimeout);
            if (conn.hasRow()) reqid = conn.GetFieldValue(0);
            return reqid;
        }

        private void savedata()
        {
            if (cust_type.SelectedValue == "PSH")
            {
                gender.Items[0].Selected = false;
                gender.Items[1].Selected = false;
                gender.CssClass = "mandatory";
                mother_name.Text = "";
            }

            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Fields = new NameValueCollection();
            if (Request.QueryString["requestid"] == "")
            {
                Fields["inputdate"] = "getdate()";
                staticFramework.saveNVC(Fields, "inputby", USERID);
                staticFramework.saveNVC(Fields, "reqstatus", "DRF");
            }
            staticFramework.saveNVC(Keys, requestid);
            staticFramework.saveNVC(Fields, productid);
            staticFramework.saveNVC(Fields, purpose);
            staticFramework.saveNVC(Fields, branchid);
            staticFramework.saveNVC(Fields, cust_type);
            staticFramework.saveNVC(Fields, cust_name);
            staticFramework.saveNVC(Fields, dob);
            staticFramework.saveNVC(Fields, ktp);
            staticFramework.saveNVC(Fields, pob);
            staticFramework.saveNVC(Fields, npwp);
            staticFramework.saveNVC(Fields, homeaddress);
            staticFramework.saveNVC(Fields, phonenumber);
            staticFramework.saveNVC(Fields, gender);
            staticFramework.saveNVC(Fields, mother_name);
            staticFramework.save(Fields, Keys, "apprequest", conn);
        }

        private void savedata_suppl()
        {
            if (supp_cust_type.SelectedValue == "PSH")
            {
                supp_gender.Items[0].Selected = false;
                supp_gender.Items[1].Selected = false;
                supp_gender.CssClass = "mandatory";
                supp_mother_name.Text = "";
            }

            NameValueCollection Keys = new NameValueCollection();
            NameValueCollection Fields = new NameValueCollection();
            staticFramework.saveNVC(Keys, requestid);
            staticFramework.saveNVC(Keys, seq);
            staticFramework.saveNVC(Fields, supp_cust_type, "supp_");
            staticFramework.saveNVC(Fields, supp_cust_name, "supp_");
            staticFramework.saveNVC(Fields, status_app);
            staticFramework.saveNVC(Fields, supp_dob, "supp_");
            staticFramework.saveNVC(Fields, supp_ktp, "supp_");
            staticFramework.saveNVC(Fields, supp_pob, "supp_");
            staticFramework.saveNVC(Fields, supp_npwp, "supp_");
            staticFramework.saveNVC(Fields, supp_homeaddress, "supp_");
            staticFramework.saveNVC(Fields, supp_phonenumber, "supp_");
            staticFramework.saveNVC(Fields, supp_gender, "supp_");
            staticFramework.saveNVC(Fields, supp_mother_name, "supp_");

            if (seq.Value != "")
                staticFramework.save(Fields, Keys, "apprequestsupp", conn);
            else
            {
                staticFramework.save(Fields, Keys, "apprequestsupp",
                    "DECLARE @seq INT \n" +
                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM apprequestsupp " +
                    "WHERE requestid='" + requestid.Text + "' \n", conn);
            }
        }

        protected void delete_suppl(string key)
        {
            object[] param = new object[] { requestid.Text, key };
            conn.ExecNonQuery("DELETE FROM apprequestsupp WHERE requestid=@1 AND seq=@2", param, dbtimeout);
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
                btn_aprv2.Visible = true;
                btn_aprv.Visible = false;
            }
            else
            {
                tr_blheader.Visible = false;
                tr_blacklist.Visible = false;
                GridViewBlacklist.Visible = false;
                btn_aprv2.Visible = false;
                btn_aprv.Visible = true;
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                initial_reffrential_parameter();
                retreive_schema();
                if (Request.QueryString["requestid"] != "")
                {
                    retrieve_data();
                    if (Request.QueryString["requestid"] != "")
                    {
                        tr_suppheader.Style.Remove("display");
                        tr_supplement.Style.Remove("display");
                    }
                    gridbind_suppl();
                    gridbindnotes();
                    gridbind_blacklist();
                }
                else
                {
                    if (Session["BranchID"] != null)
                        branchid.SelectedValue = Session["BranchID"].ToString();
                }
                if (Request.QueryString["approved"] == "1")
                {
                    btn_aprv.Visible = false;
                    btn_rjct.Visible = false;
                    btn_save.Visible = false;
                    btn_reverse.Visible = false;
                    btn_interface.Visible = true;
                }
                else
                {
                    btn_aprv.Visible = true;
                    btn_rjct.Visible = true;
                    btn_save.Visible = true;
                    btn_reverse.Visible = true;
                    btn_interface.Visible = false;
                }
                if (cid.Text != "")
                {
                    tbl_cid.Style.Remove("display");
                    btn_interface.Visible = false;
                }
            }
        }

        #region callback
        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "s")
                {
                    try
                    {
                        if (Request.QueryString["mode"] == "new")
                        {
                            requestid.Text = genreqid();
                            mainPanel.JSProperties["cp_redirect"] = "RequestSLIK.aspx?mode=edit&requestid=" + requestid.Text;
                        }
                        savedata();
                        retrieve_data();
                    }
                    catch (Exception ex)
                    {
                        string errmsg = ex.Message;
                        if (errmsg.IndexOf("Last Query") > 0)
                            errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                        mainPanel.JSProperties["cp_alert"] = errmsg;
                    }
                }
                else if (e.Parameter == "a")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "GCC", "APV", "APV", Session["UserID"], null };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    mainPanel.JSProperties["cp_alert"] = "Data permintaan SLIK checking diapprove.";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|GCC&passurl&mntitle=Approval SLIK Checking&li=L|BIC|GCC";
                }
                else if (e.Parameter == "i")
                {
                    string CID, CID1, CID2;
                    CID1 = System.Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0, 4);
                    CID2 = System.Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(System.Guid.NewGuid().ToString().ToUpper().Replace("-", "").Length - 6);
                    CID = CID1 + "-" + CID2;
                    string sql = "exec sp_update_req_to_cms @1,@2,@3";
                    object[] par = new object[] { requestid.Text, CID, Session["UserID"] };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    mainPanel.JSProperties["cp_alert"] = "Data telah berhasil dikirim ke CMS.";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|APV&passurl&mntitle=List Approved&li=L|BIC|APV&readonly=1";
                    retrieve_data();
                    tbl_cid.Style.Remove("display");
                    btn_interface.Visible = false;
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

        protected void PanelComment_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "r")
                    btn_confirm.Attributes["onclick"] = "callback(PanelComment,'reject')";
                else if (e.Parameter == "v")
                    btn_confirm.Attributes["onclick"] = "callback(PanelComment,'reverse')";
                else if (e.Parameter == "a")
                    btn_confirm.Attributes["onclick"] = "callback(PanelComment,'approve')";
                else if (e.Parameter == "reject")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "GCC", "RJC", "RJC", Session["UserID"], comment.Text };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    PanelComment.JSProperties["cp_alert"] = "Data permintaan SLIK checking direject.";
                    PanelComment.JSProperties["cp_target"] = "_parent";
                    PanelComment.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|GCC&passurl&mntitle=Approval SLIK Checking&li=L|BIC|GCC";

                }
                else if (e.Parameter == "reverse")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "GCC", "DRF", "RVS", Session["UserID"], comment.Text };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    PanelComment.JSProperties["cp_alert"] = "Data permintaan SLIK checking direverse.";
                    PanelComment.JSProperties["cp_target"] = "_parent";
                    PanelComment.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|GCC&passurl&mntitle=Approval SLIK Checking&li=L|BIC|GCC";

                }
                else if (e.Parameter == "approve")
                {
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "GCC", "APV", "APV", Session["UserID"], comment.Text };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    PanelComment.JSProperties["cp_alert"] = "Data permintaan SLIK checking diapprove.";
                    PanelComment.JSProperties["cp_target"] = "_parent";
                    PanelComment.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|GCC&passurl&mntitle=Approval SLIK Checking&li=L|BIC|GCC";
                }

            }
            catch (Exception ex)
            {
                string errmsg = "";
                if (ex.Message.IndexOf("Last Query:") > 0)
                    errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                else
                    errmsg = ex.Message;
                PanelComment.JSProperties["cp_alert"] = errmsg;
            }
        }

        protected void PanelSID_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                retrieve_data_suppl(e.Parameter.Substring(2));
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                savedata_suppl();
                gridbind_suppl();
            }
        }

        protected void GridViewSuppl_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                delete_suppl(e.Parameters.Substring(2));
                gridbind_suppl();
            }
        }

        protected void GridViewSuppl_Load(object sender, EventArgs e)
        {
            gridbind_suppl();
            if (Request.QueryString["readonly"] != null)
            {
                ModuleSupport.DisableControls(this, allowViewState);
                GridViewSuppl.Columns[7].Visible = false;
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