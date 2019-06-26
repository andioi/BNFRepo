<%@ Page Language="C#" validateRequest="false" AutoEventWireup="true" CodeBehind="ParamApprv.aspx.cs" Inherits="MikroMnt.Parameter.ParamApprv" %>

<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register src="USC_paraminput.ascx" tagname="USC_paraminput" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->
    <!-- #include file="~/include/uc/UC_Currency.html" -->
    <!-- #include file="~/include/uc/UC_Number.html" -->
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" language="javascript">
    
        function all()
        {            
            var keyAPPRV = new Array(); 
            keyAPPRV = document.getElementById("hAPPRV").value.split(","); 
            for(x=0;x<keyAPPRV.length;x++)
            {
                rbAPPRV = document.getElementById("rbAPPRV"+keyAPPRV[x]);
                if(rbAPPRV!=null)
                   rbAPPRV.checked = true; 
            }
            var keyREJCT = new Array(); 
            keyREJCT = document.getElementById("hREJCT").value.split(",");
            for(y=0;y<keyREJCT.length;y++)
            {
                rbREJCT = document.getElementById("rbREJCT"+keyREJCT[y]);
                if(rbREJCT!=null)
                    rbREJCT.checked = true; 
            }
            var keyCANCL = new Array(); 
            keyCANCL = document.getElementById("hCANCL").value.split(",");
            for(z=0;z<keyCANCL.length;z++)
            {
                rbCANCL = document.getElementById("rbCANCL"+keyCANCL[z]);
                if(rbCANCL!=null)
                    rbCANCL.checked = true; 
            }
        }
    
        function allAPPRV()
        {
            document.getElementById("hAPPRV").value = document.form1.hALL.value;
            document.getElementById("hCANCL").value = "";
            document.getElementById("hREJCT").value = "";
            all();
        }

        function allREJCT()
        {
            document.getElementById("hAPPRV").value = "";
            document.getElementById("hREJCT").value = document.form1.hALL.value;
            document.getElementById("hCANCL").value = "";
            all();
        }


        function allCANCL()
        {
            document.getElementById("hAPPRV").value = "";
            document.getElementById("hREJCT").value = "";
            document.getElementById("hCANCL").value = document.form1.hALL.value;
            all();            
        }
        
        function APPRV(key)
        {
            if(document.getElementById("hAPPRV").value.indexOf(key+",")<0)
                document.getElementById("hAPPRV").value += key+",";
            document.getElementById("hREJCT").value = document.getElementById("hREJCT").value.replace(key+",", "");
            document.getElementById("hCANCL").value = document.getElementById("hCANCL").value.replace(key+",", "");
        }
        
        function REJCT(key)
        {
            if(document.getElementById("hREJCT").value.indexOf(key+",")<0)
                document.getElementById("hREJCT").value += key+",";
            document.getElementById("hAPPRV").value = document.getElementById("hAPPRV").value.replace(key+",", "");
            document.getElementById("hCANCL").value = document.getElementById("hCANCL").value.replace(key+",", "");
        }
        
        function CANCL(key)
        {
            if(document.getElementById("hCANCL").value.indexOf(key+",")<0)
                document.getElementById("hCANCL").value += key+",";
            document.getElementById("hAPPRV").value = document.getElementById("hAPPRV").value.replace(key+",", "");
            document.getElementById("hREJCT").value = document.getElementById("hREJCT").value.replace(key+",", "");
        }

    
    </script>
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
        <input type="hidden" id="hALL" runat="server" />
        <input type="hidden" id="hAPPRV" runat="server" />
        <input type="hidden" id="hCANCL" runat="server" />
        <input type="hidden" id="hREJCT" runat="server" />
        
        <dx:ASPxGridView ID="gridpending" ClientInstanceName="gridpending" 
            runat="server" AutoGenerateColumns="False" Width="100%" Font-Size="10px"
            OnLoad="gridpending_Load"
            OnBeforeColumnSortingGrouping="gridpending_BeforeColumnSortingGrouping" >
            <SettingsPager PageSize="20" />
            <Settings ShowFilterRow="True" ShowGroupedColumns="True" ShowGroupPanel="True" />
            <ClientSideEvents  EndCallback="function(s, e) {
	        all();
}" />
             <Columns>
             <dx:GridViewCommandColumn VisibleIndex="0" Width="1px">
                 <HeaderTemplate>
                    <table>
                        <tr class="<%=funcpendCss %>" >
                          <td>
                            <input class="Bt1" type="button" onclick="gridpending.ExpandAll();"   value=" Expand All " /> 
                          </td>
                        </tr>
                        <tr class="<%=funcpendCss %>">
                          <td>
                            <input class="Bt1" type="button" onclick="gridpending.CollapseAll();" value="Collapse All" />   
                          </td>
                        </tr>
                    </table>                                        
                 </HeaderTemplate>
             </dx:GridViewCommandColumn>
             <dx:GridViewDataColumn VisibleIndex="1" Width="1%" >
                 <CellStyle Wrap="False">
                 </CellStyle>
                 <HeaderTemplate>
                    <table width="100%">
                        <tr>
                          <td align="center">
                                Approve
                          </td>
                        </tr>
                        <tr align="center">
                          <td>
                             <a href="javascript:allAPPRV()">All</a>
                          </td>
                        </tr>
                    </table>                                        
                 </HeaderTemplate>
                <DataItemTemplate>
                    <table width="100%">
                        <tr>
                          <td align="center">
                             <input type="radio" id="<%# "rbAPPRV"+Container.KeyValue.ToString() %>" name="<%# "rb"+Container.KeyValue.ToString() %>" onclick="<%# "APPRV('"+Container.KeyValue.ToString() +"')" %>" />
                          </td>
                        </tr>
                    </table>      
                </DataItemTemplate>
             </dx:GridViewDataColumn>
             <dx:GridViewDataColumn VisibleIndex="2" Width="1%" >
                 <CellStyle Wrap="False">
                 </CellStyle>
                 <HeaderTemplate>
                    <table width="100%">
                        <tr>
                          <td align="center">
                                Reject
                          </td>
                        </tr>
                        <tr align="center">
                          <td>
                             <a href="javascript:allREJCT()">All</a>
                          </td>
                        </tr>
                    </table>                                        
                 </HeaderTemplate>
                <DataItemTemplate>
                    <table width="100%">
                        <tr>
                          <td align="center">
                             <input type="radio" id="<%# "rbREJCT"+Container.KeyValue.ToString() %>" name="<%# "rb"+Container.KeyValue.ToString() %>"  onclick="<%# "REJCT('"+Container.KeyValue.ToString() +"')" %>"/>
                          </td>
                        </tr>
                    </table>      
                </DataItemTemplate>
             </dx:GridViewDataColumn>
             <dx:GridViewDataColumn VisibleIndex="3" Width="1%" >
                 <CellStyle Wrap="False">
                 </CellStyle>
                 <HeaderTemplate>
                    <table width="100%">
                        <tr>
                          <td align="center">
                                Cancel
                          </td>
                        </tr>
                        <tr align="center">
                          <td>
                             <a href="javascript:allCANCL()">All</a>
                          </td>
                        </tr>
                    </table>                                        
                 </HeaderTemplate>
                <DataItemTemplate>
                    <table width="100%">
                        <tr>
                          <td align="center">
                             <input type="radio" id="<%# "rbCANCL"+Container.KeyValue.ToString() %>" name="<%# "rb"+Container.KeyValue.ToString() %>" onclick="<%# "CANCL('"+Container.KeyValue.ToString() +"')" %>" />
                          </td>
                        </tr>
                    </table>      
                </DataItemTemplate>
             </dx:GridViewDataColumn>
             </Columns>
             </dx:ASPxGridView>
        </td>
    </tr>
    <tr class="F1">
        <td>
            <asp:Button class="btn btn-xs btn-success"  ID="btnSubmit" runat="server" Text="Submit" 
                onclick="btnSubmit_Click" />
        </td>
        
    </tr>
    </table>  
    </div>
    </form>
</body>
</html>
