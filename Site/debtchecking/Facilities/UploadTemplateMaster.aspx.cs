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
using DMS.Framework;

namespace DebtChecking.Facilities
{
    public partial class UploadTemplateMaster : System.Web.UI.Page
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
            //string fullpath = Session["UploadRestoreData"].ToString();

           // excelToDb(fullpath);

            //try { File.Delete(fullpath); }
            //catch { }
        }

        protected void ImportFile_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            HttpPostedFile userPostedFile = ImportFile.PostedFile;
            if (userPostedFile != null && userPostedFile.ContentLength > 0)
            {

                if (Path.GetExtension(userPostedFile.FileName) == ".xlt")
                {
                    string url = Request.Url.Scheme + "://" + Request.Url.Host + ResolveUrl("~/") + "/templates/master/loancalculator.xlt";
                    string oldFile = Server.MapPath("~/templates/master/loancalculator.xlt");
                    string newFile = Server.MapPath("~/templates/master/") + Path.GetFileNameWithoutExtension(oldFile) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlt";
                    File.Move(oldFile, newFile);
                    userPostedFile.SaveAs(oldFile);
                    Response.Write("<script language=javascript>alert('Upload file Loan Calculator Template Success');</script>");
                    //TXT_PROGRESS.Text = "Upload file Loan Calculator Template Success";
                    btnSave.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    Response.Write("<script language=javascript>alert('File Content Type is Invalid');</script>");
                
                }
            }
        }

        #region upload data


        #endregion
    }
}