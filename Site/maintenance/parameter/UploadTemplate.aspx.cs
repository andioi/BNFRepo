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
using System.Collections.Specialized;
using My.Framework;
using System.IO;
using My.Tools;

namespace MikroMnt.Parameter
{
    public partial class UploadTemplate : System.Web.UI.Page
    {

        private static string sql_import = "EXEC SP_UPLOAD_TEMPLATE @1";


        protected void Page_Load(object sender, EventArgs e)
        {
            //
        }

        protected void PanelFile_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
        }

        protected void ImportFile_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            HttpPostedFile userPostedFile = ImportFile.PostedFile;
            if (userPostedFile != null && userPostedFile.ContentLength > 0)
            {
                string ext = System.IO.Path.GetExtension(userPostedFile.FileName),
                    fn = "LIMIT SIMULATION PL.xlt";
                    //pathurl = "../../debtchecking/Templates/Master";
                string fullpath = ConfigurationSettings.AppSettings["TemplatePath"] + "\\" + fn;
                if (ext.ToLower()==".xlt")
                {
                    try
                    {
                        if (System.IO.File.Exists(fullpath)) System.IO.File.Delete(fullpath);
                        userPostedFile.SaveAs(fullpath);
                        /*object[] param = new object[] { Session["UserID"] };
                        using (DbConnection myconn = new DbConnection((string)ConfigurationSettings.AppSettings["connString"].ToString()))
                        {
                            myconn.ExecNonQuery(sql_import, param, 600);
                        }*/
                        Response.Write("<script>alert('Upload Success.');</script>");
                    }
                    catch
                    {
                        Response.Write("<script>alert('Upload Failed.');</script>");
                    }
                }
            }
        }     

    }
}
