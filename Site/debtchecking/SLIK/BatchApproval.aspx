<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatchApproval.aspx.cs" Inherits="DebtChecking.SLIK.BatchApproval" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
     <!-- #include file="~/include/onepost.html" -->
      <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="../vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- Animate.css -->
    <link href="../vendors/animate.css/animate.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function getMandatory() {
            //return true;
            var detan = document.getElementById("mainPanel_PopupRemark_PNL_REMARK_detAN_MESSAGE");            
            if (detan.value == "") {
                alert('Reject reason must be fill !')
                return false;
            }
            else {
                return true;
            }
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>

      <dxp:ASPxCallbackPanel ID="mainPanel" ClientInstanceName="mainPanel" runat="server" Width="100%" OnCallback="mainPanel_Callback">     
                <ClientSideEvents  EndCallback="function(s, e) {
                                                if(s.hasOwnProperty('cp_export') && s.cp_export!='')
	                                            {
		                                            window.open(s.cp_export);
		                                            s.cp_export = '';} }" />
                <PanelCollection>
                <dxp:PanelContent runat="server">
       
                    <table class="Box1" width="100%">    
                    <tr class="H1" ><td>
                       <b>
                        <asp:Label ID="TitleHeader" runat="server"></asp:Label>
                       </b>
                   </td></tr>                   
                   <tr><td> 
                        <dxp:ASPxGridView ID="grid" ClientInstanceName ="grid" runat="server" CssClass="table" 
                        AutoGenerateColumns="true" Width="100%" onload="grid_Load"  SettingsPager-PageSize="50"
                           >                        
                        </dxp:ASPxGridView></td></tr>
                        <tr runat="server" id="btnExporter" visible="true">
                            <td>
                                 <input id="Button1" runat="server" class="btn btn-xs btn-danger" onclick="callback(mainPanel, 'e:', false);" type="button" value="Export to Excel"/>
                            </td>
                        </tr>
                        <tr runat="server" id="grpBtnApprove" visible="false">
                            <td>
                               <center>  <input id="BTN_SAVE" runat="server" class="btn btn-xs btn-danger" onclick="PopupRemark.Show();" type="button" value="Reject"/>
                                   &nbsp;
                                   &nbsp;
                                   <input id="BTN_SUBMIT" runat="server" class="btn btn-xs btn-danger" onclick="if (confirm('Approve this batch, Are you sure ?')) callback(mainPanel, 'a:', false); return false;" type="button" value="Approve"/> 
                               </center>
                            </td>
                        </tr>                         
                    </table>
                    

         <dxp:ASPxPopupControl ID="PopupRemark" ClientInstanceName="PopupRemark" runat="server" HeaderText="Reject Reason" width="800px" PopupHorizontalAlign="WindowCenter" 
           PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
        <ContentCollection>
        <dxp:PopupControlContentControl ID="popupControlRemark" runat="server" Height="100%">
            <dxp:ASPxCallbackPanel ID="PNL_REMARK" runat="server" ClientInstanceName="PNL_REMARK" 
                OnCallback="PNL_REMARK_Callback" >
                <PanelCollection>
                <dxp:PanelContent ID="PanelContent6" runat="server">
                <table width="100%" class="Box1">
                    <tr>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="B11">
                                        <asp:TextBox runat="server" ID="detAN_MESSAGE" TextMode="MultiLine" Width="100%" Rows="3" CssClass="mandatory" ></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                <tr class="F1">
                    <td align="center">
                        <input id="Button7" runat="server" type="button" value="Reject" class="btn btn-xs btn-danger" onclick="if (getMandatory()) callback(PNL_REMARK, 'r:', false); return false;" />                       
                    </td>
                </tr>
                </table>
                </dxp:PanelContent>
                </PanelCollection>
            </dxp:ASPxCallbackPanel>
        </dxp:PopupControlContentControl>
        </ContentCollection>
        </dxp:ASPxPopupControl>


                </dxp:PanelContent>
                </PanelCollection>
                </dxp:ASPxCallbackPanel>

        </div>
    </form>
</body>
</html>
