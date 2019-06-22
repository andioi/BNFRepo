<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManualReview.aspx.cs" Inherits="DebtChecking.Verification.ManualReview" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxcp" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxp" %>

<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1" namespace="DevExpress.Web" tagprefix="dxpc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SID Text</title>
    <link href="../include/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/include/onepost.html" -->    
    <!-- Bootstrap -->
    <link href="../vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .boxbold { border:1px solid silver; text-align:right; }
        .boxboldleft { border:1px solid silver; text-align:left; font-size:x-small;}
        .boxboldcenter { border:1px solid silver; text-align:center; font-size:small; }
        .boxboldright { border:1px solid silver; text-align:right; font-size:small; }
        body {text-align:center;}
        table {font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;font-size:small}
        .td01 { width:300px;text-align:left; padding-left:4px;}
        .td02 { width:150px;text-align:left; padding-left:4px}
        .td11 { text-align:left; padding-left:4px}
        .td12 { text-align:right; padding-left:4px}
        .Box2
        {
	        background-color: white;
	        border-style: solid;
	        border-width: 0px 1px 1px 1px;
	        border-color: #005263 #C0C0C0 #C0C0C0 #005263;
        }
        .title1 { 
            font-size:small;font-weight:bold;
            font-family:'Franklin Gothic Medium', 'Arial Narrow', 'Arial', 'sans-serif';
            text-align:left;background-color:PeachPuff;
        }
        .title2 { 
            font-size:small;font-weight:bold;
            font-family:'Franklin Gothic Medium', 'Arial Narrow', 'Arial', 'sans-serif';
            text-align:left;background-color:lightskyblue;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function kliksave() {
            callback(mainPanel, 's');
        }

        function findLableForControl(el) {
            var idVal = el.id;
            labels = document.getElementsByTagName('label');
            for (var i = 0; i < labels.length; i++) {
                if (labels[i].htmlFor == idVal)
                    return labels[i];
            }
        }

        function sameprsn_tick(obj)
        {
            var lb = findLableForControl(obj);
            if (obj.checked) {
                lb.style.color = "red";
            } else {
                lb.style.color = "black";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="phonever_flag" runat="server" />
        <input type="hidden" id="phonever_flag_qc" runat="server" />
    <div>
    <dxcp:ASPxCallbackPanel ID="mainPanel" runat="server" Width="100%" oncallback="mainPanel_Callback" ClientInstanceName="mainPanel">
    <PanelCollection>
        <dxp:PanelContent ID="PanelContent1" runat="server">
            <table id="DataDebitur" class="Box1" width="100%">
                <tr>
                    <td colspan="2" align="left">
                        Select other customer in this requestid <asp:DropDownList ID="ddl_appid" runat="server" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
			    <tr>
				    <td class="H1" colspan="2">Customer Data</td>
			    </tr>
			    <tr valign="top">
			        <td width="50%">
			            <table class="Tbl0" width="100%">
			                <tr>
			                    <td class="B01">Customer Name</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label Font-Bold="true" Font-Size="Medium" ID="cust_name" runat="server"></asp:Label>
                                    <input type="hidden" runat="server" id="appid" />
                                    <input id="reffnumber" runat="server" type="hidden" />
                                    <input id="nikcount" runat="server" type="hidden" />
                                    <asp:Label ID="status_app" runat="server" style="display:none"></asp:Label>
                                    <a href="javascript:PopupPage('DetailCustomer.aspx?regno='+document.getElementById('mainPanel_appid').value,640,400)">[detail]</a>
			                    </td>
			                </tr>
			                <tr>
			                    <td class="B01">Place / Date of Birth</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="pob_dob" runat="server"></asp:Label></td>
			                </tr>
                            <tr>
			                    <td class="B01">NPWP</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="npwp" runat="server"></asp:Label></td>
			                </tr>
                            <tr style="display:none">
			                    <td class="B01">Gender</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="genderdesc" runat="server"></asp:Label></td>
			                </tr>
                            <tr>
			                    <td class="B01">KTP / NIK</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="ktp" runat="server"></asp:Label></td>
			                </tr>
			            </table>
			        </td>
			        <td width="50%">
			            <table width="100%">
                            <tr>
			                    <td class="B01">Mother Meiden Name</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="mother_name" runat="server"></asp:Label></td>
			                </tr>
						    <tr>
			                    <td class="B01">ID Address</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="full_ktpaddress" runat="server"></asp:Label></td>
			                </tr>
                            <tr style="display:none">
			                    <td class="B01">Policy Result</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label Font-Bold="true" Font-Size="Small" ForeColor="Red" ID="final_policy" runat="server"></asp:Label></td>
			                </tr>
			                <tr>
			                    <td class="B01">Home Address</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="full_homeaddress" runat="server"></asp:Label></td>
			                </tr>
			                <tr style="display:none">
			                    <td class="B01">Emergency Name / Address</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="full_econaddress" runat="server"></asp:Label></td>
			                </tr>
			                <tr>
			                    <td class="B01">Office Name / Address</td>
			                    <td class="BS">:</td>
			                    <td class="B11"><asp:Label ID="full_officeaddress" runat="server"></asp:Label>
			                    </td>
			                </tr>
                        </table>
			        </td>
			    </tr>
                <tr><td colspan="2"><input type="button" value="Save" onclick="callback(mainPanel,'s', false, null);" class="btn btn-xs btn-success" /></td></tr>
                <tr>
                    <td colspan="2" class="H1">MATCH TAGGING</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%" class="Tbl0">
                            <tr>
                                <td width="33%"><asp:CheckBox runat="server" ID="HADD_MATCH" Text="Home Address Match" /></td>
                                <td width="33%"><asp:CheckBox runat="server" ID="BADD_MATCH" Text="Business Address Match" /></td>
                                <td width="33%"><asp:CheckBox runat="server" ID="COYNAME_MATCH" Text="Company Name Match" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
		    </table>
            <div id="div1" class="H1">IDEB RESULT</div>
            <div id="divContent" runat="server" style="border:solid 1px black;"><asp:Label ID="lblnodata1" runat="server">NO DATA</asp:Label></div>
            <input type="button" id="btnsave" runat="server" onclick="hidesavebtn()" value="Save" class="button1" style="display:none" />
			<div id="div2" class="title2" style="display:none">Auto Match</div>
            <div id="divContent2" runat="server" style="display:none"><asp:Label ID="lblnodata2" runat="server">NO DATA</asp:Label></div>

		</dxp:PanelContent>
    </PanelCollection>
    </dxcp:ASPxCallbackPanel>
            
    </div>
    </form>
</body>
</html>
