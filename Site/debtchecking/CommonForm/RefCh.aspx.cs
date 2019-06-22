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

using DMS.Tools;

namespace DebtChecking.CommonForm
{
    public partial class RefCh : System.Web.UI.Page
    {
        private int dbtimeout;
        private string ConnString;
        private DbConnection conn;
        private string clientmsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["e"] == "0")
                {
                    CODE.Enabled = false;
                    BTN.Disabled = true;
                }
                if (Request.QueryString["c"] != null)
                    CODE.CssClass = Request.QueryString["c"];
                if (Request.QueryString["t"] != null)
                    CODE.TabIndex = short.Parse(Request.QueryString["t"]);

                //fillreflist
                string qrystr = "CtrlID=" + CODE.ClientID + "&CtrlDesc=" + DESC.ClientID;
                if (Request.QueryString["idTx"] != null)
                    qrystr += "&idTx=" + Request.QueryString["idTx"];
                if (Request.QueryString["deTx"] != null)
                    qrystr += "&deTx=" + Request.QueryString["deTx"];
                if (Request.QueryString["qry"] != null)
                {
                    qrystr += "&qry=" + Request.QueryString["qry"];
                    _query = HttpUtility.UrlDecode(Request.QueryString["qry"]).Replace("|:|", "'");
                }
                if (Request.QueryString["tbl"] != null)
                {
                    qrystr += "&tbl=" + Request.QueryString["tbl"];
                    _tblname = Request.QueryString["tbl"];
                }
                if (Request.QueryString["fid"] != null)
                {
                    qrystr += "&fid=" + Request.QueryString["fid"];
                    _fldid = Request.QueryString["fid"];
                }
                if (Request.QueryString["fdesc"] != null)
                {
                    qrystr += "&fdesc=" + Request.QueryString["fdesc"];
                    _flddesc = Request.QueryString["fdesc"];
                }
                if (Request.QueryString["cond"] != null)
                {
                    qrystr += "&cond=" + Request.QueryString["cond"];
                    _cond = HttpUtility.UrlDecode(Request.QueryString["cond"]).Replace("|:|", "'");
                }
                if (Request.QueryString["sort"] != null)
                {
                    qrystr += "&sort=" + Request.QueryString["sort"];
                    _orderby = HttpUtility.UrlDecode(Request.QueryString["sort"]);
                }

                SearchBtnAttribute(qrystr, Request.QueryString["fT"]);

                if (Request.QueryString["sv"] != null)
                    SelectedValue(Request.QueryString["sv"]);
                else if (Request.QueryString["si"] != null)
                    SelectedIndex(int.Parse(Request.QueryString["si"]));
            }
        }

        private void SearchBtnAttribute(string querystring, string title)
        {
            querystring += "&fr=1";
            BTN.Attributes.Add("onclick", "javascript:window.open('../CommonForm/RefSearch.aspx?" +
                querystring + "', '" + title + "', 'status=no,scrollbars=no,width=450,height=250')");

            //window.showModelessDialog("SMLD_target.htm","Dialog Box Arguments # 1","dialogHeight: 250px; dialogWidth: 400px; dialogTop: 155px; dialogLeft: 381px; edge: Sunken; center: Yes; help: No; resizable: No; status: No;"); 
            //BTN.Attributes.Add("onclick", "javascript:window.showModalDialog('../CommonForm/RefSearch.aspx?" + 
            //	querystring + "', '', 'dialogHeight: 250px; dialogWidth: 450px; edge: Raised; center: Yes; resizable: No; status: No;')");
        }

        #region Property
        public bool AutoPostBack
        {
            set { ViewState["autopostback"] = value; }
            get { if (ViewState["autopostback"] == null) return false; return (bool)ViewState["autopostback"]; }
        }
        private string _oldvalue
        {
            get { return (string)ViewState["oldvalue"]; }
            set { ViewState["oldvalue"] = value; }
        }
        #endregion

        #region fillRefList variables
        // need to ViewState these variables as it is probably needed back between requests in non-ddl mode
        private string _query
        {
            get { return (string)ViewState["query"]; }
            set { ViewState["query"] = value; }
        }
        private string _tblname
        {
            get { return (string)ViewState["tblname"]; }
            set { ViewState["tblname"] = value; }
        }
        private string _fldid
        {
            get { return (string)ViewState["fldid"]; }
            set { ViewState["fldid"] = value; }
        }
        private string _flddesc
        {
            get { return (string)ViewState["flddesc"]; }
            set { ViewState["flddesc"] = value; }
        }
        private string _cond
        {
            get { return (string)ViewState["cond"]; }
            set { ViewState["cond"] = value; }
        }
        private string _orderby
        {
            get { return (string)ViewState["orderby"]; }
            set { ViewState["orderby"] = value; }
        }
        #endregion

        #region user interaction
        private void SelectedIndex(int val)
        {
            if (val == 0)
            {
                CODE.Text = "";
                DESC.Text = "";
                return;
            }
            dbtimeout = (int)Session["dbTimeOut"];
            ConnString = (string)Session["ConnString"];
            using (conn = new DbConnection(ConnString))
            {
                string qry = _query;
                if (qry == null || qry.Trim() == "")
                {
                    qry = "select " + _fldid + ", " + _flddesc + " from " + _tblname;
                    if (_cond != null && _cond.Trim() != "")
                        qry += " where (" + _cond + ") ";
                    if (_orderby != null && _orderby.Trim() != "")
                        qry += " order by " + _orderby + " ";
                }
                ListItemCollection items = new ListItemCollection();
                MyPage.fillRefList(items, qry, null, dbtimeout, false, conn);
                CODE.Text = items[val].Value;
                DESC.Text = items[val].Text;
            }
        }
        private void SelectedValue(string val)
        {
            if (val.Trim() == "" || val.Trim() == "&nbsp;")
            {
                CODE.Text = "";
                DESC.Text = "";
                return;
            }
            dbtimeout = (int)Session["dbTimeOut"];
            ConnString = (string)Session["ConnString"];
            using (conn = new DbConnection(ConnString))
            {
                if (_query == null || _query.Trim() == "")
                {
                    string qry = "select " + _fldid + ", " + _flddesc + " from " + _tblname;
                    if (_cond != null && _cond.Trim() != "")
                        qry += " where (" + _cond + ") and " + _fldid + " = '" + val + "'";
                    else
                        qry += " where " + _fldid + " = '" + val + "'";
                    conn.ExecReader(qry, null, dbtimeout);
                    if (conn.hasRow())
                    {
                        CODE.Text = conn.GetFieldValue(0);
                        DESC.Text = conn.GetFieldValue(1);
                    }
                }
                else
                {
                    ListItemCollection items = new ListItemCollection();
                    MyPage.fillRefList(items, _query, null, dbtimeout, false, conn);
                    CODE.Text = "";
                    DESC.Text = "";
                    for (int i = 0; i < items.Count; i++)
                        if (items[i].Value == val)
                        {
                            CODE.Text = items[i].Value;
                            DESC.Text = items[i].Text;
                            break;
                        }
                }
            }
        }
        #endregion

        protected void CODE_TextChanged(object sender, EventArgs e)
        {
            if (CODE.Text.Trim() == "")
            {
                CODE.Text = "";
                DESC.Text = "";
                return;
            }

            dbtimeout = (int)Session["dbTimeOut"];
            ConnString = (string)Session["ConnString"];
            //WebControl ctrl;
            using (conn = new DbConnection(ConnString))
            {
                if (_query == null || _query.Trim() == "")
                {
                    string qry = "select " + _fldid + ", " + _flddesc + " from " + _tblname;
                    if (_cond != null && _cond.Trim() != "")
                        qry += " where (" + _cond + ") and " + _fldid + " = '" + CODE.Text + "'";
                    else
                        qry += " where " + _fldid + " = '" + CODE.Text + "'";
                    conn.ExecReader(qry, null, dbtimeout);
                    if (conn.hasRow())
                    {
                        DESC.Text = conn.GetFieldValue(1);
                        //ctrl = CommonForm.ModuleSupport.NextCtrl(this.Parent.Page, CODE);
                        //if (ctrl != null)
                        //	MyPage.SetFocus(this, ctrl);
                    }
                    else
                    {
                        CODE.Text = "";
                        DESC.Text = "";
                        clientmsg = "Kode tidak ditemukan";
                        MyPage.SetFocus(this, CODE);
                    }
                }
                else
                {
                    DropDownList DDL = new DropDownList();
                    MyPage.fillRefList(DDL.Items, _query, null, dbtimeout, false, conn);
                    try
                    {
                        DDL.SelectedValue = CODE.Text;
                        DESC.Text = DDL.SelectedItem.Text;
                        //ctrl = CommonForm.ModuleSupport.NextCtrl(this.Parent.Page, CODE);
                        //if (ctrl != null)
                        //	MyPage.SetFocus(this, ctrl);
                    }
                    catch
                    {
                        CODE.Text = "";
                        DESC.Text = "";
                        clientmsg = "Kode tidak ditemukan";
                        MyPage.SetFocus(this, CODE);
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script = "";
            if (clientmsg.Trim() != "")
            {
                clientmsg = clientmsg.Replace("\\", "\\\\").Replace("\r", "").Replace("\n", "\\n").Replace("'", "");
                clientmsg = clientmsg.Replace("\\\\n", "\\n");
                script += "alert('" + clientmsg + "');";
            }
            if (CODE.Text.Trim() != "" && CODE.Text != _oldvalue)
            {
                _oldvalue = CODE.Text;
                if (Request.QueryString["PostBack"] == "1")
                    DESC.Text = "__dopostback__";
            }
            script += "refupd('" + Request.QueryString["Cid"] + "', '" + Request.QueryString["Cde"] + "', '" +
                CODE.Text.Trim() + "', '" + DESC.Text.Trim() + "');";
            Response.Write("<script for=window event=onload language='JavaScript'>" + script + "</script>");
        }
    }
}
