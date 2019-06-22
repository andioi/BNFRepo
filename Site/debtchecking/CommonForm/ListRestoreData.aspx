<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListRestoreData.aspx.cs" Inherits="DebtChecking.CommonForm.ListRestoreData" %>

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

    <title>Restore Data</title>
    <script type="text/javascript" language="javascript">    
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
        <td width="300px">
             <table style="height:100%;width:100%" class="Box1" >
             <tr style="height:1%">
             <td style="text-align:left" class="B01">
                Selected count: <a id="selCount" type="text/plain">0</a>
             </td></tr>
             <tr><td style="height:99%">
                <dxe:ASPxListBox ID="selList" ClientInstanceName="selList" runat="server" Width="250px"></dxe:ASPxListBox>
             </td></tr>
             <tr class="F1">
                <td align="center">
                     <input class="Bt1" type="button" value="  Restore  " 
                     onclick="callback(mainPanel,'send')" />
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
