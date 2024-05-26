<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer_Register.aspx.cs" Inherits="Admin_DBProj.Customer.Register.Customer_Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>COFFEING - Register</title>
    <link type="text/css" rel="stylesheet" href="~/Customer/css/style.css">
</head>
<body>
    <form id="form1" runat="server">
        <nav>
            <h1><a href="cust_login.aspx">COFFEING</a></h1>
        </nav>
        <hr />
        <div class="container">
            <h1 class="h1-container">CREATE ACCOUNT</h1>
            <div class="input-group-2">
                <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" CssClass="input-textbox" />
                <asp:TextBox ID="txtLastName" runat="server" placeholder="Last Name" CssClass="input-textbox" />
            </div>
            <div class="input-group-2">
                <asp:TextBox ID="txtAddress" runat="server" placeholder="Address" CssClass="input-textbox" />
                <asp:TextBox ID="txtContactNumber" runat="server" placeholder="Contact No." CssClass="input-textbox" />
            </div>
            <div class="input-group">
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="input-textbox" TextMode="Email" />
            </div>
            <div class="input-group">
                <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" CssClass="input-textbox" TextMode="Password" />
                <asp:TextBox ID="txtConfirmPassword" runat="server" placeholder="Confirm Password" CssClass="input-textbox" TextMode="Password" />
            </div>
            <div class="links-container-2">
                <p>Already have an account? <a href="../Login/Customer_Login.aspx" class="login-link">Login</a></p>
            </div>
            <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="register-button" OnClick="btnRegister_Click" />
            <div class="other-links-container">
                <p class="ch-txt">or sign up with</p>
                <hr />
            </div>
        </div>
        <div class="img-container">
            <img class="bg-design-4" src="../../Images/beans2.png" />
            <img class="bg-design-3" src="../../Images/beans2.png" />
            <img class="bg-design-2" src="../../Images/beans2.png" />
        </div>
    </form>
</body>
</html>