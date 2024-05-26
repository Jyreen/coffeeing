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

namespace Admin_DBProj.Customer.Login
{
    public partial class Customer_Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(email.Text) && !string.IsNullOrEmpty(password.Text))
            {
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    string query = "SELECT ACC_ID_PK, ACC_EMAIL, ACC_PASSWORD FROM ACCOUNT WHERE ACC_EMAIL = @EMAIL AND ACC_PASSWORD = @PASSWORD";
                    using (SqlCommand customerCmd = new SqlCommand(query, cn))
                    {
                        customerCmd.Parameters.AddWithValue("@EMAIL", email.Text);
                        customerCmd.Parameters.AddWithValue("@PASSWORD", password.Text);

                        SqlDataAdapter customerSda = new SqlDataAdapter(customerCmd);
                        DataTable customerDt = new DataTable();
                        customerSda.Fill(customerDt);

                        if (customerDt.Rows.Count > 0)
                        {
                            // Store AccountID in session
                            int accountId = Convert.ToInt32(customerDt.Rows[0]["ACC_ID_PK"]);
                            Session["AccountId"] = accountId;

                            Response.Redirect("~/Customer/Customer_Homepage.aspx");
                        }
                        else
                        {
                            Response.Write("<script>alert('Invalid email or password');</script>");
                        }
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Please enter email and password');</script>");
            }
        }
    }
}