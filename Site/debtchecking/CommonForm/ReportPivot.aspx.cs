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
using DevExpress.Utils;
using DMS.Tools;
using DMS.Framework;

namespace DebtChecking.Report
{
    public partial class ReportPivot : MasterPage
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

                ReportSys.pivotinit(grid, Request.QueryString["PV_ID"], ref title, ref strSQL, conn);
                TitleHeader.Text = title;
                ViewState["strSQL"] = strSQL;
            };
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected void grid_Load(object sender, EventArgs e)
        {
            ReportSys.pivotBind(grid, (string)ViewState["strSQL"], UC_ReportFilter1.paramFilter, UC_ReportFilter1.strFilter, conn);
        }

        protected string pivotgridExpoort(string contentType)
        {
            gridExport.OptionsPrint.PrintHeadersOnEveryPage = true;
            gridExport.OptionsPrint.PrintFilterHeaders = DefaultBoolean.True;
            gridExport.OptionsPrint.PrintColumnHeaders = DefaultBoolean.True;
            gridExport.OptionsPrint.PrintRowHeaders = DefaultBoolean.True;
            gridExport.OptionsPrint.PrintDataHeaders = DefaultBoolean.True;
            string fileName = USERID + "_" + TitleHeader.Text + "." + contentType;
            string filepath = MapPath("~/Upload/Report");
            switch (contentType)
            {
                case "pdf":
                    DevExpress.XtraPrinting.PdfExportOptions pdfxport = new DevExpress.XtraPrinting.PdfExportOptions();
                    gridExport.ExportToPdf(filepath + "\\" + fileName, pdfxport);
                    break;
                case "xls":
                    gridExport.ExportToXls(filepath + "\\" + fileName);
                    break;
                case "mht":
                    gridExport.ExportToMht(filepath + "\\" + fileName);
                    break;
                case "rtf":
                    gridExport.ExportToRtf(filepath + "\\" + fileName);
                    break;
                case "txt":
                    gridExport.ExportToText(filepath + "\\" + fileName);
                    break;
                case "htm":
                    gridExport.ExportToHtml(filepath + "\\" + fileName);
                    break;
            }
            return "../Upload/Report/" + fileName;
        }

        protected void grid_CustomCallback(object sender, DevExpress.Web.ASPxPivotGrid.PivotGridCustomCallbackEventArgs e)
        {
            if(e.Parameters.StartsWith("e:"))
                grid.JSProperties["cp_export"] = pivotgridExpoort(e.Parameters.Substring(2));
         }


    }
}
