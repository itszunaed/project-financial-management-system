using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TalukderEngineering
{
    public partial class ExpenseCategory : BasePage
    {
        readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategoryList();
            }
        }

        private void LoadCategoryList()
        {



            string query = "SELECT * FROM TableExpenseCategory";

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
                            gvExpenseCategory.DataSource = dt;
                            gvExpenseCategory.DataBind();
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





        protected void gvExpenseCategory_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
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
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableExpenseCategory WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfEditId.Value = id.ToString();
                            txtCategoryName.Text = dr["ExpenseCategoryName"].ToString();


                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                }
                else if (e.CommandName == "DeleteRow")
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableExpenseCategory WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfDeleteId.Value = id.ToString();
                            ltDeleteDetails.Text = $@"
    <table style='width:100%; border-collapse: collapse; text-align:left; margin-top:10px;'>
        <tr>
            <td style=' padding: 4px 8px; width: 150px;border:1px solid white;'>খাতের নাম:</td>
            <td style='padding: 4px 8px;border:1px solid white;'>{dr["ExpenseCategoryName"]}</td>
        </tr>
        
    </table>
";


                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('deletePopup');", true);
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
            string newCategoryName = txtCategoryName.Text.Trim();
            string rowId = hfEditId.Value;
            string oldCategoryName = "";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                try
                {
                    con.Open();

                    // Get the old name
                    SqlCommand getOldNameCmd = new SqlCommand("SELECT ExpenseCategoryName FROM TableExpenseCategory WHERE Id = @Id", con);
                    getOldNameCmd.Parameters.AddWithValue("@Id", rowId);
                    oldCategoryName = getOldNameCmd.ExecuteScalar()?.ToString();

                    SqlCommand cmd = new SqlCommand("UPDATE TableExpenseCategory SET ExpenseCategoryName=@CategoryName WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                    cmd.Parameters.AddWithValue("@Id", rowId);
                    cmd.ExecuteNonQuery();


                    // Update ProjectDetails
                    SqlCommand updateProjectsCmd = new SqlCommand("UPDATE TableProjectDetails SET ExpenseCategory = @NewName WHERE ExpenseCategory = @OldName", con);
                    updateProjectsCmd.Parameters.AddWithValue("@NewName", newCategoryName);
                    updateProjectsCmd.Parameters.AddWithValue("@OldName", oldCategoryName);
                    updateProjectsCmd.ExecuteNonQuery();


                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Update successful.');", true);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Unique constraint violation
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name already exists.');", true);
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
            LoadCategoryList();

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
                    SqlCommand cmd = new SqlCommand("DELETE FROM TableExpenseCategory WHERE Id=@Id", con);
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
            LoadCategoryList();

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }




        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }





        protected void btnAddExpenseCategory_Click(object sender, EventArgs e)
        {
            btnSaveEdit.Visible = false;
            btnSaveAdd.Visible = true;
            
            txtCategoryName.Text = "";
            
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
        }





        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TableExpenseCategory (ExpenseCategoryName) VALUES (@CategoryName)", con);
                cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                
                con.Open();
                
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
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name already exists.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                        return;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while adding.');", true);
                        return;
                    }
                }
            }

            LoadCategoryList();

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);

        }








    }
}