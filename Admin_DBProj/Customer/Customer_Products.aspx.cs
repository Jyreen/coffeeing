using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

namespace Admin_DBProj.Customer
{
    public partial class Customer_Products : System.Web.UI.Page
    {
        protected List<Product> Products { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Products = GetProductsFromDataBase();
            }
        }

        private List<Product> GetProductsFromDataBase()
        {
            List<Product> products = new List<Product>();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT * FROM PRODUCT";

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                PRODUCT_ID = Convert.ToInt32(reader["PRODUCT_ID_PK"]),
                                PRODUCT_NAME = reader["PRODUCT_NAME"].ToString(),
                                PRODUCT_DESC = reader["PRODUCT_DESC"].ToString(),
                                PRODUCT_PRICE = Convert.ToInt64(reader["PRODUCT_PRICE"]),
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

    public class Product
    {
        public int PRODUCT_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_DESC { get; set; }
        public decimal PRODUCT_PRICE { get; set; }
        public string PRODUCT_IMG { get; set; }
        public decimal PRODUCT_STATUS { get; set; }
    }
}