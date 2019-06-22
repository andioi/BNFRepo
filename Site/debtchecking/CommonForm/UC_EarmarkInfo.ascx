<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_EarmarkInfo.ascx.cs" Inherits="DebtChecking.CommonForm.UC_EarmarkInfo" %>
<table id="tbl" runat="server" class="Tbl0">
    <tr>
        <td class="H1" colspan="3" id="t1" runat="server">Informasi Earmarking</td>
    </tr>
    <tr valign="top">
        <td width="50%">
            <table class="Tbl0">
                <tr>
					<td class="B03">Plafon</td>
					<td class="BS">:</td>
					<td class="B11" id="l12" runat="server"></td>
                </tr>
                <tr>
					<td class="B03">Plafon terpakai (Booked)</td>
					<td class="BS">:</td>
					<td class="B11" id="l21" runat="server"></td>
                </tr>
            </table>
        </td>
        <td width="50%">
            <table class="Tbl0">
                <tr>
					<td class="B03">Plafon akan terpakai (Approved)</td>
					<td class="BS">:</td>
					<td class="B11" id="l22" runat="server"></td>
                </tr>
                <tr>
					<td class="B03">Plafon akan terpakai (In Process)</td>
					<td class="BS">:</td>
					<td class="B11" id="l23" runat="server"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trmsg" runat="server" visible="false">
        <td colspan="3">
            <fieldset><legend>Keterangan</legend>
            <table class="Tbl0">
                <tr>
                    <td class="B11" id="msg" runat="server"></td>
                </tr>
            </table>
            </fieldset>
        </td>
    </tr>
</table>
