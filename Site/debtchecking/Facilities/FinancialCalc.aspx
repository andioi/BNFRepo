<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialCalc.aspx.cs" Inherits="DebtChecking.Facilities.FinancialCalc" %>
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
	<script src="../include/UC_Currency.js" language="javascript"></script>
	<script src="../include/calc.js" language="javascript"></script>
	<script src="../include/cek_entries.js" language="javascript"></script>
    <script language="javascript" type="text/javascript">
        function download(href) {
            var X = (screen.availWidth - 800) / 2;
            var Y = (screen.availHeight - 600) / 2;
            window.open(href, "", "height=600px,width=800px,left=" + X + ",top=" + Y +
			",status=no,toolbar=no,scrollbars=yes,resizable=yes,titlebar=no,menubar=no,location=no,dependent=yes");
        }
        function calc_total_income() {
            var monthly_income = parseFloat(clearValue(document.form1.mainPanel$monthly_income.value, ',', '.'));
            var join_income = parseFloat(clearValue(document.form1.mainPanel$join_income.value, ',', '.'));
            total_income = monthly_income + join_income;
            document.form1.mainPanel$total_income.value = jsmoneyformat(total_income, ',', '.', 0);
            if (document.form1.h_final_rec.value == "FAIL") {
                final_rec.style.backgroundColor = "red";
            }
            if (document.form1.h_final_rec.value == "PASS") {
                final_rec.style.backgroundColor = "green";
            }
            if (document.form1.h_final_rec.value == "HIGHER AUTHORITY") {
                final_rec.style.backgroundColor = "orange";
            }
        }
    </script>
    <style type="text/css">
        .boxbold { border:1px solid black; text-align:right; }
        .boxboldleft { border:1px solid black; text-align:left; }
        .boxboldcenter { border:1px solid black; text-align:center; font-size:medium; }
    </style>
</head>
<body onload="calc_total_income();">
    <form id="form1" runat="server">
        <table width="25%" class="Box1" cellpadding="2" cellspacing="0">
			<tr>
				<td class="B01">NO APLIKASI</td>
				<td class="BS">:</td>
				<td class="B11"><asp:TextBox ID="reffnumber" BackColor="#F9E2CB" runat="server" Width="120" CssClass="mandatory" onblur="callback(mainPanel,'load',false)"></asp:TextBox> </td>
			</tr>
        </table>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
        oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
        <ClientSideEvents EndCallback="function(s,e) { calc_total_income(); }" />
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="Content" class="Box1" width="100%" align="center">
    <tr>
        <td>
		<table id="DataApplication" width="100%" cellpadding="4" cellspacing="0" class="Box1">
			<tr>
			    <td width="50%" valign="top">
			        <table width="97%" cellpadding="0" cellspacing="0">
                        <tr>
				            <td class="B01">PRODUCT</td>
				            <td class="BS">:</td>
				            <td class="B11"><asp:DropDownList ID="product" runat="server" Width="97%" CssClass="mandatory"
                                onchange="document.form1.mainPanel$h_product.value=document.form1.mainPanel$product.value;callback(panelProgram,'',false);"></asp:DropDownList>
                                <input type="hidden" id="h_product" value="" runat="server" />
				            </td>
			            </tr>
			            <tr>
				            <td class="B01">BRANCH / AO</td>
				            <td class="BS">:</td>
				            <td class="B11"><asp:TextBox ID="msc_code" runat="server" Width="97%"></asp:TextBox> </td>
			            </tr>
			        </table>
			    </td>
                <td width="50%" valign="top">
                    <table width="97%" cellpadding="0" cellspacing="0">
                        <tr>
				            <td class="B01">DATE</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:CC_Date ID="fincal_date" runat="server" CssClass="mandatory" ImgShown="false"></cc1:CC_Date></td>
			            </tr>
			            <tr>
				            <td class="B01">PROGRAM</td>
				            <td class="BS">:</td>
				            <td class="B11">
                                <dxcp:ASPxCallbackPanel ID="panelProgram" ClientInstanceName="panelProgram" runat="server" Width="100%" 
                                        oncallback="panelProgram_Callback">
                                <PanelCollection>
                                <dxp:PanelContent ID="PanelContentprogram" runat="server">
                                    <asp:DropDownList ID="program" runat="server" CssClass="mandatory" Width="99%" onchange="document.form1.mainPanel$panelProgram$h_program.value=document.form1.mainPanel$panelProgram$program.value;"></asp:DropDownList>
                                    <input type="hidden" id="h_program" value="" runat="server" />
                                </dxp:PanelContent>
                                </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
				            </td>
			            </tr>
                    </table>
                </td>
			</tr>
        </table>
        <table width="100%" cellpadding="4" cellspacing="0" class="Box1">
            <tr>
				<td class="H1" colspan="2">APPLICATION DATA</td>
			</tr>
            <tr>
                <td width="50%" valign="top">
                    <table width="97%" cellpadding="0" cellspacing="0">
			            <tr>
				            <td class="B01">APPLICANT NAME</td>
				            <td class="BS">:</td>
				            <td class="B11"><asp:Label ID="appl_name" Font-Bold="true" Font-Size="Medium" runat="server"></asp:Label>
                                <input type="hidden" id="h_appl_name" runat="server" />
				            </td>
			            </tr>
                        <tr>
				            <td class="B01">PLAFOND PENGAJUAN</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="limit" runat="server" Width="97%"></cc1:TXT_CURRENCY> </td>
			            </tr>
                        <tr style="display:none">
                            <td class="B01" width="200">AUM</td>
				            <td class="BS">:</td>
				            <td class="B11">
                                <table width="100%" cellpadding="0" cellspacing="0" style="float:">
                                    <tr> 
                                        <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="aum" runat="server"></cc1:TXT_CURRENCY></td>
                                        <td class="B01">MOB AUM</td>
				                        <td class="BS">:</td>
				                        <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="mob_aum" Width="90" runat="server"></cc1:TXT_CURRENCY>BULAN</td>
                                    </tr>
                                </table>
				            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%" valign="top">
                    <table width="97%" cellpadding="0" cellspacing="0">
                        <tr>
				            <td class="B01">TENOR</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="tenor" runat="server"></cc1:TXT_CURRENCY> BULAN</td>
			            </tr>
			            <tr>
				            <td class="B01">INTEREST (eff p.a)</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="rate" runat="server"></cc1:TXT_CURRENCY> %</td>
			            </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <input runat="server" ID="btnsave1" type="button" class="Bt1" onclick="callback(mainPanel,'save',true);" value=" Save " />
                    <input id="btndownload1" runat="server" type="button" value="Export to Excel" class="Bt1" /></td>
            </tr>
        </table>
    
        <table id="DataEvaluation" width="100%" cellpadding="4" cellspacing="0" class="Box1">
			<tr>
				<td class="H1" colspan="3">Income Expense & Calculation</td>
			</tr>
			<tr>
			    <td width="33%" valign="top">
			        <table width="97%">
                        <tr>
				            <td class="B01">MONTHLY INCOME</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="monthly_income" runat="server" Width="97%" onblur="calc_total_income();"></cc1:TXT_CURRENCY> </td>
			            </tr>
			            <tr style="display:none">
				            <td class="B01">YEARLY INCOME / SURROGATE INCOME</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="yearly_income" ReadOnly = "true" BackColor="#F9E2CB" runat="server" Width="97%"></cc1:TXT_CURRENCY> </td>
			            </tr>
                        <tr>
				            <td class="B01">JOIN INCOME</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="join_income" runat="server" Width="97%" onblur="calc_total_income();"></cc1:TXT_CURRENCY> </td>
			            </tr>
			            <tr>
				            <td class="B01">TOTAL INCOME</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="total_income" ReadOnly = "true" BackColor="#F9E2CB" runat="server" Width="97%"></cc1:TXT_CURRENCY></td>
			            </tr>
			            <tr>
				            <td class="B01">ESTIMATED COST LIVING</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "estimated_cost") %></td>
			            </tr>
			            <tr>
				            <td class="B01"><b><font color="red">MAXIMUM DBR</font></b></td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "max_dbr1","n0") %>%</td>
			            </tr>
			        </table>
			    </td>
			    <td width="33%" valign="top">
			        <table width="97%">
			            <tr>
				            <td class="B01">Total Monthly Installment (SLIK CHECKING)</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "tot_monthly_installment", "n0") %></td>
			            </tr>
			            <tr>
				            <td class="B01">NEW INSTALLMENT</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "installment") %></td>
			            </tr>
                        <tr>
				            <td class="B01">OTHER INSTALLMENT (Not In SLIK checking)</td>
				            <td class="BS">:</td>
				            <td class="B11"><cc1:TXT_CURRENCY style="text-align:right" ID="other_installment" runat="server" Width="97%"></cc1:TXT_CURRENCY> </td>
			            </tr>
			            <tr>
				            <td class="B01">TOTAL INSTALLMENT</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "total_installment") %></td>
			            </tr>
                        <tr>
				            <td class="B01">DBR ACTUAL</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "dbr_actual") %>%</td>
			            </tr>
			        </table>
			    </td>
                <td width="34%" valign="top">
			        <table width="97%">
			            <tr>
				            <td class="B01">Recommended Max. Plafond</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "proposal_limit", "n0") %></td>
			            </tr>
			            <tr>
				            <td class="B01">Recommended Max. Installment</td>
				            <td class="BS">:</td>
				            <td class="boxbold" bgcolor="#F9E2CB"><%=DS(1, "max_installment") %></td>
			            </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
				            <td class="B01">(PASS/FAIL)</td>
				            <td class="BS">:</td>
				            <td class="boxbold" id="passfail" bgcolor="#F9E2CB"><%=DS(1, "incomeexpense_result") %>
                                <input type="hidden" id="h_passfail" value="<%=DS(1, "incomeexpense_result") %>" />
				            </td>
			            </tr>
                        <tr>
				            <td class="B01"><b>FINAL RECOMMENDATION</b></td>
				            <td class="BS">:</td>
				            <td class="boxbold" id="final_rec" bgcolor="#F9E2CB"><%=DS(1, "final_recommendation") %>
                                <input type="hidden" id="h_final_rec" value="<%=DS(1, "final_recommendation") %>" />
				            </td>
			            </tr>
                    </table>
                </td>
			</tr>
        </table>

		<table id="DataKredit" width="100%" cellpadding="4" cellspacing="0" class="Box1">
			<tr>
				<td class="H1">LENDING DATA</td>
			</tr>
			<tr>
			    <td valign="top">
			        <table width="100%" cellpadding="0" cellspacing="0">
			            <tr>
				            <td><b>CREDIT CARD</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridCC" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridCC" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridCC_Load" Font-Size="X-Small" oncustomcallback="GridCC_CustomCallback" 
                                    onafterperformcallback="GridCC_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Condition Remarks" FieldName="ket_kondisi"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nature Of Facility" FieldName="sifat"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Coll" FieldName="latest_status"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LIMIT" FieldName="plafond" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MOB (MONTH)" FieldName="tenor"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="BALANCE" FieldName="baki_debet" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MINIMUN PAYMENT / INSTALLMENT" FieldName="installment" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%" Visible="false">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW1" runat="server" type="button" value="Add" onclick="callbackpopup(popupCC,panelCC,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridCC,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupCC,panelCC,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="installment" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>UNSECURED LOAN (INSTALLMENT) </b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridUnsecured" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridUnsecured" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridUnsecured_Load" Font-Size="X-Small" oncustomcallback="GridUnsecured_CustomCallback" 
                                    onafterperformcallback="GridUnsecured_AfterPerformCallback">
                                    <Columns>   
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Condition Remarks" FieldName="ket_kondisi"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nature Of Facility" FieldName="sifat"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Coll" FieldName="latest_status"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="OS" FieldName="baki_debet" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LEFT TENOR (MONTH)*" FieldName="tenor" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="annual_interest" >
                                            <PropertiesTextEdit DisplayFormatString="{0:p}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="INSTALLMENT" FieldName="installment" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%" Visible="false">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW2" runat="server" type="button" value="Add" onclick="callbackpopup(popupPLBank,panelPLBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL2" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridUnsecured,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupPLBank,panelPLBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="installment" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>SECURED LOAN (INSTALLMENT)</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridSecured" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridSecured" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridSecured_Load" Font-Size="X-Small" oncustomcallback="GridSecured_CustomCallback" 
                                    onafterperformcallback="GridSecured_AfterPerformCallback">
                                    <Columns>   
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Condition Remarks" FieldName="ket_kondisi"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nature Of Facility" FieldName="sifat"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Coll" FieldName="latest_status"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="OS" FieldName="baki_debet" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LEFT TENOR (MONTH)*" FieldName="tenor" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="annual_interest" >
                                            <PropertiesTextEdit DisplayFormatString="{0:p}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="INSTALLMENT" FieldName="installment" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%" Visible="false">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW2" runat="server" type="button" value="Add" onclick="callbackpopup(popupCARBank,panelCARBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL2" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridSecured,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupCARBank,panelCARBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="installment" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			            <tr>
				            <td><b>MODAL KERJA LAINNYA (INSTALLMENT)</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridMK" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridMK" KeyFieldName="IDIKREDIT_ID" 
                                    OnLoad="GridMK_Load" Font-Size="X-Small" oncustomcallback="GridMK_CustomCallback" 
                                    onafterperformcallback="GridMK_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Condition Remarks" FieldName="ket_kondisi"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nature Of Facility" FieldName="sifat"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Coll" FieldName="latest_status"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="OS" FieldName="baki_debet" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LEFT TENOR (MONTH)*" FieldName="tenor" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="annual_interest" >
                                            <PropertiesTextEdit DisplayFormatString="{0:p}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="INSTALLMENT" FieldName="installment" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%" Visible="false">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupKPRBank,panelKPRBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL3" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridMK,'d:'+this.commandargument)"  disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupKPRBank,panelKPRBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="installment" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			        </table>
			        <table width="100%" cellpadding="0" cellspacing="0">
			            <tr>
				            <td><b>INVESTASI LAINNYA</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridInvest" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridInvest" KeyFieldName="IDIKREDIT_ID" Font-Size="X-Small"
                                    oncustomcallback="GridInvest_CustomCallback" onafterperformcallback="GridInvest_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Condition Remarks" FieldName="ket_kondisi"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nature Of Facility" FieldName="sifat"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Coll" FieldName="latest_status"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="OS" FieldName="baki_debet" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LEFT TENOR (MONTH)*" FieldName="tenor" >
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="annual_interest" >
                                            <PropertiesTextEdit DisplayFormatString="{0:p}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="INSTALLMENT" FieldName="installment" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%" Visible="false">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupCCOtherBank,panelCCOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridInvest,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
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
				            <td><b>MODAL KERJA LAINNYA (RK)</b></td>
			            </tr>
			            <tr>
			                <td valign="top">
			                    <dxwgv:ASPxGridView ID="GridRK" runat="server" Width="100%" AutoGenerateColumns="False" 
                                    ClientInstanceName="GridRK" KeyFieldName="IDIKREDIT_ID" Font-Size="X-Small"
                                    oncustomcallback="GridRK_CustomCallback" onafterperformcallback="GridRK_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="BANK" FieldName="bank_name" ></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Condition Remarks" FieldName="ket_kondisi"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nature Of Facility" FieldName="sifat"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Coll" FieldName="latest_status"></dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="OS" FieldName="baki_debet" Visible="false">
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="LEFT TENOR (MONTH)*" FieldName="tenor" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ANNUAL INTEREST" FieldName="annual_interest" >
                                            <PropertiesTextEdit DisplayFormatString="{0:p}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="INSTALLMENT" FieldName="installment" Width="150" >
                                            <PropertiesTextEdit DisplayFormatString="{0:###,##0}"></PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Width="15%" Visible="false">
                                            <HeaderTemplate>
                                                <input id="BTN_NEW3" runat="server" type="button" value="Add" onclick="callbackpopup(popupPLOtherBank,panelPLOtherBank,'r:')" visible="<%# isActive %>" />
                                            </HeaderTemplate>
                                            <DataItemTemplate>
                                                <input id="BTN_DEL1" runat="server" type="button" value="Delete" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callback(GridRK,'d:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                                <input id="Button5" runat="server" type="button" value="Edit" commandargument="<%# Container.KeyValue %>" 
                                                onclick="callbackpopup(popupPLOtherBank,panelPLOtherBank,'r:'+this.commandargument)" disabled=<%# Eval("notdeleteble") %> />
                                            </DataItemTemplate>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="100" />
                                    <Settings ShowFooter="True" />
                                    <TotalSummary>
                                        <dxwgv:ASPxSummaryItem FieldName="installment" SummaryType="Sum" DisplayFormat="Total= {0:###,##0.##}" />
                                    </TotalSummary>
                                </dxwgv:ASPxGridView>
			                </td>
			            </tr>
			        </table>
			    </td>
			</tr>
			<tr>
			    <td bgcolor="#f5de8c">
			        <table width="100%">
			            <tr>
			                <td width="30%" align="center"><b>INTERNAL UNSECURED EXPOSURE</b></td>
			                <td width="30%" align="center"><b>TOTAL UNSECURED EXPOSURED</b></td>
			                <td width="30%" align="center"><b>TOTAL MONTHLY INSTALLMENT</b></td>
			            </tr>
			            <tr>
			                <td width="30%" class="boxboldcenter"><b><%=DS(1, "iue") %></b></td>
			                <td width="30%" class="boxboldcenter"><b><%=DS(1, "tue") %></b></td>
			                <td width="30%" class="boxboldcenter"><b><%=DS(1, "tot_monthly_installment") %></b></td>
			            </tr>
			        </table>
			    </td>
			</tr>
            <tr>
                <td align="center" colspan="2">
                    <input runat="server" ID="btnsave2" type="button" class="Bt1" onclick="callback(mainPanel,'save',true);" value=" Save " />
                    <input id="btndownload2" runat="server" type="button" value="Export to Excel" onclick="callback(mainPanel,'export_excel',true);" class="Bt1" />
                </td>
            </tr>
		</table>
		
        <table width="100%" class="Box1" cellpadding="4" cellspacing="0">
            <tr>
				<td class="H1">SLIK CHECKING SUMMARY</td>
			</tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td width="35%" align="left"><b><u>CREDIT CARD FACILITIES</u></b></td>
                            <td width="15%" class="H1">COLL.</td>
                            <td width="15%" class="H1">DPD</td>
                            <td width="15%" class="H1">OS</td>
                            <td width="20%" class="H1">BANK</td>
                        </tr>
                        <tr>
                            <td>Worst latest collectibility with OS > 1,000,000</td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_coll_cc") %></td>
                            <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_latest_dpd_cc") %></td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_os_cc") %></td>
                            <td class="boxboldleft">&nbsp;<%=DS(0, "worst_latest_bank_cc") %></td>
                        </tr>
                        <tr>
                            <td>Worst collectibility with OS > 1,000,000</td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_coll_cc") %></td>
                            <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_dpd_cc") %></td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_os_cc") %></td>
                            <td class="boxboldleft">&nbsp;<%=DS(0, "worst_bank_cc") %></td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td width="35%">&nbsp;</td>
                            <td width="15%" class="H1">3 MONTHS</td>
                            <td width="15%" class="H1">6 MONTHS</td>
                            <td width="15%" class="H1">12 MONTHS</td>
                            <td width="20%" class="H1">24 MONTHS</td>
                        </tr>
                        <tr>
                            <td>Highest count of 31-60 DPD in the last X months with OS > 1,000,000</td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_03months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_06months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_12months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_24months_cc") %></td>
                        </tr>
                        <tr>
                            <td>Highest count of 61-90 DPD in the last X months with OS > 1,000,000</td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_03months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_06months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_12months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_24months_cc") %></td>
                        </tr>
                        <tr>
                            <td>Highest count of 90+ DPD in the last X months with OS > 1,000,000</td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_03months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_06months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_12months_cc") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_24months_cc") %></td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <tr>
                            <td width="35%" align="left"><b><u>NON CREDIT CARD FACILITIES</u></b></td>
                            <td width="15%" class="H1">COLL.</td>
                            <td width="15%" class="H1">DPD</td>
                            <td width="15%" class="H1">OS</td>
                            <td width="20%" class="H1">BANK</td>
                        </tr>
                        <tr>
                            <td>Worst latest collectibility</td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_coll_noncc") %></td>
                            <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_latest_dpd_noncc") %></td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_os_noncc") %></td>
                            <td class="boxboldleft">&nbsp;<%=DS(0, "worst_latest_bank_noncc") %></td>
                        </tr>
                        <tr>
                            <td>Worst collectibility</td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_coll_noncc") %></td>
                            <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_latest_dpd_noncc") %></td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_os_noncc") %></td>
                            <td class="boxboldleft">&nbsp;<%=DS(0, "worst_bank_noncc") %></td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td width="35%" >&nbsp;</td>
                            <td width="15%" class="H1">3 MONTHS</td>
                            <td width="15%" class="H1">6 MONTHS</td>
                            <td width="15%" class="H1">12 MONTHS</td>
                            <td width="20%" class="H1">24 MONTHS</td>
                        </tr>
                        <tr>
                            <td>Highest count of 31-60 DPD in the last X months</td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_03months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_06months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_12months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_24months_noncc") %></td>
                        </tr>
                        <tr>
                            <td>Highest count of 61-90 DPD in the last X months</td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_03months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_06months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_12months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_24months_noncc") %></td>
                        </tr>
                        <tr>
                            <td>Highest count of 90+ DPD in the last X months</td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_03months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_06months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_12months_noncc") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_24months_noncc") %></td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <tr>
                            <td width="35%" align="left"><b><u>ALL FACILITIES</u></b></td>
                            <td width="15%" class="H1">COLL.</td>
                            <td width="15%" class="H1">DPD</td>
                            <td width="15%" class="H1">OS</td>
                            <td width="20%" class="H1">BANK</td>
                        </tr>
                        <tr>
                            <td>Worst latest collectibility</td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_coll") %></td>
                            <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_latest_dpd") %></td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_latest_os") %></td>
                            <td class="boxboldleft">&nbsp;<%=DS(0, "worst_latest_bank") %></td>
                        </tr>
                        <tr>
                            <td>Worst collectibility</td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_coll") %></td>
                            <td class="boxbold" style="text-align:center">&nbsp;<%=DS(0, "worst_dpd") %></td>
                            <td class="boxbold">&nbsp;<%=DS(0, "worst_os") %></td>
                            <td class="boxboldleft">&nbsp;<%=DS(0, "worst_bank") %></td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td width="35%">&nbsp;</td>
                            <td width="15%" class="H1">3 MONTHS</td>
                            <td width="15%" class="H1">6 MONTHS</td>
                            <td width="15%" class="H1">12 MONTHS</td>
                            <td width="20%" class="H1">24 MONTHS</td>
                        </tr>
                        <tr>
                            <td>Highest count of 31-60 DPD in the last X months</td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_03months") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_06months") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_12months") %></td>
                            <td class="boxbold"><%=DS(0, "count_30dpd_24months") %></td>
                        </tr>
                        <tr>
                            <td>Highest count of 61-90 DPD in the last X months</td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_03months") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_06months") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_12months") %></td>
                            <td class="boxbold"><%=DS(0, "count_60dpd_24months") %></td>
                        </tr>
                        <tr>
                            <td>Highest count of 90+ DPD in the last X months</td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_03months") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_06months") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_12months") %></td>
                            <td class="boxbold"><%=DS(0, "count_90dpd_24months") %></td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%">
                        <tr>
                            <td width="5%">&nbsp;</td>
                            <td class="boxbold" width="35%">Highest Limit CC with MOB = 6 Months</td>
                            <td class="boxbold" width="15%"><%=DS(0, "highest_limitcc_mob06") %></td>
                            <td class="boxbold" width="30%">Highest Limit CC with MOB = 12 Months</td>
                            <td class="boxbold" width="15%"><%=DS(0, "highest_limitcc_mob12") %></td>
                        </tr>
                        <tr>
                            <td width="5%">&nbsp;</td>
                            <td class="boxbold" width="35%">Lowest Limit CC with MOB = 6 Months</td>
                            <td class="boxbold" width="15%"><%=DS(0, "lowest_limitcc_mob06") %></td>
                            <td class="boxbold" width="30%">Lowest Limit CC with MOB = 12 Months</td>
                            <td class="boxbold" width="15%"><%=DS(0, "lowest_limitcc_mob12") %></td>
                        </tr>
                        <tr>
                            <td width="5%">&nbsp;</td>
                            <td class="boxbold" width="35%"><b>SLIK CHECKING RESULT-TEMPORARY</b></td>
                            <td class="boxboldcenter" width="15%"><b><%=DS(0, "bic_result_temp2") %></b></td>
                            <td width="30%">&nbsp;</td>
                            <td width="15%">&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="center">
                    <input runat="server" ID="btnsave3" type="button" class="Bt1" onclick="callback(mainPanel,'save',true);" value=" Save " />
                    <input id="btndownload3" runat="server" type="button" value="Export to Excel" onclick="callback(mainPanel,'export_excel',false);" class="Bt1" />
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
