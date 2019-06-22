<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadBatch.aspx.cs" Inherits="DebtChecking.SLIK.UploadBatch" %>
<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" TagPrefix="dxuc" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
     <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- Animate.css -->
    <link href="../vendors/animate.css/animate.min.css" rel="stylesheet" />
</head>
<body>
    <div align="center">
        <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="h_batchid" />
         <dxuc:ASPxCallbackPanel runat="server" ID="panelUploadFile" ClientInstanceName="panelUploadFile" OnCallback="panelUploadFile_Callback">
                <PanelCollection>
                    <dxuc:PanelContent runat="server" ID="panel10">
                       <table class="table table-bordered" id="tbl_upload" style="width:40%;">
                        <tr class="warning">
                            <td colspan="3" class="active" style="padding:15px"><div style="font-weight:bold;text-align:center">Upload Data Request SLIK</div></td>
                        </tr>
                        <tr>
                            <td style="padding:15px;" align="center">
                                <dxuc:ASPxUploadControl ID="ASPxUploadControl2" runat="server" UploadMode="Auto"
                                    ClientInstanceName="upload" ShowProgressPanel="true" ShowUploadButton="false" FileUploadMode="OnPageLoad"
                                    Width="400" OnFileUploadComplete="ASPxUploadControl2_FileUploadComplete">
                                    <AdvancedModeSettings EnableFileList="False" EnableDragAndDrop="True" />                                    
                                    <ValidationSettings MaxFileSize="3145728" AllowedFileExtensions=".xlsx" />
                                    <ClientSideEvents FileUploadComplete="function(s, e) {panelUploadFile.PerformCallback(e.callbackData);}"
                                       />
                                </dxuc:ASPxUploadControl>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:center">
                                <dxuc:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False"
                                        Text="Upload" ClientInstanceName="btnUpload" CssClass="btn btn-xs btn-danger" formnovalidate>
                                        <ClientSideEvents Click="function(s, e) {upload.UploadFile(); }"></ClientSideEvents>
                                    </dxuc:ASPxButton>
                                <dxuc:ASPxButton runat="server" ID="btnTemplate" CssClass="btn btn-xs btn-danger" Text="Download Template Upload"
                                    OnClick="btnTemplate_click"></dxuc:ASPxButton>
                                </td>
                        </tr>
                        <tr>
                            <td style="text-align:center">
                               <center><asp:TextBox ID="TXT_PROGRESS" runat="server" ReadOnly="true" Font-Bold="true" BorderStyle="None" Width="100%" Visible="false"> </asp:TextBox>
                                   <asp:HyperLink ID="txtlink" Text="Lihat hasil upload" runat="server" NavigateUrl="../ScreenMenu.aspx?sm=BATCH|DRFT&passurl&mntitle=Upload%20Result" Target="_parent" Visible="false"></asp:HyperLink>
                               </center>
                            </td>
                        </tr>
                    </table>

        </dxuc:PanelContent>
    </PanelCollection>
</dxuc:ASPxCallbackPanel>   

    </form>
    </div>
  
</body>
</html>
