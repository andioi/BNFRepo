<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParamMaker.aspx.cs" Inherits="MikroMnt.Parameter.ParamMaker" %>

<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dxuc" %>

<%@ Register src="USC_paraminput.ascx" tagname="USC_paraminput" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- #include file="~/include/uc/UC_Currency.html" -->
    <!-- #include file="~/include/uc/UC_Decimal.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .hide
        {
        	display:none;
        }
        .pendingDelete
        {
        	display:none;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position:fixed;top:0;right:0;margin-right:20px;margin-top:5px;">
        <!-- Row start -->
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row rightinfo">
                    <a href="<%=BackURL %>" class="btn btn-warning"> <span class="glyphicon glyphicon-chevron-left"></span> Back </a> 
                    <a href="../main.html" class="btn btn-warning" ><span class="glyphicon glyphicon-home"></span> Mainmenu </a> 
                    <a href="../Logout.aspx" class="btn btn-warning"><span class="glyphicon glyphicon-log-out"></span> Logout </a>

                </div>
            </div>
        </div>
        <!-- Row end -->
    </div>
    <div style="margin-top:60px;">
    <table class="Box1" width="100%">
    <tr class ="H1"><td>
        <asp:Label ID="title" runat="server" Text="Label"></asp:Label>
    </td></tr>
    <tr><td>
        <dxuc:ASPxPageControl ID="PageCtrl" runat="server" ActiveTabIndex="0" 
            Width="100%">
            <TabPages>
                <dxuc:TabPage Text="Existing Parameter">
                    <ContentCollection>
                        <dxuc:ContentControl ID="ContentControl1" runat="server">
                            <dxuc:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" 
                                AutoGenerateColumns="False" 
                                Width="100%" 
                                Font-Size="10px" 
                                OnLoad="grid_Load" 
                                OnBeforeColumnSortingGrouping="grid_BeforeColumnSortingGrouping" 
                                OnCustomCallback="grid_CustomCallback">
                                <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
                                    ShowGroupPanel="True" />
                                 <Columns>
                                 <dxuc:GridViewCommandColumn VisibleIndex="0" Width="1px"></dxuc:GridViewCommandColumn>
                                 <dxuc:GridViewDataColumn Caption="Function" VisibleIndex="1" Width="1%" >
                                     <CellStyle Wrap="False">
                                     </CellStyle>
                                    <HeaderTemplate>
                                        <table>
                                            <tr>
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" value="New" onclick="popup$panel$USC_paraminput1$clrButton.click();popup.Show()" />
                                              </td>
                                            </tr>
                                            <tr class="<%=funcCss %>" >
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" onclick="grid.ExpandAll();"   value=" Expand All " /> 
                                              </td>
                                            </tr>
                                            <tr class="<%=funcCss %>">
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" onclick="grid.CollapseAll();" value="Collapse All"  />   
                                              </td>
                                            </tr>
                                        </table>                                        
                                        
                                    </HeaderTemplate>
                                    <DataItemTemplate>
                                        <table>
                                            <tr>
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" value="Edit" onclick="<%# "popup.Show();callback(panel,'r:" + Container.KeyValue.ToString().Replace("'","\\'") + "', false,  null)"%>"  />&nbsp;
                                              </td>
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" value="Delete" onclick="<%# "callback(gridpending,'d:" + Container.KeyValue.ToString().Replace("'","\\'")  + "', false,  null)"%>" />
                                              </td>
                                            </tr>
                                        </table>       
                                    </DataItemTemplate>
                                 </dxuc:GridViewDataColumn>
                                 </Columns>
                                <settingsbehavior autofilterrowinputdelay="-1" />
                                <SettingsPager PageSize="12">
                                </SettingsPager>
                             </dxuc:ASPxGridView>
                               
                        </dxuc:ContentControl>
                    </ContentCollection>
                </dxuc:TabPage>
                <dxuc:TabPage Text="Pending Approval">
                    <ContentCollection>
                        <dxuc:ContentControl ID="ContentControl2" runat="server">
                            <dxuc:ASPxGridView ID="gridpending" ClientInstanceName="gridpending" 
                                runat="server" AutoGenerateColumns="False" 
                                Width="100%"
                                Font-Size="10px"
                                OnLoad="gridpending_Load"
                                OnBeforeColumnSortingGrouping="gridpending_BeforeColumnSortingGrouping" 
                                OnCustomCallback="gridpending_CustomCallback">
                                <Settings ShowFilterRow="True" ShowGroupedColumns="True" 
                                    ShowGroupPanel="True" />
                                 <Columns>
                                 <dxuc:GridViewCommandColumn VisibleIndex="0" Width="1px">
                                 </dxuc:GridViewCommandColumn>
                                 <dxuc:GridViewDataColumn Caption="Function" VisibleIndex="0" Width="1%" >
                                     <CellStyle Wrap="False">
                                     </CellStyle>
                                     <HeaderTemplate>
                                        <table>
                                            <tr class="<%=funcpendCss %>" >
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" onclick="gridpending.ExpandAll();"   value=" Expand All " /> 
                                              </td>
                                            </tr>
                                            <tr class="<%=funcpendCss %>">
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" onclick="gridpending.CollapseAll();" value="Collapse All" />   
                                              </td>
                                            </tr>
                                        </table>                                        
                                     </HeaderTemplate>
                                    <DataItemTemplate>
                                        <table>
                                            <tr>
                                              <td class="<%# "pending" + Eval("__STATUS").ToString() %>" >
                                                <input class="btn btn-xs btn-success" type="button" value="Edit" onclick="<%# "popup.Show();callback(panel,'rp:" + Container.KeyValue.ToString().Replace("'","\\'") + "', false, null)"%>" />&nbsp;&nbsp;
                                              </td>
                                              <td>
                                                <input class="btn btn-xs btn-success" type="button" value="Delete" onclick="<%# "callback(gridpending,'dp:" + Container.KeyValue.ToString().Replace("'","\\'") + "', false, null)"%>" />
                                              </td>
                                            </tr>
                                        </table>      
                                    </DataItemTemplate>
                                 </dxuc:GridViewDataColumn>
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
    
        <dxuc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" HeaderText="" width="800px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False"><ContentCollection><dxuc:PopupControlContentControl ID="PopupControlContentControl1" runat="server" Height="100%">
        <dxuc:ASPxCallbackPanel ID="panel" runat="server" ClientInstanceName="panel" 
                OnCallback="panel_Callback" >
        <ClientSideEvents EndCallback="function(s, e){ if(panel.cp_action=='s'){processing=false;callback(gridpending);}}" />
        <PanelCollection><dxuc:PanelContent ID="PanelContent1" runat="server">
                <uc1:USC_paraminput ID="USC_paraminput1" runat="server" />
        </dxuc:PanelContent></PanelCollection></dxuc:ASPxCallbackPanel>        
        </dxuc:PopupControlContentControl></ContentCollection></dxuc:ASPxPopupControl>        
        
        </td></tr>
        </table>
        
        </div>
    </form>
</body>
</html>
