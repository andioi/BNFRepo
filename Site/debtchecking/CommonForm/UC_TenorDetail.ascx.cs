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

using DMS.Tools;

namespace DebtChecking.CommonForm
{
    public partial class UC_TenorDetail : System.Web.UI.UserControl
    {
        #region Connection & class variables
        private int dbtimeout;
        private DbConnection conn;
        private DataSet dsRep = null;
        #endregion

        public string appid { get; set; }
        public string cust_name { get; set; }

        #region Additional Function
        public string FormatedValue(object value)
        {
            string FormatType = null;
            if (value is Int32 || value is Int64 || value is float || value is double || value is decimal)
                FormatType = "n0";
            if (value is DateTime)
                FormatType = "dd MMMM yyyy";
            return FormatedValue(value, FormatType);
        }
        public string FormatedValue(object value, string FormatType)
        {
            if (value == DBNull.Value)
                value = "";
            if (FormatType != null)
            {
                if (value is Int32)
                    value = ((Int32)value).ToString(FormatType);
                else if (value is Int64)
                    value = ((Int64)value).ToString(FormatType);
                else if (value is float)
                    value = ((float)value).ToString(FormatType);
                else if (value is double)
                    value = ((double)value).ToString(FormatType);
                else if (value is decimal)
                    value = ((decimal)value).ToString(FormatType);
                else if (value is DateTime)
                    value = ((DateTime)value).ToString(FormatType);
            }
            return value.ToString();
        }

        public string DS(int tbl, string FieldName)
        {
            try
            {
                object value = dsRep.Tables[tbl].Rows[0][FieldName];
                return FormatedValue(value);
            }
            catch
            {
                return "";
            }
        }

        public string DS(int tbl, string FieldName, string FormatType)
        {
            try
            {
                if (dsRep.Tables[tbl].Rows.Count == 0)
                    return "";
                object value = dsRep.Tables[tbl].Rows[0][FieldName];
                return FormatedValue(value, FormatType);
            }
            catch
            {
                return "";
            }
        }

        public string DS_SUM(int tbl, string FieldName, string sumtype)
        {
            return DS_SUM(tbl, FieldName, sumtype, null);
        }
        public string DS_SUM(int tbl, string FieldName, string sumtype, string FormatType)
        {
            DataTable dt = dsRep.Tables[tbl];

            double value = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (sumtype)
                {
                    case "SUM":
                        if (dt.Rows[i][FieldName] != DBNull.Value)
                            value += double.Parse(dt.Rows[i][FieldName].ToString());
                        break;
                    case "CNT":
                        value += 1;
                        break;
                    case "AVG":
                        if (dt.Rows[i][FieldName] != DBNull.Value)
                            value += Convert.ToSingle(dt.Rows[i][FieldName]);
                        break;

                }
            }
            if (sumtype == "AVG" && value != 0)
                value = value / dt.Rows.Count;

            if (value != 0)
            {
                if (FormatType == null)
                    return FormatedValue(value);
                else
                    return FormatedValue(value, FormatType);
            }
            else return "";
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["bypasssession"] == "1")
                {
                    dbtimeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
                    conn = new DbConnection(MasterPage.decryptConnStr(ConfigurationSettings.AppSettings["eModuleConnectString"]));
                }
                else
                {
                    dbtimeout = (int)Session["dbTimeOut"];
                    conn = new DbConnection((string)Session["ConnString"]);
                }
                
                using (conn)
                {
                    retrieve_datacustomer(Request.QueryString["reffnum"], false);
                }
            }
        }

        private void retrieve_datacustomer(string key, bool force_calc)
        {
            label_name.Text = cust_name;

            object[] par = new object[] { appid };
            DataTable dt = conn.GetDataTable("SELECT * FROM vw_appreputasitenor WHERE appid = @1 order by bn_code", par, dbtimeout);
            if (dt.Rows.Count == 0) Content.Attributes.CssStyle.Add("display", "none");
            string tmpbank=""; HtmlTable tbl = null; int tblnum = 0; double total_os = 0;
            for (int i=0;i < dt.Rows.Count;i++)
            {
                if (tmpbank != dt.Rows[i]["bank_name"].ToString())
                {
                    tmpbank = dt.Rows[i]["bank_name"].ToString();
                    if (tbl != null)
                    {
                        tblnum++;
                        tddetail.Controls.Add(new LiteralControl("<div style='position: relative; left: -15px; top: 25px;'>"+tblnum.ToString()+".&nbsp; </div>"));
                        tddetail.Controls.Add(tbl);
                    }

                    //create new table
                    #region create table
                    tbl = new HtmlTable();
                    tbl.Width = "95%";
                    tbl.Attributes.CssStyle.Add("margin-bottom", "25px");
                    tbl.CellPadding = 2; tbl.CellSpacing = 0;
                    #endregion

                    #region create header
                    HtmlTableCell cell1 = new HtmlTableCell();
                    cell1.InnerText = dt.Rows[i]["bank_name"].ToString();
                    cell1.Attributes.Add("class", "boxboldcenter");
                    cell1.BgColor = "Orange";
                    cell1.Width = "22%";

                    HtmlTableCell cell2 = new HtmlTableCell();
                    cell2.InnerText = "Kewajiban saat ini";
                    cell2.Attributes.Add("class", "boxboldcenter");
                    cell2.BgColor = "Yellow";
                    cell2.Width = "15%";

                    HtmlTableCell cell3 = new HtmlTableCell();
                    cell3.InnerText = "Limit";
                    cell3.Attributes.Add("class", "boxboldcenter");
                    cell3.BgColor = "Yellow";
                    cell3.Width = "15%";

                    HtmlTableCell cell4 = new HtmlTableCell();
                    cell4.InnerText = "Tgl Awal";
                    cell4.Attributes.Add("class", "boxboldcenter");
                    cell4.BgColor = "Yellow";
                    cell4.Width = "10%";

                    HtmlTableCell cell5 = new HtmlTableCell();
                    cell5.InnerText = "Tgl Laporan";
                    cell5.Attributes.Add("class", "boxboldcenter");
                    cell5.BgColor = "Yellow";
                    cell5.Width = "10%";

                    HtmlTableCell cell6 = new HtmlTableCell();
                    cell6.InnerText = "Tgl Akhir";
                    cell6.Attributes.Add("class", "boxboldcenter");
                    cell6.BgColor = "Yellow";
                    cell6.Width = "10%";

                    HtmlTableCell cell7 = new HtmlTableCell();
                    cell7.InnerText = "Total Tenor";
                    cell7.Attributes.Add("class", "boxboldcenter");
                    cell7.BgColor = "Yellow";
                    cell7.Width = "6%";

                    HtmlTableCell cell8 = new HtmlTableCell();
                    cell8.InnerText = "Tenor Berjalan";
                    cell8.Attributes.Add("class", "boxboldcenter");
                    cell8.BgColor = "Yellow";
                    cell8.Width = "6%";

                    HtmlTableCell cell9 = new HtmlTableCell();
                    cell9.InnerText = "Sisa Tenor";
                    cell9.Attributes.Add("class", "boxboldcenter");
                    cell9.BgColor = "Yellow";
                    cell9.Width = "6%";

                    HtmlTableRow row1 = new HtmlTableRow();
                    row1.Cells.Add(cell1);
                    row1.Cells.Add(cell2);
                    row1.Cells.Add(cell3);
                    row1.Cells.Add(cell4);
                    row1.Cells.Add(cell5);
                    row1.Cells.Add(cell6);
                    row1.Cells.Add(cell7);
                    row1.Cells.Add(cell8);
                    row1.Cells.Add(cell9);
                    tbl.Rows.Add(row1);
                    #endregion

                }

                #region fill data
                HtmlTableCell celldata1 = new HtmlTableCell();
                celldata1.InnerText = dt.Rows[i]["jenis_kredit"].ToString();
                celldata1.Attributes.Add("class", "boxboldleft");

                HtmlTableCell celldata2 = new HtmlTableCell();
                celldata2.InnerText = FormatedValue(dt.Rows[i]["baki_debet"],"n0");
                total_os = total_os + (double)dt.Rows[i]["baki_debet"];
                celldata2.Attributes.Add("class", "boxboldright");

                HtmlTableCell celldata3 = new HtmlTableCell();
                celldata3.InnerText = FormatedValue(dt.Rows[i]["plafon"], "n0");
                celldata3.Attributes.Add("class", "boxboldright");

                HtmlTableCell celldata4 = new HtmlTableCell();
                celldata4.InnerText = FormatedValue(dt.Rows[i]["akad_awal"], "dd-MMM-yy");
                celldata4.Attributes.Add("class", "boxboldcenter");

                HtmlTableCell celldata5 = new HtmlTableCell();
                celldata5.InnerText = FormatedValue(dt.Rows[i]["tgl_update"], "dd-MMM-yy");
                celldata5.Attributes.Add("class", "boxboldcenter");

                HtmlTableCell celldata6 = new HtmlTableCell();
                celldata6.InnerText = FormatedValue(dt.Rows[i]["jatuh_tempo"], "dd-MMM-yy");
                celldata6.Attributes.Add("class", "boxboldcenter");

                HtmlTableCell celldata7 = new HtmlTableCell();
                celldata7.InnerText = FormatedValue(dt.Rows[i]["total_tenor"]);
                celldata7.Attributes.Add("class", "boxboldright");

                HtmlTableCell celldata8 = new HtmlTableCell();
                celldata8.InnerText = FormatedValue(dt.Rows[i]["tenor_berjalan"]);
                celldata8.Attributes.Add("class", "boxboldright");

                HtmlTableCell celldata9 = new HtmlTableCell();
                celldata9.InnerText = FormatedValue(dt.Rows[i]["sisa_tenor"]);
                celldata9.Attributes.Add("class", "boxboldright");

                HtmlTableRow rowdata = new HtmlTableRow();
                rowdata.BgColor = "White";

                rowdata.Cells.Add(celldata1);
                rowdata.Cells.Add(celldata2);
                rowdata.Cells.Add(celldata3);
                rowdata.Cells.Add(celldata4);
                rowdata.Cells.Add(celldata5);
                rowdata.Cells.Add(celldata6);
                rowdata.Cells.Add(celldata7);
                rowdata.Cells.Add(celldata8);
                rowdata.Cells.Add(celldata9);
                tbl.Rows.Add(rowdata);
                #endregion
            }
            //add last table
            if (tbl != null)
            {
                tddetail.Controls.Add(new LiteralControl("<div style='position: relative; left: -15px; top: 25px;'>" + (tblnum + 1).ToString() + ".&nbsp; </div>"));
                tddetail.Controls.Add(tbl);

            }
        }

    }
}