<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListNonPV.aspx.cs" Inherits="DebtChecking.CommonForm.ListNonPV" %>

<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxe" %>

<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxp" %>

<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>

<%@ Register assembly="DMSControls" namespace="DMSControls" tagprefix="cc1" %>
<%@ Register src="UC_ListFilter.ascx" tagname="UC_ListFilter" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->

    <title>Untitled Page</title>
    <script type="text/javascript" language="javascript">  
        var popupWindow = null; 
        function PopupPage(href,width,height,scroll)
        {
	        if (width == null)
	            width = screen.availWidth * 0.8;
	        if (height == null)
	            height = screen.availHeight * 0.8;
		    if (popupWindow != null){popupWindow.close();}
		    var X = (screen.availWidth-width)/2;
		    var Y = (screen.availHeight-height)/2;
		    if (scroll==null) scroll = "no";
		    popupWindow = window.open(href,"","height="+height+"px,width="+width+"px,left="+X+",top="+Y+
				    ",status=no,toolbar=no,scrollbars="+scroll+",resizable=yes,titlebar=no,menubar=no,location=no,dependent=yes");
        }    
     
        function OnGridSelecteionChange()
        { 
            var counter = document.getElementById('selCount');  
            if(counter != null) setInnerText(counter, grid.GetSelectedRowCount().toString());  
            grid.GetSelectedFieldValues('__KeyFieldDesc;__KeyField', OnGridSelectionComplete);
        }          

        function OnGridSelectionComplete(values) {
            selList.BeginUpdate();
            selList.ClearItems();
            for(var i = 0; i < values.length; i ++) {
                selList.AddItem(values[i][0],values[i][1]);
            }
            selList.EndUpdate();
        }
        function setInnerText(element, text) { 
            if(typeof element.textContent != 'undefined') { 
                element.textContent = text; 
            } 
            else if (typeof element.innerText != 'undefined') { 
                element.innerText = text; 
            } 
            else if (typeof element.removeChild != 'undefined') { 
                while (element.hasChildNodes()) { 
                    element.removeChild(element.lastChild); 
                } 
                element.appendChild(document.createTextNode(text)); 
            } 
        }        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <dxcp:ASPxCallbackPanel ID="mainPanel" ClientInstanceName="mainPanel" 
            runat="server" Width="100%" oncallback="mainPanel_Callback">     
        <ClientSideEvents EndCallback="function(s, e) {
	OnGridSelecteionChange();
	if(s.hasOwnProperty('cp_export') && s.cp_export!='')
	{
	    window.open(s.cp_export);
	    /*mydoc = window.open();
        mydoc.document.write(s.cp_export);
        mydoc.document.execCommand('saveAs',true,s.cp_filename+'.txt');
        mydoc.close();*/
		s.cp_export = '';
		return false;
	}
}" />
    <PanelCollection>
    <dxp:PanelContent ID="PanelContent1" runat="server">
        <uc1:UC_ListFilter ID="UC_ListFilter1" runat="server" />
        <table class="Box1" style="width:98%"> 
        <tr class="H1" ><td colspan="2">
           <b>
            <asp:Label ID="TitleHeader" runat="server"></asp:Label>
           </b>
        </td></tr>             
        <tr>
        <td> 
            <table width="100%">
            <tr>
                <td align="right">
                <table width="100%">
                    <tr>
                       <td>
                       <input type="button" onclick=
                       "if(this.value=='Expand All')
                        {
                            grid.ExpandAll();
                            this.value = 'Collapse All';
                        }
                        else
                        {
                            grid.CollapseAll();
                            this.value = 'Expand All';
                        }"    value="Expand All"  />
                       <input type="button" onclick=
                       "if(this.value=='Select All')
                        {
                            grid.SelectAllRowsOnPage(true);
                            this.value = 'Unselect All';
                        }
                        else
                        {
                            grid.SelectAllRowsOnPage(false);
                            this.value = 'Select All';
                        }"    value="Select All"  />
                       </td>
                   </tr>

                </table>
                </td>           
            </tr>            
            <tr>
                <td>
                <dxwgv:ASPxGridView ID="grid" runat="server" ClientInstanceName ="grid" Width="100%"
                AutoGenerateColumns="False" OnLoad="grid_Load" >
                    <ClientSideEvents SelectionChanged="function(s, e) {
        e.processOnServer = false;
	    OnGridSelecteionChange();
    }" />
                <settingsbehavior autofilterrowinputdelay="-1" />
                </dxwgv:ASPxGridView>
                </td>
            </tr>
            </table>
        </td>
        <td width="300px" valign="top"><br /><br /><br />
             <table style="height:100%;width:100%" class="Box1" >
            <tr style="display:none">
                <td align="center">
                <asp:Label ID="link" runat="server" ForeColor="blue" Font-Underline="true"><strong>Upload Verification</strong></asp:Label>
                <%--<a href="javascript:PopupPage('UploadVerification.aspx','500','400')"><strong>Upload Verification</strong></a>--%>
                </td>
             </tr>                  
             <tr style="height:1%">
             <td style="text-align:left" class="B01">
                Selected count: <a id="selCount" type="text/plain">0</a>
             </td></tr>
             <tr style="display:none"><td style="height:99%">
                <dxe:ASPxListBox ID="selList" ClientInstanceName="selList" runat="server" Width="250px"></dxe:ASPxListBox>
             </td></tr>
             <tr style="height:1%"><td>
                 <table>
                 <tr style="display:none">
                     <td class="B01" nowrap="nowrap">
                         Send to</td>
                     <td class="BS">
                         :</td>
                     <td class="B11">
                         <asp:RadioButtonList ID="rblsendchange" runat="server" 
                             RepeatDirection="Horizontal" Visible="false">
                             <asp:ListItem Value="1">Internal</asp:ListItem>
                             <asp:ListItem Selected="True" Value="2">External</asp:ListItem>
                         </asp:RadioButtonList>
                         <dxcp:ASPxCallbackPanel ID="sendtoPanel" ClientInstanceName="sendtoPanel" 
                                    runat="server" Width="100%" OnCallback="sendtoPanel_Callback">
                                    <ClientSideEvents EndCallback="function(s, e) 
                                            {
                                                document.form1.mainPanel$hSendTo.value = document.form1.mainPanel$sendtoPanel$SendTo.value;
                                            }
                                            " />     
                            <PanelCollection>
                            <dxp:PanelContent ID="PanelContent2" runat="server">
                             <asp:DropDownList ID="SendTo" runat="server" CssClass="mandatory" 
                                 OnLoad="SendTo_Load" onchange="mainPanel$hSendTo.value=this.value">
                             </asp:DropDownList>
                             </dxp:PanelContent>
                            </PanelCollection>
                            </dxcp:ASPxCallbackPanel>    
                            <input type="hidden" runat="server" id="hSendTo" >
                     </td>
                 </tr>
                     <tr style="display:none">
                         <td class="B01">
                             &nbsp;</td>
                         <td class="BS">
                             :
                         </td>
                         <td class="B11">
                            
                                
                                

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             </input>
                        </td>
                     </tr>
                 </table>                 
             </td></tr>
             <tr class="F1">
                <td align="center">
                     <input class="Bt1" type="button" value="  Flag  " style="display:none"
                     onclick="callback(mainPanel,'send')" />
                         &nbsp;
                     <input class="Bt1" type="button" value="  Auto  " 
                     onclick="sta=confirm('Confirm Auto Send?');
                              if(sta)callback(mainPanel,'auto',false)" style="display:none"  />
                     <input class="Bt1" type="button" value="  Export  " 
                     onclick="sta=confirm('Confirm Assignment?');
                              if(sta)callback(mainPanel,'confirm',false)"  />
                 </td>
              </tr>
              <tr class="F1" style="display:none">
                <td align="center">
                                         
                </td>
              </tr>
             </table>
        </td>
        </tr>
        </table>
    </dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>    
    </div>
    </form>
</body>
</html>
