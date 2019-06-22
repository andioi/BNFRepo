using System;
using System.Collections;
using System.Configuration;
using System.Data;
using DMS.Framework;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using DevExpress.Web;
using DMS.Tools;
using System.IO;
using System.Text;

namespace DebtChecking.Facilities
{
    public partial class ViewPDFPefindo : DataPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pefindoid = Request.QueryString["pefindoid"];
            string sql = "select top 1 pdfData from pdfPefindo where pefindoid = @1";
            conn.ExecReader(sql, new object[] { pefindoid }, dbtimeout);
            if (conn.hasRow())
            {
                byte[] pdfbyte = (byte[])conn.GetNativeFieldValue(0);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", pdfbyte.Length.ToString());
                Response.BinaryWrite(pdfbyte);
            } else
            {
                Response.Write("<center><b><font color='red'>PDF data not found</font></b></center>");
            }
        }

    }
}
