<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserApproval.aspx.cs" Inherits="MikroMnt.user.UserApproval" %>
<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" TagPrefix="dxuc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>UserApproval</title>
		<link href="../include/style.css" type="text/css" rel="stylesheet" />
        <!-- #include file="~/include/onepost.html" -->
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
											    <b>User Maintenance Approval</b>
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
					    <asp:datagrid id="DG_REQUEST" runat="server" AutoGenerateColumns="False" Width="100%">
							<AlternatingItemStyle CssClass="Alt1"></AlternatingItemStyle>
							<HeaderStyle CssClass="H1"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<Columns>
								<asp:BoundColumn DataField="USERID" HeaderText="User ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="SU_FULLNAME" HeaderText="Full Name">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="SG_GRPNAME" HeaderText="Group">
									<ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="CH_STA" HeaderText="STATUSID"></asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS" HeaderText="Status"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SU_REGISTERBY" HeaderText="REQUESTED BY"></asp:BoundColumn>
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
								<asp:BoundColumn Visible="False" DataField="GROUPID" HeaderText="groupid"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText=" ">
									<ItemStyle HorizontalAlign="Center" Width="75px"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton id="lnkDetail" runat="server" alt=<%# Eval("USERID")%> Visible="false" OnClientClick="callbackpopup(popup,panel,'r:'+this.alt)">Detail</asp:LinkButton>&nbsp;
										<input type="button" id="btn3" class="btn btn-xs btn-success" runat="server" value="detail" alt=<%# Eval("USERID")%>
										onclick="callbackpopup(popup,panel,'r:'+this.alt);" />
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td align="center"></td>
				</tr>
				<tr>
					<td class="F1" align="center">
					    <asp:button id="BTN_SUBMIT" runat="server" Width="87px" CssClass="Bt1" Text="Submit" onclick="BTN_SUBMIT_Click"></asp:button>
					</td>
				</tr>
			</table>
				
    </div>

    <dxuc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" HeaderText="Detail Changes" width="500px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection><dxuc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" Height="100%">
        <dxuc:ASPxCallbackPanel ID="panel" runat="server" ClientInstanceName="panel" 
                OnCallback="panel_Callback" >
        <PanelCollection><dxuc:PanelContent ID="PanelContent1" runat="server">
                Jenis Perubahan : <asp:Label ID="ch_type" Font-Bold="true" runat="server"></asp:Label>
                <table width="100%" class="Box1">
                    <tr>
                        <td width="20%" style="border-right:dotted 1px black;">&nbsp;</td>
                        <td width="40%" style="border-right:dotted 1px black;" align="center"><b>BEFORE</b></td>
                        <td width="40%" align="center"><b>AFTER</b></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><b>UserId</b></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_userid" runat="server"></asp:Label></td>
                        <td style="background-color:lightblue"><asp:Label ID="af_userid" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><b>User Name</b></td>
                        <td style="border-right:dotted 1px black;"><asp:Label ID="bf_username" runat="server"></asp:Label></td>
                        <td ><asp:Label ID="af_username" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Group</strong></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_group" runat="server"></asp:Label></td>
                        <td style="background-color:lightblue"><asp:Label ID="af_group" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Email</strong></td>
                        <td style="border-right:dotted 1px black;"><asp:Label ID="bf_email" runat="server"></asp:Label>&nbsp;</td>
                        <td ><asp:Label ID="af_email" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>No HP</strong></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_hp" runat="server"></asp:Label>&nbsp;</td>
                        <td style="background-color:lightblue"><asp:Label ID="af_hp" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Cabang</strong></td>
                        <td style="border-right:dotted 1px black;"><asp:Label ID="bf_cabang" runat="server"></asp:Label>&nbsp;</td>
                        <td ><asp:Label ID="af_cabang" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Upliner 1</strong></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_upliner1" runat="server"></asp:Label>&nbsp;</td>
                        <td style="background-color:lightblue"><asp:Label ID="af_upliner1" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Upliner 2</strong></td>
                        <td style="border-right:dotted 1px black;"><asp:Label ID="bf_upliner2" runat="server"></asp:Label>&nbsp;</td>
                        <td ><asp:Label ID="af_upliner2" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Upliner 3</strong></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_upliner3" runat="server"></asp:Label>&nbsp;</td>
                        <td style="background-color:lightblue"><asp:Label ID="af_upliner3" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Upliner 4</strong></td>
                        <td style="border-right:dotted 1px black;"><asp:Label ID="bf_upliner4" runat="server"></asp:Label>&nbsp;</td>
                        <td ><asp:Label ID="af_upliner4" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Upliner 5</strong></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_upliner5" runat="server"></asp:Label>&nbsp;</td>
                        <td style="background-color:lightblue"><asp:Label ID="af_upliner5" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Status Aktif</strong></td>
                        <td style="border-right:dotted 1px black;"><asp:Label ID="bf_status" runat="server"></asp:Label></td>
                        <td ><asp:Label ID="af_status" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="border-right:dotted 1px black;"><strong>Locked</strong></td>
                        <td style="border-right:dotted 1px black;background-color:lightblue"><asp:Label ID="bf_locked" runat="server"></asp:Label></td>
                        <td style="background-color:lightblue"><asp:Label ID="af_locked" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center"><input type="button" onclick="popup.Hide()" value="Close" /></td>
                    </tr>
                </table>
        </dxuc:PanelContent></PanelCollection></dxuc:ASPxCallbackPanel>        
        </dxuc:PopupControlContentControl></ContentCollection>
    </dxuc:ASPxPopupControl>

    </form>
</body>
</html>
