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
    public partial class EmployeeHome : BasePage
    {
        readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadExpenseCategories();
            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadExpenseCategories();
                //txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");


            }



        }

        private void LoadDropdowns()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                try
                {
                    con.Open();

                    SqlDataAdapter da1 = new SqlDataAdapter("SELECT ProjectName FROM TableProjectList WHERE Status='Running' ORDER BY ProjectName ASC", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    ddlProjectName.DataSource = dt1;
                    ddlProjectName.DataTextField = "ProjectName";
                    ddlProjectName.DataValueField = "ProjectName";
                    ddlProjectName.DataBind();
                    ddlProjectName.Items.Insert(0, new ListItem("-- None --", ""));

                    /*SqlDataAdapter da2 = new SqlDataAdapter("SELECT ExpenseCategoryName FROM TableExpenseCategory", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    ddlExpenseCategory.DataSource = dt2;
                    ddlExpenseCategory.DataTextField = "ExpenseCategoryName";
                    ddlExpenseCategory.DataValueField = "ExpenseCategoryName";
                    ddlExpenseCategory.DataBind();
                    ddlExpenseCategory.Items.Insert(0, new ListItem("-- None --", ""));*/
                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                    ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                }
            }
        }


        private void LoadExpenseCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    string query = "SELECT ExpenseCategoryName FROM TableExpenseCategory";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<string> categories = new List<string>();

                    while (reader.Read())
                    {
                        string categoryName = reader["ExpenseCategoryName"].ToString();
                        categories.Add(categoryName);
                    }

                    reader.Close();

                    // Render the list to a hidden field or directly to JS
                    string jsArray = Newtonsoft.Json.JsonConvert.SerializeObject(categories);
                    ClientScript.RegisterStartupScript(this.GetType(), "categoriesArray", $"var categories = {jsArray};", true);
                }
            }
            catch (Exception ex)
            {
                 string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                        ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }





        protected void ddlProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlProjectName.SelectedValue;
            // Use the selected value here — it will be retained after postback
        }





        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {

           

            string projectName = ddlProjectName.SelectedValue;
            string enteredBy = Session["UserName"].ToString();
            string expenseCategory= txtExpenseCategory.Text.Trim();

            string remarks = txtAddRemarks.Text.Trim();
            decimal amount = 0;
            decimal.TryParse(txtAddAmount.Text.Trim(), out amount);
            string type = "খরচ";
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time"));//DateTime.Parse(txtDate.Text);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"INSERT INTO TableProjectDetails 
                         (ProjectName, Date, EnteredBy, ExpenseCategory, Remarks, Type, Amount)
                         VALUES (@ProjectName, @Date, @EnteredBy, @ExpenseCategory, @Remarks, @Type, @Amount)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProjectName", projectName);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@EnteredBy", enteredBy);
                    cmd.Parameters.AddWithValue("@ExpenseCategory", expenseCategory);
                    cmd.Parameters.AddWithValue("@Remarks", remarks);
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
                        txtExpenseCategory.Text = "";
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