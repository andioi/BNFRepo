<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailCustomer.aspx.cs" Inherits="DebtChecking.Facilities.DetailCustomer" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Detail Customer</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <script language="javascript" type="text/javascript" src="../include/onepost.js"></script>
    <script language="javascript" type="text/javascript">
        function kliklink(linkid, url) {
            linkdesc = document.getElementById(linkid).innerHTML;
            document.getElementById(linkid).innerHTML = "<b>" + linkdesc + "</b>";
            document.getElementById("IFR_TEXT").src = url;
            document.getElementById("pdfPanel_urlframe").value = url;
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" ClientInstanceName="mainPanel">
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="DataDebitur" class="Box1" width="100%">
			<tr>
				<td class="H1" colspan="2">CUSTOMER DETAIL</td>
			</tr>
			<tr valign="top">
			    <td colspan="2">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Customer Name</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="cust_name" Font-Bold="true" Font-Size="Small" runat="server"></asp:Label>
                                <input type="hidden" runat="server" id="appid" />
                                <input id="reffnumber" runat="server" type="hidden" />
                                <asp:Label ID="status_app" runat="server" Visible="false"></asp:Label>
			                </td>
			            </tr>
			            <tr>
			                <td class="B01">Place / Date of Birth</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="pob_dob" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">KTP / NIK</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ktp" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Gender</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="genderdesc" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">NPWP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="npwp" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Mother Name</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="mother_name" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">KTP Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_ktpaddress" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Home Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_homeaddress" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Emergency Name / Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_econaddress" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Office Name / Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_officeaddress" runat="server"></asp:Label>
			                </td>
			            </tr>
			        </table>
			    </td>
			</tr>
            <tr style="display:none">
				<td class="H1" colspan="2">SCORING RESULT</td>
			</tr>
			<tr valign="top" style="display:none">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Card Utilization</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoring_totalutilization" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Ratio Over Half Utilization</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoring_ratiooverhalf" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Ratio Card Under Six Month</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoring_ratioundersixmonth" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Number Of Month Oldest Fac Opened</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoring_numofmontholdestopened" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Checking Status</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoring_checkingstatus" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
                <td width="50%">
                    <table class="Tbl0" width="100%">
                        <tr>
			                <td class="B01">Remarks</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoring_remarks" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Downloaded</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="downloaded" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Download Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="downloaddate" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Download By</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="downloadby" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Batch Scoring File</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="batchid" runat="server"></asp:Label>
			                </td>
			            </tr>
                    </table>
                </td>
			</tr>
		</table>
		</dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
            
    </div>
    </form>
</body>
</html>
