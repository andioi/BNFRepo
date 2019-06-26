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

namespace MikroMnt.user.ModuleUser
{
    public partial class GroupMikro : System.Web.UI.Page
    {
        private DbConnection conn, modconn;
        private int dbtimeout;
        private string ModConnString;
        private string moduleid, groupid;

        #region static vars
        private static string Q_RFDUPREJECT = "select menuid, menudesc from screenmenuchild ";
        private static string Q_DATADUPAUTH = "EXEC SU_SCGROUP_PENDINGMODULEGROUP_SMAUTHBIND @1, @2 ";
        private static string SP_RSTDUPAUTH = "EXEC SU_SCGROUP_PENDINGMODULEGROUP_SMAUTHRESET @1, @2 ";
        private static string SP_ADDDUPAUTH = "EXEC SU_SCGROUP_PENDINGMODULEGROUP_SMAUTHADD @1, @2, @3 ";
        private static string SP_DELDUPAUTH = "EXEC SU_SCGROUP_PENDINGMODULEGROUP_SMAUTHDEL @1, @2, @3 ";

        private static string Q_MODULEGROUP = "select groupid, sg_grpname, moduleid, modulename, approval_group, usermntpage from vw_grpaccessmodule ";
        private static string Q_USERDATA = "EXEC SU_SCUSER_PENDINGMODULEUSER @1, @2 ";
        private static string SP_SAVEUSER = "EXEC SU_SCUSER_SAVEPENDINGMODULEUSER @1, @2, @3 ";
        #endregion

        #region init page
        protected override void OnLoad(EventArgs e)
        {
            moduleid = Request.QueryString["moduleid"];
            groupid = Request.QueryString["gid"];
            dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            ModConnString = MNTTools.GetConnString(moduleid);
            modconn = new DbConnection(ModConnString);
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            try
            {
                modconn.Dispose();
            }
            catch { }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                object[] par = new object[2] { groupid, moduleid };
                modconn.ExecNonQuery(SP_RSTDUPAUTH, par, dbtimeout);
                MyPage.fillRefList(dfc_MENUID, Q_RFDUPREJECT, modconn);
                ViewData();
                //BindData();
                BindData2();
                //BindData3();
            }
            else
            {
                if (Request.Form["sta"] == "save")
                {
                    SaveData();
                }
            }
        }

        private void ViewData()
        {
            object[] paruser = new object[2] { Request.QueryString["gid"], moduleid };
            modconn.ExecReader(Q_USERDATA, paruser, dbtimeout);
            CBL_SEGMEN.Items.Clear();
            while (modconn.hasRow())
            {
                ListItem li = new ListItem();
                li.Value = modconn.GetFieldValue("segmen_id");
                li.Text = modconn.GetFieldValue("segmen_desc");
                li.Selected = (bool)modconn.GetNativeFieldValue("status");
                CBL_SEGMEN.Items.Add(li);
            }
        }

        private void SaveData()
        {
            try
            {

                for (int i = 0; i <= CBL_SEGMEN.Items.Count - 1; i++)
                {
                    object[] p = new object[] { Request.QueryString["gid"], CBL_SEGMEN.Items[i].Value, CBL_SEGMEN.Items[i].Selected };

                    modconn.ExecuteNonQuery(SP_SAVEUSER, p, dbtimeout);
                }

                //MNTTools.InsertModuleUser(Request.QueryString["uid"], moduleid);
            }
            catch (Exception ex)
            {
                Response.Write("<!-- " + ex.Message.Replace("-->", "--)") + " -->\n");
                MNTTools.LogError(this, (string)Session["UserID"], ex);
            }
        }

        #region prod pricing
        #region binddata
        /*private void BindData()
        {
            object[] par = new object[2] { groupid, moduleid };
            grdview.DataSource = modconn.GetDataTable(Q_DATAPRODPRICE, par, dbtimeout);
            grdview.DataBind();

            clearvalues();
        }

        protected void grdview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdview.PageIndex = e.NewPageIndex;
        }

        protected void grdview_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }*/

        private void BindData2()
        {
            object[] par = new object[2] { groupid, moduleid };
            grdview2.DataSource = modconn.GetDataTable(Q_DATADUPAUTH, par, dbtimeout);
            grdview2.DataBind();

            clearvalues();
        }

        protected void grdview2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdview2.PageIndex = e.NewPageIndex;
        }

        protected void grdview2_PageIndexChanged(object sender, EventArgs e)
        {
            BindData2();
        }

        /*private void BindData3()
        {
            object[] par = new object[2] { groupid, moduleid };
            grdview3.DataSource = modconn.GetDataTable(Q_DATATRESHAUTH, par, dbtimeout);
            grdview3.DataBind();

            clearvalues();
        }

        protected void grdview3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdview3.PageIndex = e.NewPageIndex;
        }

        protected void grdview3_PageIndexChanged(object sender, EventArgs e)
        {
            BindData3();
        }*/

        private void clearvalues()
        {
            //dfb_PRODUCTID.ClearSelection();
            //dfb_PRICINGAUTHORITY.Value = 0;
            dfc_MENUID.ClearSelection();
            //dfd_TH_CODE.ClearSelection();
        }
        #endregion

        #region user interaction 
        /*protected void BTN_ADD_Click(object sender, EventArgs e)
        {
            if (dfb_PRODUCTID.SelectedValue == "")
                return;
            object[] par = new object[4] { groupid, moduleid, dfb_PRODUCTID.SelectedValue, dfb_PRICINGAUTHORITY.Value };
            modconn.ExecNonQuery(SP_ADDPRODPRICE, par, dbtimeout);
            BindData();
        }*/

        protected void BTN_ADD2_Click(object sender, EventArgs e)
        {
            if (dfc_MENUID.SelectedValue == "")
                return;
            object[] par = new object[3] { groupid, moduleid, dfc_MENUID.SelectedValue };
            modconn.ExecNonQuery(SP_ADDDUPAUTH, par, dbtimeout);
            BindData2();
        }

        /*protected void BTN_ADD3_Click(object sender, EventArgs e)
        {
            if (dfd_TH_CODE.SelectedValue == "")
                return;
            object[] par = new object[3] { groupid, moduleid, dfd_TH_CODE.SelectedValue };
            modconn.ExecNonQuery(SP_ADDTRESHAUTH, par, dbtimeout);
            BindData3();
        }

        protected void grdview_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string prodid;
            object[] par;
            switch (e.CommandName)
            {
                case "edit":
                    prodid = (string)e.CommandArgument;
                    par = new object[3] { groupid, moduleid, prodid };
                    modconn.ExecReader(Q_PRODPRICE, par, dbtimeout);
                    if (modconn.hasRow())
                    {
                        dfb_PRICINGAUTHORITY.Value = (double)modconn.GetNativeFieldValue("PRICINGAUTHORITY");
                        dfb_PRODUCTID.SelectedValue = prodid;
                    }
                    break;
                case "delete":
                    prodid = (string)e.CommandArgument;
                    par = new object[3] { groupid, moduleid, prodid };
                    modconn.ExecNonQuery(SP_DELPRODPRICE, par, dbtimeout);
                    BindData();
                    break;
                default:
                    break;
            }
        }

        protected void grdview_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            switch (e.RowIndex)
            {
                default:
                    break;
            }
        }

        protected void grdview_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }*/

        protected void grdview2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string rjcode;
            object[] par;
            switch (e.CommandName)
            {
                case "delete":
                    rjcode = (string)e.CommandArgument;
                    par = new object[3] { groupid, moduleid, rjcode };
                    modconn.ExecNonQuery(SP_DELDUPAUTH, par, dbtimeout);
                    BindData2();
                    break;
                default:
                    break;
            }
        }

        protected void grdview2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            switch (e.RowIndex)
            {
                default:
                    break;
            }
        }

        protected void grdview2_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        /*protected void grdview3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string thcode;
            object[] par;
            switch (e.CommandName)
            {
                case "delete":
                    thcode = (string)e.CommandArgument;
                    par = new object[3] { groupid, moduleid, thcode };
                    modconn.ExecNonQuery(SP_DELTRESHAUTH, par, dbtimeout);
                    BindData3();
                    break;
                default:
                    break;
            }
        }

        protected void grdview3_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            switch (e.RowIndex)
            {
                default:
                    break;
            }
        }

        protected void grdview3_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }*/
        #endregion
        #endregion
    }
}
