using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace TalukderEngineering
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            // Redirect if session expired or user not logged in
           /* if (Session["UserName"] == null)
            {
                Response.Redirect("~/Login.aspx"); // Change to your actual login page
                return;
            }*/

            if (!IsPostBack && Session["UserName"] != null)
            {
                lblUserName.Text = Session["UserName"].ToString();
            }




            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);

        }




    }
}