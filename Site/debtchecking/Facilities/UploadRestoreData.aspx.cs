using System;
using System.IO;
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
using DevExpress.Web;
using DMS.Tools;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using DMS.Framework;
using Excel = Microsoft.Office.Interop.Excel;

namespace DebtChecking.Facilities
{
    public partial class UploadRestoreData : DataPage
    {


        #region initial_reffrential_parameter
        protected void initial_reffrential_parameter()
        {
            
        }
        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }

        }

        protected void PanelFile_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string fullpath = Session["UploadRestoreData"].ToString();
            
            excelToDb(fullpath);
            
            try { File.Delete(fullpath); }
            catch { }
        }

        protected void ImportFile_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            HttpPostedFile userPostedFile = ImportFile.PostedFile;
            if (userPostedFile != null && userPostedFile.ContentLength > 0)
            {
                string ext = System.IO.Path.GetExtension(userPostedFile.FileName),
                    fn = Session["UserID"].ToString() + "_" + System.IO.Path.GetFileNameWithoutExtension(userPostedFile.FileName),
                    pathurl = "~/Upload/RestoreData";
                string fullpath = Server.MapPath(pathurl) + "\\" + fn + ext;
                if (ext.ToLower() == ".xls")
                {
                    if (System.IO.File.Exists(fullpath)) System.IO.File.Delete(fullpath);
                    userPostedFile.SaveAs(fullpath);
                    Session["UploadRestoreData"] = fullpath;
                }
            }
        }

        #region upload data

        public void excelToDb(string excelfile)
        {

            #region initialization
            string appid, cust_name;            
            int success_counter = 0, failed_counter = 0;
            conn.ExecNonQuery("exec USP_RESTORE_DATA_CLEARTEMP @1", new object[] { USERID }, dbtimeout);
            #endregion

            try
            {

                Excel._Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(excelfile);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Item[1];

                for (int k = 2; xlWorkSheet.Range["A"+@k, "A"+@k].Value2 != null; k++)
                {

                    #region get excel value
                    try { appid = xlWorkSheet.Range["A" + @k, "A" + @k].Value2.ToString().Trim(); } catch {appid = "";}
                    try { cust_name = xlWorkSheet.Range["B" + @k, "B" + @k].Value2.ToString().Trim(); } catch { cust_name = ""; }
                    #endregion
                    if (appid != "")
                    {
                        try
                        {

                            #region insert into database
                            object[] paramDB = new object[] { appid, cust_name, USERID };
                            conn.ExecuteNonQuery("EXEC USP_RESTORE_DATA_INSERTTEMP @1, @2, @3", paramDB, dbtimeout);
                            success_counter++;
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            #region exception handling
                            TXT_RESULT.Text += "appid : " + appid + " failed. " + ex.Message + Environment.NewLine;
                            return;
                            #endregion
                        }
                    }
                }

                TXT_RESULT.Text = "Process Finish! success : " + success_counter.ToString() + ", failed : " + failed_counter.ToString();
                xlWorkBook.Close();
                xlApp.Quit();
                PanelFile.JSProperties["cp_redirect"] = "../CommonForm/ListRestoreData.aspx?mntitle=Upload%20Restore%20Data&li=L|URST";
            }
            catch (Exception ex)
            {
                TXT_RESULT.Text = ex.Message;
                TXT_RESULT.ForeColor = System.Drawing.Color.Red;
            }
        }

        #endregion

    }
}
