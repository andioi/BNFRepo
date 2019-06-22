<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_UploadFile.ascx.cs" Inherits="DebtChecking.CommonForm.UC_UploadFile" %>
<table class="Tbl0" id="tbm" runat="server">
    <tr>
	    <td class="H1" colspan="2"><asp:Label ID="ttl" runat="server">File Upload</asp:Label></td>
    </tr>
	<tr valign="top">
		<td width="40%">
			<table class="Tbl0">
				<tr>
					<td class="B03">Select File</td>
					<td class="BS">:</td>
					<td class="B11">
                        <dxuc:ASPxUploadControl ID="upfile" runat="server" Font-Size="X-Small" Width="100%"
                             OnFileUploadComplete="upfile_FileUploadComplete" >
                        </dxuc:ASPxUploadControl> 
                        <asp:Label ID="lblEr" runat="server"></asp:Label>
                        <!-- <label id="LBL_MAXDOCFILESIZE" runat="server">(maximum file size: 200kB)</label> -->
					</td>
				</tr>
				<tr>
					<td colspan="3" class="F1">
                        <input type="button" id="btnup" runat="server" class="Bt1" value="Upload" ></input>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>