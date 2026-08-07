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
    public partial class ProjectList : BasePage
    {
        readonly string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProjectList();

                
            }
        }


        private void LoadProjectList()
        {
            
            

            string query = "SELECT * FROM TableProjectList";

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
                            gvProjectList.DataSource = dt;
                            gvProjectList.DataBind();
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


        protected void gvProjectList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
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
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableProjectList WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfEditId.Value = id.ToString();
                            txtProjectName.Text = dr["ProjectName"].ToString();
                            txtProjectArea.Text = dr["Area"].ToString();

                            ddlProjectStatus.SelectedValue = dr["Status"].ToString();

                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                }
                else if (e.CommandName == "DeleteRow")
                {
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableProjectList WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfDeleteId.Value = id.ToString();
                            ltDeleteDetails.Text = $@"
                            <table style='width:100%; border-collapse: collapse; text-align:left; margin-top:10px;'>
                                <tr>
                                    <td style=' padding: 4px 8px; width: 150px;border:1px solid white;'>প্রোজেক্টের নাম:</td>
                                    <td style='padding: 4px 8px;border:1px solid white;'>{dr["ProjectName"]}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>অবস্থান:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Area"]}</td>
                                </tr>
        
                                <tr>
                                    <td style='padding: 4px 8px; border:1px solid white;'>স্ট্যাটাস:</td>
                                    <td style='padding: 4px 8px; border:1px solid white;'>{dr["Status"]}</td>
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
            string newProjectName = txtProjectName.Text.Trim();
            string rowId = hfEditId.Value;
            string oldProjectName = "";

            using (SqlConnection con = new SqlConnection(connStr))
                {
                try
                {
                    con.Open();

                    // Get the old name
                    SqlCommand getOldNameCmd = new SqlCommand("SELECT ProjectName FROM TableProjectList WHERE Id = @Id", con);
                    getOldNameCmd.Parameters.AddWithValue("@Id", rowId);
                    oldProjectName = getOldNameCmd.ExecuteScalar()?.ToString();

                    SqlCommand cmd = new SqlCommand("UPDATE TableProjectList SET ProjectName=@ProjectName, Area=@Area, Status=@Status WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@ProjectName", newProjectName);
                    cmd.Parameters.AddWithValue("@Area", txtProjectArea.Text);
                    cmd.Parameters.AddWithValue("@Status", ddlProjectStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Id", rowId);
                    cmd.ExecuteNonQuery();

                    // Update ProjectDetails
                    SqlCommand updateProjectsCmd = new SqlCommand("UPDATE TableProjectDetails SET ProjectName = @NewName WHERE ProjectName = @OldName", con);
                    updateProjectsCmd.Parameters.AddWithValue("@NewName", newProjectName);
                    updateProjectsCmd.Parameters.AddWithValue("@OldName", oldProjectName);
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
                LoadProjectList();

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
                    SqlCommand cmd = new SqlCommand("DELETE FROM TableProjectList WHERE Id=@Id", con);
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
            LoadProjectList();
            
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }




        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }

        protected void btnAddProjectList_Click(object sender, EventArgs e)
        {
            btnSaveEdit.Visible = false;
            btnSaveAdd.Visible = true;
            ddlProjectStatus.ClearSelection();
            txtProjectName.Text = "";
            txtProjectArea.Text = "";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
        }

        protected void btnSaveAdd_Click(object sender, EventArgs e) {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO TableProjectList (ProjectName, Area, Status) VALUES (@ProjectName, @Area, @Status)", con);
                cmd.Parameters.AddWithValue("@ProjectName", txtProjectName.Text);
                cmd.Parameters.AddWithValue("@Area", txtProjectArea.Text);
                cmd.Parameters.AddWithValue("@Status", ddlProjectStatus.SelectedValue);
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

            LoadProjectList();

            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);

        }







    }
}