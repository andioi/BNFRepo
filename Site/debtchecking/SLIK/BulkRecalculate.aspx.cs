using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
using DMS.Interface;


namespace DebtChecking.SLIK
{
    public partial class BulkRecalculate : MasterPage
    {
        string li_suffix = "";
        List<object> regno;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["li_suffix"] != null)
                li_suffix = Request.QueryString["li_suffix"];

            if (!IsPostBack)
            {
                string strSQL = "", title = "";
                ListSys.gridInit(grid, Request.QueryString["li" + li_suffix], ref title, ref strSQL, conn);
                
                TitleHeader.Text = title;
                ViewState["strSQL"] = strSQL;
            }
           
        }


        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            NameValueCollection nvcAuto = new NameValueCollection();
            NameValueCollection nvcAutoField = new NameValueCollection();
            string stage = Request.QueryString["atype"];
            

            regno = grid.GetSelectedFieldValues("__KeyField");

            if (e.Parameters == "confirm")
            {
                try
                {
                    recalculate();
                }
                catch { }
                grid.JSProperties["cp_alert"] = "Recalculate Success.";
                grid.JSProperties["cp_redirect"] = "BulkRecalculate.aspx?needFilter=1&passurl&mntitle=Bulk Recalculate&li=L|REC";
            }
            
            ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }

        public static string FixupPath(string Url)
        {
            if (Url.StartsWith("~/") || Url.StartsWith("~\\"))
                Url = HttpContext.Current.Request.PhysicalApplicationPath + Url.Substring(2);

            return Url.Replace("/", "\\");
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }

        protected void recalculate()
        {
            for (int i = 0; i < regno.Count; i++)
            {
                runcreditpolicy(regno[i].ToString());
            }
        }

        private void runcreditpolicy(string appid)
        {
            try
            {
                object[] par = new object[] { appid };
                conn.ExecNonQuery("exec slik_clearPolicy @1", par, dbtimeout);
                string sql = "select * from slik_vw_creditpolicy where appid = @1 ";
                DataTable dt = conn.GetDataTable(sql, par, dbtimeout);
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    DecSystem d = new DecSystem(conn);
                    string resultid = dt.Rows[i]["fasilitasid"].ToString();
                    par = new object[] { resultid, "SLIK" };
                    try
                    {
                        string ResultID = d.execute("AND [slik_ideb_kredit].[fasilitasid]=@1", par, "POLICY", "SLIK", null);
                        par = new object[] { resultid, "POLICY", "SLIK", ResultID };
                        conn.ExecuteNonQuery("EXEC SP_APPDECSYSRES @1,@2,@3,@4", par, dbtimeout);
                    }
                    catch { }
                }
                par = new object[] { appid };
                conn.ExecNonQuery("exec slik_updFinalPolicy @1", par, dbtimeout);
            }
            catch (Exception ex)
            {
                grid.JSProperties["cp_alert"] = ex.Message;
            }
        }

    }
}
