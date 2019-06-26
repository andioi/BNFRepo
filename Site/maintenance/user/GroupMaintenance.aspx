<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupMaintenance.aspx.cs" Inherits="MikroMnt.user.GroupMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>GroupMaintenance</title>
		<link href="../include/style.css" type="text/css" rel="stylesheet" />
		<!-- #include file="../include/cek_mandatoryOnly.html" -->
		<script language="javascript" type="text/javascript">
		    function simpan()
		    {
                var childfrm = document.getElementById("IFR_MODULE");  
			    var vgid = document.getElementById("TXT_GROUPID");
			    if (childfrm != null)
			    {
                    childfrm.contentDocument.getElementById("gid").value = vgid.value;
                    childfrm.contentDocument.getElementById("sta").value = "save";
                    childfrm.contentDocument.getElementById("form1").submit();
                    alert("Module access saved. Please save again on the group level..");
			    }
            }

            function resizeFrame() 
            {
                try{
                    var oBody = document.IFR_MODULE.document.body;
                    var oFrame = document.getElementById('IFR_MODULE');
                    oFrame.style.width = "100%"
                    var h = oBody.scrollHeight + (oBody.offsetHeight - oBody.clientHeight) + 20;
                    if (h < 150)
                        h = 150;
                    oFrame.style.height = h;
                    oFrame.style.width = oBody.scrollWidth + (oBody.offsetWidth - oBody.clientWidth);
                }
                //An error is raised if the IFrame domain != its container's domain
                catch(e)
                {
                    window.status =      'Error: ' + e.number + '; ' + e.description;
                }
            }

            function klikMenuAccess()
            {
                if (document.getElementById("TXT_GROUPID").value == "") {
                    alert("Please input Group ID.");
                    document.getElementById("TXT_GROUPID").focus();
                } else {
                    document.getElementById("TXT_GROUPID").readOnly = true;
                    window.open('GroupMenuAccess.aspx?ModuleID=61&ModuleName=CBAS&GroupID=' + document.getElementById("TXT_GROUPID").value, 'MenuAccess', 'status=no,scrollbars=yes,width=500,height=400');
                    document.getElementById("TXT_GROUPID").focus();
                }
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
											    <b>Parameter Maker: Group Maintenance</b>
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
								<td valign="top" width="50%">
									<table class="Tbl0">
										<tr style="display:none">
											<td class="B01">Module</td>
											<td class="BS">:</td>
											<td class="B11">
											    <asp:dropdownlist id="DDL_MODULEID" runat="server"></asp:dropdownlist>
											</td>
										</tr>
                                        <tr>
											<td class="B01">Group ID</td>
											<td class="BS">:</td>
											<td class="B11">
											    <asp:textbox id="TXT_FINDGROUPID" runat="server"></asp:textbox>
											</td>
										</tr>
									</table>
								</td>
								<td valign="top">
									<table class="Tbl0">
										<tr>
											<td class="B01">Group Name</td>
											<td class="BS">:</td>
											<td class="B11">
											    <asp:textbox id="TXT_FINDGROUP" runat="server"></asp:textbox>
											</td>
										</tr>
										<tr>
											<td class="B01">Group Upliner</td>
											<td class="BS">:</td>
											<td class="B11">
											    <asp:textbox id="TXT_FINDUPLINER" runat="server"></asp:textbox>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td align="center" colspan="2">
								    <asp:button id="BTN_SEARCH" runat="server" CssClass="Bt1" Text="Search" Width="100px" onclick="BTN_SEARCH_Click"></asp:button>&nbsp;
									<asp:button id="BTN_CLEARSEARCH" runat="server" Text="Clear" CssClass="Bt1" Width="100px" onclick="BTN_CLEARSEARCH_Click"></asp:button>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
					    <ASP:DATAGRID id="DatGrd" runat="server" Width="100%" 
                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False"
							CellPadding="1" onitemcommand="DatGrd_ItemCommand" onpageindexchanged="DatGrd_PageIndexChanged" 
                            onsortcommand="DatGrd_SortCommand">
							<HeaderStyle CssClass="H2"></HeaderStyle>
							<AlternatingItemStyle CssClass="Alt1"></AlternatingItemStyle>
							<ItemStyle HorizontalAlign="Center"></ItemStyle>
							<Columns>
								<asp:BoundColumn DataField="GROUPID" SortExpression="GROUPID" HeaderText="Group ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="SG_GRPNAME" SortExpression="SG_GRPNAME" HeaderText="Group Name"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SG_ROLEDESC" HeaderText="Group Description"></asp:BoundColumn>
                                <asp:BoundColumn DataField="APPROVAL_GROUP" HeaderText="Save&Recalculate"></asp:BoundColumn>
                                <asp:BoundColumn DataField="SAVECALCULATOR" HeaderText="CBAS Client"></asp:BoundColumn>
                                <asp:BoundColumn DataField="FLAG_SUPERVISOR" HeaderText="Supervisor Role"></asp:BoundColumn>
								<asp:BoundColumn Visible="False" DataField="SG_ACTIVE"></asp:BoundColumn>
								<asp:TemplateColumn HeaderText="Function">
									<ItemTemplate>
										<asp:LinkButton id="lnk_menu" Visible="false" runat="server" CommandName="menuAccess">Menu Access</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lnk_edit" runat="server" CommandName="edit">Edit</asp:LinkButton>&nbsp;
										<asp:LinkButton id="lnk_delete" runat="server" CommandName="delete">Delete</asp:LinkButton>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:BoundColumn Visible="False" DataField="sg_grpupliner"></asp:BoundColumn>
							</Columns>
							<PagerStyle Mode="NumericPages"></PagerStyle>
						</ASP:DATAGRID>
					</td>
				</tr>
				<tr>
					<td align="center"><asp:label id="LBL_RESULT" runat="server" Font-Bold="True" ForeColor="Red"></asp:label></td>
				</tr>
				<tr>
					<td class="H1" valign="top">Detail Information</td>
				</tr>
				<tr>
					<td>
						<table class="Tbl0">
							<tr>
								<td valign="top" width="0%">
									<table class="Tbl0">
										<tr>
											<td class="B01">Group ID</td>
											<td class="BS"></td>
											<td class="B11"><asp:textbox id="TXT_GROUPID" runat="server" MaxLength="20" CssClass="mandatory"></asp:textbox></td>
										</tr>
										<tr>
											<td class="B01">Group Name</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="TXT_SG_GRPNAME" MaxLength="50" runat="server" Width="175px" CssClass="mandatory"></asp:textbox></td>
										</tr>
										<tr>
											<td class="B01">Enable Save & Recalculate</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="CHK_SG_APPRSTA" runat="server" Text="(check for enable access)"></asp:checkbox></td>
										</tr>
                                        <tr>
											<td class="B01">CBAS Client</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="CHK_SG_CALCULATOR" runat="server" Text="(check for enable access)"></asp:checkbox></td>
										</tr>
                                        <tr>
											<td class="B01">Supervisor Role</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkbox id="FLAG_SUPERVISOR" runat="server" Text="(check for true)"></asp:checkbox></td>
										</tr>
										<tr>
											<td class="B01">Group Upliner</td>
											<td class="BS">:</td>
											<td class="B11"><asp:dropdownlist id="DDL_SG_GRPUPLINER" runat="server" Width="250px"></asp:dropdownlist></td>
										</tr>
                                        <tr style="display:none">
											<td class="B01">Role AD</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="MEMBEROF_AD" runat="server" Width="100px" MaxLength="10" Text="" /></td>
										</tr>
										<tr>
											<td class="B01">Module Access</td>
											<td class="BS">:</td>
											<td class="B11"><asp:checkboxlist id="CHK_MODULEID" runat="server" Width="100%" 
                                                    RepeatColumns="1" AutoPostBack="True" RepeatDirection="Vertical"
                                                    onselectedindexchanged="CHK_MODULEID_SelectedIndexChanged" ></asp:checkboxlist>
                                            </td>
										</tr>
                                        <tr>
                                            <td class="B01">Menu Access</td>
											<td class="BS">:</td>
											<td class="B11">
                                                <asp:HyperLink id="lnk_menu" NavigateUrl="#" onclick="javascript:klikMenuAccess();" runat="server" Text="Menu Access"></asp:HyperLink>
                                                </td>
                                        </tr>
                                        <tr>
											<td class="B01">Group/Role Description</td>
											<td class="BS">:</td>
											<td class="B11"><asp:textbox id="SG_ROLEDESC" runat="server" MaxLength="500" TextMode="MultiLine" Width="95%" Rows="8"></asp:textbox></td>
										</tr>
									</table>
								</td>
								<td valign="top" width="50%">
									<table class="Tbl0">
										<tr>
											<td class="H2">Module</td>
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
					    <asp:button id="BTN_NEW" runat="server" Text="New" Width="70px" CssClass="Bt1" onclick="BTN_NEW_Click"></asp:button>&nbsp;
						<asp:button id="BTN_SAVE" runat="server" Text="Save" Width="70px" Visible="False" CssClass="Bt1" onclick="BTN_SAVE_Click"></asp:button>&nbsp;
						<asp:button id="BTN_CANCEL" runat="server" Text="Cancel" Width="70px" Visible="False" CssClass="Bt1" onclick="BTN_CANCEL_Click"></asp:button>
						<asp:label id="LBL_SAVEMODE" runat="server" Visible="False">1</asp:label>
					</td>
				</tr>
			</table>
			
    </div>
    </form>
</body>
</html>
