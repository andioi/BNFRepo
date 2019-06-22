<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Collateral.aspx.cs" Inherits="DebtChecking.SLIK.Collateral" %>
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
    <table class="Box1" width="100%">
    <tr>
        <td>
		<table id="DataDebitur" width="100%">
            <tr>
                <td colspan="3">
                    Select other customer in this requestid <asp:DropDownList ID="ddl_appid" runat="server" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
			<tr valign="top">
				<td class="H1" colspan="2">Data Debitur</td>
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
			            <tr>
			                <td class="B01">KTP / NIK</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ktp" runat="server"></asp:Label></td>
			            </tr>
						<tr>
			                <td class="B01">KTP Address</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="full_ktpaddress" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Policy Result</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label Font-Bold="true" Font-Size="Small" ForeColor="Red" ID="final_policy" runat="server"></asp:Label></td>
			            </tr>
			        </table>
			    </td>
			</tr>
            <tr>
                <td align="center" colspan="2"><input type="button" id="btnprint" runat="server" class="btn btn-xs btn-success" value="Print" onclick="this.style.display = 'none'; document.getElementById('btnpdf').style.display = 'none'; window.print(); this.style.display = ''; document.getElementById('btnpdf').style.display = '';" />
                    <input type="button" id="btnpdf" value="Save As PDF" runat="server" class="btn btn-xs btn-success" onclick="callback(pdfPanel, '', false, null)" />
                    <dxcp:ASPxCallbackPanel ID="pdfPanel" runat="server" Width="100%" 
                        oncallback="pdfPanel_Callback" ClientInstanceName="pdfPanel">
                        <PanelCollection><dxp:PanelContent ID="PanelContent2" runat="server">
                        <input type="hidden" id="urlframe" runat="server" />
                    </dxp:PanelContent></PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
		</table>	
		<table id="DataAgunan" width="100%">
			<tr>
				<td colspan="2" class="H1">Data Agunan</td>
			</tr>
			<tr>
			    <td>
			        <dxwgv:ASPxGridView ID="GridViewColl" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewColl" KeyFieldName="COLL_SEQ"  
                        OnLoad="GridViewColl_Load" Font-Size="X-Small">
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Bank" FieldName="ljkName" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Jenis Agunan" FieldName="jenisAgunanKet" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nilai Bank" FieldName="nilaiAgunanMenurutLJK" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nilai Independen" FieldName="nilaiAgunanIndep" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tgl Penilaian" FieldName="tglPenilaianPelapor" >
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Pemilik" FieldName="namaPemilikAgunan" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Bukti Kepemilikan" FieldName="buktiKepemilikan" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Pengikatan" FieldName="jenisPengikatanKet" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Alamat Jaminan" FieldName="alamatAgunan" ></dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="10" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowGroupedColumns="true" />
                        <SettingsBehavior AllowGroup="true" />
                    </dxwgv:ASPxGridView>
			    </td>
			</tr>
		</table>
		</td>
    </tr>
    </table>
		
    </form>
</body>
</html>
