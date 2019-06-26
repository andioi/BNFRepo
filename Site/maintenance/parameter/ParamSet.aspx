<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParamSet.aspx.cs" Inherits="MikroMnt.Parameter.ParamSet" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" TagPrefix="dxuc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ParamSet Page</title>
	<link href="../include/style.css" type="text/css" rel="stylesheet" />
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:fixed;top:0;right:0;margin-right:20px;margin-top:5px;">
        <!-- Row start -->
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row rightinfo">
                    <a href="../main.html" class="btn btn-warning" ><span class="glyphicon glyphicon-home"></span> Mainmenu </a> 
                    <a href="../Logout.aspx" class="btn btn-warning"><span class="glyphicon glyphicon-log-out"></span> Logout </a>

                </div>
            </div>
        </div>
        <!-- Row end -->
    </div>
    <div style="margin-top:60px;">
        <table class="Box1" width="100%">
            <tr class="H1">
                <td>
                    <b><asp:Label ID="LBL_TITLE" runat="server"></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td>
                    <dxuc:ASPxDataView ID="dataView" runat="server" onload="dataView_Load" >
                    <ItemStyle Height="1%" />
                    <ItemTemplate> 
                        <asp:HyperLink ID="lnk" runat="server" Text='<%# Eval("PARAMDESC") %>' NavigateUrl='<%# Eval("PARAMLINK") %>' ></asp:HyperLink>
                    </ItemTemplate>
                    <PagerSettings>
                        <AllButton Visible="True"></AllButton>
                    </PagerSettings>
                    </dxuc:ASPxDataView>
                </td>
            </tr>
        </table>
        
    </div>
    </form>
</body>
</html>
