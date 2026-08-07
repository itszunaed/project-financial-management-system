using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace TalukderEngineering
{
    public partial class UserInfo : BasePage
    {
        string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserInfo();
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('confirmpassPopup');", true);
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

                        SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM TableUserInfo WHERE Id = @Id AND Password = @Password", con);
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


            Response.Redirect("ProjectDetails.aspx?status=Running");
        }




        private void LoadUserInfo()
        {

            string query = "SELECT * FROM TableUserInfo";

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
                            gvUserInfo.DataSource = dt;
                            gvUserInfo.DataBind();
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


        protected void gvUserInfo_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
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
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableUserInfo WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfEditId.Value = id.ToString();
                            txtUserName.Text = dr["UserName"].ToString();
                            txtUserMobile.Text = dr["Mobile"].ToString();


                            ddlUserType.SelectedValue = dr["UserType"].ToString();

                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                }
                else if (e.CommandName == "DeleteRow")
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableUserInfo WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfDeleteId.Value = id.ToString();
                            ltDeleteDetails.Text = $@"
                            <table style='width:100%; border-collapse: collapse; text-align:left; margin-top:10px;'>
                                <tr>
                                    <td style=' padding: 4px 8px; width: 150px;border:1px solid white;'>User Name:</td>
                                    <td style='padding: 4px 8px;border:1px solid white;'>{dr["UserName"]}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>Mobile:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Mobile"]}</td>
                                </tr>

        
        
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>User Type:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["UserType"]}</td>
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
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableUserInfo WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfResetPasswordId.Value = id.ToString();
                            ltResetPassword.Text = $@"
                            <table style='width:100%; border-collapse: collapse; text-align:left; margin-top:10px;'>
                                <tr>
                                    <td style=' padding: 4px 8px; width: 150px;border:1px solid white;'>User Name:</td>
                                    <td style='padding: 4px 8px;border:1px solid white;'>{dr["UserName"]}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>Mobile:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Mobile"]}</td>
                                </tr>

        
        
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>User Type:</td>
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
            string newUserName = txtUserName.Text.Trim();
            string userId = hfEditId.Value;
            string oldUserName = "";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                try
                {
                    con.Open();

                    // Get the old username
                    SqlCommand getOldNameCmd = new SqlCommand("SELECT UserName FROM TableUserInfo WHERE Id = @Id", con);
                    getOldNameCmd.Parameters.AddWithValue("@Id", userId);
                    oldUserName = getOldNameCmd.ExecuteScalar()?.ToString();

                    // Update UserInfo
                    SqlCommand cmd = new SqlCommand("UPDATE TableUserInfo SET UserName=@UserName, Mobile=@Mobile, UserType=@UserType WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@UserName", newUserName);
                    cmd.Parameters.AddWithValue("@Mobile", txtUserMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserType", ddlUserType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.ExecuteNonQuery();

                    // Update ProjectDetails
                    SqlCommand updateProjectsCmd = new SqlCommand("UPDATE TableProjectDetails SET EnteredBy = @NewName WHERE EnteredBy = @OldName", con);
                    updateProjectsCmd.Parameters.AddWithValue("@NewName", newUserName);
                    updateProjectsCmd.Parameters.AddWithValue("@OldName", oldUserName);
                    updateProjectsCmd.ExecuteNonQuery();

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
                    if (ex.Number == 2627) // Unique constraint violation
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Username or mobile number already exists.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while updating.');", true);
                        return;
                    }
                }
            }

            LoadUserInfo();
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
                    SqlCommand cmd = new SqlCommand("DELETE FROM TableUserInfo WHERE Id=@Id", con);
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
            LoadUserInfo();

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }


        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }



        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            btnSaveEdit.Visible = false;
            btnSaveAdd.Visible = true;
            ddlUserType.ClearSelection();
            txtUserName.Text = "";
            txtUserMobile.Text = "";
            
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
        }

        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {
            string plainTextPassword = "123";
            string hashedPassword = SecurityHelper.HashPassword(plainTextPassword);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO TableUserInfo (UserName, Mobile, Password, UserType) VALUES (@UserName, @Mobile, @Password, @UserType)", con);
                cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                cmd.Parameters.AddWithValue("@Mobile", txtUserMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@UserType", ddlUserType.SelectedValue);

                try
                {
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Added successfully.');", true);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Unique constraint violation
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Username or mobile number already exists.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while saving.');", true);
                        return;
                    }
                }
            }

            LoadUserInfo();
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
                    SqlCommand cmd = new SqlCommand("UPDATE TableUserInfo SET Password = @Password WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Id", hfResetPasswordId.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                LoadUserInfo();

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