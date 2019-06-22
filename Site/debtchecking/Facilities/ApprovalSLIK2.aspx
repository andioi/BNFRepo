<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalSLIK2.aspx.cs" Inherits="DebtChecking.Facilities.ApprovalSLIK2" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>
<%@ Register assembly="DMSControls" namespace="DMSControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Request SLIK Checking</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    
    <script type="text/javascript" language="javascript">
        function ubahjenis(val) {
            if (val == "IND") {
                document.getElementById("mainPanel_npwp").className = "";
                document.getElementById("mainPanel_gender").className = "mandatory";

                document.getElementById("mainPanel_tr_gender").style.display = "";
                document.getElementById("mainPanel_tr_mother_name").style.display = "";

            } else if (val == "PSH") {
                document.getElementById("mainPanel_npwp").className = "mandatory";
                document.getElementById("mainPanel_gender").className = "";

                document.getElementById("mainPanel_tr_gender").style.display = "none";
                document.getElementById("mainPanel_tr_mother_name").style.display = "none";
            }
        }
        function ubahjenis_supl(val) {
            if (val == "IND") {
                document.getElementById("PopupSID_PanelSID_supp_npwp").className = "";
                document.getElementById("PopupSID_PanelSID_supp_gender").className = "mandatory";

                document.getElementById("PopupSID_PanelSID_tr_supp_gender").style.display = "";
                document.getElementById("PopupSID_PanelSID_tr_supp_mother_name").style.display = "";

            } else if (val == "PSH") {
                document.getElementById("PopupSID_PanelSID_supp_npwp").className = "mandatory";
                document.getElementById("PopupSID_PanelSID_supp_gender").className = "";

                document.getElementById("PopupSID_PanelSID_tr_supp_gender").style.display = "none";
                document.getElementById("PopupSID_PanelSID_tr_supp_mother_name").style.display = "none";
            }
        }

        function validasiktp() {
            var ret = true
            var noktp = document.getElementById("mainPanel_ktp").value;
            var nonpwp = document.getElementById("mainPanel_npwp").value;
            if (document.getElementById("mainPanel_cust_type_0").checked) {
                if (noktp.length < 10) {
                    alert("No KTP tidak valid!");
                    ret = false;
                }
            }
            if (document.getElementById("mainPanel_cust_type_1").checked) {
                if (nonpwp.length < 15) {
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

            if (document.getElementById("PopupSID_PanelSID_supp_cust_type_0").checked) {
                if (noktp.length < 10) {
                    alert("No KTP tidak valid!");
                    ret = false;
                }
            }
            if (document.getElementById("PopupSID_PanelSID_supp_cust_type_1").checked) {
                if (nonpwp.length < 15) {
                    alert("No NPWP tidak valid!");
                    ret = false;
                }
            }
            return ret
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:50px;">
        <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
         ClientInstanceName="mainPanel" OnCallback="mainPanel_Callback">        
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
        <table width="100%" class="Box1">
            <tr>
                <td colspan="2" class="H1">request SLIK checking</td>
            </tr>
            <tr>
                <td width="50%">
                    <table width="100%">
                        <tr>
                            <td class="B01">Request ID</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:Label ID="requestid" Text="(autogenerated)" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="B01">Jenis Produk</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:DropDownList ID="productid" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table width="100%">
                        <tr>
                            <td class="B01">Cabang</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:DropDownList ID="branchid" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="B01">Tujuan SLIK Checking</td>
                            <td class="B11">:</td>
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
                            <td class="B01">Nama Customer</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:TextBox ID="cust_name" runat="server" CssClass="mandatory" Width="95%" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Tgl Lahir/Pendirian</td>
                            <td class="B11">:</td>
                            <td class="B11"><cc1:CC_Date ID="dob" runat="server" CssClass="mandatory" ImgShown="false"></cc1:CC_Date></td>
                        </tr>
                        <tr>
                            <td class="B01">Tempat Lahir/Pendirian</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:TextBox ID="pob" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor KTP / Akta</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:TextBox ID="ktp" runat="server" CssClass="mandatory" Width="180px" MaxLength="20" onkeypress="return digitsonly();" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Jenis Customer</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:RadioButtonList ID="cust_type" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                <asp:ListItem Text="Individu" Value="IND" onclick="ubahjenis(this.value)"></asp:ListItem>
                                <asp:ListItem Text="Perusahaan" Value="PSH" onclick="ubahjenis(this.value)"></asp:ListItem>
                                            </asp:RadioButtonList></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor NPWP</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:TextBox ID="npwp" runat="server" Width="180px" MaxLength="20" onkeypress="return digitsonly();" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table width="100%">                        
                        <tr id="tr_gender" runat="server">
                            <td class="B01">Jenis Kelamin</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:RadioButtonList ID="gender" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                <asp:ListItem Text="Laki-laki" Value="M"></asp:ListItem>
                                <asp:ListItem Text="Perempuan" Value="F"></asp:ListItem>
                                            </asp:RadioButtonList></td>
                        </tr>
                        <tr id="tr_mother_name" runat="server">
                            <td class="B01">Nama Ibu Kandung</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:TextBox ID="mother_name" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Alamat</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:TextBox ID="homeaddress" runat="server" TextMode="MultiLine" Rows="2" Width="95%" MaxLength="250"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor Telp</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:TextBox ID="phonenumber" runat="server" Width="120px" MaxLength="15" onkeypress="return digitsonly();" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="B01">Status Whitelist</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="status_wl" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="button" id="btn_save" value="Save" class="btn btn-xs btn-danger" onclick="if (validasiktp()) callback(mainPanel, 's'); " runat="server" />
                    <input type="button" id="btn_aprv" value="Approve" class="btn btn-xs btn-danger" onclick="if (confirm('Approve request?')) callback(mainPanel, 'a')" runat="server" />
                    <input type="button" id="btn_aprv2" value="Approve" class="btn btn-xs btn-danger" onclick="if (confirm('Approve request?')) callbackpopup(PopupComment, PanelComment, 'a')" runat="server" />
                    <input type="button" id="btn_rjct" value="Reject" class="btn btn-xs btn-danger" onclick="if (confirm('Reject request?')) callbackpopup(PopupComment, PanelComment, 'r')" runat="server" />
                    <input type="button" id="btn_reverse" value="Reverse" class="btn btn-xs btn-danger" onclick="if (confirm('Reverse request to inputter?')) callbackpopup(PopupComment, PanelComment, 'v')" runat="server" />
                </td>
            </tr>
            <tr id="tr_suppheader" runat="server" style="display:none">
                <td colspan="2" class="H1">Permintaan SLIK Checking Lainnya/Tambahan</td>
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
                            <dxwgv:GridViewDataTextColumn Caption="Telp" FieldName="phonenumber"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Width="1%" CellStyle-Wrap="False">
                                <HeaderTemplate>
                                    <input id="button2" runat="server" type="button" value="New" onclick="callbackpopup(PopupSID, PanelSID, 'r:')" />
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <input id="Button1" type="button" value="Edit" alt="<%# Container.KeyValue %>" onclick="callbackpopup(PopupSID, PanelSID, 'r:' + this.alt)" />
                                    <input id="Button3" type="button" value="Delete" alt="<%# Container.KeyValue %>" onclick="if (confirm('Hapus data?')) callback(GridViewSuppl, 'd:' + this.alt)" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" />
                        <SettingsBehavior AllowGroup="False" />
                    </dxwgv:ASPxGridView>

                </td>
            </tr>
            <tr id="tr_blheader" runat="server">
                <td colspan="2" class="H1" style="color:red">Informasi VIP List</td>
            </tr>
            <tr id="tr_blacklist" runat="server">
                <td colspan="2">
                    <script>alert('Data request ini terdeteksi sama dengan tabel VIP List');</script>
                    <dxwgv:ASPxGridView ID="GridViewBlacklist" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewBlacklist" KeyFieldName="blacklistid" OnLoad="GridViewBlacklist_Load">
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Nama" FieldName="cust_name" PropertiesTextEdit-EncodeHtml="false"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Hubungan" FieldName="relation" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tgl Lahir" FieldName="dob" PropertiesTextEdit-EncodeHtml="false">
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nomor KTP" FieldName="ktp" PropertiesTextEdit-EncodeHtml="false"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="NPWP" FieldName="npwp"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tempat Lahir" FieldName="pob"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Alamat" FieldName="homeaddress"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="No Telp" FieldName="phonenumber"></dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" />
                        <SettingsBehavior AllowGroup="False" />
                    </dxwgv:ASPxGridView>

                </td>
            </tr>
            <tr id="tr_wlheader" runat="server">
                <td colspan="2" class="H1" style="color:green">Informasi Whitelist</td>
            </tr>
            <tr id="tr_whitelist" runat="server">
                <td colspan="2">
                    <dxwgv:ASPxGridView ID="GridViewWhitelist" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewWhitelist" KeyFieldName="whitelistid" OnLoad="GridViewWhitelist_Load">
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Nama" FieldName="cust_name" PropertiesTextEdit-EncodeHtml="false"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tgl Lahir" FieldName="dob" PropertiesTextEdit-EncodeHtml="false">
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nomor KTP" FieldName="ktp" PropertiesTextEdit-EncodeHtml="false"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Kantor Cabang" FieldName="branch_name"></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Receive Date" FieldName="receive_date" PropertiesTextEdit-EncodeHtml="false">
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy HH:ii"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" />
                        <SettingsBehavior AllowGroup="False" />
                    </dxwgv:ASPxGridView>

                </td>
            </tr>
        </table><br />
        <table id="tbl_cid" runat="server" style="display:none" width="50%">
            <tr>
                <td class="B01">Nomor CID CMS</td>
                <td class="BS">:</td>
                <td class="B11"><asp:Label ID="cid" runat="server"></asp:Label></td>
            </tr>
        </table>
        <center><input type="button" id="btn_interface" value="Send to CMS" onclick="if (confirm('Kirim data customer ini ke CMS?')) callback(mainPanel, 'i')" class="btn btn-xs btn-danger" runat="server" /></center>
        <table width="100%">
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

        <dxpc:ASPxPopupControl ID="PopupSID" ClientInstanceName="PopupSID" runat="server" HeaderText="Permintaan SLIK Checking Lainnya" width="750px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
        <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" >
            <dxcp:ASPxCallbackPanel ID="PanelSID" runat="server" ClientInstanceName="PanelSID" OnCallback="PanelSID_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent8" runat="server">

            <table width="100%" class="Box1">
                <tr>
                    <td width="50%">
                        <table width="100%">
                            <tr>
                                <td class="B01">Jenis</td>
                                <td class="BS">:</td>
                                <td class="B11"><asp:RadioButtonList ID="supp_cust_type" runat="server" RepeatDirection="Horizontal" CssClass="mandatory">
                                    <asp:ListItem Text="Individu" Value="IND" onclick="ubahjenis_supl(this.value)"></asp:ListItem>
                                    <asp:ListItem Text="Perusahaan" Value="PSH" onclick="ubahjenis_supl(this.value)"></asp:ListItem>
                                                </asp:RadioButtonList></td>
                            </tr>
                            <tr>
                                <td class="B01">Hubungan</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:DropDownList ID="status_app" runat="server" CssClass="mandatory"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="B01">Nama Customer</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:TextBox ID="supp_cust_name" runat="server" CssClass="mandatory" Width="95%" MaxLength="100"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Tgl Lahir/Pendirian</td>
                                <td class="B11">:</td>
                                <td class="B11"><cc1:CC_Date ID="supp_dob" runat="server" CssClass="mandatory"></cc1:CC_Date></td>
                            </tr>
                            <tr>
                                <td class="B01">Nomor KTP / Akta</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:TextBox ID="supp_ktp" runat="server" CssClass="mandatory" Width="180px" MaxLength="20" onkeypress="return digitsonly();" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Nomor NPWP</td>
                                <td class="BS">:</td>
                                <td class="B11"><asp:TextBox ID="supp_npwp" runat="server" Width="180px" MaxLength="20" onkeypress="return digitsonly();" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%" valign="bottom">
                        <table width="100%">                            
                            <tr>
                                <td class="B01">Tempat Lahir/Pendirian</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:TextBox ID="supp_pob" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr id="tr_supp_gender" runat="server">
                                <td class="B01">Jenis Kelamin</td>
                                <td class="BS">:</td>
                                <td class="B11"><asp:RadioButtonList ID="supp_gender" CssClass="mandatory" runat="server" RepeatDirection="Horizontal" >
                                    <asp:ListItem Text="Laki-laki" Value="M"></asp:ListItem>
                                    <asp:ListItem Text="Perempuan" Value="F"></asp:ListItem>
                                                </asp:RadioButtonList></td>
                            </tr>
                            <tr id="tr_supp_mother_name" runat="server">
                                <td class="B01">Nama Ibu Kandung</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:TextBox ID="supp_mother_name" runat="server" Width="180px" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Alamat</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:TextBox ID="supp_homeaddress" runat="server" TextMode="MultiLine" Rows="2" Width="95%" MaxLength="250"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="B01">Nomor Telp</td>
                                <td class="B11">:</td>
                                <td class="B11"><asp:TextBox ID="supp_phonenumber" runat="server" Width="120px" MaxLength="15" onkeypress="return digitsonly();" onpaste="if(parseInt(clipboardData.getData('Text')) != clipboardData.getData('Text')) return false;"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <input runat="server" ID="BTN_SAVE1" type="button" class="btn btn-xs btn-danger"
                            onclick="if (validasiktp_supl()) callbackpopup(PopupSID, PanelSID, 's:', GridViewSuppl);" value=" Save " />
                        <input ID="BTN_CANCEL1" runat="server" class="btn btn-xs btn-danger"
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

    <dxpc:ASPxPopupControl ID="PopupComment" ClientInstanceName="PopupComment" runat="server" HeaderText="Comment" width="600px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" >
            <dxcp:ASPxCallbackPanel ID="PanelComment" runat="server" ClientInstanceName="PanelComment" OnCallback="PanelComment_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent2" runat="server">

                <table width="100%" class="Box1">
                <tr>
                    <td width="100%">
                        <asp:TextBox runat="server" ID="comment" CssClass="mandatory" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <input type="button" id="btn_confirm" runat="server" value="  O K  " class="btn btn-xs btn-danger" />
                        <input type="button" id="Button2" value="Cancel" class="btn btn-xs btn-danger" onclick="PopupComment.Hide()" />
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
</body>
</html>
