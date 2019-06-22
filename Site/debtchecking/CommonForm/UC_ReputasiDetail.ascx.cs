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
    public partial class UC_ReputasiDetail : System.Web.UI.UserControl
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
            DataTable dt = conn.GetDataTable("SELECT * FROM vw_reputasidetail WHERE appid = @1 order by bn_code", par, dbtimeout);
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
                    cell1.RowSpan = 2;
                    cell1.Attributes.Add("class", "boxboldcenter");
                    cell1.BgColor = "Orange";
                    cell1.Width = "40%";
                    cell1.Attributes.CssStyle.Add("font-weight", "bold");

                    HtmlTableCell cell2 = new HtmlTableCell();
                    cell2.InnerText = "Kewajiban saat ini";
                    cell2.RowSpan = 2;
                    cell2.Attributes.Add("class", "boxboldcenter");
                    cell2.BgColor = "Yellow";
                    cell2.Width = "25%";

                    HtmlTableCell cell3 = new HtmlTableCell();
                    cell3.InnerText = "Reputasi";
                    cell3.ColSpan = 3;
                    cell3.Attributes.Add("class", "boxboldcenter");
                    cell3.BgColor = "Yellow";
                    cell1.Width = "15%";

                    HtmlTableCell cell4 = new HtmlTableCell();
                    cell4.InnerText = "%";
                    cell4.RowSpan = 2;
                    cell4.Attributes.Add("class", "boxboldcenter");
                    cell4.BgColor = "Yellow";
                    cell4.Width = "15%";

                    HtmlTableCell cell5 = new HtmlTableCell();
                    cell5.InnerText = "1-6 bulan";
                    cell5.Attributes.Add("class", "boxboldcenter");
                    cell5.BgColor = "Yellow";
                    cell5.Width = "15%";

                    HtmlTableCell cell6 = new HtmlTableCell();
                    cell6.InnerText = "7-12 bulan";
                    cell6.Attributes.Add("class", "boxboldcenter");
                    cell6.BgColor = "Yellow";
                    cell6.Width = "15%";

                    HtmlTableCell cell7 = new HtmlTableCell();
                    cell7.InnerText = "13-24 bulan";
                    cell7.Attributes.Add("class", "boxboldcenter");
                    cell7.BgColor = "Yellow";
                    cell7.Width = "10%";

                    HtmlTableRow row1 = new HtmlTableRow();
                    row1.Cells.Add(cell1);
                    row1.Cells.Add(cell2);
                    row1.Cells.Add(cell3);
                    row1.Cells.Add(cell4);
                    HtmlTableRow row2 = new HtmlTableRow();
                    row2.Cells.Add(cell5);
                    row2.Cells.Add(cell6);
                    row2.Cells.Add(cell7);
                    tbl.Rows.Add(row1);
                    tbl.Rows.Add(row2);
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
                celldata3.InnerText = dt.Rows[i]["rep1"].ToString();
                celldata3.Attributes.Add("class", "boxboldcenter");

                HtmlTableCell celldata4 = new HtmlTableCell();
                celldata4.InnerText = dt.Rows[i]["rep2"].ToString();
                celldata4.Align = "center";
                celldata4.Attributes.Add("class", "boxboldcenter");

                HtmlTableCell celldata5 = new HtmlTableCell();
                celldata5.InnerText = dt.Rows[i]["rep3"].ToString();
                celldata5.Align = "center";
                celldata5.Attributes.Add("class", "boxboldcenter");

                HtmlTableCell celldata6 = new HtmlTableCell();
                celldata6.InnerText = FormatedValue(dt.Rows[i]["prc"], "n2") + " %";
                celldata6.Align = "center";
                celldata6.Attributes.Add("class", "boxboldcenter");

                HtmlTableRow rowdata = new HtmlTableRow();
                rowdata.BgColor = "White";

                rowdata.Cells.Add(celldata1);
                rowdata.Cells.Add(celldata2);
                rowdata.Cells.Add(celldata3);
                rowdata.Cells.Add(celldata4);
                rowdata.Cells.Add(celldata5);
                rowdata.Cells.Add(celldata6);
                tbl.Rows.Add(rowdata);
                #endregion
            }
            //add last table
            if (tbl != null)
            {
                tddetail.Controls.Add(new LiteralControl("<div style='position: relative; left: -15px; top: 25px;'>" + (tblnum + 1).ToString() + ".&nbsp; </div>"));
                tddetail.Controls.Add(tbl);
                //add total kewajiban
                tddetail.Controls.Add(new LiteralControl("<div style='font-weight:bold;'>Total : " + FormatedValue(total_os,"n0") + "</div>"));
                
                //add summary reputasi
                #region table reputasi summary header
                HtmlTable tblsum = new HtmlTable();
                tblsum.Width = "40%";
                tblsum.Align = "center";
                tblsum.CellPadding = 0; tblsum.CellSpacing = 0;

                HtmlTableCell cellsum1 = new HtmlTableCell();
                cellsum1.InnerText = "Reputasi Per Periode";
                cellsum1.ColSpan = 3;
                cellsum1.Attributes.Add("class", "boxboldcenter");
                cellsum1.BgColor = "Yellow";
                HtmlTableRow rowsum1 = new HtmlTableRow();
                rowsum1.Cells.Add(cellsum1);

                HtmlTableCell cellsum2 = new HtmlTableCell();
                cellsum2.InnerText = "1-6 bulan";
                cellsum2.Attributes.Add("class", "boxboldcenter");
                cellsum2.BgColor = "Yellow";
                cellsum2.Width = "33%";

                HtmlTableCell cellsum3 = new HtmlTableCell();
                cellsum3.InnerText = "7-12 bulan";
                cellsum3.Attributes.Add("class", "boxboldcenter");
                cellsum3.BgColor = "Yellow";
                cellsum3.Width = "33%";

                HtmlTableCell cellsum4 = new HtmlTableCell();
                cellsum4.InnerText = "13-24 bulan";
                cellsum4.Attributes.Add("class", "boxboldcenter");
                cellsum4.BgColor = "Yellow";
                cellsum4.Width = "33%";

                HtmlTableRow rowsum2 = new HtmlTableRow();
                rowsum2.Cells.Add(cellsum2);
                rowsum2.Cells.Add(cellsum3);
                rowsum2.Cells.Add(cellsum4);

                #endregion

                #region table reputasi summary data
                par = new object[] { appid };
                DataTable dt2 = conn.GetDataTable("SELECT * FROM vw_appreputasidetail WHERE appid = @1", par, dbtimeout);
                if (dt2.Rows.Count == 0)
                {
                    dt2 = conn.GetDataTable("SELECT * FROM vw_appreputasi WHERE appid = @1", par, dbtimeout);
                }
                if (dt2.Rows.Count > 0)
                {
                    HtmlTableCell celldata1 = new HtmlTableCell();
                    celldata1.InnerText = dt2.Rows[0]["repburuk_1"].ToString() + " / " + dt2.Rows[0]["jmlkreditur_1"].ToString();
                    celldata1.Attributes.Add("class", "boxboldcenter");
                    HtmlTableCell celldata2 = new HtmlTableCell();
                    celldata2.InnerText = dt2.Rows[0]["repburuk_2"].ToString() + " / " + dt2.Rows[0]["jmlkreditur_2"].ToString();
                    celldata2.Attributes.Add("class", "boxboldcenter");
                    HtmlTableCell celldata3 = new HtmlTableCell();
                    celldata3.InnerText = dt2.Rows[0]["repburuk_3"].ToString() + " / " + dt2.Rows[0]["jmlkreditur_3"].ToString();
                    celldata3.Attributes.Add("class", "boxboldcenter");
                    HtmlTableRow rowsum3 = new HtmlTableRow();
                    rowsum3.Cells.Add(celldata1);
                    rowsum3.Cells.Add(celldata2);
                    rowsum3.Cells.Add(celldata3);

                    tblsum.Rows.Add(rowsum1);
                    tblsum.Rows.Add(rowsum2);
                    tblsum.Rows.Add(rowsum3);
                    tddetail.Controls.Add(tblsum);
                }
                #endregion

            }
        }

    }
}