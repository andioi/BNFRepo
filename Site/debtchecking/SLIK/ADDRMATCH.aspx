<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ADDRMATCH.aspx.cs" Inherits="DebtChecking.SLIK.ADDRMATCH" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxwgv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Scoring Bureau</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <script language="javascript" type="text/javascript">
        function cek() {
            if (document.form1.mainPanel_home_match_0.checked == false &&
                document.form1.mainPanel_home_match_1.checked == false) {
                alert("Please select match / no match for home address..!");
                document.form1.mainPanel_home_match_0.focus();
                return false;
            }
            else if (document.form1.mainPanel_office_match_0.checked == false &&
                document.form1.mainPanel_office_match_1.checked == false) {
                alert("Please select match / no match for office address..!");
                document.form1.mainPanel_office_match_0.focus();
                return false;
            }
            else if (document.form1.mainPanel_coyname_match_0.checked == false &&
                document.form1.mainPanel_coyname_match_1.checked == false) {
                alert("Please select match / no match for company name..!");
                document.form1.mainPanel_coyname_match_0.focus();
                return false;
            }
            else
                return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
       OnCallback="mainPanel_Callback" ClientInstanceName="mainPanel">
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
    <table class="Box1" width="100%">
    <tr>
        <td>
		<table id="DataDebitur" width="100%">
			<tr valign="top">
				<td class="H1" colspan="2">General Information</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">App ID / REFFNUMBER</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="reffnumber" runat="server"></asp:Label>
			                </td>
			            </tr>
			            <tr>
			                <td class="B01">Name</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="cust_name" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Date Of Birth</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="dob" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">ID Number</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ktp_num" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			</tr>
            <tr>
                <td align="center" colspan="2"><input type="button" id="btnprint" runat="server" value="Print" onclick="this.style.display = 'none'; document.getElementById('mainPanel_btnpdf').style.display = 'none'; window.print(); this.style.display = ''; document.getElementById('mainPanel_btnpdf').style.display = '';" />
                    <input type="button" id="btnpdf" value="Save As PDF" runat="server" onclick="callback(pdfPanel, '')" />
                    <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
                        oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
                        <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
                        <input type="hidden" id="urlframe" runat="server" />
                    </dxp:PanelContent></PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
			<tr valign="top">
				<td class="H1" colspan="2">Address Matching</td>
			</tr>
			<tr valign="top">
			    <td colspan="2">
			        <table width="100%" border="1" cellpadding="2">
			            <tr>
			                <td bgcolor="#009933" style="color:white" width="10%"><center><b>TYPE</b></center></td>
			                <td bgcolor="#009933" style="color:white" width="35%"><center><b>APPLICANT DATA</b></center></td>
			                <td bgcolor="#009933" style="color:white" width="40%"><center><b>SLIK</b></center></td>
			                <td bgcolor="#009933" style="color:white" width="15%"><center><b>Match/No Match</b></center></td>
			            </tr>
			            <tr valign="top">
			                <td align="center"><center><b>HOME & KTP ADDRESS</b></center></td>
			                <td>
			                <table width="100%" class="Box1">
                                <tr><td colspan="3" style="border:1px solid black;font-weight:bold;background-color:lightcoral" >HOME</td></tr>
			                    <tr>
			                        <td class="B01">Address</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="home_addr_app" runat="server"></asp:Label></td>
			                    </tr>
			                    <tr>
			                        <td class="B01">Home Phone</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="home_phone_app" runat="server"></asp:Label></td>
			                    </tr>
                                <tr><td colspan="3" style="border:1px solid black;font-weight:bold;background-color:lightcoral">KTP</td></tr>
			                    <tr>
			                        <td class="B01">Address</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="ktp_addr_app" runat="server"></asp:Label></td>
			                    </tr>
                                <tr>
			                        <td class="B01">Mobile Phone</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="mobilenum" runat="server"></asp:Label></td>
			                    </tr>
			                </table>
			                </td>
			                <td rowspan="2">
			                    <asp:GridView ID="GridViewAddr" runat="server" AutoGenerateColumns="false" width="100%" CssClass="Dg1">
                                    <HeaderStyle BorderColor="Black" BackColor="LightCoral" Height="0" HorizontalAlign="Center" BorderWidth="0" BorderStyle="None" />
                                    <RowStyle HorizontalAlign="Center" Font-Size="X-Small" />
                                    <Columns>
                                        <asp:BoundField DataField="alamat" HeaderText="ADDRESS FROM SLIK" ItemStyle-VerticalAlign="Middle" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HtmlEncode="false"/>
                                    </Columns>
                                </asp:GridView><br />
                                <asp:GridView ID="GridViewComp" runat="server" AutoGenerateColumns="false" width="100%" CssClass="Dg1">
                                    <HeaderStyle BorderColor="Black" BackColor="LightCoral" Height="0" HorizontalAlign="Center" BorderWidth="0" BorderStyle="None" />
                                    <RowStyle HorizontalAlign="Center" Font-Size="X-Small" />
                                    <Columns>
                                        <asp:BoundField DataField="tempatBekerja" HeaderText="COMPANY NAME FROM SLIK" ItemStyle-VerticalAlign="Middle" 
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HtmlEncode="false"/>
                                    </Columns>
                                </asp:GridView>
			                </td>
			                <td><b>Home/KTP Address Match:</b>
                                <asp:RadioButtonList ID="home_match" runat="server" RepeatDirection="Horizontal">
			                    <asp:ListItem Value="1" Text="Match"></asp:ListItem>
			                    <asp:ListItem Value="0" Text="No Match"></asp:ListItem>
			                </asp:RadioButtonList></td>
			            </tr>
			            <tr valign="top">
			                <td align="center" bgcolor="#ccffcc"><center><b>COMPANY & EMERGENCY CONTACT</b></center></td>
			                <td bgcolor="#ccffcc">
			                 <table width="100%" class="Box1">
                                <tr><td colspan="3" style="border:1px solid black;font-weight:bold;background-color:lightcoral">COMPANY</td></tr>
                                 <tr>
			                        <td class="B01">Company Name</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="officename" runat="server"></asp:Label></td>
			                    </tr>
			                    <tr>
			                        <td class="B01">Office Address</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="office_addr_app" runat="server"></asp:Label></td>
			                    </tr>
			                    <tr>
			                        <td class="B01">Office Phone</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="office_phone_app" runat="server"></asp:Label></td>
			                    </tr>
                                 <tr><td colspan="3" style="border:1px solid black;font-weight:bold;background-color:lightcoral">EMERGENCY ADDRESS</td></tr>
			                    <tr>
			                        <td class="B01">Address</td>
			                        <td class="BS">:</td>
			                        <td class="B11"><asp:Label ID="econ_addr_app" runat="server"></asp:Label></td>
			                    </tr>
			                </table>
			                </td>
			                <td bgcolor="#ccffcc">
                                <b>Company Name Match:</b>
                                <asp:RadioButtonList ID="coyname_match" runat="server" RepeatDirection="Horizontal" >
			                        <asp:ListItem Value="1" Text="Match"></asp:ListItem>
			                        <asp:ListItem Value="0" Text="No Match"></asp:ListItem>
			                    </asp:RadioButtonList><br /><br />
                                <b>Company Address Match:</b>
                                <asp:RadioButtonList ID="office_match" runat="server" RepeatDirection="Horizontal" >
			                        <asp:ListItem Value="1" Text="Match"></asp:ListItem>
			                        <asp:ListItem Value="0" Text="No Match"></asp:ListItem>
			                    </asp:RadioButtonList>

			                </td>
			            </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                            <td align="center">
                                    <input id="BTN_SAVE" runat="server" class="Bt1" onclick="if (cek()) callback(mainPanel, 'save')" type="button" value="Save"/>
                                    <input id="BTN_SUBMIT" runat="server" class="Bt1" onclick="if (cek()) callback(mainPanel, 'submit')" type="button" value="Submit" visible="false" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
			        </table>
			    </td>
			</tr>
			
		</table>	
		</td>
    </tr>
    </table>
    </dxp:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    </form>
</body>
</html>
