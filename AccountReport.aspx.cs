using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TalukderEngineering
{
    public partial class AccountReport : BasePage
    {

            readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            protected void Page_Load(object sender, EventArgs e)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
                Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

                // Ensure categories JS is emitted every full page render and before client-side autocomplete runs
                LoadExpenseCategories();

                if (!IsPostBack)
                {
                    string status = Request.QueryString["status"] ?? "Running"; // default to "Running"

                    LoadAccountNames();
                    pnlFilteredExpense.Visible = false;
                    LoadEditDropdowns();

                if (Session["UserType"] != null && Session["UserType"].ToString() == "Accountant")
                {
                    // Hide the Action column (index 2 in your markup)
                    gvProjectDetails.Columns[9].Visible = false;
                }
            }
            }

        private void LoadAccountNames()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    string query = string.Empty;

                    // Decide query based on UserType
                    if (Session["UserType"] != null && Session["UserType"].ToString() == "Accountant")
                    {
                        query = "SELECT AccountName FROM TableAccountList WHERE AccountantAccess = 'Yes' ORDER BY AccountName ASC";
                    }
                    else if (Session["UserType"] != null && Session["UserType"].ToString() == "Admin")
                    {
                        query = "SELECT AccountName FROM TableAccountList ORDER BY AccountName ASC";
                        //query = "SELECT DISTINCT AccountName FROM TableProjectDetails WHERE AccountName IS NOT NULL AND AccountName <> '' ORDER BY AccountName ASC";
                    }
                    

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        ddlProject.DataSource = reader;
                        ddlProject.DataTextField = "AccountName";
                        ddlProject.DataValueField = "AccountName"; // or "ProjectID" if available
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
            try
            {
                string selectedProject = ddlProject.SelectedItem.Text;

                string query = @"SELECT *
                     FROM TableProjectDetails
                     WHERE AccountName = @projectName
                     ORDER BY [Date] ASC";

                using (SqlConnection con = new SqlConnection(conStr))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@projectName", selectedProject);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add column
                    dt.Columns.Add("CashIn", typeof(long));
                    dt.Columns.Add("CashOut", typeof(long));
                    dt.Columns.Add("Balance", typeof(long));


                    long runningBalance = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        long amount = Convert.ToInt64(row["Amount"]);
                        string type = row["Type"].ToString();

                        if (type == "জমা" || type == "ফেরত")
                        {
                            runningBalance += amount;

                            row["CashIn"] = amount;
                            row["CashOut"] = DBNull.Value;
                        }
                        else
                        {
                            runningBalance -= amount;

                            row["CashOut"] = amount;
                            row["CashIn"] = DBNull.Value;
                        }

                        row["Balance"] = runningBalance;
                    }

                    // 🔥 Store full data with correct balance
                    Session["FullProjectData"] = dt;

                    if (string.IsNullOrEmpty(ddlProject.SelectedValue))
                    {
                        lblProjectName.Text = "এ্যাকাউন্টের নাম:";


                        lblAvailableAmount.Text = "ব্যালান্স:";
                    }
                    else
                    {

                        lblProjectName.Text = "এ্যাকাউন্টের নাম:" + "\u00A0\u00A0" + selectedProject;
                        lblAvailableAmount.Text = "ব্যালান্স:" + "\u00A0\u00A0" + runningBalance.ToString("N0", new System.Globalization.CultureInfo("hi-IN")) + " ৳";
                        hfPDFBalance.Value = lblAvailableAmount.Text;
                    }

                    gvProjectDetails.DataSource = dt;
                    gvProjectDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }
        }


        /* Optional: Format balance in RowDataBound event for better display
        protected void gvProjectDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblBalance = (Label)e.Row.FindControl("lblBalance");
                if (lblBalance != null)
                {
                    long balance = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "Balance"));
                    lblBalance.Text = balance.ToString("N0") + " ৳";

                    // Optional: Color code based on positive/negative balance
                    if (balance < 0)
                    {
                        lblBalance.ForeColor = System.Drawing.Color.Red;
                    }
                    else if (balance > 0)
                    {
                        lblBalance.ForeColor = System.Drawing.Color.Green;
                    }
                }
            }
        }*/

        /*private void LoadProjectSummary()
        {
            if (string.IsNullOrEmpty(ddlProject.SelectedValue))
            {
                lblProjectName.Text = "একাউন্টের নাম:";
                // Hide or remove the cash in/out labels
                lblTotalFunding.Visible = false;
                lblTotalSpent.Visible = false;
                lblAvailableAmount.Text = "ব্যালান্স:";
            }
            else
            {
                string selectedProject = ddlProject.SelectedItem.Text;
                lblProjectName.Text = "একাউন্টের নাম:" + "\u00A0\u00A0" + selectedProject;

                long totalFunding;
                long totalSpent;
                try
                {
                    using (SqlConnection conn = new SqlConnection(conStr))
                    {
                        conn.Open();

                        // Total Funding (Deposit)
                        SqlCommand cmdFunding = new SqlCommand("SELECT ISNULL(SUM(CAST(Amount AS BIGINT)), 0) FROM TableProjectDetails WHERE AccountName = @project AND Type = N'জমা' ", conn);
                        cmdFunding.Parameters.AddWithValue("@project", selectedProject);
                        totalFunding = Convert.ToInt64(cmdFunding.ExecuteScalar());

                        // Total Spent (Expense - everything except জমা)
                        SqlCommand cmdSpent = new SqlCommand("SELECT ISNULL(SUM(CAST(Amount AS BIGINT)), 0) FROM TableProjectDetails WHERE AccountName = @project AND Type <> N'জমা' ", conn);
                        cmdSpent.Parameters.AddWithValue("@project", selectedProject);
                        totalSpent = Convert.ToInt64(cmdSpent.ExecuteScalar());

                        conn.Close();
                    }

                    // Hide cash in/out labels
                    lblTotalFunding.Visible = false;
                    lblTotalSpent.Visible = false;

                    // Show only current balance
                    lblAvailableAmount.Text = "ব্যালান্স: " + (totalFunding - totalSpent).ToString("N0") + " ৳";
                }
                catch (Exception ex)
                {
                    string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                    ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
                }
            }
        }*/

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["FullProjectData"] == null)
                    return;

                DataTable fullDt = (DataTable)Session["FullProjectData"];

                DataView dv = new DataView(fullDt);
                List<string> filters = new List<string>();

                // Date filter
                if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    filters.Add($"[Date] >= #{Convert.ToDateTime(txtStartDate.Text):MM/dd/yyyy}#");
                }

                if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    filters.Add($"[Date] < #{Convert.ToDateTime(txtEndDate.Text).AddDays(1):MM/dd/yyyy}#");
                }

                // Entry person filter
                if (!string.IsNullOrWhiteSpace(ddlEntryPersonInDate.SelectedValue))
                {
                    filters.Add($"TransactionBy = '{ddlEntryPersonInDate.SelectedValue.Replace("'", "''")}'");
                }

                // Category filter
                if (!string.IsNullOrWhiteSpace(ddlCategoryInDate.SelectedValue))
                {
                    filters.Add($"ExpenseCategory = '{ddlCategoryInDate.SelectedValue.Replace("'", "''")}'");
                }

                if (!string.IsNullOrWhiteSpace(ddlProjectFilter.SelectedValue))
                {
                    filters.Add($"ProjectName = '{ddlProjectFilter.SelectedValue.Replace("'", "''")}'");
                }

                if (!string.IsNullOrWhiteSpace(ddlTypeFilter.SelectedValue))
                {
                    filters.Add($"Type = '{ddlTypeFilter.SelectedValue.Replace("'", "''")}'");
                }

                dv.RowFilter = string.Join(" AND ", filters);

                gvProjectDetails.DataSource = dv.ToTable();
                gvProjectDetails.DataBind();

                // ---------- SUMMARY ----------
                decimal totalIn = 0;
                decimal totalOut = 0;

                foreach (DataRowView row in dv)
                {
                    decimal amt = Convert.ToDecimal(row["Amount"]);
                    string type = row["Type"].ToString();

                    if (type == "জমা" || type == "ফেরত")
                        totalIn += amt;
                    else
                        totalOut += amt;
                }

                lblFilteredExpense.Text =
                    $"ক্যাশ ইন: {totalIn.ToString("N0", new System.Globalization.CultureInfo("hi-IN"))} ৳ &nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp; ক্যাশ আউট: {totalOut.ToString("N0", new System.Globalization.CultureInfo("hi-IN"))} ৳";

                pnlFilteredExpense.Visible = true;

                hfPDFAppliedFilter.Value = GetAppliedFilterText();
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "scrollBottom", "scrollToBottom();", true);
        }


        protected void gvProjectDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string type = DataBinder.Eval(e.Row.DataItem, "Type").ToString();

                if (type == "প্রদান" || type == "খরচ")
                {
                    e.Row.CssClass += " soft-red-row";
                }
                else if (type == "জমা" || type == "ফেরত")
                {
                    e.Row.CssClass += " soft-green-row";
                }
            }
        }




        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
            {
                hfPDFProjectName.Value = ddlProject.SelectedItem.Text;
                hfPDFAppliedFilter.Value = "N/A";
            pnlFilteredExpense.Visible = false;

                //LoadProjectSummary();
                LoadProjectDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollBottom", "scrollToBottom();", true);
            LoadFilterDropdowns();
            }

            protected void btnLogout_Click(object sender, EventArgs e)
            {
                Response.Redirect("Logout.aspx");
            }

            protected void btnClear_Click(object sender, EventArgs e)
            {
                hfPDFAppliedFilter.Value = "N/A";
                ddlCategoryInDate.ClearSelection();
                ddlEntryPersonInDate.ClearSelection();
            ddlProjectFilter.ClearSelection();
            ddlTypeFilter.ClearSelection();
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                pnlFilteredExpense.Visible = false;
                LoadProjectDetails();
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollBottom", "scrollToBottom();", true);
        }

            private void LoadFilterDropdowns()
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();

                        string selectedProject = ddlProject.SelectedItem?.Text ?? "";

                    // Load EnteredBy dropdown filtered by selected project
                    SqlCommand cmdEnteredBy = new SqlCommand(
                        "SELECT DISTINCT TransactionBy FROM TableProjectDetails WHERE AccountName = @project AND TransactionBy IS NOT NULL AND TransactionBy <> ''", con);
                    cmdEnteredBy.Parameters.AddWithValue("@project", selectedProject);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmdEnteredBy);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);

                    ddlEntryPersonInDate.DataSource = dt1;
                    ddlEntryPersonInDate.DataTextField = "TransactionBy";
                    ddlEntryPersonInDate.DataValueField = "TransactionBy";
                    ddlEntryPersonInDate.DataBind();
                    ddlEntryPersonInDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));

                    // Load ExpenseCategory dropdown filtered by selected project
                    SqlCommand cmdCategory = new SqlCommand(
                         "SELECT DISTINCT ExpenseCategory FROM TableProjectDetails WHERE AccountName = @project AND ExpenseCategory IS NOT NULL AND ExpenseCategory <> ''", con);
                    cmdCategory.Parameters.AddWithValue("@project", selectedProject);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmdCategory);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);

                    ddlCategoryInDate.DataSource = dt2;
                    ddlCategoryInDate.DataTextField = "ExpenseCategory";
                    ddlCategoryInDate.DataValueField = "ExpenseCategory";
                    ddlCategoryInDate.DataBind();
                    ddlCategoryInDate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));



                    // Load Project Filter
                    // =========================
                    SqlCommand cmdProject = new SqlCommand(
                        @"SELECT DISTINCT ProjectName 
                  FROM TableProjectDetails 
                  WHERE AccountName = @project AND ProjectName IS NOT NULL 
                    AND ProjectName <> ''", con);

                    cmdProject.Parameters.AddWithValue("@project", selectedProject);

                    SqlDataAdapter daProject = new SqlDataAdapter(cmdProject);
                    DataTable dtProject = new DataTable();
                    daProject.Fill(dtProject);

                    ddlProjectFilter.DataSource = dtProject;
                    ddlProjectFilter.DataTextField = "ProjectName";
                    ddlProjectFilter.DataValueField = "ProjectName";
                    ddlProjectFilter.DataBind();
                    ddlProjectFilter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- None --", ""));
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

                        SqlDataAdapter da1 = new SqlDataAdapter("SELECT ProjectName FROM TableProjectList WHERE Status = 'Running' ", con);
                        DataTable dt1 = new DataTable();
                        da1.Fill(dt1);
                        ddlProjectName.DataSource = dt1;
                        ddlProjectName.DataTextField = "ProjectName";
                        ddlProjectName.DataValueField = "ProjectName";
                        ddlProjectName.DataBind();
                        ddlProjectName.Items.Insert(0, new ListItem("-- প্রযোজ্য নয় --", ""));

                        SqlDataAdapter da2 = new SqlDataAdapter("SELECT AccountName FROM TableAccountList ORDER BY AccountName ASC", con);
                        DataTable dt2 = new DataTable();
                        da2.Fill(dt2);
                        ddlEditAccount.DataSource = dt2;
                        ddlEditAccount.DataTextField = "AccountName";
                        ddlEditAccount.DataValueField = "AccountName";
                        ddlEditAccount.DataBind();
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
                                ddlEditAccount.SelectedValue = dr["AccountName"].ToString();
                                ddlProjectName.SelectedValue = dr["ProjectName"].ToString();
                                txtEditEnteredBy.Text = dr["TransactionBy"].ToString();
                                DateTime dt = (DateTime)dr["Date"];
                                txtEditDate.Text = dt.ToString("yyyy-MM-ddTHH:mm:ss");

                                txtRemarks.Text = dr["Remarks"].ToString();
                                ddlType.SelectedValue = dr["Type"].ToString();
                                txtExpenseCategory.Text = dr["ExpenseCategory"].ToString();
                                
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
                            <td style=' padding: 4px 8px;border:1px solid white;'>তারিখ:</td>
                            <td style='padding: 4px 8px;border:1px solid white;'>{Convert.ToDateTime(dr["Date"]).ToString("dd-MM-yyyy hh:mm tt")}</td>

                        </tr>

                        <tr>
                            <td style='padding: 4px 8px; border:1px solid white;'>সম্পাদনকারী ব্যক্তি:</td>
                            <td style='padding: 4px 8px; border:1px solid white;'>{dr["TransactionBy"]}</td>
                        </tr>
                        
                        <tr>
                            <td style='padding: 4px 8px; border:1px solid white;'>ধরণ:</td>
                            <td style='padding: 4px 8px; border:1px solid white;'>{dr["Type"]}</td>
                        </tr>
                        <tr>
                            <td style=' padding: 4px 8px; border:1px solid white;'>খরচের খাত:</td>
                            <td style='padding: 4px 8px; border:1px solid white;'>{dr["ExpenseCategory"]}</td>
                        </tr>
                         <tr>
                            <td style=' padding: 4px 8px; border:1px solid white;'>প্রোজেক্টের নাম:</td>
                            <td style='padding: 4px 8px; border:1px solid white;'>{dr["ProjectName"]}</td>
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
                                      SET 
                                        Date=@Date, AccountName = @AccountName, Type=@Type, ExpenseCategory=@ExpenseCategory,
                                        ProjectName=@ProjectName, Remarks=@Remarks, Amount=@Amount  
                                        WHERE Id=@Id", con);

                    cmd.Parameters.AddWithValue("@ProjectName", ddlProjectName.SelectedValue);
                cmd.Parameters.AddWithValue("@AccountName", ddlEditAccount.SelectedValue);
                cmd.Parameters.AddWithValue("@Type", ddlType.SelectedValue);
                

                    DateTime dateValue;
                    if (DateTime.TryParse(txtEditDate.Text.Trim(), out dateValue))
                    {
                        cmd.Parameters.AddWithValue("@Date", dateValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Date", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@ExpenseCategory", txtExpenseCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());

                    decimal amountValue;
                    if (decimal.TryParse(txtEditAmount.Text.Trim(), out amountValue))
                    {
                        cmd.Parameters.AddWithValue("@Amount", amountValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Amount", 0);
                    }

                    cmd.Parameters.AddWithValue("@Id", hfEditId.Value);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
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
                //LoadProjectSummary();
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
                    //LoadProjectSummary();
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

            protected string GetAppliedFilterText()
            {
                string filter = "";

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

            if (!string.IsNullOrEmpty(ddlProjectFilter.SelectedValue))
            {
                filter += " প্রোজেক্ট: " + ddlProjectFilter.SelectedItem.Text;
            }

            if (!string.IsNullOrEmpty(ddlTypeFilter.SelectedValue))
            {
                filter += " ধরণ: " + ddlTypeFilter.SelectedItem.Text;
            }

            return string.IsNullOrEmpty(filter) ? "N/A" : filter.TrimStart(',');
            }


       



    }
}