<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_UploadedFile.ascx.cs" Inherits="DebtChecking.CommonForm.UC_UploadedFile" %>
<table class="Tbl0" id="tbm" runat="server">
    <tr>
	    <td class="H1" colspan="2"><asp:Label ID="ttl" runat="server">File Upload</asp:Label></td>
    </tr>
	<tr valign="top">
		<td width="40%" style="display:none">
			<table class="Tbl0">
				<tr>
					<td class="B03">File</td>
					<td class="BS">:</td>
					<td class="B11">
                        <dxuc:ASPxUploadControl ID="upfile" runat="server" Font-Size="X-Small" Width="100%"
                             OnFileUploadComplete="upfile_FileUploadComplete" >
                            <ValidationSettings 
                               MaxFileSize="2048000" 
                               MaxFileSizeErrorText="File size must not exceed 200kB!">
                            </ValidationSettings>
                        </dxuc:ASPxUploadControl> 
                        <asp:Label ID="lblEr" runat="server"></asp:Label>
                        <label id="LBL_MAXDOCFILESIZE" runat="server">(maximum file size: 200kB)</label>
					</td>
				</tr>
				<tr>
					<td colspan="3" class="F1">
                        <input type="button" id="btnup" runat="server" class="Bt1" value="Upload" ></input>
					</td>
				</tr>
			</table>
		</td>
		<td width="100%">
			<table class="Tbl0">
				<tr>
					<td>
                        <dxcp:ASPxCallbackPanel ID="panelFile" runat="server" Width="100%" 
                            oncallback="panelFile_Callback">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent3" runat="server">
		                        <table class="Tbl0">
                                    <tr>
                                        <td>
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