using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace TalukderEngineering
{
    public partial class DisbursementEntryAdmin : BasePage
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadExpenseCategories();
            if (!IsPostBack)
            {
                LoadProjects();
                LoadUsers();
                LoadAccounts();

                txtDate.Text = TimeZoneInfo.ConvertTimeFromUtc(
                   DateTime.UtcNow,
                   TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time")
               ).ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }

        private void LoadProjects()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProjectName FROM TableProjectList WHERE Status = 'Running' ORDER BY ProjectName ASC";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlProject.Items.Clear();
                    ddlProject.Items.Add(new ListItem("-- সিলেক্ট প্রোজেক্ট--", ""));

                    while (reader.Read())
                    {
                        ddlProject.Items.Add(new ListItem(reader["ProjectName"].ToString(), reader["ProjectName"].ToString()));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading projects: " + ex.Message);
            }
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Name FROM TableEmployeeDetails WHERE UserType ='Employee' ORDER BY Name ASC";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlGivenTo.Items.Clear();
                    ddlEnteredByExpense.Items.Clear();

                    ddlGivenTo.Items.Add(new ListItem("-- সিলেক্ট --", ""));
                    ddlEnteredByExpense.Items.Add(new ListItem("-- সিলেক্ট --", ""));
                     ddlEnteredByExpense.Items.Add(
        new ListItem(Session["UserName"].ToString(), Session["UserName"].ToString())
    );


                    string currentUser = Session["UserName"] != null ? Session["UserName"].ToString() : "";

                    while (reader.Read())
                    {
                        string userName = reader["Name"].ToString();
                        ddlGivenTo.Items.Add(new ListItem(userName, userName));

                        // Add all users to expense dropdown (will be filtered by JS for others' expense)
                        ddlEnteredByExpense.Items.Add(new ListItem(userName, userName));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading users: " + ex.Message);
            }
        }

        private void LoadAccounts()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = string.Empty;

                    
                    if (Session["UserType"] != null && Session["UserType"].ToString() == "Accountant")
                    {
                        query = "SELECT AccountName FROM TableAccountList WHERE AccountantAccess = 'Yes' ORDER BY AccountName ASC";
                    }
                    else if (Session["UserType"] != null && Session["UserType"].ToString() == "Admin")
                    {
                        query = "SELECT AccountName FROM TableAccountList ORDER BY AccountName ASC";
                    }
                    
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlSource.Items.Clear();
                    ddlDepositAccount.Items.Clear();
                    ddlTransferSource.Items.Clear();
                    ddlTransferDestination.Items.Clear();

                    ddlSource.Items.Add(new ListItem("-- সিলেক্ট সোর্স --", ""));
                    ddlDepositAccount.Items.Add(new ListItem("-- সিলেক্ট এ্যাকাউন্ট --", ""));
                    ddlTransferSource.Items.Add(new ListItem("-- সিলেক্ট সোর্স --", ""));
                    ddlTransferDestination.Items.Add(new ListItem("-- সিলেক্ট ডেসটিনেশন --", ""));

                    while (reader.Read())
                    {
                        string accountName = reader["AccountName"].ToString();
                        ddlSource.Items.Add(new ListItem(accountName, accountName));
                        ddlDepositAccount.Items.Add(new ListItem(accountName, accountName));
                        ddlTransferSource.Items.Add(new ListItem(accountName, accountName));
                        ddlTransferDestination.Items.Add(new ListItem(accountName, accountName));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading accounts: " + ex.Message);
            }
        }

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string categoryName = txtNewCategory.Text.Trim();

                if (string.IsNullOrEmpty(categoryName))
                {
                    ShowMessage("Please enter a category name");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string checkQuery = "SELECT COUNT(*) FROM TableExpenseCategory WHERE ExpenseCategoryName = @CategoryName";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@CategoryName", categoryName);

                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        ShowMessage("Category already exists");
                        return;
                    }

                    string insertQuery = "INSERT INTO TableExpenseCategory (ExpenseCategoryName) VALUES (@CategoryName)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@CategoryName", categoryName);

                    int result = insertCmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        ShowMessage("Category added successfully");
                        txtNewCategory.Text = "";
                        LoadExpenseCategories();
                        ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", "closeCategoryModal();", true);
                    }
                    else
                    {
                        ShowMessage("Failed to add category");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving category: " + ex.Message);
            }
        }

        private void LoadExpenseCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ExpenseCategoryName FROM TableExpenseCategory ORDER BY ExpenseCategoryName ASC";
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

                    string jsArray = Newtonsoft.Json.JsonConvert.SerializeObject(categories);
                    string script = string.Format("var categories = {0};", jsArray);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "categoriesArray", script, true);
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "categoriesArray", "var categories = [];", true);
                ShowMessage("Error loading expense categories: " + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDate.Text))
                {
                    ShowMessage("Please select a date");
                    return;
                }

                // Get type from hidden field if dropdown is disabled (others' expense case)
                string type = ddlType.SelectedValue;
                string hfTypeValue = Request.Form["hfTypeValue"];

                if (!string.IsNullOrEmpty(hfTypeValue))
                {
                    type = hfTypeValue;
                }

                if (string.IsNullOrEmpty(type) || type == "None" || type == "-- Select Type --")
                {
                    ShowMessage("Please select a valid type");
                    return;
                }

                string expenseType = hfExpenseType.Value;
                DateTime date = DateTime.Parse(txtDate.Text);
                int savedCount = 0;

                if (expenseType == "others")
                {
                    // Others' Expense
                    string projectName = ddlProject.SelectedValue;

                    if (string.IsNullOrEmpty(projectName))
                    {
                        ShowMessage("Please select a project");
                        return;
                    }

                    if (string.IsNullOrEmpty(hfExpenseData.Value))
                    {
                        ShowMessage("Please add at least one row to save");
                        return;
                    }

                    savedCount = SaveOthersExpense(projectName, date);
                }
                else
                {
                    // Self transactions
                    if (type == "Give")
                    {
                        string source = ddlSource.SelectedValue;

                        if (string.IsNullOrEmpty(source))
                        {
                            ShowMessage("Please select a source");
                            return;
                        }

                        if (string.IsNullOrEmpty(hfGiveData.Value))
                        {
                            ShowMessage("Please add at least one row to save");
                            return;
                        }

                        savedCount = SaveSelfGive(source, date);
                    }
                    else if (type == "Expense")
                    {
                        string source = ddlSource.SelectedValue;

                        if (string.IsNullOrEmpty(source))
                        {
                            ShowMessage("Please select a source");
                            return;
                        }

                        if (string.IsNullOrEmpty(hfExpenseData.Value))
                        {
                            ShowMessage("Please add at least one row to save");
                            return;
                        }

                        savedCount = SaveSelfExpense(source, date);
                    }
                    else if (type == "Deposit")
                    {
                        if (string.IsNullOrEmpty(hfDepositData.Value))
                        {
                            ShowMessage("Please add at least one row to save");
                            return;
                        }

                        savedCount = SaveDeposit(date);
                    }
                    else if (type == "Transfer")
                    {
                        if (string.IsNullOrEmpty(hfTransferData.Value))
                        {
                            ShowMessage("Please add at least one row to save");
                            return;
                        }

                        savedCount = SaveTransfer(date);
                    }
                }

                if (savedCount > 0)
                {
                    ShowMessage($"Successfully saved {savedCount} record(s)");
                    ClearForm();
                }
                else
                {
                    ShowMessage("No records were saved");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving data: " + ex.Message);
            }
        }

        private int SaveOthersExpense(string projectName, DateTime date)
        {
            int savedCount = 0;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = serializer.Deserialize<List<Dictionary<string, object>>>(hfExpenseData.Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var row in rows)
                    {
                        string query = @"INSERT INTO TableProjectDetails 
                                       (Date, AccountName,  Type, ProjectName, EnteredBy, ExpenseCategory, Remarks, Amount)
                                       VALUES (@Date, @AccountName,  @Type, @ProjectName, @EnteredBy, @ExpenseCategory, @Remarks, @Amount)";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@AccountName", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@TransactionBy", Session["UserName"].ToString());
                        cmd.Parameters.AddWithValue("@Type", "খরচ");
                        cmd.Parameters.AddWithValue("@ProjectName", projectName);
                        cmd.Parameters.AddWithValue("@EnteredBy", row["enteredByValue"].ToString());
                        cmd.Parameters.AddWithValue("@ExpenseCategory", row["category"].ToString());
                        cmd.Parameters.AddWithValue("@Remarks", row["remarks"].ToString());
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToInt32(row["amount"]));

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0) savedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving others' expense data: " + ex.Message);
            }

            return savedCount;
        }

        private int SaveSelfGive(string source, DateTime date)
        {
            int savedCount = 0;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = serializer.Deserialize<List<Dictionary<string, object>>>(hfGiveData.Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var row in rows)
                    {
                        string projectName = ddlProject.SelectedValue;

                        string query = @"INSERT INTO TableProjectDetails 
                                       (Date, AccountName, TransactionBy, Type, ProjectName, EnteredBy, ExpenseCategory, Remarks, Amount)
                                       VALUES (@Date, @AccountName, @TransactionBy, @Type, @ProjectName, @EnteredBy, @ExpenseCategory, @Remarks, @Amount)";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@AccountName", source);
                        cmd.Parameters.AddWithValue("@TransactionBy", Session["UserName"].ToString());
                        cmd.Parameters.AddWithValue("@Type", "প্রদান");
                        cmd.Parameters.AddWithValue("@ProjectName", string.IsNullOrEmpty(projectName) ? (object)DBNull.Value : projectName);
                        cmd.Parameters.AddWithValue("@EnteredBy", row["givenToValue"].ToString());
                        cmd.Parameters.AddWithValue("@ExpenseCategory", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Remarks", row["remarks"].ToString() + " [" + source.ToString() + " => " + Session["UserName"].ToString() + " => " + row["givenToValue"].ToString() + "]");
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToInt32(row["amount"]));

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0) savedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving self give data: " + ex.Message);
            }

            return savedCount;
        }

        private int SaveSelfExpense(string source, DateTime date)
        {
            int savedCount = 0;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = serializer.Deserialize<List<Dictionary<string, object>>>(hfExpenseData.Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var row in rows)
                    {
                        string projectName = ddlProject.SelectedValue;

                        string query = @"INSERT INTO TableProjectDetails 
                                       (Date, AccountName, TransactionBy, Type, ProjectName, EnteredBy, ExpenseCategory, Remarks, Amount)
                                       VALUES (@Date, @AccountName, @TransactionBy, @Type, @ProjectName, @EnteredBy, @ExpenseCategory, @Remarks, @Amount)";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@AccountName", source);
                        cmd.Parameters.AddWithValue("@TransactionBy", Session["UserName"].ToString());
                        cmd.Parameters.AddWithValue("@Type", "খরচ");
                        cmd.Parameters.AddWithValue("@ProjectName", string.IsNullOrEmpty(projectName) ? (object)DBNull.Value : projectName);
                        cmd.Parameters.AddWithValue("@EnteredBy", Session["UserName"].ToString());
                        cmd.Parameters.AddWithValue("@ExpenseCategory", row["category"].ToString());
                        cmd.Parameters.AddWithValue("@Remarks", row["remarks"].ToString() + " [" + source.ToString() + " হতে খরচ]");
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToInt32(row["amount"]));

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0) savedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving self expense data: " + ex.Message);
            }

            return savedCount;
        }

        private int SaveDeposit(DateTime date)
        {
            int savedCount = 0;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = serializer.Deserialize<List<Dictionary<string, object>>>(hfDepositData.Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var row in rows)
                    {
                        string query = @"INSERT INTO TableProjectDetails 
                                       (Date, AccountName, TransactionBy, Type, Remarks, Amount)
                                       VALUES (@Date, @AccountName, @TransactionBy, @Type, @Remarks, @Amount)";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@AccountName", row["accountValue"].ToString());
                        cmd.Parameters.AddWithValue("@TransactionBy", Session["UserName"].ToString());
                        cmd.Parameters.AddWithValue("@Type", "জমা");
                        //cmd.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@EnteredBy", Session["UserName"].ToString());
                        //cmd.Parameters.AddWithValue("@ExpenseCategory", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Remarks", row["remarks"].ToString());
                        cmd.Parameters.AddWithValue("@Amount", Convert.ToInt32(row["amount"]));

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0) savedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving deposit data: " + ex.Message);
            }

            return savedCount;
        }

        private int SaveTransfer(DateTime date)
        {
            int savedCount = 0;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = serializer.Deserialize<List<Dictionary<string, object>>>(hfTransferData.Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var row in rows)
                    {
                        string userRemarks = row["remarks"].ToString();

                        // First entry: Transfer from source
                        string query1 = @"INSERT INTO TableProjectDetails 
                                       (Date, AccountName, TransactionBy, Type, ProjectName,  ExpenseCategory, Remarks, Amount)
                                       VALUES (@Date, @AccountName, @TransactionBy, @Type, @ProjectName,  @ExpenseCategory, @Remarks, @Amount)";

                        SqlCommand cmd1 = new SqlCommand(query1, conn);
                        cmd1.Parameters.AddWithValue("@Date", date);
                        cmd1.Parameters.AddWithValue("@AccountName", row["sourceValue"].ToString());
                        cmd1.Parameters.AddWithValue("@TransactionBy", Session["UserName"].ToString());
                        cmd1.Parameters.AddWithValue("@Type", "ট্রান্সফার");
                        cmd1.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                        //cmd1.Parameters.AddWithValue("@EnteredBy", Session["UserName"].ToString());
                        cmd1.Parameters.AddWithValue("@ExpenseCategory", DBNull.Value);
                        cmd1.Parameters.AddWithValue("@Remarks", userRemarks + " [Transferred to " + row["destination"].ToString() + "]");
                        cmd1.Parameters.AddWithValue("@Amount", Convert.ToInt32(row["amount"]));

                        int result1 = cmd1.ExecuteNonQuery();
                        if (result1 > 0) savedCount++;

                        // Second entry: Deposit to destination
                        string query2 = @"INSERT INTO TableProjectDetails 
                                       (Date, AccountName, TransactionBy, Type, ProjectName,  ExpenseCategory, Remarks, Amount)
                                       VALUES (@Date, @AccountName, @TransactionBy, @Type, @ProjectName,  @ExpenseCategory, @Remarks, @Amount)";

                        SqlCommand cmd2 = new SqlCommand(query2, conn);
                        cmd2.Parameters.AddWithValue("@Date", date);
                        cmd2.Parameters.AddWithValue("@AccountName", row["destinationValue"].ToString());
                        cmd2.Parameters.AddWithValue("@TransactionBy", Session["UserName"].ToString());
                        cmd2.Parameters.AddWithValue("@Type", "জমা");
                        cmd2.Parameters.AddWithValue("@ProjectName", DBNull.Value);
                        //cmd2.Parameters.AddWithValue("@EnteredBy", Session["UserName"].ToString());
                        cmd2.Parameters.AddWithValue("@ExpenseCategory", DBNull.Value);
                        cmd2.Parameters.AddWithValue("@Remarks", userRemarks + " [Transferred from " + row["source"].ToString() + "]");
                        cmd2.Parameters.AddWithValue("@Amount", Convert.ToInt32(row["amount"]));

                        int result2 = cmd2.ExecuteNonQuery();
                        if (result2 > 0) savedCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving transfer data: " + ex.Message);
            }

            return savedCount;
        }

        private void ClearForm()
        {
            ddlProject.SelectedIndex = 0;
            ddlSource.SelectedIndex = 0;
            txtDate.Text = TimeZoneInfo.ConvertTimeFromUtc(
                   DateTime.UtcNow,
                   TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time")
               ).ToString("dd-MM-yyyy hh:mm tt");
            ddlType.SelectedIndex = 0;
            ddlGivenTo.SelectedIndex = 0;
            ddlEnteredByExpense.SelectedIndex = 0;
            ddlDepositAccount.SelectedIndex = 0;
            ddlTransferSource.SelectedIndex = 0;
            ddlTransferDestination.SelectedIndex = 0;
            txtExpenseCategory.Text = "";
            txtRemarksGive.Text = "";
            txtRemarksExpense.Text = "";
            txtAmountGive.Text = "";
            txtAmountExpense.Text = "";
            txtRemarksDeposit.Text = "";
            txtAmountDeposit.Text = "";
            txtRemarksTransfer.Text = "";
            txtAmountTransfer.Text = "";
            hfGiveData.Value = "";
            hfExpenseData.Value = "";
            hfDepositData.Value = "";
            hfTransferData.Value = "";

            ScriptManager.RegisterStartupScript(this, GetType(), "clearRows",
                "giveRows = []; expenseRows = []; depositRows = []; transferRows = []; " +
                "document.getElementById('giveTableBody').innerHTML = ''; " +
                "document.getElementById('expenseTableBody').innerHTML = ''; " +
                "document.getElementById('depositTableBody').innerHTML = ''; " +
                "document.getElementById('transferTableBody').innerHTML = ''; " +
                "document.getElementById('giveTableContainer').style.display = 'none'; " +
                "document.getElementById('expenseTableContainer').style.display = 'none'; " +
                "document.getElementById('depositTableContainer').style.display = 'none'; " +
                "document.getElementById('transferTableContainer').style.display = 'none'; " +
                "handleTypeChange();", true);
        }

        private void ShowMessage(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('{message}');", true);
        }
    }
}