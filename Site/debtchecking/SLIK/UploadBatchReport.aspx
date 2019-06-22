<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadBatchReport.aspx.cs" Inherits="DebtChecking.SLIK.UploadBatchReport" %>

<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web" TagPrefix="dxp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- Animate.css -->
    <link href="../vendors/animate.css/animate.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dxp:ASPxCallbackPanel ID="mainPanel" ClientInstanceName="mainPanel" runat="server" Width="100%" OnCallback="mainPanel_Callback">
                <ClientSideEvents EndCallback="function(s, e) {
                                                if(s.hasOwnProperty('cp_export') && s.cp_export!='')
	                                            {
		                                            window.open(s.cp_export);
		                                            s.cp_export = '';} }" />
                <PanelCollection>
                    <dxp:PanelContent runat="server">

                        <table class="Box1" width="100%">
                            <tr class="H1">
                                <td>
                                    <b>
                                        <asp:Label ID="TitleHeader" runat="server"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <dxp:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" CssClass="table"
                                        AutoGenerateColumns="true" Width="100%" OnLoad="grid_Load" SettingsPager-PageSize="50">
                                    </dxp:ASPxGridView>
                                </td>
                            </tr>
                            <tr runat="server" id="btnExporter" visible="true">
                                <td>
                                    <input id="Button1" runat="server" class="btn btn-xs btn-danger" onclick="callback(mainPanel, 'e:', false);" type="button" value="Export to Excel" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center">
                                    <input id="btnSubmit" runat="server" class="btn btn-xs btn-danger" onclick="if (confirm('Yakin akan submit batch request ini ?')) callback(mainPanel, 's:', false); return false;" type="button" value="Submit" />&nbsp;
                                    <input id="btnCancel" runat="server" class="btn btn-xs btn-danger" onclick="if (confirm('Batalkan batch request ini?')) callback(mainPanel, 'c:', false); return false;" type="button" value="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </dxp:PanelContent>
                </PanelCollection>
            </dxp:ASPxCallbackPanel>
        </div>
    </form>
</body>
</html>