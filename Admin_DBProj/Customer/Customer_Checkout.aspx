<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer_Checkout.aspx.cs" Inherits="Admin_DBProj.Customer.Customer_Checkout" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>COFFEEING - Checkout</title>
    <!-- Include Bootstrap and Font Awesome CSS -->
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css'>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css'>
    <!-- Your custom CSS file -->
    <link href="~/Customer/css/checkout.css" rel="stylesheet" type="text/css">
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div class="logo">
                <a href="Customer_Homepage.aspx"><span>COFFEING</span></a>
            </div>
            <nav>
                <a href="#"><i class="fas fa-search"></i></a>
                <a href="#"><i class="fas fa-bell"></i></a>
                <a href="Customer_Homepage.aspx">Home</a>
                <a href="Customer_Products.aspx">Shop</a>
            </nav>
        </header>
        <section class="container mt-5">
            <div class="row justify-content-center text-center">
                <div class="col-md-8 col-lg-6">
                    <div class="header">
                        <h2>Confirm your Order</h2>
                    </div>
                </div>
            </div>
            <div class="card">
                <div class="card-body">
                    <!-- Order Summary -->
                    <div>
                        <h4>Order Summary</h4>
                        <asp:Repeater ID="rptOrderSummary" runat="server">
                            <ItemTemplate>
                                <p><strong>Product Name:</strong> <%# Eval("ProductName") %></p>
                                <p><strong>Price:</strong> $<%# Eval("ProductPrice", "{0:0.00}") %></p>
                                <p><strong>Quantity:</strong> <%# Eval("Quantity") %></p>
                                <hr />
                            </ItemTemplate>
                        </asp:Repeater>
                        <div>
                            <p><strong>Subtotal:</strong> $<asp:Literal runat="server" ID="ltlSubtotal" /></p>
                            <p><strong>Tax:</strong> $<asp:Literal runat="server" ID="ltlTax" /></p>
                            <p><strong>Shipping:</strong> $<asp:Literal runat="server" ID="ltlShipping" /></p>
                            <p><strong>Grand Total:</strong> $<asp:Literal runat="server" ID="ltlGrandTotal" /></p>
                            <!-- Payment Method Dropdown -->
                            <label for="payment-method"><strong>Payment Method:</strong></label>
                            <select id="payment-method" name="payment-method">
                                <option value="1">Cash</option>
                                <option value="2">GCash</option>
                                <option value="3">Credit Card</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <button type="button" class="btn btn-primary btn-confirm" onclick="confirmOrder()">Confirm Order</button>
            <asp:Button ID="btnConfirmOrder" runat="server" Text="Confirm" OnClick="ConfirmOrder" Style="display: none;" />
        </section>
    </form>
    <script type="text/javascript">
        function confirmOrder() {
            document.getElementById('<%= btnConfirmOrder.ClientID %>').click();
        }
    </script>
</body>
</html>