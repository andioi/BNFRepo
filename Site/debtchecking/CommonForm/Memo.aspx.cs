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
    public partial class Memo : DataPage
    {
        #region static vars
        private static string Q_DATA = "select * from vw_memo where mm_id = @1 ";
        private static string U_SAVE = "exec USP_MEMO_SAVE @1, @2, @3 ";
        #endregion

        #region retrieve & save & delete & gridbind

        #region save_data
        private bool save_data()
        {
            bool retval = false;
            try
            {
                object[] param = new object[] { MM_ID, TXT_MEMO.Text.Replace(Environment.NewLine, "<BR>"), USERID };
                conn.ExecNonQuery(U_SAVE, param, dbtimeout);
                cleardata();
                retval = true;
            }
            catch (Exception ex)
            {
                string errmsg = "";
                if (ex.Message.IndexOf("Last Query:") > 0)
                    errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                else
                    errmsg = ex.Message;
                DMS.Tools.MyPage.popMessage(this, errmsg);
            }
            return retval;
        }
        #endregion

        #region griddata
        private void binddata()
        {
            object[] par = new object[1] { MM_ID };
            griddata.DataSource = conn.GetDataTable(Q_DATA, par, dbtimeout);
            griddata.DataBind();
        }
        #endregion

        #endregion

        private string MM_ID
        {
            get { return Request.QueryString["regno"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cleardata();
                binddata();
            }
            btn_save.Attributes.Add("onclick", "if (!cek_mandatory(document.form1)) return false; ");
        }

        private void cleardata()
        {
            TXT_MEMO.Text = "";
            Response.Write("<script for=window event=onload language='JavaScript'>hideTR();</script>");
        }

        protected void griddata_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            griddata.PageIndex = e.NewPageIndex;
        }

        protected void griddata_PageIndexChanged(object sender, EventArgs e)
        {
            binddata();
        }

        protected void griddata_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    DataRowView dv = (DataRowView)e.Row.DataItem;
                    Label lbl = (Label)e.Row.FindControl("lblmsg");
                    lbl.Text = dv["MM_MESSAGE"].ToString();
                    break;
                default:
                    break;
            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            if (save_data())
                binddata();
        }

        protected void btn_new_Click(object sender, EventArgs e)
        {
            TXT_MEMO.Text = "";
            Response.Write("<script for=window event=onload language='JavaScript'>showTR();</script>");
        }
    }
}
