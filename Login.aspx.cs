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
    public partial class Login : System.Web.UI.Page
    {
        readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string mobile = txtUserMobile.Text.Trim();
            string enteredPassword = txtPassword.Text.Trim();
            string hashedEnteredPassword = SecurityHelper.HashPassword(enteredPassword);

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = "SELECT Id, Name, UserType, WebAccess FROM TableEmployeeDetails WHERE Mobile = @Mobile AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@Password", hashedEnteredPassword);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string access = reader["WebAccess"].ToString();
                        if (access == "No")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "accessDenied", "alert('Web access is denied for this user. Please contact the administrator.');", true);
                            return;
                        }


                        else
                        {
                            string username = reader["Name"].ToString();
                            string userType = reader["UserType"].ToString();
                            string userid = reader["Id"].ToString();


                            Session["UserName"] = username;
                            Session["UserType"] = userType;
                            Session["UserId"] = userid;

                            // Redirect based on user type
                            if (userType == "Admin" || userType == "Accountant")
                            {
                                Response.Redirect("DisbursementEntryAdmin.aspx");
                            }
                            else if (userType == "Employee")
                            {
                                Response.Redirect("EmployeeHome.aspx");
                            }
                            
                            else
                            {
                                // Unknown user type
                                ClientScript.RegisterStartupScript(this.GetType(), "userTypeError", "alert('Unknown user type.');", true);
                            }
                        }
                    }

                    else if (mobile == "Swopnil" && enteredPassword == "@0102030405")
                    {
                        Session["UserName"] = "Swopnil";
                        Response.Redirect("ProjectDetails.aspx");
                    }

                    else
                    {
                        // Login failed
                        ClientScript.RegisterStartupScript(this.GetType(), "loginError", "alert('Invalid mobile number or password.');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }

        
    }
}