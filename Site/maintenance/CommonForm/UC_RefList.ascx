<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_RefList.ascx.cs" Inherits="MikroMnt.CommonForm.UC_RefList" %>
<asp:dropdownlist id="DDL" runat="server" onselectedindexchanged="DDL_REF_SelectedIndexChanged"></asp:dropdownlist>
<div id="d1" runat="server">
	<input type="hidden" id="cd" runat="server" NAME="cd"> <input type="hidden" id="de" runat="server" NAME="de">
	<iframe id="fr" frameBorder="no" width="100%" scrolling="no" height="35" runat="server"
		tabindex="-1"></iframe>
</div>
<div id="d2" runat="server">
	<asp:textbox id="CODE" runat="server" AutoPostBack="True" Width="50px" ontextchanged="CODE_TextChanged"></asp:textbox>
	<INPUT id="BTN" class="Bt1" type="button" value="Cari" runat="server" tabIndex="-1" NAME="BTN">
	<asp:textbox id="DESC" runat="server" Width="100%" ReadOnly="True" BorderWidth="0px"
		tabIndex="-1"></asp:textbox>
</div>
