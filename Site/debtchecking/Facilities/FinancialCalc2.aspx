<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialCalc2.aspx.cs" Inherits="DebtChecking.Facilities.FinancialCalc2" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
        oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="Content" class="Box1" width="1000" align="center">
    <tr>
        <td>
        
		<table id="DataDebitur" width="100%">
			<tr>
				<td class="H1" colspan="2">Data Debitur</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Nama</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:DropDownList ID="appid" runat="server" onchange="callback(mainPanel,'r:');"></asp:DropDownList></td>
			            </tr>
			            <tr>
			                <td class="B01">Status App</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="STATUS_APP" runat="server"></asp:Label><input type="hidden" runat="server" id="ID_REG" /></td>
			            </tr>
			            <tr>
			                <td class="B01">Tanggal Lahir</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="BORN_DATE" runat="server"></asp:Label></td>
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
			            <tr>
			                <td class="B01">No Telp / HP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="TELP_HP" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			</tr>
		</table>	
   
		<table id="DataKredit" width="100%" cellpadding="4" cellspacing="0" class="Box1">
			<tr>
				<td class="H1">CONSUMER LENDING DATA</td>
			</tr>
			<tr>
			    <td valign="top">
			        <table width="100%" cellpadding="0" cellspacing="0">
			            <tr>
				            <td><b><u><font size="2" color="red">Permata Loan History</font> </u></b></td>
			            </tr>
			            <tr>
				            <td><b>KARTU KREDIT</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridCCBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridCCBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridCCBank_Load" Font-Size="X-Small" oncustomcallback="GridCCBank_CustomCallback" 
                                    onafterperformcallback="GridCCBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LIMIT" FieldName="LIMIT" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="BALANCE" FieldName="BALANCE" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="MIN_PAYMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW1" runat="server" type="button" value="Add" onclick="callbackpopup(popupCCBank,panelCCBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridCCBank,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupCCBank,panelCCBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="MIN_PAYMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>PERSONAL LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridPLBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridPLBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridPLBank_Load" Font-Size="X-Small" oncustomcallback="GridPLBank_CustomCallback" 
                                    onafterperformcallback="GridPLBank_AfterPerformCallback">
                                    <Columns>   
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW2" runat="server" type="button" value="Add" onclick="callbackpopup(popupPLBank,panelPLBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL2" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridPLBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupPLBank,panelPLBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>JF CAR / MOTOR LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridCARBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridCARBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridCARBank_Load" Font-Size="X-Small" oncustomcallback="GridCARBank_CustomCallback" 
                                    onafterperformcallback="GridCARBank_AfterPerformCallback">
                                    <Columns>   
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW2" runat="server" type="button" value="Add" onclick="callbackpopup(popupCARBank,panelCARBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL2" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridCARBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupCARBank,panelCARBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>MORTGAGE LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridKPRBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridKPRBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridKPRBank_Load" Font-Size="X-Small" oncustomcallback="GridKPRBank_CustomCallback" 
                                    onafterperformcallback="GridKPRBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupKPRBank,panelKPRBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL3" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridKPRBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupKPRBank,panelKPRBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>TERM LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridKMKBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridKMKBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridKMKBank_Load" Font-Size="X-Small" oncustomcallback="GridKMKBank_CustomCallback" 
                                    onafterperformcallback="GridKMKBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupKMKBank,panelKMKBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL3" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridKMKBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupKMKBank,panelKMKBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>OVERDRAFT / REVOLVING LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridODRLBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridODRLBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridODRLBank_Load" Font-Size="X-Small" oncustomcallback="GridODRLBank_CustomCallback" 
                                    onafterperformcallback="GridODRLBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupODRLBank,panelODRLBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL3" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridODRLBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupODRLBank,panelODRLBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			        </table>
			        <table width="100%" cellpadding="0" cellspacing="0">
			            <tr>
				            <td><b><u><font size="2" color="red">Other Bank Loan History</font> </u></b></td>
			            </tr>
			            <tr>
				            <td><b>KARTU KREDIT</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridCCOtherBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridCCOtherBank" KeyFieldName="IDIKREDIT_ID" Font-Size="X-Small"
                                    oncustomcallback="GridCCOtherBank_CustomCallback" onafterperformcallback="GridCCOtherBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LIMIT" FieldName="LIMIT" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="BALANCE" FieldName="BALANCE" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="MIN_PAYMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupCCOtherBank,panelCCOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridCCOtherBank,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupCCOtherBank,panelCCOtherBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="MIN_PAYMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>PERSONAL LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridPLOtherBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridPLOtherBank" KeyFieldName="IDIKREDIT_ID" Font-Size="X-Small"
                                    oncustomcallback="GridPLOtherBank_CustomCallback" onafterperformcallback="GridPLOtherBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupPLOtherBank,panelPLOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridPLOtherBank,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupPLOtherBank,panelPLOtherBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>AUTO / HOME LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridConsOtherBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridConsOtherBank" KeyFieldName="IDIKREDIT_ID" Font-Size="X-Small"
                                    oncustomcallback="GridConsOtherBank_CustomCallback" onafterperformcallback="GridConsOtherBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupConsOtherBank,panelConsOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridConsOtherBank,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupConsOtherBank,panelConsOtherBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>TERM LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridKMKOtherBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridKMKOtherBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridKMKOtherBank_Load" Font-Size="X-Small" oncustomcallback="GridKMKOtherBank_CustomCallback" 
                                    onafterperformcallback="GridKMKOtherBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupKMKOtherBank,panelKMKOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL3" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridKMKOtherBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupKMKOtherBank,panelKMKOtherBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>OVERDRAFT / REVOLVING LOAN</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridODRLOtherBank" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridODRLOtherBank" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridODRLOtherBank_Load" Font-Size="X-Small" oncustomcallback="GridODRLOtherBank_CustomCallback" 
                                    onafterperformcallback="GridODRLOtherBank_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="PLAFOND" FieldName="PLAFOND" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="TENOR" FieldName="TENOR" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="INTEREST" >
                                            <PropertiesTextEdit DisplayFormatString="{0}%"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MONTHLY OBLIGATION" FieldName="INSTALLMENT" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="KOLEKTABILITAS" FieldName="KOLEKTABILITAS" Width="60" CellStyle-HorizontalAlign="Center"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupODRLOtherBank,panelODRLOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL3" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridODRLOtherBank,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupODRLOtherBank,panelODRLOtherBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="INSTALLMENT" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			        </table>
			    </td>
			</tr>
			<tr>
			    <td bgcolor="#f5de8c">
			        <table width="100%" cellpadding="0" cellspacing="0">
			            <tr>
			                <td width="50%">
			                    <table width="100%" cellpadding="0" cellspacing="0">
			                        <tr>
			                            <td align="left"><b>TOTAL MONTHLY OBLIGATION INTERNAL</b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="TML_INTERNAL" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                        <tr>
			                            <td align="left"><b>TOTAL MONTHLY OBLIGATION EXTERNAL</b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="TML_EXTERNAL" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                        <tr>
			                            <td align="left"><b>TOTAL</b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="TML" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                        <tr>
			                            <td align="left"><b>INCOME</b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="INCOME_RECOGNATION" runat="server" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                    </table>
			                </td>
			                <td width="50%">
			                    <table width="100%" cellpadding="0" cellspacing="0">
			                        <tr>
			                            <td align="right"><b><font color="red">TOTAL MONTHLY OBLIGATION INTERNAL (Konsolidasi)</font></b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="TML_KONSOL_INTERNAL" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                        <tr>
			                            <td align="right"><b><font color="red">TOTAL MONTHLY OBLIGATION EXTERNAL (Konsolidasi)</font></b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="TML_KONSOL_EXTERNAL" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                        <tr>
			                            <td align="right"><b><font color="red">TOTAL MONTHLY OBLIGATION (Konsolidasi)</font></b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="TML_KONSOL_TOTAL" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                        <tr>
			                            <td align="right"><b><font color="red">DBR (%)</font></b></td>
			                            <td width="30%" align="right"><cc1:TXT_CURRENCY style="text-align:right" ID="PRC_DBR" runat="server" ReadOnly = "true" BackColor="#F9E2CB" Width="90%"></cc1:TXT_CURRENCY></td>
			                        </tr>
			                    </table>
			                </td>
			            </tr>
			        </table>
			    </td>
			</tr>
		</table>		
     
        <table width="100%">
            <tr>
                <td align="center">
                    <input runat="server" ID="BTN_SAVE1" type="button" class="Bt1" onclick="callback(mainPanel,'s:');" value="Save & Recalculate" />
                </td>
            </tr>
        </table>	
      
		</td>
    </tr>
    </table>
	</dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    
    <dxpc:ASPxPopupControl ID="popupCCBank" ClientInstanceName="popupCCBank" runat="server" 
            HeaderText="Input Lending Data Kartu Kredit Permata" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelCCBank" runat="server" ClientInstanceName="panelCCBank" OnCallback="panelCCBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent3" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Limit</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="limit_ccbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_ccbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Balance</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="balance_ccbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="btnsave1" type="button" class="Bt1" onclick="callbackpopup(popupCCBank,panelCCBank,'s:',GridCCBank)" value=" Save " />
                        <input ID="BTN_CANCEL1" runat="server" class="Bt1" onclick="popupCCBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupPLBank" ClientInstanceName="popupPLBank" runat="server" 
            HeaderText="Input Lending Data PL Permata" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl4" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelPLBank" runat="server" ClientInstanceName="panelPLBank" OnCallback="panelPLBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_plbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_plbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_plbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_plbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button5" type="button" class="Bt1" onclick="callbackpopup(popupPLBank,panelPLBank,'s:',GridPLBank)" value=" Save " />
                        <input ID="Button6" runat="server" class="Bt1" onclick="popupPLBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupCARBank" ClientInstanceName="popupCARBank" runat="server" 
            HeaderText="Input Lending Data JF CAR/MOTOR Permata" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl2" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelCARBank" runat="server" ClientInstanceName="panelCARBank" OnCallback="panelCARBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent4" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_carbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_carbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_carbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_carbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button1" type="button" class="Bt1" onclick="callbackpopup(popupCARBank,panelCARBank,'s:',GridCARBank)" value=" Save " />
                        <input ID="Button2" runat="server" class="Bt1" onclick="popupCARBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupKPRBank" ClientInstanceName="popupKPRBank" runat="server" 
            HeaderText="Input Lending Data KPR Permata" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelKPRBank" runat="server" ClientInstanceName="panelKPRBank" OnCallback="panelKPRBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent5" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_kprbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_kprbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_kprbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_kprbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button3" type="button" class="Bt1" onclick="callbackpopup(popupKPRBank,panelKPRBank,'s:',GridKPRBank)" value=" Save " />
                        <input ID="Button4" runat="server" class="Bt1" onclick="popupKPRBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupKMKBank" ClientInstanceName="popupKMKBank" runat="server" 
            HeaderText="Input Lending Data TL Permata" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl5" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelKMKBank" runat="server" ClientInstanceName="panelKMKBank" OnCallback="panelKMKBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent6" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_kmkbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_kmkbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_kmkbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_kmkbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button7" type="button" class="Bt1" onclick="callbackpopup(popupKMKBank,panelKMKBank,'s:',GridKMKBank)" value=" Save " />
                        <input ID="Button8" runat="server" class="Bt1" onclick="popupKMKBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupODRLBank" ClientInstanceName="popupODRLBank" runat="server" 
            HeaderText="Input Lending Data OD/RL Permata" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl9" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelODRLBank" runat="server" ClientInstanceName="panelODRLBank" OnCallback="panelODRLBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent10" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_odrlbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_odrlbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_odrlbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_odrlbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button15" type="button" class="Bt1" onclick="callbackpopup(popupODRLBank,panelODRLBank,'s:',GridODRLBank)" value=" Save " />
                        <input ID="Button16" runat="server" class="Bt1" onclick="popupODRLBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupCCOtherBank" ClientInstanceName="popupCCOtherBank" runat="server" 
            HeaderText="Edit Lending Data Kartu Kredit Bank Lain" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl6" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelCCOtherBank" runat="server" ClientInstanceName="panelCCOtherBank" OnCallback="panelCCOtherBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent7" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Limit</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="limit_ccothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_ccothbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Balance</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="balance_ccothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button9" type="button" class="Bt1" onclick="callbackpopup(popupCCOtherBank,panelCCOtherBank,'s:',GridCCOtherBank)" value=" Save " />
                        <input ID="Button10" runat="server" class="Bt1" onclick="popupCCOtherBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupPLOtherBank" ClientInstanceName="popupPLOtherBank" runat="server" 
            HeaderText="Edit Lending Data PL Bank Lain" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl7" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelPLOtherBank" runat="server" ClientInstanceName="panelPLOtherBank" OnCallback="panelPLOtherBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent8" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_plothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_plothbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_plothbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_plothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button11" type="button" class="Bt1" onclick="callbackpopup(popupPLOtherBank,panelPLOtherBank,'s:',GridPLOtherBank)" value=" Save " />
                        <input ID="Button12" runat="server" class="Bt1" onclick="popupPLOtherBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupConsOtherBank" ClientInstanceName="popupConsOtherBank" runat="server" 
            HeaderText="Edit Lending Data AUTO/HOME Loan Bank Lain" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl8" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelConsOtherBank" runat="server" ClientInstanceName="panelConsOtherBank" OnCallback="panelConsOtherBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent9" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_consothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_consothbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_consothbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_consothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button13" type="button" class="Bt1" onclick="callbackpopup(popupConsOtherBank,panelConsOtherBank,'s:',GridConsOtherBank)" value=" Save " />
                        <input ID="Button14" runat="server" class="Bt1" onclick="popupConsOtherBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupKMKOtherBank" ClientInstanceName="popupKMKOtherBank" runat="server" 
            HeaderText="Edit Lending Data TL Bank Lain" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl10" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelKMKOtherBank" runat="server" ClientInstanceName="panelKMKOtherBank" OnCallback="panelKMKOtherBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent11" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_kmkothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_kmkothbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_kmkothbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_kmkothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button17" type="button" class="Bt1" onclick="callbackpopup(popupKMKOtherBank,panelKMKOtherBank,'s:',GridKMKOtherBank)" value=" Save " />
                        <input ID="Button18" runat="server" class="Bt1" onclick="popupKMKOtherBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="popupODRLOtherBank" ClientInstanceName="popupODRLOtherBank" runat="server" 
            HeaderText="Edit Lending Data OD/RL Bank Lain" width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection>
     <dxpc:PopupControlContentControl ID="PopupControlContentControl11" runat="server" Height="100%">
        <dxcp:ASPxCallbackPanel ID="panelODRLOtherBank" runat="server" ClientInstanceName="panelODRLOtherBank" OnCallback="panelODRLOtherBank_Callback">
        <PanelCollection><dxp:PanelContent ID="PanelContent12" runat="server">
            <table width="100%" class="Box1">
                <tr>
                    <td class="B01">Plafond</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="plafond_odrlothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY>
                    <input type="hidden" runat="server" id="seq_odrlothbank" />
                    </td>
                </tr>
                <tr>
                    <td class="B01">Tenor</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_NUMBER ID="tenor_odrlothbank" runat="server" CssClas="mandatory"></cc1:TXT_NUMBER> Bulan</td>
                </tr>
                <tr>
                    <td class="B01">Annual Interest</td>
                    <td class="BS">:</td>
                    <td class="B11"><cc1:TXT_CURRENCY ID="interest_odrlothbank" runat="server" CssClas="mandatory"></cc1:TXT_CURRENCY> %</td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <input runat="server" ID="Button19" type="button" class="Bt1" onclick="callbackpopup(popupODRLOtherBank,panelODRLOtherBank,'s:',GridODRLOtherBank)" value=" Save " />
                        <input ID="Button20" runat="server" class="Bt1" onclick="popupODRLOtherBank.Hide();" type="button" value="Cancel" />
                    </td>
                </tr>
            </table>
        </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl></ContentCollection></dxpc:ASPxPopupControl>
    
    </form>
</body>
</html>
