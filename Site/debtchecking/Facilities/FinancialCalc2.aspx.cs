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

namespace DebtChecking.Facilities
{
    public partial class FinancialCalc2 : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        private string Q_VW_CCBANK = "exec SP_VW_KREDIT_CCBANK @1";
        private string Q_VW_CCOTHBANK = "exec SP_VW_KREDIT_CCOTHBANK @1";
        private string Q_VW_CARBANK = "exec SP_VW_KREDIT_CARBANK @1";
        private string Q_VW_PLOTHBANK = "exec SP_VW_KREDIT_PLOTHBANK @1";
        private string Q_VW_PLBANK = "exec SP_VW_KREDIT_PLBANK @1";
        private string Q_VW_KMKBANK = "exec SP_VW_KREDIT_KMKBANK @1";
        private string Q_VW_ODRLBANK = "exec SP_VW_KREDIT_ODRLBANK @1";
        private string Q_VW_KMKOTHBANK = "exec SP_VW_KREDIT_KMKOTHBANK @1";
        private string Q_VW_ODRLOTHBANK = "exec SP_VW_KREDIT_ODRLOTHBANK @1";
        private string Q_VW_KPRBANK = "exec SP_VW_KREDIT_KPRBANK @1";
        private string Q_VW_CONSOTHBANK = "exec SP_VW_KREDIT_CONSOTHBANK @1";
        private string Q_INITIATE_FINCAL = "exec SP_INITIATE_FINCAL @1";
        private string Q_VW_FINCAL = "select * from vw_appfincal where appid = @1";
        private static string Q_NAME_LIST = "select appid NM_CODE, cust_name NM_DESC from slik_applicant where idreg = @1";
        #endregion        

        #region initial
        private void initiate_fincal(string key)
        {
            object[] par = new object[] { key };
            conn.ExecuteNonQuery(Q_INITIATE_FINCAL, par, dbtimeout);
        }

        public bool isActive
        {
            get { return !(Session["ApprovalGroup"].ToString() != "1"); }
        }
        #endregion

        #region retrieve
        private void retrieve_debiturinfo(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_SID_DEBITURINFO, par, dbtimeout);
            staticFramework.retrieve(dt, ID_REG);
            object[] par2 = new object[] { ID_REG.Value };
            staticFramework.reff(appid, Q_NAME_LIST, par2, conn);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, BORN_DATE);
            staticFramework.retrieve(dt, STATUS_APP);
            staticFramework.retrieve(dt, KTP_NUM);
            staticFramework.retrieve(dt, ALAMAT_DOM);
            staticFramework.retrieve(dt, TELP_HP);
        }

        private void retrieve_datafincal(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_FINCAL, par, dbtimeout);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, TML_INTERNAL);
            staticFramework.retrieve(dt, TML_EXTERNAL);
            staticFramework.retrieve(dt, TML);
            staticFramework.retrieve(dt, TML_KONSOL_INTERNAL);
            staticFramework.retrieve(dt, TML_KONSOL_EXTERNAL);
            staticFramework.retrieve(dt, TML_KONSOL_TOTAL);
            staticFramework.retrieve(dt, INCOME_RECOGNATION);
            staticFramework.retrieve(dt, PRC_DBR);
        }
        #endregion

        #region databinding
        private void gridbindall(string key)
        {
            gridbind_ccbank(key);
            gridbind_carbank(key);
            gridbind_kprbank(key);
            gridbind_ccothbank(key);
            gridbind_consothbank(key);
            gridbind_plothbank(key);
            gridbind_plbank(key);
            gridbind_odrlbank(key);
            gridbind_odrlothbank(key);
        }

        private void gridbind_ccbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_CCBANK, par, dbtimeout);
            GridCCBank.DataSource = dt;
            GridCCBank.DataBind();
        }
        private void gridbind_ccothbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_CCOTHBANK, par, dbtimeout);
            GridCCOtherBank.DataSource = dt;
            GridCCOtherBank.DataBind();
        }

        private void gridbind_carbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_CARBANK, par, dbtimeout);
            GridCARBank.DataSource = dt;
            GridCARBank.DataBind();
        }
        private void gridbind_plothbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_PLOTHBANK, par, dbtimeout);
            GridPLOtherBank.DataSource = dt;
            GridPLOtherBank.DataBind();
        }
        private void gridbind_plbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_PLBANK, par, dbtimeout);
            GridPLBank.DataSource = dt;
            GridPLBank.DataBind();
        }
        private void gridbind_kprbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_KPRBANK, par, dbtimeout);
            GridKPRBank.DataSource = dt;
            GridKPRBank.DataBind();
        }
        private void gridbind_kmkbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_KMKBANK, par, dbtimeout);
            GridKMKBank.DataSource = dt;
            GridKMKBank.DataBind();
        }
        private void gridbind_kmkothbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_KMKOTHBANK, par, dbtimeout);
            GridKMKOtherBank.DataSource = dt;
            GridKMKOtherBank.DataBind();
        }
        private void gridbind_odrlbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_ODRLBANK, par, dbtimeout);
            GridODRLBank.DataSource = dt;
            GridODRLBank.DataBind();
        }
        private void gridbind_odrlothbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_ODRLOTHBANK, par, dbtimeout);
            GridODRLOtherBank.DataSource = dt;
            GridODRLOtherBank.DataBind();
        }
        private void gridbind_consothbank(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_CONSOTHBANK, par, dbtimeout);
            GridConsOtherBank.DataSource = dt;
            GridConsOtherBank.DataBind();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
                initiate_fincal(Request.QueryString["regno"]);
                gridbindall(Request.QueryString["regno"]);
                initiate_fincal(Request.QueryString["regno"]);
                retrieve_datafincal(Request.QueryString["regno"]);
            }
            if (Session["ApprovalGroup"].ToString() != "1")
                ModuleSupport.DisableControls(this, allowViewState);
        }

        #region gridccbank callback
        protected void GridCCBank_Load(object sender, EventArgs e)
        {
            gridbind_ccbank(appid.SelectedValue);
        }
        protected void GridCCBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridCCBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridCCBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_ccbank(appid.SelectedValue);
        }
        protected void panelCCBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_ccbank);
                staticFramework.retrieve(dt, "limit", limit_ccbank);
                staticFramework.retrieve(dt, "balance", balance_ccbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_ccbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "limit", limit_ccbank);
                    staticFramework.saveNVC(Fields, "balance", balance_ccbank);
                    staticFramework.saveNVC(Fields, "calc_category", "cc_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_ccbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelCCBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelCCBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridplbank callback
        protected void GridPLBank_Load(object sender, EventArgs e)
        {
            gridbind_plbank(appid.SelectedValue);
        }
        protected void GridPLBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridPLBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridPLBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_plbank(appid.SelectedValue);
        }
        protected void panelPLBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_plbank);
                staticFramework.retrieve(dt, "balance", plafond_plbank);
                staticFramework.retrieve(dt, "tenor", tenor_plbank);
                staticFramework.retrieve(dt, "interest", interest_plbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_plbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_plbank);
                    staticFramework.saveNVC(Fields, "interest", interest_plbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_plbank);
                    staticFramework.saveNVC(Fields, "calc_category", "pl_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_plbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelPLBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelPLBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridcarbank callback
        protected void GridCARBank_Load(object sender, EventArgs e)
        {
            gridbind_carbank(appid.SelectedValue);
        }
        protected void GridCARBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridCARBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridCARBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_carbank(appid.SelectedValue);
        }
        protected void panelCARBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_carbank);
                staticFramework.retrieve(dt, "balance", plafond_carbank);
                staticFramework.retrieve(dt, "tenor", tenor_carbank);
                staticFramework.retrieve(dt, "interest", interest_carbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_carbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_carbank);
                    staticFramework.saveNVC(Fields, "interest", interest_carbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_carbank);
                    staticFramework.saveNVC(Fields, "calc_category", "car_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_carbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelCARBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelCARBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridkprbank callback
        protected void GridKPRBank_Load(object sender, EventArgs e)
        {
            gridbind_kprbank(appid.SelectedValue);
        }
        protected void GridKPRBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridKPRBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridKPRBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_kprbank(appid.SelectedValue);
        }
        protected void panelKPRBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_kprbank);
                staticFramework.retrieve(dt, "balance", plafond_kprbank);
                staticFramework.retrieve(dt, "tenor", tenor_kprbank);
                staticFramework.retrieve(dt, "interest", interest_kprbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_kprbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_kprbank);
                    staticFramework.saveNVC(Fields, "interest", interest_kprbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_kprbank);
                    staticFramework.saveNVC(Fields, "calc_category", "kpr_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_kprbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelKPRBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelKPRBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridkmkbank callback
        protected void GridKMKBank_Load(object sender, EventArgs e)
        {
            gridbind_kmkbank(appid.SelectedValue);
        }
        protected void GridKMKBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridKMKBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridKMKBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_kmkbank(appid.SelectedValue);
        }
        protected void panelKMKBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_kmkbank);
                staticFramework.retrieve(dt, "balance", plafond_kmkbank);
                staticFramework.retrieve(dt, "tenor", tenor_kmkbank);
                staticFramework.retrieve(dt, "interest", interest_kmkbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_kmkbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_kmkbank);
                    staticFramework.saveNVC(Fields, "interest", interest_kmkbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_kmkbank);
                    staticFramework.saveNVC(Fields, "calc_category", "kmk_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_kmkbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelKMKBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelKMKBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridodrlbank callback
        protected void GridODRLBank_Load(object sender, EventArgs e)
        {
            gridbind_odrlbank(appid.SelectedValue);
        }
        protected void GridODRLBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridODRLBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridODRLBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_odrlbank(appid.SelectedValue);
        }
        protected void panelODRLBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_odrlbank);
                staticFramework.retrieve(dt, "balance", plafond_odrlbank);
                staticFramework.retrieve(dt, "tenor", tenor_odrlbank);
                staticFramework.retrieve(dt, "interest", interest_odrlbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_odrlbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "limit", plafond_odrlbank);
                    staticFramework.saveNVC(Fields, "interest", interest_odrlbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_odrlbank);
                    staticFramework.saveNVC(Fields, "calc_category", "odrl_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_odrlbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelODRLBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelODRLBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridccotherbank callback
        protected void GridCCOtherBank_Load(object sender, EventArgs e)
        {
            gridbind_ccothbank(appid.SelectedValue);
        }
        protected void GridCCOtherBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridCCOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridCCOtherBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_ccothbank(appid.SelectedValue);
        }
        protected void panelCCOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_ccothbank);
                staticFramework.retrieve(dt, "limit", limit_ccothbank);
                staticFramework.retrieve(dt, "balance", balance_ccothbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_ccothbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "limit", limit_ccothbank);
                    staticFramework.saveNVC(Fields, "balance", balance_ccothbank);
                    staticFramework.saveNVC(Fields, "calc_category", "cc_oth_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_ccothbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelCCOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelCCOtherBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridplotherbank callback
        protected void GridPLOtherBank_Load(object sender, EventArgs e)
        {
            gridbind_plothbank(appid.SelectedValue);
        }
        protected void GridPLOtherBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridPLOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridPLOtherBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_plothbank(appid.SelectedValue);
        }
        protected void panelPLOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_plothbank);
                staticFramework.retrieve(dt, "balance", plafond_plothbank);
                staticFramework.retrieve(dt, "tenor", tenor_plothbank);
                staticFramework.retrieve(dt, "interest", interest_plothbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_plothbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_plothbank);
                    staticFramework.saveNVC(Fields, "interest", interest_plothbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_plothbank);
                    staticFramework.saveNVC(Fields, "calc_category", "pl_oth_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_plothbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelPLOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelPLOtherBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridconsotherbank callback
        protected void GridConsOtherBank_Load(object sender, EventArgs e)
        {
            gridbind_consothbank(appid.SelectedValue);
        }
        protected void GridConsOtherBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridConsOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridConsOtherBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_consothbank(appid.SelectedValue);
        }
        protected void panelConsOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_consothbank);
                staticFramework.retrieve(dt, "balance", plafond_consothbank);
                staticFramework.retrieve(dt, "tenor", tenor_consothbank);
                staticFramework.retrieve(dt, "interest", interest_consothbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_consothbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_consothbank);
                    staticFramework.saveNVC(Fields, "interest", interest_consothbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_consothbank);
                    staticFramework.saveNVC(Fields, "calc_category", "cons_oth_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_consothbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelConsOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelConsOtherBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridkmkotherbank callback
        protected void GridKMKOtherBank_Load(object sender, EventArgs e)
        {
            gridbind_kmkothbank(appid.SelectedValue);
        }
        protected void GridKMKOtherBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridKMKOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridKMKOtherBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_kmkothbank(appid.SelectedValue);
        }
        protected void panelKMKOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_kmkothbank);
                staticFramework.retrieve(dt, "balance", plafond_kmkothbank);
                staticFramework.retrieve(dt, "tenor", tenor_kmkothbank);
                staticFramework.retrieve(dt, "interest", interest_kmkothbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_kmkothbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "balance", plafond_kmkothbank);
                    staticFramework.saveNVC(Fields, "interest", interest_kmkothbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_kmkothbank);
                    staticFramework.saveNVC(Fields, "calc_category", "kmk_oth_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_kmkothbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelKMKOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelKMKOtherBank.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion

        #region gridodrlotherbank callback
        protected void GridODRLOtherBank_Load(object sender, EventArgs e)
        {
            gridbind_odrlothbank(appid.SelectedValue);
        }
        protected void GridODRLOtherBank_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "idi_kredit_calc", conn);
                    GridODRLOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
            }
        }
        protected void GridODRLOtherBank_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_odrlothbank(appid.SelectedValue);
        }
        protected void panelODRLOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                object[] par = new object[] { appid.SelectedValue, key };
                DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
                staticFramework.retrieve(dt, "seq", seq_odrlothbank);
                staticFramework.retrieve(dt, "balance", plafond_odrlothbank);
                staticFramework.retrieve(dt, "tenor", tenor_odrlothbank);
                staticFramework.retrieve(dt, "interest", interest_odrlothbank);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "appid", appid.SelectedValue);
                    staticFramework.saveNVC(Keys, "seq", seq_odrlothbank);
                    NameValueCollection Fields = new NameValueCollection();
                    staticFramework.saveNVC(Fields, "limit", plafond_odrlothbank);
                    staticFramework.saveNVC(Fields, "interest", interest_odrlothbank);
                    staticFramework.saveNVC(Fields, "tenor", tenor_odrlothbank);
                    staticFramework.saveNVC(Fields, "calc_category", "odrl_oth_bank");
                    staticFramework.saveNVC(Fields, "inputby", USERID);
                    Fields["inputdate"] = "GETDATE()";
                    if (seq_odrlothbank.Value == "")
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc",
                            "DECLARE @seq INT \n" +
                            "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
                            "WHERE appid='" + appid.SelectedValue + "' \n", conn);
                    }
                    else
                    {
                        staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
                    }
                    panelODRLOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    panelODRLOtherBank.JSProperties["cp_alert"] = errmsg;
                }
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
                    object[] par = new object[] { INCOME_RECOGNATION.Value, ID_REG.Value };
                    conn.ExecNonQuery("update appfincal set income_recognation = @1 where appid in (select appid from slik_applicant where idreg = @2)", par, dbtimeout);
                    initiate_fincal(appid.SelectedValue);
                    gridbindall(appid.SelectedValue);
                    retrieve_datafincal(appid.SelectedValue);
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
            else if (e.Parameter == "r:")
            {
                retrieve_debiturinfo(appid.SelectedValue);
                initiate_fincal(appid.SelectedValue);
                gridbindall(appid.SelectedValue);
                initiate_fincal(appid.SelectedValue);
                retrieve_datafincal(appid.SelectedValue);
            }
        }
        #endregion

    }
}
