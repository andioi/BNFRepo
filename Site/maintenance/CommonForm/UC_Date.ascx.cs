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

using My.Tools;

namespace MikroMnt.CommonForm
{
    public partial class UC_Date : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (DDL_MM.Items.Count == 0)	// not init yet
                    MyPage.initDateForm(TXT_DD, DDL_MM, TXT_YY);
                string code;
                TextBox tmpDD, tmpYY;
                DropDownList tmpMM;
                tmpDD = (TextBox)FindControl("TXT_DD");
                tmpMM = (DropDownList)FindControl("DDL_MM");
                tmpYY = (TextBox)FindControl("TXT_YY");

                code = "if(!numbersonly()) return false; if (this.value<=0 || this.value >31) this.value = this.value.substr(0,this.value.length-1);";
                tmpDD.Attributes.Add("onkeyup", code);
                tmpDD.Attributes.Add("onkeydown", code);
                code = "if(!numbersonly()) return false; if (this.value <= 0) this.value = this.value.substr(0,this.value.length-1);";
                tmpYY.Attributes.Add("onkeyup", code);
                tmpYY.Attributes.Add("onkeydown", code);

                string date_code;
                date_code = "displayDatePicker('" + tmpDD.ClientID + "/" + tmpMM.ClientID + "/" + tmpYY.ClientID + "',this.name,this);";
                IB_DATE.Attributes.Add("onclick", date_code);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (DDL_MM.Items.Count == 0)	// not init yet
                MyPage.initDateForm(TXT_DD, DDL_MM, TXT_YY);
        }

        #region Properties
        public string TXT_DD_TEXT
        {
            get { return TXT_DD.Text; }
            set { TXT_DD.Text = value; }
        }
        public string TXT_YY_TEXT
        {
            get { return TXT_YY.Text; }
            set { TXT_YY.Text = value; }
        }
        public string DDL_MM_SelectedValue
        {
            get { return DDL_MM.SelectedValue; }
            set { DDL_MM.SelectedValue = value; }
        }
        public int DDL_MM_SelectedIndex
        {
            get { return DDL_MM.SelectedIndex; }
            set { DDL_MM.SelectedIndex = value; }
        }
        public void setDate(TextBox Day, DropDownList Month, TextBox Year)
        {
            try
            {
                TXT_DD.Text = Day.Text;
                DDL_MM.SelectedValue = Month.SelectedValue;
                TXT_YY.Text = Year.Text;
            }
            catch { ClearDate(); }
        }

        public void setDate(int Day, int Month, int Year)
        {
            try
            {
                TXT_DD.Text = Day.ToString();
                DDL_MM.SelectedValue = Month.ToString();
                TXT_YY.Text = Year.ToString();
            }
            catch { ClearDate(); }
        }

        public void setDate(string Day, string Month, string Year)
        {
            try
            {
                TXT_DD.Text = Day;
                DDL_MM.SelectedValue = Month;
                TXT_YY.Text = Year;
            }
            catch { ClearDate(); }
        }

        public void setDate(DateTime date)
        {
            try
            {
                MyPage.initDateForm(TXT_DD, DDL_MM, TXT_YY);
                TXT_DD.Text = date.Day.ToString();
                DDL_MM.SelectedValue = date.Month.ToString();
                TXT_YY.Text = date.Year.ToString();
            }
            catch { ClearDate(); }
        }

        public void setDate(string date)
        {
            try
            {
                DateTime tmp = DateTime.Parse(date);
                MyPage.initDateForm(TXT_DD, DDL_MM, TXT_YY);
                TXT_DD.Text = tmp.Day.ToString();
                DDL_MM.SelectedValue = tmp.Month.ToString();
                TXT_YY.Text = tmp.Year.ToString();
            }
            catch { ClearDate(); }
        }


        public DateTime getDate
        {
            get
            {
                try { return MyPage.ToDateTime(TXT_DD, DDL_MM, TXT_YY); }
                catch { return DateTime.Parse("1/1/1900"); }
            }
        }

        public bool Enabled
        {
            set { TXT_DD.Enabled = value; TXT_YY.Enabled = value; DDL_MM.Enabled = value; IB_DATE.Visible = value; }
        }

        public void ClearDate()
        {
            TXT_DD.Text = "";
            DDL_MM.ClearSelection();
            TXT_YY.Text = "";
        }

        // Added by			: Tri Maryanto
        // Purposes			: Mengambil property dalam bentuk object untuk mendisplay tanggal dari query database

        public TextBox OBJ_TXT_DD
        {
            get { return TXT_DD; }
        }

        public TextBox OBJ_TXT_YY
        {
            get { return TXT_YY; }
        }

        public DropDownList OBJ_DDL_MM
        {
            get { return DDL_MM; }
        }

        public string CssClass
        {
            set
            {
                TXT_DD.CssClass = value;
                DDL_MM.CssClass = value;
                TXT_YY.CssClass = value;
            }
        }

        public short TabIndex
        {
            set
            {
                TXT_DD.TabIndex = value;
            }
        }
        #endregion

        public object GetDate()
        {
            try
            {
                if (TXT_DD.Text.Trim() != "" && DDL_MM.SelectedIndex > 0 && TXT_YY.Text.Trim() != "")
                    return MyPage.ToDateTime(TXT_DD, DDL_MM, TXT_YY);
                else
                    return null;
            }
            catch { return null; }
        }
    }
}