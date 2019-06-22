using System;
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

namespace DebtChecking.CommonForm
{
    public partial class ListRestoreData : MasterPage
    {
        #region initial_reffrential_parameter
        protected void initial_reffrential_parameter()
        {
            
        }
        #endregion

        string li_suffix = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["li_suffix"] != null)
                li_suffix = Request.QueryString["li_suffix"];

            if (!IsPostBack)
            {
                initial_reffrential_parameter();
                string strSQL = "", title = "";
                ListSys.gridInit(grid, Request.QueryString["li" + li_suffix], ref title, ref strSQL, conn);
                TitleHeader.Text = title;
                ViewState["strSQL"] = strSQL;
            }
        }


        protected void mainPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            NameValueCollection nvcAuto = new NameValueCollection();

            List<object> regno = grid.GetSelectedFieldValues("__KeyField");

            for (int i = 0; i < regno.Count; i++)
            //for (int i = 0; i < selList.Items.Count; i++)
            {
                switch (e.Parameter)
                {                       
                    case "send":
                        string reffnumber = regno[i].ToString();

                        DataTable dt = conn.GetDataTable("select appid from applicant_bkp where reffnumber = @1",
                            new object[] { reffnumber }, dbtimeout);
                        string appid = "";
                        for (int k = 0; k < dt.Rows.Count; k++ )
                        {
                            appid = dt.Rows[k]["appid"].ToString();
                            conn.ExecReader("exec USP_RESTORE_DATA @1, @2, @3",
                            new object[] { appid, USERID, "1" }, dbtimeout);
                        }

                    break;
                }
            }

            grid.Selection.UnselectAll();
            ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            //object[] regno = new object[] { grid.GetSelectedFieldValues("__KeyField") };
            ListSys.gridBind(grid, (string)ViewState["strSQL"], UC_ListFilter1.paramFilter, UC_ListFilter1.strFilter, conn);
        }
    }
}
