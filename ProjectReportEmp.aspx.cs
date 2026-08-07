using System;
using System.IO;
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
    public partial class ProjectReportEmp : BasePage
    {
        readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {


            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            

            if (!IsPostBack)
            {
                string status = Request.QueryString["status"] ?? "Running"; // default to "Running"

                LoadProjectNames(status);
                pnlFilteredExpense.Visible = false;
               


                
            }

        }

        private void LoadProjectNames(string status)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    string query = "SELECT ProjectName FROM TableProjectList WHERE Status = @Status";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", status);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        ddlProject.DataSource = reader;
                        ddlProject.DataTextField = "ProjectName";
                        ddlProject.DataValueField = "ProjectName"; // or "ProjectID" if available
                        ddlProject.DataBind();

                        ddlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));

                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }








        private void LoadProjectDetails()
        {
            string selectedProject = ddlProject.SelectedItem.Text;


            string query = "SELECT Id, ProjectName, [Date],    EnteredBy,   Remarks,   Type,    ExpenseCategory,   Amount FROM TableProjectDetails WHERE ProjectName = @projectName AND Type = N'খরচ' ORDER BY [Date] ASC;";

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@projectName", selectedProject); // add parameter here

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            gvProjectDetails.DataSource = dt;
                            gvProjectDetails.DataBind();
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








      









        

        protected void btnSearch_Click(object sender, EventArgs e)
        {



            if (!string.IsNullOrEmpty(ddlProject.SelectedValue))
            {

                pnlFilteredExpense.Visible = false;

                string query = @"SELECT Id, ProjectName, [Date], EnteredBy, Remarks, Type, ExpenseCategory, Amount 
                     FROM TableProjectDetails 
                     WHERE ProjectName = @projectName AND Type = N'খরচ'";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@projectName", ddlProject.SelectedValue));

                // Start Date Filter
                if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    query += " AND [Date] >= @startDate";
                    parameters.Add(new SqlParameter("@startDate", Convert.ToDateTime(txtStartDate.Text)));
                }

                // End Date Filter
                if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    query += " AND [Date] < DATEADD(DAY, 1, @endDate)";
                    parameters.Add(new SqlParameter("@endDate", Convert.ToDateTime(txtEndDate.Text)));
                }

                // Entered By Filter
                if (!string.IsNullOrWhiteSpace(ddlEntryPersonInDate.SelectedValue))
                {
                    query += " AND EnteredBy = @entryPerson";
                    parameters.Add(new SqlParameter("@entryPerson", ddlEntryPersonInDate.SelectedValue));
                }

                // Expense Category Filter
                bool isCategorySelected = !string.IsNullOrWhiteSpace(ddlCategoryInDate.SelectedValue);
                if (isCategorySelected)
                {
                    query += " AND ExpenseCategory = @category";
                    parameters.Add(new SqlParameter("@category", ddlCategoryInDate.SelectedValue));
                }

                // Final ORDER BY
                query += " ORDER BY [Date] ASC";

                try
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddRange(parameters.ToArray());

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvProjectDetails.DataSource = dt;
                        gvProjectDetails.DataBind();

                        // ======== CALCULATE SUMMARY ========
                        //decimal totalPradan = 0;
                        decimal totalKhroch = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["Type"] != DBNull.Value && row["Amount"] != DBNull.Value)
                            {
                                string t = row["Type"].ToString();
                                decimal amt = Convert.ToDecimal(row["Amount"]);

                                //if (t == "প্রদান") totalPradan += amt;
                                if (t == "খরচ") totalKhroch += amt;
                            }
                        }

                        lblFilteredExpense.Text = $"মোট খরচ: {totalKhroch:N0} ৳";

                        /* If category is selected → show ONLY total খরচ
                    if (isCategorySelected)
                    {
                        
                    }
                    else
                    {
                        decimal balance = totalPradan - totalKhroch;

                            lblFilteredExpense.Text =
                                $"মোট প্রদান: {totalPradan:N0} ৳ &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp; " +
                                $"মোট খরচ: {totalKhroch:N0} ৳ &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp; " +
                                $"অবশিষ্ট: {balance:N0} ৳";
                        }*/

                        pnlFilteredExpense.Visible = true;
                    }

                    hfPDFAppliedFilter.Value = GetAppliedFilterText();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.Replace("'", "").Replace("\n", "").Replace("\r", "");
                    ClientScript.RegisterStartupScript(this.GetType(), "err", $"alert('Error: {msg}');", true);
                }



            }

            ScriptManager.RegisterStartupScript(this, GetType(), "scroll", "scrollToBottom();", true);
        }







        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {

            /* pnlDateFilter.Visible = false;
             pnlEntryPersonFilter.Visible = false;
             pnlCategoryFilter.Visible = false;
             pnlFilteredExpense.Visible = false;
            ddlSearchBy.ClearSelection();
            txtAddProjectName.Text = ddlProject.SelectedValue;*/
            hfPDFProjectName.Value = ddlProject.SelectedItem.Text;

            hfPDFAppliedFilter.Value = "N/A";
            pnlFilteredExpense.Visible = false;

            
            LoadProjectDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "scroll", "scrollToBottom();", true);
            LoadFilterDropdowns();
        }








        protected void btnClear_Click(object sender, EventArgs e)
        {
            /*pnlDateFilter.Visible = false;
            pnlEntryPersonFilter.Visible = false;
            pnlCategoryFilter.Visible = false;
            pnlFilteredExpense.Visible = false;
            ddlSearchBy.ClearSelection();*/
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            hfPDFAppliedFilter.Value = "N/A";
            ddlCategoryInDate.ClearSelection();
            ddlEntryPersonInDate.ClearSelection();
            pnlFilteredExpense.Visible = false;
            LoadProjectDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "scroll", "scrollToBottom();", true);
        }








       

        private void LoadFilterDropdowns()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();



                    // Get selected project
                    string selectedProject = ddlProject.SelectedItem?.Text ?? "";

                    // Load EnteredBy dropdown filtered by selected project
                    SqlCommand cmdEnteredBy = new SqlCommand(
                        "SELECT DISTINCT EnteredBy FROM TableProjectDetails WHERE ProjectName = @project", con);
                    cmdEnteredBy.Parameters.AddWithValue("@project", selectedProject);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmdEnteredBy);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);

                    ddlEntryPersonInDate.DataSource = dt1;
                    ddlEntryPersonInDate.DataTextField = "EnteredBy";
                    ddlEntryPersonInDate.DataValueField = "EnteredBy";
                    ddlEntryPersonInDate.DataBind();
                    ddlEntryPersonInDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));

                    // Load ExpenseCategory dropdown filtered by selected project
                    SqlCommand cmdCategory = new SqlCommand(
                         "SELECT DISTINCT ExpenseCategory FROM TableProjectDetails WHERE ProjectName = @project AND ExpenseCategory IS NOT NULL AND ExpenseCategory <> ''", con);
                    cmdCategory.Parameters.AddWithValue("@project", selectedProject);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmdCategory);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);

                    ddlCategoryInDate.DataSource = dt2;
                    ddlCategoryInDate.DataTextField = "ExpenseCategory";
                    ddlCategoryInDate.DataValueField = "ExpenseCategory";
                    ddlCategoryInDate.DataBind();
                    ddlCategoryInDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }


        





      





       








        protected string GetAppliedFilterText()
        {
            string filter = "";

            /*filter += "Search By: " + ddlSearchBy.SelectedItem.Text;*/


            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                DateTime startDate = DateTime.Parse(txtStartDate.Text);


                filter += " " + startDate.ToString("dd/MM/yyyy") + " হতে ";



            }

            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {

                DateTime endDate = DateTime.Parse(txtEndDate.Text);

                filter += endDate.ToString("dd/MM/yyyy") + " পর্যন্ত ";



            }

            if (!string.IsNullOrEmpty(ddlEntryPersonInDate.SelectedValue))
            {
                filter += " ব্যক্তি: " + ddlEntryPersonInDate.SelectedItem.Text;


            }



            if (!string.IsNullOrEmpty(ddlCategoryInDate.SelectedValue))
            {
                filter += " খরচের খাত: " + ddlCategoryInDate.SelectedItem.Text;
            }




            return string.IsNullOrEmpty(filter) ? "N/A" : filter.TrimStart(',');
        }








    }
}