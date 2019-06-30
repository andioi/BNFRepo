<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestSLIK.aspx.cs" Inherits="DebtChecking.Facilities.RequestSLIK" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>
<%@ Register assembly="DMSControls" namespace="DMSControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Request Checking</title>
    <!-- CSS -->
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!--<link href="../include/bootstrap-datetimepicker.css" type="text/css" rel="Stylesheet" />
    <link href="../include/bootstrap-combobox.css" type="text/css" rel="Stylesheet" />-->
    <!-- #include file="~/include/onepost.html" -->

    <!-- Script -->
	<script src="jquery.min.js" type="text/javascript"></script>
    <!--<script src="../include/bootstrap.min.js" type="text/javascript"></script>
    <script src="../include/moment-with-locales.js" type="text/javascript"></script>-->
	<script src="jquery.maskedinput.js" type="text/javascript"></script>
    <!--<script src="../include/bootstrap-datetimepicker.js" type="text/javascript"></script>
    <script src="../include/bootstrap-combobox.js" type="text/javascript"></script>-->
    <script type="text/javascript">
        function ubahjenis(val) {
            if (val == "IND") {
                document.getElementById("mainPanel_npwp").className = "";
                document.getElementById("mainPanel_gender").className = "mandatory";
                document.getElementById("mainPanel_ktp").className = "mandatory";

                document.getElementById("mainPanel_tr_gender").style.display = "";
                document.getElementById("mainPanel_tr_nationality").style.display = "";
                document.getElementById("mainPanel_tr_marital").style.display = "";
                document.getElementById("mainPanel_tr_mother_name").style.display = "";

            } else if (val == "PSH") {
                document.getElementById("mainPanel_npwp").className = "mandatory";
                document.getElementById("mainPanel_gender").className = "";
                document.getElementById("mainPanel_ktp").className = "";

                document.getElementById("mainPanel_tr_gender").style.display = "none";
                document.getElementById("mainPanel_tr_nationality").style.display = "none";
                document.getElementById("mainPanel_tr_marital").style.display = "none";
                document.getElementById("mainPanel_tr_mother_name").style.display = "none";
            }
        }
        function ubahjenis_supl(val) {
            if (val == "IND") {
                document.getElementById("PopupSID_PanelSID_supp_npwp").className = "";
                document.getElementById("PopupSID_PanelSID_supp_gender").className = "mandatory";
                document.getElementById("PopupSID_PanelSID_supp_ktp").className = "mandatory";

                document.getElementById("PopupSID_PanelSID_tr_supp_gender").style.display = "";
                document.getElementById("PopupSID_PanelSID_tr_supp_mother_name").style.display = "";
                document.getElementById("PopupSID_PanelSID_tr_supp_nationality").style.display = "";
            } else if (val == "PSH") {
                document.getElementById("PopupSID_PanelSID_supp_npwp").className = "mandatory";
                document.getElementById("PopupSID_PanelSID_supp_gender").className = "";
                document.getElementById("PopupSID_PanelSID_supp_ktp").className = "";

                document.getElementById("PopupSID_PanelSID_tr_supp_gender").style.display = "none";
                document.getElementById("PopupSID_PanelSID_tr_supp_mother_name").style.display = "none";
                document.getElementById("PopupSID_PanelSID_tr_supp_nationality").style.display = "none";
            }
        }

        function validasiktp() {
            var ret = true
            var noktp = document.getElementById("mainPanel_ktp").value;
            var nonpwp = document.getElementById("mainPanel_npwp").value;
            if (document.getElementById("mainPanel_cust_type_0").checked) {
                if (document.getElementById("mainPanel_nationality_0").checked && noktp.length != 16) {
                    alert("No KTP tidak valid!");
                    ret = false;
                }
                if (document.getElementById("mainPanel_nationality_1").checked && noktp.length < 6) {
                    alert("No Paspor tidak valid!");
                    ret = false;
                }
            }
            
            if (nonpwp.length > 0) {
                if (nonpwp.length < 20 || nonpwp.length > 20) {
                    alert("No NPWP tidak valid!");
                    ret = false;
                }
            }
            return ret
        }

        function validasiktp_supl() {
            var ret = true
            var noktp = document.getElementById("PopupSID_PanelSID_supp_ktp").value;
            var nonpwp = document.getElementById("PopupSID_PanelSID_supp_npwp").value;
            var e = document.getElementById("PopupSID_PanelSID_status_app");
            var status_app = e.options[e.selectedIndex].value;
            var nama_supp = document.getElementById("PopupSID_PanelSID_supp_cust_name").value;
            //var tgl = document.getElementById("PopupSID_PanelSID_supp_dob_DD").value;
            var obj = document.getElementById("PopupSID_PanelSID_supp_dob_MM");
            //var bln = obj.options[obj.selectedIndex].value;
            //var thn = document.getElementById("PopupSID_PanelSID_supp_dob_YY").value;
            var dob = document.getElementById("PopupSID_PanelSID_supp_dob_I").value;

            if (document.getElementById("PopupSID_PanelSID_supp_cust_type_0").checked == false &&
                document.getElementById("PopupSID_PanelSID_supp_cust_type_1").checked == false)
            {
                alert("Jenis harus diisi!");
                ret = false;
            }
            else if (status_app == "") {
                alert("Hubungan dengan debitur harus diisi!");
                ret = false;
            }
            else if (nama_supp == "")
            {
                alert("Nama harus diisi!");
                ret = false;
            }
            /*else if (tgl == "" || bln=="" || thn=="") {
                alert("Tgl lahir/pendirian harus diisi!");
                ret = false;
            }*/
            else if (dob == "") {
                alert("Tgl lahir/pendirian harus diisi!");
                ret = false;
            }
            else if (document.getElementById("PopupSID_PanelSID_supp_cust_type_0").checked) {
                if (noktp.length < 6) {
                    alert("No KTP tidak valid!");
                    ret = false;
                }
                else if (document.getElementById("PopupSID_PanelSID_supp_gender_0").checked == false &&
                    document.getElementById("PopupSID_PanelSID_supp_gender_1").checked == false) {
                    alert("Gender harus diisi!");
                    ret = false;
                }
            }
            else if (document.getElementById("PopupSID_PanelSID_supp_cust_type_1").checked) {
                if (nonpwp.length < 15) {
                    alert("No NPWP tidak valid!");
                    ret = false;
                }
            }
            else {
                
            }
            
            return ret
        }
		
		function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }  
    </script>
</head>
<body onload="ubahjenis(document.getElementById('mainPanel_gender').value);">
    <form id="form1" runat="server">
    <div style="margin-top:50px;">
        <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
         ClientInstanceName="mainPanel" OnCallback="mainPanel_Callback"> 
         <ClientSideEvents EndCallback="function(s,e) { ubahjenis(document.getElementById('mainPanel_gender').value); 
			jQuery(function($){
				$('#mainPanel_npwp').mask('99.999.999.9-999.999');
			});
		 }" />       
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
        <table width="100%" class="Box1">
            <tr>
                <td colspan="2" class="H1">request checking</td>
            </tr>
            <tr>
                <td width="50%">
                    <table width="100%">
                        <tr>
                            <td class="B01">Request ID</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:Label ID="requestid" Text="(autogenerated)" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="B01">Segmen</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:DropDownList ID="productid" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table width="100%">
                        <tr>
                            <td class="B01">Cabang</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:DropDownList ID="branchid" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="B01">Tujuan Checking</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:DropDownList ID="purpose" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="H1">Informasi Debitur</td>
            </tr>
            <tr>
                <td width="50%">
                    <table width="100%">
						<tr>
                            <td class="B01">Jenis Customer</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:RadioButtonList ID="cust_type" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                <asp:ListItem Text="Individu" Value="IND" onclick="ubahjenis(this.value)"></asp:ListItem>
                                <asp:ListItem Text="Perusahaan" Value="PSH" onclick="ubahjenis(this.value)"></asp:ListItem>
                                            </asp:RadioButtonList></td>
                        </tr>
                        <tr id="tr_nationality" runat="server">
                            <td class="B01">Kewarganegaraan</td>
                            <td class="BS"></td>
                            <td class="B11">
                                <asp:RadioButtonList ID="nationality" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                    <asp:ListItem Text="WNI" Value="WNI" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="WNA" Value="WNA"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="B01">Nama Lengkap</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="cust_name" runat="server" CssClass="mandatory" Width="260px" MaxLength="100"></asp:TextBox>
                                <input id="btnlookup" type="button" class="btn btn-xs btn-warning" style="color:#00573D" runat="server" value="Cari.." onclick="callbackpopup(PopFindExisting, PNL_FindExisting, 'n:')" />
                            </td>
                        </tr>
                        <tr>
                            <td class="B01">Tgl Lahir / Pendirian</td>
                            <td class="BS"></td>
                            <td class="B11"><dxp:ASPxDateEdit ID="dob" runat="server" CssClass="mandatory" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy"></dxp:ASPxDateEdit></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor KTP / Paspor / Akta</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="ktp" runat="server" CssClass="mandatory" Width="180px" MaxLength="16" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor NPWP</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox name="npwp" id="npwp" runat="server" Width="180px" MaxLength="20" onkeypress="javascript:return isNumber(event)" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table width="100%">                        
                        <tr>
                            <td class="B01">Tempat Lahir/Pendirian</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="pob" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr id="tr_gender" runat="server">
                            <td class="B01">Jenis Kelamin</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:RadioButtonList ID="gender" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                <asp:ListItem Text="Laki-laki" Value="M"></asp:ListItem>
                                <asp:ListItem Text="Perempuan" Value="F"></asp:ListItem>
                                            </asp:RadioButtonList></td>
                        </tr>
                        <tr id="tr_marital" runat="server">
                            <td class="B01">Status Pernikahan</td>
                            <td class="BS"></td>
                            <td class="B11">
                                <asp:RadioButtonList ID="marital_status" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                    <asp:ListItem Text="Single" Value="single" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Married" Value="married"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="tr_mother_name" runat="server">
                            <td class="B01">Nama Ibu Kandung</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="mother_name" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Alamat</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="homeaddress" runat="server" TextMode="MultiLine" Rows="2" Width="300px" MaxLength="250"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Kota</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="homecity" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor Telp</td>
                            <td class="BS"></td>
                            <td class="B11"><asp:TextBox ID="phonenumber" runat="server" Width="120px" MaxLength="15" onkeypress="javascript:return isNumber(event)" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="button" id="btn_save" value="Save" class="btn btn-xs btn-warning" style="color:#00573D" onclick="if (validasiktp()) callback(mainPanel, 's'); " />
                    <input type="button" id="btn_del" runat="server" style="display:none;color:#00573D;" value="Delete" class="btn btn-xs btn-warning" onclick="if (confirm('Hapus data permintaan checking?')) callback(mainPanel, 'd');" />
                </td>
            </tr>
            <tr id="tr_suppheader" runat="server" style="display:none">
                <td colspan="2" class="H1">Permintaan Checking Lainnya/Tambahan</td>
            </tr>
            <tr id="tr_supplement" runat="server" style="display:none">
                <td colspan="2">
                    <dxwgv:ASPxGridView ID="GridViewSuppl" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewSuppl" KeyFieldName="seq" OnCustomCallback="GridViewSuppl_CustomCallback"
                        OnLoad="GridViewSuppl_Load">
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Nama" FieldName="cust_name" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Hubungan" FieldName="relation" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tgl Lahir/Pendirian" FieldName="dob" >
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nomor KTP/Akta" FieldName="ktp" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="NPWP" FieldName="npwp"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Alamat" FieldName="homeaddress"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Kota" FieldName="homecity"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Telp" FieldName="phonenumber"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Width="1%" CellStyle-Wrap="False">
                                <HeaderTemplate>
                                    <input id="button2" runat="server" type="button" value="New" onclick="callbackpopup(PopupSID, PanelSID, 'r:', null, null, false);jQuery(function($){$('#PopupSID_PanelSID_supp_npwp').mask('99.999.999.9-999.999');});" class="btn btn-xs btn-success" />
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <input id="Button1" type="button" value="Edit" alt="<%# Container.KeyValue %>" onclick="callbackpopup(PopupSID, PanelSID, 'r:' + this.alt)" class="btn btn-xs btn-warning" style="color:#00573D" />
                                    <input id="Button3" type="button" value="Delete" alt="<%# Container.KeyValue %>" onclick="if (confirm('Hapus data?')) callback(GridViewSuppl, 'd:' + this.alt)" class="btn btn-xs btn-warning" style="color:#00573D" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" />
                        <SettingsBehavior AllowGroup="False" />
                    </dxwgv:ASPxGridView>

                </td>
            </tr> 
        </table><br />
        <table width="100%">
            <tr id="tr_submit" runat="server" style="display:none">
                <td align="center">
                    <input type="button" id="btn_submit" value="Submit" class="btn btn-xs btn-success" onclick="if (confirm('Submit request!! Pastikan data yang diinput sudah benar??')) callback(mainPanel, 'u')" />
                </td>
            </tr>
        </table>
        <table width="100%" id="tbl_history" runat="server" style="display:none">
            <tr><td><b>History</b></td></tr>
            <tr>
                <td>
                <asp:GridView ID="GRID_NOTES" runat="server" width="100%" CssClass="Dg1" AutoGenerateColumns="false">
                    <FooterStyle Font-Bold="true" ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle Font-Bold="true" ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <AlternatingRowStyle />
                    <RowStyle ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <Columns>
                       <asp:BoundField DataField="seq" HeaderText="No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" Visible="false"  />
                       <asp:TemplateField HeaderText="Tgl Masuk" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%#FormatedValue(Eval("in_date")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tgl Keluar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%#FormatedValue(Eval("out_date")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:BoundField DataField="userid" HeaderText="User ID" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"  />
                       <asp:BoundField DataField="sts_desc" HeaderText="Status" ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                       <asp:BoundField DataField="act_desc" HeaderText="Action" ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                       <asp:BoundField DataField="comment" HeaderText="Comment" ItemStyle-HorizontalAlign="left" />
                    </Columns>
                </asp:GridView>
                </td>
            </tr>
        </table>
            

        </dxp:PanelContent>
        </PanelCollection>
        </dxcp:ASPxCallbackPanel>

        <dxpc:ASPxPopupControl ID="PopupSID" ClientInstanceName="PopupSID" runat="server" HeaderText="Permintaan Checking Lainnya" width="800px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
        <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" >
            <dxcp:ASPxCallbackPanel ID="PanelSID" runat="server" ClientInstanceName="PanelSID" OnCallback="PanelSID_Callback">
			   <ClientSideEvents EndCallback="function(s,e) { jQuery(function($){$('#PopupSID_PanelSID_supp_npwp').mask('99.999.999.9-999.999');}); }" />
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent8" runat="server">

            <table width="100%" class="Box1">
                <tr>
                    <td width="50%">
                        <table width="100%">
                            <tr>
                                <td class="B01">Jenis</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:RadioButtonList ID="supp_cust_type" runat="server" RepeatDirection="Horizontal" CssClass="mandatory">
                                    <asp:ListItem Text="Individu" Value="IND" onclick="ubahjenis_supl(this.value)"></asp:ListItem>
                                    <asp:ListItem Text="Perusahaan" Value="PSH" onclick="ubahjenis_supl(this.value)"></asp:ListItem>
                                                </asp:RadioButtonList></td>
                            </tr>
                            <tr id="tr_supp_nationality" runat="server">
                                <td class="B01">Kewarganegaraan</td>
                                <td class="BS"></td>
                                <td class="B11">
                                    <asp:RadioButtonList ID="supp_nationality" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                        <asp:ListItem Text="WNI" Value="WNI" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="WNA" Value="WNA"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="B01">Hubungan dengan Debitur</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:DropDownList ID="status_app" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="B01">Nama Lengkap</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_cust_name" runat="server" CssClass="mandatory" Width="85%" MaxLength="100"></asp:TextBox>
                                    <input id="Button4" type="button" class="btn btn-xs btn-warning" style="color:#00573D" runat="server" value="..." onclick="callbackpopup(PopFindExisting, PNL_FindExisting, 'np:')" />
                                </td>
                            </tr>
                            <tr>
                                <td class="B01">Tgl Lahir/Pendirian</td>
                                <td class="BS"></td>
                                <td class="B11"><dxp:ASPxDateEdit ID="supp_dob" runat="server" CssClass="mandatory" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy"></dxp:ASPxDateEdit></td>
                            </tr>
                            <tr>
                                <td class="B01">Nomor KTP/Paspor/Akta</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_ktp" runat="server" CssClass="mandatory" Width="180px" MaxLength="16" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Nomor NPWP</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox name="supp_npwp" ID="supp_npwp" runat="server" Width="180px" MaxLength="20" onclick="javascript:jQuery(function($){$('#PopupSID_PanelSID_supp_npwp').mask('99.999.999.9-999.999');});" onfocus="jQuery(function($){$('#PopupSID_PanelSID_supp_npwp').mask('99.999.999.9-999.999');});" onkeypress="javascript:return isNumber(event)" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%" valign="bottom">
                        <table width="100%">                            
                            <tr>
                                <td class="B01">Tempat Lahir/Pendirian</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_pob" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr id="tr_supp_gender" runat="server">
                                <td class="B01">Jenis Kelamin</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:RadioButtonList ID="supp_gender" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                    <asp:ListItem Text="Laki-laki" Value="M"></asp:ListItem>
                                    <asp:ListItem Text="Perempuan" Value="F"></asp:ListItem>
                                                </asp:RadioButtonList></td>
                            </tr>
                            <tr id="tr_supp_mother_name" runat="server">
                                <td class="B01">Nama Ibu Kandung</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_mother_name" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Alamat</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_homeaddress" runat="server" TextMode="MultiLine" Rows="2" Width="300px" MaxLength="250"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Kota</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_homecity" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Nomor Telp</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="supp_phonenumber" runat="server" Width="120px" MaxLength="15" onkeypress="javascript:return isNumber(event)" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <input runat="server" ID="BTN_SAVE1" type="button" class="btn btn-xs btn-warning" style="color:#00573D"
                            onclick="if (validasiktp_supl()) callbackpopup(PopupSID, PanelSID, 's:', GridViewSuppl, true);" value=" Save " />
                        <input ID="BTN_CANCEL1" runat="server" class="btn btn-xs btn-warning" style="color:#00573D"
                            onclick="PopupSID.Hide();" type="button" value="Cancel" />
                        <input type="hidden" ID="seq" runat="server" />
                    </td>
                </tr>
            </table>
             </dxp:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl>
    </ContentCollection>
    </dxpc:ASPxPopupControl>

        <dxpc:ASPxPopupControl ID="PopFindExisting" ClientInstanceName="PopFindExisting" runat="server" HeaderText="Pencarian Data Existing" width="750px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
        <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" >
            <dxcp:ASPxCallbackPanel ID="PNL_FindExisting" runat="server" ClientInstanceName="PNL_FindExisting" OnCallback="PNL_FindExisting_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent2" runat="server">

            <table width="100%" class="Box1">
                <tr>
                    <td>
                        <table width="100%">
                            <tr style="display:none">
                                <td class="B01">Sumber</td>
                                <td class="BS"></td>
                                <td class="B11">
                                    <asp:RadioButtonList ID="find_source" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                        <asp:ListItem Text="Existing Data" Value="EXS"></asp:ListItem>
                                        <asp:ListItem Text="Whitelist" Value="WHL"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="B01">Nama Customer</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="find_name" runat="server" Width="200px" MaxLength="100" onkeydown="if (event.keyCode == 13) { callback(PNL_FindExisting, 'f:',false); }" ></asp:TextBox></td>
                            </tr>
                            <tr style="display:none">
                                <td class="B01">Requestid/Whitelistid</td>
                                <td class="BS"></td>
                                <td class="B11"><asp:TextBox ID="find_reqid" runat="server" Width="200px" MaxLength="20" onkeydown="if (event.keyCode == 13) { callback(PNL_FindExisting, 'f:',false); }"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">&nbsp;</td>
                                <td class="BS"></td>
                                <td class="B11"><input type="button" value="Cari" class="btn btn-xs btn-warning" style="color:#00573D" onclick="callback(PNL_FindExisting, 'f:', false);" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="GridFind" runat="server" width="100%" CssClass="Dg1" AutoGenerateColumns="false" ShowHeader="true">
                        <FooterStyle Font-Bold="true" ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderStyle Font-Bold="true" ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <AlternatingRowStyle />
                        <RowStyle ForeColor="black" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                           <asp:BoundField DataField="cust_name" HeaderText="Nama" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"  />
                           <asp:BoundField DataField="ktp" HeaderText="No KTP" ItemStyle-HorizontalAlign="left" ItemStyle-Width="15%"  />
                           <asp:TemplateField HeaderText="Tgl Lahir" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                           <ItemTemplate>
                                <asp:Label ID="lbl3" runat="server" Text=<%#FormatedValue(Eval("dob"),"dd/MM/yyyy")%> />
                           </ItemTemplate>
                           </asp:TemplateField>
                           <asp:BoundField DataField="npwp" HeaderText="NPWP" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%"  />
                           <asp:TemplateField HeaderText="Tgl Input" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12%">
                           <ItemTemplate>
                                <asp:Label ID="lbl3" runat="server" Text=<%#FormatedValue(Eval("inputdate"),"dd/MM/yyyy")%> />
                           </ItemTemplate>
                           </asp:TemplateField>
                           <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                            <ItemTemplate>
                                <input type="button" id="btn3" class="btn btn-xs btn-success" runat="server" value="Ambil" alt=<%# Eval("requestid") %>
                                    onclick="callbackpopup(PopFindExisting, PNL_FindExisting, 'f:', mainPanel, 'g:' + this.alt, false);" />
                            </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                            <ItemTemplate>
                            <input type="button" id="btn4" class="btn btn-xs btn-success" runat="server" value="Ambil" alt=<%# Eval("requestid") %>
                                    onclick="callbackpopup(PopFindExisting, PNL_FindExisting, 'fp:', PanelSID, 'gp:' + this.alt, false);" />
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblnotfound" runat="server" ForeColor="Red" Font-Bold="true" Visible="false" Text="Data tidak ditemukan"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <input ID="Button5" runat="server" class="btn btn-xs btn-warning" style="color:#00573D" onclick="PopFindExisting.Hide();" type="button" value="Cancel" />
                        <input type="hidden" ID="searchsup" runat="server" />
                    </td>
                </tr>
            </table>

             </dxp:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </dxpc:PopupControlContentControl>
    </ContentCollection>
    </dxpc:ASPxPopupControl>

    </div>
    </form>

	<script>
	jQuery(function($){
        $("#mainPanel_npwp").mask("99.999.999.9-999.999");
        $("#mainPanel_dob").mask("31/12/9999");
        $("#PopupSID_PanelSID_supp_dob_I").mask("31/12/9999");
        /*
        $('#mainPanel_dob').datetimepicker({
            //language: 'fr',
            ignoreReadonly: true,
            format: 'DD/MM/YYYY',
            showTodayButton: true,
            //pickTime: false
        });*/
    });

	</script>
</body>
</html>
