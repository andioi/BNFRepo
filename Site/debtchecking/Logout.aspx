<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="DebtChecking.Logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <!-- Bootstrap -->
    <link href="vendors/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
       html,
        body {
            height: 100%;
        }

        .container {
            height: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-top:100px;
        }
    </style>

    <title>Logout</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="background-color:#776A56;height:60px;float:top;"></div>
	<div style="background-color:#FF7A00;height:20px;"></div>
        <div class="container">
                <div><h4>your session has expired, please login again</h4></div>
        </div>
    </form>
</body>
</html>
