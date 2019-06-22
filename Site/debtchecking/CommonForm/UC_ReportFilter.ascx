<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ReportFilter.ascx.cs" Inherits="DebtChecking.Report.UC_ReportFilter" %>
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
            <input runat="server" class="btn btn-xs btn-danger" type="submit" value="Search" onclick="callback(grid, '', false, null);" />
            <input runat="server" id="clrButton" class="btn btn-xs btn-danger" type="button" value=" Clear " /></td>
    </tr>
    </table>
</td></tr>
<tr>
<td style="height:2px">
</td>
</tr>
</table>