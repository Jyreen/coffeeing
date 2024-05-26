    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="Admin_DBProj.home" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Admin Dashboard - Coffee Shop</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" integrity="sha512-A/n8r9iKqroIiJsCxpx88lRYabNt7emRyVzR2T+1GnL/LbBCFLhIOBkye5ofd40wYbIB+DxXp1Bcy2r34W0zZA==" crossorigin="anonymous" referrerpolicy="no-referrer" />
<style>
    body {
        font-family: Arial, sans-serif;
        background-color: #F8F5EC;
        margin: 0;
        padding: 0;
    }
    .logout {
        text-align: center;
        margin-top: 80px;
    }
    .container {
        display: flex;
        height: 100vh;
    }
    .sidebar {
        width: 250px;
        background-color: #C2A375;
        color: #fff;
        padding: 20px;
        box-sizing: border-box;
    }
    .sidebar ul {
        list-style: none;
        padding: 0;
        margin: 0;
    }
    .sidebar ul li {
        margin-bottom: 15px;
    }
    .sidebar ul li a {
        color: #fff;
        text-decoration: none;
        display: flex;
        align-items: center;
    }
    .sidebar ul li a img {
        width: 30px;
        height: 30px; 
        margin-right: 10px;
    }
    .content {
        flex: 1;
        padding: 20px;
    }
    .header {
        background-color: #F8F5EC;
        color: black;
        padding: 20px;
        margin-bottom: 10px;
    }
    h1 {
        margin: 0;
        font-weight: bold;
    }
    .report {
        padding: 5px;
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap; 
    }
    .report-box {
        text-align: center;
        padding: 20px;
        border-radius: 10px;
        background-color: #b37333;
        color: white;
        width: 150px;
        height: 150px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        margin-bottom: 10px;
    }
    .report-box h3 {
        margin-top: 0;
        font-size: 18px;
    }
    .report-box p {
        margin: 0;
        font-size: 24px;
    }
    .logout a img {
        width: 25px; 
        height: 25px; 
    }
    .order-section {
        background-color: #fff;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        border-radius: 5px;
        padding: 20px;
        width: 100%;
        margin-top: 20px; 
    }
    .report-box-button {
        background-color: #b37333;
        color: white; 
        border: none; 
        padding: 10px 20px; 
        border-radius: 5px; 
        cursor: pointer; 
        transition: background-color 0.3s; 
        margin-top: 10px;
    }
    .report-box-button:hover {
        background-color: #a2622c;
    }
    .gridview-container {
        margin-top: 20px;
    }
    .gridview-container h2 {
        margin-top: 0;
        margin-bottom: 20px;
    }
    .styled-gridview {
        width: 100%;
        border-collapse: collapse;
    }
    .styled-gridview th, .styled-gridview td {
        padding: 10px;
        border: 1px solid #ddd;
        text-align: left;
    }
    .styled-gridview th {
        background-color: #b37333;
        color: white;
    }
    .styled-gridview tr:nth-child(even) {
        background-color: #f9f9f9;
    }
    .styled-gridview tr:hover {
        background-color: #f1f1f1;
    }
</style>
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="sidebar">
                <ul>
                    <li><a href="home.aspx"><img src="../Images/icons8-home-64.png" /> <span style="font-weight: bold;">Home</span></a></li>
                    <li><a href="orders.aspx"><img src="../Images/icons8-cart-48.png" /> <span style="font-weight: bold;">Orders</span></a></li>
                    <li><a href="accounts.aspx"><img src="../Images/icons8-person-64.png"/> <span style="font-weight: bold;">Customers</span></a></li>
                    <li><a href="products.aspx"><img src="../Images/icons8-cardboard-box-50.png" /> <span style="font-weight: bold;">Products</span></a></li>
                    <li><a href="analytics.aspx"><img src="../Images/icons8-analytics-60.png" /> <span style="font-weight: bold;">Reports</span></a></li>
                </ul>
                <div class="logout">
                    <a href="admin.aspx"><img src="../Images/icons8-logout-52.png" /></a>
                </div>
            </div>
            <div class="content">
                <div class="header">
                    <h1>COFFEEING</h1>
                </div>
                <div class="report">
                    <div class="report-box">
                        <h3>Total Customers</h3>
                        <asp:Label ID="totalCustomers" runat="server" Text=" "></asp:Label>
                        <asp:Button ID="Button1" runat="server" CssClass="report-box-button" Text="Export to CSV" OnClick="Button1_Click" />
                    </div>
                    <div class="report-box">
                        <h3>Total Products</h3>
                        <asp:Label ID="totalProducts" runat="server" Text=" "></asp:Label>
                        <asp:Button ID="Button2" runat="server" CssClass="report-box-button" Text="Export to CSV"  OnClick="Button2_Click" />
                    </div>
                    <div class="report-box">
                        <h3>Total Orders</h3>
                        <asp:Label ID="totalOrders" runat="server" Text=" "></asp:Label>
                        <asp:Button ID="Button3" runat="server" CssClass="report-box-button" Text="Export to CSV" OnClick="Button3_Click" />
                    </div>
                </div>
            </div>
            <div class="order-section gridview-container">
                <h2>Recent Orders</h2>
                <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="ORDER_ID_PK" CssClass="styled-gridview">
                    <Columns>
                        <asp:BoundField DataField="ORDER_ID_PK" HeaderText="Order ID" InsertVisible="False" ReadOnly="True" SortExpression="ORDER_ID_PK" />
                        <asp:BoundField DataField="ORDER_DATE" HeaderText="Order Date" SortExpression="ORDER_DATE" />
                        <asp:BoundField DataField="ORDER_TOTAL" HeaderText="Order Total" SortExpression="ORDER_TOTAL" />
                        <asp:BoundField DataField="ORDER_STATUS_ID_FK" HeaderText="Order Status" SortExpression="ORDER_STATUS_ID_FK" />
                        <asp:BoundField DataField="SD_ADD_ID_FK" HeaderText="Shipping Address" SortExpression="SD_ADD_ID_FK" />
                        <asp:BoundField DataField="ACC_ID_FK" HeaderText="Customer ID" SortExpression="ACC_ID_FK" />
                        <asp:BoundField DataField="PAY_ID" HeaderText="Payment ID" SortExpression="PAY_ID" />
                        <asp:BoundField DataField="FBACK_ID_FK" HeaderText="Feedback ID" SortExpression="FBACK_ID_FK" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SP_RECENTORDERS_ADMIN" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </div>
        </div>
    </form>
</body>
</html>

