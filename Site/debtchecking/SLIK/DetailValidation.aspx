<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetailValidation.aspx.cs" Inherits="DebtChecking.Facilities.DetailValidation" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Detail Validation</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <script language="javascript" type="text/javascript" src="../include/onepost.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table id="DataDebitur" class="Box1" width="100%">
			<tr>
				<td class="H1" colspan="2">VALIDATION RESULT DETAIL</td>
			</tr>
			<tr valign="top">
			    <td>
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">NIK</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "result_nik") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Total Matching Score</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "total_score") %></td>
			            </tr>
			        </table>
                    <table class="Tbl1" width="100%">
                        <tr>
			                <td class="B01" style="font-weight:bold" >FIELD</td>
			                <td class="B01" style="font-weight:bold" >MATCH/NOT MATCH</td>
			                <td class="B01" style="font-weight:bold" >APPLICATION DATA</td>
                            <td class="B01" style="font-weight:bold" >RESULT SLIK</td>
                            <td class="B01" style="font-weight:bold" >SCORE</td>
			            </tr>
			            <tr>
			                <td class="B01">NAME</td>
			                <td class="B11"><%=DS(0, "name_match") %></td>
			                <td class="B11"><%=DS(0, "app_name") %></td>
                            <td class="B11"><%=DS(0, "result_name") %></td>
                            <td class="B11"><%=DS(0, "name_score") %></td>
			            </tr>
                        <tr>
			                <td class="B01">DATE OF BIRTH</td>
			                <td class="B11"><%=DS(0, "dob_match") %></td>
			                <td class="B11"><%=DS(0, "app_dob") %></td>
                            <td class="B11"><%=DS(0, "result_dob") %></td>
                            <td class="B11"><%=DS(0, "dob_score") %></td>
			            </tr>
                        <tr>
			                <td class="B01">ID NUMBER / KTP</td>
			                <td class="B11"><%=DS(0, "nik_match") %></td>
			                <td class="B11"><%=DS(0, "app_nik") %></td>
                            <td class="B11"><%=DS(0, "result_nik") %></td>
                            <td class="B11"><%=DS(0, "nik_score") %></td>
			            </tr>
                        <tr>
			                <td class="B01">MOTHER MEIDEN NAME</td>
			                <td class="B11"><%=DS(0, "mmn_match") %></td>
			                <td class="B11"><%=DS(0, "app_mmn") %></td>
                            <td class="B11"><%=DS(0, "result_mmn") %></td>
                            <td class="B11"><%=DS(0, "mmn_score") %></td>
			            </tr>
                        <tr>
			                <td class="B01">PLACE OF BIRTH</td>
			                <td class="B11"><%=DS(0, "pob_match") %></td>
			                <td class="B11"><%=DS(0, "app_pob") %></td>
                            <td class="B11"><%=DS(0, "result_pob") %></td>
                            <td class="B11"><%=DS(0, "pob_score") %></td>
			            </tr>
                        <tr>
			                <td class="B01">NPWP</td>
			                <td class="B11"><%=DS(0, "npwp_match") %></td>
			                <td class="B11"><%=DS(0, "app_npwp") %></td>
                            <td class="B11"><%=DS(0, "result_npwp") %></td>
                            <td class="B11"><%=DS(0, "npwp_score") %></td>
			            </tr>
                        <tr>
			                <td class="B01">ADDRESS</td>
			                <td class="B11"><%=DS(0, "addr_match") %></td>
			                <td class="B11" valign="top">
                                <u>ID ADDRESS</u>:<br />
                                <%=DS(0, "ktp_addr") %><br /><br />
                                <u>HOME ADDRESS</u>:<br />
                                <%=DS(0, "home_addr") %><br /><br />
                                <u>OFFICE ADDRESS</u>:<br />
                                <%=DS(0, "office_addr") %><br />
			                </td>
                            <td class="B11"><%=DS(0, "result_addr") %></td>
                            <td class="B11"><%=DS(0, "addr_score") %></td>
			            </tr>
			        </table>
			    </td>
			</tr>
		</table>
		
            
    </div>
    </form>
</body>
</html>
