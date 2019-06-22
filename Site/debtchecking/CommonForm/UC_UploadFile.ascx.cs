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
using Ionic.Zip;
using DMS.Tools;
using System.Collections.Generic;

namespace DebtChecking.CommonForm
{
    public partial class UC_UploadFile : System.Web.UI.UserControl
    {
        private string _svrpathurl = "../Upload", _cab;
        private int _maxfiles = 5;
        DbConnection conn;

        public string Cabinet
        {
            set { _cab = value; }
        }
        public string Title
        {
            set { ttl.Text = value; }
        }
        public int MaxFiles
        {
            set { _maxfiles = value; }
        }
        private string SvrPathUrl
        {
            get { return _svrpathurl + "/"; }
        }
        private string SvrPathPhysic
        {
            //get { return Server.MapPath(SvrPathUrl); }
            get { return ConfigurationSettings.AppSettings["filepath"]; }
        }
        private string UserID
        {
            get { return (string)Session["UserID"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_cab == null)
            {
                tbm.Visible = false;
            }

            if ((Session["fn"] != null) && ((string)Session["fnuc"] == this.ClientID))
            {
                string src = (string)Session["fn"];
                Session["fn"] = null;
                Session["fnuc"] = null;
            }
            upfile.ClientSideEvents.FileUploadComplete = "function(s, e) { processing=false; }";
            btnup.Attributes["onclick"] = "if (" + upfile.ClientID + ".GetText() != '') {  if (!processing) {processing=true; " + upfile.ClientID + ".UploadFile();};  }"; 
        }

        #region callback
        protected void upfile_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            HttpPostedFile userPostedFile = upfile.PostedFile;
            string msg;

            if (userPostedFile != null && userPostedFile.ContentLength > 0)
            {
                try
                {
                    string filename = Path.GetFileName(userPostedFile.FileName);
                    string fullpath = SvrPathPhysic + "Temp\\" + filename;
                    if (!Directory.Exists(SvrPathPhysic + "Temp\\"))
                        Directory.CreateDirectory(SvrPathPhysic + "Temp\\");
                    userPostedFile.SaveAs(fullpath);
                    Session["fn"] = fullpath;               //this callback happen before any other vars is initialized, 
                    Session["fnuc"] = this.ClientID;        //thus, put the currently uploaded filename to the session and move it at the page_load
                    string extension = Path.GetExtension(fullpath);
                    string filenamewext = Path.GetFileNameWithoutExtension(fullpath);
                    string filepath = Path.GetDirectoryName(fullpath);
                    string unpackDirectory = filepath + "\\" + Session["UserID"];
                    
                    if (extension == ".zip")
                    {
                        try
                        {
                            if (Directory.Exists(unpackDirectory)) { Directory.Delete(unpackDirectory, true); }
                            using (ZipFile zip1 = ZipFile.Read(fullpath))
                            {
                                foreach (ZipEntry ze in zip1)
                                {
                                    ze.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                                }
                            }
                        }
                        catch (System.Exception ex1)
                        {
                            lblEr.ForeColor = System.Drawing.Color.Red;
                            msg = "exception: " + ex1;
                        }
                        int cntupl = 0;
                        List<string> dirs = FileHelper.GetFilesRecursive(unpackDirectory);
                        conn = new DbConnection((string)Session["ConnString"]);
                        foreach (string p in dirs)
                        {
                            if (copyfiletoverfiles(p)) cntupl++;
                        }
                        lblEr.ForeColor = System.Drawing.Color.DarkGreen;
                        msg = cntupl.ToString() + " Files Uploaded";
                        if (File.Exists(fullpath)) File.Delete(fullpath);
                    }
                    else
                    {
                        if (copyfiletoverfiles(fullpath))
                        {
                            lblEr.ForeColor = System.Drawing.Color.DarkGreen;
                            msg = "1 File Uploaded";
                            if (File.Exists(fullpath)) File.Delete(fullpath);
                        }
                        else
                        {
                            lblEr.ForeColor = System.Drawing.Color.Red;
                            msg = "Nama file tidak dikenali";
                        }
                    }
                }                                           
                catch (Exception ex)
                {
                    string errmsg = "";
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        errmsg = ex.Message;
                    lblEr.ForeColor = System.Drawing.Color.Red;
                    msg = errmsg;
                }
                //upfile.ClientSideEvents.FileUploadComplete = "function(s, e) { processing=false; if (e.isValid) {alert('" + msg + "');} }";
                e.ErrorText = msg;
                e.IsValid = false;
            }
        }

        protected bool copyfiletoverfiles(string fp)
        {
            bool ret = false;
            string fn = Path.GetFileNameWithoutExtension(fp);
            string ffn = Path.GetFileName(fp);
            string header = fn.Substring(0, 1);
            if ((header == "H" || header == "O" || header == "P") && fn.Contains("_"))
            {
                string appid = fn.Substring(1, fn.IndexOf("_") - 1);
                string footer = fn.Substring(fn.IndexOf("_") + 1);
                string df, fd;
                switch (header)
                {
                    case "H":
                        df = SvrPathPhysic + "VerFiles\\" + appid + "\\HOME\\";
                        if (!Directory.Exists(df)) Directory.CreateDirectory(df);
                        fd = df + ffn;
                        File.Copy(fp, fd, true);
                        ret = true;
                        break;
                    case "O":
                        df = SvrPathPhysic + "VerFiles\\" + appid + "\\OFFICE\\";
                        if (!Directory.Exists(df)) Directory.CreateDirectory(df);
                        fd = df + ffn;
                        File.Copy(fp, fd, true);
                        ret = true;
                        break;
                    case "P":
                        df = SvrPathPhysic + "VerFiles\\" + appid + "\\PDF\\";
                        if (!Directory.Exists(df)) Directory.CreateDirectory(df);
                        fd = df + ffn;
                        File.Copy(fp, fd, true);
                        int dbtimeout = (int)Session["DbTimeOut"];
                        object[] par = new object[] { appid };
                        if (footer == "PV") conn.ExecNonQuery("update applicant set uploadpdf_pv = 1 where reffnumber = @1", par, dbtimeout);
                        if (footer == "FS") conn.ExecNonQuery("update applicant set uploadpdf_fs = 1 where reffnumber = @1", par, dbtimeout);
                        ret = true;
                        break;
                }

            }
            return ret;
        }
        #endregion

        static class FileHelper
        {
            public static List<string> GetFilesRecursive(string b)
            {
                // 1.
                // Store results in the file results list.
                List<string> result = new List<string>();

                // 2.
                // Store a stack of our directories.
                Stack<string> stack = new Stack<string>();

                // 3.
                // Add initial directory.
                stack.Push(b);

                // 4.
                // Continue while there are directories to process
                while (stack.Count > 0)
                {
                    // A.
                    // Get top directory
                    string dir = stack.Pop();

                    try
                    {
                        // B
                        // Add all files at this directory to the result List.
                        result.AddRange(Directory.GetFiles(dir, "*.*"));

                        // C
                        // Add all directories at this directory.
                        foreach (string dn in Directory.GetDirectories(dir))
                        {
                            stack.Push(dn);
                        }
                    }
                    catch
                    {
                        // D
                        // Could not open the directory
                    }
                }
                return result;
            }
        }

    }
}