using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TalukderEngineering
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["UserId"] = null;
            Session["UserName"] = null;

            // Optional: clear all session data
            Session.Clear();
            
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("Login.aspx");

        }
    }
}