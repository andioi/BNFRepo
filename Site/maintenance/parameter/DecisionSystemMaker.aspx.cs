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
using My.Tools;
using My.Framework;

namespace MikroMnt.Parameter
{
    public partial class DecisionSystemMaker : System.Web.UI.Page
    {
        DbConnection conn = null;
        int dbtimeout = 600;
        string clearjs;
        string CARD_COND_TMPL="";

        #region retrieve & save & delete & gridbind

        #region initial_reffrential_parameter
        private void initial_reffrential_parameter()
        {
            object[] param = new object[] { Request.QueryString["DEC_ID"] };
            staticFramework.reff(ITEM_ID, 
                "SELECT ITEM_ID, ITEM_DESC FROM DECISIONSYSTEMITEM WHERE DEC_ID=@1 ", 
                param, conn);
            staticFramework.reff(RES01,
                "SELECT RES_ID, RES_DESC FROM DECISIONSYSTEMRESULT WHERE DEC_ID=@1 ",
                param, conn);
            staticFramework.reff(TMPLCOND,
                "SELECT FIELDID,FIELDDESC FROM FRAMEWORK_FORMULAFIELD ",
                param, conn);
        }
        #endregion

        #region retrieve_data & schema
        private void retrieve_schema()
        {
            DataTable dt = conn.GetDataTable("SELECT TOP 0 * FROM DECISIONSYSTEMCARD", null, dbtimeout, true, true);
            staticFramework.retrieveschema(dt, ITEM_ID);
            staticFramework.retrieveschema(dt, CARD_COND);
            staticFramework.retrieveschema(dt, RES01);
        }

        private void retrieve_data(string CardID)
        {
            object[] param = new object[] { CardID };
            conn.ExecReader("SELECT * FROM DECISIONSYSTEMCARD WHERE CARD_ID=@1", 
                param, dbtimeout);
            if (conn.hasRow())
            {
                staticFramework.retrieve(conn, CARD_ID);
                staticFramework.retrieve(conn, ITEM_ID);
                string[] CARD_COND_FW = conn.GetFieldValue("CARD_COND").Split(new string[]{"AND/n" },StringSplitOptions.RemoveEmptyEntries);
                string[] CARD_COND_DESC = conn.GetFieldValue("CARD_COND_DESC").Split(new string[]{"/n"},StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < CARD_COND_FW.Length; i++)
                {
                    ListItem li = new ListItem(CARD_COND_DESC[i], CARD_COND_FW[i]);
                    CARD_COND.Items.Add(li);
                }

                staticFramework.retrieve(conn, "CARD_COND", hCARD_COND_FW);
                staticFramework.retrieve(conn, "CARD_COND_DESC", hCARD_COND_DESC);
                staticFramework.retrieve(conn, RES01);
            }
        }
        private void delete_data(string CardID)
        {
            NameValueCollection Keys = new NameValueCollection();
            staticFramework.saveNVC(Keys, "CARD_ID", CardID);
            staticFramework.Delete(Keys, "DECISIONSYSTEMCARD", conn);
        }

        private void gridbind()
        {
            grid.DataSource = conn.GetDataTable(
                "SELECT * FROM VW_DECISIONSYSTEMCARD WHERE DEC_ID=@1 AND CARD_COND_TMPL=@2",
                new object[] { Request.QueryString["DEC_ID"], CARD_COND_TMPL }, dbtimeout);
            grid.DataBind();
            if (grid.GroupCount > 0)
                funcCss = "";
        }
        #endregion

        #region save_data
        private void save_data()
        {
            NameValueCollection Keys = new NameValueCollection();
            if (CARD_ID.Value == "")
                Keys["CARD_ID"] = "NEWID()";
            else
                staticFramework.saveNVC(Keys, CARD_ID);

            NameValueCollection Fields = new NameValueCollection();
            staticFramework.saveNVC(Fields, ITEM_ID);
            staticFramework.saveNVC(Fields, "CARD_COND", hCARD_COND_FW);
            staticFramework.saveNVC(Fields, "CARD_COND_DESC", hCARD_COND_DESC);
            staticFramework.saveNVC(Fields, RES01);
            staticFramework.saveNVC(Fields, "DEC_ID", Request.QueryString["DEC_ID"]);
            staticFramework.saveNVC(Fields, "CARD_COND_TMPL", CARD_COND_TMPL);
            staticFramework.saveNVC(Fields, "STAGE", "1");
            staticFramework.save(Fields, Keys, "DECISIONSYSTEMCARD", conn);
        }
        #endregion

        #endregion
        
        public string BackURL
        {
            get { return (string)Session["BackURL"]; }
        }
        public string funcCss = "hide";
        public string funcpendCss = "hide";

        #region init page
        protected override void OnLoad(EventArgs e)
        {
            string modid = "61";
            if (Request.QueryString["moduleid"] != null && Request.QueryString["moduleid"].Trim() != "")
                modid = Request.QueryString["moduleid"];
            conn = new DbConnection(MNTTools.GetConnString(modid));
            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            try
            {
                conn.Dispose();
            }
            catch { }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            clearjs += "document.getElementById('" + CARD_ID.ClientID + "').value = '';";
            clearjs += "document.getElementById('" + ITEM_ID.ClientID + "').value = '';";
            clearjs += "clearlistbox(document.getElementById('" + CARD_COND.ClientID + "'));";
            clearjs += "document.getElementById('" + RES01.ClientID + "').value = '';";
            clearjs += "document.getElementById('" + TMPLCOND.ClientID + "').value = '';";
            clearjs += "document.getElementById('" + TMPLCONDREFF.ClientID + "').style.display = 'none';";
            clearjs += "document.getElementById('" + TMPLCONDCUSTOM.ClientID + "').style.display = 'none';";
            clrButton.Attributes["onclick"] = clearjs;
        }
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !IsCallback)
            {
                title.Text = Request.QueryString["title"];
                initial_reffrential_parameter();
                retrieve_schema();
            }
            if(Request.QueryString["pksid"]!=null)
                CARD_COND_TMPL += "AND @[PKS|PKSID]='" + Request.QueryString["pksid"] + "'";
            if(CARD_COND_TMPL!="")
                CARD_COND_TMPL = CARD_COND_TMPL.Substring(4);
        }

        protected void panel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            panel.JSProperties["cp_action"] = "";
            if (e.Parameter.StartsWith("s:"))
            {
                panel.JSProperties["cp_action"] = "s";
                save_data();   
            }
            else if (e.Parameter.StartsWith("r:"))
            {
                panel.JSProperties["cp_action"] = "r";
                retrieve_data(e.Parameter.Substring(2));
            }

        }

        protected void grid_Load(object sender, EventArgs e)
        {
            gridbind();
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                delete_data(e.Parameters.Substring(2));
                gridbind();
            }
        }

        protected void grid_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (grid.GroupCount > 0)
                funcCss = "";
            else
                funcCss = "hide";

        }

        protected void gridpending_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (gridpending.GroupCount > 0)
                funcpendCss = "";
        }

        protected void gridpending_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void condpanel_Load(object sender, EventArgs e)
        {
            if (IsCallback)
            {
                TMPLCONDREFF.Style["display"] = "none";
                TMPLCONDCUSTOM.Style["display"] = "none";

                object[] par = new object[] { TMPLCOND.SelectedValue };
                conn.ExecReader("SELECT FIELDREFF FROM FRAMEWORK_FORMULAFIELD WHERE FIELDID = @1 ", par, dbtimeout);
                if (conn.hasRow() && conn.GetFieldValue(0) != "")
                {
                    if (conn.GetFieldValue(0) != "CUSTOM")
                    {
                        staticFramework.reff(TMPLCONDREFF, conn.GetFieldValue(0), null, conn);
                        TMPLCONDREFF.Style["display"] = "";
                    }
                    else
                    {
                        TMPLCONDCUSTOM.Style["display"] = "";
                    }
                }
            }
        }
    }
}
