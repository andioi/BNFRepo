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

using System.Diagnostics;
using System.IO;
using DMS.Tools;
using DMS.Framework;

namespace DebtChecking.CommonForm
{
    public partial class UC_GenExcel : System.Web.UI.UserControl
    {
        private string _svrpathurl = "../Upload/GenFiles", _svrtplurl = "../Template", _tplgrp, _title;
        private static string Q_TPL = "select EXCELTPL, TPLDESC from RFEXCELTPLGROUP where TPLGROUP = @1 ";
        private static string Q_TEMPLATEPROC = "exec USP_EXPORTEXCEL_LISTPROCEDURE @1 ";
        private static string Q_TEMPLATEDETAIL = "exec USP_EXPORTEXCEL_LISTDETAIL @1, @2 ";
        private static string SP_MON = "exec USP_APPGENFILEMONITOR @1, @2, @3, @4, @5 ";

        public DbConnection conn;
        public int dbtimeout;
        public string TplGroup
        {
            set { _tplgrp = value; }
        }
        public string Title
        {
            set { _title = value; }
        }
        private string SvrPathUrl
        {
            get 
            {
                string datedir = Request.QueryString["regno"].Substring(1, 6);     //dont use system date! its a running time.. 
                return _svrpathurl + "/" + datedir + "/" + Request.QueryString["regno"] + "/" + _tplgrp + "/";
            }
        }
        private string SvrPathPhysic
        {
            get { return Server.MapPath(SvrPathUrl); }
        }
        private string SvrTplUrl
        {
            get { return _svrtplurl + "/"; }
        }
        private string SvrTplPhysic
        {
            get { return Server.MapPath(SvrTplUrl); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_tplgrp == null)
                tbm.Visible = false;
            if (_title != null && _title.Trim() != "")
                lbT.Text = _title;
            if (!IsPostBack)
            {
                object[] par = new object[] { _tplgrp };
                staticFramework.reff(ddltpl, Q_TPL, par, conn, false);
                bindfiles();
            }
            ddltpl.Attributes["onchange"] = "javascript:document.form1." + h_tpl.ClientID + ".value=document.form1." + ddltpl.ClientID + ".value;";
        }

        #region binding files
        private FileInfo[] listfiles()
        {
            FileInfo[] ret = new FileInfo[] { };
            DirectoryInfo dir = new DirectoryInfo(SvrPathPhysic);
            if (!dir.Exists)
            {
                if (Request.QueryString["readonly"] == null)
                    dir.Create();
            }
            else
            {
                ret = dir.GetFiles("*.xls", SearchOption.AllDirectories);
            }
            return ret;
        }

        private void bindfiles()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SEQ");
            dt.Columns.Add("FILENAME");
            dt.Columns.Add("DATE");
            FileInfo[] fi = listfiles();
            for (int i = 0; i < fi.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["SEQ"] = i + 1;
                dr["FILENAME"] = fi[i].Name;
                dr["DATE"] = fi[i].LastWriteTime;
                dt.Rows.Add(dr);
            }
            gridfile.DataSource = dt;
            gridfile.DataBind();
        }

        protected void gridfile_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    DataRowView dv = (DataRowView)e.Row.DataItem;
                    string fn = dv["FILENAME"].ToString();
                    //int pos1 = fn.IndexOf("-");
                    //int pos2 = fn.IndexOf("-", pos1 + 1);
                    //if (pos1 > 0 && pos2 > pos1)
                    //    fuser = fn.Substring(pos1 + 1, pos2 - pos1 - 1);

                    HyperLink lnkdownload = (HyperLink)e.Row.FindControl("LNK_DOWN");
                    lnkdownload.NavigateUrl = SvrPathUrl + fn;
                    lnkdownload.Target = "_Blank";

                    HyperLink lnkdel = (HyperLink)e.Row.FindControl("LNK_DEL");
                    //lnkdel.NavigateUrl = "javascript:callback(panelFile, 'd:" + fn + "', false)";
                    lnkdel.Visible = false;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region generate method
        private string CreateExcel(string xtpl, string regno, string var_user)
        {
            string mStatus = "Template parameter not set!";
            bool bSukses = true;

            /// Mengambil application root
            string path_in = SvrTplPhysic;
            string path_out = SvrPathPhysic;
            string file_xlt = xtpl + ".XLT";
            string fileNm = xtpl + "-" + regno + ".XLS";

            /// Cek apakah file templatenya (input) ada atau tidak
            if (!File.Exists(path_in + file_xlt))
                throw new Exception("File Template tidak ada!");
            /// Cek direktori untuk menyimpan file hasil export (output)
            if (!Directory.Exists(path_out))
                Directory.CreateDirectory(path_out);
            /// Cek file untuk menyimpan file hasil export (output)
            if (File.Exists(path_out + fileNm))
                File.Delete(path_out + fileNm);

            // rony's excel object variables 
            xExcel objExcel = new xExcel();
            objExcel.OpenFile(path_in + file_xlt, "");

            #region Fill Data
            object[] parproc = new object[1] { xtpl };
            DataTable dt_proc = conn.GetDataTable(Q_TEMPLATEPROC, parproc, dbtimeout);
            for (int k = 0; k < dt_proc.Rows.Count; k++)
            {
                int procseq = (int)dt_proc.Rows[k]["PROC_SEQ"];
                string procname = dt_proc.Rows[k]["PROC_NAME"].ToString();

                object[] pardet = new object[2] { xtpl, procseq };
                DataTable dt_field = conn.GetDataTable(Q_TEMPLATEDETAIL, pardet, dbtimeout);
                DataTable dt_data = conn.GetDataTable("EXEC " + procname + " '" + regno + "'", null, dbtimeout);
                if (dt_data.Rows.Count > 0)
                    for (int i = 0; i < dt_field.Rows.Count; i++)
                    {
                        try
                        {
                            string SheetNm = dt_field.Rows[i]["SHEET"].ToString();
                            int col = (int)dt_field.Rows[i]["COL"];
                            int row = (int)dt_field.Rows[i]["ROW"];
                            string Field = dt_field.Rows[i]["FIELD"].ToString();
                            object objValue = dt_data.Rows[0][Field].ToString();
                            string strObject = objValue.ToString();

                            objExcel.excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)objExcel.excelSheets[0];
                            Microsoft.Office.Interop.Excel.Range objCell = (Microsoft.Office.Interop.Excel.Range)objExcel.excelWorksheet.Cells[row, col];
                            objCell.Value2 = objValue;
                        }
                        catch (Exception e2)
                        {
                            mStatus = "Terjadi error saat mengisi data";
                            ModuleSupport.LogError(this.Page, e2);
                        }
                    }
            }
            #endregion

            try
            {
                objExcel.CloseFile(path_out + fileNm);
                objExcel.stopExcel();
                mStatus = "Pembuatan File Berhasil";
            }
            catch (Exception e3)
            {
                ModuleSupport.LogError(this.Page, e3);
                mStatus = "Pembuatan File Gagal";
                try
                {
                    objExcel.CloseFile();
                    objExcel.stopExcel();
                }
                catch { }
            }

            return mStatus;
        }
        #endregion

        #region callback
        protected void panelFile_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter == "generate")
                {
                    string status = CreateExcel(h_tpl.Value, Request.QueryString["regno"], (string)Session["UserID"]);

                    if (status == "Pembuatan File Berhasil")
                    {
                        object[] monpar = new object[5]
                        {
                            Request.QueryString["regno"], _tplgrp, h_tpl.Value, (string)Session["UserID"], true
                        };
                        conn.ExecuteNonQuery(SP_MON, monpar, dbtimeout);
                    }
                    panelFile.JSProperties["cp_alert"] = status;
                }
                else if (e.Parameter.StartsWith("d:"))
                {
                    string filename = e.Parameter.Substring(2);
                    string fullpath = SvrPathPhysic + filename;
                    FileInfo fi = new FileInfo(fullpath);
                    fi.Delete();
                }
                bindfiles();
            }
            catch (Exception ex)
            {
                string errmsg = "";
                if (ex.Message.IndexOf("Last Query:") > 0)
                    errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                else
                    errmsg = ex.Message;
                panelFile.JSProperties["cp_alert"] = errmsg;
            }
        }
        #endregion
    }
}