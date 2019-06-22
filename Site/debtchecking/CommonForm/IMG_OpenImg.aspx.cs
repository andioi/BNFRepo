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


namespace Mikro.CommonForm
{
    public partial class IMG_OpenImg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string imgext = ".TIF";
            if (Request.QueryString["ext"] != null && Request.QueryString["ext"].Trim() != "")
                imgext = "." + Request.QueryString["ext"];
            string path = Server.MapPath("../Upload/Image") + "\\" + Request.QueryString["img"] + imgext;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (!file.Exists)
            {
                TRBTN.Visible = false;
            }
            else
            {
                switch (imgext.ToLower())
                {
                    case ".tif":
                        int pg = 0;
                        try { pg = int.Parse(Request.QueryString["pg"]); }
                        catch { }
                        if (pg >= 0)
                        {
                            IMDOC.Attributes["src"] = "IMG_TifImg.aspx?regno=" + Request.QueryString["regno"] +
                                "&img=" + Request.QueryString["img"] + "&pg=" + pg.ToString();
                        }
                        else
                        {
                            TRBTN.Visible = false;
                        }
                        break;

                    case ".jpg": 
                    case ".gif":
                        IMDOC.Attributes["src"] = "../Upload/Image/" + Request.QueryString["img"] + imgext;
                        break;

                    default:
                        DMS.Tools.MyPage.popMessage(this, "Image extention not supported!!");
                        break;
                }
            }
        }
    }
}
