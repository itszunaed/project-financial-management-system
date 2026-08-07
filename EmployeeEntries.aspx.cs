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
    public partial class EmployeeEntries : BasePage
    {
        readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadEntries();
            }
        }





        private void LoadEntries()
        {
            try
            {
                string user = Session["UserName"] as string;


                string query = "SELECT ProjectName, [Date], EnteredBy, Remarks, Type, ExpenseCategory, Amount FROM TableProjectDetails WHERE EnteredBy = @user ORDER BY[Date] ASC";

           
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@user", user); // add parameter here

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);


                            // Add Balance column
                            dt.Columns.Add("Balance", typeof(long));

                            long runningBalance = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                long amount = Convert.ToInt64(row["Amount"]);
                                string type = row["Type"].ToString();

                                if (type == "প্রদান")
                                    runningBalance += amount;
                                else
                                    runningBalance -= amount;

                                row["Balance"] = runningBalance;
                            }

                            


                            gvEntries.DataSource = dt;
                            gvEntries.DataBind();
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



        protected void gvProjectDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string type = DataBinder.Eval(e.Row.DataItem, "Type").ToString();

                if (type == "প্রদান")
                {
                    e.Row.CssClass += " soft-green-row";
                }
                
            }
        }




    }
}