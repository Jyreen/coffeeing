using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Admin_DBProj.Customer
{
    public partial class Customer_Homepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["AccountID"] != null)
                {
                    Login.Visible = false;
                }
                else
                {
                    Login.Visible = true;
                }
            }
        }

        private List<RecentOrder> GetRecentOrders(int accountID)
        {
            List<RecentOrder> recentOrders = new List<RecentOrder>();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SP_RECENTORDERS_USER", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AccountID", accountID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecentOrder order = new RecentOrder
                            {
                                ProductName = reader["PRODUCT_NAME"].ToString(),
                                ProductPrice = Convert.ToDecimal(reader["PRODUCT_PRICE"]),
                                OrderQuantity = Convert.ToInt32(reader["ORDER_DETAILS_QUANTITY"]),
                                OrderDate = Convert.ToDateTime(reader["ORDER_DATE"])
                            };
                            recentOrders.Add(order);
                        }
                    }
                }
            }

            return recentOrders;
        }

    }

    public class RecentOrder
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}