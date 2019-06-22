<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialCalcSwitch.aspx.cs" Inherits="DebtChecking.Facilities.FinancialCalcSwitch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Detail Usaha</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <script type="text/javascript" language="javascript">
        function refreshChildFrame()
        {
            urlLocation = document.form1.FINCAL_URL.value;
            if (urlLocation != "")
                document.FRDETUSAHA.window.location = urlLocation;
            else
            {
	            var fr = document.getElementById('FRDETUSAHA');
	            fr.style.height = 0;
            }
        }
        
	    function resizeFrame() 
	    {
	        var frHeight = document.FRDETUSAHA.document.body.scrollHeight;
	        var fr = document.getElementById('FRDETUSAHA');
	        fr.style.height = frHeight;
	        
	        parent.resizeFrame();
	    }
	</script>
</head>
<body onload="refreshChildFrame()">
    <form id="form1" runat="server">
    <div>
    
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
	        <td valign="top" width="100%">
	            <input type="hidden" ID="FINCAL_URL" runat="server" />
                <iframe id="FRDETUSAHA" name="FRDETUSAHA" frameBorder="no" scrolling="yes" width="100%" height="600"></iframe>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
