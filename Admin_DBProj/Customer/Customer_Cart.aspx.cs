using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace Admin_DBProj.Customer
{
    public partial class Customer_Cart : System.Web.UI.Page
    {
        public List<CartItem> CartItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal GrandTotal { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCartItems();

                if (Request.QueryString["productId"] != null && Request.QueryString["productName"] != null && Request.QueryString["quantity"] != null)
                {
                    int productId;
                    if (int.TryParse(Request.QueryString["productId"], out productId))
                    {
                        string productName = Request.QueryString["productName"];
                        int quantity;
                        if (int.TryParse(Request.QueryString["quantity"], out quantity))
                        {
                            AddToCart(productId, productName, quantity);
                        }
                    }
                }

                CalculateTotals();
            }
            else
            {
                // Process the hidden field data during postback
                string cartDataJson = cartDataHiddenField.Value;
                if (!string.IsNullOrEmpty(cartDataJson))
                {
                    var cartData = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartDataJson);
                    foreach (var item in cartData)
                    {
                        var cartItem = CartItems.FirstOrDefault(ci => ci.ProductID == item.ProductID);
                        if (cartItem != null)
                        {
                            cartItem.Quantity = item.Quantity;
                        }
                    }
                    Session["CartItems"] = CartItems;
                }
            }
        }

        private void AddToCart(int productId, string productName, int quantity)
        {
            Product product = GetProductsFromDataBase().FirstOrDefault(p => p.PRODUCT_ID == productId);
            if (product != null)
            {
                CartItem existingCartItem = CartItems.FirstOrDefault(ci => ci.ProductID == productId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    CartItem cartItem = new CartItem
                    {
                        ProductID = product.PRODUCT_ID,
                        ProductName = product.PRODUCT_NAME,
                        Quantity = quantity,
                        ProductDesc = product.PRODUCT_DESC,
                        ProductPrice = product.PRODUCT_PRICE,
                        ProductImg = product.PRODUCT_IMG
                    };
                    CartItems.Add(cartItem);
                }

                Session["CartItems"] = CartItems;
            }
        }

        private void LoadCartItems()
        {
            if (Session["CartItems"] != null)
            {
                CartItems = (List<CartItem>)Session["CartItems"];
            }
            else
            {
                CartItems = new List<CartItem>();
            }

            // Load quantities from localStorage
            if (Request.Cookies["cartData"] != null)
            {
                string cartDataJson = Request.Cookies["cartData"].Value;
                var cartData = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartDataJson);
                foreach (var item in cartData)
                {
                    var cartItem = CartItems.FirstOrDefault(ci => ci.ProductID == item.ProductID);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = item.Quantity;
                    }
                }
            }
        }

        private void CalculateTotals()
        {
            Subtotal = CartItems.Sum(item => item.ProductPrice * item.Quantity);
            Tax = Subtotal * 0.05m;
            Shipping = 15.00m;
            GrandTotal = Subtotal + Tax + Shipping;
        }

        private List<Product> GetProductsFromDataBase()
        {
            List<Product> products = new List<Product>();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT * FROM PRODUCT";

            using (System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, cn))
                {
                    cn.Open();
                    using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                PRODUCT_ID = Convert.ToInt32(reader["PRODUCT_ID_PK"]),
                                PRODUCT_NAME = reader["PRODUCT_NAME"].ToString(),
                                PRODUCT_DESC = reader["PRODUCT_DESC"].ToString(),
                                PRODUCT_PRICE = Convert.ToDecimal(reader["PRODUCT_PRICE"]),
                                PRODUCT_IMG = reader["PRODUCT_IMG"].ToString(),
                                PRODUCT_STATUS = Convert.ToInt32(reader["PRODUCT_STATUS"])
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
    }

    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
    }
}