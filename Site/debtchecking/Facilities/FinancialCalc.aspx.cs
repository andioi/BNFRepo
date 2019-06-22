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
using System.IO;
using DMS.Tools;
using DMS.Framework;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace DebtChecking.Facilities
{
    public partial class FinancialCalc : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        private string Q_VW_LENDING = "select * from vw_appfincal_lending where reffnumber=@1 and category = @2 order by seq";
        private string Q_VW_CCBANK = "exec SP_VW_CALCLENDING_CC @1";
        private string Q_VW_PLBANK = "exec SP_VW_KREDIT_PLBANK @1";
        private string Q_VW_CCOTHBANK = "exec SP_VW_KREDIT_CCOTHBANK @1";
        private string Q_VW_CCOTHBANK_NONSCB = "exec SP_VW_KREDIT_CCOTHBANK_NONSCB @1";
        private string Q_VW_CCOTHBANK_SCB = "exec SP_VW_KREDIT_CCOTHBANK_SCB @1";
        private string Q_VW_CARBANK = "exec SP_VW_KREDIT_CARBANK @1";
        private string Q_VW_PLOTHBANK = "exec SP_VW_KREDIT_PLOTHBANK @1";
        private string Q_VW_KPRBANK = "exec SP_VW_KREDIT_KPRBANK @1";
        private string Q_VW_CONSOTHBANK = "exec SP_VW_KREDIT_CONSOTHBANK @1";
        private string Q_LOAD_LENDINGDATA = "exec SP_CALC_LOADLENDING @1";
        private string Q_VW_FINCAL = "select * from vw_appfincal where reffnumber = @1";
        DataSet dsMMUE = null;
        #endregion        

        #region initial
        protected void initial_reffrential_parameter()
        {
            staticFramework.reff(product, "select * FROM rfproduct_calc", null, conn);
        }

        private void retrieve_schema()
        {
            DataTable dt = conn.GetDataTable("select top 0 * from appfincal", null, dbtimeout);
            staticFramework.retrieveschema(dt, reffnumber);
            staticFramework.retrieveschema(dt, fincal_date);
            staticFramework.retrieveschema(dt, msc_code);
            staticFramework.retrieveschema(dt, product);
            staticFramework.retrieveschema(dt, program);
            staticFramework.retrieveschema(dt, limit);
            staticFramework.retrieveschema(dt, tenor);
            staticFramework.retrieveschema(dt, rate);
            staticFramework.retrieveschema(dt, monthly_income);
            staticFramework.retrieveschema(dt, yearly_income);
            staticFramework.retrieveschema(dt, join_income);
        }

        private void load_lendingdata(string key)
        {
            object[] par = new object[] { key };
            conn.ExecuteNonQuery(Q_LOAD_LENDINGDATA, par, dbtimeout);
            conn.ExecNonQuery("exec SP_CALC_BICSUMMARY @1", par, dbtimeout);
        }

        public bool isActive
        {
            get { return !(Session["ApprovalGroup"].ToString() != "1"); }
        }
        #endregion

        #region retrieve

        private void retrieve_datafincal(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_FINCAL, par, dbtimeout);
            staticFramework.retrieve(dt, reffnumber);
            staticFramework.retrieve(dt, fincal_date);
            staticFramework.retrieve(dt, msc_code);
            staticFramework.retrieve(dt, appl_name);
            staticFramework.retrieve(dt, h_appl_name, "h_");
            staticFramework.retrieve(dt, product);
            staticFramework.retrieve(dt, h_product, "h_");
            par = new object[] { h_product.Value };
            staticFramework.reff(program, "select * from rfprogram_calc where productid = @1", par, conn);
            staticFramework.retrieve(dt, program);
            staticFramework.retrieve(dt, h_program, "h_");
            staticFramework.retrieve(dt, limit);
            staticFramework.retrieve(dt, tenor);
            staticFramework.retrieve(dt, rate);
            staticFramework.retrieve(dt, monthly_income);
            staticFramework.retrieve(dt, yearly_income);
            staticFramework.retrieve(dt, join_income);
            staticFramework.retrieve(dt, other_installment);
            staticFramework.retrieve(dt, aum);
            staticFramework.retrieve(dt, mob_aum);

            string excelPath = "../Templates/Download/" + key.Trim() + ".xls";
            string svrPath = Server.MapPath(excelPath);
            if (File.Exists(svrPath))
            {
                btndownload1.Visible = false;
                btndownload1.Attributes.Add("onclick", "javascript:download('" + excelPath + "')");
                btndownload2.Visible = false;
                btndownload2.Attributes.Add("onclick", "javascript:download('" + excelPath + "')");
                btndownload3.Visible = false;
                btndownload3.Attributes.Add("onclick", "javascript:download('" + excelPath + "')");
            }
            else
            {
                btndownload1.Visible = true;
                btndownload2.Visible = true;
                btndownload3.Visible = true;
            }
        }

        private void retrieve_bisummary(string key)
        {
            DataTable dt = conn.GetDataTable("select * from vw_appfincal_bicsummary where reffnumber = @1", new object[] { key }, dbtimeout);
            DataTable dt2 = conn.GetDataTable("select * from vw_appfincal_summary where reffnumber = @1", new object[] { key }, dbtimeout);
            dsMMUE = new DataSet();
            dsMMUE.Tables.Add(dt);
            dsMMUE.Tables.Add(dt2);
        }

        #endregion

        #region databinding
        private void gridbindall()
        {
            gridbind_lending(GridCC, "CC");
            gridbind_lending(GridUnsecured, "UL");
            gridbind_lending(GridSecured, "SL");
            gridbind_lending(GridMK, "MKI");
            gridbind_lending(GridInvest, "INV");
            gridbind_lending(GridRK, "PRK");
        }
    
        private void gridbind_lending(DevExpress.Web.ASPxGridView Grid, string category)
        {
            string key = reffnumber.Text;
            object[] par = new object[] { key, category };
            DataTable dt = conn.GetDataTable(Q_VW_LENDING, par, dbtimeout);
            Grid.DataSource = dt;
            Grid.DataBind();
        }
        #endregion

        #region save
        private void save_data()
        {
            NameValueCollection Keys = new NameValueCollection();
            staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
            NameValueCollection Fields = new NameValueCollection();
            staticFramework.saveNVC(Fields, fincal_date);
            staticFramework.saveNVC(Fields, msc_code);
            staticFramework.saveNVC(Fields, h_appl_name, "h_");
            staticFramework.saveNVC(Fields, product);
            staticFramework.saveNVC(Fields, h_program, "h_");
            staticFramework.saveNVC(Fields, limit);
            staticFramework.saveNVC(Fields, tenor);
            staticFramework.saveNVC(Fields, rate);
            staticFramework.saveNVC(Fields, monthly_income);
            staticFramework.saveNVC(Fields, yearly_income);
            staticFramework.saveNVC(Fields, join_income);
            staticFramework.saveNVC(Fields, other_installment);
            staticFramework.saveNVC(Fields, aum);
            staticFramework.saveNVC(Fields, mob_aum);
            if (Request.QueryString["mode"]=="new") staticFramework.saveNVC(Fields, "input_by", USERID);
            staticFramework.saveNVC(Fields, "lastchanges_by", USERID);
            staticFramework.save(Fields, Keys, "appfincal", conn);
        }
        #endregion

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
                object value = dsMMUE.Tables[tbl].Rows[0][FieldName];
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
                if (dsMMUE.Tables[tbl].Rows.Count == 0)
                    return "";
                object value = dsMMUE.Tables[tbl].Rows[0][FieldName];
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
            DataTable dt = dsMMUE.Tables[tbl];

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

        #region excel
        private void write_lending_toexcel(string reff, string cat, int startrow, xExcel objExcel)
        {
            object[] par = new object[] { reff, cat };
            DataTable dt = conn.GetDataTable("select * from vw_appfincal_lending where reffnumber=@1 and category = @2 order by seq", par, dbtimeout);
            int r; string seq;
            double installment;
            Microsoft.Office.Interop.Excel.Range objCell;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                r = startrow + i;
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 6];
                objCell.Value2 = dt.Rows[i]["bank_name"].ToString();
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 8];
                objCell.Value2 = dt.Rows[i]["ket_kondisi"].ToString();
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 10];
                objCell.Value2 = dt.Rows[i]["sifat"].ToString();
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 12];
                objCell.Value2 = dt.Rows[i]["latest_status"].ToString();
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 14];
                if (cat == "CC" || cat == "PRK")
                    objCell.Value2 = dt.Rows[i]["plafond"].ToString();
                else
                    objCell.Value2 = dt.Rows[i]["baki_debet"].ToString();
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 17];
                if (cat != "PRK") objCell.Value2 = dt.Rows[i]["tenor"].ToString();
                objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[r, 20];
                if (cat == "CC")
                    objCell.Value2 = dt.Rows[i]["baki_debet"].ToString();
                else
                    objCell.Value2 = dt.Rows[i]["annual_interest"].ToString();

                installment = read_excel_cell_numeric(r, 22, objExcel);
                seq = dt.Rows[i]["seq"].ToString();
                try
                {
                    string sql = "exec usp_appfincal_lending_updinst @1, @2, @3, @4";
                    object[] param = new object[] { reff, cat, seq, installment };
                    conn.ExecNonQuery(sql, param, dbtimeout);
                } catch {};
            }
        }

        private double read_excel_cell_numeric(int row, int col, xExcel objExcel )
        {
            Microsoft.Office.Interop.Excel.Range objCell;
            objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[row, col];
            try { return (double)objCell.Cells.Value2; }
            catch { return 0; }
        }

        private string read_excel_cell_string(int row, int col, xExcel objExcel)
        {
            Microsoft.Office.Interop.Excel.Range objCell;
            objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[row, col];
            try { return (string)objCell.Cells.Value2; }
            catch { return ""; }
        }

        private void write_read_to_template()
        {
            string Q_TEMPLATEPROC = "exec USP_EXPORTEXCEL_LISTPROCEDURE @1 ";
            string Q_TEMPLATEDETAIL = "exec USP_EXPORTEXCEL_LISTDETAIL @1, @2 ";
            string xtpl = "CALC";
            string appid = reffnumber.Text.Trim();
            string resultPath = Server.MapPath("../Templates/Download");
            string resultFilename = appid + ".xls";
            string resultFullpath = resultPath + "/" + resultFilename;
            string tplFile = Server.MapPath("../Templates/Master/LoanCalculator.xlt");
            if (!Directory.Exists(resultPath)) Directory.CreateDirectory(resultPath);
            if (File.Exists(resultFullpath)) File.Delete(resultFullpath);

            xExcel objExcel = new xExcel();
            Microsoft.Office.Interop.Excel.Range objCell;
            objExcel.OpenFile(tplFile, "");

            #region Fill Data
            object[] parproc = new object[1] { xtpl };
            DataTable dt_proc = conn.GetDataTable(Q_TEMPLATEPROC, parproc, dbtimeout);
            for (int k = 0; k < dt_proc.Rows.Count; k++)
            {
                int procseq = (int)dt_proc.Rows[k]["proc_seq"];
                string procname = dt_proc.Rows[k]["proc_name"].ToString();

                object[] pardet = new object[2] { xtpl, procseq };
                DataTable dt_field = conn.GetDataTable(Q_TEMPLATEDETAIL, pardet, dbtimeout);
                DataTable dt_data = conn.GetDataTable("EXEC " + procname + " '" + appid + "'", null, dbtimeout);
                if (dt_data.Rows.Count > 0)
                    for (int i = 0; i < dt_field.Rows.Count; i++)
                    {
                        try
                        {
                            string SheetNm = dt_field.Rows[i]["sheet"].ToString();
                            int col = (int)dt_field.Rows[i]["col"];
                            int row = (int)dt_field.Rows[i]["row"];
                            string Field = dt_field.Rows[i]["field"].ToString();
                            object objValue = dt_data.Rows[0][Field].ToString();
                            string strObject = objValue.ToString();

                            objExcel.excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)objExcel.excelSheets[SheetNm];
                            objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[row, col];
                            objCell.Value2 = objValue;

                        }
                        catch (Exception e2)
                        {
                            ModuleSupport.LogError(this.Page, e2);
                        }
                    }
            }
            #endregion

            #region write lending data
            write_lending_toexcel(reffnumber.Text.Trim(), "CC", 15, objExcel);
            write_lending_toexcel(reffnumber.Text.Trim(), "UL", 48, objExcel);
            write_lending_toexcel(reffnumber.Text.Trim(), "SL", 66, objExcel);
            write_lending_toexcel(reffnumber.Text.Trim(), "MKI", 89, objExcel);
            write_lending_toexcel(reffnumber.Text.Trim(), "INV", 107, objExcel);
            write_lending_toexcel(reffnumber.Text.Trim(), "PRK", 120, objExcel);
            #endregion

            #region read from excel
            double iue, tue, tot_monthly_installment, mue1, mue2, income_multiplier1, income_multiplier2, max_dbr1, max_dbr2, dbr_actual, proposal_limit, installment, max_installment;
            string estimated_cost, final_recommendation, incomeexpense_result;
            NameValueCollection key = new NameValueCollection();
            staticFramework.saveNVC(key, reffnumber);
            NameValueCollection Fields = new NameValueCollection();
            iue = read_excel_cell_numeric(133, 3, objExcel); staticFramework.saveNVC(Fields, "iue", iue);
            tue = read_excel_cell_numeric(133, 14, objExcel); staticFramework.saveNVC(Fields, "tue", tue);
            tot_monthly_installment = read_excel_cell_numeric(133, 20, objExcel); staticFramework.saveNVC(Fields, "tot_monthly_installment", tot_monthly_installment);
            //mue1 = read_excel_cell_numeric(168, 14, objExcel); staticFramework.saveNVC(Fields, "mue1", mue1);
            //mue2 = read_excel_cell_numeric(168, 17, objExcel); staticFramework.saveNVC(Fields, "mue2", mue2);
            //income_multiplier1 = read_excel_cell_numeric(169, 14, objExcel); staticFramework.saveNVC(Fields, "income_multiplier1", income_multiplier1);
            //income_multiplier2 = read_excel_cell_numeric(169, 17, objExcel); staticFramework.saveNVC(Fields, "income_multiplier2", income_multiplier2);
            max_dbr1 = read_excel_cell_numeric(180, 8, objExcel); staticFramework.saveNVC(Fields, "max_dbr1", max_dbr1);
            //max_dbr2 = read_excel_cell_numeric(170, 17, objExcel); staticFramework.saveNVC(Fields, "max_dbr2", max_dbr2);
            dbr_actual = read_excel_cell_numeric(180, 17, objExcel); staticFramework.saveNVC(Fields, "dbr_actual", dbr_actual);
            proposal_limit = read_excel_cell_numeric(176, 22, objExcel); staticFramework.saveNVC(Fields, "proposal_limit", proposal_limit);
            max_installment = read_excel_cell_numeric(177, 22, objExcel); staticFramework.saveNVC(Fields, "max_installment",max_installment);
            installment = read_excel_cell_numeric(177, 17, objExcel); staticFramework.saveNVC(Fields, "installment", installment);
            estimated_cost = read_excel_cell_string(179, 8, objExcel); staticFramework.saveNVC(Fields, "estimated_cost", estimated_cost);
            try
            {
                estimated_cost = read_excel_cell_numeric(179, 8, objExcel).ToString(); staticFramework.saveNVC(Fields, "estimated_cost", estimated_cost);
            } catch {}
            incomeexpense_result = read_excel_cell_string(180, 22, objExcel); staticFramework.saveNVC(Fields, "incomeexpense_result", incomeexpense_result);
            final_recommendation = read_excel_cell_string(183, 8, objExcel); staticFramework.saveNVC(Fields, "final_recommendation", final_recommendation);
            staticFramework.save(Fields, key, "appfincal_summary", conn);

            double highest_limitcc_mob06, highest_limitcc_mob12, lowest_limitcc_mob06, lowest_limitcc_mob12;
            string bic_result_temp;
            Fields.Clear();
            highest_limitcc_mob06 = read_excel_cell_numeric(163, 14, objExcel); staticFramework.saveNVC(Fields, "highest_limitcc_mob06", highest_limitcc_mob06);
            highest_limitcc_mob12 = read_excel_cell_numeric(163, 22, objExcel); staticFramework.saveNVC(Fields, "highest_limitcc_mob12", highest_limitcc_mob12);
            lowest_limitcc_mob06 = read_excel_cell_numeric(164, 14, objExcel); staticFramework.saveNVC(Fields, "lowest_limitcc_mob06", lowest_limitcc_mob06);
            lowest_limitcc_mob12 = read_excel_cell_numeric(164, 22, objExcel); staticFramework.saveNVC(Fields, "lowest_limitcc_mob12", lowest_limitcc_mob12);
            bic_result_temp = read_excel_cell_string(166, 14, objExcel); staticFramework.saveNVC(Fields, "bic_result_temp", bic_result_temp);
            staticFramework.save(Fields, key, "appfincal_bicsummary", conn);

            try
            {
                objExcel.CloseFile(resultFullpath);
                objExcel.stopExcel();
            }
            catch (Exception e3)
            {
                ModuleSupport.LogError(this.Page, e3);
                try
                {
                    objExcel.CloseFile();
                    objExcel.stopExcel();
                }
                catch { }
            }
            #endregion

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_schema();
                initial_reffrential_parameter();
                retrieve_datafincal(Request.QueryString["reffnumber"]);
                gridbindall();
                retrieve_bisummary(reffnumber.Text);
                if (Session["SaveCalculator"].ToString() == "1")
                {
                    btnsave1.Disabled = false; btnsave2.Disabled = false; btnsave3.Disabled = false;
                }
                else
                {
                    btnsave1.Disabled = true; btnsave2.Disabled = true; btnsave3.Disabled = true;
                }
                if (Request.QueryString["mode"] == "edit") reffnumber.ReadOnly = true;
            }
        }

        #region gridcc callback
        protected void GridCC_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridCC, "CC");
        }
        protected void GridCC_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
                    staticFramework.saveNVC(Keys, "category", "CC");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridCC.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridCC_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridCC, "CC");
        }
        //protected void panelCCBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_ccbank);
        //        staticFramework.retrieve(dt, "limit", limit_ccbank);
        //        staticFramework.retrieve(dt, "balance", balance_ccbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_ccbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "limit", limit_ccbank);
        //            staticFramework.saveNVC(Fields, "balance", balance_ccbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "cc_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_ccbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n",conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelCCBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelCCBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridunsecured callback
        protected void GridUnsecured_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridUnsecured, "UL");
        }
        protected void GridUnsecured_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
                    staticFramework.saveNVC(Keys, "category", "UL");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridUnsecured.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridUnsecured_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridUnsecured, "UL");
        }
        //protected void panelPLBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_plbank);
        //        staticFramework.retrieve(dt, "balance", plafond_plbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_plbank);
        //        staticFramework.retrieve(dt, "interest", interest_plbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_plbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", plafond_plbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_plbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_plbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "pl_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_plbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelPLBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelPLBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridsecured callback
        protected void GridSecured_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridSecured, "SL");
        }
        protected void GridSecured_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
                    staticFramework.saveNVC(Keys, "category", "SL");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridSecured.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridSecured_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridSecured, "SL");
        }
        //protected void panelCARBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_carbank);
        //        staticFramework.retrieve(dt, "balance", plafond_carbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_carbank);
        //        staticFramework.retrieve(dt, "interest", interest_carbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_carbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", plafond_carbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_carbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_carbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "car_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_carbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelCARBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelCARBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridmodalkerja callback
        protected void GridMK_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridMK, "MKI");
        }
        protected void GridMK_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
                    staticFramework.saveNVC(Keys, "category", "MKI");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridMK.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridMK_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridMK, "MKI");
        }

        //protected void panelKPRBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_kprbank);
        //        staticFramework.retrieve(dt, "balance", limit_kprbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_kprbank);
        //        staticFramework.retrieve(dt, "interest", interest_kprbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_kprbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", limit_kprbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_kprbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_kprbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "kpr_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_kprbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelKPRBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelKPRBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridinvest callback
        protected void GridInvest_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridInvest, "INV");
        }
        protected void GridInvest_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
                    staticFramework.saveNVC(Keys, "category", "INV");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridInvest.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridInvest_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridInvest, "INV");
        }
        //protected void panelCCOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_ccothbank);
        //        staticFramework.retrieve(dt, "limit", limit_ccothbank);
        //        staticFramework.retrieve(dt, "balance", balance_ccotherbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_ccothbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "limit", limit_ccothbank);
        //            staticFramework.saveNVC(Fields, "balance", balance_ccotherbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "cc_oth_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_ccothbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelCCOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelCCOtherBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region gridprk callback
        protected void GridRK_Load(object sender, EventArgs e)
        {
            gridbind_lending(GridRK, "PRK");
        }
        protected void GridRK_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("d:"))
            {
                string k = e.Parameters.Substring(2);
                if (k != "")
                {
                    NameValueCollection Keys = new NameValueCollection();
                    staticFramework.saveNVC(Keys, "reffnumber", reffnumber.Text);
                    staticFramework.saveNVC(Keys, "category", "PRK");
                    staticFramework.saveNVC(Keys, "seq", k);
                    staticFramework.Delete(Keys, "appfincal_lending", conn);
                    GridRK.JSProperties["cp_alert"] = "Please click save to update calculation";
                }
            }
        }
        protected void GridRK_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            gridbind_lending(GridRK, "PRK");
        }
        //protected void panelPLOtherBank_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.StartsWith("r:"))
        //    {
        //        string key = e.Parameter.Substring(2);
        //        object[] par = new object[] { Request.QueryString["regno"], key };
        //        DataTable dt = conn.GetDataTable("select * from idi_kredit_calc where appid = @1 and seq = @2", par, dbtimeout);
        //        staticFramework.retrieve(dt, "seq", seq_plothbank);
        //        staticFramework.retrieve(dt, "balance", plafond_plothbank);
        //        staticFramework.retrieve(dt, "tenor", tenor_plothbank);
        //        staticFramework.retrieve(dt, "interest", interest_plothbank);
        //    }
        //    else if (e.Parameter.StartsWith("s:"))
        //    {
        //        try
        //        {
        //            NameValueCollection Keys = new NameValueCollection();
        //            staticFramework.saveNVC(Keys, "appid", Request.QueryString["regno"]);
        //            staticFramework.saveNVC(Keys, "seq", seq_plothbank);
        //            NameValueCollection Fields = new NameValueCollection();
        //            staticFramework.saveNVC(Fields, "balance", plafond_plothbank);
        //            staticFramework.saveNVC(Fields, "interest", interest_plothbank);
        //            staticFramework.saveNVC(Fields, "tenor", tenor_plothbank);
        //            staticFramework.saveNVC(Fields, "calc_category", "pl_oth_bank");
        //            staticFramework.saveNVC(Fields, "inputby", USERID);
        //            Fields["inputdate"] = "GETDATE()";
        //            if (seq_plothbank.Value == "")
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc",
        //                    "DECLARE @seq INT \n" +
        //                    "SELECT @seq=ISNULL(MAX(seq),0)+1 FROM idi_kredit_calc " +
        //                    "WHERE appid='" + Request.QueryString["regno"] + "' \n", conn);
        //            }
        //            else
        //            {
        //                staticFramework.save(Fields, Keys, "idi_kredit_calc", conn);
        //            }
        //            panelPLOtherBank.JSProperties["cp_alert"] = "please recalculate to update the result";
        //        }
        //        catch (Exception ex)
        //        {
        //            string errmsg = ex.Message;
        //            if (errmsg.IndexOf("Last Query") > 0)
        //                errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
        //            panelPLOtherBank.JSProperties["cp_alert"] = errmsg;
        //        }
        //    }
        //}
        #endregion

        #region panel callback
        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter == "save")
            {
                try
                {
                    save_data();
                    load_lendingdata(reffnumber.Text);
                    write_read_to_template();
                    gridbindall();
                    retrieve_bisummary(reffnumber.Text);
                    retrieve_datafincal(reffnumber.Text);
                    mainPanel.JSProperties["cp_alert"] = "Data telah tersimpan.";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
            else if (e.Parameter == "load")
            {
                if (reffnumber.Text.Trim() != "" && Request.QueryString["mode"] == "new")
                {
                    string qgetdata = "select top 1 cust_name from slik_applicant where reffnumber = @1";
                    conn.ExecReader(qgetdata, new object[] { reffnumber.Text }, dbtimeout);
                    bool isValid = false; string cust_name = "";
                    if (conn.hasRow()) { isValid = true; cust_name = conn.GetFieldValue(0).ToString(); }
                    if (isValid)
                    {
                        conn.ExecReader("select 1 from appfincal where reffnumber=@1", new object[] { reffnumber.Text }, dbtimeout);
                        if (conn.hasRow())
                        {
                            retrieve_datafincal(reffnumber.Text);
                            load_lendingdata(reffnumber.Text);
                            gridbindall();
                            retrieve_bisummary(reffnumber.Text);
                        }
                        else
                        {
                            appl_name.Text = cust_name;
                            h_appl_name.Value = cust_name;
                        }
                    }
                    else { mainPanel.JSProperties["cp_alert"] = "No Aplikasi tidak ditemukan"; }
                }
            }
        }

        protected void panelProgram_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            object[] par = new object[] { h_product.Value };
            staticFramework.reff(program, "select * from rfprogram_calc where productid = @1", par, conn);
        }

        #endregion

    }
}

