<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MMUE.aspx.cs" Inherits="DebtChecking.Facilities.MMUE" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>
<%@ Register assembly="DMSControls" namespace="DMSControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SID Text Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- #include file="~/include/UC/UC_Number.html" -->
    <!-- #include file="~/include/UC/UC_Date.html" -->
    <!-- #include file="~/include/UC/UC_Currency.html" -->
    <!-- #include file="~/include/UC/UC_Decimal.html" -->
    <style type="text/css">
        .boxbold { border:2px solid black; text-align:right; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table id="Content" class="Box1" width="100%" align="center">
    <tr>
        <td>
            <table id="DataDebitur" width="100%">
			<tr valign="top">
				<td class="H1" colspan="2">Data Debitur</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Nama</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:DropDownList ID="appid" runat="server"></asp:DropDownList></td>
			            </tr>
			            <tr>
			                <td class="B01">Tanggal Lahir</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="BORN_DATE" runat="server"></asp:Label><asp:Label ID="STATUS_APP" runat="server" style="display:none"></asp:Label><input type="hidden" runat="server" id="reffnumber" /></td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">No KTP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="KTP_NUM" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Alamat Domisili</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ALAMAT_DOM" runat="server"></asp:Label></td>
			            </tr>
			            <tr style="display:none">
			                <td class="B01">No Telp / HP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="TELP_HP" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			</tr>
		</table>	  
		</td>
    </tr>
    <tr>
        <td align="center"><input type="button" id="btnprint" runat="server" value="Print" onclick="this.style.display = 'none'; document.getElementById('btnpdf').style.display = 'none'; window.print(); this.style.display = ''; document.getElementById('btnpdf').style.display = '';" />
            <input type="button" id="btnpdf" value="Save As PDF" runat="server" onclick="callback(pdfPanel, '')" />
            <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
                oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
                <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
                <input type="hidden" id="urlframe" runat="server" />
            </dxp:PanelContent></PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%">
                <tr>
                    <td width="40%" class="H1">&nbsp;</td>
                    <td width="20%" class="H1">Collectibility</td>
                    <td width="20%" class="H1">Date of NPL (DD/MM/YYYY)</td>
                    <td width="20%" class="H1">DPD</td>
                </tr>
                <tr>
                    <td>Worst latest delinquency of all credit facilities</td>
                    <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_collect") %></td>
                    <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_latest_npldate") %></td>
                    <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_dpasdue") %></td>
                </tr>
                <tr>
                    <td>Worst delinquency of all credit facilities</td>
                    <td class="boxbold">&nbsp;<%=DS(0, "worst_collect") %></td>
                    <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_npldate") %></td>
                    <td class="boxbold">&nbsp;<%=DS(0, "worst_dpasdue") %></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="40%" class="H1">&nbsp;</td>
                    <td width="20%" class="H1">6 months</td>
                    <td width="20%" class="H1">12 months</td>
                    <td width="20%" class="H1">24 months</td>
                </tr>
                <tr>
                    <td>Highest count of 30+DPD in the last X months for all credit facilities</td>
                    <td class="boxbold"><%=DS(0, "count_30dpd_06months") %></td>
                    <td class="boxbold"><%=DS(0, "count_30dpd_12months") %></td>
                    <td class="boxbold"><%=DS(0, "count_30dpd_24months") %></td>
                </tr>
                <tr>
                    <td>Highest count of 60+DPD in the last X months for all credit facilities</td>
                    <td class="boxbold"><%=DS(0, "count_60dpd_06months") %></td>
                    <td class="boxbold"><%=DS(0, "count_60dpd_12months") %></td>
                    <td class="boxbold"><%=DS(0, "count_60dpd_24months") %></td>
                </tr>
                <tr>
                    <td>Highest count of 90+DPD in the last X months for all credit facilities</td>
                    <td class="boxbold"><%=DS(0, "count_90dpd_06months") %></td>
                    <td class="boxbold"><%=DS(0, "count_90dpd_12months") %></td>
                    <td class="boxbold"><%=DS(0, "count_90dpd_24months") %></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="40%" class="H1">&nbsp;</td>
                    <td width="20%" class="H1">Credit Limit</td>
                    <td width="20%" class="H1">Outstanding</td>
                    <td width="20%" class="H1">Count</td>
                </tr>
                <tr>
                    <td>Total Credit Card</td>
                    <td class="boxbold"><%=DS(0, "total_cc_limit") %></td>
                    <td class="boxbold"><%=DS(0, "total_cc_outst") %></td>
                    <td class="boxbold"><%=DS(0, "total_cc_count") %></td>
                </tr>
                <tr>
                    <td>Total Loan Other Than Credit Card</td>
                    <td class="boxbold"><%=DS(0, "total_unsec_limit") %></td>
                    <td class="boxbold"><%=DS(0, "total_unsec_outst") %></td>
                    <td class="boxbold"><%=DS(0, "total_unsec_count") %></td>
                </tr>
                <tr>
                    <td>Total Credit Facilities</td>
                    <td class="boxbold"><%=DS(0, "total_credit_limit") %></td>
                    <td class="boxbold"><%=DS(0, "total_credit_outst") %></td>
                    <td class="boxbold"><%=DS(0, "total_credit_count") %></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="40%" class="H1">&nbsp;</td>
                    <td width="20%" class="H1">Month(s)</td>
                    <td width="20%" class="H1">&nbsp;</td>
                    <td width="20%" class="H1">&nbsp;</td>
                </tr>
                <tr>
                    <td>Months Since Most Recent Trade Line Opened</td>
                    <td class="boxbold"><%=DS(0, "month_since_cc_opened") %></td>
                    <td >&nbsp;</td>
                    <td >&nbsp;</td>
                </tr>
                <tr>
                    <td>Months Since Recent 30+DPD Past Due</td>
                    <td class="boxbold"><%=DS(0, "month_since_30dpd_pastdue") %></td>
                    <td >&nbsp;</td>
                    <td >&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table width="100%" class="Box1">
                <tr>
                    <td width="50%" class="H1">Credit Card < 6 Months</td>
                    <td width="50%" class="H1">Credit Card >= 6 Months</td>
                </tr>
                <tr>
                    <td valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                        <asp:GridView ID="GRID_1" runat="server" AutoGenerateColumns="false" width="100%" CssClass="Dg1" ShowHeader="false">
                            <AlternatingRowStyle />
                            <RowStyle ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                <asp:BoundField DataField="bank_desc" HeaderText="bank_desc" ItemStyle-Width="60%" />
                                <asp:TemplateField HeaderText="Plafon" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# FormatedValue(Eval("plafon")) %>' />
                                    </ItemTemplate>      
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top" style="border-right:1px solid black;border-bottom:1px solid black;">
                        <asp:GridView ID="GRID_2" runat="server" AutoGenerateColumns="false" width="100%" CssClass="Dg1" ShowHeader="false">
                            <AlternatingRowStyle />
                            <RowStyle ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                <asp:BoundField DataField="bank_desc" HeaderText="bank_desc" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60%" />
                                <asp:TemplateField HeaderText="Plafon" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# FormatedValue(Eval("plafon")) %>' />
                                    </ItemTemplate>      
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="60%" align="right">High CC <= 6 Months :</td>
                                <td width="40%" class="boxbold"><%=DS(0, "high_cc_upto6months") %></td>
                            </tr>
                            <tr>
                                <td align="right">Low CC <= 6 Months :</td>
                                <td class="boxbold"><%=DS(0, "low_cc_upto6months")%></td>
                            </tr>
                            <tr>
                                <td align="left" class="boxbold">Credit Card <= 6 Months :</td>
                                <td class="boxbold"><%=DS(0, "count_cc_upto6months")%></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="60%" align="right">High CC > 6 Months :</td>
                                <td width="40%" class="boxbold"><%=DS(0, "high_cc_upper6months")%></td>
                            </tr>
                            <tr>
                                <td align="right">Low CC > 6 Months :</td>
                                <td class="boxbold"><%=DS(0, "low_cc_upper6months")%></td>
                            </tr>
                            <tr>
                                <td align="left" class="boxbold">Credit Card > 6 Months :</td>
                                <td class="boxbold"><%=DS(0, "count_cc_upper6months")%></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
	</form>
</body>
</html>
