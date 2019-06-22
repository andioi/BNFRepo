<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unittest.aspx.cs" Inherits="DebtChecking.unittest" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="include/style.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBox1" runat="server" CssClass="mandatory">ss0</asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
        <br />
        <br />
        <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Rows=5>cmd</asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="Button" onclick="Button2_Click" />
        
    </div>
    </form>
</body>
</html>
