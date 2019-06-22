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


namespace DebtChecking.CommonForm
{
    public partial class ListAgency : MasterPage
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

                if (Request.QueryString["atype"]=="RATING")
                    rblsendchange.Items[1].Text = "Excel";

                TitleHeader.Text = title;
                ViewState["strSQL"] = strSQL;

                link.Attributes.Add("onclick", "javascript:window.open('" +
                    "../List/UploadVerification.aspx?atype=" + Request.QueryString["atype"] +
                    "', 'Upload', 'status=no,scrollbars=no,width=500,height=400')");
            }
            foreach (ListItem li in rblsendchange.Items)
                li.Attributes.Add("onclick", "callback(sendtoPanel,'',false)");
        }


        protected void mainPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            SortedList uploadagency = new SortedList();
            ExcelUpload oExcelUpload = new ExcelUpload(conn);
            object[] param;

            NameValueCollection nvcAuto = new NameValueCollection();
            NameValueCollection nvcAutoField = new NameValueCollection();
            int idxAuto = 0;
            string stage = Request.QueryString["atype"];
            string assigntrack = "1.1";
            string assigntrackfrom = "1.0";
            string assigntype = "1";
            string stagequota = stage;
            string nexttrack = "";

            regno = grid.GetSelectedFieldValues("__KeyField");

            if (e.Parameter == "confirm")
            {
                try
                {
                    generate(SendTo.SelectedValue.ToString());
                }
                catch { }
            }

            //for (int i = 0; i < selList.Items.Count; i++)
            for (int i = 0; i < regno.Count; i++)
            {
                string sendto = SendTo.SelectedValue;

                switch (e.Parameter)
                {
                    case "confirm":
                        grid.Selection.UnselectAll();
                        param = new object[] { regno[i].ToString(), sendto, USERID };
                        conn.ExecuteNonQuery("EXEC SP_SENDTOAGENCY @1, @2, @3", param, dbtimeout);
                        break;
                    case "send":
                        param = new object[]{regno[i].ToString(),sendto,USERID};
                        conn.ExecuteNonQuery("EXEC SP_FLAG_SENDTOAGENCY @1, @2, @3", param, dbtimeout);
                        break;

                }
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

        protected void sendtoPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

        }

        protected void SendTo_Load(object sender, EventArgs e)
        {
            if (rblsendchange.SelectedValue == "1")
            {
                object[] param = new object[]{
                    USERID,
                    Request.QueryString["stg"]
                };
                staticFramework.reff(SendTo,
                    "EXEC SP_REFFSPV_ASSIGN @1,@2"
                    , param, conn, false);
            }
            else
            {
                string stg = null;
                if (Request.QueryString["atype"] == "RATING")
                    stg = Request.QueryString["stg"];
                else
                    stg = Request.QueryString["stg"] + "|A";

                object[] param = new object[]{
                    USERID,
                    stg
                };
                staticFramework.reff(SendTo,
                    "EXEC SP_REFFSPV_ASSIGN @1,@2"
                    , param, conn, false);
            }
            try
            {
                SendTo.SelectedValue = hSendTo.Value;
            }
            catch { }
        }

        protected void generate(string vendor)
        {
            #region excel
            string STR_ACCOUNT = "'";
            string excelFilePath = null, UploadFile = null;

            UploadFile = "../Download/Scoring/";
            string PhysicalPath = Server.MapPath(UploadFile);

            DateTime now = DateTime.Now;
            string date = now.Day.ToString("00") + now.Month.ToString("00") + now.Year.ToString();
            //              + now.Hour.ToString("00") + now.Minute.ToString("00") + now.Second.ToString("00");
            //string nameoffile = "scoring_" + date + "_" + USERID +".csv";
            //string filepath = PhysicalPath + "\\" + nameoffile;
            string sql = "exec slik_get_scoring_batchid";
            conn.ExecReader(sql, null, dbtimeout);
            string seq = "0";
            if (conn.hasRow()) { seq = conn.GetFieldValue(0).ToString(); }
            string nameoffile = "SID" + date + seq.PadLeft(2, '0') + ".csv";
            string filepath = PhysicalPath + "\\" + nameoffile;

            StreamWriter fp;
            fp = File.CreateText(filepath);

            for (int i = 0; i < regno.Count; i++)
            {
                STR_ACCOUNT += "," + regno[i].ToString();
            }

            if (!Directory.Exists(Server.MapPath(excelFilePath)))
                Directory.CreateDirectory(Server.MapPath(excelFilePath));

            STR_ACCOUNT = STR_ACCOUNT.Replace(",", "','") + "'";
            string Q_APP_LIST = "select * from slik_vw_scoring_export WHERE reffnumber in (" + STR_ACCOUNT + ")";
            DataTable APP_LIST = conn.GetDataTable(Q_APP_LIST, null, dbtimeout);
            string ROW = "", ROW_ALL = "";

            for (int i = 0; i < APP_LIST.Rows.Count; i++)
            {
                ROW = "";
                for (int x = 1; x < APP_LIST.Columns.Count; x++)
                {
                    ROW += APP_LIST.Rows[i][x];
                    //ROW_ALL += APP_LIST.Rows[i][x];
                }
                fp.WriteLine(ROW);
                //ROW_ALL += System.Environment.NewLine;
            }
            fp.Close();

            try
            {
                sql = "update [slik_scoring_result] set downloaded = '1', downloadby = @1, downloaddate = getdate(), batchid = @2 " +
                    "where scoring_reffnumber in (" + STR_ACCOUNT + ")";
                conn.ExecNonQuery(sql, new object[] { USERID, nameoffile.Replace(".csv","") }, dbtimeout);
            } catch(Exception ex)
            { mainPanel.JSProperties["cp_alert"] = ex.Message; }

            mainPanel.JSProperties["cp_export"] = UploadFile + nameoffile;
            //mainPanel.JSProperties["cp_export"] = ROW_ALL;
            mainPanel.JSProperties["cp_filename"] = nameoffile;
            #endregion

        }
    }
}
