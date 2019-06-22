<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenMenu.aspx.cs" Inherits="DebtChecking.ScreenMenu" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Menu Page</title>
    <link href="include/menuStyle.css" type="text/css" rel="Stylesheet" />
    <link href="include/screenmenu.css" type="text/css" rel="Stylesheet" />
    <script src="include/menu.js" language="javascript" type="text/javascript"></script>
    <!-- Bootstrap -->
    <link href="vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">

        // convert all characters to lowercase to simplify testing
        var agt = navigator.userAgent.toLowerCase();

        // *** BROWSER VERSION ***
        // Note: On IE5, these return 4, so use is_ie5up to detect IE5.
        var is_major = parseInt(navigator.appVersion);
        var is_minor = parseFloat(navigator.appVersion);

        var is_ie = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));
        var is_ie3 = (is_ie && (is_major < 4));
        var is_ie4 = (is_ie && (is_major == 4) && (agt.indexOf("msie 4") != -1));
        var is_ie4up = (is_ie && (is_major >= 4));
        var is_ie5 = (is_ie && (is_major == 4) && (agt.indexOf("msie 5.0") != -1));
        var is_ie5_5 = (is_ie && (is_major == 4) && (agt.indexOf("msie 5.5") != -1));
        var is_ie5up = (is_ie && !is_ie3 && !is_ie4);
        var is_ie5_5up = (is_ie && !is_ie3 && !is_ie4 && !is_ie5);
        var is_ie6 = (is_ie && (is_major == 4) && (agt.indexOf("msie 6.") != -1));
        var is_ie6up = (is_ie && !is_ie3 && !is_ie4 && !is_ie5 && !is_ie5_5);
        
        function init() {
            var ol = document.getElementById("ul1");
            var url = document.getElementsByTagName("li")[0].getAttribute("menuid");
            li2 = document.getElementsByTagName("li")[1];
            if (li2 == undefined) {
                ol.style.display = "none";
                document.getElementById('framex').src = url;
            }
            if (url == "" || url == null) {
				var li3 = document.getElementsByTagName("li")[0];
				if (li3 == undefined) var li3 = document.getElementsByTagName("li")[0];
                var menuid = li3.getAttribute("id");
                var menudesc = li3.getAttribute("menudesc");
                MenuPanel.PerformCallback(menuid);
                li3.className = "current-jungle";
                document.getElementById("last_menudesc").value = menudesc;
                document.getElementById("last_menuid").value = menuid;
                li3.innerHTML = menudesc;
            }
        }

        function clickparent(menuid, menudesc, menuurl) {
            last_menuid = document.getElementById("last_menuid").value;
            last_menudesc = document.getElementById("last_menudesc").value;
            if (last_menuid != "") {
                url = "<a href=\"javascript:clickparent('" + last_menuid + "','" + last_menudesc + "')\">" + last_menudesc + "</a>";
                document.getElementById(last_menuid).innerHTML = url;
            }
            var ol = document.getElementById("ul1");
            for (var i = 0; i < ol.childNodes.length; i++) {
                ol.childNodes[i].className = "";
            }
            var li = document.getElementById(menuid);
            li.className = "current-jungle";
            document.getElementById("last_menudesc").value = menudesc;
            document.getElementById("last_menuid").value = menuid;
            li.innerHTML = menudesc;
            MenuPanel.PerformCallback(menuid);
        }

        function clickchild(menuid, menudesc, menuurl) {
            last_menuid = document.getElementById("last_menuid_child").value;
            last_menudesc = document.getElementById("last_menudesc_child").value;
            last_menuurl = document.getElementById("last_menuurl_child").value;
            if (last_menuid != "") {
                url = "<a href=\"javascript:clickchild('" + last_menuid + "','" + last_menudesc + "','" + last_menuurl + "')\">" + last_menudesc + "</a>";
                document.getElementById(last_menuid).innerHTML = url;
            }
            var ol = document.getElementById("ul2");
            for (var i = 0; i < ol.childNodes.length; i++) {
                ol.childNodes[i].className = "";
            }
            var li = document.getElementById(menuid);
            li.className = "current-blue";
            document.getElementById("last_menudesc_child").value = menudesc;
            document.getElementById("last_menuid_child").value = menuid;
            document.getElementById("last_menuurl_child").value = menuurl;
            li.innerHTML = menudesc;
            document.getElementById("framex").src = menuurl;
        }

        function resizeFrame() {
            try {
                var oBody = framex.document.body;
                var oFrame = document.getElementById('framex');
                oFrame.style.width = "100%";
                var h = oBody.scrollHeight + (oBody.offsetHeight - oBody.clientHeight) + 20;
                if (h < 800)
                    h = 800;
                oFrame.height = h;
                oFrame.width = oBody.scrollWidth + (oBody.offsetWidth - oBody.clientWidth);
            }
            catch (e) {
                window.status = 'Error: ' + e.number + '; ' + e.description;
            }
        }

        var popupWindow = null;
        function PopupPage(href, width, height, scroll, onepop) {
            if (!is_ie6up) {
                href = href.replace("../", "");
            } 
            if (popupWindow != null) { popupWindow.close(); }
            if (width == null) width = screen.availWidth * 0.7;
            if (height == null) height = screen.availHeight * 0.7;
            var X = (screen.availWidth - width) / 2;
            var Y = (screen.availHeight - height) / 2;
            if (scroll == null) scroll = "yes";
            popupWindow = window.open(href, "", "height=" + height + "px,width=" + width + "px,left=" + X + ",top=" + Y +
			",status=no,toolbar=no,scrollbars=" + scroll + ",resizable=yes,titlebar=no,menubar=no,location=no,dependent=yes");

            var resize = false;
            var counter = 0;
            if (width == 0) {
                counter = 0;
                while (popupWindow.document.readyState != 'complete' && counter < 1000000)
                    counter++;

                if (popupWindow.document.readyState == 'interactive' || popupWindow.document.readyState == 'complete') {
                    if (popupWindow.document.body.scrollWidth < screen.availWidth * 0.9)
                        width = popupWindow.document.body.scrollWidth;
                    else
                        width = screen.availWidth * 0.9;
                    resize = true;
                }
            }
            if (height == 0) {
                counter = 0;
                while (popupWindow.document.readyState != 'complete' && counter < 1000000)
                    counter++;

                if (popupWindow.document.readyState == 'interactive' || popupWindow.document.readyState == 'complete') {
                    if (popupWindow.document.body.scrollHeight < screen.availHeight * 0.9)
                        height = popupWindow.document.body.scrollHeight;
                    else
                        height = screen.availHeight * 0.9;
                    resize = true;
                }
            }

            if (resize) {
                X = (screen.availWidth - width) / 2;
                Y = (screen.availHeight - height) / 2;
                popupWindow.resizeTo(width, height);
                popupWindow.moveTo(X, Y);
            }

            if (onepop == "no")
                popupWindow = null;
        }
    </script>
</head>
<body onload="init();">
    <form id="form1" runat="server" >
    <input type="hidden" id="last_menudesc" />
    <input type="hidden" id="last_menuid" />
    <input type="hidden" id="last_menudesc_child" />
    <input type="hidden" id="last_menuid_child" />
    <input type="hidden" id="last_menuurl_child" />
    <div style="padding:4px">
        <table class="Tbl0" cellpadding="1" cellspacing="1">
            <tr valign="middle">
                <td style="width:70%" valign="middle">
                    <table border="0" width="400" class="bg-warning" id="I1" runat="server" visible="false"
                        style="margin-left:3px;">
                        <tr>
                            <td>
                                <b><asp:Label ID="TitleHeader" runat="server" CssClass="text-danger" face="tahoma" ForeColor="#858687" Font-Underline="true"></asp:Label></b>
                            </td>
                        </tr>
				        <tr valign="top">
		                    <td align="left">
		                        <font color="#858687" face="tahoma" size="1">
		                            <asp:Label id="l1" runat="server"></asp:Label>
		                            <asp:Label id="l2" runat="server"></asp:Label>
				                </font>
		                    </td>
		                </tr>
		                <tr>
		                    <td align="left" valign="top">
		                        <font color="#858687" face="tahoma" size="1">
				                    <asp:Label id="l3" runat="server"></asp:Label>
				                    <asp:Label id="l4" runat="server"></asp:Label>
				                </font>
		                    </td>
				        </tr>
			        </table>
                </td>
            </tr>
        </table>
        <div style="position:fixed;top:0;right:0;margin-right:20px;margin-top:5px;">
                <!-- Row start -->
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="row rightinfo">
                            <a id="HrefBack" runat="server" class="btn btn-warning"> <span class="glyphicon glyphicon-chevron-left"></span> Back </a> 
                            <a href="main.html" class="btn btn-warning" target="_parent"><span class="glyphicon glyphicon-home"></span> Mainmenu </a> 
                            <a href="Logout.aspx" class="btn btn-warning" target="_parent"><span class="glyphicon glyphicon-log-out"></span> Logout </a>

                        </div>
                    </div>
                </div>
                <!-- Row end -->
            </div>
        <table class="Tbl0">
            <tr id="trscrmn" runat="server" align="center" >
                <td>
                    <div id="menu-jungle">
                        <ul id="ul1" style="list-style:none;">
                            <asp:Literal ID="Menu1" runat="server"></asp:Literal>
                        </ul>
                    </div>
                    <div id="menu-grayscale" style="display:none">
                        <ul id="ul0" runat="server">
                            <li><a id="linkksn" runat="server" title="Keterangan Singkat Nasabah" target="framex">KSN</a></li>
                            <li><a id="linklkslik" runat="server" title="Laporan Keuangan SLIK" target="framex">LK SLIK</a></li>
                            <li><a id="linksid" runat="server" title="SID Result" target="framex">SID</a></li>
                            <li><a id="linkbir" runat="server" title="Permohonan Persetujuan Kredit" target="framex">PPK</a></li>
                            <li><a id="linkupload" runat="server" title="Attached Files" target="framex">Files</a></li>		
		                    <li><a id="linkmemo" runat="server" title="Memo / Comment" target="framex">Memo</a></li>
		                    <li><a id="linktrack" runat="server" title="Track History" target="framex">Track</a></li>
		                </ul>
		            </div>
                </td>
            </tr>
            <tr id="trscrmnchild" runat="server" align="center">
                <td>
                    <dxcp:ASPxCallbackPanel ID="MenuPanel" runat="server" 
                        ClientInstanceName="MenuPanel" 
                        oncallback="MenuPanel_Callback">
                        <ClientSideEvents EndCallback="function(s,e) { 
                                url = document.getElementById('MenuPanel_h_startupurl').value;
                                menuid = document.getElementById('MenuPanel_h_startupid').value;
                                menudesc = document.getElementById('MenuPanel_h_startupdesc').value;
                                var li = document.getElementById(menuid);
                                li.className = 'current-blue';
                                document.getElementById('last_menudesc_child').value = menudesc;
                                document.getElementById('last_menuid_child').value = menuid;
                                document.getElementById('last_menuurl_child').value = url;
                                li.innerHTML = menudesc;
                                document.getElementById('framex').src = url;
                                var ol = document.getElementById('ul1');
                                if (ol.childNodes.length == 1 || (!is_ie6up && ol.childNodes.length == 3)) ol.style.display = 'none';
                            }" />
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent1" runat="server">
                            <div id="menu-blue">
                                <ul id="ul2">
                                    <asp:Literal ID="Menu2" runat="server"></asp:Literal>
                                </ul>
                            </div>
                            <input type="hidden" id="h_startupurl" runat="server" />
                            <input type="hidden" id="h_startupid" runat="server" />
                            <input type="hidden" id="h_startupdesc" runat="server" />
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
