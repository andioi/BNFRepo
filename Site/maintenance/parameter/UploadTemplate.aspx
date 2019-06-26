<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadTemplate.aspx.cs" Inherits="MikroMnt.Parameter.UploadTemplate" %>

<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- #include file="~/include/uc/UC_Number.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%">
        <tr>
            <td class="H0" align="right">
                <a href="ParamSet.aspx?set=ent&ismaker=1&title=Calculation Parameter"><img src="../image/back.jpg" alt="back" /></a>
                <a href="../Body.aspx" > <img alt="" src="../image/MainMenu.jpg" border="0" /> </a>
                <a href="../Logout.aspx" > <img alt="" src="../image/logout.jpg" border="0" /> </a>
            </td>
        </tr>
    </table>
    
    <dxcp:ASPxCallbackPanel ID="PanelFile" ClientInstanceName="PanelFile" runat="server" >
    <PanelCollection><dxp:PanelContent ID="PanelContent3" runat="server">
        <table width="50%" align="center" class="Box1">
            <tr>
                <td colspan="3" align="center" class="H1">Upload Calculator Template</td>
            </tr>
            <tr>
                <td class="B01">Catatan </td>
                <td class="BS">:</td>
                <td class="B11">
                    <asp:Label runat="server" ID="Label1" ForeColor="#336699">Pastikan nama sheet 
                    dari file yang akan diimport adalah &quot;New Loan&quot;</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="B01">Select File </td>
                <td class="BS">:</td>
                <td class="B11">
                     <dxuc:ASPxUploadControl ID="ImportFile" runat="server" ClientInstanceName="ImportFile"
                         OnFileUploadComplete="ImportFile_FileUploadComplete" Font-Size="X-Small" Width="100%">
                         <ClientSideEvents FileUploadComplete="function(s, e) { processing=false; callback(PanelFile,'r:',false); }"></ClientSideEvents>
                     </dxuc:ASPxUploadControl> 
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <input id="btnSave" runat="server" class="Bt1" onclick="
                    if (ImportFile.GetText() != '') 
                    {    if (!processing) 
                         {
                            processing=true; 
                            ImportFile.UploadFile();
                         };
                    }" type="button" value=" Upload "></input>
                    </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Label runat="server" ID="lbl_result" ForeColor="Red"></asp:Label>
            </tr>
        </table>
        
    </dxp:PanelContent></PanelCollection></dxcp:ASPxCallbackPanel>
    
    </div>    
    </form>
</body>
</html>
