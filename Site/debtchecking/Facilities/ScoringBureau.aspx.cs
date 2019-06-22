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
using DMS.Framework;

namespace DebtChecking.Facilities
{
    public partial class ScoringBureau : DataPage
    {
        #region static vars
        private string Q_VW_PV = "select * from VW_SCORING_BIRO where appnumber = (select reffnumber from applicant where appid = @1)";
        #endregion

        #region retrieve
        private void initial_reffparameter()
        {
            //
        }

        private void retrieve_schema()
        {
            //

        }

        private void retrieve_data(string key)
        {
            object[] par = new object[] { key };
            DataTable dt = conn.GetDataTable(Q_VW_PV, par, dbtimeout);
            staticFramework.retrieve(dt, appnumber);
            staticFramework.retrieve(dt, product);
            staticFramework.retrieve(dt, createdate);
            staticFramework.retrieve(dt, statestartdate);
            staticFramework.retrieve(dt, bureaurequestdate);
            staticFramework.retrieve(dt, bi_response);
            staticFramework.retrieve(dt, monthlyincomefrombureau);
            staticFramework.retrieve(dt, hasccfacility);
            staticFramework.retrieve(dt, scoringdecdate);
            staticFramework.retrieve(dt, finaldecision);
            staticFramework.retrieve(dt, scoreband);
            staticFramework.retrieve(dt, limitassigned);
            staticFramework.retrieve(dt, dbr);
            staticFramework.retrieve(dt, tue);
            staticFramework.retrieve(dt, mue);
            staticFramework.retrieve(dt, total_debt_burden);
            staticFramework.retrieve(dt, total_uns_exp);
            staticFramework.retrieve(dt, reject_score);
            staticFramework.retrieve(dt, msccode);
            staticFramework.retrieve(dt, mscsegment);
            staticFramework.retrieve(dt, rejectreason);
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            //retrieve_schema();
            //initial_reffparameter();
            retrieve_data(Request.QueryString["regno"]);
        }

    }
}
