<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_Date.ascx.cs" Inherits="MikroMnt.CommonForm.UC_Date" %>
<script src="../include/date_picker.js" language="javascript"></script>
<LINK href="../include/date_picker.css" type="text/css" rel="stylesheet">
<script src="../../include/date_picker.js" language="javascript"></script>
<LINK href="../../include/date_picker.css" type="text/css" rel="stylesheet">
<asp:TextBox id="TXT_DD" runat="server" Width="32px" MaxLength="2" onpaste="return false;"></asp:TextBox><asp:DropDownList id="DDL_MM" runat="server" onpaste="return false;"></asp:DropDownList><asp:TextBox id="TXT_YY" runat="server" Width="42px" MaxLength="4" onpaste="return false;"></asp:TextBox>
<img src="../image/ic_datetime.gif" runat="server" id="IB_DATE" width="21" height="21"
	style="CURSOR:hand">
