<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InqStatus.aspx.cs" Inherits="DebtChecking.Facilities.InqStatus" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxe" %>


<%@ Register TagPrefix="uc1" TagName="UC_GeneralInfo" Src="../CommonForm/UC_GeneralInfo.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Inquiry by Status Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%">
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
    
    <table width="100%" class="Box1">   
        <tr>
            <td>
                <uc1:UC_GeneralInfo ID="gi" runat="server" />
            </td>
        </tr>
        <tr class="H1">
            <td>Track History</td>
        </tr>
        <tr>
            <td>
                <dxwgv:ASPxGridView ID="gridTr" runat="server" ClientInstanceName="gridTr" 
                    AutoGenerateColumns="False" Width="100%" KeyFieldName="AP_REGNO" 
                    OnLoad="gridTr_Load">    
                    <Styles>
                        <Header HorizontalAlign="Center"></Header>
                        <Cell HorizontalAlign="Center"></Cell>
                    </Styles>
                    <SettingsBehavior AllowSort="False" />
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="Track Name" FieldName="TR_DESC" >
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Track Date" FieldName="AP_LASTTRDATE" >
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Track By" FieldName="AP_NEXTTRBY" >
                        </dxwgv:GridViewDataTextColumn>
                      </Columns>
                </dxwgv:ASPxGridView>   
                <dxwgv:ASPxGridView ID="gridTrHist" runat="server" ClientInstanceName="gridTrHist" 
                    AutoGenerateColumns="False" Width="100%" KeyFieldName="TH_SEQ" 
                    OnLoad="gridTrHist_Load">
                    <Styles>
                        <Header HorizontalAlign="Center"></Header>
                        <Cell HorizontalAlign="Center"></Cell>
                    </Styles>
                    <SettingsBehavior AllowSort="False" />
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="start date" FieldName="LASTTR_DATE">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="end date" FieldName="TR_DATE">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Track Name" FieldName="TR_DESC" >
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Track By" FieldName="TR_BY" >
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Aging" FieldName="AGING" >
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
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
