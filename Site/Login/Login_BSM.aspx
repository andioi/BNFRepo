<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MikroLogin.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Login :: iDEB CHECKING</title>
		<link href="style.css" type="text/css" rel="stylesheet" />
		<!-- #include file="include/onepost.html" -->
    <!-- Bootstrap -->
    <link href="vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="vendors/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <!-- NProgress -->
    <link href="vendors/nprogress/nprogress.css" rel="stylesheet" />
    <!-- Animate.css -->
    <link href="vendors/animate.css/animate.min.css" rel="stylesheet" />

    <!-- Custom Theme Style -->
    <link href="include/custom.css" rel="stylesheet" />
		<script language="JavaScript" type="text/javascript">
			if (top != self) { top.location = self.location; }
			function kutip_satu()
			{
				if ((event.keyCode == 35) || (event.keyCode == 39))
				{
					return false;
				}
				else
				{
					return true;
				}
			}
		</script>
</head>
<body class="login">
	<div style="background-color:#00573D;height:60px;float:top;"></div>
	<div style="background-color:#FFCB0B;height:20px;"></div>
    <div>
        <a class="hiddenanchor" id="signup"></a>
        <a class="hiddenanchor" id="signin"></a>
		<div class="animate form login_form">
			<section class="login_content" >
				<img src="image\logo-BSM-white.png" style="margin:20px 0 30px;" />
				<form runat="server" id="form1">
					<h1>iDEB Checking System</h1>
					<asp:Label ID="lblMessage" runat="server" CssClass="small" ForeColor="Red" ></asp:Label>
					<div class="col-xs-12 form-group has-feedback">
						<asp:TextBox  class="form-control has-feedback-left" runat="server" ID="TXT_USERNAME" placeholder="Userid"></asp:TextBox>
						<span class="fa fa-user form-control-feedback left" aria-hidden="true"></span>
					</div>
					<div class="col-xs-12 form-group has-feedback">
						<asp:TextBox TextMode="Password"  class="form-control has-feedback-left" runat="server" ID="TXT_PASSWORD" placeholder="Password"></asp:TextBox>
						<span class="fa fa-key form-control-feedback left" aria-hidden="true"></span>
					</div>

					<div class="col-xs-12" style="text-align:center">
                        <asp:HiddenField ID="callback" runat="server" />
						<asp:Button CssClass="btn btn-success btn-lg submit"  ID="btnLogin" runat="server" Text="LOGIN" Style=" text-decoration:none;background-color:#FDC746;color:#00573D;" OnClick="BTN_SUBMIT_Click" />
					</div>

					
					<div class="separator">

						<div class="clearfix"></div>
						<br />

						<div style="margin-top:50px">
							<h2>PT Bank Syariah Mandiri</h2>
							<p>© 2018 All Rights Reserved.</p>
						</div>
					</div>
				</form>
			</section>
		</div>
    </div>
	<div class="new-footer"> abc
	</div>
</body>

</html>