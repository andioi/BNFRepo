<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReputasiDetail.aspx.cs" Inherits="DebtChecking.Facilities.ReputasiDetail" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>
<%@ Register assembly="DMSControls" namespace="DMSControls" tagprefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="UC_ReputasiCustomer" Src="../CommonForm/UC_ReputasiCustomer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ReputasiDetail</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- #include file="~/include/UC/UC_Number.html" -->
    <!-- #include file="~/include/UC/UC_Currency.html" -->
    <!-- #include file="~/include/UC/UC_Decimal.html" -->
    <!-- #include file="~/include/UC/UC_Date.html" -->
    <script language="javascript" type="text/javascript">
        function download(href) {
            var X = (screen.availWidth - 800) / 2;
            var Y = (screen.availHeight - 600) / 2;
            window.open(href, "", "height=600px,width=800px,left=" + X + ",top=" + Y +
			",status=no,toolbar=no,scrollbars=yes,resizable=yes,titlebar=no,menubar=no,location=no,dependent=yes");
        }
        
        function fnExcelReport() {
            var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
            var textRange; var j = 0;
            tab = document.getElementById('allContent'); // id of table

            for (j = 0 ; j < tab.rows.length ; j++) {
                tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
                //tab_text=tab_text+"</tr>";
            }

            tab_text = tab_text + "</table>";
            tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
            tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
            tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(tab_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, "ReportReputasiDetail.xls");
            }
            else                 //other browser not tested on IE 11
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

            return (sa);
        }

    </script>
    <style type="text/css">
        .boxbold { border:1px solid black; text-align:right; }
        .boxboldleft { border:1px solid black; text-align:left; }
        .boxboldcenter { border:1px solid black; text-align:center; font-size:small; }
        .boxboldright { border:1px solid black; text-align:right; font-size:small; }
        body {text-align:center;}
        table {font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;font-size:small}
        .td01 { width:300px;text-align:left; padding-left:4px;}
        .td02 { width:150px;text-align:left; padding-left:4px}
        .td11 { text-align:left; padding-left:4px}
        .td12 { text-align:right; padding-left:4px}
        .Box2
        {
	        background-color: white;
	        border-style: solid;
	        border-width: 0px 1px 1px 1px;
	        border-color: #005263 #C0C0C0 #C0C0C0 #005263;
        }
    </style>
</head>
<body>
    <iframe id="txtArea1" style="display:none"></iframe>
    <form id="form1" runat="server">
    <input type="button" id="btnexcel" value="EXPORT EXCEL" style="display:none" runat="server" onclick="fnExcelReport();"></input>
    <center>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="910" 
    oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
        <input type="button" id="btnexcel_new" value="EXPORT EXCEL" runat="server" onclick="callback(mainPanel, 'export_excel');"> </input> 
    
        <input type="button" id="btnpdf" value="EXPORT PDF" runat="server" onclick="callback(pdfPanel,document.getElementById('pdfPanel_urlframe').value)"></input>


        <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
        oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
        <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
            <input type="hidden" id="urlframe" runat="server" />
        </dxp:PanelContent></PanelCollection>
        </dxcp:ASPxCallbackPanel>

    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
	</center>
    <table width="100%" id="allContent" border="0" cellpadding="0" cellspacing="0">
        <tr><td>
    <center><div id="divUC" runat="server"></div></center>
        </td></tr>
    </table>
    </form>
</body>
</html>
