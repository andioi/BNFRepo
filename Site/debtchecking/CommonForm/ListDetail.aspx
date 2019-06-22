<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListDetail.aspx.cs" Inherits="DebtChecking.List.ListDetail" %>

<%@ Register src="UC_ListDetail.ascx" tagname="UC_ListDetail" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ListDetail Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <uc1:UC_ListDetail ID="ld" runat="server" />
        
    </div>
    </form>
</body>
</html>
