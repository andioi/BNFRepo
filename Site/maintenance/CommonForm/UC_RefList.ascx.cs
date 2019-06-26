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

using My.Tools;

namespace MikroMnt.CommonForm
{
    public partial class UC_RefList : System.Web.UI.UserControl
    {
        private int _maxItems = 30;
        private int dbtimeout;
        private string ConnString;
        private DbConnection conn;
        private string _frsrc = "../CommonForm/RefCh.aspx?";
        private bool _Enabled = true;
        private string _CssClass;
        private System.Web.UI.WebControls.Unit _Width = 0;
        private short _TabIndex = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_ddlmode)
            {
                d1.Visible = false;
                d2.Visible = false;
                CODE.EnableViewState = false;
                DESC.EnableViewState = false;
            }
            else if (_frmmode)
            {
                DDL.EnableViewState = false;
                DDL.Visible = false;
                d1.Visible = true;
                d2.Visible = false;
                CODE.EnableViewState = true;
                DESC.EnableViewState = true;
            }
            else
            {
                DDL.EnableViewState = false;
                DDL.Visible = false;
                d1.Visible = false;
                d2.Visible = true;
                CODE.EnableViewState = true;
                DESC.EnableViewState = true;
            }
            if (!IsPostBack)
            {
                if (_frmmode)
                {
                    _frsrc += "&Cid=" + cd.ClientID + "&Cde=" + de.ClientID;
                    if (AutoPostBack)
                        _frsrc += "&PostBack=1";
                    fr.Attributes.Add("src", _frsrc);
                }
            }
            else
            {
                if (ViewState["maxItems"] != null)
                    _maxItems = (int)ViewState["maxItems"];
                if (_frmmode)
                {
                    if (de.Value == "__dopostback__")
                    {
                        CD_TextChanged(this, null);
                        return;
                    }
                    if (_frsrc != "../CommonForm/RefCh.aspx?")
                    {
                        _frsrc += "&Cid=" + cd.ClientID + "&Cde=" + de.ClientID;
                        if (AutoPostBack)
                            _frsrc += "&PostBack=1";
                        fr.Attributes.Add("src", _frsrc);
                    }
                }
            }
        }

        #region uc events
        protected void DDL_REF_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AutoPostBack && _oldvalue != DDL.SelectedValue)
            {
                _oldvalue = DDL.SelectedValue;
                OnSelectedIndexChanged(e);
            }
        }

        private void CD_TextChanged(object sender, System.EventArgs e)
        {
            if (AutoPostBack && _oldvalue != cd.Value)
            {
                _oldvalue = cd.Value;
                OnSelectedIndexChanged(e);
            }
        }

        protected void CODE_TextChanged(object sender, System.EventArgs e)
        {
            if (CODE.Text.Trim() == "")
            {
                CODE.Text = "";
                DESC.Text = "";
                if (AutoPostBack && _oldvalue != CODE.Text)
                {
                    _oldvalue = CODE.Text;
                    OnSelectedIndexChanged(e);
                }
                return;
            }

            dbtimeout = (int)Session["dbTimeOut"];
            ConnString = (string)ConfigurationSettings.AppSettings["connString"].ToString();
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
                        //	MyPage.SetFocus(this.Parent.Page, ctrl);
                    }
                    else
                    {
                        CODE.Text = "";
                        DESC.Text = "";
                        MyPage.popMessage(this.Parent.Page, "Kode tidak ditemukan");
                        MyPage.SetFocus(this.Parent.Page, CODE);
                    }
                }
                else
                {
                    ListItemCollection items = new ListItemCollection();
                    MyPage.fillRefListINA(items, _query, null, dbtimeout, false, conn);
                    try
                    {
                        int i = 0;
                        for (i = 0; i < items.Count; i++)
                            if (items[i].Value.ToLower() == CODE.Text.ToLower())	//found
                            {
                                CODE.Text = items[i].Value;
                                DESC.Text = items[i].Text;
                                break;
                            }
                        //ctrl = CommonForm.ModuleSupport.NextCtrl(this.Parent.Page, CODE);
                        //if (ctrl != null)
                        //	MyPage.SetFocus(this.Parent.Page, ctrl);

                        if (i == items.Count)		// not found
                        {
                            CODE.Text = "";
                            DESC.Text = "";
                            MyPage.popMessage(this.Parent.Page, "Kode tidak ditemukan");
                            MyPage.SetFocus(this.Parent.Page, CODE);
                        }
                    }
                    catch
                    {
                        CODE.Text = "";
                        DESC.Text = "";
                        MyPage.popMessage(this.Parent.Page, "Kode tidak ditemukan");
                        MyPage.SetFocus(this.Parent.Page, CODE);
                    }
                }
            }
            if (AutoPostBack && _oldvalue != CODE.Text)
            {
                _oldvalue = CODE.Text;
                OnSelectedIndexChanged(e);
            }
        }

        #endregion

        #region custom events fires up
        public delegate void SelectedIndexChangeHandler(object sender, EventArgs e);
        public event SelectedIndexChangeHandler SelectedIndexChanged;
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }
        #endregion

        #region Properties
        #region common public properties
        public bool Enabled
        {
            set
            {
                _Enabled = value;
                if (_ddlmode) DDL.Enabled = value;
                else if (_frmmode)
                {
                    if (!value)
                        _frsrc += "&e=0";
                    fr.Attributes.Remove("src");
                    fr.Attributes.Add("src", _frsrc);
                }
                else
                {
                    CODE.Enabled = value;
                    BTN.Disabled = !value;
                }
            }
        }

        public System.Web.UI.WebControls.Unit Width
        {
            set { _Width = value; DDL.Width = value; }
        }

        public string CssClass
        {
            set
            {
                _CssClass = value;
                if (_ddlmode) DDL.CssClass = value;
                else if (_frmmode)
                {
                    _frsrc += "&c=" + value;
                    fr.Attributes.Remove("src");
                    fr.Attributes.Add("src", _frsrc);
                }
                else
                    CODE.CssClass = value;
            }
        }

        public short TabIndex
        {
            set
            {
                _TabIndex = value;
                if (_ddlmode) DDL.TabIndex = value;
                else if (_frmmode)
                {
                    _frsrc += "&t=" + value.ToString();
                    fr.Attributes.Remove("src");
                    fr.Attributes.Add("src", _frsrc);
                }
                else
                {
                    CODE.TabIndex = value;
                }
            }
        }
        #endregion

        #region user interaction
        public int SelectedIndex
        {
            get
            {
                try
                {
                    if (_ddlmode) return DDL.SelectedIndex;
                    else if (_frmmode)
                    {
                        if (cd.Value.Trim() == "")
                            return 0;
                        return -1;						//wont support query to the frame or maintain query viewstate in this user control.... 
                        //too much overhead for something probably wont be used.. 
                    }
                    else
                    {
                        if (CODE.Text.Trim() == "")
                            return 0;
                        dbtimeout = (int)Session["dbTimeOut"];
                        ConnString = (string)ConfigurationSettings.AppSettings["connString"].ToString();
                        int ret = -1;
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
                            // borrow ddl_ref to contain data.. should not need to clears it off as it would still be hidden and without viewstate
                            MyPage.fillRefListINA(DDL, qry, null, dbtimeout, false, conn);
                            for (int i = 0; i < DDL.Items.Count; i++)
                                if (DDL.Items[i].Value == CODE.Text)
                                    ret = i;
                        }
                        return ret;
                    }
                }
                catch (Exception ex)
                {	//silent log
                    MNTTools.LogError("UC_REFLIST:SelectedIndex:get: " + Request.RawUrl, (string)Session["UserID"], ex);
                    return -1;
                }
            }
            set
            {
                try
                {
                    if (_ddlmode) DDL.SelectedIndex = value;
                    else if (_frmmode) _frsrc += "&si=" + value.ToString();
                    else
                    {
                        if (value == 0)
                        {
                            CODE.Text = "";
                            DESC.Text = "";
                            return;
                        }
                        dbtimeout = (int)Session["dbTimeOut"];
                        ConnString = (string)ConfigurationSettings.AppSettings["connString"].ToString();
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
                            // borrow ddl_ref to contain data.. should not need to clears it off as it would still be hidden and without viewstate
                            MyPage.fillRefListINA(DDL, qry, null, dbtimeout, false, conn);
                            DDL.SelectedIndex = value;
                            CODE.Text = DDL.SelectedValue;
                            DESC.Text = DDL.SelectedItem.Text;
                        }
                    }
                }
                catch (Exception ex)
                {	//silent log
                    MNTTools.LogError("UC_REFLIST:SelectedIndex:set: " + Request.RawUrl, (string)Session["UserID"], ex);
                }
            }
        }

        public ListItem SelectedItem
        {
            get
            {
                if (_ddlmode) return DDL.SelectedItem;
                else if (_frmmode) return new ListItem(de.Value, cd.Value);
                else return new ListItem(DESC.Text, CODE.Text);
            }
        }

        public string SelectedValue
        {
            get
            {
                if (_ddlmode) return DDL.SelectedValue;
                else if (_frmmode) return cd.Value;
                else return CODE.Text;
            }
            set
            {
                try
                {
                    if (_ddlmode)
                    {
                        if (value == null || value.Trim() == "" || value.Trim() == "&nbsp;")
                            DDL.SelectedIndex = 0;
                        else
                            DDL.SelectedValue = value;
                    }
                    else if (_frmmode)
                    {
                        _frsrc += "&sv=" + value;
                    }
                    else
                    {
                        if (value == null || value.Trim() == "" || value.Trim() == "&nbsp;")
                        {
                            CODE.Text = "";
                            DESC.Text = "";
                            return;
                        }
                        dbtimeout = (int)Session["dbTimeOut"];
                        ConnString = (string)ConfigurationSettings.AppSettings["connString"].ToString();
                        using (conn = new DbConnection(ConnString))
                        {
                            if (_query == null || _query.Trim() == "")
                            {
                                string qry = "select " + _fldid + ", " + _flddesc + " from " + _tblname;
                                if (_cond != null && _cond.Trim() != "")
                                    qry += " where (" + _cond + ") and " + _fldid + " = '" + value + "'";
                                else
                                    qry += " where " + _fldid + " = '" + value + "'";
                                conn.ExecReader(qry, null, dbtimeout);
                                if (conn.hasRow())
                                {
                                    CODE.Text = conn.GetFieldValue(0);
                                    DESC.Text = conn.GetFieldValue(1);
                                }
                            }
                            else
                            {
                                // borrow ddl_ref to contain data.. should not need to clears it off as it would still be hidden and without viewstate
                                MyPage.fillRefListINA(DDL.Items, _query, null, dbtimeout, false, conn);
                                DDL.SelectedValue = value;
                                CODE.Text = DDL.SelectedValue;
                                DESC.Text = DDL.SelectedItem.Text;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {	//silent log
                    MNTTools.LogError("UC_REFLIST:SelectedValue:set: " + Request.RawUrl, (string)Session["UserID"], ex);
                }
            }
        }

        public ListItemCollection Items
        {
            get
            {
                if (_ddlmode)
                    return DDL.Items;
                else
                    return new ListItemCollection();
            }
        }

        public bool AutoPostBack
        {
            set { if (_ddlmode) DDL.AutoPostBack = value; ViewState["autopostback"] = value; SearchBtnAttribute(_btnqrystr, _btntitle); }
            get { if (ViewState["autopostback"] == null) return false; return (bool)ViewState["autopostback"]; }
        }
        #endregion

        public int DDLMaxItems
        {
            set { _maxItems = value; ViewState["maxItems"] = value; }
        }

        private bool _ddlmode
        {
            get { if (ViewState["ddlmode"] == null) return true; return (bool)ViewState["ddlmode"]; }
            set { ViewState["ddlmode"] = value; }
        }

        private bool _frmmode
        {
            get { return false; }           // parent postback caused selected value to be lost....need to be refined.. 
            set { }                         // on set, do nothing.... until previous probs been solved..
            //get{if (ViewState["frmmode"] == null) return false; return (bool)ViewState["frmmode"];}
            //set{ViewState["frmmode"] = value;}
        }

        private string _oldvalue
        {
            get { return (string)ViewState["oldvalue"]; }
            set { ViewState["oldvalue"] = value; }
        }

        #region fillRefList Viewstates for CODE Text Mode
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
        private string _btnqrystr
        {
            get { return (string)ViewState["btnqrystr"]; }
            set { ViewState["btnqrystr"] = value; }
        }
        private string _btntitle
        {
            get { return (string)ViewState["btntitle"]; }
            set { ViewState["btntitle"] = value; }
        }
        #endregion
        #endregion

        #region Methods
        public void ClearSelection()
        {
            if (_ddlmode) DDL.ClearSelection();
            else if (_frmmode) _frsrc += "&si=0";
            else
            {
                CODE.Text = "";
                DESC.Text = "";
            }
        }

        public void fillRefList(string query, bool withRefCode)
        {
            fillRefList(null, null, null, query, withRefCode, false);
        }

        public void fillRefList(string tblname, string fldid, string flddesc, string cond, string orderby, bool withRefCode)
        {
            fillRefList(null, null, null, tblname, fldid, flddesc, cond, orderby, withRefCode, false);
        }

        public void fillRefList(string schtitle, string schfldidtext, string schflddesctext,
            string query, bool withRefCode)
        {
            fillRefList(schtitle, schfldidtext, schflddesctext, query, withRefCode, false);
        }

        public void fillRefList(string schtitle, string schfldidtext, string schflddesctext,
            string tblname, string fldid, string flddesc, string cond, string orderby, bool withRefCode)
        {
            fillRefList(schtitle, schfldidtext, schflddesctext, tblname, fldid, flddesc, cond, orderby, withRefCode, false);
        }

        public void fillRefList(string schtitle, string schfldidtext, string schflddesctext,
            string query, bool withRefCode, bool FrameMode)
        {
            _frmmode = FrameMode;
            dbtimeout = (int)Session["dbTimeOut"];
            ConnString = (string)Session["ConnStringLogin"];
            using (conn = new DbConnection(ConnString))
            {
                MyPage.fillRefListINA(DDL.Items, query, null, dbtimeout, withRefCode, conn);
            }
            if (DDL.Items.Count <= _maxItems)
            {
                _ddlmode = true;
                d1.Visible = false;
                d2.Visible = false;
                CODE.EnableViewState = false;
                DESC.EnableViewState = false;
            }
            else
            {
                _ddlmode = false;
                DDL.Items.Clear();
                DDL.EnableViewState = false;
                DDL.Visible = false;

                if (_frmmode)
                {
                    d1.Visible = true;
                    d2.Visible = false;
                    CODE.EnableViewState = false;
                    DESC.EnableViewState = false;

                    if (schtitle != null && schtitle.Trim() != "")
                        _frsrc += "&fT=" + schtitle.Replace(" ", "_");
                    if (schfldidtext != null && schfldidtext.Trim() != "")
                        _frsrc += "&idTx=" + schfldidtext;
                    if (schflddesctext != null && schflddesctext.Trim() != "")
                        _frsrc += "&deTx=" + schflddesctext;
                    _frsrc += "&qry=" + HttpUtility.UrlEncode(query.Replace("'", "|:|"));
                }
                else
                {
                    d1.Visible = false;
                    d2.Visible = true;
                    CODE.EnableViewState = true;
                    DESC.EnableViewState = true;

                    _query = query;
                    string qrystr = "CtrlID=" + CODE.ClientID + "&CtrlDesc=" + DESC.ClientID;
                    if (schfldidtext != null && schfldidtext.Trim() != "")
                        qrystr += "&idTx=" + schfldidtext;
                    if (schflddesctext != null && schflddesctext.Trim() != "")
                        qrystr += "&deTx=" + schflddesctext;
                    if (schtitle == null)
                        schtitle = "";
                    qrystr += "&qry=" + HttpUtility.UrlEncode(query.Replace("'", "|:|"));

                    SearchBtnAttribute(qrystr, schtitle.Replace(" ", "_"));
                }
            }

            Enabled = _Enabled;
            if (_Width != 0)
                Width = _Width;
            if (_TabIndex != 0)
                TabIndex = _TabIndex;
            if (_CssClass != null)
                CssClass = _CssClass;
        }

        public void fillRefList(string schtitle, string schfldidtext, string schflddesctext,
            string tblname, string fldid, string flddesc, string cond, string orderby, bool withRefCode, bool FrameMode)
        {
            _frmmode = FrameMode;
            dbtimeout = (int)Session["dbTimeOut"];
            ConnString = (string)ConfigurationSettings.AppSettings["connString"].ToString();
            string query = "select " + fldid + ", " + flddesc + " from " + tblname;
            if (cond != null && cond.Trim() != "")
                query += " where " + cond;
            if (orderby != null && orderby.Trim() != "")
                query += " order by " + orderby;
            using (conn = new DbConnection(ConnString))
            {
                MyPage.fillRefListINA(DDL.Items, query, null, dbtimeout, withRefCode, conn);
            }
            if (DDL.Items.Count <= _maxItems)
            {
                _ddlmode = true;
                d1.Visible = false;
                d2.Visible = false;
                CODE.EnableViewState = false;
                DESC.EnableViewState = false;
            }
            else
            {
                _ddlmode = false;
                DDL.Items.Clear();
                DDL.EnableViewState = false;
                DDL.Visible = false;

                if (_frmmode)
                {
                    d1.Visible = true;
                    d2.Visible = false;
                    CODE.EnableViewState = false;
                    DESC.EnableViewState = false;

                    if (schtitle != null && schtitle.Trim() != "")
                        _frsrc += "&fT=" + schtitle.Replace(" ", "_");
                    if (schfldidtext != null && schfldidtext.Trim() != "")
                        _frsrc += "&idTx=" + schfldidtext;
                    if (schflddesctext != null && schflddesctext.Trim() != "")
                        _frsrc += "&deTx=" + schflddesctext;
                    _frsrc += "&tbl=" + tblname;
                    _frsrc += "&fid=" + fldid;
                    _frsrc += "&fdesc=" + flddesc;
                    if (cond != null && cond.Trim() != "")
                        _frsrc += "&cond=" + HttpUtility.UrlEncode(cond.Replace("'", "|:|"));
                    if (orderby != null && orderby.Trim() != "")
                        _frsrc += "&sort=" + HttpUtility.UrlEncode(orderby);
                }
                else
                {
                    d1.Visible = false;
                    d2.Visible = true;
                    CODE.EnableViewState = true;
                    DESC.EnableViewState = true;

                    _tblname = tblname;
                    _fldid = fldid;
                    _flddesc = flddesc;
                    _cond = cond;
                    _orderby = orderby;

                    string qrystr = "CtrlID=" + CODE.ClientID + "&CtrlDesc=" + DESC.ClientID;
                    if (schfldidtext != null && schfldidtext.Trim() != "")
                        qrystr += "&idTx=" + schfldidtext;
                    if (schflddesctext != null && schflddesctext.Trim() != "")
                        qrystr += "&deTx=" + schflddesctext;
                    if (schtitle == null)
                        schtitle = "";
                    qrystr += "&tbl=" + tblname;
                    qrystr += "&fid=" + fldid;
                    qrystr += "&fdesc=" + flddesc;
                    if (cond != null && cond.Trim() != "")
                        qrystr += "&cond=" + HttpUtility.UrlEncode(cond.Replace("'", "|:|"));
                    if (orderby != null && orderby.Trim() != "")
                        qrystr += "&sort=" + HttpUtility.UrlEncode(orderby);
                    SearchBtnAttribute(qrystr, schtitle.Replace(" ", "_"));
                }
            }

            Enabled = _Enabled;
            if (_Width != 0)
                Width = _Width;
            if (_TabIndex != 0)
                TabIndex = _TabIndex;
            if (_CssClass != null)
                CssClass = _CssClass;
        }

        private void SearchBtnAttribute(string querystring, string title)
        {
            _btnqrystr = querystring;
            _btntitle = title;
            if (AutoPostBack)
                querystring += "&fr=1";         //mode fr (framemode) will cause RefSearch to invoke parent submit, thus can be use for autopostback
            BTN.Attributes.Remove("onclick");
            BTN.Attributes.Add("onclick", "javascript:window.open('../CommonForm/RefSearch.aspx?" +
                querystring + "', '" + title + "', 'status=no,scrollbars=no,width=450,height=250')");

            //window.showModelessDialog("SMLD_target.htm","Dialog Box Arguments # 1","dialogHeight: 250px; dialogWidth: 400px; dialogTop: 155px; dialogLeft: 381px; edge: Sunken; center: Yes; help: No; resizable: No; status: No;"); 
            //BTN.Attributes.Add("onclick", "javascript:window.showModalDialog('../CommonForm/RefSearch.aspx?" + 
            //	querystring + "', '', 'dialogHeight: 250px; dialogWidth: 450px; edge: Raised; center: Yes; resizable: No; status: No;')");
        }
        #endregion
    }
}