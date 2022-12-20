using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyBugTracker.Model;
using static MyBugTracker.Pages.Bugs.IndexModel;

namespace MyBugTracker.Pages.Bugs
{
    public class CreateBugModel : PageModel
    {
        private readonly ConnectionStringClass _db;

        public CreateBugModel (ConnectionStringClass db)
        {
            _db = db;
        }

        public IEnumerable<SelectEmployeeClass> displayData { get; set; }

        public async Task OnGet()
        {
            displayData = await _db.staff.ToListAsync();
        }

        public BugInfo bugInfo = new BugInfo();
        public string errorMessage = "";
        public void OnPost()
        {
            displayData = _db.staff.ToList();
            bugInfo.title = Request.Form["Title"];
            bugInfo.description = Request.Form["Description"];
            bugInfo.assigned = Request.Form["Assigned"];

            if (bugInfo.title.Length == 0 || bugInfo.description.Length == 0 || bugInfo.assigned == null)
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
                    string sql = "INSERT INTO bugs" +
                        "(title, description, assigned) VALUES" +
                        "(@title, @description, @assigned);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@title", bugInfo.title);
                        command.Parameters.AddWithValue("@description", bugInfo.description);
                        command.Parameters.AddWithValue("@assigned", bugInfo.assigned);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            //bugInfo.title = "";
            //bugInfo.description = "";
            //bugInfo.assigned = "";

            Response.Redirect("/Bugs/Index");
        }
    }
}
