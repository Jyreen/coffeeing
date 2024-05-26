﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer_Cart.aspx.cs" Inherits="Admin_DBProj.Customer.Customer_Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>COFFEEING - Cart</title>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css'>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css'>
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i"
        rel="stylesheet">
    <link type="text/css" rel="stylesheet" href="~/Customer/css/cart.css">
</head>
<body>
    <form id="form1" runat="server">
    <header class="auto-style1">
        <!-- Hidden field to store cart data -->
        <input type="hidden" id="cartDataHiddenField" runat="server" />

        <a href="../App_Start/"></a>
        <div class="logo">
            <a href="Customer_Homepage.aspx"><span>COFFEING</span></a>
        </div>
       <nav>
            <a href="#"><i class="fas fa-search"></i></a> <!-- Magnifying glass icon -->
            <a href="#"><i class="fas fa-bell"></i></a> <!-- Notification icon -->
            <a href="Customer_Homepage.aspx">Home</a>
            <a href="Customer_Products.aspx">Shop</a> <!-- Link to your shop page -->
      
        </nav>
    </header>
        <section>
            <div class="shopping-cart">
                <div class="row justify-content-center text-center">
                    <div class="col-md-8 col-lg-6">
                        <div class="header">
                            <h2>Your Cart</h2>
                        </div>
                    </div>
                </div>
                <div class="cart-grid">
                    <% foreach (var cartItem in CartItems) { %>
                        <div class="cart-item">
                            <img src="<%= cartItem.ProductImg %>" alt="<%= cartItem.ProductName %>">
                            <div class="cart-item-details">
                                <div class="cart-item-name"><%= cartItem.ProductName %></div>
                                <div class="cart-item-description"><%= cartItem.ProductDesc %></div>
                                <div class="cart-item-price">$<%= cartItem.ProductPrice.ToString("0.00") %></div>
                            </div>
                            <div class="quantity-and-remove">
                                <input type="number" class="quantity-input" value="<%= cartItem.Quantity %>" min="1" data-product-name="<%= cartItem.ProductName %>" />
                                <button class="remove-from-cart-button" onclick="removeFromCart('<%= cartItem.ProductName %>')">Remove</button>
                            </div>
                        </div>
                    <% } %>
                </div>

                <div class="totals">
                    <div class="totals-item">
                        <label>Subtotal</label>
                        <div class="totals-value" id="cart-subtotal"><%= Subtotal.ToString("0.00") %></div>
                    </div>
                    <div class="totals-item">
                        <label>Tax (5%)</label>
                        <div class="totals-value" id="cart-tax"><%= Tax.ToString("0.00") %></div>
                    </div>
                    <div class="totals-item">
                        <label>Shipping</label>
                        <div class="totals-value" id="cart-shipping"><%= Shipping.ToString("0.00") %></div>
                    </div>
                    <div class="totals-item totals-item-total">
                        <label>Grand Total</label>
                        <div class="totals-value" id="cart-total"><%= GrandTotal.ToString("0.00") %></div>
                    </div>
                </div>
                <div class="button-container text-center">
                    <button type="button" class="checkout" onclick="checkout()">Checkout</button>
                    <button type="button" class="cancel" onclick="cancel()">Cancel</button>
                </div>
        </section>
    </form>
    <script src="js/cart.js"></script>
</body>
</html>