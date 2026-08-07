using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;

namespace TalukderEngineering
{
    public partial class EmployeeMoneyReturn : BasePage
    {
        readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
              


            }
        }

        private void LoadDropdowns()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();

                    SqlDataAdapter da1 = new SqlDataAdapter("SELECT AccountName FROM TableAccountList WHERE AccountantAccess = 'Yes' ORDER BY AccountName ASC", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    ddlAccountName.DataSource = dt1;
                    ddlAccountName.DataTextField = "AccountName";
                    ddlAccountName.DataValueField = "AccountName";
                    ddlAccountName.DataBind();
                    ddlAccountName.Items.Insert(0, new ListItem("-- None --", ""));

                    
                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                    ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                }
            }
        }


        protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlAccountName.SelectedValue;
            // Use the selected value here — it will be retained after postback
        }





        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {



            string accountName = ddlAccountName.SelectedValue;
            string enteredBy = Session["UserName"].ToString();
            

            string remarks = txtAddRemarks.Text.Trim();
            decimal amount = 0;
            decimal.TryParse(txtAddAmount.Text.Trim(), out amount);
            string type = "ফেরত";
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time"));//DateTime.Parse(txtDate.Text);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"INSERT INTO TableReturnedMoney 
                         (AccountName, Date, EnteredBy,  Remarks, Type, Amount)
                         VALUES (@AccountName, @Date, @EnteredBy, @Remarks, @Type, @Amount)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@AccountName", accountName);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@EnteredBy", enteredBy);

                    cmd.Parameters.AddWithValue("@Remarks", remarks.ToString() + " [" + Session["UserName"].ToString() +  " => " + accountName.ToString() + "]");
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@Amount", amount);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('এন্ট্রি সফল হয়েছে।');", true);
                        //ddlProjectName.ClearSelection();
                        //ddlExpenseCategory.ClearSelection();
                        txtAddRemarks.Text = "";
                        txtAddAmount.Text = "";
                        
                    }

                    catch (Exception ex)
                    {
                        string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                        ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }



        }




    }
}