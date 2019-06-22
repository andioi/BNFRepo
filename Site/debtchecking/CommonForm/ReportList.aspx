<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportList.aspx.cs" Inherits="DebtChecking.Report.ReportList" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>


<%@ Register src="UC_ReportFilter.ascx" tagname="UC_ReportFilter" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->   
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div style="position:fixed;top:0;right:0;margin-right:20px;margin-top:5px;">
        <!-- Row start -->
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row rightinfo">
                    <a href="<%=BackURL %>" class="btn btn-warning"> <span class="glyphicon glyphicon-chevron-left"></span> Back </a> 
                    <a href="../main.html" class="btn btn-warning" ><span class="glyphicon glyphicon-home"></span> Mainmenu </a> 
                    <a href="../Logout.aspx" class="btn btn-warning"><span class="glyphicon glyphicon-log-out"></span> Logout </a>

                </div>
            </div>
        </div>
        <!-- Row end -->
    </div>
    <div style="margin-top:5px;">
    <dxcp:ASPxCallbackPanel ID="mainPanel" ClientInstanceName="mainPanel" 
            runat="server" Width="100%">     
    <PanelCollection>
    <dxp:PanelContent runat="server">
        <uc1:UC_ReportFilter ID="UC_ReportFilter1" runat="server" />

        <table class="Box1" width="100%">    
        <tr class="H1" ><td>
           <b>
            <asp:Label ID="TitleHeader" runat="server"></asp:Label>
           </b>
       </td></tr>
       <tr><td>
                 <input type="button" class="btn btn-xs btn-success" value="Export to PDF" onclick="grid.PerformCallback('e:pdf')" />&nbsp;
                 <input type="button" class="btn btn-xs btn-success" value="Export to XLS" onclick="grid.PerformCallback('e:xls')" />&nbsp;
                 <input type="button" class="btn btn-xs btn-success" value="Export to RTF" onclick="grid.PerformCallback('e:rtf')" />&nbsp;
                 <input type="button" class="btn btn-xs btn-success" value="Export to CSV" onclick="grid.PerformCallback('e:csv')" />&nbsp;
       </td></tr>
       <tr><td> 
            <dxwgv:ASPxGridView ID="grid" ClientInstanceName ="grid" runat="server" 
            AutoGenerateColumns="False" Width="100%" onload="grid_Load" 
                OnCustomCallback="grid_CustomCallback">
            <settingsbehavior autofilterrowinputdelay="-1" />
                <ClientSideEvents EndCallback="function(s, e) {
    if(s.hasOwnProperty('cp_export') && s.cp_export!='')
	{
		window.open(s.cp_export);
		s.cp_export = '';
	}
}" />
            </dxwgv:ASPxGridView>
             <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dxwgv:ASPxGridViewExporter>

        </td></tr>
        </table>
    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>    
    </div>
    </form>
</body>
</html>
