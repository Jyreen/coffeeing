﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customer_Products.aspx.cs" Inherits="Admin_DBProj.Customer.Customer_Products" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>COFFEEING - Products</title>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/css/bootstrap.min.css'>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css'>
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i"
        rel="stylesheet">
    <link type="text/css" rel="stylesheet" href="~/Customer/css/prod.css">
</head>
<body>
    <form id="form1" runat="server">
    <header class="auto-style1"  CssClass="center" Height="100px" Width="100px">
        <div class="logo">
            <a href="Customer_Homepage.aspx"><span>COFFEING</span></a>
        </div>
        <nav>
            <a href="#"><i class="fas fa-search"></i></a> 
            <a href="Customer_Homepage.aspx">Home</a>
            <a href="Customer_Profile.aspx">Profile</a> 
            <a href="Customer_products.aspx">Shop</a> 
            <a href="Login/Customer_Login.aspx" id="Login" runat="server">Login</a>
            <a href="Customer_Cart.aspx"><i class="fas fa-shopping-cart"></i>Cart</a>
        </nav>
    </header>

    <!-- Sidebar -->
        <div class="sidebar">
        <input type="text" id="searchInput" placeholder="Search..." />
        <div>
        <ul>
            <li><a href="#">Coffee</a></li>
            <li><a href="#">Donuts</a></li>
            <!-- Add more categories as needed -->
        </ul>
            </div>
    </div>



    <!-- Product Section -->
    <section class="section-products product-section">
        <div class="container" >
            <div class="row justify-content-center text-center">
                <div class="col-md-8 col-lg-6">
                    <div class="header">
                        <h2>Popular Products</h2>
                    </div>
                </div>
            </div>
            <div class="menu-grid">
                <% foreach (var product in Products) { %>
                    <% if (product.PRODUCT_STATUS == 1) { %> 
                        <div class="menu-item">
                            <img class="center" src="<%= product.PRODUCT_IMG %>" />
                            <div class="menu-item-details">
                                <div class="menu-item-name"><%= product.PRODUCT_NAME %></div>
                                <div class="menu-item-description"><%= product.PRODUCT_DESC %></div>
                                <div class="menu-item-price">$ <%= product.PRODUCT_PRICE %></div>
                            </div>
                            <div class="quantity-and-cart">
                                <input type="number" class="quantity-input" value="1" min="1" id="quantity_<%= product.PRODUCT_ID %>">
                                <a class="add-to-cart-button" href="#" onclick="addToCart(<%= product.PRODUCT_ID %>, '<%= product.PRODUCT_NAME %>'); return false;">Add to Cart</a>
                            </div>
                        </div>
                    <% } %>
                <% } %>
            </div>
    </section>
    </form>
    <script>
        function addToCart(productId, productName) {
            const quantity = document.getElementById(`quantity_${productId}`).value;
            window.location.href = `Customer_Cart.aspx?productId=${productId}&productName=${productName}&quantity=${quantity}`;
        }
    </script>
</body>
</html>