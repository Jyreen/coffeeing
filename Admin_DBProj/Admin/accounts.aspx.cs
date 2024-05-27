using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Admin_DBProj
{
    public partial class accounts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();

                AddPanel.Visible = false;
                UpdatePanel.Visible = false;
            }
        }

        protected void btnShowAddCustomer_Click(object sender, EventArgs e)
        {
            AddPanel.Visible = !AddPanel.Visible;
            UpdatePanel.Visible = false;
        }

        protected void btnShowUpdateCustomer_Click(object sender, EventArgs e)
        {
            UpdatePanel.Visible = !UpdatePanel.Visible;
            AddPanel.Visible = false;   
        }

        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string fname = txtFirstName.Text;
            string lname = txtLastName.Text;
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            string contactNumber = txtContactNumber.Text;
            string password = txtPassword.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_CREATE_ACCOUNT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ACC_FNAME", fname);
                    command.Parameters.AddWithValue("@ACC_LNAME", lname);
                    command.Parameters.AddWithValue("@ACC_ADDRESS", address);
                    command.Parameters.AddWithValue("@ACC_EMAIL", email);
                    command.Parameters.AddWithValue("@ACC_PNUM", contactNumber);
                    command.Parameters.AddWithValue("@ACC_PASSWORD", password);

                    // Define the output parameter
                    SqlParameter outputIdParam = new SqlParameter("@ACC_ID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        // Retrieve the newly created ACC_ID
                        int newAccountId = (int)outputIdParam.Value;

                        // Refresh the GridView after adding the customer
                        BindGridView();
                        // Clear the input fields after adding the customer
                        ClearInputBoxes();

                        string script = $"<script type=\"text/javascript\">alert('User added successfully! New Account ID: {newAccountId}');</script>";
                        Response.Write(script);
                    }
                    catch (Exception ex)
                    {
                        string error = $"<script type=\"text/javascript\">alert('An error occurred: {ex.Message}');</script>";
                        Response.Write(error);
                    }
                }
            }
        }
        private void ClearInputBoxes()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            txtContactNumber.Text = "";
            txtPassword.Text = "";
        }

        private void BindGridView()
        {
            ctl02.DataBind();
        }

        protected void getCustomerData_Click1(object sender, EventArgs e)
        {
            string searchInput = getName.Text.Trim();
            if (string.IsNullOrEmpty(searchInput))
            {
                Response.Write("<script>alert('Input boxes cannot be empty.');</script>");
                return;
            }


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GET_ACCOUNT_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SEARCHINPUT", searchInput);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            uFirstName.Text = reader["FIRST NAME"].ToString();
                            uLastName.Text = reader["LAST NAME"].ToString();
                            uAddress.Text = reader["ADDRESS"].ToString();
                            uContactNumber.Text = reader["CONTACT NUMBER"].ToString();
                        }
                        else
                        {
                            string script = "<script type=\"text/javascript\">alert('User not found!');</script>";
                            Response.Write(script);
                        }
                    }
                }
            }
        }

        protected void btnUpdateCustomer_Click1(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_UPDATE_ACCOUNT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FIRSTNAME", uFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@LASTNAME", uLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ADDRESS", uAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@CONTACTNUMBER", uContactNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@STATUS", ddlStatus.SelectedValue);

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            string script = "<script type=\"text/javascript\">alert('User updated successfully!');</script>";
            Response.Write(script);

            BindGridView();
        }
    }
}

