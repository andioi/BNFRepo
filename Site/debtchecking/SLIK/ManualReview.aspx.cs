using System;
using System.Collections;
using System.Configuration;
using System.Data;
using DMS.Framework;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using DevExpress.Web;
using DMS.Tools;
using EvoPdf.HtmlToPdf;
using System.Collections.Generic;

namespace DebtChecking.Verification
{
    public partial class ManualReview : DataPage
    {
        int dincnt = 0;

        private void retrieve_debiturinfo(string key)
        {
            string sql = "select * from slik_vw_applicant where appid = @1";
            DataTable dt = conn.GetDataTable(sql, new object[] { key }, dbtimeout);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, reffnumber);
            string Q_NAME_LIST = "select appid, cust_name+' ('+status_app+')' cust_name from slik_applicant where " +
                "reffnumber = (select reffnumber from slik_applicant where appid = @1)";
            staticFramework.reff(ddl_appid, Q_NAME_LIST, new object[] { key }, conn, false);
            staticFramework.retrieve(dt, "appid", ddl_appid);
            staticFramework.retrieve(dt, status_app);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, pob_dob);
            staticFramework.retrieve(dt, ktp);
            staticFramework.retrieve(dt, npwp);
            staticFramework.retrieve(dt, genderdesc);
            staticFramework.retrieve(dt, mother_name);
            staticFramework.retrieve(dt, full_ktpaddress);
            staticFramework.retrieve(dt, full_homeaddress);
            staticFramework.retrieve(dt, full_officeaddress);
            staticFramework.retrieve(dt, full_econaddress);
            staticFramework.retrieve(dt, final_policy);
            staticFramework.retrieve(dt, HADD_MATCH);
            staticFramework.retrieve(dt, BADD_MATCH);
            staticFramework.retrieve(dt, COYNAME_MATCH);
        }

        private void retrieve_datanik(string key, int scoremin, int scoremax, string color, Control div, string prefix_chk, Label lblnodata)
        {
            object[] par = new object[] { key, scoremin, scoremax };
            DataTable dt = conn.GetDataTable("select * from vw_manual_review where appid =@1", par, dbtimeout);

            #region create table
            HtmlTable tbl = new HtmlTable();
            tbl.Width = "95%";
            tbl.Border = 0;
            tbl.CellPadding = 10; tbl.CellSpacing = 20;
            #endregion

            #region looping data
            HtmlTableRow row = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i % 2 == 0) {row = new HtmlTableRow();}
                HtmlTableCell cell = new HtmlTableCell();
                cell.Align = "center";cell.VAlign = "top";
                cell.Width = "50%";
                HtmlTable tblc = generate_tablenik(dt, i, color, prefix_chk);
                cell.Controls.Add(tblc);
                row.Cells.Add(cell);
                if (i % 2 == 1) { tbl.Rows.Add(row);}
            }
            if (dt.Rows.Count % 2 == 1)
            {
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerHtml = "&nbsp;";
                cell.Width = "50%";
                row.Cells.Add(cell);
                tbl.Rows.Add(row);
            }
            #endregion

            if (dt.Rows.Count > 0)
            { 
                div.Controls.Add(tbl);
                lblnodata.Visible = false;
                dincnt = dincnt + dt.Rows.Count;
            }
        }

        private HtmlTable generate_tablenik(DataTable dt, int datarow, string color, string prefix_chk)
        {
            #region create table
            HtmlTable tbl = new HtmlTable();
            tbl.Width = "100%";
            tbl.Border = 0;
            tbl.CellPadding = 2; tbl.CellSpacing = 0;
            #endregion

            HtmlTableCell cell;
            HtmlTableRow row;

            #region checkbox & flag
            row = new HtmlTableRow();

            cell = new HtmlTableCell(); cell.Width = "30%"; cell.Align = "left";
            cell.Style.Add("font-size", "small");
            //hidden
            HiddenField hdn = new HiddenField();
            hdn.ID = "h" + prefix_chk + datarow.ToString();
            hdn.Value = dt.Rows[datarow]["nik"].ToString();
            cell.Controls.Add(hdn);
            //checkbox
            CheckBox chk = new CheckBox();
            chk.ID = prefix_chk + datarow.ToString();
            chk.Checked = bool.Parse(dt.Rows[datarow]["match_flag"].ToString());
            if (chk.Checked) chk.ForeColor = System.Drawing.Color.Red;
            chk.Attributes.Add("onclick", "sameprsn_tick(this)");
            chk.Text = "Same Person";chk.Font.Bold = true;
            if (Request.QueryString["QC"] != null) chk.Enabled = false;
            cell.Controls.Add(chk);
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.Style.Add("font-size", "small");
            cell.Width = "35%"; cell.Align = "left";
            if (Request.QueryString["QC"] != null & prefix_chk == "cm_")
            {
                //checkbox qc
                CheckBox chkqc = new CheckBox();
                chkqc.ID = "qc" + prefix_chk + datarow.ToString();
                chkqc.Checked = bool.Parse(dt.Rows[datarow]["match_qc_flag"].ToString());
                chkqc.Text = "Same Person (QC)"; chkqc.Font.Bold = true;
                cell.Controls.Add(chkqc);
            }
            row.Cells.Add(cell);

            //match group
            cell = new HtmlTableCell(); cell.Style.Add("font-size", "small");
            cell.Width = "35%"; cell.Align = "right";
            Label lbl = new Label();
            lbl.Text = "Data Matching : <b>" + dt.Rows[datarow]["match_group"].ToString() + "</b>";
            cell.Controls.Add(lbl);

            row.Cells.Add(cell);
            tbl.Rows.Add(row);
            #endregion

            #region din
            row = new HtmlTableRow();
            
            cell = new HtmlTableCell();
            cell.InnerText = "NIK"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerText = dt.Rows[datarow]["nik"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region nama
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.InnerText = "NAMA"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerHtml = dt.Rows[datarow]["nama"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region dob
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.InnerText = "DOB"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerHtml = dt.Rows[datarow]["dob"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region pob
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.InnerText = "POB"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerHtml = dt.Rows[datarow]["pob"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region idnumber
            //row = new HtmlTableRow();

            //cell = new HtmlTableCell();
            //cell.InnerText = "ID#"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            //cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell(); cell.ColSpan = 2;
            //cell.InnerHtml = dt.Rows[datarow]["idnumber"].ToString();
            //cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            //row.Cells.Add(cell);

            //tbl.Rows.Add(row);
            #endregion

            #region npwp
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.InnerText = "NPWP"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerHtml = dt.Rows[datarow]["npwp"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region address
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.InnerText = "ADD"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerHtml = dt.Rows[datarow]["sid_address"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region office name
            row = new HtmlTableRow();

            cell = new HtmlTableCell();
            cell.InnerText = "COMPANY NAME"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            row.Cells.Add(cell);

            cell = new HtmlTableCell(); cell.ColSpan = 2;
            cell.InnerHtml = dt.Rows[datarow]["office_name"].ToString();
            cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            row.Cells.Add(cell);

            tbl.Rows.Add(row);
            #endregion

            #region phone
            //row = new HtmlTableRow();

            //cell = new HtmlTableCell();
            //cell.InnerText = "PHONE"; cell.Width = "30%"; cell.Attributes.Add("class", "boxboldleft");
            //cell.BgColor = color; cell.Style.Add("font-weight", "bold");
            //row.Cells.Add(cell);

            //cell = new HtmlTableCell(); cell.ColSpan = 2;
            //cell.InnerHtml = dt.Rows[datarow]["sid_phone"].ToString();
            //cell.Width = "70%"; cell.Attributes.Add("class", "boxboldleft");
            //row.Cells.Add(cell);

            //tbl.Rows.Add(row);
            #endregion

            return tbl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable dt = conn.GetDataTable("select din from appdin where appid=@1 and total_score is null",
            //    new object[] { Request.QueryString["regno"] }, dbtimeout);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    object[] par = new object[] { Request.QueryString["regno"], dt.Rows[i]["nik"].ToString() };
            //    conn.ExecNonQuery("exec sp_validationdin @1,@2", par, dbtimeout);
            //}
            if (!IsPostBack)
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
            }
            else
            {
                retrieve_debiturinfo(ddl_appid.SelectedValue);
            }
            retrieve_datanik(appid.Value, 0, 3, "PeachPuff", divContent, "cm_", lblnodata1);
            //retrieve_datadin(Request.QueryString["regno"], 4, 99, "lightskyblue", divContent2, "ca_", lblnodata2);
            //string script = "<script>parent.document.getElementById('btnsave').style.display = 'none';parent.document.getElementById('btnexport').style.display = 'none';</script>";
            //if (dincnt == 0) Response.Write(script);
            //if (!IsPostBack)
            //{
            //    dt = conn.GetDataTable("select * from vw_app_phnver_flag where appid=@1",
            //        new object[] { Request.QueryString["regno"] }, dbtimeout);
            //    staticFramework.retrieve(dt, phonever_flag);
            //    staticFramework.retrieve(dt, phonever_flag_qc);
            //}
        }

        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {
                if (c is HiddenField) list.Add(c);
                if (c is CheckBox) list.Add(c);
                if (c.Controls.Count > 0)
                    list = GetAllControls(c, list);
            }

            return list;
        }
        private List<Control> GetAllControls(Control container)
        {
            return GetAllControls(container, new List<Control>());
        }

        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter == "s")
            {

                #region save
                List<Control> ControlList = GetAllControls(divContent);
                string din = null; bool sameperson = false;

                NameValueCollection Keys = new NameValueCollection();
                staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
                NameValueCollection Fields = new NameValueCollection();
                staticFramework.saveNVC(Fields, "home_addr_match", HADD_MATCH);
                staticFramework.saveNVC(Fields, "office_addr_match", BADD_MATCH);
                staticFramework.saveNVC(Fields, "coyname_match", COYNAME_MATCH);
                staticFramework.saveNVC(Fields, "match_addr_by", USERID);
                Fields["match_addr_date"] = "GETDATE()";
                staticFramework.save(Fields, Keys, "appverassignment", conn);

                if (Request.QueryString["QC"] == null)
                {
                    foreach (Control ctl in ControlList)
                    {
                        if (ctl.ID.IndexOf("hcm_") == 0) { din = (ctl as HiddenField).Value; }
                        if (ctl.ID.IndexOf("cm_") == 0)
                        {
                            sameperson = (ctl as CheckBox).Checked;
                            object[] par = new object[] { Request.QueryString["regno"], din, sameperson, USERID };
                            conn.ExecNonQuery("exec sp_manualreviewsave @1,@2,@3,@4", par, dbtimeout);
                        }
                    }
                } else
                {
                    foreach (Control ctl in ControlList)
                    {
                        if (ctl.ID.IndexOf("hcm_") == 0) { din = (ctl as HiddenField).Value; }
                        if (ctl.ID.IndexOf("qccm_") == 0)
                        {
                            sameperson = (ctl as CheckBox).Checked;
                            object[] par = new object[] { Request.QueryString["regno"], din, sameperson, USERID };
                            conn.ExecNonQuery("exec sp_manualqcsave @1,@2,@3,@4", par, dbtimeout);
                        }
                    }
                    object[] par2 = new object[] { Request.QueryString["regno"], null, null, USERID };
                    conn.ExecNonQuery("exec sp_manualqcsave @1,@2,@3,@4", par2, dbtimeout);
                }
                ControlList = GetAllControls(divContent2);
                foreach (Control ctl in ControlList)
                {
                    if (ctl.ID.IndexOf("hca_") == 0) { din = (ctl as HiddenField).Value; }
                    if (ctl.ID.IndexOf("ca_") == 0) {
                        sameperson = (ctl as CheckBox).Checked;
                        object[] par = new object[] { Request.QueryString["regno"], din, sameperson, USERID };
                        conn.ExecNonQuery("exec sp_manualreviewsave @1,@2,@3,@4", par, dbtimeout);
                    }
                }
                #endregion

                if (dincnt == 0) btnsave.Visible = false;
                mainPanel.JSProperties["cp_alert"] = "Data saved.";
                mainPanel.JSProperties["cp_redirect"] = Request.Url.AbsoluteUri;
            }
        }
    }
}
