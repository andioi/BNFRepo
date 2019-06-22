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
    public partial class UC_ReputasiCustomer : System.Web.UI.UserControl
    {
        #region Connection & class variables
        private int dbtimeout;
        private DbConnection conn;
        private DataSet dsRep = null;
        #endregion

        public string appid { get; set; }

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
            object[] par = new object[] { key, appid };
            DataTable dt = conn.GetDataTable("SELECT * FROM vw_appreputasidetail WHERE reffnumber = @1 and appid = @2", par, dbtimeout);
            dsRep = new DataSet();
            dsRep.Tables.Add(dt);
        }

    }
}