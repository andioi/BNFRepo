<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_GenExcel.ascx.cs" Inherits="DebtChecking.CommonForm.UC_GenExcel" %>
<table class="Tbl0" id="tbm" runat="server">
    <tr>
	    <td class="H1" colspan="2">
	        <asp:Label ID="lbT" runat="server" Text="Generate Excel Sheet"></asp:Label>
	    </td>
    </tr>
	<tr valign="top">
		<td width="30%">
			<table class="Tbl0">
				<tr>
					<td class="B03">Template Surat</td>
					<td class="BS">:</td>
					<td class="B11">
					    <asp:DropDownList ID="ddltpl" runat="server"></asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td colspan="3" class="F1">
                        <input type="button" id="btngen" runat="server" class="Bt1" value="Generate" onclick="callback(panelFile,'generate',false);" ></input>
					</td>
				</tr>
			</table>
		</td>
		<td width="70%">
			<table class="Tbl0">
				<tr>
					<td>
                        <dxcp:ASPxCallbackPanel ID="panelFile" ClientInstanceName="panelFile" 
                                runat="server" Width="100%" oncallback="panelFile_Callback">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent3" runat="server">
		                        <table class="Tbl0">
                                    <tr>
                                        <td>
                                            <input type="hidden" id="h_tpl" runat="server" />
                                            <asp:GridView ID="gridfile" runat="server" AutoGenerateColumns="false" 
                                                width="100%" CssClass="Dg1" OnRowDataBound="gridfile_RowDataBound" >
                                                <HeaderStyle CssClass="H1" />
                                                <AlternatingRowStyle CssClass="Alt1" />
                                                <RowStyle HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:BoundField DataField="SEQ" HeaderText="No" />
                                                    <asp:BoundField DataField="FILENAME" HeaderText="Nama File" >
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DATE" HeaderText="Tanggal" DataFormatString="{0:dd-MMM-yyyy}" />
                                                    <asp:TemplateField HeaderText="Function">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="LNK_DOWN" runat="server">download</asp:HyperLink>
                                                            <asp:HyperLink ID="LNK_DEL" runat="server">delete</asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>