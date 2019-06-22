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

namespace DebtChecking
{
    public partial class ScreenMenu : MasterPage
    {
        string urlplus = "";
        private DbConnection connESecurity;
        string startupurl, startupid, startupdesc;

        #region static vars
        private static string Q_MENU = "SELECT TYPEID, MENUID, MENUDESC, MENUURL, MENUIMAGEURL, " + 
            "MENUPARENT, MENUPOSITION, PASSINGURL, MDTRQRYSTR, NOQRYSTR " +
            "FROM SCREENMENU WHERE TYPEID=@1 "+
            /*"UNION " + 
            "SELECT @1 as TYPEID, 'MM' as MENUID, 'Memo' as MENUDESC, '~/CommonForm/Memo.aspx' as MENUURL, null as MENUIMAGEURL, " + 
            "null as MENUPARENT, 99 as MENUPOSITION, null as PASSINGURL, 'mmid' as MDTRQRYSTR, null as NOQRYSTR " + */
            "ORDER BY MENUPARENT, MENUPOSITION ";
        private static string Q_PASSURL = "SELECT PASSINGURL FROM SCREENMENU WHERE TYPEID=@1 AND MENUID=@2";
        private static string Q_MENUCHILD = "SELECT TYPEID, MENUID, MENUDESC, MENUURL, MENUIMAGEURL, " + 
            "MENUPARENT, MENUPOSITION, PASSINGURL, MDTRQRYSTR, NOQRYSTR " + 
            "FROM SCREENMENUCHILD WHERE TYPEID=@1 " + 
            //"and menuid in (select menuid from scgroupsmauthority where groupid = @2) " +
            "ORDER BY MENUPARENT, MENUPOSITION ";
        private static string Q_APPDATA = "select * from vw_mn_appinfo where ap_regno = @1 ";
        #endregion

        protected void load_menu(Literal menu, DataTable dtmenu, string MenuParent)
        {
            if (MenuParent == null)
                MenuParent = "is NULL";
            else
                MenuParent = "= '" + MenuParent + "'";
            DataView dv = new DataView(dtmenu, "MENUPARENT " + MenuParent, "", DataViewRowState.OriginalRows);
            System.Text.StringBuilder menubuilder = new System.Text.StringBuilder();
            for (int i = 0; i < dv.Count; i++)
            {
                string MandatoryQueryString = dv[i]["MDTRQRYSTR"].ToString(),
                    NoQueryString = dv[i]["NOQRYSTR"].ToString();
                char[] sep = { ';', ',' };
                if (MandatoryQueryString != null && MandatoryQueryString.Trim() != "")
                {
                    string[] mdtrqrystr = MandatoryQueryString.Split(sep);
                    bool QueryStringMissing = false;
                    for (int j = 0; j < mdtrqrystr.Length; j++)
                        if (Request.QueryString[mdtrqrystr[j]] == null || Request.QueryString[mdtrqrystr[j]].Trim() == "")
                        {
                            QueryStringMissing = true;
                            break;
                        }
                    if (QueryStringMissing)
                        continue;
                }
                if (NoQueryString != null && NoQueryString.Trim() != "")
                {
                    string[] noqrystr = NoQueryString.Split(sep);
                    bool QueryStringExists = false;
                    for (int j = 0; j < noqrystr.Length; j++)
                        if (Request.QueryString[noqrystr[j]] != null && Request.QueryString[noqrystr[j]].Trim() != "")
                        {
                            QueryStringExists = true;
                            break;
                        }
                    if (QueryStringExists)
                        continue;
                }

                string menuid = dv[i]["MENUID"].ToString();
                string menudesc = dv[i]["MENUDESC"].ToString();
                string menuurl = dv[i]["MENUURL"].ToString();
                string passingurl = "";
                try { passingurl = dv[i]["PASSINGURL"].ToString(); }
                catch { }

                if (menuurl != "")
                {
                    if (menuurl.IndexOf("?") > 0)
                        menuurl += "&" + urlplus + passingurl;
                    else
                        menuurl += "?" + urlplus + passingurl;
                }
                if (menu.ID.ToString() == "Menu2")
                    menubuilder.Append("<li class='' menuid=\"" + FixupUrl(menuurl) + "\" menudesc=\"" + 
                            menudesc + "\" id=\"" + menuid + "\"><a href=\"javascript:clickchild('" +
                        menuid + "','" + menudesc + "','" + FixupUrl(menuurl) + "')\">" + menudesc + "</a></li>");
                else
                {
                    if (menuurl != "")
                        menubuilder.Append("<li class='' menuid=\"" + FixupUrl(menuurl) + "\" menudesc=\"" + 
                            menudesc + "\" id=\"" + menuid + "\"><a href=\"" + FixupUrl(menuurl) + "\" onclick=\"setcurrent2('" + 
                            menuid + "','" + menudesc + "','" + FixupUrl(menuurl) + "')\">" + menudesc + "</a></li>");
                    else
                        menubuilder.Append("<li class='' menuid=\"" + FixupUrl(menuurl) + "\" menudesc=\"" +
                            menudesc + "\" id=\"" + menuid + "\"><a href=\"javascript:clickparent('" + menuid + "','" +
                            menudesc + "','" + FixupUrl(menuurl) + "');\" >" + menudesc + "</a></li>");
                }
                if (i == 0)
                {
                    startupurl = menuurl;
                    startupid = menuid;
                    startupdesc = menudesc;
                }
            }

            menu.Text = menubuilder.ToString();
        }

        private static string FixupUrl(string Url)
        {
            if (Url.StartsWith("~"))

                return (HttpContext.Current.Request.ApplicationPath +
                        Url.Substring(1)).Replace("//", "/");

            return Url;
        }

        private void ViewAppData()
        {
            object[] par = new object[1] { Request.QueryString["regno"] };
            conn.ExecReader(Q_APPDATA, par, dbtimeout);
            if (conn.hasRow())
            {
                I1.Visible = true;
                if (conn.GetFieldValue("l1") != "")
                    l1.Text = conn.GetFieldValue("l1");
                if (conn.GetFieldValue("l2") != "")
                    l2.Text = conn.GetFieldValue("l2");
                if (conn.GetFieldValue("l3") != "")
                    l3.Text = conn.GetFieldValue("l3");
                if (conn.GetFieldValue("l4") != "")
                    l4.Text = conn.GetFieldValue("l4");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try { urlplus = Request.RawUrl.Substring(Request.RawUrl.IndexOf("passurl")); }
            catch { };
            if (!IsPostBack)
            {
                if (Session["BackURL"] == null)
                    Session["BackURL"] = Request.RawUrl;
                else
                    HrefBack.HRef = (string)Session["BackURL"];
                HrefBack.Visible = (Request.RawUrl != (string)Session["BackURL"]);

                if (Request.QueryString["requestid"] != null)
                {
                    if (Request.QueryString["readonly"] != null)
                        linkupload.Attributes.Add("href", "javascript:PopupPage('../CommonForm/FileUpload.aspx?regno=" + Request.QueryString["requestid"] + "&readonly=" + Request.QueryString["readonly"] + "',700,500)");
                    else
                        linkupload.Attributes.Add("href", "javascript:PopupPage('../CommonForm/FileUpload.aspx?regno=" + Request.QueryString["requestid"] + "',700,500)");

                    if (Request.QueryString["readonly"] != null)
                        linkmemo.Attributes.Add("href", "javascript:PopupPage('../CommonForm/Memo.aspx?regno=" + Request.QueryString["requestid"] + "&readonly=" + Request.QueryString["readonly"] + "',700,500)");
                    else
                        linkmemo.Attributes.Add("href", "javascript:PopupPage('../CommonForm/Memo.aspx?regno=" + Request.QueryString["requestid"] + "',700,500)");
                    //ViewAppData();
                }
                else
                {
                    ul0.Visible = false;
                }
                ul0.Visible = false;
                object[] param = new object[] { Request.QueryString["sm"] };
                load_menu(Menu1, conn.GetDataTable(Q_MENU, param, dbtimeout), null);
                dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
                connESecurity = new DbConnection(Session["ConnStringLogin"].ToString());
                TitleHeader.Text = Request.QueryString["mntitle"];
            }
        }

        protected void MenuPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Menu2.Text = "";
            if (e.Parameter == "")
            {
                //MenuChild.Items.Add("", "", "");
                return;
            }
            object[] param = new object[] { Request.QueryString["sm"], e.Parameter };
            conn.ExecReader(Q_PASSURL, param, dbtimeout);
            if (conn.hasRow())
            {
                string passingurl = "";
                try { passingurl = conn.GetFieldValue("PASSINGURL").ToString(); }
                catch { };
                urlplus += passingurl;
            }
            param = new object[] { e.Parameter, Session["GroupID"] };
            load_menu(Menu2,conn.GetDataTable(Q_MENUCHILD, param, dbtimeout), null);
            h_startupurl.Value = FixupUrl(startupurl);
            h_startupid.Value = FixupUrl(startupid);
            h_startupdesc.Value = FixupUrl(startupdesc);
        }

    }
}
