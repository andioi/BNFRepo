<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DecisionSystemMaker.aspx.cs" Inherits="MikroMnt.Parameter.DecisionSystemMaker" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" TagPrefix="dxuc" %>

<%@ Register src="USC_paraminput.ascx" tagname="USC_paraminput" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- #include file="~/include/uc/UC_Currency.html" -->
    <!-- #include file="~/include/uc/UC_Number.html" -->
    <style type="text/css">
        .hide
        {
        	display:none;
        }
        
    </style>
    <script type="text/javascript" language="javascript">
        function buildcondition()
        {
            var CARD_COND_FW = "";
            var CARD_COND_DESC = "";
            for(i = 0;i< document.form1.popup$panel$CARD_COND.options.length;i++)
            {
                if(i!=0)
                {
                    CARD_COND_FW += "AND/n" + document.form1.popup$panel$CARD_COND.options[i].value;
                    CARD_COND_DESC += "/n" + document.form1.popup$panel$CARD_COND.options[i].text;
                }
                else
                {
                    CARD_COND_FW += document.form1.popup$panel$CARD_COND.options[i].value;
                    CARD_COND_DESC += document.form1.popup$panel$CARD_COND.options[i].text;
                }                
            }
            document.form1.popup$panel$hCARD_COND_FW.value = CARD_COND_FW;
            document.form1.popup$panel$hCARD_COND_DESC.value = CARD_COND_DESC;
             
        }
        function clearlistbox(listbox)
        {
            j = listbox.options.length;
            for(i = 0;i< j;i++)
                document.form1.popup$panel$CARD_COND.options.remove();
        }

        function removecondition()
        {
            if(document.form1.popup$panel$CARD_COND.selectedIndex!=-1)
                document.form1.popup$panel$CARD_COND.options.remove(document.form1.popup$panel$CARD_COND.options[document.form1.popup$panel$CARD_COND.selectedIndex])
            buildcondition();
        }
    
        function addcondition()
        {
            //alert(document.form1.popup$panel$condpanel$TMPLCONDREFF);
            if(document.form1.popup$panel$condpanel$TMPLCONDREFF.style.display==""&&
               document.form1.popup$panel$condpanel$TMPLCONDREFF.value=="")
            {
                alert(document.form1.popup$panel$TMPLCOND.options[document.form1.popup$panel$TMPLCOND.selectedIndex].text + " tidak boleh kosong " );
                return;
            }
            
            if(document.form1.popup$panel$condpanel$TMPLCONDCUSTOM.style.display==""&&
               document.form1.popup$panel$condpanel$TMPLCONDCUSTOM.value=="")
            {
                alert(document.form1.popup$panel$TMPLCOND.options[document.form1.popup$panel$TMPLCOND.selectedIndex].text + " tidak boleh kosong " );
                return;
            }
        
            if(document.form1.popup$panel$TMPLCOND.value!="")
            {
                var newOpt = new Option();
                newOpt.text = document.form1.popup$panel$TMPLCOND.options[document.form1.popup$panel$TMPLCOND.selectedIndex].text;
                newOpt.value = "@["+ document.form1.popup$panel$TMPLCOND.value +"]";
                if(document.form1.popup$panel$condpanel$TMPLCONDREFF.style.display=="")
                {
                    newOpt.text += " = " + document.form1.popup$panel$condpanel$TMPLCONDREFF.options[document.form1.popup$panel$condpanel$TMPLCONDREFF.selectedIndex].text;
                    newOpt.value += " = '" + document.form1.popup$panel$condpanel$TMPLCONDREFF.value + "'";
                }
                if(document.form1.popup$panel$condpanel$TMPLCONDCUSTOM.style.display=="")
                {
                    var temp = document.form1.popup$panel$condpanel$TMPLCONDCUSTOM.value;
                    var i = temp.indexOf(",");
                    var repwith = " AND "+ newOpt.value;
                    while(i > -1)
                    {
                        temp = temp.replace(",", repwith);
                        i = temp.indexOf(",", i + repwith.length + 1);
                    }
                    newOpt.text += ", " + document.form1.popup$panel$condpanel$TMPLCONDCUSTOM.value;
                    newOpt.value += temp;
                }                    
                document.form1.popup$panel$CARD_COND.options[document.form1.popup$panel$CARD_COND.options.length] = newOpt;
            }
            buildcondition();    
        }
    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <!--
    <table width="100%">
        <tr>
            <td class="H0" align="right">
                <a href="<% =BackURL %>"><img src="../image/back.jpg" alt="back" /></a>
                <a href="../Body.aspx"><img src="../image/MainMenu.JPG" alt="mainmenu" /></a>
                <a href="../Logout.aspx" target="_top"><img src="../image/logout.jpg" alt="logout" /></a>
            </td>
        </tr>
    </table>
    -->
    <table class="Box1" width="100%">
    <tr class ="H1"><td>
        <asp:Label ID="title" runat="server" Text="Label"></asp:Label>
    </td></tr>
    <tr><td>
        <dxuc:ASPxPageControl ID="PageCtrl" runat="server" ActiveTabIndex="0" 
            Width="100%">
            <TabPages>
                <dxuc:TabPage Text="CARD">
                    <ContentCollection>
                        <dxuc:ContentControl ID="ContentControl1" runat="server">
                            <dxuc:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" 
                                Width="100%" OnLoad="grid_Load" Font-Size="10px" KeyFieldName="CARD_ID"
                                OnBeforeColumnSortingGrouping="grid_BeforeColumnSortingGrouping" 
                                OnCustomCallback="grid_CustomCallback">
                                <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
                                    ShowGroupPanel="True" />
                                 <Columns>
                                 <dxuc:GridViewDataColumn Caption="Function" VisibleIndex="0" Width="1%" >
                                     <CellStyle Wrap="False">
                                     </CellStyle>
                                    <HeaderTemplate>
                                        <input class="Bt1" type="button" value="New" onclick="popup$panel$clrButton.click();popup.Show()" />
                                    </HeaderTemplate>
                                    <DataItemTemplate>
                                        <input class="Bt1" type="button" value="Edit" title="<%# Container.KeyValue %>" onclick="popup.Show();callback(panel,'r:'+this.title,false)" />
                                        <input class="Bt1" type="button" value="Delete" title="<%# Container.KeyValue %>" onclick="callback(grid,'d:'+this.title,false)" />
                                    </DataItemTemplate>
                                 </dxuc:GridViewDataColumn>
                                 <dxuc:GridViewCommandColumn VisibleIndex="1" Width="1%">
                                     <ClearFilterButton Visible="True">
                                     </ClearFilterButton>
                                     <HeaderTemplate>
                                        <table>
                                            </td>
                                            <tr class="<%=funcCss %>" ><td>
                                                <input class="Bt1" type="button" onclick="grid.ExpandAll();"   value=" Expand All " > 
                                            </tr>
                                            <tr class="<%=funcCss %>"><td>
                                                <input class="Bt1" type="button" onclick="grid.CollapseAll();" value="Collapse All" font-size="10px" />   
                                            </tr></td>
                                        </table>                                        
                                     </HeaderTemplate>
                                 </dxuc:GridViewCommandColumn>
                                     <dxuc:GridViewDataTextColumn Caption="Item" FieldName="ITEM_DESC" 
                                         VisibleIndex="2">
                                     </dxuc:GridViewDataTextColumn>
                                     <dxuc:GridViewDataTextColumn Caption="Condition" FieldName="CARD_COND_DESC" 
                                         VisibleIndex="3">
                                     </dxuc:GridViewDataTextColumn>
                                     <dxuc:GridViewDataTextColumn Caption="Result" FieldName="RES_DESC" 
                                         VisibleIndex="4">
                                     </dxuc:GridViewDataTextColumn>
                                 </Columns>
                                <settingsbehavior autofilterrowinputdelay="-1" />
                                <SettingsPager PageSize="12">
                                </SettingsPager>
                             </dxuc:ASPxGridView>
                               
                        </dxuc:ContentControl>
                    </ContentCollection>
                </dxuc:TabPage>
                <dxuc:TabPage Text="Pending Approval" Visible="False">
                    <ContentCollection>
                        <dxuc:ContentControl ID="ContentControl2" runat="server">
                            <dxuc:ASPxGridView ID="gridpending" ClientInstanceName="gridpending" 
                                runat="server" AutoGenerateColumns="False" 
                                Width="100%" 
                                OnBeforeColumnSortingGrouping="gridpending_BeforeColumnSortingGrouping" 
                                OnCustomCallback="gridpending_CustomCallback">
                                <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
                                    ShowGroupPanel="True" />
                                 <Columns>
                                 <dxuc:GridViewDataColumn Caption="Function" VisibleIndex="0" Width="1%" >
                                     <CellStyle Wrap="False">
                                     </CellStyle>
                                    <DataItemTemplate>
                                        <input class="Bt1" type="button" value="Edit" title="<%# Container.KeyValue %>" onclick="popup.Show();callback(panel,'p:'+this.title,false)" />
                                        <input class="Bt1" type="button" value="Delete" title="<%# Container.KeyValue %>" onclick="callback(gridpending,'d:'+this.title,false)" />
                                    </DataItemTemplate>
                                 </dxuc:GridViewDataColumn>
                                 <dxuc:GridViewCommandColumn VisibleIndex="1" Width="1%">
                                     <ClearFilterButton Visible="True">
                                     </ClearFilterButton>
                                     <HeaderTemplate>
                                        <table>
                                            <tr class="<%=funcpendCss %>" ><td>
                                                <input class="Bt1" type="button" onclick="grid.ExpandAll();"   value=" Expand All " > 
                                            </tr></td>
                                            <tr class="<%=funcpendCss %>"><td>
                                                <input class="Bt1" type="button" onclick="grid.CollapseAll();" value="Collapse All" font-size="10px" />   
                                            </tr></td>
                                        </table>                                        
                                     </HeaderTemplate>
                                 </dxuc:GridViewCommandColumn>
                                 </Columns>
                                <settingsbehavior autofilterrowinputdelay="-1" />
                                <SettingsPager PageSize="12">
                                </SettingsPager>
                             </dxuc:ASPxGridView>
                        </dxuc:ContentControl>
                    </ContentCollection>
                </dxuc:TabPage>
            </TabPages>
        </dxuc:ASPxPageControl>
    
        <dxuc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" HeaderText="" width="800px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
        <dxuc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" Height="100%">
        <dxuc:ASPxCallbackPanel ID="panel" runat="server" ClientInstanceName="panel" 
                OnCallback="panel_Callback" >
        <ClientSideEvents EndCallback="function(s, e){ if(panel.cp_action=='s'){popup.Hide();processing=false;callback(grid);}}" />
        <PanelCollection>
        <dxuc:PanelContent ID="PanelContent1" runat="server">
        <table class="Box1" width="100%" >
        <tr>
            <td>
                <table class="Box1" width="100%" >
                <tr>
                    <td class="B01">Item</td>
                    <td class="BS">:</td>
                    <td class="B11">
                        <input id="CARD_ID" type="hidden" runat="server"/> 
                        <asp:DropDownList ID="ITEM_ID" runat="server" CssClass="mandatory">
                        </asp:DropDownList>                          
                    </td>
                </tr>
                <tr>
                    <td class="B01">Condition</td>
                    <td class="BS">:</td>
                    <td class="B11">
                        <table>
                        <tr>
                            <td>
                                <input type="hidden" runat="server" ID="hCARD_COND_DESC" value="" />
                                <input type="hidden" runat="server" ID="hCARD_COND_FW" value="" />
                                
                                <asp:DropDownList ID="TMPLCOND" runat="server" onchange="callback(condpanel,'',false);" ></asp:DropDownList>
                                <dxuc:ASPxCallbackPanel ID="condpanel" ClientInstanceName="condpanel" 
                                    runat="server" Width="100%" OnLoad="condpanel_Load">
                                <PanelCollection>
                                <dxuc:PanelContent ID="PanelContent4" runat="server">
                                <asp:DropDownList ID="TMPLCONDREFF" runat="server" CssClass="mandatory"></asp:DropDownList>
                                <asp:TextBox ID="TMPLCONDCUSTOM" runat="server" CssClass="mandatory"></asp:TextBox>
                                </dxuc:PanelContent>
                                </PanelCollection>
                                </dxuc:ASPxCallbackPanel>       
                            </td>
                            <td valign="top">
                            
                                <input type="button" class="Bt1" value="Add Condition" onclick="if(!processing)addcondition();" />                            
                                &nbsp;
                                <input type="button" class="Bt1" value="Remove Condition" onclick="if(!processing)removecondition();" />                            
                            </td>                            
                            </tr>
                        </tr></table>
                        <asp:ListBox ID="CARD_COND" Width="100%" runat="server"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td class="B01">Result</td>
                    <td class="BS">:</td>
                    <td class="B11">
                        <asp:DropDownList ID="RES01" runat="server" CssClass="mandatory">
                        </asp:DropDownList>                          
                   </td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td runat="server" id="td_filter" align="center" >
                <input id="Button1" runat="server" class="Bt1" type="button" value=" Save " onclick="callback(panel,'s:',true,new Array('popup$panel$condpanel'))" />
                <input id="clrButton" runat="server" class="Bt1" type="button" value=" Clear " style="display:none"/>
                <input id="Button2" runat="server" class="Bt1" type="button" value=" Cancel " onclick="popup.Hide();" />
                
            </td>
        </tr>
        </table>
        </dxuc:PanelContent>
        </PanelCollection>
        </dxuc:ASPxCallbackPanel>        
        </dxuc:PopupControlContentControl>
        </ContentCollection>
        </dxuc:ASPxPopupControl>        
        
        </td></tr>
        </table>
        
        </div>
    </form>
</body>
</html>
