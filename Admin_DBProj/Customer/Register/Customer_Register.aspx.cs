using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin_DBProj.Customer.Register
{
    public partial class Customer_Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fname = txtFirstName.Text;
            string lname = txtLastName.Text;
            string address = txtAddress.Text;
            string email = txtEmail.Text;
            string contactNumber = txtContactNumber.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Check if the password meets the minimum length requirement
            if (password.Length < 6)
            {
                string script = "<script type=\"text/javascript\">alert('Password must be at least 6 characters long.');</script>";
                Response.Write(script);
                return;
            }

            if (password != confirmPassword)
            {
                string script = "<script type=\"text/javascript\">alert('Passwords do not match.');</script>";
                Response.Write(script);
                return;
            }

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

                    SqlParameter accIdParam = new SqlParameter("@ACC_ID", SqlDbType.Int);
                    accIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(accIdParam);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                        int accId = (int)accIdParam.Value;

                        // Call the AddAddressToSavedDelivery stored procedure
                        using (SqlCommand addAddressCommand = new SqlCommand("SP_INSERT_SAVEDADDRESS", connection))
                        {
                            addAddressCommand.CommandType = CommandType.StoredProcedure;
                            addAddressCommand.Parameters.AddWithValue("@AccId", accId);
                            addAddressCommand.ExecuteNonQuery();
                        }

                        // Clear the input fields after adding the customer
                        ClearInputBoxes();

                        string script = "<script type=\"text/javascript\">alert('Registration successful. Thank you for registering!');</script>";
                        Response.Write(script);
                    }
                    catch (Exception ex)
                    {
                        string error = "<script type=\"text/javascript\">alert('An error occurred: " + ex.Message + "');</script>";
                        Response.Write(error);
                    }
                }
            }
        }


        private void ClearInputBoxes()
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtContactNumber.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
        }
    }
}
