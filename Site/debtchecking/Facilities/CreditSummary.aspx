<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreditSummary.aspx.cs" Inherits="DebtChecking.Facilities.CreditSummary" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
        oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="Content" class="Box1" width="100%" align="center">
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
			                <td class="B01">Tgl Lahir</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="BORN_DATE" runat="server"></asp:Label><asp:Label ID="STATUS_APP" runat="server" style="display:none"></asp:Label><input type="hidden" runat="server" id="reffnumber" /></td>
			            </tr>
			            <tr id="Tr1" runat="server">
			                <td class="B01">No KTP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="KTP_NUM" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr id="tr_alamatdom" runat="server">
			                <td class="B01">Alamat Domisili</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ALAMAT_DOM" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Policy Result</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="POLICYRES" runat="server"></asp:Label>&nbsp;
			                <div id="tr_recalculate2" runat="server"><input runat="server" ID="Button5" type="button" class="Bt1" onclick="callback(mainPanel,'s:');" value="Save & Recalculate" /></div>
			                </td>
			            </tr>
			            <tr runat="server">
			                <td class="B01">&nbsp;</td>
			                <td class="BS">:</td>
			                <td class="B11"><input runat="server" ID="Button4" type="button" class="Bt1" onclick="callback(mainPanel,'s:');" value="Recalculate Policy" />
			                <input id="productid" runat="server" type="hidden" /></td>
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
		</table>	
		<table id="DataKredit" width="100%">
			<tr>
				<td class="H1">History Kredit</td>
			</tr>
			<tr>
			    <td>
			        <dxwgv:ASPxGridView ID="GridViewKREDIT" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewKREDIT" KeyFieldName="IDIKREDIT_ID" Font-Size="X-Small"
                        OnLoad="GridViewKREDIT_Load" >
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Nama Debitur" FieldName="NAMA_DEBITUR" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Bank" FieldName="BANK_NAME" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Akad Awal" FieldName="AKAD_AWAL" >
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Jatuh Tempo" FieldName="JATUH_TEMPO" >
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Plafon" FieldName="PLAFON" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Baki Debet" FieldName="BAKI_DEBET" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="% Bunga" FieldName="PERCENT_BUNGA" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Sifat Kredit" FieldName="sifat" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Rek Aktif" FieldName="REKENING_AKTIF" PropertiesTextEdit-EncodeHtml="false"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Policy Result" FieldName="POLICYRES" PropertiesTextEdit-EncodeHtml="false" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Filter Lanjutan" FieldName="POLICYRES_PASS" PropertiesTextEdit-EncodeHtml="false" Visible="False" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Visible="false" Caption="exclude in calc" FieldName="exclude_calc_desc" PropertiesTextEdit-EncodeHtml="false" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Width="1%" CellStyle-Wrap="False">
                                <DataItemTemplate>
                                    <input id="Button1" runat="server" type="button" value="Detail" commandargument="<%# Container.KeyValue %>" onclick="callbackpopup(PopupSID,PNL_KREDIT,'r:' + this.commandargument)" />
                                    <input id="Button3" type="button" value="History Pembayaran" commandargument="<%# Container.KeyValue %>" onclick="callbackpopup(popupKolektabilitas,panelKolektabilitas,'r:' + this.commandargument)" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="100" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowGroupedColumns="true" ShowFooter="true" ShowPreview="true" />
                        <SettingsBehavior AllowGroup="true" />
                        <TotalSummary>
                            <dxwgv:ASPxSummaryItem FieldName="pelapor" SummaryType="Count" />
                        </TotalSummary>
                    </dxwgv:ASPxGridView>
			    </td>
			</tr>
		</table>
		</td>
    </tr>
    </table>
    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
		
	<dxpc:ASPxPopupControl ID="PopupSID" ClientInstanceName="PopupSID" 
            runat="server" HeaderText="History Kredit Detail" width="800px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" 
            EnableAnimation="False">
    <ContentCollection>
    <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" >
        <dxcp:ASPxCallbackPanel ID="PNL_KREDIT" runat="server" ClientInstanceName="PNL_KREDIT" 
            OnCallback="PNL_KREDIT_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent8" runat="server">
                <pre dir="ltr" style="
		        margin: 0px;
		        padding: 2px;
		        border: 1px inset;
		        width: 800px;
		        height: 500px;
		        text-align: left;
		        overflow: auto">

            <table width="100%" class="Box1">
                <tr>
                    <td align="center">
                        <table class="Tbl0">
                            <tr valign="top">
                                <td width="50%">
                                    <table class="Tbl0">
                                        <tr>
                                            <td class="B01">Tanggal Update</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="TGL_UPDATE" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Sandi Pelapor</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="PELAPOR" runat="server" ></asp:Label>
                                                <input type="hidden" id="KREDIT_ID" value="" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Nama bank</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="BANK_NAME" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Sifat</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="SIFAT" runat="server" ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Akad Awal</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="AKAD_AWAL" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Jatuh Tempo</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="JATUH_TEMPO" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Nomor Rekening</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="NO_REKENING" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Rekening Aktif</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="REKENING_AKTIF" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Plafon</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="PLAFON" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Outstanding</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="BAKI_DEBET" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">% Bunga</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="PERCENT_BUNGA" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="50%">
                                    <table class="Tbl0">
                                        <tr>
                                            <td class="B01">Tunggakan Pokok</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="TUNGGAKAN_POKOK" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Bunga ON</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="BUNGA_ON" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Bunga OFF</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="BUNGA_OFF" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Frek</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="FREK" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Sektor Ekonomi</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="SEKTOR_EKONOMI" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Jenis Penggunaan</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="JENIS_PENGGUNAAN" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Kondisi</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="KONDISI" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Keterangan Kondisi</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="DESKRIPSI_KONDISI" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Tgl Kondisi</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="TGL_KONDISI" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Sebab Macet</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="SEBAB_MACET" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="B01">Tgl Macet</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="TGL_MACET" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display:none">
                                            <td class="B01">Exclude in Calculator</td>
                                            <td class="BS">:</td>
                                            <td class="B11">
                                                <asp:Label ID="exclude_calc" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><b>"Policy Result" yang Failed</b></td>
                </tr>
                <tr>
                    <td>
                        <dxwgv:ASPxGridView ID="GridPolicy" runat="server" Width="100%" AutoGenerateColumns="False" 
                            ClientInstanceName="GridPolicy" KeyFieldName="">
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Kriteria" FieldName="KRITERIA" ></dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Policy Result" PropertiesTextEdit-EncodeHtml="false" FieldName="POLICYRES" ></dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager PageSize="50" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
                <tr style="display:none">
                    <td><b>"Filter Lanjutan" yang Failed</b></td>
                </tr>
                <tr style="display:none">
                    <td>
                        <dxwgv:ASPxGridView ID="GridPolicy2" runat="server" Width="100%" AutoGenerateColumns="False" 
                            ClientInstanceName="GridPolicy2" KeyFieldName="">
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Kriteria" FieldName="KRITERIA" ></dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Policy Result" PropertiesTextEdit-EncodeHtml="false" FieldName="POLICYRES" ></dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <SettingsPager PageSize="50" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
                <tr class="F1">
                    <td align="center">
                        <input runat="server" ID="BTN_SAVE1" type="button" class="Bt1" onclick="callbackpopup(PopupSID,PNL_KREDIT,'s:'+PopupSID_PNL_KREDIT_KREDIT_ID.value,GridViewKREDIT)" value=" Masukan ke Credit Calculation " style="display:none" />
                        <input id="Button2" runat="server" type="button" value="Close" class="Bt1" onclick="PopupSID.Hide();" />
                    </td>
                </tr>
            </table>

                    </pre>
            </dxp:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl>
    </ContentCollection>
    </dxpc:ASPxPopupControl>

    <dxpc:ASPxPopupControl ID="popupKolektabilitas" ClientInstanceName="popupKolektabilitas" 
            runat="server" HeaderText="History Pembayaran" width="600px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" 
            EnableAnimation="False">
    <ContentCollection>
    <dxpc:PopupControlContentControl ID="PopupControlContentControl4" runat="server" >
        <dxcp:ASPxCallbackPanel ID="panelKolektabilitas" runat="server" ClientInstanceName="panelKolektabilitas" 
            OnCallback="panelKolektabilitas_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent4" runat="server">
                <input type="hidden" id="IDIKREDIT_ID" runat="server" />
                <dxwgv:ASPxGridView ID="GridViewKolek" runat="server" Width="100%" AutoGenerateColumns="False" 
                    ClientInstanceName="GridViewKolek" KeyFieldName="KOLEK_SEQ" OnAfterPerformCallback="GridViewKolek_AfterPerformCallback">
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="Bulan" FieldName="KOLEK_MONTH"></dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Tahun" FieldName="KOLEK_YEAR"></dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Kolektabilitas" FieldName="KOLEKTABILITAS"></dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Tunggakan" FieldName="TUNGGAKAN"></dxwgv:GridViewDataTextColumn>
                    </Columns>
                    <SettingsPager PageSize="12" />
                </dxwgv:ASPxGridView>                
            </dxp:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl>
    </ContentCollection>
    </dxpc:ASPxPopupControl>        
    </div>
    
    </form>
</body>
</html>
