<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPivot.aspx.cs" Inherits="DebtChecking.Report.ReportPivot" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v17.1" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dxwpg" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>


<%@ Register src="UC_ReportFilter.ascx" tagname="UC_ReportFilter" tagprefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%">
        <tr>
            <td class="H0" align="right">
                <a href="<% =BackURL %>"><img src="../image/back.jpg" alt="back" /></a>
                <a href="../Body.aspx"><img src="../image/MainMenu.JPG" alt="mainmenu" /></a>
                <a href="../Logout.aspx" target="_top"><img src="../image/logout.jpg" alt="logout" /></a>
            </td>
        </tr>
    </table>
    
    <dxcp:ASPxCallbackPanel ID="mainPanel" ClientInstanceName="mainPanel" 
            runat="server" Width="100%">     
    <PanelCollection>
    <dxp:PanelContent runat="server">
        <table width="100%">    
        <tr><td>
        <uc1:UC_ReportFilter ID="UC_ReportFilter1" runat="server" />
        </td></tr>
        </table>
        
        <table class="Box1" width="98%">    
        <tr class="H1" ><td>
           <b>
            <asp:Label ID="TitleHeader" runat="server"></asp:Label>
           </b>           
        </td></tr>
        <tr>
             <td>                 
                 <input type="button" class="Bt1" value="Export to PDF" onclick="grid.PerformCallback('e:pdf')" />&nbsp;
                 <input type="button" class="Bt1" value="Export to XLS" onclick="grid.PerformCallback('e:xls')" />&nbsp;
                 <input type="button" class="Bt1" value="Export to RTF" onclick="grid.PerformCallback('e:rtf')" />&nbsp;
                 <input type="button" class="Bt1" value="Export to TXT" onclick="grid.PerformCallback('e:txt')" />&nbsp;
                 
             </td>
         </tr>
        <tr><td>
              <dxwpg:ASPxPivotGrid ID="grid" runat="server" ClientInstanceName="grid" 
                  CssClass="" OnLoad="grid_Load" OnCustomCallback="grid_CustomCallback">
                  <ClientSideEvents AfterCallback="function(s, e) {
    if(s.hasOwnProperty('cp_export') && s.cp_export!='')
	{
		window.open(s.cp_export);
		s.cp_export = '';
	}
}" />
              </dxwpg:ASPxPivotGrid>
             &nbsp;
        </td></tr>
        </table>
        <dxwpg:ASPxPivotGridExporter ID="gridExport" runat="server" ASPxPivotGridID="grid">
        </dxwpg:ASPxPivotGridExporter>
    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>    
    </div>
    </form>
</body>
</html>
