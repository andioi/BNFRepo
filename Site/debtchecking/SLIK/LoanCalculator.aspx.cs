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

namespace DebtChecking.SLIK
{
    public partial class LoanCalculator : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        private string Q_VW_LENDING = "select * from vw_appfincal_lending where reffnumber =@1 and category = @2 order by seq";
        private string Q_VW_CCBANK = "exec SP_VW_CALCLENDING_CC @1";
        private string Q_VW_PLBANK = "exec SP_VW_KREDIT_PLBANK @1";
        private string Q_VW_CCOTHBANK = "exec SP_VW_KREDIT_CCOTHBANK @1";
        private string Q_VW_CCOTHBANK_NONSCB = "exec SP_VW_KREDIT_CCOTHBANK_NONSCB @1";
        private string Q_VW_CCOTHBANK_SCB = "exec SP_VW_KREDIT_CCOTHBANK_SCB @1";
        private string Q_VW_CARBANK = "exec SP_VW_KREDIT_CARBANK @1";
        private string Q_VW_PLOTHBANK = "exec SP_VW_KREDIT_PLOTHBANK @1";
        private string Q_VW_KPRBANK = "exec SP_VW_KREDIT_KPRBANK @1";
        private string Q_VW_CONSOTHBANK = "exec SP_VW_KREDIT_CONSOTHBANK @1";
        private string Q_LOAD_LENDINGDATA = "exec SP_CALC_LOADLENDING @1";
        private string Q_VW_FINCAL = "select * from vw_appfincal where reffnumber in (select reffnumber from slik_applicant where appid = @1)";
        DataSet dsMMUE = null;
        #endregion        

        #region initial

        private void load_lendingdata(string key)
        {
            object[] par = new object[] { key };
            //conn.ExecReader("select top 1 reffnumber from slik_applicant where appid = @1", par, dbtimeout);
            //if (conn.hasRow())
            //{
            //    string reff = conn.GetFieldValue("reffnumber");
            //    reffnumber.Value = reff;
                //object[] par2 = new object[] { reff };
                conn.ExecuteNonQuery(Q_LOAD_LENDINGDATA, par, dbtimeout);
                conn.ExecNonQuery("exec SP_CALC_BICSUMMARY @1", par, dbtimeout);
            //}
        }

        public bool isActive
        {
            get { return !(Session["ApprovalGroup"].ToString() != "1"); }
        }
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
            staticFramework.retrieve(dt, genderdesc);
            staticFramework.retrieve(dt, full_ktpaddress);
            staticFramework.retrieve(dt, final_policy);
        }
        
        private void retrieve_bisummary(string key)
        {
            DataTable dt = conn.GetDataTable("select * from vw_appfincal_bicsummary where reffnumber = @1", new object[] { key }, dbtimeout);
            DataTable dt2 = conn.GetDataTable("select * from vw_appfincal_summary where reffnumber = @1", new object[] { key }, dbtimeout);
            dsMMUE = new DataSet();
            dsMMUE.Tables.Add(dt);
            dsMMUE.Tables.Add(dt2);
        }

        #endregion

        #region databinding
        private void gridbindall()
        {
            gridbind_lending(GridCC, "CC");
            gridbind_lending(GridUnsecured, "UL");
            gridbind_lending(GridSecured, "SL");
            gridbind_lending(GridMK, "MKI");
            gridbind_lending(GridInvest, "INV");
            gridbind_lending(GridRK, "PRK");
        }
    
        private void gridbind_lending(DevExpress.Web.ASPxGridView Grid, string category)
        {
            string key = appid.Value;
            object[] par = new object[] { key, category };
            DataTable dt = conn.GetDataTable(Q_VW_LENDING, par, dbtimeout);
            Grid.DataSource = dt;
            Grid.DataBind();
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
                load_lendingdata(appid.Value);
                gridbindall();
                retrieve_bisummary(appid.Value);
            }
            else
            {
                retrieve_debiturinfo(ddl_appid.SelectedValue);
                load_lendingdata(appid.Value);
                gridbindall();
                retrieve_bisummary(appid.Value);
            }
        }

        #region gridcc callback
        protected void GridCC_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridCC, "CC");
        }
        protected void GridCC_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Value);
                    staticFramework.saveNVC(Keys, "category", "CC");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridCC.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridCC_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridCC, "CC");
        }
        //protected void panelCCBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_ccbank);
        //        staticFramework.retrieve(dt, "limit", limit_ccbank);
        //        staticFramework.retrieve(dt, "balance", balance_ccbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_ccbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "limit", limit_ccbank);
        //            staticFramework.saveNVC(Fields, "balance", balance_ccbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "cc_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_ccbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n",conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelCCBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelCCBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridunsecured callback
        protected void GridUnsecured_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridUnsecured, "UL");
        }
        protected void GridUnsecured_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Value);
                    staticFramework.saveNVC(Keys, "category", "UL");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridUnsecured.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridUnsecured_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridUnsecured, "UL");
        }
        //protected void panelPLBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_plbank);
        //        staticFramework.retrieve(dt, "balance", plafond_plbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_plbank);
        //        staticFramework.retrieve(dt, "interest", interest_plbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_plbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", plafond_plbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_plbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_plbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "pl_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_plbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelPLBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelPLBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridsecured callback
        protected void GridSecured_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridSecured, "SL");
        }
        protected void GridSecured_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Value);
                    staticFramework.saveNVC(Keys, "category", "SL");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridSecured.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridSecured_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridSecured, "SL");
        }
        //protected void panelCARBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_carbank);
        //        staticFramework.retrieve(dt, "balance", plafond_carbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_carbank);
        //        staticFramework.retrieve(dt, "interest", interest_carbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_carbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", plafond_carbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_carbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_carbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "car_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_carbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelCARBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelCARBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridmodalkerja callback
        protected void GridMK_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridMK, "MKI");
        }
        protected void GridMK_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Value);
                    staticFramework.saveNVC(Keys, "category", "MKI");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridMK.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridMK_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridMK, "MKI");
        }

        //protected void panelKPRBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_kprbank);
        //        staticFramework.retrieve(dt, "balance", limit_kprbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_kprbank);
        //        staticFramework.retrieve(dt, "interest", interest_kprbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_kprbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", limit_kprbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_kprbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_kprbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "kpr_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_kprbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelKPRBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelKPRBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridinvest callback
        protected void GridInvest_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridInvest, "INV");
        }
        protected void GridInvest_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Value);
                    staticFramework.saveNVC(Keys, "category", "INV");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridInvest.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridInvest_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridInvest, "INV");
        }
        //protected void panelCCOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_ccothbank);
        //        staticFramework.retrieve(dt, "limit", limit_ccothbank);
        //        staticFramework.retrieve(dt, "balance", balance_ccotherbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_ccothbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "limit", limit_ccothbank);
        //            staticFramework.saveNVC(Fields, "balance", balance_ccotherbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "cc_oth_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_ccothbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelCCOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelCCOtherBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridprk callback
        protected void GridRK_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridRK, "PRK");
        }
        protected void GridRK_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Value);
                    staticFramework.saveNVC(Keys, "category", "PRK");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridRK.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridRK_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridRK, "PRK");
        }
        //protected void panelPLOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_plothbank);
        //        staticFramework.retrieve(dt, "balance", plafond_plothbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_plothbank);
        //        staticFramework.retrieve(dt, "interest", interest_plothbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_plothbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", plafond_plothbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_plothbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_plothbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "pl_oth_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_plothbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelPLOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelPLOtherBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
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

