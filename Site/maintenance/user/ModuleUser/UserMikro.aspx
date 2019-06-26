<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMikro.aspx.cs" Inherits="MikroMnt.user.ModuleUser.UserMikro" %>

<%@ Register TagPrefix="cc1" Namespace="DMSControls" Assembly="DMSControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <title>User Maintenance</title>
		<link href="../../include/style.css" type="text/css" rel="stylesheet" />
		<!-- #include file="../../include/cek_mandatoryOnly.html" -->
        <script src="../../include/UC_Currency.js" language="javascript" type="text/javascript"></script>
        <script src="../../include/cek_entries.js" language="javascript" type="text/javascript"></script>
        <script src="../../include/calc.js" language="javascript" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
			<input type="hidden" id="sta" name="sta"></input>
			<input type="hidden" id="uid" name="uid"></input>
			<input type="hidden" id="br" name="br"></input>
			<table class="Box1" width="100%" style="display:none">
				<tr>
					<td><strong>Bisnis Segmen Authority&nbsp;:</strong></td>
				</tr>
				<tr>
					<td>
                        <asp:CheckBoxList id="CBL_SEGMEN" runat="server" RepeatDirection="Vertical"
                            RepeatLayout="Flow"></asp:CheckBoxList>
                    </td>
				</tr>
			</table>
    
    </div>
    </form>
</body>
</html>
