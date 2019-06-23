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
    public partial class RequestSLIK : DataPage
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
            staticFramework.reff(purpose, "select * FROM rfrequestpurpose", null, conn);
            staticFramework.reff(branchid, "select * FROM rfbranch", null, conn);
            staticFramework.reff(productid, "select * FROM rfproduct order by kode_idreg", null, conn);
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
            staticFramework.retrieveschema(dt, homecity);
            staticFramework.retrieveschema(dt, phonenumber);
            staticFramework.retrieveschema(dt, gender);
            staticFramework.retrieveschema(dt, mother_name);
        }

        private void retrieve_data()
        {
            DataTable dt = conn.GetDataTable("select * from apprequest where requestid = @1",
                new object[] { Request.QueryString["requestid"] }, dbtimeout);
            staticFramework.retrieve(dt, requestid);
            staticFramework.retrieve(dt, productid);
            staticFramework.retrieve(dt, purpose);
            staticFramework.retrieve(dt, branchid);
            staticFramework.retrieve(dt, cust_type);
            staticFramework.retrieve(dt, nationality);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, dob);
            staticFramework.retrieve(dt, ktp);
            staticFramework.retrieve(dt, pob);
            staticFramework.retrieve(dt, npwp);
            staticFramework.retrieve(dt, homeaddress);
            staticFramework.retrieve(dt, homecity);
            staticFramework.retrieve(dt, phonenumber);
            staticFramework.retrieve(dt, gender);
            staticFramework.retrieve(dt, marital_status);
            staticFramework.retrieve(dt, mother_name);

            if (cust_type.SelectedValue == "PSH")
            {
                tr_gender.Style["display"] = "none";
                tr_mother_name.Style["display"] = "none";
                tr_nationality.Style["display"] = "none";
                tr_marital.Style["display"] = "none";
                npwp.CssClass = "mandatory";
                gender.CssClass = "";
                ktp.CssClass = "";
            }
            else
            {
                tr_gender.Style["display"] = "";
                tr_mother_name.Style["display"] = "";
                tr_nationality.Style["display"] = "";
                tr_marital.Style["display"] = "";
                npwp.CssClass = "";
                gender.CssClass = "mandatory";
                ktp.CssClass = "mandatory";
            }
        }

        private void retrieve_data_suppl(string key)
        {
            DataTable dt = conn.GetDataTable("select * from apprequestsupp where requestid = @1 and seq = @2",
                new object[] { Request.QueryString["requestid"], key }, dbtimeout);
            staticFramework.retrieve(dt, seq);
            staticFramework.retrieve(dt, supp_cust_type, "supp_");
            staticFramework.retrieve(dt, supp_nationality, "supp_");
            staticFramework.retrieve(dt, supp_cust_name, "supp_");
            staticFramework.retrieve(dt, status_app);
            staticFramework.retrieve(dt, supp_dob, "supp_");
            staticFramework.retrieve(dt, supp_ktp, "supp_");
            staticFramework.retrieve(dt, supp_pob, "supp_");
            staticFramework.retrieve(dt, supp_npwp, "supp_");
            staticFramework.retrieve(dt, supp_homeaddress, "supp_");
            staticFramework.retrieve(dt, supp_homecity, "supp_");
            staticFramework.retrieve(dt, supp_phonenumber, "supp_");
            staticFramework.retrieve(dt, supp_gender, "supp_");
            staticFramework.retrieve(dt, supp_mother_name, "supp_");

            if (supp_cust_type.SelectedValue == "PSH")
            {
                tr_supp_gender.Style["display"] = "none";
                tr_supp_mother_name.Style["display"] = "none";
                tr_supp_nationality.Style["display"] = "none";
                supp_npwp.CssClass = "mandatory";
                supp_gender.CssClass = "";
            }
            else
            {
                tr_supp_gender.Style["display"] = "";
                tr_supp_mother_name.Style["display"] = "";
                tr_supp_nationality.Style["display"] = "";
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
            staticFramework.saveNVC(Fields, nationality);
            staticFramework.saveNVC(Fields, cust_name);
            staticFramework.saveNVC(Fields, dob);
            staticFramework.saveNVC(Fields, ktp);
            staticFramework.saveNVC(Fields, pob);
            staticFramework.saveNVC(Fields, npwp);
            staticFramework.saveNVC(Fields, homeaddress);
            staticFramework.saveNVC(Fields, homecity);
            staticFramework.saveNVC(Fields, phonenumber);
            staticFramework.saveNVC(Fields, gender);
            staticFramework.saveNVC(Fields, marital_status);
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
            staticFramework.saveNVC(Fields, supp_nationality, "supp_");
            staticFramework.saveNVC(Fields, supp_cust_name, "supp_");
            staticFramework.saveNVC(Fields, status_app);
            staticFramework.saveNVC(Fields, supp_dob, "supp_");
            staticFramework.saveNVC(Fields, supp_ktp, "supp_");
            staticFramework.saveNVC(Fields, supp_pob, "supp_");
            staticFramework.saveNVC(Fields, supp_npwp, "supp_");
            staticFramework.saveNVC(Fields, supp_homeaddress, "supp_");
            staticFramework.saveNVC(Fields, supp_homecity, "supp_");
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
                tbl_history.Style.Remove("display");
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
                        tr_submit.Style.Remove("display");
                        btn_del.Style.Remove("display");
                    }
                    gridbind_suppl();
                    gridbindnotes();
                }
                if (Session["BranchID"] != null && Session["BranchID"].ToString() != "999" && Session["BranchID"].ToString() != "")
                {
                    branchid.SelectedValue = Session["BranchID"].ToString();
                    branchid.Enabled = false;
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
                        mainPanel.JSProperties["cp_alert"] = "Data telah tersimpan.";
                    }
                    catch (Exception ex)
                    {
                        string errmsg = ex.Message;
                        if (errmsg.IndexOf("Last Query") > 0)
                            errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                        mainPanel.JSProperties["cp_alert"] = errmsg;
                    }
                }
                else if (e.Parameter == "d")
                {
                    object[] param = new object[] { Request.QueryString["requestid"] };
                    conn.ExecNonQuery("DELETE FROM apprequestsupp WHERE requestid=@1", param, dbtimeout);
                    conn.ExecNonQuery("DELETE FROM apprequest WHERE requestid=@1", param, dbtimeout);
                    mainPanel.JSProperties["cp_alert"] = "Data permintaan SLIK checking berhasil dihapus.";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|REQ&passurl&mntitle=Request SLIK Checking&li=L|BIC|REQ";

                }
                else if (e.Parameter == "u")
                {
                    savedata();
                    string sql = "exec sp_update_request @1,@2,@3,@4,@5,@6";
                    object[] par = new object[] { requestid.Text, "DRF", "BMA", "SBT", Session["UserID"], null };
                    conn.ExecNonQuery(sql, par, dbtimeout);
                    mainPanel.JSProperties["cp_alert"] = "Data permintaan SLIK checking berhasil disubmit.";
                    mainPanel.JSProperties["cp_target"] = "_parent";
                    mainPanel.JSProperties["cp_redirect"] = "../ScreenMenu.aspx?sm=BIC|REQ&passurl&mntitle=Request SLIK Checking&li=L|BIC|REQ";
                }
                else if (e.Parameter.StartsWith("g:"))
                {
                    string key = e.Parameter.Substring(2);
                    DataTable dt = conn.GetDataTable("select * from vw_apprequest_all where requestid = @1", new object[] { key }, dbtimeout);
                    if (dt.Rows.Count==0)
                    {
                        dt = conn.GetDataTable("select * from vw_whitelist_all where whitelistid = @1", new object[] { key }, dbtimeout);
                    }
                    
                    cust_name.Text = dt.Rows[0]["cust_name"].ToString();
                    ktp.Text = dt.Rows[0]["ktp"].ToString();
                    npwp.Text = dt.Rows[0]["npwp"].ToString();
                    cust_type.SelectedValue = dt.Rows[0]["cust_type"].ToString();
                    pob.Text = dt.Rows[0]["pob"].ToString();
                    homeaddress.Text = dt.Rows[0]["homeaddress"].ToString();
                    phonenumber.Text = dt.Rows[0]["phonenumber"].ToString();
                    dob.Text = dt.Rows[0]["dob"].ToString();
                    try
                    {
                        gender.SelectedValue = dt.Rows[0]["gender"].ToString();
                    }
                    catch (Exception)
                    {

                    }
                    mother_name.Text = dt.Rows[0]["mother_name"].ToString();
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
            staticFramework.reff(status_app, "select * FROM rfrelationbic where cust_type = @1",
                    new object[] { cust_type.SelectedValue }, conn);
            if (e.Parameter.StartsWith("r:"))
            {
                retrieve_data_suppl(e.Parameter.Substring(2));
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                savedata_suppl();
                gridbind_suppl();
            }
            else if (e.Parameter.StartsWith("gp:"))
            {
                string key = e.Parameter.Substring(3);
                DataTable dt = conn.GetDataTable("select * from vw_apprequest_all where requestid = @1", new object[] { key }, dbtimeout);
                if (dt.Rows.Count == 0)
                {
                    dt = conn.GetDataTable("select * from vw_whitelist_all where whitelistid = @1", new object[] { key }, dbtimeout);
                }

                supp_cust_name.Text = dt.Rows[0]["cust_name"].ToString();
                supp_ktp.Text = dt.Rows[0]["ktp"].ToString();
                supp_npwp.Text = dt.Rows[0]["npwp"].ToString();
                supp_cust_type.SelectedValue = dt.Rows[0]["cust_type"].ToString();
                supp_pob.Text = dt.Rows[0]["pob"].ToString();
                supp_homeaddress.Text = dt.Rows[0]["homeaddress"].ToString();
                supp_phonenumber.Text = dt.Rows[0]["phonenumber"].ToString();
                supp_dob.Value = dt.Rows[0]["dob"].ToString();
                try
                {
                    supp_gender.SelectedValue = dt.Rows[0]["gender"].ToString();
                }
                catch (Exception) { }
                supp_mother_name.Text = dt.Rows[0]["mother_name"].ToString();
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
                ModuleSupport.DisableControls(this, allowViewState);
        }

        protected void PNL_FindExisting_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("n"))
            {
                find_name.Text = "";
                find_name.Focus();
                find_reqid.Text = "";
                string qnull = "select * from vw_apprequest_all where 1=0";
                DataTable dtnull = conn.GetDataTable(qnull, null, dbtimeout);
                dtnull.Rows.Add(dtnull.NewRow());
                GridFind.DataSource = dtnull;
                GridFind.DataBind();
                lblnotfound.Visible = false;
                GridFind.Columns[5].Visible = false;
                GridFind.Columns[6].Visible = false;
            }

            if (e.Parameter.StartsWith("n:")) searchsup.Value = "0";
            else if (e.Parameter.StartsWith("np:")) searchsup.Value = "1";
            else if (e.Parameter.StartsWith("f:"))
            {
                try
                {
                    string query = "select * from vw_apprequest_all where cust_name like '%" + find_name.Text + "%' and requestid like '%" + find_reqid.Text + "%'";
                    string queryWhitelist = "select * from vw_whitelist_all where cust_name like '%" + find_name.Text + "%' and whitelistid like '%" + find_reqid.Text + "%'";
                    if (Session["BranchID"] != null && Session["BranchID"].ToString() != "999")
                    {
                        queryWhitelist += " and branch_name = @1";
                    }

                    if (find_source.Text == "WHL")
                    {
                        query = queryWhitelist;
                    }

                    query += " order by inputdate desc";

                    if (find_name.Text == "" && find_reqid.Text == "")
                    {
                        query = "select * from vw_apprequest_all where 1=0";
                    }
                    DataTable dt = conn.GetDataTable(query, new object[] { Session["BranchID"] }, dbtimeout);
                    if (dt.Rows.Count > 0)
                    {
                        GridFind.DataSource = dt;
                        GridFind.DataBind();
                        lblnotfound.Visible = false;
                    }
                    else
                    {
                        dt.Rows.Add(dt.NewRow());
                        GridFind.DataSource = dt;
                        GridFind.DataBind();
                        GridFind.Rows[0].Visible = false;
                        lblnotfound.Visible = true;
                        if (find_name.Text == "" && find_reqid.Text == "") { lblnotfound.Text = ""; }
                        else { lblnotfound.Text = "Data tidak ditemukan"; }
                    }
                    if (searchsup.Value == "1")
                    {
                        GridFind.Columns[5].Visible = false;
                        GridFind.Columns[6].Visible = true;
                    }
                    else
                    {
                        GridFind.Columns[5].Visible = true;
                        GridFind.Columns[6].Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    string errmsg = "";
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        errmsg = ex.Message;
                    PNL_FindExisting.JSProperties["cp_alert"] = errmsg;
                }
            }

        }
        #endregion

    }
}