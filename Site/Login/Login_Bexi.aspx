<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MikroLogin.Login" %>

<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html><head>
<meta http-equiv="content-type" content="text/html; charset=UTF-8">
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <title>iFOS - Signin</title>
                <!-- Tell the browser to be responsive to screen width -->
        <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
        <link rel="stylesheet" type="text/css" href="include/bootstrap/css/bootstrap.min.css" />
		<link rel="stylesheet" type="text/css" href="include/font-awesome/css/font-awesome.min.css" />
        <!-- Ionicons -->
        <link rel="stylesheet" href="ionicons.css">
        <!-- Theme style -->
        <link rel="stylesheet" href="AdminLTE.css">
        <!-- iCheck -->
        <link rel="stylesheet" href="blue.css">

        <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
        <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
        <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
        <![endif]-->
		<script type="text/javascript" src="include/jquery-2.1.0.js"></script>
		<script type="text/javascript" src="include/bootstrap/js/bootstrap.min.js"></script>
		<script lang="javascript" type="text/javascript">
			if (top != self) { top.location = self.location; }
		</script>
    </head>
    <body class="hold-transition" style="background-size: 100% 100%" background="image/target-customer.png">
        <div class="col-md-9">
            <div class="pull-left">
                <!--<img class="img-responsive" src="http://172.16.10.94/CDD/dist/img/photo1.png" alt="Photo">-->
            </div>
        </div>
        <div class="col-md-3 login-box" style="float:right">
            <div class="">
                <div class="login-logo">
                    <img src="image/eximbank_logo.png">
                </div>
                <!-- /.login-logo -->
                <div class="login-box-body">
                    <p class="login-box-msg">Credit Bureau Automation System</p>

                    <form id="form1" method="post" runat="server">
                                        <div class="form-group has-feedback">
                        <asp:TextBox ID="TXT_USERNAME" runat="server" CssClass="form-control" placeholder="User ID" required></asp:TextBox>
                        <span class="glyphicon glyphicon-user form-control-feedback"></span>
                    </div>
                    <div class="form-group has-feedback">
                        <asp:TextBox ID="TXT_PASSWORD" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" required></asp:TextBox>
                        <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                    </div>
                    <div class="row">
                        <div class="col-xs-8">
                            <div class="checkbox icheck">
                                <label>
                                    <asp:Label ID="Label1" runat="server" CssClass="control-label"></asp:Label>
                                </label>
                            </div>
                        </div>
                        <!-- /.col -->
                        <div class="col-xs-4">
                            <asp:Button ID="BTN_SUBMIT" runat="server" Text="Sign In" CssClass="btn btn-primary submit-btn" OnClick="BTN_SUBMIT_Click"></asp:Button>
                        </div>
                        <!-- /.col -->
                    </div>

                    </form>

                    <!-- /.social-auth-links -->

                </div>
                <!-- /.login-box-body -->
            </div>
            <!-- /.login-box -->
            <br>
            <div class="col-white text-center"><p>Copyright@2018</p></div>
        </div>
        <!-- jQuery 2.2.3
        <script src="include/jquery-2.js"></script> -->
        <!-- Bootstrap 3.3.6
        <script src="include/bootstrap.js"></script>  -->
        <!-- iCheck -->
        <script src="include/icheck.js"></script>
        <script>
            $(function () {
                $('input').iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-blue',
                    increaseArea: '20%' // optional
                });
            });
        </script>
    

</body></html>