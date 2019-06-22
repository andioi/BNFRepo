<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IMG_OpenImg.aspx.cs" Inherits="Mikro.CommonForm.IMG_OpenImg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Open Image</title>
	<link href="../include/style.css" type="text/css" rel="stylesheet" /> 
	<meta http-equiv="imagetoolbar" content="no" />
	<script language="javascript" type="text/javascript">
	    var imgH = 0, imgW = 0, img;
		function fitToScreen() 
		{
			if (img == null) 
			    img = document.getElementById('IMDOC');
			if (imgH == 0)
				imgH = img.height;	//save actual width first
			if (imgW == 0)
				imgW = img.width;
				
			var scrW = window.screen.width;
			
			if (imgW > 0.9 * scrW)
			{
				img.width = 0.9 * scrW;
				img.height = imgH * (0.9 * scrW / imgW);
			}
			
			initmydiv();
		    //parent.resizeFrame();
		}
	    
		function actualSize() 
		{
			img.height = imgH;
			img.width = imgW;
			
			initmydiv();
		    //parent.resizeFrame();
		}
		
		function zoomIn() 
		{
			img.height = img.height * 1.1;
			img.width = img.width * 1.1;
			
			initmydiv();
		    //parent.resizeFrame();
		}
		
		function zoomOut() 
		{
			img.height = img.height * 0.9;
			img.width = img.width * 0.9;
			
			initmydiv();
		    //parent.resizeFrame();
		}
		
	    function closeParentFrame()
	    {
		    //parent.closeFrame();
		    window.close();
	    }
	    
		var rot = 0, mir = 0;
		function initmydiv()
		{
			mydiv.style.width = img.width;
			mydiv.style.height = img.height;
		}
		function rotateLeft()
		{
			rot = rot - 1;
			if (rot < 0) rot = 3;
			mydiv.style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=" + rot + ", mirror=" + mir + ")";
		}
		function rotateRight()
		{
			rot = rot + 1;
			if (rot > 3) rot = 0;
			mydiv.style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=" + rot + ", mirror=" + mir + ")";
		}
		function flipH()
		{
			if (mir == 0) mir = 1;
			else mir = 0;
			mydiv.style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=" + rot + ", mirror=" + mir + ")";
		}
		function flipV()
		{
			if (mir == 0) mir = 1;
			else mir = 0;
			rot = rot + 2;
			if (rot > 3) rot = rot - 4;
			mydiv.style.filter="progid:DXImageTransform.Microsoft.BasicImage(rotation=" + rot + ", mirror=" + mir + ")";
		}
		</script>
</head>
<body onload="fitToScreen()">
    <form id="form1" runat="server">
    <div>
    
			<table cellspacing="2" cellpadding="2" width="100%">
				<tr id="TRBTN" runat="server">
					<td>
						<input type="button" value="Actual Size" onclick="actualSize()" />
						&nbsp;&nbsp;
						<input type="button" value="Fit To Screen" onclick="fitToScreen()" />
						&nbsp;&nbsp;
						<input type="button" value="zoom in" onclick="zoomIn()" />
						&nbsp;&nbsp;
						<input type="button" value="zoom out" onclick="zoomOut()" />
						&nbsp;&nbsp;
						<input type="button" value="rotate left" onclick="rotateLeft()" />
						&nbsp;&nbsp;
						<input type="button" value="rotate right" onclick="rotateRight()" />
						&nbsp;&nbsp;
						<input type="button" value="Close" onclick="closeParentFrame()" />
					</td>
				</tr>
				<tr>
					<td>
                		<div id="mydiv">
				    		<img id="IMDOC" name="IMDOC" src="" runat="server" alt="" />
				    	</div>
					</td>
				</tr>
			</table>
			
    </div>
    </form>
</body>
</html>
