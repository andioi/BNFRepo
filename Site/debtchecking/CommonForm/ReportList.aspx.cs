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

using DMS.Tools;
using DMS.Framework;

namespace DebtChecking.Report
{
    public partial class ReportList : MasterPage
    {
        public string BackURL
        {
            get { return (string)Session["BackURL"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strSQL = "", title = "";

                ReportSys.gridInit(grid, Request.QueryString["PV_ID"], ref title, ref strSQL, conn);
                TitleHeader.Text = title;
                ViewState["strSQL"] = strSQL;
            };
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            
            for(int flg =0 ; flg < UC_ReportFilter1.paramFilter.Length ;flg++)
            {
                if (UC_ReportFilter1.paramFilter[flg] is string)
                {
                    string param = (string)UC_ReportFilter1.paramFilter[flg];
                    if (param == "(none)")
                    {
                        UC_ReportFilter1.paramFilter[flg] = null;
                    }
                }
            }
            ReportSys.gridBind(grid, (string)ViewState["strSQL"], UC_ReportFilter1.paramFilter, UC_ReportFilter1.strFilter, conn);
        }

        protected string pivotgridExpoort(string contentType)
        {
            string fileName = USERID + "_" + TitleHeader.Text +"." + contentType;
            string filepath = MapPath("~/Upload/Report");
            System.IO.FileStream fs = new System.IO.FileStream(filepath + "\\" + fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            switch (contentType)
            {
                case "pdf":
                    DevExpress.XtraPrinting.PdfExportOptions pdfxport = new DevExpress.XtraPrinting.PdfExportOptions();
                    gridExport.WritePdf(fs, pdfxport);
                    break;
                case "xls":
                    gridExport.WriteXls(fs);
                    break;
                case "rtf":
                    gridExport.WriteRtf(fs);
                    break;
                case "csv":
                    gridExport.WriteCsv(fs);
                    break;
            }
            fs.Dispose();
            return "../Upload/Report/" + fileName;
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters.StartsWith("e:"))
                grid.JSProperties["cp_export"] = pivotgridExpoort(e.Parameters.Substring(2));
        }
    }
}
