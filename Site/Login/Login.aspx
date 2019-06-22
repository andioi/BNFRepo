<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MikroLogin.Login" %>

<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CBAS</title>
    <!-- <link rel="stylesheet" type="text/css" href="include/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="include/font-awesome/css/font-awesome.min.css" /> -->
	<link rel="stylesheet" type="text/css" href="login-custom.css" />
    <!-- <script type="text/javascript" src="include/jquery-2.1.0.js"></script>
    <script type="text/javascript" src="include/bootstrap/js/bootstrap.min.js"></script> -->

	<link href="include/bootstrap/4.0.0/bootstrap.min.css" rel="stylesheet" />
	<script src="include/bootstrap/4.0.0/bootstrap.min.js"></script>
	<script src="include/jquery-1.11.1.min.js"></script>

    <script lang="javascript" type="text/javascript">
        if (top != self) { top.location = self.location; }
    </script>
</head>
<body>

<section class="login-block">
    <div class="container">
	<div class="row">
		<div class="col-md-4 login-sec">
		    <!-- <h2 class="text-center">Login iFOS</h2> -->
			<div style="text-align:center;margin-top:-10px;margin-bottom:20px"><img src="image/skyworx-logo-2018.png" width="200" style="display:none" /></div>
<form class="login-form" id="form1" method="post" runat="server">
  <div class="form-group">
    <label for="exampleInputEmail1" class="text-uppercase">Userid</label>
    <asp:TextBox ID="TXT_USERNAME" runat="server" CssClass="form-control" placeholder="User ID" required></asp:TextBox>
  </div>
  <div class="form-group">
    <label for="exampleInputPassword1" class="text-uppercase">Password</label>
	<asp:TextBox ID="TXT_PASSWORD" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" required></asp:TextBox>
  </div>
  
  
    <div class="form-check">
    <!-- <label class="form-check-label">
      <input type="checkbox" class="form-check-input">
      <small>Remember Me</small>
    </label> -->
	<asp:HiddenField ID="callback" runat="server" />
	<small><asp:Label ID="lblMessage" runat="server" CssClass="control-label" forecolor="red"></asp:Label></small>
	<asp:Button ID="BTN_SUBMIT" runat="server" Text="L o g i n" CssClass="btn btn-login float-right" OnClick="BTN_SUBMIT_Click"></asp:Button>
  </div>
  
</form>
<div class="copy-text" style="display:none">copyright @ 2017 <a href="http://www.skyworx.co.id/">PT. ASLI RI</a></div>
		</div>
		<div class="col-md-8 banner-sec d-none d-md-block">
			<div class="banner-text"><h2>Credit Bureau Automation System (CBAS)</h2>
		<p>One Stop Checking System</p>
		</div>
         <!--  <div id="carouselExampleIndicators" class="carousel slide" data-ride="carousel">
                 <ol class="carousel-indicators">
                    <li data-target="#carouselExampleIndicators" data-slide-to="0" class="active"></li>
                    <li data-target="#carouselExampleIndicators" data-slide-to="1"></li>
                    <li data-target="#carouselExampleIndicators" data-slide-to="2"></li>
                  </ol>
            <div class="carousel-inner" role="listbox">

    <div class="carousel-item active">
      <img class="d-block img-fluid" src="https://static.pexels.com/photos/33972/pexels-photo.jpg" alt="First slide">
      <div class="carousel-caption d-none d-md-block">
        <div class="banner-text">
            <h2>This is Heaven</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation</p>
        </div>	
	  </div>
    </div>

    <div class="carousel-item">
      <img class="d-block img-fluid" src="https://images.pexels.com/photos/7097/people-coffee-tea-meeting.jpg" alt="First slide">
      <div class="carousel-caption d-none d-md-block">
        <div class="banner-text">
            <h2>This is Heaven</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation</p>
        </div>	
      </div>
    </div>

    <div class="carousel-item">
      <img class="d-block img-fluid" src="https://images.pexels.com/photos/872957/pexels-photo-872957.jpeg" alt="First slide">
      <div class="carousel-caption d-none d-md-block">
        <div class="banner-text">
            <h2>This is Heaven</h2>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation</p>
        </div>	
    </div>
  </div> -->

            </div>	   
		    
		</div>
	</div>
</div>
</section>

</body>
</html>