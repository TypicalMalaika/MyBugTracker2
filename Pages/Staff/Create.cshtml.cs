using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static MyBugTracker.Pages.Staff.IndexModel;

namespace MyBugTracker.Pages.Staff
{
    public class CreateModel : PageModel
    {
        public StaffInfo staffInfo = new StaffInfo();
        public string errorMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            staffInfo.name = Request.Form["Name"];
            staffInfo.email = Request.Form["Email"];
            staffInfo.phone = Request.Form["Phone"];
            staffInfo.address = Request.Form["Address"];

            if (staffInfo.name.Length == 0 || staffInfo.email.Length == 0 || staffInfo.phone.Length == 0 || staffInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mybugtracker;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO staff" +
                        "(name, email, phone, address) VALUES" +
                        "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", staffInfo.name);
                        command.Parameters.AddWithValue("@email", staffInfo.email);
                        command.Parameters.AddWithValue("@phone", staffInfo.phone);
                        command.Parameters.AddWithValue("@address", staffInfo.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Staff/Index");
        }
    }
}
