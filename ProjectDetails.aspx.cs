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
    public partial class Dashboard : BasePage
    {
        readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            LoadExpenseCategories();

            if (!IsPostBack)
            {
                string status = Request.QueryString["status"] ?? "Running"; // default to "Running"
                
                LoadProjectNames(status);
                pnlFilteredExpense.Visible = false;
                LoadEditDropdowns();


                if (Session["UserType"] != null && Session["UserType"].ToString() == "Accountant")
                {
                    // Hide the Action column (index 2 in your markup)
                    gvProjectDetails.Columns[5].Visible = false;
                }


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








        private void LoadProjectSummary()
        {
            if (string.IsNullOrEmpty(ddlProject.SelectedValue))
            {

                lblProjectName.Text = "প্রোজেক্টের নাম:";
                //lblTotalFunding.Text = "মোট প্রদান:";
                lblTotalSpent.Text = "মোট খরচ:";
                //lblAvailableAmount.Text = "অবশিষ্ট:";

            }

            else
            {
                
                string selectedProject = ddlProject.SelectedItem.Text;

                lblProjectName.Text = "প্রোজেক্টের নাম:" + "\u00A0\u00A0" + selectedProject;

                //long totalFunding;
                long totalSpent;

                try
                {
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();

                        /* Total Funding (Deposit)
                        SqlCommand cmdFunding = new SqlCommand("SELECT ISNULL(SUM(CAST(Amount AS BIGINT)), 0) FROM TableProjectDetails WHERE ProjectName = @project AND Type = N'প্রদান'", conn);
                        cmdFunding.Parameters.AddWithValue("@project", selectedProject);
                        totalFunding = Convert.ToInt64(cmdFunding.ExecuteScalar());*/

                        // Total Spent (Expense)
                        SqlCommand cmdSpent = new SqlCommand("SELECT ISNULL(SUM(CAST(Amount AS BIGINT)), 0) FROM TableProjectDetails WHERE ProjectName = @project AND Type = N'খরচ'", conn);
                        cmdSpent.Parameters.AddWithValue("@project", selectedProject);
                        totalSpent = Convert.ToInt64(cmdSpent.ExecuteScalar());

                        conn.Close();
                    }

                    //lblTotalFunding.Text = "মোট প্রদান: " + totalFunding.ToString("N0")+ " ৳";
                    lblTotalSpent.Text = "মোট খরচ: " + totalSpent.ToString("N0", new System.Globalization.CultureInfo("hi-IN")) + " ৳";
                    hfPDFBalance.Value = lblTotalSpent.Text;
                    //lblAvailableAmount.Text = "অবশিষ্ট: " + (totalFunding - totalSpent).ToString("N0") + " ৳";

                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                    ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                }



            }
        }









        protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDateFilter.Visible = false;
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            ddlEntryPersonInDate.ClearSelection();
            pnlEntryPersonFilter.Visible = false;
            ddlEntryPerson.ClearSelection();
            ddlCategory2.ClearSelection();
            pnlCategoryFilter.Visible = false;
            ddlCategory.ClearSelection();


            switch (ddlSearchBy.SelectedValue)
            {
                case "Date":
                    pnlDateFilter.Visible = true;

                    break;
                case "EntryPerson":
                    
                    pnlEntryPersonFilter.Visible = true;

                    break;
                case "ExpenseCategory":
                    pnlCategoryFilter.Visible = true;

                    break;
            }
        }







       /* private void LoadEntryPersons()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT UserName FROM TableUserInfo", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlEntryPerson.DataSource = reader;
                    ddlEntryPerson.DataTextField = "UserName";
                    ddlEntryPerson.DataValueField = "UserName";
                    ddlEntryPerson.DataBind();
                    ddlEntryPerson.Items.Insert(0, new ListItem("-- None --", ""));
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }*/

       /* private void LoadExpenseCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ExpenseCategoryName FROM TableExpenseCategory", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlCategory.DataSource = reader;
                    ddlCategory.DataTextField = "ExpenseCategoryName";
                    ddlCategory.DataValueField = "ExpenseCategoryName";
                    ddlCategory.DataBind();
                    ddlCategory.Items.Insert(0, new ListItem("-- None --", ""));
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }*/


      /*  private void LoadExpenseCategories2()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT ExpenseCategoryName FROM TableExpenseCategory", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlCategory2.DataSource = reader;
                    ddlCategory2.DataTextField = "ExpenseCategoryName";
                    ddlCategory2.DataValueField = "ExpenseCategoryName";
                    ddlCategory2.DataBind();
                    ddlCategory2.Items.Insert(0, new ListItem("-- None --", ""));
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }
      */







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

                        lblFilteredExpense.Text = $"মোট খরচ: {totalKhroch.ToString("N0", new System.Globalization.CultureInfo("hi-IN"))} ৳";

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
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollBottom", "scrollToBottom();", true);
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
            
            hfPDFAppliedFilter.Value ="N/A";
            pnlFilteredExpense.Visible = false;

            LoadProjectSummary();
            LoadProjectDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollBottom", "scrollToBottom();", true);
            LoadFilterDropdowns();
        }







        protected void btnLogout_Click(object sender, EventArgs e)
        {
            /* Clear session if needed
            Session.Clear();
            Session.Abandon();*/

            // Redirect to login page
            Response.Redirect("Logout.aspx");
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
            ScriptManager.RegisterStartupScript(this, GetType(),"scrollBottom","scrollToBottom();",true);
        }








        /*private void LoadDropdowns()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    SqlDataAdapter da1 = new SqlDataAdapter("SELECT ProjectName FROM TableProjectList", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    ddlProjectName.DataSource = dt1;
                    ddlProjectName.DataTextField = "ProjectName";
                    ddlProjectName.DataValueField = "ProjectName";
                    ddlProjectName.DataBind();

                    SqlDataAdapter da2 = new SqlDataAdapter("SELECT DISTINCT EnteredBy FROM TableProjectDetails ", con);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    /*ddlEnteredBy.DataSource = dt2;
                    ddlEnteredBy.DataTextField = "UserName";
                    ddlEnteredBy.DataValueField = "UserName";
                    ddlEnteredBy.DataBind();
                    ddlEntryPerson.DataSource = dt2;
                    ddlEntryPerson.DataTextField = "UserName";
                    ddlEntryPerson.DataValueField = "UserName";
                    ddlEntryPerson.DataBind();
                    ddlEntryPerson.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));

                    ddlEntryPersonInDate.DataSource = dt2;
                    ddlEntryPersonInDate.DataTextField = "UserName";
                    ddlEntryPersonInDate.DataValueField = "UserName";
                    ddlEntryPersonInDate.DataBind();
                    ddlEntryPersonInDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));



                    SqlDataAdapter da3 = new SqlDataAdapter("SELECT DISTINCT ExpenseCategory FROM TableProjectDetails", con);
                    DataTable dt3 = new DataTable();
                    da3.Fill(dt3);
                    /*ddlExpenseCategory.DataSource = dt3;
                    ddlExpenseCategory.DataTextField = "ExpenseCategoryName";
                    ddlExpenseCategory.DataValueField = "ExpenseCategoryName";
                    ddlExpenseCategory.DataBind();

                    

                    ddlAddExpenseCategory.DataSource = dt3;
                    ddlAddExpenseCategory.DataTextField = "ExpenseCategoryName";
                    ddlAddExpenseCategory.DataValueField = "ExpenseCategoryName";
                    ddlAddExpenseCategory.DataBind();
                    ddlAddExpenseCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));

                    ddlCategory2.DataSource = dt3;
                    ddlCategory2.DataTextField = "ExpenseCategoryName";
                    ddlCategory2.DataValueField = "ExpenseCategoryName";
                    ddlCategory2.DataBind();
                    ddlCategory2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));

                    ddlCategoryInDate.DataSource = dt3;
                    ddlCategoryInDate.DataTextField = "ExpenseCategoryName";
                    ddlCategoryInDate.DataValueField = "ExpenseCategoryName";
                    ddlCategoryInDate.DataBind();
                    ddlCategoryInDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));


                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        })*/


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


        private void LoadEditDropdowns()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // Load Project dropdown
                    SqlDataAdapter da1 = new SqlDataAdapter("SELECT ProjectName FROM TableProjectList WHERE Status='Running' ", con);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);
                    ddlProjectName.DataSource = dt1;
                    ddlProjectName.DataTextField = "ProjectName";
                    ddlProjectName.DataValueField = "ProjectName";
                    ddlProjectName.DataBind();

                    



                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
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

                    // Use ScriptManager so the script is emitted properly for pages using ScriptManager/UpdatePanel
                    string jsArray = Newtonsoft.Json.JsonConvert.SerializeObject(categories);
                    ScriptManager.RegisterStartupScript(this, GetType(), "categoriesArray", $"var categories = {jsArray};", true);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }





        protected void gvProjectDetails_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            try
            {

                if (e.CommandName == "EditRow")
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableProjectDetails WHERE Id=@Id", con);
                        cmd.Parameters.AddWithValue("@Id", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfEditId.Value = id.ToString();
                            ddlProjectName.SelectedValue = dr["ProjectName"].ToString();
                            txtEditEnteredBy.Text = dr["EnteredBy"].ToString();
                            DateTime dt = (DateTime)dr["Date"];
                            txtEditDate.Text = dt.ToString("yyyy-MM-ddTHH:mm:ss");

                            txtRemarks.Text = dr["Remarks"].ToString();
                            txtType.Text = dr["Type"].ToString();
                            txtExpenseCategory.Text = dr["ExpenseCategory"].ToString();
                            if (txtType.Text == "প্রদান")
                            {
                                txtExpenseCategory.ReadOnly = true;  // Optional (it's already readonly)
                            }
                            else
                            {
                                txtExpenseCategory.ReadOnly = false;
                            }
                            txtEditAmount.Text = dr["Amount"].ToString();
                        }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showPopup('editPopup');", true);
                }
                else if (e.CommandName == "DeleteRow")
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TableProjectDetails WHERE Id=@Id", con);
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
                        <td style='padding: 4px 8px; border:1px solid white;'>ব্যক্তি:</td>
                        <td style='padding: 4px 8px; border:1px solid white;'>{dr["Enteredby"]}</td>
                    </tr>
                    <tr>
                        <td style=' padding: 4px 8px;border:1px solid white;'>তারিখ:</td>
                        <td style='padding: 4px 8px;border:1px solid white;'>{Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy hh:mm tt")}</td>

                    </tr>
        
        
                  
                    <tr>
                        <td style=' padding: 4px 8px; border:1px solid white;'>খরচের খাত:</td>
                        <td style='padding: 4px 8px; border:1px solid white;'>{dr["ExpenseCategory"]}</td>
                    </tr>
                    <tr>
                        <td style=' padding: 4px 8px; border:1px solid white;'>বর্ণনা:</td>
                        <td style='padding: 4px 8px; border:1px solid white;'>{dr["Remarks"]}</td>
                    </tr>
                    <tr>
                        <td style=' padding: 4px 8px; border:1px solid white;'>এমাউন্ট (৳):</td>
                        <td style='padding: 4px 8px; border:1px solid white;'>{dr["Amount"]}</td>
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
            
            pnlFilteredExpense.Visible = false;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(@"UPDATE TableProjectDetails 
                                  SET ProjectName=@ProjectName, 
                                       
                                      Date=@Date, 
                                      ExpenseCategory=@ExpenseCategory, 
                                      Remarks=@Remarks,  
                                      Amount=@Amount 
                                  WHERE Id=@Id", con);

                // Project
                cmd.Parameters.AddWithValue("@ProjectName", ddlProjectName.SelectedValue);

                

                // Date
                DateTime dateValue;
                if (DateTime.TryParse(txtEditDate.Text.Trim(), out dateValue))
                {
                    cmd.Parameters.AddWithValue("@Date", dateValue);
                }
                else
                {
                    // Optional: handle invalid date
                    cmd.Parameters.AddWithValue("@Date", DBNull.Value);
                }

                // ExpenseCategory
                cmd.Parameters.AddWithValue("@ExpenseCategory", txtExpenseCategory.Text.Trim());

                // Remarks
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());

                // Type
                //cmd.Parameters.AddWithValue("@Type", txtType.Text.Trim());

                // Amount
                decimal amountValue;
                if (decimal.TryParse(txtEditAmount.Text.Trim(), out amountValue))
                {
                    cmd.Parameters.AddWithValue("@Amount", amountValue);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Amount", 0); // or handle invalid input
                }

                // Id
                cmd.Parameters.AddWithValue("@Id", hfEditId.Value);


                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    
                    // Optionally show success message
                     ClientScript.RegisterStartupScript(this.GetType(), "successAlert", "alert('Update successful.');", true);
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
            LoadProjectDetails();
            LoadProjectSummary();
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);
        }







        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('editPopup');", true);
        }






        protected void btnYesDelete_Click(object sender, EventArgs e)
        {
            
            pnlFilteredExpense.Visible = false;
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM TableProjectDetails WHERE Id=@Id", con);
                    cmd.Parameters.AddWithValue("@Id", hfDeleteId.Value);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Deleted successfully.');", true);
                }
                LoadProjectDetails();
                LoadProjectSummary();
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }





        protected void btnCancelDelete_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('deletePopup');", true);
        }



      



        protected void btnSaveAdd_Click(object sender, EventArgs e)
        {

            pnlFilteredExpense.Visible = false;

            string projectName = txtAddProjectName.Text.Trim();
            string enteredBy = txtAddEnteredBy.Text.Trim();
            string expenseCategory;

            if (hfAddType.Value == "জমা")
            {
                expenseCategory = "(+)";
            }
            else
            {
                expenseCategory = ddlAddExpenseCategory.SelectedValue;
            }

            string remarks = txtAddRemarks.Text.Trim();
            decimal amount = 0;
            decimal.TryParse(txtAddAmount.Text.Trim(), out amount);
            string type = hfAddType.Value;
            DateTime date = DateTime.Now;

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
                    alert('Added successfully.');", true);
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

            // Refresh GridView
            LoadProjectDetails();
            LoadProjectSummary();

            // Close popup
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", "hidePopup('addPopup');", true);

        }







        protected void LoadAddDropDowns()
        {
            try
            {
                txtAddProjectName.Text = ddlProject.SelectedValue;
            txtAddEnteredBy.Text = Session["UserName"].ToString();

                ddlAddExpenseCategory.ClearSelection();
                txtAddAmount.Text = string.Empty; // Always clear first
                txtAddRemarks.Text = string.Empty;



                /* using (SqlConnection con = new SqlConnection(conStr))
                 {
                     con.Open();
                     SqlDataAdapter da = new SqlDataAdapter("SELECT ExpenseCategoryName FROM TableExpenseCategory", con);
                     DataTable dt = new DataTable();
                     da.Fill(dt);
                     ddlAddExpenseCategory.DataSource = dt;
                     ddlAddExpenseCategory.DataTextField = "ExpenseCategoryName";
                     ddlAddExpenseCategory.DataValueField = "ExpenseCategoryName";
                     ddlAddExpenseCategory.DataBind();
                     ddlAddExpenseCategory.Items.Insert(0, new ListItem("-- None --", ""));
                 }*/
                }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }






        protected void btnShowPopup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlProject.SelectedValue))
            {
                // Show an alert message using JavaScript
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Please select a project.');", true);
            }
            else
            {
                LoadAddDropDowns();
                
                // Re-show the popup after postback
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowPopup", "showAddPopup('" + hfAddType.Value + "');", true);
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

                filter +=  endDate.ToString("dd/MM/yyyy") + " পর্যন্ত ";



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
