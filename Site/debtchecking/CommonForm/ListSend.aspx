<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSend.aspx.cs" Inherits="DebtChecking.CommonForm.ListSend" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register src="UC_ListFilter.ascx" tagname="UC_ListFilter" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ListSend Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <script src="../include/cek_mandatoryOnly.js" language="javascript" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <dxcp:ASPxCallbackPanel ID="listPanel" ClientInstanceName="listPanel" runat="server" Width="100%">
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
        <uc1:UC_ListFilter ID="UC_ListFilter1" runat="server" />
        <table class="Box1" width="98%">
            <tr class="H1" >
                <td><b><asp:Label ID="TitleHeader" runat="server"></asp:Label></b></td>
            </tr>
            <tr>
                <td style="vertical-align:top"> 
                    <dxwgv:ASPxGridView ID="grid" runat="server" ClientInstanceName ="grid"
                        AutoGenerateColumns="False" Width="100%" Font-Size="11px" onload="grid_Load" 
                        OnHtmlRowPrepared="grid_HtmlRowPrepared" 
                        OnAfterPerformCallback="grid_AfterPerformCallback" >
                        <settingsbehavior autofilterrowinputdelay="-1" />
                        <Columns>
                        <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="1px" ShowClearFilterButton="false" >                          
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <a href="javascript:grid.SelectRows();">Select All</a>&nbsp;<br />
                                            <a href="javascript:grid.UnselectRows();">Unselect All</a>&nbsp;
                                        </td>
                                    </tr>
                                </table>                          
                            </HeaderTemplate>
                        </dxwgv:GridViewCommandColumn>
                        </Columns>
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td class="F2">
                    <table class="Tbl02">
                        <tr>
                            <td>Assign to:</td>
                            <td><asp:DropDownList ID="ddl_Officer" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                            <td><asp:Button class="Bt1" ID="btnSave" runat="server" Text="Assign" OnClick="btnSave_Click" OnClientClick="if (!confirm('Are you sure??')) return false;" /></td>
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
