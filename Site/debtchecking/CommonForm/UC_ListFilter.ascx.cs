using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using DMS.Tools;
using DMS.Framework;

namespace DebtChecking.List
{
    public partial class UC_ListFilter : System.Web.UI.UserControl
    {
        public DbConnection conn;
        public object[] paramFilter;
        public string strFilter;
        string li_suffix = "";
        public string clrfilter = "";
        public NameValueCollection nvcFilterReff = new NameValueCollection();

        public string clearfilter()
        {
            return clrfilter;
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Request.QueryString["li_suffix"] != null)
                li_suffix = Request.QueryString["li_suffix"];
            conn = new DbConnection((string)Session["ConnString"]);
            clrfilter = ListSys.FilterInit(tblSearch, Request.QueryString["li" + li_suffix], conn, (DevExpress.Web.CallbackEventHandlerBase)dxComboBox_Callback, nvcFilterReff);
            clrButton.Attributes["onclick"] = clrfilter;
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            conn.Dispose();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            paramFilter = ListSys.FilterParam(this, ref strFilter);
            if (paramFilter.Length == 0)
                mainTbl.Style["display"] = "none";
            if (!IsPostBack && !this.Page.IsCallback)
            {
                initreff();
                DataTable dt = conn.GetDataTable("select LISTNEWBUTTON, NEWBUTTONCLICK from list where listid = @1", new object[] { Request.QueryString["li" + li_suffix] }, 600);
                if (dt.Rows[0]["LISTNEWBUTTON"].ToString() == "1")
                {
                    btnnew.Style.Remove("display");
                    btnnew.Attributes["onclick"] = "parent.window.location='" + dt.Rows[0]["NEWBUTTONCLICK"].ToString() + "'";
                }
            }
        }

        protected void initreff()
        {
            for (int i = 1; i <= paramFilter.Length; i++)
            {
                WebControl oCtrl = (WebControl)tblSearch.FindControl(ReportSys.FilterId + i.ToString());
                if (oCtrl is DevExpress.Web.ASPxComboBox)
                {
                    DevExpress.Web.ASPxComboBox oCtrlASPxComboBox = (DevExpress.Web.ASPxComboBox)oCtrl;
                    string FieldReff = nvcFilterReff[oCtrlASPxComboBox.ID];
                    staticFramework.reff(oCtrlASPxComboBox, FieldReff, paramFilter, conn);
                    for (int j = 1; j <= paramFilter.Length; j++)
                    {
                        if (FieldReff.IndexOf("@" + j.ToString()) >= 0)
                        {
                            WebControl oCtrlCascade = (WebControl)this.FindControl(ReportSys.FilterId + j.ToString());
                            if (oCtrlCascade is DevExpress.Web.ASPxComboBox)
                            {
                                DevExpress.Web.ASPxComboBox oCtrlCascadeASPxComboBox = (DevExpress.Web.ASPxComboBox)oCtrlCascade;
                                if (oCtrlCascadeASPxComboBox.ClientSideEvents.ValueChanged == "")
                                    oCtrlCascadeASPxComboBox.ClientSideEvents.ValueChanged = "function(s,e){}";
                                oCtrlCascadeASPxComboBox.ClientSideEvents.ValueChanged =
                                      oCtrlCascadeASPxComboBox.ClientSideEvents.ValueChanged.Substring(0, oCtrlCascadeASPxComboBox.ClientSideEvents.ValueChanged.Length - 1) +
                                      oCtrlASPxComboBox.ClientID + ".PerformCallback('r');}";
                                oCtrlCascadeASPxComboBox.ClientSideEvents.EndCallback = oCtrlCascadeASPxComboBox.ClientSideEvents.ValueChanged;
                            }
                        }
                    }
                }
            }
        }

        protected void dxComboBox_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            DevExpress.Web.ASPxComboBox oCtrlASPxComboBox = (source as DevExpress.Web.ASPxComboBox);
            string FieldReff = nvcFilterReff[oCtrlASPxComboBox.ID];
            staticFramework.reff(oCtrlASPxComboBox, FieldReff, paramFilter, conn);
        }
    }
}