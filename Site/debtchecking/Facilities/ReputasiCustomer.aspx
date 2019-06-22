<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReputasiCustomer.aspx.cs" Inherits="DebtChecking.Facilities.ReputasiCustomer" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>
<%@ Register assembly="DMSControls" namespace="DMSControls" tagprefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="UC_ReputasiCustomer" Src="../CommonForm/UC_ReputasiCustomer.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ReputasiCustomer</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <script language="javascript" type="text/javascript">
        function savevalidation()
        {
            var r = true;
            var limitbaru = document.getElementById("mainPanel_limit_baru").value;
            var limittmt = document.getElementById("mainPanel_limit_tmt").value;
            var limittu = document.getElementById("mainPanel_limit_trakindo").value;
            if (limitbaru <= 0) {
                alert("Limit baru tidak boleh <= 0");
                r = false;
            }
            if (limittmt < 0 || limittu < 0) {
                alert("Limit Group tidak boleh < 0");
                r = false;
            }
            return r;
        }
        
        function fnExcelReport() {
            var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
            var textRange; var j = 0;
            tab = document.getElementById('Content'); // id of table

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
                sa = txtArea1.document.execCommand("SaveAs", true, "ReportReputasi.xls");
            }
            else                 //other browser not tested on IE 11
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

            return (sa);
        }
		
		function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }  

    </script>
    <style type="text/css">
        .boxbold { border:1px solid black; text-align:right; }
        .boxboldleft { border:1px solid black; text-align:left; }
        .boxboldcenter { border:1px solid black; text-align:center; font-size:small; }
        body {text-align:center;}
        table {font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;font-size:small}
        .td01 { width:300px;text-align:left; padding-left:4px}
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

 <center>
    <form id="form1" runat="server">
    <input type="button" id="btnexcel" value="EXPORT EXCEL" runat="server" style="display:none" onclick="fnExcelReport();"> </input>
    <input type="button" id="btnexcel_new" value="EXPORT EXCEL" runat="server" onclick="callback(mainPanel, 'export_excel');"> </input>
    <input type="button" id="btnpdf" value="EXPORT PDF" runat="server" onclick="callback(pdfPanel,document.getElementById('pdfPanel_urlframe').value)"></input>

    <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
    oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
    <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
        <input type="hidden" id="urlframe" runat="server" />
    </dxp:PanelContent></PanelCollection>
    </dxcp:ASPxCallbackPanel>

    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="910" 
        oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">

    <table id="Content" class="Box1" width="100%" align="center">
    <tr>
        <td width="650" bgcolor="#33cc33">&nbsp;</td>
        <td width="80" bgcolor="#e7ebcb">&nbsp;</td>
        <td width="180" bgcolor="white" align="center" valign="bottom"><img src="../image/logo-hino-finance.png" alt="hino-logo" width=40 height=40 /></td>
    </tr>
    <tr>
        <td colspan="3" align="center" style="border-bottom:dashed black 1px;"><b><font size="3">LAPORAN INFORMASI CUSTOMER</font></b></td>
    </tr>
    <tr>
        <td colspan="3">

		<table width="100%" cellpadding="0" cellspacing="0">
            <tr>
				<td class="td02">Nama Customer</td>
				<td class="BS">:</td>
				<td class="td11"><%=DS(0, "cust_name") %></td>
			</tr>
            <tr>
				<td class="td02" valign="top">Alamat</td>
				<td class="BS" valign="top">:</td>
				<td class="td11"><%=DS(0, "alamat") %>
				</td>
			</tr>
            <tr>
				<td class="td02" valign="top">Bidang Usaha</td>
				<td class="BS" valign="top">:</td>
				<td class="td11"><%=DS(0, "bidang_usaha") %>
				</td>
			</tr>
        </table>
   
		</td>
    </tr>
    <tr>
        <td colspan="3" align="center"><b>REPUTASI CUSTOMER</b><br /></td>
    </tr>
    <tr>
        <td colspan="3" align="left">
            - Customer memiliki reputasi yang <%=DS(0, "repdesc_1") %> selama periode bulan ke 1 s/d bulan ke 6  di <%=DS(0, "jmlkreditur_1") %> kreditur <br />
            - Customer memiliki reputasi yang <%=DS(0, "repdesc_2") %> selama periode bulan ke 7 s/d bulan ke 12  di <%=DS(0, "jmlkreditur_2") %> kreditur<br />
            - Customer memiliki reputasi yang <%=DS(0, "repdesc_3") %> selama periode bulan ke 13 s/d bulan ke 24  di <%=DS(0, "jmlkreditur_3") %> kreditur<br /><br />
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center" style="border-bottom:dashed black 1px;">
        
		<table width="40%" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3" align="left">Tabel Reputasi Per Periode</td>
            </tr>
            <tr>
				<td style="background-color:yellow" class="boxboldcenter" align="center">1-6 bulan</td>
				<td style="background-color:yellow" class="boxboldcenter" align="center">7-12 bulan</td>
				<td style="background-color:yellow" class="boxboldcenter" align="center">13-24 bulan</td>
			</tr>
            <tr>
				<td class="boxboldcenter" align="center"><%=DS(0, "repburuk_1") %>/<%=DS(0, "jmlkreditur_1") %></td>
				<td class="boxboldcenter" align="center"><%=DS(0, "repburuk_2") %>/<%=DS(0, "jmlkreditur_2") %></td>
				<td class="boxboldcenter" align="center"><%=DS(0, "repburuk_3") %>/<%=DS(0, "jmlkreditur_3") %></td>
			</tr>
        </table>
        <br />
		</td>
    </tr>
    <tr>
        <td colspan="3" align="center"><b>KAPASITAS CUSTOMER</b><br /></td>
    </tr>
    <tr>
        <td colspan="3" style="border-bottom:dashed black 1px;" align="left">
            <table width="500" cellpadding="0" cellspacing="0">
                <tr>
				    <td class="td01">Eksposur Community Check</td>
				    <td class="BS">:</td>
				    <td class="td12"><%=DS(0, "exps_commcheck", "n0") %>
				    </td>
			    </tr>
                <tr>
				    <td class="td01">Limit Pengajuan Baru</td>
				    <td class="BS">:</td>
				    <td class="td11"><cc1:TXT_CURRENCY style="text-align:right" onkeypress="javascript:return isNumber(event)" ID="limit_baru" runat="server" Width="210px"></cc1:TXT_CURRENCY>
				    </td>
			    </tr>
                <tr>
				    <td class="td01">Limit Hino Group</td>
				    <td class="BS">:</td>
				    <td class="td11"><cc1:TXT_CURRENCY style="text-align:right" onkeypress="javascript:return isNumber(event)" ID="limit_tmt" runat="server" Width="210px"></cc1:TXT_CURRENCY>
				    </td>
			    </tr>
                <tr>
				    <td class="td01" valign="top">Estimasi Total Hutang</td>
				    <td valign="top">:</td>
				    <td class="td12"><%=DS(0, "estm_tot_loan", "n0") %><br /><br />
				    </td>
			    </tr>
                <tr>
				    <td class="td01" valign="top">Limit Hino Finance (saat ini)</td>
				    <td valign="top">:</td>
				    <td class="td11"><cc1:TXT_CURRENCY style="text-align:right" onkeypress="javascript:return isNumber(event)" ID="limit_trakindo" runat="server" Width="210px"></cc1:TXT_CURRENCY><br /><br />
				    </td>
			    </tr>
                <tr>
				    <td class="td01">Estimasi % Total Hutang Group</td>
				    <td class="BS">:</td>
				    <td class="td12"><%=DS(0, "estm_prc_loantmt", "n0") %> %
				    </td>
			    </tr>
                <tr>
				    <td class="td01">Estimasi % Total Hutang Hino Fianance</td>
				    <td class="BS">:</td>
				    <td class="td12"><%=DS(0, "estm_prc_loantrk", "n0") %> %
				    </td>
			    </tr>
            </table>
            <br />
            <center><input runat="server" ID="btnsave1" type="button" class="Bt1" onclick="if (savevalidation()) callback(mainPanel, 'save', true);" value=" Save " /></center>
        </td>
    </tr>
    </table>
    
	</dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    </form>
    <div id="divSupp" runat="server" style="margin-top:-10px;"></div>

    </center>
</body>
</html>
