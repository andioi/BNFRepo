<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="USC_paraminput.ascx.cs" Inherits="MikroMnt.Parameter.USC_paraminput" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxe" %>

<table  class="Box1" width="100%" >
<tr>
    <td align="center">
    <pre dir="ltr" style="
		margin: 0px;
		padding: 2px;
		border: 1px inset;
		width: 700px;
		height: 400px;
		text-align: left;
		overflow: auto">
        <asp:Table ID="TableInput" runat="server" Width="100%">
        </asp:Table>
    </pre>
    </td>
</tr>
<tr>
    <td runat="server" id="td_filter" align="center" >
        <input id="Button1" runat="server" class="btn btn-xs btn-success" type="button" value=" Save " onclick="callback(panel,'s:', true, null)" />
        <input id="clrButton" runat="server" class="btn btn-xs btn-success" type="button" value=" Clear " style="display:none"/>
        <input id="Button2" runat="server" class="btn btn-xs btn-success" type="button" value=" Cancel " onclick="popup.Hide();" />
        
    </td>
</tr>
</table>
