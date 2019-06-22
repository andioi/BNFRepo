<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="MikroLogin.ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Change Password</title>
		<link href="style.css" type="text/css" rel="stylesheet" />
		<!-- #include file="include/onepost.html" -->
		<script language="javascript">
			function kutip_satu()
			{
				if ((event.keyCode == 35) || (event.keyCode == 39))
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
			<table width="100%" bgcolor="#ffffff" border="0">
				<tr>
					<td style="height:100px">&nbsp;</td>
				</tr>
				<tr bgcolor="#f5f5f5" style="height:400px">
					<td valign="middle" align="center">
						<table class="td" cellspacing="1" cellpadding="1" border="1">
							<tr>
								<td class="T1">Change Password
								    <a id="loginURL" href="Login.aspx?logon" runat="server">
								    <img id="IMG_BACK" src="image/spacer.gif" alt="" />
								    </a>
								</td>
							</tr>
							<tr>
								<td valign="top" align="center">
									<table id="Table2" cellspacing="0" cellpadding="0" width="400" border="0">
										<tr>
											<td class="B01">Old Password&nbsp;</td>
											<td class="B11">
											    &nbsp;&nbsp;<asp:textbox onkeypress="return kutip_satu()" id="TXT_OLD" runat="server" BackColor="#f5f5f5" TextMode="Password"></asp:textbox>
											</td>
										</tr>
										<tr>
											<td class="B01">New Password&nbsp;</td>
											<td class="B11">
											    &nbsp;&nbsp;<asp:textbox onkeypress="return kutip_satu()" id="TXT_NEW" runat="server" BackColor="#f5f5f5" TextMode="Password"></asp:textbox>
										    </td>
										</tr>
										<tr>
											<td class="B01">Verify Password&nbsp;</td>
											<td class="B11">
											    &nbsp;&nbsp;<asp:textbox onkeypress="return kutip_satu()" id="TXT_VERIFY" runat="server" BackColor="#f5f5f5" TextMode="Password"></asp:textbox>
											</td>
										</tr>
										<tr>
											<td colspan="2"></td>
										</tr>
										<tr>
											<td valign="top" align="center" colspan="2">
											    <asp:label id="LBL_MESSAGE" runat="server" ForeColor="Red"></asp:label>&nbsp;
												<asp:label id="LBL_SC_ID" runat="server" Visible="False"></asp:label>
											</td>
										</tr>
									</table>
									<asp:button id="BTN_CHANGE" runat="server" Width="85px" Text="Change" 
                                        CssClass="Button1" onclick="BTN_CHANGE_Click"></asp:button>&nbsp;&nbsp;
									<input id="BTN_CANCEL" runat="server" class="Button1" style="WIDTH: 85px" onclick="javascript:form1.IMG_BACK.click()" type="button" value="Cancel" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td style="height:100px"></td>
				</tr>
			</table>
    </div>
    </form>
</body>
</html>
