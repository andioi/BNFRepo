<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="MikroMnt.user.UserMaintenance" %>
<%@ Register TagPrefix="uc1" TagName="UC_RefList" Src="../CommonForm/UC_RefList.ascx" %>
<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" TagPrefix="dxuc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>UserMaintenance</title>
		<link href="../include/style.css" type="text/css" rel="stylesheet" />
		<!-- #include file="~/include/cek_mandatoryOnly.html" -->
        <!-- #include file="~/include/onepost.html" -->
		<script language="javascript" type="text/javascript">
		function simpan()
		{
			var childfrm = document.IFR_MODULE.document.form1;
			var vuser = document.form1.TXT_USERID;
			var branch = document.form1.uREF_BRANCHID_CODE;
			if (branch == null)
			    branch = document.form1.uREF_BRANCHID_DDL;
			if (childfrm != null)
			{
				childfrm.uid.value = vuser.value;
				childfrm.sta.value = "save";
				childfrm.br.value = branch.value;
				childfrm.submit();
			}
		}
		
		function showpwdmsg()
		{
		 //   if (!form1.panelNama_TXT_SU_PWD.disabled && form1.panelNama_TXT_SU_PWD.dataFld == '')
			//{
		 //       form1.panelNama_TXT_SU_PWD.dataFld = '1';
			//	alert(form1.pwdmsg.value);
			//}
		}

		function tabE(e) {
		    var e = (typeof event != 'undefined') ? window.event : e; // IE : Moz 
		    if (e.keyCode == 13) { e.keyCode = 9; return e.keyCode }
            }

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }    
		</script>
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
											    <b>Parameter Maker: User Maintenance</b>
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
					<td class="H1">Existing Data</td>
				</tr>
				<tr>
					<td>
						<table class="Tbl0">
							<tr>
								<td valign="top">
									<table class="Tbl0">
										<tr>
											<td class="B01">Module</td>
											<td class="BS">:</td>
											<td class="B11">
											    <asp:dropdownlist id="DDL_RFMODULE" runat="server"></asp:dropdownlist>
    											<input id="pwdmsg" type="hidden" name="pwdmsg" runat="server" />
											</td>
										</tr>
										<tr>
											<td class="B01">Group</td>
											<td class="BS">:</td>
											<td class="B11">
											    <asp:dropdownlist id="DDL_RFGROUP" runat="server"></asp:dropdownlist>
											</td>
										</tr>
										<tr>
											<td class="B01">Branch</td>
											<td class="BS">:</td>
											<td class="B11">
											    <uc1:uc_reflist id="uREF_BRANCH" runat="server"></uc1:uc_reflist>
											</td>
										</tr>
                                        <tr>
											<td class="B01">User ID</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SEARCH_USERID" runat="server"></asp:textbox></td>
										</tr>
									</table>
								</td>
								<td valign="top" align="center">
									<table class="Tbl0">										
										<tr>
											<td class="B01">Full Name</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SEARCH_USERNAME" runat="server"></asp:textbox></td>
										</tr>
										<tr>
											<td class="B01">Upliner 1</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SEARCH_UPLINER" runat="server"></asp:textbox>&nbsp;</td>
										</tr>
                                        <tr>
											<td class="B01">Upliner 2</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SEARCH_UPLINER2" runat="server"></asp:textbox>&nbsp;</td>
										</tr>
                                        <tr>
											<td class="B01">Upliner 3</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SEARCH_UPLINER3" runat="server"></asp:textbox>&nbsp;</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td style="WIDTH: 50%" align="right"><asp:button id="BTN_SEARCH" runat="server" Width="100px" Text="Search" CssClass="Bt1" onclick="BTN_SEARCH_Click"></asp:button></td>
								<td style="WIDTH: 50%" align="left"><asp:button id="BTN_CLEAR" runat="server" Width="100px" Text="Clear" CssClass="Bt1" onclick="BTN_CLEAR_Click"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
					    <ASP:DATAGRID id="DatGrd" runat="server" Width="100%" AllowSorting="True" AutoGenerateColumns="False"
							CellPadding="1" AllowPaging="True" onitemcommand="DatGrd_ItemCommand" 
                            onpageindexchanged="DatGrd_PageIndexChanged" onsortcommand="DatGrd_SortCommand">
							<AlternatingItemStyle CssClass="Alt1"></AlternatingItemStyle>
							<HeaderStyle CssClass="H2"></HeaderStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<Columns>
								<asp:BoundColumn Visible="False" DataField="MODULEID"></asp:BoundColumn>
								<asp:BoundColumn DataField="MODULENAME" SortExpression="MODULENAME" HeaderText="Module"></asp:BoundColumn>
								<asp:BoundColumn DataField="USERID" SortExpression="USERID" HeaderText="User ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="SU_FULLNAME" SortExpression="SU_FULLNAME" HeaderText="Full Name">
								    <ItemStyle HorizontalAlign="Left"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="GROUPID"></asp:BoundColumn>
								<asp:BoundColumn DataField="SG_GRPNAME" SortExpression="SG_GRPNAME" HeaderText="Group"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="BRANCHID" HeaderText="Branch ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="BRANCHNAME" SortExpression="BRANCHNAME" HeaderText="Branch"></asp:BoundColumn>
								<asp:BoundColumn DataField="SU_LOGON" HeaderText="Logon"></asp:BoundColumn>
								<asp:BoundColumn DataField="SU_REVOKE" HeaderText="Revoke" Visible="false"></asp:BoundColumn>
								<asp:BoundColumn DataField="SU_ACTIVE" HeaderText="Active" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="JenisUserDesc" HeaderText="Jenis User"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Function">
									<ItemStyle HorizontalAlign="Center" Width="75px"></ItemStyle>
									<ItemTemplate>
										<asp:LinkButton id="lnkEdit" runat="server" CommandName="edit">Edit</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lnkDelete" runat="server" CommandName="delete" >Delete</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lnkUndelete" runat="server" CommandName="undelete" >UnDelete</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="inpending"></asp:BoundColumn>
							</Columns>
							<PagerStyle Mode="NumericPages"></PagerStyle>
						</ASP:DATAGRID>
					</td>
				</tr>
				<tr>
					<td align="center"><asp:label id="LBL_RESULT" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></td>
				</tr>
				<tr>
					<td class="H1">Detail Information</td>
				</tr>
				<tr>
					<td>
						<table class="Tbl0">
							<tr>
								<td valign="top">
									<table class="Tbl0">
										<tr>
											<td class="B01">UserID</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_USERID" runat="server" CssClass="mandatory" MaxLength="20"></asp:textbox>
                                                <input type="button" class="Bt1" id="btn_cekAD" runat="server" value="Cek AD User" style="display:none" onclick="callbackpopup(popup, panel, 'new');" />&nbsp;
											</td>
										</tr>
										<tr>
											<td class="B01">Full Name</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <asp:textbox id="TXT_SU_FULLNAME" runat="server" Width="250px" CssClass="mandatory" onchange="document.form1.panelNama_hdn_nama.value=this.value;" MaxLength="50"></asp:textbox>
                                                        <input type="hidden" id="hdn_nama" runat="server" />
											</td>
										</tr>
                                        <tr>
											<td class="B01">Group</td>
											<td class="BS">:</td>
											<td class="B11"><asp:dropdownlist id="DDL_GROUPID" runat="server" Width="250px" AutoPostBack="True" CssClass="mandatory"></asp:dropdownlist></td>
										</tr>
										<tr >
											<td class="B01">Password</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SU_PWD" runat="server" CssClass="mandatory" TextMode="Password" MaxLength="12"></asp:textbox></td>
										</tr>
										<tr >
											<td class="B01">Verify</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_VERIFYPWD" runat="server" CssClass="mandatory" TextMode="Password" MaxLength="12"></asp:textbox></td>
										</tr>
										<tr>
											<td class="B01">HP No.</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SU_HPNUM" MaxLength="15" runat="server" onkeypress="javascript:return isNumber(event)"></asp:textbox></td>
										</tr>
										<tr>
											<td class="B01">Email</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SU_EMAIL" runat="server" Width="250px" onchange="document.form1.panelNama_hdn_email.value=this.value;" MaxLength="150"></asp:textbox>
                                                <input type="hidden" id="hdn_email" runat="server" />
											</td>
										</tr>
								        <tr>
								            <td class="B01">Branch</td>
											<td class="BS">:</td>
								            <td class="B11">
                                                <uc1:uc_reflist id="uREF_BRANCHID" runat="server"></uc1:uc_reflist>
								            </td>
								        </tr>
                                        <tr style="display:none">
								            <td class="B01">Area</td>
											<td class="BS">:</td>
								            <td class="B11">
                                                <uc1:uc_reflist id="uREF_AREAID" runat="server"></uc1:uc_reflist>
								            </td>
								        </tr>
										<tr>
											<td class="B01">Upliner 1</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <uc1:uc_reflist id="uREF_UPLINER" runat="server"></uc1:uc_reflist>
											</td>
										</tr>
                                        <tr>
											<td class="B01">Upliner 2</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <uc1:uc_reflist id="uREF_UPLINER2" runat="server"></uc1:uc_reflist>
											</td>
										</tr>
                                        <tr>
											<td class="B01">Upliner 3</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <uc1:uc_reflist id="uREF_UPLINER3" runat="server"></uc1:uc_reflist>
											</td>
										</tr>
                                        <tr>
											<td class="B01">Upliner 4</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <uc1:uc_reflist id="uREF_UPLINER4" runat="server"></uc1:uc_reflist>
											</td>
										</tr>
                                        <tr>
											<td class="B01">Upliner 5</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <uc1:uc_reflist id="uREF_UPLINER5" runat="server"></uc1:uc_reflist>
											</td>
										</tr>
                                        <tr style="display:none">
											<td class="B01">Jenis User</td>
											<td class="BS">:</td>
											<td class="B11"><asp:dropdownlist id="ddl_JenisUser" runat="server">
                                                <asp:ListItem Value="" Text="- PILIH -"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Active Directory"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="User Local"></asp:ListItem>
											</asp:dropdownlist></td>
										</tr>
                                        <tr style="display:none">
											<td class="B01">Jenis User</td>
											<td class="BS">:</td>
											<td class="B11"><asp:Label id="lbl_jenisuser" runat="server"></asp:Label></td>
										</tr>
										<tr>
											<td colspan="3"></td>
										</tr>
										<tr>
											<td class="B01">Logon status</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="cb_logon" runat="server" Text="(clear to reset)" AutoPostBack="True" oncheckedchanged="cb_logon_CheckedChanged"></asp:checkbox></td>
										</tr>
										<tr >
											<td class="B01">Revoke</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="cb_revoke" runat="server" Text="(check for yes)"></asp:checkbox></td>
										</tr>
										<tr >
											<td class="B01">Active</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="CHK_SU_ACTIVE" runat="server" Text="(check for yes)"></asp:checkbox></td>
										</tr>
										<tr>
											<td colspan="3"></td>
										</tr>
										<tr style="display:none">
											<td class="B01">Password Reset (to default)</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="cb_resetpwd" runat="server" Text="(check for yes)"></asp:checkbox></td>
										</tr>
									</table>
								</td>
								<td valign="top" style="display:none">
									<table class="Tbl0">
										<tr>
											<td class="H2">Module</td>
										</tr>
										<tr>
											<td id="TD_UC" runat="server"></td>
										</tr>
										<tr>
											<td>
											    <asp:radiobuttonlist id="RBL_MODULE" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
													RepeatLayout="Flow" onselectedindexchanged="RBL_MODULE_SelectedIndexChanged"></asp:radiobuttonlist>
											</td>
										</tr>
									</table>
									<table class="Tbl0">
										<tr>
											<td>
											    <iframe id="IFR_MODULE" tabindex="0" name="IFR_MODULE" frameborder="0" width="100%" height="150" runat="server"></iframe>
											</td>
										</tr>
									</table>
									<table cellspacing="0" cellpadding="0" width="100%">
										<tr>
											<td class="F1" align="center">
											    <input class="Bt1" id="BtnSaveModule" type="button" value="Save" onclick="simpan()" ></input>
										    </td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="F1" valign="top" align="center">
                        <asp:button id="BTN_NEW_AD" runat="server" Text="New AD User" CssClass="Bt1" onclick="BTN_NEW_AD_Click"></asp:button>&nbsp;
					    <asp:button id="BTN_NEW" runat="server" Text="New User" CssClass="Bt1" onclick="BTN_NEW_Click"></asp:button>&nbsp;
					    <asp:button id="BTN_SAVE" runat="server" Width="70px" Text="Submit" CssClass="Bt1" Visible="False" onclick="BTN_SAVE_Click"></asp:button>&nbsp;
						<asp:button id="BTN_CANCEL" runat="server" Width="70px" Text="Cancel" CssClass="Bt1" Visible="False" onclick="BTN_CANCEL_Click"></asp:button>
						<asp:label id="LBL_SAVEMODE" runat="server" Visible="False">1</asp:label>
					</td>
				</tr>
			</table>
    </div>

    </form>
</body>
</html>
