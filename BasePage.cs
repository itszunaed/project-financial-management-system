using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace TalukderEngineering
{
    public class BasePage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            

            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

            // Redirect to Login if not logged in (except on Login page)
            if (Session["UserName"] == null && currentPage != "Login.aspx")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Get role
            string userType = Session["UserType"]?.ToString();


            // Define pages with case-insensitive comparison
            var adminPages = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "UserList.aspx",
                "ExpenseCategory.aspx",
                "ProjectDetails.aspx",
                "EmployeeDetails.aspx",
                "ProjectList.aspx",
                "PasswordChange.aspx",
                "DisbursementEntryAdmin.aspx",
                "AccountReport.aspx",
                "EmployeeReport.aspx",
                "AccountList.aspx",
                "ReturnedMoneyApproval.aspx",
                "PendingBillList.aspx"
            };

            var employeePages = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "EmployeeHome.aspx",
                "EmployeeEntries.aspx",
                "ProjectReportEmp.aspx",
                "EmployeeReportEmp.aspx",
                "EmployeePasswordChange.aspx"
            };

            var accountantPages = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "UserList.aspx",
                "ExpenseCategory.aspx",
                "ProjectDetails.aspx",
                "EmployeeDetails.aspx",
                "ProjectList.aspx",
                "PasswordChange.aspx",
                "DisbursementEntryAdmin.aspx",
                "AccountReport.aspx",
                "EmployeeReport.aspx",
                "AccountList.aspx",
                "ReturnedMoneyApproval.aspx",
                "PendingBillList.aspx"
            };

            // Restrict admin pages from employees and accountants
            if ((userType == "Employee") && adminPages.Contains(currentPage))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Restrict employee pages from admins and accountants
            if ((userType == "Admin" || userType == "Accountant") && employeePages.Contains(currentPage))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            /*Restrict accountant pages from admins and employees
            if ((userType == "Admin" || userType == "Employee") && accountantPages.Contains(currentPage))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }*/

            base.OnLoad(e);

        }

        


        }
    }