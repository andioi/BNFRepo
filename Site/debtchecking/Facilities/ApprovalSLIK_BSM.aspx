<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalSLIK.aspx.cs" Inherits="DebtChecking.Facilities.ApprovalSLIK" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Approval IDEB Checking</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body >
    <form id="form1" runat="server">
    <div style="margin-top:50px;">
        <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" 
         ClientInstanceName="mainPanel" OnCallback="mainPanel_Callback">        
        <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
        <table width="100%" class="Box1">
            <tr>
                <td colspan="2" class="H1">request iDEB checking</td>
            </tr>
            <tr>
                <td width="50%">
                    <table width="100%">
                        <tr>
                            <td class="B01">Request ID</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:Label ID="requestid" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="B01">Request By</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="inputby" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Request Date</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="reqdate" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table width="100%">
                        <tr>
                            <td class="B01">Jenis Produk</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="productdesc" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Cabang</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:Label ID="branchname" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Tujuan iDEB Checking</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="purposedesc" runat="server"></asp:Label></td>
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
                            <td class="B11"><asp:Label ID="cust_name" runat="server" Font-Bold="true"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Tgl Lahir/Pendirian</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="dob" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Tempat Lahir</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="pob" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor KTP / Akta</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="ktp" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Jenis Customer</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:Label ID="cust_type" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor NPWP</td>
                            <td class="BS">:</td>
                            <td class="B11"><asp:Label ID="npwp" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table width="100%">                                                                      
                        <tr id="tr_gender" runat="server">
                            <td class="B01">Jenis Kelamin</td>
                            <td class="B11">:</td>
                            <td class="B11">                                
                                <asp:Label ID="gender_desc" runat="server"></asp:Label>
                            </td>
                        </tr>  
                        <tr id="tr_mother_name" runat="server">
                            <td class="B01">Nama Ibu Kandung</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="mother_name" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Alamat</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="homeaddress" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="B01">Nomor Telp</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="phonenumber" runat="server"></asp:Label></td>
                        </tr>
                        <tr style="display:none">
                            <td class="B01">Status Whitelist</td>
                            <td class="B11">:</td>
                            <td class="B11"><asp:Label ID="status_wl" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr id="tr_suppheader" runat="server">
                <td colspan="2" class="H1">Permintaan iDEB Checking Lainnya/Tambahan</td>
            </tr>
            <tr id="tr_supplement" runat="server">
                <td colspan="2">
                    <dxwgv:ASPxGridView ID="GridViewSuppl" runat="server" Width="100%" AutoGenerateColumns="False" 
                        ClientInstanceName="GridViewSuppl" KeyFieldName="seq" OnLoad="GridViewSuppl_Load">
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Nama" FieldName="cust_name" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Hubungan" FieldName="relation" ></dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Tgl Lahir/Pendirian" FieldName="dob" >
                                <PropertiesTextEdit DisplayFormatString="dd/MMM/yyyy"></PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nomor KTP/Akta" FieldName="ktp" ></dxwgv:GridViewDataTextColumn>
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
        <table width="100%">
            <tr id="tr_submit" runat="server">
                <td align="center">
                    <input type="button" id="btn_back" value="Reverse" class="btn btn-xs btn-warning" onclick="if (confirm('Reverse, anda yakin?')) callbackpopup(PopupSID, PanelSID, 'v');" runat="server" style="color:#00573D" />
                    <input type="button" id="btn_apprv" value="Approve" class="btn btn-xs btn-warning" onclick="if (confirm('Approve, anda yakin?')) callback(mainPanel, 'a');" runat="server" style="color:#00573D" />
                    <input type="button" id="btn_reject" value="Reject" class="btn btn-xs btn-warning" onclick="if (confirm('Reject, anda yakin?')) callbackpopup(PopupSID, PanelSID, 'r');" runat="server" style="color:#00573D" />
                </td>
            </tr>
        </table>

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

        <dxpc:ASPxPopupControl ID="PopupSID" ClientInstanceName="PopupSID" runat="server" HeaderText="Comment" width="600px" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
            CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" >
            <dxcp:ASPxCallbackPanel ID="PanelSID" runat="server" ClientInstanceName="PanelSID" OnCallback="PanelSID_Callback">
            <PanelCollection>
            <dxp:PanelContent ID="PanelContent8" runat="server">

                <table width="100%" class="Box1">
                <tr>
                    <td width="100%">
                        <asp:TextBox runat="server" ID="comment" CssClass="mandatory" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <input type="button" id="btn_confirm" runat="server" value="  O K  " class="btn btn-xs btn-warning" style="color:#00573D" />
                        <input type="button" id="Button2" value="Cancel" class="btn btn-xs btn-warning" onclick="PopupSID.Hide()" style="color:#00573D" />
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
