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


namespace DebtChecking.CommonForm
{
    public partial class RefSearch : MasterPage
    {
        #region Property
        private string qryIdTx
        {
            get { if (Request.QueryString["idTx"] != null) return Request.QueryString["idTx"].Trim(); return ""; }
        }
        private string qryDeTx
        {
            get { if (Request.QueryString["deTx"] != null) return Request.QueryString["deTx"].Trim(); return ""; }
        }
        private string qryRsTx
        {
            get { if (Request.QueryString["rsTx"] != null) return Request.QueryString["rsTx"].Trim(); return ""; }
        }
        private string qryCtrlId
        {
            get { return Request.QueryString["CtrlID"].Trim(); }
        }
        private string qryCtrlDesc
        {
            get { return Request.QueryString["CtrlDesc"].Trim(); }
        }
        private string qryQry
        {
            get { if (Request.QueryString["qry"] != null) return HttpUtility.UrlDecode(Request.QueryString["qry"]).Replace("|:|", "'").Trim(); return ""; }
        }
        private string qryFId
        {
            get { return Request.QueryString["fid"].Trim(); }
        }
        private string qryFDesc
        {
            get { return Request.QueryString["fdesc"].Trim(); }
        }
        private string qryTbl
        {
            get { return Request.QueryString["tbl"].Trim(); }
        }
        private string qryCond
        {
            get { if (Request.QueryString["cond"] != null) return HttpUtility.UrlDecode(Request.QueryString["cond"]).Replace("|:|", "'").Trim(); return ""; }
        }
        private string qrySort
        {
            get { if (Request.QueryString["sort"] != null) return HttpUtility.UrlDecode(Request.QueryString["sort"]).Trim(); return ""; }
        }
        private string qryInitVal
        {
            get { if (Request.QueryString["initval"] != null) return Request.QueryString["initval"].Trim(); return ""; }
        }
        private string qryPreEndCallback
        {
            get { if (Request.QueryString["preendcallback"] != null) return Request.QueryString["preendcallback"].Trim(); return ""; }
        }
        private string qryPostEndCallback
        {
            get { if (Request.QueryString["postendcallback"] != null) return HttpUtility.UrlDecode(Request.QueryString["postendcallback"]).Replace("|:|", "'").Trim(); return ""; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (qryIdTx != "")
                tdcode.InnerText = qryIdTx;
            if (qryDeTx != "")
                tddesc.InnerText = qryDeTx;
            if (qryRsTx != "")
                tdres.InnerText = qryRsTx;
            if (!IsPostBack)
            {
                if (qryInitVal != "")
                {
                    TXT_CODE.Text = qryInitVal;
                    BTN_SEARCH_Click(null, null);
                }
                setwidth();
                DMS.Tools.MyPage.SetFocus(this, TXT_DESC);
            }

            string args = "'" + qryCtrlId + "', '" + qryCtrlDesc + "'";
            ok.Attributes.Add("onclick", "pilih(" + args + ");");
            if (qryPreEndCallback != "")
                args += ", opener." + qryPreEndCallback + "(document.form1.LST_RESULT.value)";
            LST_RESULT.Attributes.Add("ondblclick", "pilih(" + args + ");");
            if (qryPostEndCallback != "")
            {
                Response.Write(
                    "<script for=window event=onunload language='JavaScript'>" +
                    "   if (picked) " +
                    "       opener." + qryPostEndCallback +
                    "</script>");
            }
        }

        private void setwidth()
        {
            string qry = qryQry.ToLower().Trim();
            if (qryQry == "")
                qry = "select " + qryFId + ", " + qryFDesc + " from " + qryTbl;
            if (qry.StartsWith("select"))
                qry = "select top 0 " + qry.Substring(7);
            
            //DataTable dt = conn.GetDataTable(qry, null, dbtimeout, true, true);
            //if (dt.Columns[1].MaxLength > 80)
                //set new width (maybe must use js)
        }

        protected void BTN_SEARCH_Click(object sender, EventArgs e)
        {
            string cond = " where ", qry = "";
            if (qryQry == "")
            {
                LST_RESULT.Items.Clear();
                qry = "select " + qryFId + ", " + qryFDesc +
                    " from " + qryTbl;
                if (qryCond != "")
                    cond += "(" + qryCond + ")";
                if (TXT_CODE.Text.Trim() != "")
                {
                    if (cond != " where ")
                        cond += " AND ";
                    cond += qryFId + " like '" + TXT_CODE.Text + "%' ";
                }
                if (TXT_DESC.Text.Trim() != "")
                {
                    if (cond != " where ")
                        cond += " AND ";
                    cond += qryFDesc + " like '%" + TXT_DESC.Text + "%' ";
                }
                if (qrySort != "")
                    cond += " order by " + qrySort;
                conn.ExecReader(qry + cond, null, dbtimeout);
                while (conn.hasRow())
                {
                    string val = conn.GetFieldValue(0), text = conn.GetFieldValue(1);
                    if (text.IndexOf(val + " - ") < 0)
                        text = val + " - " + text;
                    ListItem li = new ListItem(text, val);
                    LST_RESULT.Items.Add(li);
                }
            }
            else
            {
                LST_RESULT.Items.Clear();
                qry = qryQry;
                conn.ExecReader(qry, null, dbtimeout);
                while (conn.hasRow())
                {
                    string val = conn.GetFieldValue(0), text = conn.GetFieldValue(1);
                    bool include = true;
                    if (TXT_CODE.Text.Trim() != "")
                        if (!val.ToLower().StartsWith(TXT_CODE.Text.ToLower()))
                            include = false;
                    if (TXT_DESC.Text.Trim() != "")
                        if (text.ToLower().IndexOf(TXT_DESC.Text.ToLower()) < 0)
                            include = false;
                    if (!include)
                        continue;
                    if (text.IndexOf(val + " - ") < 0)
                        text = val + " - " + text;
                    ListItem li = new ListItem(text, val);
                    LST_RESULT.Items.Add(li);
                }
            }
            if (LST_RESULT.Items.Count == 1)
                LST_RESULT.SelectedIndex = 0;
        }
    }
}
