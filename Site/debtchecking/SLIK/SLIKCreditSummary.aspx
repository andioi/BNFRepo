<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLIKCreditSummary.aspx.cs" Inherits="DebtChecking.Facilities.SLIKCreditSummary" %>
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
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
        oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
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
				<td class="H1" colspan="2">Customer Data</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Customer Name</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label Font-Size="Medium" Font-Bold="true" ID="cust_name" runat="server"></asp:Label>
                                <input type="hidden" runat="server" id="status_app" />
                                <input type="hidden" runat="server" id="reffnumber" />
                                <input type="hidden" runat="server" id="appid" />
			                </td>
			            </tr>
			            <tr>
			                <td class="B01">Place / Date Of Birth</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="pob_dob" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Gender</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="genderdesc" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table class="Tbl0" width="100%">
                        <tr id="Tr1" runat="server">
			                <td class="B01">KTP / NIK</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ktp" runat="server"></asp:Label></td>
			            </tr>
			            <tr>
			                <td class="B01">Policy Result</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ForeColor="Red" Font-Bold="true" ID="final_policy" runat="server"></asp:Label></td>
			            </tr>
			            <tr runat="server">
			                <td class="B01">&nbsp;</td>
			                <td class="BS">:</td>
			                <td class="B11"><input runat="server" ID="Button1" type="button" class="btn btn-xs btn-danger" onclick="callback(mainPanel,'r:', false, null);" value="Recalculate Policy" />
			                <input id="productid" runat="server" type="hidden" /></td>
			            </tr>
			        </table>
			    </td>
			</tr>
            <tr>
                <td align="center" colspan="2">
                    <input type="button" id="btnprint" runat="server" class="btn btn-xs btn-success" value="Print" onclick="this.style.display = 'none'; document.getElementById('mainPanel_btnpdf').style.display = 'none'; window.print(); this.style.display = ''; document.getElementById('mainPanel_btnpdf').style.display = '';" />
                    <input type="button" id="btnpdf" value="Save As PDF" class="btn btn-xs btn-success" runat="server" onclick="callback(pdfPanel, '', false, null)" />
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
                        ClientInstanceName="GridViewKREDIT" KeyFieldName="fasilitasid" Font-Size="X-Small"
                        OnLoad="GridViewKREDIT_Load" >
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="NIK" FieldName="nik" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="LJK" FieldName="ljkName" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="FACILITY TYPE" FieldName="jenisKreditPembiayaanKet" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="START DATE" FieldName="tanggalMulai" >
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="MATURITY DATE" FieldName="tanggalJatuhTempo" >
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="SISA TENOR" FieldName="SisaTenor" CellStyle-HorizontalAlign="Center" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="PLAFON" FieldName="plafon" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="OUTSTANDING" FieldName="outstandingPrincipal" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="INTEREST (%)" FieldName="sukuBungaImbalan" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="COLLECTABILITY" FieldName="kualitasTerakhir" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="DPD" FieldName="jumlahHariTunggakan" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CONDITION" FieldName="kondisiKet" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="POLICY" FieldName="policy_result" PropertiesTextEdit-EncodeHtml="false" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Width="1%" CellStyle-Wrap="False">
                                <DataItemTemplate>
                                    <input id="Button1" runat="server" class="btn btn-xs btn-success" type="button" value="Detail" alt="<%# Container.KeyValue %>" onclick="callbackpopup(PopupSID,PNL_KREDIT,'r:' + this.alt)" />
                                    <input id="Button6" runat="server" class="btn btn-xs btn-success" type="button" value="History" alt="<%# Container.KeyValue %>" onclick="callbackpopup(PopupHistory,PNL_HISTORY,'r:' + this.alt)" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowGroupedColumns="true" ShowFooter="true" ShowPreview="true" />
                        <SettingsBehavior AllowGroup="true" />
                        <TotalSummary>
                            <dxwgv:ASPxSummaryItem FieldName="ljkName" SummaryType="Count" />
                            <dxwgv:ASPxSummaryItem FieldName="plafon" SummaryType="Sum" DisplayFormat="{0:###,##0.##}" />
                            <dxwgv:ASPxSummaryItem FieldName="outstandingPrincipal" SummaryType="Sum" DisplayFormat="{0:###,##0.##}" />
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
            runat="server" HeaderText="Detail" width="800px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" 
            EnableAnimation="False">
    <ContentCollection>
    <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" >
        <dxcp:ASPxCallbackPanel ID="PNL_KREDIT" runat="server" ClientInstanceName="PNL_KREDIT" 
            OnCallback="PNL_KREDIT_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent8" runat="server">
                <!-- <pre dir="ltr" style="
		        margin: 0px;
		        padding: 2px;
		        border: 1px inset;
		        width: 800px;
		        height: 500px;
		        text-align: left;
		        overflow: auto"> -->

            <table width="100%" class="Box1" style="vertical-align:top">
                <tr>
                    <td align="center">
                        <table class="Tbl0">
                            <tr valign="top">
                                <td width="50%">
                                    <table class="Tbl0">
                                         <tr><td class="B01">Pelapor</td><td class="BS">:</td><td class="B11"><%=DS(0, "ljkName") %></td></tr>
                                         <tr><td class="B01">Cabang</td><td class="BS">:</td><td class="B11"><%=DS(0, "cabangKet") %></td></tr>
                                         <tr><td class="B01">No Rekening</td><td class="BS">:</td><td class="B11"><%=DS(0, "noRekening") %></td></tr>
                                         <tr><td class="B01">Sifat Kredit</td><td class="BS">:</td><td class="B11"><%=DS(0, "sifatKreditPembiayaanKet") %></td></tr>
                                         <tr><td class="B01">Jenis Kredit</td><td class="BS">:</td><td class="B11"><%=DS(0, "jenisKreditPembiayaanKet") %></td></tr>
                                         <tr><td class="B01">Akad Kredit</td><td class="BS">:</td><td class="B11"><%=DS(0, "akadKreditPembiayaanKet") %></td></tr>
                                         <tr><td class="B01">Baru/Perpanjangan</td><td class="BS">:</td><td class="B11"><%=DS(0, "akadKreditPembiayaanKet") %></td></tr>
                                         <tr><td class="B01">No Akad Awal</td><td class="BS">:</td><td class="B11"><%=DS(0, "noAkadAwal") %></td></tr>
                                         <tr><td class="B01">Tanggal Akad Awal</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalAkadAwal") %></td></tr>
                                         <tr><td class="B01">No Akad Akhir</td><td class="BS">:</td><td class="B11"><%=DS(0, "noAkadAkhir") %></td></tr>
                                         <tr><td class="B01">Tanggal Akad Akhir</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalAkadAkhir") %></td></tr>
                                         <tr><td class="B01">Tanggal Awal Kredit</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalAwalKredit") %></td></tr>
                                         <tr><td class="B01">Tanggal Mulai</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalMulai") %></td></tr>
                                         <tr><td class="B01">Tanggal Jatuh Tempo</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalJatuhTempo") %></td></tr>
                                         <tr><td class="B01">Kategori Debitur</td><td class="BS">:</td><td class="B11"><%=DS(0, "kategoriDebiturKet") %></td></tr>
                                         <tr><td class="B01">Jenis Penggunaan</td><td class="BS">:</td><td class="B11"><%=DS(0, "jenisPenggunaanKet") %></td></tr>
                                         <tr><td class="B01">Sektor Ekonomi</td><td class="BS">:</td><td class="B11"><%=DS(0, "sektorEkonomiKet") %></td></tr>
                                         <tr><td class="B01">Kredit Program Pemerintah</td><td class="BS">:</td><td class="B11"><%=DS(0, "kreditProgramPemerintahKet") %></td></tr>
                                         <tr><td class="B01">Kab/Kota Lokasi Proyek</td><td class="BS">:</td><td class="B11"><%=DS(0, "lokasiProyekKet") %></td></tr>
                                         <tr><td class="B01">Valuta</td><td class="BS">:</td><td class="B11"><%=DS(0, "valutaKode") %></td></tr>
                                         <tr><td class="B01">Suku Bunga/Margin</td><td class="BS">:</td><td class="B11"><%=DS(0, "sukuBungaImbalan") %> %</td></tr>
                                         <tr><td class="B01">Keterangan</td><td class="BS">:</td><td class="B11"><%=DS(0, "keterangan") %></td></tr>
                                    </table>
                                </td>
                                <td width="50%">
                                    <table class="Tbl0">
                                        <tr><td class="B01">Tanggal Update</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalUpdate") %></td></tr>
                                        <tr><td class="B01">Kolektibilitas</td><td class="BS">:</td><td class="B11"><%=DS(0, "kualitasTerakhir") %></td></tr>
                                        <tr><td class="B01">Jumlah Hari Tunggakan</td><td class="BS">:</td><td class="B11"><%=DS(0, "jumlahHariTunggakan") %></td></tr>
                                        <tr><td class="B01">Nilai Proyek</td><td class="BS">:</td><td class="B11"><%=DS(0, "nilaiProyek") %></td></tr>
                                        <tr><td class="B01">Plafon Awal</td><td class="BS">:</td><td class="B11"><%=DS(0, "plafonAwal") %></td></tr>
                                        <tr><td class="B01">Plafon</td><td class="BS">:</td><td class="B11"><%=DS(0, "plafon") %></td></tr>
                                        <tr><td class="B01">Baki Debet</td><td class="BS">:</td><td class="B11"><%=DS(0, "outstandingPrincipal") %></td></tr>
                                        <tr><td class="B01">Realisasi/Pencairan Bulan Berjalan</td><td class="BS">:</td><td class="B11"><%=DS(0, "realisasiBulanBerjalan") %></td></tr>
                                        <tr><td class="B01">Nilai dalam Mata Uang Asal</td><td class="BS">:</td><td class="B11"><%=DS(0, "nilaiDalamMataUangAsal") %></td></tr>
                                        <tr><td class="B01">Sebab Macet</td><td class="BS">:</td><td class="B11"><%=DS(0, "sebabMacetKet") %></td></tr>
                                        <tr><td class="B01">Tanggal Macet</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalMacet") %></td></tr>
                                        <tr><td class="B01">Tunggakan Pokok</td><td class="BS">:</td><td class="B11"><%=DS(0, "tunggakanPokok") %></td></tr>
                                        <tr><td class="B01">Tunggakan Bunga</td><td class="BS">:</td><td class="B11"><%=DS(0, "tunggakanBunga") %></td></tr>
                                        <tr><td class="B01">Frekuensi Tunggakan</td><td class="BS">:</td><td class="B11"><%=DS(0, "frekuensiTunggakan") %></td></tr>
                                        <tr><td class="B01">Denda</td><td class="BS">:</td><td class="B11"><%=DS(0, "denda") %></td></tr>
                                        <tr><td class="B01">Frekuensi Restrukturisasi</td><td class="BS">:</td><td class="B11"><%=DS(0, "frekuensiRestrukturisasi") %></td></tr>
                                        <tr><td class="B01">Tanggal Restrukturisasi Akhir</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalRestrukturisasiAkhir") %></td></tr>
                                        <tr><td class="B01">Cara Restrukturisasi</td><td class="BS">:</td><td class="B11"><%=DS(0, "restrukturisasiKet") %></td></tr>
                                        <tr><td class="B01">Kondisi</td><td class="BS">:</td><td class="B11"><%=DS(0, "kondisiKet") %></td></tr>
                                        <tr><td class="B01">Tanggal Kondisi</td><td class="BS">:</td><td class="B11"><%=DS(0, "tanggalKondisi") %></td></tr>
                                        <tr><td class="B01">Jenis Suku Bunga Kredit</td><td class="BS">:</td><td class="B11"><%=DS(0, "jenisSukuBungaImbalanKet") %></td></tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="F1">
                    <td align="center">
                        <input id="Button2" runat="server" class="btn btn-xs btn-success" type="button" value="Close" onclick="PopupSID.Hide();" />
                    </td>
                </tr>
            </table>

                    <!-- </pre> -->
            </dxp:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl>
    </ContentCollection>
    </dxpc:ASPxPopupControl>
    
    <dxpc:ASPxPopupControl ID="PopupHistory" ClientInstanceName="PopupHistory" 
            runat="server" HeaderText="History" width="800px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" 
            EnableAnimation="False">
    <ContentCollection>
    <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" >
        <dxcp:ASPxCallbackPanel ID="PNL_HISTORY" runat="server" ClientInstanceName="PNL_HISTORY" 
            OnCallback="PNL_HISTORY_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent3" runat="server">
                <pre dir="ltr" style="
		        margin: 0px;
		        padding: 2px;
		        border: 1px inset;
		        width: 800px;
		        height: 300px;
		        text-align: left;
		        overflow: auto">

            <table width="100%" class="Box1">
                <tr>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan12Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan11Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan10Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan09Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan08Ket") %></td>
                    <td align="center" colspan="2" width="15%"><%=DS(0, "tahunBulan07Ket") %></td>
                </tr>
                <tr>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan12Color") %>"><%=DS(0, "tahunBulan12Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan12Color") %>"><%=DS(0, "tahunBulan12Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan11Color") %>"><%=DS(0, "tahunBulan11Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan11Color") %>"><%=DS(0, "tahunBulan11Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan10Color") %>"><%=DS(0, "tahunBulan10Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan10Color") %>"><%=DS(0, "tahunBulan10Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan09Color") %>"><%=DS(0, "tahunBulan09Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan09Color") %>"><%=DS(0, "tahunBulan09Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan08Color") %>"><%=DS(0, "tahunBulan08Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan08Color") %>"><%=DS(0, "tahunBulan08Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan07Color") %>"><%=DS(0, "tahunBulan07Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan07Color") %>"><%=DS(0, "tahunBulan07Ht") %>&nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan06Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan05Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan04Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan03Ket") %></td>
                    <td align="center" colspan="3" width="15%"><%=DS(0, "tahunBulan02Ket") %></td>
                    <td align="center" colspan="2" width="15%"><%=DS(0, "tahunBulan01Ket") %></td>
                </tr>
                <tr>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan06Color") %>"><%=DS(0, "tahunBulan06Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan06Color") %>"><%=DS(0, "tahunBulan06Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan05Color") %>"><%=DS(0, "tahunBulan05Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan05Color") %>"><%=DS(0, "tahunBulan05Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan04Color") %>"><%=DS(0, "tahunBulan04Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan04Color") %>"><%=DS(0, "tahunBulan04Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan03Color") %>"><%=DS(0, "tahunBulan03Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan03Color") %>"><%=DS(0, "tahunBulan03Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan02Color") %>"><%=DS(0, "tahunBulan02Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan02Color") %>"><%=DS(0, "tahunBulan02Ht") %>&nbsp;</td>
                    <td align="center" width="1%">&nbsp;</td>
                    <td align="center" width="6%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan01Color") %>"><%=DS(0, "tahunBulan01Kol") %>&nbsp;</td>
                    <td align="center" width="7%" style="border:solid 1px black;" bgcolor="<%=DS(0, "tahunBulan01Color") %>"><%=DS(0, "tahunBulan01Ht") %>&nbsp;</td>
                </tr>
                <tr class="F1">
                    <td align="center" colspan="17">&nbsp;</td>
                </tr>
                <tr class="F1">
                    <td align="center" colspan="17">
                        <input id="Button3" runat="server" type="button" value="Close" class="btn btn-xs btn-success"  onclick="PopupHistory.Hide();" />
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

    </div>
    
    </form>
</body>
</html>
