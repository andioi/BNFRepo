<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MikroLogin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<link rel="shortcut icon" href="logo.ico" type="image/x-icon" />
<head runat="server">
    <title>CBAS - OCBC NISP</title>
		<script language="JavaScript" type="text/javascript">
			if (top != self) { top.location = self.location; }
			function kutip_satu()
			{
				if ((event.keyCode == 35) || (event.keyCode == 39))
				{
					return false;
				}
				else
				{
					return true;
				}
			}

			// convert all characters to lowercase to simplify testing
			var agt=navigator.userAgent.toLowerCase();

			// *** BROWSER VERSION ***
			// Note: On IE5, these return 4, so use is_ie5up to detect IE5.
			var is_major = parseInt(navigator.appVersion);
			var is_minor = parseFloat(navigator.appVersion);

			var is_ie     = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));
			var is_ie3    = (is_ie && (is_major < 4));
			var is_ie4    = (is_ie && (is_major == 4) && (agt.indexOf("msie 4")!=-1) );
			var is_ie4up  = (is_ie && (is_major >= 4));
			var is_ie5    = (is_ie && (is_major == 4) && (agt.indexOf("msie 5.0")!=-1) );
			var is_ie5_5  = (is_ie && (is_major == 4) && (agt.indexOf("msie 5.5") !=-1));
			var is_ie5up  = (is_ie && !is_ie3 && !is_ie4);
			var is_ie5_5up =(is_ie && !is_ie3 && !is_ie4 && !is_ie5);
			var is_ie6    = (is_ie && (is_major == 4) && (agt.indexOf("msie 6.")!=-1) );
			var is_ie6up  = (is_ie && !is_ie3 && !is_ie4 && !is_ie5 && !is_ie5_5);
			
			function checkIE()
			{
				 if (!is_ie6up) 
				{ 
				  window.location = "invalidbrowser.html";
				 } 
			}

		</script>
</head>
<body onload="checkIE();">
    <form id="form1" runat="server">
		<table align="center" style="margin-top:100px;">
		    <tr>
		        <td colspan="2" align="center" valign="middle"><img src="image/logo01.jpg" /><br><B><u>Credit Bureau Automation System</u></B></td>
		    </tr>
			<tr>
				<td><strong><label for="email" style="font-size:small">Username</label></strong></td>
				<td><asp:textbox id="TXT_USERNAME" Width="150" onkeypress="return kutip_satu()" runat="server"></asp:textbox></td>
			</tr>
			<tr>
				<td><strong><label for="password" style="font-size:small">Password</label></strong></td>
				<td><asp:textbox id="TXT_PASSWORD" Width="150" runat="server" TextMode="Password"></asp:textbox></td>
			</tr>
			<tr>
				<td align="center" colspan="2">
				<asp:button id="BTN_SUBMIT" runat="server" Width="88px" Text="L o g i n" 
				style="BORDER-RIGHT: whitesmoke 1px solid; BORDER-TOP: whitesmoke 1px solid; FONT-WEIGHT: 700; FONT-SIZE: 11px; BORDER-LEFT: whitesmoke 1px solid; COLOR: white; BORDER-BOTTOM: whitesmoke 1px solid; FONT-FAMILY: tahoma; BACKGROUND-COLOR: #D82D25;height:26px;"
				onclick="BTN_SUBMIT_Click"></asp:button><br />
				<asp:Label ID="Label1" runat="server" ForeColor="#CC000B"></asp:Label>
				</td>
			</tr>
		</table>
		<div align="center" style="font-size:xx-small;font-family:Arial">
		copyright(c)2011 - PT. Skyworx Indonesia
		</div>
		<!-- <br />
		<div align="center" style="font-size:small;font-family:Arial;color:red">
		<b>WARNING</b> - This is a UAT Environment<br />
		<small>only used for testing purposes</small>
		</div> -->
	</form>
</body>
</html>
