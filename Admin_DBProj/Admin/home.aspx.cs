using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin_DBProj
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetReportValues();
            }
        }

        protected void SetReportValues()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Count for Customers
                string customerQuery = "SELECT COUNT(*) FROM ACCOUNT";
                int customerCount = 0;
                using (SqlCommand command = new SqlCommand(customerQuery, connection))
                {
                    connection.Open();
                    customerCount = (int)command.ExecuteScalar();
                }
                totalCustomers.Text = customerCount.ToString();

                // Count for Products
                string productQuery = "SELECT COUNT(*) FROM PRODUCT";
                int productCount = 0;
                using (SqlCommand command = new SqlCommand(productQuery, connection))
                {
                    productCount = (int)command.ExecuteScalar();
                }
                totalProducts.Text = productCount.ToString();

                // Count for Orders
                string orderQuery = "SELECT COUNT(*) FROM [ORDER]";
                int orderCount = 0;
                using (SqlCommand command = new SqlCommand(orderQuery, connection))
                {
                    orderCount = (int)command.ExecuteScalar();
                }
                totalOrders.Text = orderCount.ToString();
            }
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            ExportCSV(sender, e, "PRODUCT");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ExportCSV(sender, e, "ACCOUNT");
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            ExportCSV(sender, e, "[ORDER]");
        }
    }
}