<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLIKResult.aspx.cs" Inherits="DebtChecking.Facilities.SLIKResult" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SID Text</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript" src="../include/onepost.js"></script>
    <script language="javascript" type="text/javascript">
        function kliklink(linkid, url) {
            linkdesc = document.getElementById("mainPanel_"+linkid).innerHTML;
            document.getElementById("mainPanel_" +linkid).innerHTML = "<b>" + linkdesc + "</b>";
            document.getElementById("mainPanel_IFR_TEXT").src = url;
            document.getElementById("mainPanel_pdfPanel_urlframe").value = url;
            if (url.indexOf('PDF') > 0 || url.indexOf('notyetuploaded') >= 0) document.getElementById("mainPanel_btnpdf").style.display = 'none';
            else document.getElementById("mainPanel_btnpdf").style.display = '';
        }

        function ceknik(nik, obj)
        {
            //alert(nik);
            v = null; v2 = null;
            if (obj.checked) {
                v = "1"; v2 = "0";
            } else {
                v = "0"; v2 = "1";
            }
            hd = document.getElementById("mainPanel_listvaluestring");
            hd.value = hd.value.replace(nik + ":" + v2, nik + ":" + v);
			//alert(hd.value);
        }
    </script>
    
</head>
<body onload="callback(mainPanel, 'load', false, null);">
    <form id="form1" runat="server">
    <div>Select other customer in this requestid 
        <asp:DropDownList ID="ddl_appid" runat="server" onchange="callback(mainPanel, 'r:'+this.value, false, null);">
        </asp:DropDownList>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
    oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
        <ClientSideEvents EndCallback="function(s, e) {
	if(s.hasOwnProperty('cp_export') && s.cp_export!='')
	{
	    window.open(s.cp_export);
	    /*mydoc = window.open();
        mydoc.document.write(s.cp_export);
        mydoc.document.execCommand('saveAs',true,s.cp_filename+'.txt');
        mydoc.close();
		s.cp_export = '';*/
		return false;
	}
}" />
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="DataDebitur" class="Box1" width="100%">
			<tr style="display:none">
				<td class="H1" colspan="2">Customer Data</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Customer Name</td>
			                <td class="BS">:</td>
			                <td class="B11">
                                <asp:Label ID="cust_name" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                <input id="urlframe2" type="hidden" />
                                <asp:Label ID="status_app" runat="server" style="display:none"></asp:Label>
                                <a href="javascript:PopupPage('DetailCustomer.aspx?regno='+document.getElementById('mainPanel_appid').value,640,400)">[detail]</a>
			                </td>
			            </tr>
			            <tr>
			                <td class="B01">Place / Date of Birth</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="pob_dob" runat="server"></asp:Label></td>
			            </tr>
                        <tr style="display:none">
			                <td class="B01">NPWP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="npwp" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Gender</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="genderdesc" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table width="100%">
						<tr>
			                <td class="B01">KTP / NIK</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ktp" runat="server"></asp:Label></td>
			            </tr>
						<tr>
			                <td class="B01">KTP Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_ktpaddress" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Policy Result</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label Font-Bold="true" Font-Size="Small" ForeColor="Red" ID="final_policy" runat="server"></asp:Label></td>
			            </tr>
                        <tr style="display:none">
			                <td class="B01">Mother Name</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="mother_name" runat="server"></asp:Label></td>
			            </tr>
			            <tr style="display:none">
			                <td class="B01">Home Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_homeaddress" runat="server"></asp:Label></td>
			            </tr>
			            <tr style="display:none">
			                <td class="B01">Emergency Name / Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_econaddress" runat="server"></asp:Label></td>
			            </tr>
			            <tr style="display:none">
			                <td class="B01">Office Name / Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_officeaddress" runat="server"></asp:Label>
			                </td>
			            </tr>
                    </table>
			    </td>
			</tr>
		</table>
		

    <table width="100%" class="Box1">
		<tr style="display:none">
			<td class="H1" colspan="2">CHECKING RESULT</td>
		</tr>
		<tr>
			<td align="center" colspan="2">
                <input type="button" visible="false" value="Preview" id="btnprint" runat="server" name="btnprint" onclick="javascript:PopupPage(document.getElementById('IFR_TEXT').contentWindow.document.location+'&preview=1',800,600,'yes');" />
                <input type="button" id="btnpdf" class="btn btn-xs btn-success" value="Save As PDF" runat="server" onclick="callback(pdfPanel,document.getElementById('mainPanel_pdfPanel_urlframe').value,false,null)" />
                <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
                oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
                <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
                    <input type="hidden" id="urlframe" runat="server" />
                </dxp:PanelContent></PanelCollection>
                </dxcp:ASPxCallbackPanel>
			</td>
		</tr>
		<tr id="TR_FRAME" runat="server">
			<td colspan="2">
				<table width="100%">
					<tr>
						<td valign="top" width="210">
                            <input type="hidden" runat="server" id="appid" />
                            <input id="reffnumber" runat="server" type="hidden" />
                            <input id="nikcount" runat="server" type="hidden" />
                            <input id="listvaluestring" runat="server" type="hidden" />
                            <div id="dv_found" runat="server"><u><strong>FOUND</strong></u><br />
						    <asp:table id="TB_SIDLIST" Runat="server" Width="100%" CellSpacing="0" CellPadding="2"></asp:table>
                                <div style="margin-top:10px"><input runat="server" ID="Button1" class="btn btn-xs btn-success" type="button" onclick="callback(mainPanel, 's:' + document.getElementById('mainPanel_listvaluestring').value, false, null);" value="Save & Recalculate" /></div>
                                <div style="margin-top:6px"><input runat="server" ID="Button2" class="btn btn-xs btn-success" type="button" onclick="callback(mainPanel, 'd:', false, null);" value="Download PDF&TXT " /></div>
                            </div>
                            <div id="dv_nihil" runat="server" style="margin-top:20px;"><u><strong>NOT FOUND</strong></u></div>
                            <asp:table id="TB_NIHIL" Runat="server" Width="100%" CellSpacing="0" CellPadding="2"></asp:table>
						</td>
						<td style="BORDER-RIGHT: #33ffff groove; BORDER-TOP: #33ffff groove; BORDER-LEFT: #33ffff groove; 
						        BORDER-BOTTOM: #33ffff groove" valign="top" bordercolor="#33ffff" align="center">
							<iframe ID="IFR_TEXT" name="IFR_TEXT" frameborder="no" width="100%" height="600" runat="server"></iframe>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="TR_MSG" runat="server">
			<td align="center" colspan="2"><asp:label id="LBL_MSG" Font-Bold="true" runat="server" ForeColor="Red" Text="DATA ONPROCESS"></asp:label></td>
		</tr>
    </table>
        </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    </div>
    </form>
</body>
</html>
