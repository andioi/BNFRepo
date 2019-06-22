<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Collateral.aspx.cs" Inherits="DebtChecking.Facilities.Collateral" %>
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
			<tr valign="top">
				<td class="H1" colspan="2">Data Debitur</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Nama</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:DropDownList ID="appid" runat="server" AutoPostBack="true"></asp:DropDownList></td>
			            </tr>
			            <tr>
			                <td class="B01">Tgl Lahir</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="BORN_DATE" runat="server"></asp:Label><asp:Label style="display:none" ID="STATUS_APP" runat="server"></asp:Label><input type="hidden" runat="server" id="reffnumber" /></td>
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
            <tr>
                <td align="center" colspan="2"><input type="button" id="btnprint" runat="server" value="Print" class="btn btn-xs btn-success"  onclick="this.style.display = 'none'; document.getElementById('btnpdf').style.display = 'none'; window.print(); this.style.display = ''; document.getElementById('btnpdf').style.display = '';" />
                    <input type="button" id="btnpdf" value="Save As PDF" class="btn btn-xs btn-success"  runat="server" onclick="callback(pdfPanel, '')" />
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
                            <dxwgv:GridViewDataTextColumn Caption="Bank" FieldName="BANK_NAME" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Jenis Agunan" FieldName="JENIS_AGUNAN" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nilai Bank" FieldName="NILAI_BANK" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nilai Independen" FieldName="NILAI_INDEPENDEN" >
                                <PropertiesTextEdit DisplayFormatString="{0:###,##0.##}"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tgl Penilaian" FieldName="TGL_PENILAIAN" >
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Pemilik" FieldName="PEMILIK" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Bukti Kepemilikan" FieldName="BUKTI_KEPEMILIKAN" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Pengikatan" FieldName="PENGIKATAN" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Alamat Jaminan" FieldName="ALAMAT_JAMINAN" ></dxwgv:GridViewDataTextColumn>
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
