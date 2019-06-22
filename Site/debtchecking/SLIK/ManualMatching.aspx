<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManualMatching.aspx.cs" Inherits="DebtChecking.Verification.ManualMatching" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SID Text</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <script language="javascript">
        function simpan()
        {
            if (document.form1.mainPanel_flag_spv.value == "False") {
                if (confirm('Kesempatan simpan hanya sekali, Apakah anda yakin review DIN sudah benar?')) {
                    document.getElementById('IFR_TEXT').contentWindow.kliksave();
                    document.form1.mainPanel_btnsave.disabled = 'true';
                }
            } else {
                document.getElementById('IFR_TEXT').contentWindow.kliksave();
            }
        }

        function tickmatch() {
            var hmaddr_match = (document.form1.mainPanel_addr_match.checked == true ? "1" : "0");
            var compnm_match = (document.form1.mainPanel_coyname_match.checked == true ? "1" : "0");
            var bizadd_match = (document.form1.mainPanel_bizaddr_match.checked == true ? "1" : "0");
            var homphn_match = (document.form1.mainPanel_homephone_match.checked == true ? "1" : "0");
            var bizphn_match = (document.form1.mainPanel_bizphone_match.checked == true ? "1" : "0");
            var mobphn_match = (document.form1.mainPanel_cellphone_match.checked == true ? "1" : "0");
            var phonever_flag = hmaddr_match + compnm_match + bizadd_match + homphn_match + bizphn_match + mobphn_match;
            document.getElementById('IFR_TEXT').contentWindow.form1.phonever_flag.value = phonever_flag;
            document.getElementById('IFR_TEXT').contentWindow.form1.phonever_flag_qc.value = phonever_flag;
        }

        function tickmatch_qc() {
            var hmaddr_match = (document.form1.mainPanel_addr_match_qc.checked == true ? "1" : "0");
            var compnm_match = (document.form1.mainPanel_coyname_match_qc.checked == true ? "1" : "0");
            var bizadd_match = (document.form1.mainPanel_bizaddr_match_qc.checked == true ? "1" : "0");
            var homphn_match = (document.form1.mainPanel_homephone_match_qc.checked == true ? "1" : "0");
            var bizphn_match = (document.form1.mainPanel_bizphone_match_qc.checked == true ? "1" : "0");
            var mobphn_match = (document.form1.mainPanel_cellphone_match_qc.checked == true ? "1" : "0");
            var phonever_flag_qc = hmaddr_match + compnm_match + bizadd_match + homphn_match + bizphn_match + mobphn_match;
            document.getElementById('IFR_TEXT').contentWindow.form1.phonever_flag_qc.value = phonever_flag_qc;
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="hd_appid" />
        <input type="hidden" runat="server" id="exportfilename" />
    <div>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" ClientInstanceName="mainPanel">
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
    <table id="DataDebitur" width="100%">
			<tr>
				<td class="H1" colspan="2">Data Debitur</td>
			</tr>
			<tr valign="top">
			    <td width="50%">
			        <table class="Tbl0" width="100%">
			            <tr>
			                <td class="B01">Nama Debitur / Terkait</td>
			                <td class="BS">:</td>
			                <td class="B11">
                                <table class="Tbl0" width="100%">
                                    <tr>
                                        <td width="20%"><asp:DropDownList ID="appid" runat="server"></asp:DropDownList></td>
                                        <td class="B01">Nama On ID</td>
			                            <td class="BS">:</td>
			                            <td class="B11"><asp:Label ID="NAME_ON_ID" runat="server"></asp:Label>&nbsp;</td>
                                    </tr>
                                </table>
			                </td>
			            </tr>
			            <tr>
			                <td class="B01">Tempat, Tgl Lahir / Pendirian</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="BORN_DATE" runat="server"></asp:Label><asp:Label ID="STATUS_APP" runat="server" style="display:none"></asp:Label><input type="hidden" runat="server" id="reffnumber" /></td>
			            </tr>
                        <tr>
			                <td class="B01">No KTP / Akta</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ktp_num" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">NPWP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="npwp" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Phone Ver Flag</td>
			                <td class="BS">:</td>
			                <td class="B11">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td><asp:CheckBox ID="addr_match" runat="server" Text="Home Addr Match" onclick="tickmatch()"></asp:CheckBox></td>
                                        <td><asp:CheckBox ID="coyname_match" runat="server" Text="Company Name Match" onclick="tickmatch()"></asp:CheckBox></td>
                                        <td><asp:CheckBox ID="homephone_match" runat="server" Text="Home Phn Match" onclick="tickmatch()"></asp:CheckBox></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:CheckBox ID="bizaddr_match" runat="server" Text="Biz Addr Match" onclick="tickmatch()"></asp:CheckBox></td>
                                        <td><asp:CheckBox ID="bizphone_match" runat="server" Text="Biz Phn Match" onclick="tickmatch()"></asp:CheckBox></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td><asp:CheckBox ID="cellphone_match" runat="server" Text="Mobile Phn Match" onclick="tickmatch()"></asp:CheckBox></td>
                                    </tr>
                                </table>
                                <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tbl_qc" runat="server">
                                    <tr>
                                        <td><asp:CheckBox ID="addr_match_qc" runat="server" Text="Home Addr Match" onclick="tickmatch_qc()"></asp:CheckBox></td>
                                        <td><asp:CheckBox ID="coyname_match_qc" runat="server" Text="Company Name Match" onclick="tickmatch_qc()"></asp:CheckBox></td>
                                        <td><asp:CheckBox ID="homephone_match_qc" runat="server" Text="Home Phn Match" onclick="tickmatch_qc()"></asp:CheckBox></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td><asp:CheckBox ID="bizaddr_match_qc" runat="server" Text="Biz Addr Match" onclick="tickmatch_qc()"></asp:CheckBox></td>
                                        <td><asp:CheckBox ID="bizphone_match_qc" runat="server" Text="Biz Phn Match" onclick="tickmatch_qc()"></asp:CheckBox></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>&nbsp;</td>
                                        <td><asp:CheckBox ID="cellphone_match_qc" runat="server" Text="Mobile Phn Match" onclick="tickmatch_qc()"></asp:CheckBox></td>
                                    </tr>
                                </table>
			                </td>
			            </tr>
                        <tr>
			                <td class="B01">Policy Result</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="POLICYRES" runat="server"></asp:Label>
			                </td>
			            </tr>
			        </table>
			    </td>
			    <td width="50%">
			        <table width="100%">
			            <tr>
			                <td class="B01">Alamat Rumah</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ALAMAT_DOM" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Alamat KTP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ALAMAT_KTP" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Telp Rumah / HP</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="TELP_HP" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Alamat Kantor</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="ALAMAT_OFC" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Nama / Telp Kantor</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="TLP_OFC" runat="server"></asp:Label></td>
			            </tr>
                        <tr>
			                <td class="B01">Manual Review By / Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="manualmatch_by" runat="server"></asp:Label> / 
                                <asp:Label ID="manualmatch_date" runat="server"></asp:Label>
			                </td>
			            </tr>
                        <tr id="tr_qc" runat="server">
			                <td class="B01">QC By / Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="manualmatch_qc_by" runat="server"></asp:Label> / 
                                <asp:Label ID="manualmatch_qc_date" runat="server"></asp:Label>
			                </td>
			            </tr>
                        <tr id="tr1" runat="server">
			                <td class="B01">Last Export SIF By / Date</td>
			                <td class="BS">:</td>
			                <td class="B11"><asp:Label ID="exportsif_by" runat="server"></asp:Label> / 
                                <asp:Label ID="exportsif_date" runat="server"></asp:Label>
			                </td>
			            </tr>
			        </table>
			    </td>
			</tr>
            <tr>
				<td colspan="2" align="center">
                    <input type="button" id="btnsave" runat="server" onclick="simpan()" value="Save" /> 
                    <input type="hidden" runat="server" id="flag_spv" />
                    <input type="button" id="btnexport" onclick="PopupPage('ExportCoreSIF.aspx?appid=' + document.form1.hd_appid.value + '&filename=' + document.form1.exportfilename.value)" value="Export SIF" />
				</td>
			</tr>
		</table>
        </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    <table width="100%" class="Box1">
		<tr style="display:none">
			<td class="H1">MANUAL REVIEW</td>
		</tr>
		<tr runat="server">
			<td>
				<iframe ID="IFR_TEXT" name="IFR_TEXT" frameborder="no" width="100%" height="450" runat="server"></iframe>
			</td>
		</tr>
    </table>
            
    </div>
    </form>
</body>
</html>
