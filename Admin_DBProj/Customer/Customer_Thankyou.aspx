<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer_Thankyou.aspx.cs" Inherits="Admin_DBProj.Customer.Customer_Thankyou" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Thank You - Coffee Shop</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #F8F5EC;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .thank-you-container {
            text-align: center;
            background-color: #fff;
            padding: 50px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .thank-you-container h1 {
            font-size: 2em;
            margin-bottom: 20px;
            color: #333;
        }
        .thank-you-container p {
            font-size: 1.2em;
            margin-bottom: 30px;
            color: #666;
        }
        .button-container {
            display: flex;
            justify-content: center;
            gap: 20px;
        }
        .button {
            background-color: #b37333;
            color: white;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            text-decoration: none;
            transition: background-color 0.3s;
        }
        .button:hover {
            background-color: #a2622c;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="thank-you-container">
            <h1>Thank You for Your Order!</h1>
            <p>Your order has been successfully placed. We appreciate your business!</p>
            <div class="button-container">
                <asp:Button ID="Button1" runat="server" Text="Order Again" CssClass="button" OnClick="Button1_Click" />
                <asp:Button ID="Button2" runat="server" Text="Home" CssClass="button" OnClick="Button2_Click" />
            </div>
        </div>
    </form>
</body>
</html>