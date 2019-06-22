<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomCreditSummary.aspx.cs" Inherits="DebtChecking.Facilities.CustomCreditSummary" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SID Text Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" ClientInstanceName="mainPanel">
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="Content" class="Box1" width="100%" align="center">
    <tr>
        <td>
		<table id="DataDebitur" width="100%">
            <tr>
                <td colspan="3">
                    Select other customer in this requestid <asp:DropDownList ID="ddl_appid" runat="server" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
			<tr>
				<td class="H1" colspan="3">Data Debitur</td>
			</tr>
			<tr valign="top">
			    <td width="30%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Customer Name</td>
			                <td class="BS">:</td>
			                <td class="B11"><div style="font-size:medium;font-weight:bold"><%=DS(0, "cust_name") %>
                                <input type="hidden" id="appid" runat="server" />
			                                </div>
			                </td>
			            </tr>
                        <tr>
			                <td class="B01">NIK/NPWP</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "idnumber") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Request Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "request_date") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Request Time</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "request_time") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Request No.</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "reffnumber") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Worst Condition</td>
			                <td class="BS">:</td>
			                <td class="B11"><%=DS(0, "worst_collect") %> on <%=DS(0, "worst_collect_date") %></td>
			            </tr>
			        </table>
			    </td>
			    <td width="30%">
			        <table class="Tbl0" width="100%" style="border:1px solid #ccc;border-collapse:collapse">
                        <tr runat="server">
			                <td class="B11"></td>
                            <td class="B01">Active Plafond</td>
			                <td class="B01">Outstanding</td>
			            </tr>
                        <tr id="Tr1" runat="server">
			                <td class="B01">Credit</td>
			                <td class="B12"><%=DS(0, "total_plafon_credit_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_credit_active") %></td>
			            </tr>
			            <tr>
			                <td class="B01">LC</td>
			                <td class="B12"><%=DS(0, "total_plafon_lc_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_lc_active") %></td>
			            </tr>
			            <tr runat="server">
			                <td class="B01">Guarantee</td>
			                <td class="B12"><%=DS(0, "total_plafon_guarantee_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_guarantee_active") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Securities</td>
			                <td class="B12"><%=DS(0, "total_plafon_securities_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_securities_active") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Other</td>
			                <td class="B12"><%=DS(0, "total_plafon_other_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_other_active") %></td>
			            </tr>
                        <tr>
			                <td class="B01"><strong>Total</strong></td>
			                <td class="B12"><%=DS(0, "total_plafon_all_fac_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_all_fac_active") %></td>
			            </tr>
			        </table>
			    </td>
                <td width="30%">
                    <table class="Tbl0" width="100%">
                        <tr runat="server">
			                <td class="B11"></td>
                            <td class="B01">Active Plafond</td>
			                <td class="B01">Outstanding</td>
                            <td class="B01">Unused</td>
			            </tr>
                        <tr id="Tr2" runat="server">
			                <td class="B01">Working Capital</td>
			                <td class="B12"><%=DS(0, "total_plafon_wc_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_wc_active") %></td>
                            <td class="B12"><%=DS(0, "total_unused_wc_active") %></td>
			            </tr>
			            <tr>
			                <td class="B01">Investment</td>
			                <td class="B12"><%=DS(0, "total_plafon_invest_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_invest_active") %></td>
                            <td class="B12"><%=DS(0, "total_unused_invest_active") %></td>
			            </tr>
			            <tr runat="server">
			                <td class="B01">Consumption</td>
			                <td class="B12"><%=DS(0, "total_plafon_cf_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_cf_active") %></td>
                            <td class="B12"><%=DS(0, "total_unused_cf_active") %></td>
			            </tr>
                        <tr>
			                <td class="B01">Total Credit</td>
			                <td class="B12"><%=DS(0, "total_plafon_credit_active") %></td>
                            <td class="B12"><%=DS(0, "total_os_credit_active") %></td>
                            <td class="B12"><%=DS(0, "total_unused_credit_active") %></td>
			            </tr>
			        </table>
                </td>
			</tr>
            <tr>
                <td align="center" colspan="3">
                    <input type="button" id="btnprint" runat="server" class="btn btn-xs btn-success" value="Print" onclick="this.style.display = 'none'; document.getElementById('mainPanel_btnpdf').style.display = 'none'; window.print(); this.style.display = ''; document.getElementById('mainPanel_btnpdf').style.display = '';" />
                    <input type="button" id="btnpdf" value="Save As PDF" class="btn btn-xs btn-success" runat="server" onclick="callback(pdfPanel, '', false, null)" />
                    <asp:Button ID="btnExport" runat="server" class="btn btn-xs btn-success" Text="Save As XLS" OnClick="btnExport_Click" />
                    <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
                        oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
                        <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
                        <input type="hidden" id="urlframe" runat="server" />
                    </dxp:PanelContent></PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
		</table>	
		<table id="DataKredit" width="100%">
			<tr>
				<td class="H1">Summary Kredit</td>
			</tr>
			<tr>
			    <td>
			        <dxwgv:ASPxGridView ID="GridViewKREDIT" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewKREDIT" KeyFieldName="fasilitasid" Font-Size="X-Small"
                        OnLoad="GridViewKREDIT_Load" >
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Name" FieldName="cust_name" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Financiers" FieldName="financiers" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Type" FieldName="fac_type" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usage" FieldName="Usage" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Int (%)" FieldName="interest">
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Typ" FieldName="Int_type" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Status" FieldName="Status" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curr" FieldName="Currency" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Active Plfnd" FieldName="plafon" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Outstanding" FieldName="Outstanding" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Unused" FieldName="Unused">
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Start" FieldName="Date_Start" >
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="End" FieldName="Date_end" >
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tenor" FieldName="tenor" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Remain" FieldName="remain" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Max CL" FieldName="MAX_CL" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Max OD" FieldName="MAX_OD" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Last CL" FieldName="LAST_CL" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Last OD" FieldName="LAST_OD" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M1" FieldName="M1" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M2" FieldName="M2" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M3" FieldName="M3" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M4" FieldName="M4" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M5" FieldName="M5" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M6" FieldName="M6" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M7" FieldName="M7" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M8" FieldName="M8" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M9" FieldName="M9" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M10" FieldName="M10" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M11" FieldName="M11" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="M12" FieldName="M12" ></dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" />
                        <Settings ShowFilterRow="false" ShowGroupPanel="true" ShowGroupedColumns="true" ShowFooter="false" ShowPreview="true" />
                        <SettingsBehavior AllowGroup="true" />
                    </dxwgv:ASPxGridView>
                    <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="GridViewKREDIT"></dxwgv:ASPxGridViewExporter>
			    </td>
			</tr>
		</table>
		</td>
    </tr>
    </table>
    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>

    </div>
    
    </form>
</body>
</html>
