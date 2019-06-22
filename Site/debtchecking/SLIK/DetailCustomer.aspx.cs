using System;
using System.Collections;
using System.Configuration;
using System.Data;
using DMS.Framework;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using DevExpress.Web;
using DMS.Tools;
using EvoPdf.HtmlToPdf;

namespace DebtChecking.Facilities
{
    public partial class DetailCustomer : DataPage
    {
        #region static vars
        private string Q_VW_SID_DEBITURINFO = "SELECT * FROM VW_SID_DEBITURINFO WHERE APPID = @1";
        int i;
        #endregion

        private void retrieve_debiturinfo(string key)
        {
            string sql = "select * from slik_vw_applicant where appid = @1";
            DataTable dt = conn.GetDataTable(sql, new object[] { key }, dbtimeout);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, reffnumber);
            staticFramework.retrieve(dt, status_app);
            staticFramework.retrieve(dt, cust_name);
            staticFramework.retrieve(dt, pob_dob);
            staticFramework.retrieve(dt, ktp);
            staticFramework.retrieve(dt, npwp);
            staticFramework.retrieve(dt, genderdesc);
            staticFramework.retrieve(dt, mother_name);
            staticFramework.retrieve(dt, full_ktpaddress);
            staticFramework.retrieve(dt, full_homeaddress);
            staticFramework.retrieve(dt, full_officeaddress);
            staticFramework.retrieve(dt, full_econaddress);

            sql = "select * from slik_vw_scoring_result where reffnumber = @1";
            dt = conn.GetDataTable(sql, new object[] { reffnumber.Value }, dbtimeout);
            staticFramework.retrieve(dt, scoring_totalutilization, "scoring_");
            staticFramework.retrieve(dt, scoring_ratiooverhalf, "scoring_");
            staticFramework.retrieve(dt, scoring_ratioundersixmonth, "scoring_");
            staticFramework.retrieve(dt, scoring_numofmontholdestopened, "scoring_");
            staticFramework.retrieve(dt, scoring_checkingstatus, "scoring_");
            staticFramework.retrieve(dt, scoring_remarks, "scoring_");
            staticFramework.retrieve(dt, downloaded);
            staticFramework.retrieve(dt, downloadby);
            staticFramework.retrieve(dt, downloaddate);
            staticFramework.retrieve(dt, batchid);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
                Page.Title = cust_name.Text;
            }
        }


        

    }
}
