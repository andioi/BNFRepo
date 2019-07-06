using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace DebtChecking.SLIK
{
    public partial class RequestData
    {
        public string ReqID;
        public string NIK;
        public string sUserId;
    }

    public partial class Dukcapil : DataPage
    {

        private void retrieve_dukcapil(string nik)
        {
            try
            {
                RequestData ReqData = new RequestData();
                ReqData.ReqID = "Req_" + nik;
                ReqData.NIK = nik;
                ReqData.sUserId = "cbas";

                RestRequest Request = new RestRequest("/request", Method.POST, DataFormat.Json);
                Request.RequestFormat = DataFormat.Json;
                Request.AddJsonBody(ReqData);

                var Client = new RestClient("http://10.10.1.43/RequestIdeb/Form/PostBackPage.asmx/GetDetailKTP");
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                var Response = Client.Execute(Request);
                dynamic msg = JsonConvert.DeserializeObject(Response.Content);
                content.InnerHtml = msg.d;
            }
            catch (Exception ex)
            {
                content.InnerHtml = ex.Message;
            }
        }

 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ktp = Request.QueryString["ktp"];
                retrieve_dukcapil(ktp);
                Page.Title = "Data Dukcapil " + ktp;
            }
        }

    }
}
