using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;


namespace Admin_DBProj
{
    public partial class products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();

                AddPanel.Visible = false;
                UpdateProductPanel.Visible = false;
            }
        }
        private void ClearInputBoxes()
        {
            txtName.Text = "";
            txtDesc.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string description = txtDesc.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string quantityText = txtQuantity.Text.Trim();

            int categoryId = 0;
            if (ddlCategory.SelectedItem != null)
            {
                string selectedCategory = ddlCategory.SelectedItem.Value;
                if (selectedCategory == "Donut")
                {
                    categoryId = 0; // Category 0 is for donuts
                }
                else if (selectedCategory == "Coffee")
                {
                    categoryId = 1; // Category 1 is for coffee
                }
            }

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_ADD_PRODUCT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Validate and convert the price
                    decimal priceValue;
                    if (decimal.TryParse(priceText, out priceValue))
                    {
                        if (priceValue < 0)
                        {
                            Response.Write("<script>alert('Price cannot be negative.');</script>");
                            return;
                        }
                        command.Parameters.AddWithValue("@PRODUCT_PRICE", priceValue);
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid price value.');</script>");
                        return;
                    }

                    // Validate and convert the quantity
                    int quantityValue;
                    if (int.TryParse(quantityText, out quantityValue))
                    {
                        if (quantityValue < 0)
                        {
                            Response.Write("<script>alert('Quantity cannot be negative.');</script>");
                            return;
                        }
                        command.Parameters.AddWithValue("@PRODUCT_QUANTITY", quantityValue);
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid quantity value.');</script>");
                        return;
                    }

                    command.Parameters.AddWithValue("@PRODUCT_NAME", name);
                    command.Parameters.AddWithValue("@PRODUCT_DESC", description);
                    command.Parameters.AddWithValue("@CATEGORY_ID", categoryId); // Pass the category ID

                    try
                    {
                        connection.Open();
                        int productId = Convert.ToInt32(command.ExecuteScalar());

                        BindGridView();
                        ClearInputBoxes();

                        Response.Write("<script>alert('Product added successfully.');</script>");
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('An error occurred: " + ex.Message + "');</script>");
                    }
                }
            }
        }


        protected void btnShowAddProduct_Click(object sender, EventArgs e)
        {
            AddPanel.Visible = !AddPanel.Visible;
            UpdateProductPanel.Visible = false;
        }
        protected void btnShowUpdateProduct_Click(object sender, EventArgs e)
        {
            UpdateProductPanel.Visible = !UpdateProductPanel.Visible;
            AddPanel.Visible = false;
        }
        protected void getName_TextChanged(object sender, EventArgs e)
        {
            string searchText = getName.Text.Trim();

   
            Response.Write($"You entered: {searchText}");
        }
        protected void ClearUpdateFields()
        {
            uName.Text = "";
            uDesc.Text = "";
            uPrice.Text = "";
            uQuantity.Text = "";
            uCategory.ClearSelection();
            uStatus.ClearSelection();
        }


        protected void getProductData_Click(object sender, EventArgs e)
        {
            string searchInput = getName.Text.Trim();
            if (string.IsNullOrEmpty(searchInput))
            {
                Response.Write("<script>alert('Input boxes cannot be empty.');</script>");
                return;
            }


            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GET_PDETAILS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SEARCHINPUT", searchInput);


                    try

                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            uName.Text = reader["PRODUCT_NAME"].ToString();
                            uDesc.Text = reader["PRODUCT_DESC"].ToString();
                            uPrice.Text = reader["PRODUCT_PRICE"].ToString();
                            uQuantity.Text = reader["PRODUCT_QUANTITY"].ToString();
                            uCategory.SelectedValue = reader["CATEGORY_ID"].ToString();

                            // Populate the status dropdown list
                            string productStatus = reader["PRODUCT_STATUS"].ToString();
                            if (productStatus == "Available")
                            {
                                uStatus.SelectedValue = "Available";
                            }
                            else if (productStatus == "Unavailable")
                            {
                                uStatus.SelectedValue = "Unavailable";
                            }
                            else
                            {
                                Response.Write("<script>alert('Product found.');</script>");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('An error occurred: " + ex.Message + "');</script>");

                    }
                }
            }
        }
        private void BindGridView()
        {
            prod.DataBind();
        }


        protected void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_UPDATE_PRODUCT", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PRODUCT_NAME", uName.Text.Trim());
                    command.Parameters.AddWithValue("@PRODUCT_DESC", uDesc.Text.Trim());

                    // Validate and convert the price
                    decimal priceValue;
                    if (decimal.TryParse(uPrice.Text.Trim(), out priceValue))
                    {
                        if (priceValue < 0)
                        {
                            Response.Write("<script>alert('Price cannot be negative.');</script>");
                            return;
                        }
                        command.Parameters.AddWithValue("@PRODUCT_PRICE", priceValue);
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid price value.');</script>");
                        return;
                    }

                    // Validate and convert the quantity
                    int quantityValue;
                    if (int.TryParse(uQuantity.Text.Trim(), out quantityValue))
                    {
                        if (quantityValue < 0)
                        {
                            Response.Write("<script>alert('Quantity cannot be negative.');</script>");
                            return;
                        }
                        command.Parameters.AddWithValue("@PRODUCT_QUANTITY", quantityValue);
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid quantity value.');</script>");
                        return;
                    }


                    int categoryValue;
                    if (uCategory.SelectedValue == "1" || uCategory.SelectedValue == "0")
                    {
                        categoryValue = int.Parse(uCategory.SelectedValue);
                        command.Parameters.AddWithValue("@PRODUCT_CATEGORY", categoryValue);
                    }
                    else
                    {
                        Response.Write("Invalid category value.");
                        return;
                    }

                    // Validate and convert the status value
                    int statusValue;
                    if (uStatus.SelectedValue == "1" || uStatus.SelectedValue == "0")
                    {
                        statusValue = int.Parse(uStatus.SelectedValue);
                        command.Parameters.AddWithValue("@PRODUCT_STATUS", statusValue);
                    }
                    else
                    {
                        Response.Write("Invalid status value.");
                        return;
                    }

                    try
                    {
                        con.Open();
                        command.ExecuteNonQuery();
                        string script = "<script type=\"text/javascript\">alert('Product updated successfully!');</script>";
                        Response.Write(script);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("An error occurred: " + ex.Message);
                    }
                }
            }
        }

    }
}
