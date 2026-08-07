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
    public partial class EmployeePasswordChange : BasePage
    {
        readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnSavePass_Click(object sender, EventArgs e)
        {
            string currentPassword = txtCurrentPass.Text.Trim();
            string newPassword = txtRetypeNewPass.Text.Trim();

            string hashedCurrentPassword = SecurityHelper.HashPassword(currentPassword);
            string hashedNewPassword = SecurityHelper.HashPassword(newPassword);
            string userId = Session["UserId"]?.ToString();

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM TableEmployeeDetails WHERE Id = @Id AND Password = @Password", con);
                    checkCmd.Parameters.AddWithValue("@Id", userId);
                    checkCmd.Parameters.AddWithValue("@Password", hashedCurrentPassword);

                    int matchCount = (int)checkCmd.ExecuteScalar();

                    if (matchCount == 1)
                    {
                        SqlCommand updateCmd = new SqlCommand("UPDATE TableEmployeeDetails SET Password = @NewPassword WHERE Id = @Id", con);
                        updateCmd.Parameters.AddWithValue("@NewPassword", hashedNewPassword);
                        updateCmd.Parameters.AddWithValue("@Id", userId);
                        updateCmd.ExecuteNonQuery();

                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('পাসওয়ার্ড পরিবর্তন সফল হয়েছে।');
                    document.getElementById('{txtCurrentPass.ClientID}').value = '';
                    document.getElementById('{txtNewPass.ClientID}').value = '';
                    document.getElementById('{txtRetypeNewPass.ClientID}').value = '';
                ", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('বর্তমান পাসওয়ার্ড সঠিক নয়!');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                // You can also log ex.Message to a file or database if needed
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }
    }
}