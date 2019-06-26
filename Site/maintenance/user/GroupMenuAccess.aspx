<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupMenuAccess.aspx.cs" Inherits="MikroMnt.user.GroupMenuAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>GroupMenuAccess</title>
		<link href="../include/style.css" type="text/css" rel="stylesheet" />
		<!-- #include file="../include/cek_mandatoryOnly.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
			<table class="Box1">
				<tr>
					<td valign="top" align="center">
						<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
					</td>
				</tr>
				<tr>
					<td valign="top">
						<asp:Table id="TBL_MENU" runat="server" Width="475px" CellPadding="0"></asp:Table>
					</td>
				</tr>
				<tr class="F1">
					<td>
						<asp:button id="BTN_SAVE" runat="server" Text="Save" CssClass="Bt1" Width="70px" onclick="BTN_SAVE_Click"></asp:button>
					</td>
				</tr>
			</table>
			
    </div>
    </form>
</body>
</html>
