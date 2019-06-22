<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoringBureau.aspx.cs" Inherits="DebtChecking.Facilities.ScoringBureau" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Scoring Bureau</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <table class="Box1" width="100%">
    <tr>
        <td>
		<table id="DataDebitur" width="100%">
			<tr valign="top">
				<td class="H1" colspan="2">Hasil Scoring Bureau</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">App ID</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="appnumber" runat="server"></asp:Label>
			                </td>
			            </tr>
			            <tr>
			                <td class="B01">Product</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="product" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Create Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="createdate" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">State Start Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="statestartdate" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Bureau Request Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="bureaurequestdate" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">BI Response</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="bi_response" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Total Debt Burden</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="total_debt_burden" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Monthly Income From Bureau</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="monthlyincomefrombureau" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Has CC Facility</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="hasccfacility" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">MSC code</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="msccode" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">MSC description</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="mscsegment" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Total Unsecured Exposure</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="total_uns_exp" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Scoring Decision Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoringdecdate" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Scoring Decision Time</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoringdectime" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Reject Score</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="reject_score" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Score Band</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="scoreband" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Limit Assigned</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="limitassigned" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">mue</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="mue" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">tue</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="tue" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">dbr</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="dbr" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Final Decision</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="finaldecision" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">reject reason</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="rejectreason" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			</tr>
		</table>	
		</td>
    </tr>
    </table>
		
    </form>
</body>
</html>
