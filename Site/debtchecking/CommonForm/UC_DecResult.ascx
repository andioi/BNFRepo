<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_DecResult.ascx.cs" Inherits="DebtChecking.CommonForm.UC_DecResult" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v8.2" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
        <dxwgv:ASPxGridView ID="GridViewDec" ClientInstanceName="GridViewDec" runat="server" AutoGenerateColumns="False" 
            onload="GridViewDec_Load"  KeyFieldName="RESULT_ID" >
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="System Name" FieldName="DEC_DESC" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Result" FieldName="DEC_RES" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <SettingsDetail ShowDetailRow="true"/>
            <ClientSideEvents EndCallback="function (s,e) {try { parent.resizeFrame(); } catch (e) {} }" />
            <Templates>
             <DetailRow>
                     <dxwgv:ASPxGridView ID="GridViewDecDtl" runat="server"  Width="100%" AutoGenerateColumns="false"
                     onload="GridViewDecDtl_Load" >
                      <Columns>
                            <dxwgv:GridViewDataColumn FieldName="ITEM_DESC" Caption="Item Name" VisibleIndex="1">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="RES_DESC" Caption="Item Result" VisibleIndex="3">
                            </dxwgv:GridViewDataColumn>                            
                        </Columns>                         
                         <SettingsDetail IsDetailGrid="true"/>
                     
                     </dxwgv:ASPxGridView>
             </DetailRow>
         </Templates>
        </dxwgv:ASPxGridView>
        <asp:Label ID="LBL_MSG" runat="server" ></asp:Label>
