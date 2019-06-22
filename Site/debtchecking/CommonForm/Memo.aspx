<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Memo.aspx.cs" Inherits="DebtChecking.CommonForm.Memo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Memo Page</title>
    <!-- #include file="~/include/cek_mandatoryOnly.html" -->
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript">
        function hideTR()
        {
            document.form1.TXT_MEMO.value = "";
            TR_UPL.style.display = "none";
            TR_NEWBTN.style.display = "";
        }
        function showTR()
        {
            document.form1.TXT_MEMO.value = "";
            TR_UPL.style.display = "";
            TR_NEWBTN.style.display = "none";
        }
        var processing = false;
	</script>
    <script for=form1 event=onsubmit language=javascript>
	    if (processing) return false;
	    processing = true;
	    return true;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <table width="100%" class="Box1">
        <tr class="H1">
            <td>Memo</td>
        </tr>
        <tr id="TR_NEWBTN" runat="server">
            <td align="center">
                <input id="btn_new" type="button" runat="server" value="New" class="Bt1" onclick="showTR()" />
            </td>
        </tr>
        <tr id="TR_UPL" runat="server">
            <td align="center">
                <table width="90%">
			        <tr>
				        <td>
					        <asp:TextBox id="TXT_MEMO" runat="server" Width="100%" TextMode="MultiLine" Rows="15" CssClass="mandatory"></asp:TextBox>
					    </td>
			        </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_save" runat="server" CssClass="Bt1" Text="Save" onclick="btn_save_Click" />
                            <input id="btn_cancel" type="button" runat="server" value="Cancel" class="Bt1" onclick="hideTR()" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="griddata" runat="server" AutoGenerateColumns="false" width="100%"
                    CssClass="Dg1" HorizontalAlign="Center" AllowPaging="True" PageSize="10" 
                    onpageindexchanged="griddata_PageIndexChanged" CellPadding="5"
                    onpageindexchanging="griddata_PageIndexChanging" 
                    OnRowDataBound="griddata_RowDataBound" >
                    <PagerStyle HorizontalAlign="Left" />
                    <HeaderStyle CssClass="H3" />
                    <AlternatingRowStyle CssClass="Alt1" />
                    <RowStyle HorizontalAlign="Center" VerticalAlign="Top" CssClass="B12" />
                    <Columns>
                        <asp:BoundField DataField="MM_SEQ" HeaderText="No" ItemStyle-Width="5%" />
                        <asp:BoundField DataField="su_fullname" HeaderText="Oleh" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="sg_grpname" HeaderText="Unit" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="MM_DATE" HeaderText="Tanggal" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Isi" ItemStyle-HorizontalAlign="Left" >
                            <ItemTemplate>
                                <asp:Label ID="lblmsg" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
