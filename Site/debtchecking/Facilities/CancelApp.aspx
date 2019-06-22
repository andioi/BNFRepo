<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelApp.aspx.cs" Inherits="DebtChecking.Facilities.CancelApp" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>


<%@ Register TagPrefix="uc1" TagName="UC_GeneralInfo" Src="../CommonForm/UC_GeneralInfo.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CancelApp Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <dxcp:ASPxCallbackPanel ID="mainPanel" ClientInstanceName="mainPanel" runat="server" Width="100%" oncallback="mainPanel_Callback">
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
    <table width="100%" class="Box1">
        <tr>
            <td>
                <uc1:UC_GeneralInfo ID="gi" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <table class="Tbl0">
                    <tr valign="top">
                        <td width="50%">
                            <table class="Tbl0">
                                <tr class="H1">
                                    <td colspan="3">Customer Info</td>
                                </tr>
                                <tr>
                                    <td class="B01">Nama sesuai KTP</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfc_CU_NAME" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Tanggal Lahir</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfc_CU_BORNDATE" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Nomor KTP</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfc_CU_KTPNO" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Telepon Rumah</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfc_CU_HMPHN" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%">
                            <table class="Tbl0">
                                <tr class="H1">
                                    <td colspan="3">Product Info</td>
                                </tr>
                                <tr>
                                    <td class="B01">Jenis Pengajuan</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfd_APPTYPEDESC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Produk</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfd_PROD_TYPEDESC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Kategori Produk</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfd_PRODCATDESC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Jenis Kredit</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="dfd_PRODUCTDESC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table class="Tbl0">
                    <tr valign="top">
                        <td width="40%">
                            <table class="Tbl0">
                                <tr>
                                    <td class="B01">Current Stage</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:Label ID="df_TR_DESC" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Cancel Type</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:DropDownList ID="df_TR_CODE" runat="server" CssClass="mandatory"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="B01">Cancel Reason</td>
                                    <td class="BS">:</td>
                                    <td class="B11">
                                        <asp:DropDownList ID="df_CN_CODE" runat="server" CssClass="mandatory"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="F1">
            <td>
                <input id="btn_cancel" runat="server" type="button" class="Bt2" onclick="if (confirmupdate('Cancel Aplikasi?')) callback(mainPanel,'cancel')" value="Cancel Aplikasi"></input>
            </td>
        </tr>
    </table>
    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>  
    
    </div>
    </form>
</body>
</html>
