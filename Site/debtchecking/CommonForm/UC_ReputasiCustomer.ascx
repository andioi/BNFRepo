<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_ReputasiCustomer.ascx.cs" Inherits="DebtChecking.CommonForm.UC_ReputasiCustomer" %>

<table id="Content" class="Box2" width="910px" align="center">
    
    <tr>
        <td colspan="3">

		<table width="100%" cellpadding="0" cellspacing="0">
            <tr>
				<td class="td02">Nama Pengurus</td>
				<td class="BS">:</td>
				<td class="td11"><%=DS(0, "supp_name") %></td>
			</tr>
            <tr>
				<td class="td02" valign="top">Alamat</td>
				<td class="BS" valign="top">:</td>
				<td class="td11"><%=DS(0, "alamat") %>
				</td>
			</tr>
            <tr>
				<td class="td02" valign="top">Jabatan</td>
				<td class="BS" valign="top">:</td>
				<td class="td11"><%=DS(0, "jabatan") %>
				</td>
			</tr>
        </table>
   
		</td>
    </tr>
    <tr>
        <td colspan="3" align="center"><b>REPUTASI CUSTOMER</b><br /></td>
    </tr>
    <tr>
        <td colspan="3" align="left">
            - Customer memiliki reputasi yang <%=DS(0, "repdesc_1") %> selama periode bulan ke 1 s/d bulan ke 6  di <%=DS(0, "jmlkreditur_1") %> kreditur <br />
            - Customer memiliki reputasi yang <%=DS(0, "repdesc_2") %> selama periode bulan ke 7 s/d bulan ke 12  di <%=DS(0, "jmlkreditur_2") %> kreditur<br />
            - Customer memiliki reputasi yang <%=DS(0, "repdesc_3") %> selama periode bulan ke 13 s/d bulan ke 24  di <%=DS(0, "jmlkreditur_3") %> kreditur<br /><br />
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center" style="border-bottom:dashed black 1px;">
        
		<table width="40%" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="3" align="left">Tabel Reputasi Per Periode</td>
            </tr>
            <tr>
				<td style="background-color:yellow" class="boxboldcenter" align="center">1-6 bulan</td>
				<td style="background-color:yellow" class="boxboldcenter" align="center">7-12 bulan</td>
				<td style="background-color:yellow" class="boxboldcenter" align="center">13-24 bulan</td>
			</tr>
            <tr>
				<td class="boxboldcenter" align="center"><%=DS(0, "repburuk_1") %>/<%=DS(0, "jmlkreditur_1") %></td>
				<td class="boxboldcenter" align="center"><%=DS(0, "repburuk_2") %>/<%=DS(0, "jmlkreditur_2") %></td>
				<td class="boxboldcenter" align="center"><%=DS(0, "repburuk_3") %>/<%=DS(0, "jmlkreditur_3") %></td>
			</tr>
        </table>
        <br />
		</td>
    </tr>
</table>