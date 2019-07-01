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
using System.IO;
using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using Ionic.Zip;
using static DebtChecking.Facilities.IdebJson;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DebtChecking.Facilities
{
    public partial class SLIKResult : DataPage
    {

        int i;

        private void retrieve_debiturinfo(string key)
        {
            string sql = "select * from slik_vw_applicant where appid = @1";
            DataTable dt = conn.GetDataTable(sql, new object[] { key }, dbtimeout);
            staticFramework.retrieve(dt, appid);
            staticFramework.retrieve(dt, reffnumber);
            staticFramework.retrieve(dt, status_app);
            staticFramework.retrieve(dt, "appid", ddl_appid);
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
            staticFramework.retrieve(dt, final_policy);
        }

        private void runcreditpolicy(string key)
        {
            object[] par = new object[] { key };
            conn.ExecNonQuery("exec slik_clearPolicy @1", par, dbtimeout);
            string sql = "select * from slik_vw_creditpolicy where appid = @1 ";
            DataTable dt = conn.GetDataTable(sql, par, dbtimeout);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DecSystem d = new DecSystem(conn);
                string resultid = dt.Rows[i]["fasilitasid"].ToString();
                par = new object[] { resultid, "SLIK" };
                try
                {
                    string ResultID = d.execute("AND [slik_ideb_kredit].[fasilitasid]=@1", par, "POLICY", "SLIK", null);
                    par = new object[] { resultid, "POLICY", "SLIK", ResultID };
                    conn.ExecuteNonQuery("EXEC SP_APPDECSYSRES @1,@2,@3,@4", par, dbtimeout);
                }
                catch { }
            }
            par = new object[] { key };
            conn.ExecNonQuery("exec slik_updFinalPolicy @1", par, dbtimeout);
        }

        private void cekrules()
        {
            conn.ExecReader("select approval_group from scallgroup where groupid = @1", new object[] { GROUPID }, dbtimeout);
            if (conn.hasRow())
            {
                if (conn.GetFieldValue(0).ToString() == "1")
                {
                    Button1.Disabled = false;
                }
                else
                {
                    Button1.Disabled = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Q_NAME_LIST = "select appid, cust_name+' ('+status_app+')' cust_name from slik_applicant where " +
                "reffnumber = (select reffnumber from slik_applicant where appid = @1)";
                staticFramework.reff(ddl_appid, Q_NAME_LIST, new object[] { Request.QueryString["regno"] }, conn, false);
                ddl_appid.SelectedValue = Request.QueryString["regno"];
                //ListItem ddlItem = new ListItem("- All -","all");
                //ddl_appid.Items.Add(ddlItem);
            }
        }

        private void ViewData(string key)
        {

            string src = "";
            try
            {
                DataTable dt = conn.GetDataTable("select * from slik_resultmatch where appid = @1", new object[] { key }, dbtimeout);
                listvaluestring.Value = "";
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    string nik = dt.Rows[i]["nik"].ToString();
                    string detailid = dt.Rows[i]["trn_ideb_detail_id"].ToString();
                    string idebid = dt.Rows[i]["trn_ideb_id"].ToString();
                    //string pdfviewsite = ConfigurationSettings.AppSettings["pdfviewsite"];
                    string pdfviewsite = "ViewPDF.aspx?";

                    TB_SIDLIST.Rows.Add(new TableRow());
                    TB_SIDLIST.Rows[i].Cells.Add(new TableCell());
                    //checkbox
                    System.Web.UI.WebControls.CheckBox ck = new CheckBox();
                    ck.ID = "CK_" + i.ToString();
                    ck.Attributes.Add("onclick", "ceknik('"+ dt.Rows[i]["nik"].ToString() + "',this);");
                    if ((bool)dt.Rows[i]["selected"])
                    {
                        ck.Checked = true;
                        listvaluestring.Value += dt.Rows[i]["nik"].ToString()+":1,";
                    }
                    else
                    {
                        ck.Checked = false;
                        listvaluestring.Value += dt.Rows[i]["nik"].ToString() + ":0,";
                    }
                    TB_SIDLIST.Rows[i].Cells[0].Controls.Add(ck);
                    System.Web.UI.WebControls.HyperLink h = new HyperLink();
                    h.ID = "HL_" + i.ToString();
                    h.Target = "IFR_TEXT";
                    h.Text = dt.Rows[i]["linkname"].ToString();
                    h.Attributes.Add("style", "cursor:hand");
                    string urlnavigate = "notyetuploaded.html";
                    if (!String.IsNullOrEmpty(idebid) && !String.IsNullOrEmpty(detailid))
                        urlnavigate = pdfviewsite + "idebid="+idebid+"&detailid="+detailid;
                    h.Attributes.Add("onclick", "javascript:kliklink('HL_" + i.ToString() + "','" + urlnavigate + "')");
                    if (dt.Rows[i]["POLRES"].ToString() != "1") h.ForeColor = System.Drawing.Color.Red;
                    h.ToolTip = dt.Rows[i]["result_name"].ToString();
                    TB_SIDLIST.Rows[i].Cells[0].Controls.Add(h);
                    if (i == 0)
                    {
                        src = urlnavigate;
                        urlframe.Value = urlnavigate;
                    }
                    //hyperlink matching score
                    System.Web.UI.WebControls.HyperLink h2 = new HyperLink();
                    h2.ID = "HLmatch_" + i.ToString();
                    h2.Text = " (" + dt.Rows[i]["match_level"].ToString() + ")";
                    //h2.Font.Underline = true;
                    h2.Attributes.Add("style", "cursor:hand");
                    h2.Attributes.Add("onclick", "javascript:PopupPage('DetailValidation.aspx?id=" + dt.Rows[i]["resultid"].ToString()+"',800,600)");
                    TB_SIDLIST.Rows[i].Cells[0].Controls.Add(h2);
                }
            }
            catch (Exception ex)
            {
                MyPage.popMessage(this, ex.Message);
            }
            nikcount.Value = i.ToString();
            if (i == 0)
            {
                dv_found.Attributes.Add("style", "display:none");
            }
            else
            {
                TR_MSG.Visible = false;
                TR_FRAME.Visible = true;
                IFR_TEXT.Attributes.Add("src", src);
                btnpdf.Style.Add("display", "none");
                dv_found.Attributes.Remove("style");
            }
        }

        private void ViewDataNotFound(string key)
        {
            string src = "";
            int found = i; 
            try
            {
                DataTable dt2 = conn.GetDataTable("SELECT * FROM slik_notfound WHERE appid = @1", new object[] { key }, dbtimeout); 
                for (int c = 0; c <= dt2.Rows.Count - 1; c++)
                {
                    TB_NIHIL.Rows.Add(new TableRow());
                    TB_NIHIL.Rows[c].Cells.Add(new TableCell());
                    i++;
                    //hyperlink
                    System.Web.UI.WebControls.HyperLink h = new HyperLink();
                    h.ID = "HN_" + i.ToString();
                    h.Target = "IFR_TEXT";
                    h.Text = "- <u>Combination " + (c+1).ToString() + "</u>";
                    h.Attributes.Add("style", "cursor:hand");
                    string urlnavigate = "SLIKNotFound.aspx?id=" + dt2.Rows[c]["combinationid"].ToString();
                    h.Attributes.Add("onclick", "javascript:kliklink('HN_" + i.ToString() + "','" + urlnavigate + "')");
                    TB_NIHIL.Rows[c].Cells[0].Controls.Add(h);
                    if (found == 0 && c == 0)
                    {
                        src = urlnavigate;
                        urlframe.Value = urlnavigate;
                    }
                }
                if (dt2.Rows.Count == 0) dv_nihil.Attributes.Add("style", "display:none");
                else dv_nihil.Attributes.Remove("style");
            }
            catch (Exception ex)
            {
                //MyPage.popMessage(this, ex.Message);
            }
            if (found == 0) dv_found.Attributes.Add("style", "display:none");
            else dv_found.Attributes.Remove("style");
            if (i == 0)
            {
                LBL_MSG.Text = "DATA ONPROCESS";
                TR_MSG.Visible = true;
                TR_FRAME.Visible = false;
                btnprint.Visible = false;
                btnpdf.Visible = false;
            }
            if (found == 0 && i > 0)
            {
                TR_MSG.Visible = false;
                TR_FRAME.Visible = true;
                IFR_TEXT.Attributes.Add("src", src);
            }
        }

        protected static bool EnabledControls(Control ctrl, bool postback)
        {
            foreach (Control child in ctrl.Controls)
                try
                {
                    postback = EnabledControls(child, postback);
                }
                catch { }

            switch (ctrl.GetType().Name)
            {
                case "HyperLink":
                    HyperLink a = (HyperLink)ctrl;
                    a.Enabled = true;
                    break;
                default:
                    break;
            }

            return postback;
            
        }

        #region mainpanel callback

        protected void mainPanel_Callback(object source, CallbackEventArgsBase e)
        {
            mainPanel.JSProperties["cp_export"] = "";
            mainPanel.JSProperties["cp_filename"] = "";

            if (e.Parameter == "load")
            {
                retrieve_debiturinfo(Request.QueryString["regno"]);
                ViewData(Request.QueryString["regno"]);
                ViewDataNotFound(Request.QueryString["regno"]);
                cekrules();
            }
            else if (e.Parameter.StartsWith("r:"))
            {
                string key = e.Parameter.Substring(2);
                retrieve_debiturinfo(key);
                ViewData(key);
                ViewDataNotFound(key);
            }
            else if (e.Parameter.StartsWith("s:"))
            {
                try
                {
                    string val = e.Parameter.Substring(2);
                    string[] arr = val.Split(',');
                    for (int i=0;i<arr.Length;i++)
                    {
                        string val2 = arr[i];
                        if (!String.IsNullOrEmpty(val2))
                        {
                            string[] xarr = val2.Split(':');
                            string SP = "exec SLIK_UPDATE_SELECTION @1,@2,@3";
                            object[] prmtr = new object[] { appid.Value, xarr[0], xarr[1] };
                            conn.ExecuteNonQuery(SP, prmtr, dbtimeout);
                        }
                    }
                    runcreditpolicy(appid.Value);
                    retrieve_debiturinfo(appid.Value);
                    ViewData(appid.Value);
                    ViewDataNotFound(appid.Value);
                    mainPanel.JSProperties["cp_alert"] =  "Data Saved & Recalculated.";
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }
            }
            else if (e.Parameter == "d:")
            {
                try
                {
                    string userName = "";
                    conn.ExecReader("select SU_FULLNAME from scalluser where USERID = @1", new object[] { USERID }, dbtimeout);
                    if (conn.hasRow())
                    {
                        userName = conn.GetFieldValue(0).ToString();
                    }

                    string sql = "select p.reffnumber, p.cust_name, n.nik, a.ideb_content_ina, a.ideb_pdf, r.resultid " +
                            "from trn_ideb_detail_attrs a " +
                            "join trn_ideb_details d on d.trn_ideb_detail_id = a.trn_ideb_detail_id "+
                            "join slik_appnik n on n.trn_ideb_detail_id = d.trn_ideb_detail_id and n.match = 1 " +
                            "join slik_applicant p on p.appid = n.appid "+
                            "join slik_appsearchresult r on r.reffnumber = p.reffnumber and r.status_app = p.status_app and r.nik = n.nik "+
                            "where p.appid = @1";
                    DataTable dt = conn.GetDataTable(sql, new object[] { appid.Value } , dbtimeout);

                    string DownloadPath = "../Download/All/"+USERID+"/";
                    string PhysicalPath = Server.MapPath(DownloadPath);
                    if (!Directory.Exists(PhysicalPath)) Directory.CreateDirectory(PhysicalPath);

                    DirectoryInfo diTemp = new DirectoryInfo(PhysicalPath);
                    foreach (FileInfo Finf_loopVariable in diTemp.GetFiles("*.*"))
                    {
                        File.Delete(Finf_loopVariable.FullName);
                    }

                    string reffnumber = null, cust_name = null;

                    for (int i=0;i < dt.Rows.Count; i++)
                    {
                        reffnumber = dt.Rows[i]["reffnumber"].ToString();
                        cust_name = dt.Rows[i]["cust_name"].ToString();
                        string pdffilename = reffnumber + "_" + cust_name + "_" + dt.Rows[i]["nik"].ToString() + ".pdf";
                        pdffilename = PhysicalPath + "\\" + pdffilename;
                        byte[] pdfbyte = (byte[])dt.Rows[i]["ideb_pdf"];
                        //File.WriteAllBytes(pdffilename, pdfbyte);

                        System.IO.File.WriteAllBytes(Server.MapPath("../temp/") + reffnumber + "_" + i.ToString() + ".pdf", pdfbyte);
                        new SLIK.Watermark().AddWatermark("Downloaded by: " + userName, Server.MapPath("../temp/") + reffnumber + "_" + i.ToString() + ".pdf",
                            Server.MapPath("../temp/") + reffnumber + "_" + i.ToString() + "_result.pdf");
                        pdfbyte = System.IO.File.ReadAllBytes(Server.MapPath("../temp/") + reffnumber + "_" + i.ToString() + "_result.pdf");
                        File.WriteAllBytes(pdffilename, pdfbyte);
                        File.Delete(Server.MapPath("../temp/") + reffnumber + "_" + i.ToString() + ".pdf");
                        File.Delete(Server.MapPath("../temp/") + reffnumber + "_" + i.ToString() + "_result.pdf");

                        string txtfilename = reffnumber + "_" + cust_name + "_" + dt.Rows[i]["nik"].ToString() + ".txt";
                        txtfilename = PhysicalPath + "\\" + txtfilename;
                        //string json = dt.Rows[i]["ideb_content_ina"].ToString();
                        string json = generateIdebJson(conn, dt.Rows[i]["resultid"].ToString(), dbtimeout);
                        var fjson = JToken.Parse(json).ToString(Formatting.Indented);
                        File.WriteAllText(txtfilename, fjson);
                    }

                    if (!String.IsNullOrEmpty(reffnumber))
                    {
                        System.Collections.Generic.IList<string> documentPaths = new System.Collections.Generic.List<string>();
                        DirectoryInfo diTemp2 = new DirectoryInfo(PhysicalPath);
                        foreach (FileInfo Finf_loopVariable in diTemp2.GetFiles("*.*"))
                        {
                            documentPaths.Add(Finf_loopVariable.FullName);
                        }

                        using (ZipFile Zip = new ZipFile())
                        {
                            Zip.AddFiles(documentPaths, false, "");
                            Zip.Save(string.Format(PhysicalPath + "{0}_{1}.zip", reffnumber, cust_name));
                        }

                        mainPanel.JSProperties["cp_export"] = DownloadPath + reffnumber + "_" + cust_name + ".zip";
                        mainPanel.JSProperties["cp_filename"] = reffnumber + "_" + cust_name + ".zip";
                    }
                    else
                    {
                        mainPanel.JSProperties["cp_alert"] = "No data to download";
                    }
                    ViewData(appid.Value);
                    ViewDataNotFound(appid.Value);
                }
                catch (Exception ex)
                {
                    string errmsg = ex.Message;
                    if (errmsg.IndexOf("Last Query") > 0)
                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                    mainPanel.JSProperties["cp_alert"] = errmsg;
                }

            }
        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            int seq;
            for (int x = 0; x < i; x++)
            {
                CheckBox c = Page.FindControl("CK_" + x.ToString()) as CheckBox;
                HtmlInputHidden hd = Page.FindControl("HD_" + x.ToString()) as HtmlInputHidden;
                if (c != null)
                {
                    seq = x + 1;
                    string selection = "1";
                    string chkSeq = "";
                    if (c.Checked) { selection = "1"; } else { selection = "0"; }
                    chkSeq = hd.Value;
                    string SP = "exec SLIK_UPDATE_SELECTION @1,@2,@3";
                    object[] prmtr = new object[] { appid.Value, chkSeq, selection };
                    conn.ExecuteNonQuery(SP, prmtr, dbtimeout);
                }
            }
            runcreditpolicy(appid.Value);
            MyPage.popMessage(this, "Data Saved & Recalculated.");
        }
  
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            EnabledControls(this, false);
        }

        protected void pdfPanel_Callback(object source, CallbackEventArgsBase e)
        {
            string url = "";
            try
            {
                string DownloadPath = Server.MapPath("../Download/pdf/");

                PdfConverter pdfConverter = new PdfConverter();
                // set the license key
                pdfConverter.LicenseKey = "Vn1ndmVldmdkdmV4Y3ZlZ3hnZHhvb29v";
                // save the PDF bytes in a file on disk
                //string url = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) + "/" + e.Parameter + "&bypasssession=1";
                url = Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf("/")) + "/" + e.Parameter + "&bypasssession=1";
                Guid guid = Guid.NewGuid();
                string filename = guid.ToString() + ".pdf";
                string fullfilename = DownloadPath + filename;
                pdfConverter.SavePdfFromUrlToFile(url, fullfilename);
                pdfPanel.JSProperties["cp_redirect"] = "../Download/pdf/" + filename;
                pdfPanel.JSProperties["cp_target"] = "_blank";
                //Response.Write("<script>window.open('../Download/pdf/" + filename + "');</script>");
            }
            catch (Exception ex)
            {
                pdfPanel.JSProperties["cp_alert"] = url+ " could not be load.";
            }
        }

        public T BindData<T>(DataTable dt, int row)
        {
            DataRow dr = dt.Rows[row];

            // Get all columns' name
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            // Create object
            var ob = Activator.CreateInstance<T>();

            // Get all fields
            var fields = typeof(T).GetFields();
            foreach (var fieldInfo in fields)
            {
                if (columns.Contains(fieldInfo.Name))
                {
                    // Fill the data into the field
                    fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                }
            }

            // Get all properties
            var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (columns.Contains(propertyInfo.Name))
                {
                    // Fill the data into the property
                    if (!DBNull.Value.Equals(dr[propertyInfo.Name]))
                        propertyInfo.SetValue(ob, dr[propertyInfo.Name], null);
                    else
                        propertyInfo.SetValue(ob, "", null);
                }
            }

            return ob;
        }

        public string generateIdebJson(DbConnection conn, string resultid, int dbTimeout)
        {
            string result = "";
            DataTable dtheader = conn.GetDataTable("select * from slik_ideb where resultid = @1", new object[] { resultid }, dbTimeout);
            DataTable dtpokok = conn.GetDataTable("select * from slik_ideb_datapokokdebitur where resultid = @1", new object[] { resultid }, dbTimeout);
            DataTable dtkredit = conn.GetDataTable("select * from slik_ideb_kredit where resultid = @1", new object[] { resultid }, dbTimeout);

            if (dtheader.Rows.Count > 0)
            {
                if (dtheader.Rows[0]["isCompany"].ToString() == "0")
                {
                    header header = BindData<header>(dtheader, 0);
                    individual individu = BindData<individual>(dtheader, 0);
                    parameterPencarian searchParam = BindData<parameterPencarian>(dtheader, 0);
                    ringkasanFasilitas ringkasanfasilitas = BindData<ringkasanFasilitas>(dtheader, 0);
                    //datapokok
                    List<dataPokokDebitur> datapokoks = new List<dataPokokDebitur>();
                    for (int i = 0; i < dtpokok.Rows.Count; i++)
                    {
                        dataPokokDebitur datapokok = BindData<dataPokokDebitur>(dtpokok, i);
                        datapokoks.Add(datapokok);
                    }
                    //kredit
                    List<kreditPembiayan> kredits = new List<kreditPembiayan>();
                    for (int i = 0; i < dtkredit.Rows.Count; i++)
                    {
                        kreditPembiayan kredit = BindData<kreditPembiayan>(dtkredit, i);
                        string fasilitasid = dtkredit.Rows[i]["fasilitasid"].ToString();
                        DataTable dtagunan = conn.GetDataTable("select * from slik_ideb_agunan where resultid = @1 and fasilitasid = @2", new object[] { resultid, fasilitasid }, dbTimeout);
                        //agunan
                        List<agunan> agunans = new List<agunan>();
                        for (int x = 0; x < dtagunan.Rows.Count; x++)
                        {
                            agunan agunan = BindData<agunan>(dtagunan, x);
                            agunans.Add(agunan);
                        }

                        kredit.agunan = agunans.ToArray();
                        List<penjamin> penjamins = new List<penjamin>();
                        kredit.penjamin = penjamins.ToArray();
                        kredits.Add(kredit);
                    }

                    individu.dataPokokDebitur = datapokoks.ToArray();
                    individu.parameterPencarian = searchParam;
                    individu.ringkasanFasilitas = ringkasanfasilitas;
                    fasilitas fasilitas = new fasilitas();
                    fasilitas.kreditPembiayan = kredits.ToArray();
                    individu.fasilitas = fasilitas;

                    idebIndividu ideb = new idebIndividu();
                    ideb.header = header;
                    ideb.individual = individu;
                    result = JsonConvert.SerializeObject(ideb);

                } else
                {
                    header header = BindData<header>(dtheader, 0);
                    perusahaan perusahaan = BindData<perusahaan>(dtheader, 0);
                    parameterPencarianPerusahaan searchParam = BindData<parameterPencarianPerusahaan>(dtheader, 0);
                    ringkasanFasilitas ringkasanfasilitas = BindData<ringkasanFasilitas>(dtheader, 0);
                    //datapokok
                    List<dataPokokDebiturPerusahaan> datapokoks = new List<dataPokokDebiturPerusahaan>();
                    for (int i = 0; i < dtpokok.Rows.Count; i++)
                    {
                        dataPokokDebiturPerusahaan datapokok = BindData<dataPokokDebiturPerusahaan>(dtpokok, i);
                        datapokoks.Add(datapokok);
                    }
                    //kredit
                    List<kreditPembiayan> kredits = new List<kreditPembiayan>();
                    for (int i = 0; i < dtkredit.Rows.Count; i++)
                    {
                        kreditPembiayan kredit = BindData<kreditPembiayan>(dtkredit, i);
                        string fasilitasid = dtkredit.Rows[i]["fasilitasid"].ToString();
                        DataTable dtagunan = conn.GetDataTable("select * from slik_ideb_agunan where resultid = @1 and fasilitasid = @2", new object[] { resultid, fasilitasid }, dbTimeout);
                        //agunan
                        List<agunan> agunans = new List<agunan>();
                        for (int x = 0; x < dtagunan.Rows.Count; x++)
                        {
                            agunan agunan = BindData<agunan>(dtagunan, x);
                            agunans.Add(agunan);
                        }

                        kredit.agunan = agunans.ToArray();
                        List<penjamin> penjamins = new List<penjamin>();
                        kredit.penjamin = penjamins.ToArray();
                        kredits.Add(kredit);
                    }

                    perusahaan.dataPokokDebitur = datapokoks.ToArray();
                    perusahaan.parameterPencarian = searchParam;
                    perusahaan.ringkasanFasilitas = ringkasanfasilitas;
                    fasilitas fasilitas = new fasilitas();
                    fasilitas.kreditPembiayan = kredits.ToArray();
                    perusahaan.fasilitas = fasilitas;

                    idebPerusahaan ideb = new idebPerusahaan();
                    ideb.header = header;
                    ideb.perusahaan = perusahaan;
                    result = JsonConvert.SerializeObject(ideb);
                }
            }

            return result;
        }

    }
}
