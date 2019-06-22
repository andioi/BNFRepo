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
using System.Drawing;
using DMS.Tools;
using DMS.Framework;

namespace DebtChecking
{
    public partial class Body : System.Web.UI.Page
    {
        #region static vars
        private static string Q_GETMODULE = "SELECT MODULEID FROM RFMODULE WHERE MODULENAME = @1 ";
        private static string Q_LISTMODULE = "SELECT GROUPID, SG_GRPNAME, MODULEID, MODULENAME " +
            "FROM VW_GRPACCESSMODULE WHERE GROUPID = @1 ";
        private static string SP_SAVETOKEN = "EXEC ES_SAVETOKEN @1, @2, @3 ";
        private static string Q_GETMSG = "exec USP_GETMSG";
        #endregion

        private DbConnection connESecurity;
        private int dbtimeout;
        System.Text.StringBuilder htmlbuilder = new System.Text.StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
                connESecurity = new DbConnection(Session["ConnStringLogin"].ToString());
                LoadModule();
                if (!IsPostBack)
                {
                    Session.Remove("BackURL");
                    LoadMenu();
                    /*object[] param = new object[] { Session["UserID"], Session["ModuleID"] };
                    connESecurity.ExecReader(Q_GETMSG, param, dbtimeout);
                    if (connESecurity.hasRow())
                    {
                        svrmsg.Value = "1";
                        Label2.Visible = true;
                        Label2.Text = connESecurity.GetFieldValue(0).Trim();
                    }
                    connESecurity.Dispose();*/
                }
            } catch(Exception ex)
            {
                //Response.Write("<script language='JavaScript'>alert(' body=" + ex.Message.ToString() + "');</script>");
                Response.Write("<script language='JavaScript'>top.location.href = \"Logout.aspx?p=session\";</script>");
            }
        }

        private void LoadModule()
        {
            object[] pargrp = new object[1] { Session["GroupID"] };
            connESecurity.ExecReader(Q_LISTMODULE, pargrp, dbtimeout);

            int loopcounter = 0;
            while (connESecurity.hasRow())
            {
                loopcounter++;
                if (loopcounter == 1)
                    if (!IsPostBack)
                        try
                        {
                            Label1.Text = connESecurity.GetFieldValue(2);
                        }
                        catch (Exception ex1)
                        {
                            Response.Write("<!-- " + ex1.Message.Replace("-->", "--)") + " -->\n");
                            ModuleSupport.LogError(this, ex1);
                        }

                LinkButton lb = new LinkButton();
                lb.Text = connESecurity.GetFieldValue(3);		//modulename 
                lb.Font.Bold = true;
                lb.Font.Name = "verdana";
                lb.Font.Size = FontUnit.Point(8);
                lb.ForeColor = Color.RoyalBlue;

                if (connESecurity.GetFieldValue(2) == ConfigurationSettings.AppSettings["ModuleID"])
                {
                    lb.ForeColor = Color.BlueViolet;
                    lb.Attributes.Add("onclick", "return false;");
                }
                else lb.Click += new EventHandler(lb_Click);

                PlaceHolder1.Controls.Add(lb);
                PlaceHolder1.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }

            if (loopcounter <= 1)
                PlaceHolder1.Visible = false;
        }

        private void LoadMenu()
        {
            try
            {
                string modid = (string)Session["ModuleID"];
                object[] param = new object[] { modid, Session["GroupID"] };
                load_menu(connESecurity.GetDataTable(
                    "SELECT A.* FROM RFMENUX A JOIN GRPMENUX B ON A.TYPEID = B.TYPEID and A.MENUID = B.MENUID " +
                    "WHERE B.TYPEID = @1 and B.GROUPID = @2 " +
                    "union select '" + modid + "' as TYPEID, '99' as MENUID, 'Logout' as MENUDESC, 'Logout.aspx' as MENUURL, null as MENUIMAGEURL, null as MENUPARENT, 99 as MENUPOSITION, '' as PASSINGURL " +
                    "ORDER BY MENUPARENT, MENUPOSITION ",
                                    param, dbtimeout)
                    , null);
            }
            catch (Exception ex)
            {
                connESecurity.Dispose();
                Response.Redirect("Logout.aspx?menu", true);
            }
        }

        protected void load_menu(DataTable dtmenu, string MenuParent)
        {
            if (MenuParent == null)
                MenuParent = "is NULL";
            else
                MenuParent = "= '" + MenuParent + "'";

            DataView dv = new DataView(dtmenu, "MENUPARENT " + MenuParent, "", DataViewRowState.OriginalRows);
            htmlbuilder.Append("<ul> \n");

            for (int i = 0; i < dv.Count; i++)
            {
                string menuid = dv[i]["MENUID"].ToString();
                string menudesc = dv[i]["MENUDESC"].ToString();
                string menuurl = dv[i]["MENUURL"].ToString();
                string passingurl = "";
                try { passingurl = dv[i]["PASSINGURL"].ToString(); }
                catch { }
                if (menuurl != "")
                {
                    if (menuurl.IndexOf("?") < 0)
                        menuurl += "?";
                    else
                        menuurl += "&";
                    menuurl += "passurl";
                    if (menuurl.IndexOf("mntitle") < 0)
                        menuurl += "&mntitle=" + menudesc;
                    menuurl += passingurl;
                }
                //string menutarget = "_self";
                if (menuurl != "")
				    htmlbuilder.Append("<li><a href=\""+menuurl+"\">"+menudesc+"</a> \n");
                else
                    htmlbuilder.Append("<li><a href=\"#\">" + menudesc + "</a> \n");

				DataView dv2 = new DataView(dtmenu, "MENUPARENT = '" + menuid + "'", "", DataViewRowState.OriginalRows);
                if (dv2.Count > 0)
                    load_menu(dtmenu, menuid);
                else
                    htmlbuilder.Append("</li> \n");

            }
            htmlbuilder.Append("</ul> \n");
            smoothmenu.Text = htmlbuilder.ToString();
        }

        private void lb_Click(object sender, System.EventArgs e)
        {
            LinkButton lbl = (LinkButton)sender;

            object[] modname = new object[1] { lbl.Text };
            string moduleid = null, url = "";

            connESecurity.ExecReader(Q_GETMODULE, modname, dbtimeout);
            if (connESecurity.hasRow())
                moduleid = connESecurity.GetFieldValue(0);

            Guid token = System.Guid.NewGuid();
            object[] objtkn = new object[3] { token, Session["UserID"], moduleid };

            connESecurity.ExecReader(SP_SAVETOKEN, objtkn, dbtimeout);
            if (connESecurity.hasRow())
            {
                string tempurl = connESecurity.GetFieldValue(0);
                if (tempurl.IndexOf("?") >= 0)
                    url = tempurl + "&tkn=" + token.ToString() + "&sessionid=" + Session["sessionid"];
                else
                    url = tempurl + "?tkn=" + token.ToString() + "&sessionid=" + Session["sessionid"];
            }
            connESecurity.Dispose();

            if (url != "")
            {
                RemoveSession();
                Response.Redirect(url, true);
            }
            else
                MyPage.popMessage(this, "Invalid Server State!");
        }

        private void RemoveSession()
        {
            Session.Clear();
            Session.Abandon();
        }

    }
}
