<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefCh.aspx.cs" Inherits="MikroMnt.CommonForm.RefCh" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>RefCh</title>
		<LINK href="../include/style.css" type="text/css" rel="stylesheet">
		<script src="../include/reflist.js" language="javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
				<asp:textbox id="CODE" runat="server" EnableViewState=False AutoPostBack="True" Width="40px" ontextchanged="CODE_TextChanged"></asp:textbox>
				<INPUT id="BTN" type="button" value="cari" runat="server" tabIndex="-1" NAME="BTN">
				<asp:textbox id="DESC" runat="server" Width="100%" ReadOnly="True" EnableViewState="False" BorderWidth="0px"
					tabIndex="-1"></asp:textbox>
    </div>
    </form>
</body>
</html>
