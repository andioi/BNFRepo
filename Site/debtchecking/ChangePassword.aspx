<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="DebtChecking.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Change Password</title>
		<link href="include/style.css" type="text/css" rel="stylesheet" />
		<script language="javascript" type="text/javascript">
            function kutip_satu()
            {
	            if ((event.keyCode == 35) || (event.keyCode == 39))
	            {
		            return false;
	            } else
	            {
		            return true;
	            }	
            }
		</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<table border="0" width="100%">
			<tr>
				<td class="H0" width="421">
					<table>
						<tr>
							<td class="H1" style="WIDTH: 400px" align="center"><b>Change Password</b></td>
						</tr>
					</table>
				</td>
				<td class="H0" align="right">
				    <a href="Body.aspx"><img id="IMG_BACK" src="Image/MainMenu.jpg" alt="back" /></a>
					<a href="Logout.aspx"><img src="Image/Logout.jpg" alt="logout" /></a>
				</td>
			</tr>
			<tr>
				<td colspan="2"></td>
			</tr>
			<tr>
				<td style="HEIGHT: 200px" align="center" colspan="2">
					<table cellspacing="1" cellpadding="1" border="1">
						<tr>
							<td class="H1">Change Password</td>
						</tr>
						<tr>
							<td valign="top" align="center">
								<table cellspacing="0" cellpadding="0" width="400" border="0">
									<tr>
										<td class="B01">Old Password</td>
										<td width="17"></td>
										<td class="B11">
										    &nbsp;<asp:textbox onkeypress="return kutip_satu()" id="TXT_OLD" runat="server" Width="168px" TextMode="Password"></asp:textbox>
										</td>
									</tr>
									<tr>
										<td class="B01">New Password</td>
										<td></td>
										<td class="B11">
										    &nbsp;<asp:textbox onkeypress="return kutip_satu()" id="TXT_NEW" runat="server" Width="168px" TextMode="Password"></asp:textbox>
										</td>
									</tr>
									<tr>
										<td class="B01">Verify Password</td>
										<td></td>
										<td class="B11">
										    &nbsp;<asp:textbox onkeypress="return kutip_satu()" id="TXT_VERIFY" runat="server" Width="168px" TextMode="Password"></asp:textbox>
										</td>
									</tr>
									<tr>
										<td valign="top" align="center" colspan="3"></td>
									</tr>
									<tr>
										<td valign="top" align="center" colspan="3">
										    <asp:label id="LBL_MESSAGE" runat="server" ForeColor="Red"></asp:label>
										    &nbsp;
											<asp:label id="LBL_SC_ID" runat="server" Visible="False"></asp:label>
										</td>
									</tr>
								</table>
								<asp:button id="BTN_CHANGE" runat="server" Width="85px" Text="Change" CssClass="Bt1" onclick="BTN_CHANGE_Click"></asp:button>
								&nbsp;&nbsp;
								<input class="Bt1" style="WIDTH: 85px" onclick="javascript:form1.IMG_BACK.click()" type="button" value="Cancel">
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
			</tr>
		</table>
		
    </div>
    </form>
</body>
</html>
