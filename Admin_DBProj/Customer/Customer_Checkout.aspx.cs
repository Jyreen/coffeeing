using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Net;

namespace Admin_DBProj.Customer
{
    public partial class Customer_Checkout : System.Web.UI.Page
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
                CalculateTotals();
                BindOrderSummary();
                DisplayTotals();
            }
        }

        private void BindOrderSummary()
        {
            rptOrderSummary.DataSource = CartItems;
            rptOrderSummary.DataBind();
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

            // Load quantities from localStorage if session is empty
            if (CartItems.Count == 0 && Request.Cookies["cartData"] != null)
            {
                string cartDataJson = Request.Cookies["cartData"].Value;
                var cartData = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartDataJson);
                foreach (var item in cartData)
                {
                    CartItems.Add(item);
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

        private void DisplayTotals()
        {
            ltlSubtotal.Text = Subtotal.ToString("0.00");
            ltlTax.Text = Tax.ToString("0.00");
            ltlShipping.Text = Shipping.ToString("0.00");
            ltlGrandTotal.Text = GrandTotal.ToString("0.00");
        }

        protected void ConfirmOrder(object sender, EventArgs e)
        {
            // Retrieve account ID from session or authentication context
            if (Session["AccountID"] == null)
            {
                // Handle error: account ID not found
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                    "alert('Account ID not found. Please log in again.');window.location ='Login/Customer_Login.aspx';",
                true);

                return;
            }
            int accountID = (int)Session["AccountID"];

            // Retrieve payment ID from form input
            if (!int.TryParse(Request.Form["payment-method"], out int paymentID))
            {
                // Handle error: invalid payment method
                Response.Write("<script>alert('Invalid payment method selected.');</script>");
                return;
            }

            // Other necessary variables
            DateTime orderDate = DateTime.Now;
            if (!decimal.TryParse(ltlGrandTotal.Text.Replace("$", ""), out decimal orderTotal))
            {
                // Handle error: invalid order total
                Response.Write("<script>alert('Invalid order total.');</script>");
                return;
            }
            int orderStatusID = 1; // Default status
            int addressID = GetAddressID(accountID); // Replace with actual retrieval logic
            int? feedbackID = null; // Or set a default value if needed

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            List<CartItem> cartItems = Session["CartItems"] as List<CartItem>;
            if (cartItems == null || !cartItems.Any())
            {
                Response.Write("<script>alert('Your cart is empty.');</script>");
                return;
            }

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                // Begin a transaction
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                        // Insert into ORDER table
                        int newOrderID;
                        using (SqlCommand cmd = new SqlCommand("SP_InsertOrder", cn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                            cmd.Parameters.AddWithValue("@OrderTotal", orderTotal);
                            cmd.Parameters.AddWithValue("@OrderStatusID", orderStatusID);
                            cmd.Parameters.AddWithValue("@AddressID", addressID);
                            cmd.Parameters.AddWithValue("@AccountID", accountID);
                            cmd.Parameters.AddWithValue("@FeedbackID", feedbackID.HasValue ? (object)feedbackID.Value : DBNull.Value);
                            cmd.Parameters.AddWithValue("@PaymentID", paymentID);

                            SqlParameter outputIdParam = new SqlParameter("@NewOrderID", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            cmd.Parameters.Add(outputIdParam);
                            cmd.ExecuteNonQuery();

                            newOrderID = (int)outputIdParam.Value;
                        }

                        // Insert into ORDER_DETAILS table
                        foreach (var item in cartItems)
                        {
                            using (SqlCommand cmd = new SqlCommand("SP_InsertOrderDetails", cn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmd.Parameters.AddWithValue("@ProductPrice", item.ProductPrice);
                                cmd.Parameters.AddWithValue("@OrderID", newOrderID);
                                cmd.Parameters.AddWithValue("@OrderDetailsQuantity", item.Quantity);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Clear the cart after successful order
                        Session["CartItems"] = null;
                        cartItems.Clear();
                        Response.Cookies["cartData"].Expires = DateTime.Now.AddDays(-1);

                        // Redirect to homepage or any other page
                        Response.Redirect("Customer_Homepage.aspx");
                }
            }
        }

        private int GetAddressID(int accountID)
        {
            int addressID = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT SD_ADD_ID_PK FROM SAVED_DELIVERY_ADDRESS WHERE ACC_ID_FK = @AccountID";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@AccountID", accountID);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        addressID = Convert.ToInt32(result);
                    }
                }
            }

            return addressID;
        }


        protected void ExportCSV(object sender, EventArgs e, string tableName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand($"SELECT * FROM {tableName}", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);

                            string csv = string.Empty;

                            foreach (DataColumn column in dt.Columns)
                            {
                                csv += column.ColumnName + ',';
                            }

                            csv += "\r\n";

                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (DataColumn column in dt.Columns)
                                {
                                    csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                                }

                                csv += "\r\n";
                            }

                            Response.Clear();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", $"attachment;filename={tableName}Export.csv"); // Here we correctly concatenate/interpolate the tableName variable
                            Response.Charset = "";
                            Response.ContentType = "application/text";
                            Response.Output.Write(csv);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
        }
    }
}