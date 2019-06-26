<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefSearch.aspx.cs" Inherits="MikroMnt.CommonForm.RefSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Refference Search Page</title>
	<link href="../include/style.css" type="text/css" rel="stylesheet" />
	<script language="javascript" type="text/javascript">
	    var picked = false;
		function pilih(ctrlID,ctrlDesc,mypreendcallback)
        {
            
		    if (mypreendcallback != null)
		        eval(mypreendcallback);
            
            picked = true;
            
			if (document.getElementById("LST_RESULT").value != "")
			{
                eval('parent.window.opener.document.getElementById("' + ctrlID + '").value = document.getElementById("LST_RESULT").value');
                eval('parent.window.opener.document.getElementById("' + ctrlDesc + '").value = document.getElementById("LST_RESULT").options[document.getElementById("LST_RESULT").selectedIndex].text.replace(document.getElementById("LST_RESULT").value + " - ", "")');
			}
			else
			{
                eval('parent.window.opener.document.getElementById("' + ctrlID + '").value = ""');
                eval('parent.window.opener.document.getElementById("' + ctrlDesc + '").value = ""');
            }
            
			window.close();
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<table class="Box1" width="450">
			<tr>
				<td valign="top">
					<table class="Tbl0">
						<tr>
							<td class="B01" id="tdcode" width="70" runat="server">Kode</td>
							<td class="BS">:</td>
							<td class="B11"><asp:textbox id="TXT_CODE" runat="server"></asp:textbox></td>
						</tr>
						<tr>
							<td class="B01" id="tddesc" runat="server">Deskripsi</td>
							<td class="BS">:</td>
							<td class="B11"><asp:textbox id="TXT_DESC" runat="server" Width="100%"></asp:textbox></td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td>
							    <asp:button id="BTN_SEARCH" runat="server" Text="Cari" Width="80px" onclick="BTN_SEARCH_Click"></asp:button>
							</td>
						</tr>
						<tr>
							<td class="B01">Hasil Pencarian</td>
							<td class="BS"></td>
							<td class="B11"><asp:listbox id="LST_RESULT" runat="server" Width="100%" EnableViewState="False" Rows="10"></asp:listbox></td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td></td>
						</tr>
						<tr class="F1">
							<td colspan="3">
								<input class="Bt1" id="ok" type="button" value="OK" name="ok" runat="server" style="WIDTH: 80px" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	
    </div>
    </form>
</body>
</html>
