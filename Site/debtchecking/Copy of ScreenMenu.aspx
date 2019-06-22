<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenMenu.aspx.cs" Inherits="WebMikro.ScreenMenu" %>

<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxMenu" tagprefix="dxm" %>
<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v8.2" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Menu Page</title>
    <link href="include/menuStyle.css" type="text/css" rel="Stylesheet" />
    <link href="include/screenmenu.css" type="text/css" rel="Stylesheet" />
    <script src="include/menu.js" language="javascript" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function init()
        {
            menupick(MenuParent,MenuParent.GetItem(0));
            if(MenuParent.GetItemCount()>1) 
	           document.getElementById('trscrmn').style.display = '';
	        else
	           document.getElementById('trscrmn').style.display = 'none';
            MenuPanel.PerformCallback(MenuParent.GetItem(0).name);	
        }
    </script>
</head>
<body onload="init();">
    <form id="form1" runat="server" >
    <div >
        <table class="Tbl0">
            <tr valign="middle">
                <td style="width:70%" valign="middle">
                    <table border="0" width="350" bgcolor="#245984" id="I1" runat="server" visible="false"
                        style="margin-left:3px;">
                        <tr>
                            <td>
                                <b><asp:Label ID="TitleHeader" runat="server" ForeColor="White" Font-Underline="true"></asp:Label></b>
                            </td>
                        </tr>
				        <tr valign="top">
		                    <td align="left">
		                        <font color="#ffffff" face="tahoma" size="1">
		                            <asp:Label id="l1" runat="server">Nama Debitur : </asp:Label>
		                            <asp:Label id="l2" runat="server"></asp:Label>
				                </font>
		                    </td>
		                </tr>
		                <tr>
		                    <td align="left" valign="top">
		                        <font color="#ffffff" face="tahoma" size="1">
				                    <asp:Label id="l3" runat="server">No. Aplikasi : </asp:Label>
				                    <asp:Label id="l4" runat="server"></asp:Label>
				                </font>
		                    </td>
				        </tr>
			        </table>
                </td>
                <td align="right" valign="middle">
                    <a id="HrefBack" runat="server" style="text-decoration:none;"><img alt="" src="image/backward.png" border="0" /> Back</a>&nbsp;&nbsp;&nbsp;
                    <a href="Body.aspx" style="text-decoration:none;"><img alt="Mainmenu" src="image/mainmenu.png" border="0" /> Mainmenu</a>&nbsp;&nbsp;&nbsp;
                    <a href="Logout.aspx" style="text-decoration:none;"><img alt="Logout" src="image/logout.png" border="0" /> Logout</a>
                </td>
            </tr>
        </table>
        <table class="Tbl0">
            <tr id="trscrmn" runat="server" align="center" >
                <td>
                    <div id="menu-grayscale">
                        <ul>
                            <li><a id="linkupload" runat="server" target="framex">Uploaded Files</a></li>		
		                    <li><a id="linkmemo" runat="server" target="framex">Memo</a></li>
		                </ul>
		            </div>
                    <div style="position:relative;float:none;margin-left:0px;">
                    <dxm:ASPxMenu ID="MenuParent" runat="server" AllowSelectItem="True" ClientInstanceName="MenuParent" 
                        DisabledStyle-CssClass="S1" DisabledStyle-ForeColor="#084e94" 
                        DisabledStyle-Font-Bold="true">
                        <ClientSideEvents ItemClick="function(s, e) {
                            menupick(s,e.item);
                            document.getElementById('trscrmnchild').style.display = '';
                            MenuPanel.PerformCallback(e.item.name);	
                            }" />
                    </dxm:ASPxMenu>
                    </div>
                </td>
            </tr>
            <tr id="trscrmnchild" runat="server" align="center">
                <td>
                    <dxcp:ASPxCallbackPanel ID="MenuPanel" runat="server" 
                        ClientInstanceName="MenuPanel" 
                        oncallback="MenuPanel_Callback">
                        <ClientSideEvents EndCallback="function(s, e) {
	try
	{
	    if(document.getElementById('framex').src=='')
	    {
            frameurl(MenuChild.GetItem(0).GetNavigateUrl());
            MenuChild.GetItem(0).SetEnabled(false);
	    }
	    if(MenuChild.GetItemCount()&gt;1) 
            document.getElementById('trscrmnchild').style.display = '';
	    else
	        document.getElementById('trscrmnchild').style.display = 'none';
	}
	catch(e)
	{
	    document.getElementById('trscrmnchild').style.display = 'none';
	}
}" />
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent1" runat="server">
                            <dxm:ASPxMenu ID="MenuChild" runat="server" AllowSelectItem="True" ClientInstanceName="MenuChild" 
                                DisabledStyle-CssClass="S1" DisabledStyle-ForeColor="#084e94" 
                                    DisabledStyle-Font-Bold="true"> 
                            <ClientSideEvents ItemClick="function(s, e) {
    menupick(s,e.item);
}" />
                            </dxm:ASPxMenu>
                            </dxp:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
        <table class="Tbl0">
            <tr>
                <td align="center">
                    <iframe id="framex" name="framex" onload="resizeFrame()" frameborder="0" width="100%"></iframe>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
