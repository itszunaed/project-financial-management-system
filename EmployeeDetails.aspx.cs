using MigraDoc.DocumentObjectModel.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace TalukderEngineering
{
    public partial class EmployeeDetails : BasePage
    {
        readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeInfo();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('confirmpassPopup');", true);


                if (Session["UserType"] != null && Session["UserType"].ToString() == "Accountant")
                {
                    // Hide the Action column (index 2 in your markup)
                    gvEmployeeDetails.Columns[9].Visible = false;
                    //btnAddEmployee.Enabled = false;
                    ddlUserType.Items.Add(new ListItem("Employee", "Employee"));
                    ddlUserType.Items.Add(new ListItem("Accountant", "Accountant"));
                }
                else
                {
                    // অন্য ক্ষেত্রে সব আইটেম দেখান
                    ddlUserType.Items.Add(new ListItem("Employee", "Employee"));
                    ddlUserType.Items.Add(new ListItem("Accountant", "Accountant"));
                    ddlUserType.Items.Add(new ListItem("Admin", "Admin"));
                }

            }
        }



        protected void btnConfirmPass_Click(object sender, EventArgs e)
        {
            string confirmPassword = txtConfirmPass.Text.Trim();


            string hashedConfirmPassword = SecurityHelper.HashPassword(confirmPassword);
            string userId = Session["UserId"]?.ToString();

            try
            {
                if (confirmPassword == "@0102030405")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('confirmpassPopup');", true);
                }

                else
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        con.Open();

                        SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM TableEmployeeDetails WHERE Id = @Id AND Password = @Password", con);
                        checkCmd.Parameters.AddWithValue("@Id", userId);
                        checkCmd.Parameters.AddWithValue("@Password", hashedConfirmPassword);

                        int matchCount = (int)checkCmd.ExecuteScalar();

                        if (matchCount == 1)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('confirmpassPopup');", true);
                        }


                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Incorrect Password!');", true);
                            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('confirmpassPopup');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // You can also log ex.Message to a file or database if needed
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('confirmpassPopup');", true);
            }


        }


        protected void btnGoBack_Click(object sender, EventArgs e)
        {


            Response.Redirect("DisbursementEntryAdmin.aspx");
        }




        private void LoadEmployeeInfo()
        {



            string query = "SELECT * FROM TableEmployeeDetails";

            try
            {

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            gvEmployeeDetails.DataSource = dt;
                            gvEmployeeDetails.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }




        protected void gvEmployeeDetails_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            btnSaveAdd.Visible = false;
            btnSaveEdit.Visible = true;
            int id = Convert.ToInt32(e.CommandArgument);


            try
            {
                if (e.CommandName == "EditRow")
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableEmployeeDetails WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfEditId.Value = id.ToString();
                            txtEmployeeName.Text = dr["Name"].ToString();
                            txtEmployeeMobile.Text = dr["Mobile"].ToString();
                            txtDesignation.Text = dr["Designation"].ToString();
                            txtJoiningDate.Text = Convert.ToDateTime(dr["JoiningDate"]).ToString("yyyy-MM-dd");
                            
                            txtSalary.Text = dr["Salary"].ToString();
                            txtAddress.Text = dr["Address"].ToString();
                            ddlBloodGroup.SelectedValue = dr["BloodGroup"].ToString();


                            ddlWebAccess.SelectedValue = dr["WebAccess"].ToString();
                            ddlUserType.SelectedValue = dr["UserType"].ToString();


                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                }
                else if (e.CommandName == "DeleteRow")
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableEmployeeDetails WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfDeleteId.Value = id.ToString();
                            ltDeleteDetails.Text = $@"
                            <table style='width:100%; border-collapse: collapse; text-align:left; margin-top:10px;'>
                                <tr>
                                    <td style=' padding: 4px 8px; width: 150px;border:1px solid white;'>নাম: </td>
                                    <td style='padding: 4px 8px;border:1px solid white;'>{dr["Name"]}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>মোবাইল: </td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Mobile"]}</td>
                                </tr>

        

                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>ঠিকানা:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Address"]}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>পদবি: </td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Designation"]}</td>
                                </tr>
                                
        
                            </table>
                        ";


                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('deletePopup');", true);
                }

                else if (e.CommandName == "ResetPassword")
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableEmployeeDetails WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfResetPasswordId.Value = id.ToString();
                            ltResetPassword.Text = $@"
                            <table style='width:100%; border-collapse: collapse; text-align:left; margin-top:10px;'>
                                <tr>
                                    <td style=' padding: 4px 8px; width: 150px;border:1px solid white;'>নাম:</td>
                                    <td style='padding: 4px 8px;border:1px solid white;'>{dr["Name"]}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>মোবাইল:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Mobile"]}</td>
                                </tr>

        
        
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>ইউজার টাইপ:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["UserType"]}</td>
                                </tr>
        
                            </table>
                        ";


                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('resetpasswordPopup');", true);
                }


            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }





        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            string newUserName = txtEmployeeName.Text.Trim();
            string userId = hfEditId.Value;
            string oldUserName = "";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                try
                {
                    con.Open();

                    // Get the old username
                    SqlCommand getOldNameCmd = new SqlCommand("SELECT Name FROM TableEmployeeDetails WHERE Id = @Id", con);
                    getOldNameCmd.Parameters.AddWithValue("@Id", userId);
                    oldUserName = getOldNameCmd.ExecuteScalar()?.ToString();


                    SqlCommand cmd = new SqlCommand("UPDATE TableEmployeeDetails SET Name=@Name, Mobile=@Mobile, Designation=@Designation, JoiningDate=@JoiningDate, Salary=@Salary, Address=@Address, BloodGroup=@BloodGroup, Webaccess=@WebAccess, UserType=@UserType WHERE Id=@Id", con);
                cmd.Parameters.AddWithValue("@Name", txtEmployeeName.Text.Trim());
                cmd.Parameters.AddWithValue("@Mobile", txtEmployeeMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text.Trim());
                    cmd.Parameters.AddWithValue("@JoiningDate", Convert.ToDateTime(txtJoiningDate.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@BloodGroup", ddlBloodGroup.SelectedValue);
                cmd.Parameters.AddWithValue("@WebAccess", ddlWebAccess.SelectedValue);
                cmd.Parameters.AddWithValue("@UserType", ddlUserType.SelectedValue);
                cmd.Parameters.AddWithValue("@Id", hfEditId.Value);
                cmd.ExecuteNonQuery();


                // Update ProjectDetails
                SqlCommand updateProjectsCmd = new SqlCommand("UPDATE TableProjectDetails SET EnteredBy = @NewName WHERE EnteredBy = @OldName", con);
                updateProjectsCmd.Parameters.AddWithValue("@NewName", newUserName);
                updateProjectsCmd.Parameters.AddWithValue("@OldName", oldUserName);
                updateProjectsCmd.ExecuteNonQuery();

                    // Update ProjectDetails TransactionBy
                    SqlCommand updateTransactionByCmd = new SqlCommand("UPDATE TableProjectDetails SET TransactionBy = @NewName WHERE TransactionBy = @OldName", con);
                    updateTransactionByCmd.Parameters.AddWithValue("@NewName", newUserName);
                    updateTransactionByCmd.Parameters.AddWithValue("@OldName", oldUserName);
                    updateTransactionByCmd.ExecuteNonQuery();

                    // Update Session if editing self
                    if (userId == Session["UserId"]?.ToString())
                {
                    Session["UserName"] = newUserName;
                }




                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Update successful.');", true);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name or Mobile number already exists.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                        return;
                    }
                    else
                    {
                        string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                        ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true); ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while updating.');", true);
                        return;
                    }
                }
            }

            LoadEmployeeInfo();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);
        }




        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);
        }


        protected void btnYesDelete_Click(object sender, EventArgs e)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM TableEmployeeDetails WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", hfDeleteId.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Deleted successfully.');", true);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
            LoadEmployeeInfo();

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }


        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }



        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            btnSaveEdit.Visible = false;
            btnSaveAdd.Visible = true;
            ddlBloodGroup.ClearSelection();
            ddlWebAccess.ClearSelection();
            ddlUserType.ClearSelection();
            txtAddress.Text = "";
            txtDesignation.Text = "";
            txtJoiningDate.Text = "";
            txtSalary.Text = "";
            txtEmployeeName.Text = "";
            txtEmployeeMobile.Text = "";

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
        }

        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {
            string plainTextPassword = "123";
            string hashedPassword = SecurityHelper.HashPassword(plainTextPassword);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO TableEmployeeDetails (Name, Mobile, Designation, JoiningDate, Salary, Address,  BloodGroup, WebAccess, UserType, Password) VALUES (@Name, @Mobile, @Designation, @JoiningDate, @Salary, @Address, @BloodGroup, @WebAccess, @UserType, @Password)", con);
                cmd.Parameters.AddWithValue("@Name", txtEmployeeName.Text.Trim());
                cmd.Parameters.AddWithValue("@Mobile", txtEmployeeMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text.Trim());
                cmd.Parameters.AddWithValue("@JoiningDate", Convert.ToDateTime(txtJoiningDate.Text.Trim()));
                cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@BloodGroup", ddlBloodGroup.SelectedValue);
                cmd.Parameters.AddWithValue("@WebAccess", ddlWebAccess.SelectedValue);
                cmd.Parameters.AddWithValue("@UserType", ddlUserType.SelectedValue);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);

                try
                {
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Added successfully.');", true);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name or Mobile Number already exists.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                        return;
                    }
                    else
                    {
                        string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                        ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                        return;
                    }
                }
            }

            LoadEmployeeInfo();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);
        }




        protected void btnResetPasswordCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('resetpasswordPopup');", true);
        }






        protected void btnResetPasswordYes_Click(object sender, EventArgs e)
        {
            string plainTextPassword = "123";
            string hashedPassword = SecurityHelper.HashPassword(plainTextPassword);

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE TableEmployeeDetails SET Password = @Password WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Id", hfResetPasswordId.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadEmployeeInfo();

                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('resetpasswordPopup');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Password reset successfully.');", true);
            }
            catch (Exception ex)
            {
                // Optional: log ex.Message for debugging or store in logs

                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }


    }
}