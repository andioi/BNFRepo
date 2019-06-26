<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Body.aspx.cs" Inherits="MikroMnt.Body" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>Mainmenu</title>
		<link rel="stylesheet" type="text/css" href="include/ddsmoothmenu.css" />
		<script type="text/javascript" src="include/jquery.min.js"></script>
        <script type="text/javascript" src="include/ddsmoothmenu.js"></script>
		<script type="text/javascript">
		    ddsmoothmenu.init({
		        mainmenuid: "smoothmenu1", //menu DIV id
		        orientation: 'h', //Horizontal or vertical menu: Set to "h" or "v"
		        classname: 'ddsmoothmenu', //class added to menu's outer DIV
		        customtheme: ["#FFCB0B", "#00573D"],
                contentsource: "markup" //"markup" or ["container_id", "path_to_menu_file"]
		    })
        </script>
    <!-- Bootstrap -->
    <link href="vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

</head>
<body topmargin="0" leftmargin="0" bgcolor="#FFFFFF">
    <form id="form1" runat="server" enableviewstate="False">
    <div id="smoothmenu1" class="ddsmoothmenu">
    <asp:Literal ID="smoothmenu" runat="server"></asp:Literal>
    </div>
		<table id="tblmain" width="100%">
			<tr>
				<td align="center" valign="top">
					<table id="tblbody" cellspacing="0" cellpadding="0" width="100%" border="0" >
						<tr>
							<td align="right" colspan="3" valign="top">
								<asp:Label id="Label1" runat="server" Visible="False"></asp:Label>
								<asp:PlaceHolder id="PlaceHolder1" runat="server"></asp:PlaceHolder>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>	
    </form>
</body>
</html>
