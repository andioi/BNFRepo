<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="DebtChecking.CommonForm.FileUpload" %>

<%@ Register src="../CommonForm/UC_UploadFile.ascx" tagname="UC_UploadFile" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>File Upload Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <uc3:UC_UploadFile ID="uf1" runat="server" MaxFiles="1" Title="Laporan Verifikasi" Cabinet="1_VER" />
        <uc3:UC_UploadFile ID="uf2" runat="server" MaxFiles="1" Title="Laporan Penilaian Agunan" Cabinet="2_COL" />
        <uc3:UC_UploadFile ID="uf3" runat="server" MaxFiles="3" Title="Lainnya" Cabinet="3_LAIN" />
    
    </div>
    </form>
</body>
</html>
