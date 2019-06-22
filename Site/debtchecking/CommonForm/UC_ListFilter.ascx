<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ListFilter.ascx.cs" Inherits="DebtChecking.List.UC_ListFilter" %>
<table id="mainTbl" runat="server" width="100%">
<tr>
<td align="center">
    <table class="table table-bordered" style="width:40%">
    <tr class="warning">
        <td>
            <div style="font-weight:bold;text-align:center">SEARCH CRITERIA</div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Table ID="tblSearch" runat="server" Width="100%" >
            </asp:Table>
        </td>
    </tr>
    <tr>
        <td runat="server" id="td_filter" align="center" >
            <input id="Button1" runat="server" class="btn btn-xs btn-warning" style="color:#00573D" type="submit" value="Search" onclick="callback(grid, '', false, null);" />
            <input runat="server" id="clrButton" class="btn btn-xs btn-warning" type="button" value=" Clear " style="color:#00573D" />
            <input id="btnnew" runat="server" class="btn btn-xs btn-warning" type="button" value="Input New" onclick="" style="display:none;color:#00573D" />
        </td>
    </tr>
    </table>
</td></tr>
<tr>
<td style="height:2px">
</td>
</tr>
</table>