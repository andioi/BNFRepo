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

namespace DebtChecking.CommonForm
{
    public partial class UC_UploadedFile : System.Web.UI.UserControl
    {
        private string _svrpathurl = "../Upload/VerFiles", _cab;
        private int _maxfiles = 5;
        protected DbConnection conn;
        protected int dbtimeout;

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
            get 
            {
                string regno = Request.QueryString["regno"];
                string appid = "";
                dbtimeout = (int)Session["DbTimeOut"];
                conn = new DbConnection((string)Session["ConnString"]);
                conn.ExecReader("select reffnumber from slik_applicant where appid = @1", new object[] { regno }, dbtimeout);
                if (conn.hasRow()) appid = conn.GetFieldValue("reffnumber").ToString();
                return _svrpathurl + "/" + appid + "/" + _cab + "/";
            }
        }
        private string SvrPathPhysic
        {
            get { return Server.MapPath(SvrPathUrl); }
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
            if (!IsPostBack)
            {
                bindfiles();
            }

            if ((Session["fn"] != null) && ((string)Session["fnuc"] == this.ClientID))
            {
                string src = (string)Session["fn"];
                Session.Remove("fn");
                Session.Remove("fnuc");

                string dest = SvrPathPhysic + UserID + "\\" + Path.GetFileName(src);        //the correct cab
                if (src != dest)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(dest)))
                        Directory.CreateDirectory(Path.GetDirectoryName(dest));
                    int filecount = Directory.GetFiles(Path.GetDirectoryName(dest)).Length;

                    if (filecount > _maxfiles)
                    {
                        Session["errmsg"] = "Maximum file reached!";
                        File.Delete(src);
                    }
                    else
                    {
                        if (File.Exists(dest))
                            File.Delete(dest);
                        File.Move(src, dest);
                    }
                }
            }
            upfile.ClientSideEvents.FileUploadComplete = "function(s, e) { processing=false; if (e.isValid) {callback(" + panelFile.ClientID + ", 'refresh', false);} }";
            btnup.Attributes["onclick"] = "if (" + upfile.ClientID + ".GetText() != '') {  if (!processing) {processing=true; " + upfile.ClientID + ".UploadFile();};  }"; 
        }

        #region binding
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
                ret = dir.GetFiles("*.*", SearchOption.AllDirectories);
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
                dr["FILENAME"] = fi[i].FullName.Substring(SvrPathPhysic.Length);
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

                    //if (fuser == (string)Session["UserID"])
                    if (fn.StartsWith(UserID + "\\"))
                    {
                        HyperLink lnkdel = (HyperLink)e.Row.FindControl("LNK_DEL");
                        lnkdel.NavigateUrl = "javascript:callback(" + panelFile.ClientID + ", 'd:" + fn.Replace("\\", "/") + "', false)";
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region callback
        protected void panelFile_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.StartsWith("d:"))
            {
                try
                {
                        string filename = e.Parameter.Substring(2);
                        string fullpath = Server.MapPath(SvrPathUrl + filename);
                        FileInfo fi = new FileInfo(fullpath);
                        fi.Delete();
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
            else if (Session["errmsg"] != null)
            {
                panelFile.JSProperties["cp_alert"] = (string)Session["errmsg"];
                Session.Remove("errmsg");
            }
            bindfiles();
        }

        protected void upfile_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            HttpPostedFile userPostedFile = upfile.PostedFile;

            if (userPostedFile != null && userPostedFile.ContentLength > 0)
            {
                try
                {
                    string filename = Path.GetFileName(userPostedFile.FileName);
                    string fullpath = SvrPathPhysic + filename;
                    if (!Directory.Exists(SvrPathPhysic))
                        Directory.CreateDirectory(SvrPathPhysic);
                    userPostedFile.SaveAs(fullpath);
                    Session["fn"] = fullpath;               //this callback happen before any other vars is initialized, 
                                                            //hence, no correct cab for this file yet.. 
                    Session["fnuc"] = this.ClientID;        //thus, put the currently uploaded filename to the session and move it at the page_load
                }                                           
                catch (Exception ex)
                {
                    string errmsg = "";
                    if (ex.Message.IndexOf("Last Query:") > 0)
                        errmsg = ex.Message.Substring(0, ex.Message.IndexOf("Last Query:"));
                    else
                        errmsg = ex.Message;
                    upfile.JSProperties["cp_alert"] = errmsg;
                }
            }
        }
        #endregion
    }
}