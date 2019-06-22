<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ListDetail.ascx.cs" Inherits="DebtChecking.List.UC_ListDetail" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register src="UC_ListFilter.ascx" tagname="UC_ListFilter" tagprefix="uc1" %>

    <dx:ASPxCallbackPanel ID="listPanel" ClientInstanceName="listPanel" runat="server" Width="100%">
    <PanelCollection>
    <dx:PanelContent ID="PanelContent1" runat="server">
        <uc1:UC_ListFilter ID="UC_ListFilter1" runat="server" />
        <table class="Box1" width="98%">
        <tr class="H1" ><td>
           <b>
            <asp:Label ID="TitleHeader" runat="server"></asp:Label>
           </b>
       </td></tr>
        <tr><td style="vertical-align:top"> 
            <dx:ASPxGridView ID="grid" runat="server" ClientInstanceName ="grid"
                AutoGenerateColumns="False" Width="100%" Font-Size="11px" onload="grid_Load" 
                OnHtmlRowPrepared="grid_HtmlRowPrepared" 
                OnAfterPerformCallback="grid_AfterPerformCallback" >
            <ClientSideEvents 
                EndCallback="function(s, e) {
                try
                {
                    window.parent.resizeFrame();
                }
                catch(e)
                {
                };
                }" />
                <settingsbehavior autofilterrowinputdelay="-1" />
            <Columns>
                     <dx:GridViewCommandColumn Visible="false" VisibleIndex="0" Width="1px" ShowClearFilterButton="true">                        
                    </dx:GridViewCommandColumn>
                    <dx:GridViewCommandColumn Visible="false" ShowSelectCheckbox="True" VisibleIndex="0" Width="1px" ShowClearFilterButton="true">                        
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
                    </dx:GridViewCommandColumn>
            </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>
        </td></tr>
        <tr>
        <td class="F2">
            <asp:Button class="Bt1" ID="btnSave" runat="server" Text="Process" Visible="false" OnClick="btnSave_Click" />
            <asp:Button class="Bt1" ID="btnExport" runat="server" Text="Export" Visible="false" OnClick="btnExport_Click" />
        </td>
        </tr>
        </table>
    </dx:PanelContent>
    </PanelCollection>
    </dx:ASPxCallbackPanel>
