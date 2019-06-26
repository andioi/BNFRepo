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


namespace MikroMnt.Parameter
{
    public partial class ParamMaker : System.Web.UI.Page
    {
        public string BackURL
        {
            get { return (string)Session["BackURL"]; }
        }
        public string funcCss = "hide";
        public string funcpendCss = "hide";

        public string funcpenddelCss(string Status)
        {
            if (Status == "Delete")
                return "hide";
            else
                return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                title.Text = Request.QueryString["title"];
                USC_paraminput1.createGridColumns(grid, gridpending);
            }
        }

        protected void panel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                panel.JSProperties["cp_action"] = "";
                if (e.Parameter.StartsWith("s:"))
                {
                    panel.JSProperties["cp_action"] = "s";
                    USC_paraminput1.savepending(null);
                    panel.JSProperties["cp_closepopup"] = "1";
                }
                else if (e.Parameter.StartsWith("r:"))
                {
                    panel.JSProperties["cp_action"] = "r";
                    USC_paraminput1.retrieve(e.Parameter.Substring(2));
                    panel.JSProperties["cp_closepopup"] = "";
                }
                else if (e.Parameter.StartsWith("rp:"))
                {
                    panel.JSProperties["cp_action"] = "rp";
                    USC_paraminput1.retrievepending(e.Parameter.Substring(3));
                    panel.JSProperties["cp_closepopup"] = "";
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                if (errmsg.IndexOf("Last Query") > 0)
                    errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                panel.JSProperties["cp_alert"] = errmsg;
            }
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            USC_paraminput1.dtbinddata(grid);
            funcCss = (grid.GroupCount > 0) ? "" : "hide";
        }
        protected void gridpending_Load(object sender, EventArgs e)
        {
            USC_paraminput1.dtbindpending(gridpending);
            funcpendCss = (grid.GroupCount > 0) ? "" : "hide";
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void gridpending_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                if (e.Parameters.StartsWith("d:"))
                {
                    USC_paraminput1.savepending(e.Parameters.Substring(2));
                    USC_paraminput1.dtbindpending(gridpending);
                    gridpending.JSProperties["cp_alert"] = "Data deletion requested.";
                }
                if (e.Parameters.StartsWith("dp:"))
                {
                    USC_paraminput1.deletepending(e.Parameters.Substring(3));
                    USC_paraminput1.dtbindpending(gridpending);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                if (errmsg.IndexOf("Last Query") > 0)
                    errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                gridpending.JSProperties["cp_alert"] = errmsg;
            }
        }

        protected void grid_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            funcCss = (grid.GroupCount > 0) ? "" : "hide";
        }

        protected void gridpending_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            funcpendCss = (gridpending.GroupCount > 0) ? "" : "hide";
        }
    }
}
