<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupApproval.aspx.cs" Inherits="MikroMnt.user.GroupApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>GroupApproval</title>
		<link href="../include/style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
			<table class="Box1" width="100%">
				<tr>
					<td>
						<table width="100%">
							<tr>
								<td class="H0">
									<table>
										<tr>
											<td class="H1" style="WIDTH: 400px" align="center">
											    <b>Group Maintenance Approval</b>
											</td>
										</tr>
									</table>
								</td>
								<td class="H0" align="right">
								    <a href="../Body.aspx"><img src="../Image/MainMenu.jpg" alt="mainmenu" /></a>
								    <a href="../Logout.aspx" target="_top"><img src="../Image/Logout.jpg" alt="logout" /></a>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
					    <asp:datagrid id="DG_REQUEST" runat="server" AutoGenerateColumns="False" Height="32px" Width="100%">
							<HeaderStyle CssClass="H1"></HeaderStyle>
							<AlternatingItemStyle CssClass="Alt1"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<Columns>
								<asp:BoundColumn DataField="GROUPID" HeaderText="Group ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="SG_GRPNAME" HeaderText="Group Description"></asp:BoundColumn>
								<asp:BoundColumn DataField="SG_GRPUNAME" HeaderText="Group Upliner"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="MODULEIDS" HeaderText="MODULEIDS"></asp:BoundColumn>
								<asp:BoundColumn DataField="MODULENAMES" HeaderText="Access Modules"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CH_STA" HeaderText="CH_STA"></asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS" HeaderText="Status"></asp:BoundColumn>
                                <asp:BoundColumn DataField="REQUESTBY" HeaderText="REQUESTED BY"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="MENU ACCESS">
									<ItemStyle HorizontalAlign="Center" Width="75px"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton id="lnkMenuAccess" OnClientClick='<%#clickMenuAccess(Eval("GROUPID"))%>' runat="server" >Menu Access</asp:LinkButton>&nbsp;
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Accept">
									<ItemTemplate>
										<asp:RadioButton id="RDO_APPROVE" runat="server" GroupName="function"></asp:RadioButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Reject">
									<ItemTemplate>
										<asp:RadioButton id="RDO_REJECT" runat="server" GroupName="function"></asp:RadioButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Pending">
									<ItemTemplate>
										<asp:RadioButton id="RDO_PENDING" runat="server" GroupName="function" Checked="True"></asp:RadioButton>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
					    </asp:datagrid>
					</td>
				</tr>
				<tr>
					<td align="center" colspan="2"></td>
				</tr>
				<tr class="F1">
					<td>
					    <asp:button id="BTN_SUBMIT" runat="server" Width="87px" CssClass="Bt1" Text="Submit" onclick="BTN_SUBMIT_Click"></asp:button>
					</td>
				</tr>
			</table>
				
    </div>

    
    </form>
</body>
</html>
