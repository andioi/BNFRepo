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
    public partial class UC_GenWord : System.Web.UI.UserControl
    {
        private string _svrpathurl = "../Upload/GenFiles", _svrtplurl = "../Template", _tplgrp, _title;
        private bool _cekmand = false;
        public DbConnection conn;
        public int dbtimeout;
        private static string Q_TPL = "select WORDTPL, TPLDESC from RFWORDTPLGROUP where TPLGROUP = @1 ";
        private static string Q_TEMPLATEPROC = "exec USP_EXPORTWORD_LISTPROCEDURE @1 ";
        private static string Q_TEMPLATEDETAIL = "exec USP_EXPORTWORD_LISTDETAIL @1, @2 ";
        private static string SP_MON = "exec USP_APPGENFILEMONITOR @1, @2, @3, @4, @5 ";

        public string TplGroup
        {
            set { _tplgrp = value; }
        }
        public string Title
        {
            set { _title = value; }
        }
        public bool CekMand
        {
            set { _cekmand = value; }
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
            if (_cekmand)
            {
                btngen.Attributes.Remove("onclick");
                btngen.Attributes.Add("onclick", "callback(panelFile,'generate');");
            }
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
                ret = dir.GetFiles("*.doc", SearchOption.AllDirectories);
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
        private string CreateWord(string wtpl, string regno, string var_user)
        {
            string mStatus = "Template parameter not set!";
            bool bSukses = true;
            int postrace = 0;

            /// Mengambil application root
            string path_in = SvrTplPhysic;
            string path_out = SvrPathPhysic;
            string file_dot = wtpl + ".DOT";
            string fileNm = wtpl + "-" + regno + ".DOC";

            /// Cek apakah file templatenya (input) ada atau tidak
            if (!File.Exists(path_in + file_dot))
                throw new Exception("File Template tidak ada!");
            /// Cek direktori untuk menyimpan file hasil export (output)
            if (!Directory.Exists(path_out))
                Directory.CreateDirectory(path_out);
            /// Cek file untuk menyimpan file hasil export (output)
            if (File.Exists(path_out + fileNm))
                File.Delete(path_out + fileNm);

            // Word object variables 
            object objFileIn = path_in + file_dot;
            object objFileOut = path_out + fileNm;
            object oMissing = System.Reflection.Missing.Value;
            object oTrue = true;
            object oFalse = false;

            Microsoft.Office.Interop.Word.Application wordApp = null;
            Microsoft.Office.Interop.Word.Document wordDoc = null;

            //Array of running winword process id in Taskbar
            ArrayList orgId = new ArrayList();
            ArrayList newId = new ArrayList();

            //Collecting Existing Winword in Taskbar
            Process[] oldProcess = Process.GetProcessesByName("WINWORD");
            foreach (Process thisProcess in oldProcess)
                orgId.Add(thisProcess);

            try
            {
                postrace = 11;
                wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = false;

                /// Collecting Existing Winword in Taskbar 
                postrace = 12;
                Process[] newProcess = Process.GetProcessesByName("WINWORD");
                foreach (Process thisProcess in newProcess)
                    newId.Add(thisProcess);
                //SupportTools.saveProcessWord(wordApp, newId, orgId, conn);

                //wordDoc = wordApp.Documents.Open(ref objFileIn, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, 
                //	ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                postrace = 13;
                wordDoc = wordApp.Documents.Add(ref objFileIn, ref oMissing, ref oMissing, ref oMissing);
                postrace = 14;
                wordDoc.Activate();
            }
            catch (Exception e1)
            {
                string myedata = "CreateWord(" + wtpl + ", " + regno + ", " + var_user + ") at position: " + postrace.ToString() +
                    "    objFileIn set to: " + objFileIn.ToString();
                ModuleSupport.LogError(myedata, this.Page, e1);
                try
                {
                    if (wordDoc != null)
                    {
                        wordDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                        wordDoc = null;
                    }
                    if (wordApp != null)
                    {
                        wordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                        wordApp = null;
                        // Killing Proses after Export
                        for (int x = 0; x < newId.Count; x++)
                        {
                            Process xnewId = (Process)newId[x];
                            bool bSameId = false;
                            for (int z = 0; z < orgId.Count; z++)
                            {
                                Process xoldId = (Process)orgId[z];

                                if (xnewId.Id == xoldId.Id)
                                {
                                    bSameId = true;
                                    break;
                                }
                            }
                            if (!bSameId)
                            {
                                xnewId.Kill();
                                break;
                            }
                        }
                    }
                }
                catch (Exception e2)
                {
                    ModuleSupport.LogError(this.Page, e2);
                }
                throw e1;
            }

            Microsoft.Office.Interop.Word.Bookmarks wordBookMark = wordDoc.Bookmarks;
            Microsoft.Office.Interop.Word.Bookmark oBook;

            #region Fill Data
            int iItem = 0;
            object[] parproc = new object[1] { wtpl };
            DataTable dt_proc = conn.GetDataTable(Q_TEMPLATEPROC, parproc, dbtimeout);
            for (int k = 0; k < dt_proc.Rows.Count; k++)
            {
                int procseq = (int)dt_proc.Rows[k]["PROC_SEQ"];
                string procname = dt_proc.Rows[k]["PROC_NAME"].ToString();

                object[] pardet = new object[2] { wtpl, procseq };
                DataTable dt_field = conn.GetDataTable(Q_TEMPLATEDETAIL, pardet, dbtimeout);
                DataTable dt_data = conn.GetDataTable("EXEC " + procname + " '" + regno + "'", null, dbtimeout);

                for (int j = 0; j < dt_data.Rows.Count; j++)
                {
                    for (int i = 0; i < dt_field.Rows.Count; i++) 
                    {
                        try
                        {
                            postrace = 21;
                            object Bookmark = dt_field.Rows[i]["BOOKMARK"];
                            string Field = dt_field.Rows[i]["FIELD"].ToString();
                            object objValue = dt_data.Rows[j][Field].ToString();
                            string strObject = objValue.ToString();
                            if (dt_field.Rows[i]["WORDDET_TYPE"].ToString().ToUpper() == "TABLE") strObject = strObject + "\n";
                            if (wordBookMark.Exists(Bookmark.ToString()))
                            {
                                postrace = 22;
                                //oBook = wordBookMark.Item(ref Bookmark);
                                oBook = wordBookMark.get_Item(ref Bookmark);
                                oBook.Select();
                                postrace = 23;
                                oBook.Range.Text = strObject;
                            }

                            iItem++;
                        }
                        catch (Exception e2)
                        {
                            string myedata = "CreateWord at filling data seq (k:" + k.ToString() + ", j:" + j.ToString() + "i:" + i.ToString() + ") at position: " + postrace.ToString();
                            ModuleSupport.LogError(myedata, this.Page, e2);
                        }
                    }
                }
            }
            #endregion

            /// simpan hasil export
            if (iItem > 0)
            {
                //wordDoc.SaveAs(ref objFileOut, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                //    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                wordDoc.SaveAs(ref objFileOut, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
                bSukses = true;
                mStatus = "Pembuatan File Berhasil";
            }
            else
            {
                bSukses = false;
                mStatus = "Tidak ada data utk membuat File";
            }

            try
            {
                if (wordDoc != null)
                {
                    wordDoc.Close(ref oFalse, ref oMissing, ref oMissing);
                    wordDoc = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                    wordApp = null;

                    // Killing Proses after Export
                    for (int x = 0; x < newId.Count; x++)
                    {
                        Process xnewId = (Process)newId[x];

                        bool bSameId = false;
                        for (int z = 0; z < orgId.Count; z++)
                        {
                            Process xoldId = (Process)orgId[z];

                            if (xnewId.Id == xoldId.Id)
                            {
                                bSameId = true;
                                break;
                            }
                        }
                        if (!bSameId)
                        {
                            xnewId.Kill();
                            break;
                        }
                    }
                }
            }
            catch (Exception e3)
            {
                ModuleSupport.LogError(this.Page, e3);
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
                    string status = CreateWord(h_tpl.Value, Request.QueryString["regno"], (string)Session["UserID"]);

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