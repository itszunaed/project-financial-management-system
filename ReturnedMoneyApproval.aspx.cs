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
    public partial class ReturnedMoneyApproval : BasePage
    {
        readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }

        }

        private void BindGrid()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT * FROM TableReturnedMoney ORDER BY Date DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvReturnedMoneyApproval.DataSource = dt;
                gvReturnedMoneyApproval.DataBind();
            }
        }

        protected void gvReturnedMoneyApproval_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ApproveRow")
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    string transactionBy = Session["UserName"].ToString();

                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        con.Open();

                        // Step 1: Get the row data from TableReturnedMoney
                        SqlCommand getCmd = new SqlCommand("SELECT * FROM TableReturnedMoney WHERE Id=@Id", con);
                        getCmd.Parameters.AddWithValue("@Id", id);
                        SqlDataReader reader = getCmd.ExecuteReader();

                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            // Step 2: Insert into TableProjectDetails (without Id, but with TransactionBy)
                            SqlCommand insertCmd = new SqlCommand(@"
    INSERT INTO TableProjectDetails 
        (Date, AccountName, EnteredBy, Type, Remarks, Amount, TransactionBy)
    VALUES 
        (@Date, @AccountName, @EnteredBy, @Type, @Remarks, @Amount, @TransactionBy)", con);

                            insertCmd.Parameters.AddWithValue("@Date", row["Date"]);
                            insertCmd.Parameters.AddWithValue("@AccountName", row["AccountName"]);
                            insertCmd.Parameters.AddWithValue("@EnteredBy", row["EnteredBy"]);
                            insertCmd.Parameters.AddWithValue("@Type", row["Type"]);
                            insertCmd.Parameters.AddWithValue("@Remarks", row["Remarks"]);
                            insertCmd.Parameters.AddWithValue("@Amount", row["Amount"]);
                            insertCmd.Parameters.AddWithValue("@TransactionBy", transactionBy); // from Session


                            insertCmd.ExecuteNonQuery();

                            // Step 3: Delete from TableReturnedMoney
                            SqlCommand deleteCmd = new SqlCommand("DELETE FROM TableReturnedMoney WHERE Id=@Id", con);
                            deleteCmd.Parameters.AddWithValue("@Id", id);
                            deleteCmd.ExecuteNonQuery();

                            ClientScript.RegisterStartupScript(this.GetType(), "alert", $@"
                    alert('Approved successfully.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ClientScript.RegisterStartupScript(this.GetType(), "errorAlert", $"alert('Error: {message}');", true);
            }

            // Refresh GridView
            BindGrid();

            


        }
    }
}