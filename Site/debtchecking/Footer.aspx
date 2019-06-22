<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Footer.aspx.cs" Inherits="DebtChecking.Footer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<title>Footer</title>
		<script language="javascript" type="text/javascript">
		function postback()
		{
			form1.post_cnt.value = eval(form1.post_cnt.value + '+1');
			form1.submit();
		}
		function set_post()
		{
			try {
                if (form1.post_cnt.value == form1.timeout_warning.value)	window.parent.execScript('warn_timeout()');
                if (form1.post_cnt.value == form1.timeout_logoff.value)	window.parent.execScript('logout_now()');
				else	setTimeout('postback()',60000);
			}	catch (e) { }
		}
		function reset_post()
		{
			form1.post_cnt.value = 0;
			//clearTimeout(timer);
			//set_post();
		}
		</script>
</head>
<body topmargin="0" leftmargin="0" rightmargin="0" onload="set_post()">
    <form id="form1" runat="server">
		<table border="0" width="100%" bgcolor="#FFCB0B">
				<tr valign="top">
					<td width="35%" valign="top">
					    <font face="tahoma" size="1" style="FONT-SIZE:9px" color="#00573D">
					        &nbsp;<asp:Label id="Label3" runat="server" Height="15">UserName : </asp:Label>
					        &nbsp;<asp:Label id="Label2" runat="server" Height="15"></asp:Label>
					        &nbsp;<asp:Label id="Label5" runat="server" Height="15"></asp:Label>
					    </font>
					</td>
					<td width="30%" align="center">
					    <font color="#00573D" face="tahoma" size="1" style="FONT-SIZE:9px">Copyright @ PT. Skyworx Indonesia</font>
					</td>
					<td width="35%" align="right" valign="top">
					    <font color="#00573D" face="tahoma" size="1" style="FONT-SIZE:9px">
					        <input type="hidden" id="post_cnt" runat="server" name="post_cnt" />
                            <input type="hidden" id="timeout_warning" runat="server" name="timeout_warning" />
                            <input type="hidden" id="timeout_logoff" runat="server" name="timeout_logoff" />
						    <asp:Label id="Label4" runat="server">Login Since : </asp:Label>
						    &nbsp;<asp:Label id="Label1" runat="server"></asp:Label>
						</font>
					</td>
				</tr>
			</table>
    </form>
</body>
</html>
