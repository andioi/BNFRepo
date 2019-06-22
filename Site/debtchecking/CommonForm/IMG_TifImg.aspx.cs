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
    public partial class IMG_TifImg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string path = Server.MapPath("../Upload/Image") + "\\" + Request.QueryString["img"] + ".TIF";
            System.IO.FileInfo tif = new System.IO.FileInfo(path);
            if (!tif.Exists)
                return;

            int pg = int.Parse(Request.QueryString["pg"]);
            AMA.Util.TiffManager mgr = new AMA.Util.TiffManager(tif.FullName);
            System.Drawing.Image img = mgr.GetSpecificPage(pg);
            mgr.Dispose();

            Response.ContentType = "image/gif";
            img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
        }
    }
}
