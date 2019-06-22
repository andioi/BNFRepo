<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DupcheckResult.aspx.cs" Inherits="DebtChecking.Facilities.DupcheckResult" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SID Text Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <table id="Content" class="Box1" width="100%" align="center">
    <tr>
        <td>	
		<table id="DataKredit" width="100%">
			<tr>
				<td class="H1">Hasil Dupcheck</td>
			</tr>
			<tr>
			    <td>
			        <dxwgv:ASPxGridView ID="GridViewKREDIT" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewKREDIT" KeyFieldName="SEARCHID" 
                        OnAfterPerformCallback="GridViewKREDIT_AfterPerformCallback" 
                        OnLoad="GridViewKREDIT_Load" >
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Kriteria" FieldName="SEARCHDESC" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Dupcheck Result" PropertiesTextEdit-EncodeHtml="false" FieldName="DUPCHECKRES" ></dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="50" />
                    </dxwgv:ASPxGridView>
			    </td>
			</tr>
		</table>
		</td>
    </tr>
    </table>
		
    </form>
</body>
</html>
