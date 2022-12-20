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
    public class EditBugsModel : PageModel
    {
        private readonly ConnectionStringClass _db;

        public EditBugsModel(ConnectionStringClass db)
        {
            _db = db;
        }

        public IEnumerable<SelectEmployeeClass> displayData { get; set; }

        //public async Task OnGet()
        //{
        //    displayData = await _db.staff.ToListAsync();
        //}
        public BugInfo bugInfo = new BugInfo();
        public string errorMessage = "";

        public void OnGet()
        {
            displayData = _db.staff.ToList();

            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mybugtracker;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM bugs WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bugInfo.id = "" + reader.GetInt32(0);
                                bugInfo.title = reader.GetString(1);
                                bugInfo.description = reader.GetString(2);
                                bugInfo.assigned = reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            displayData = _db.staff.ToList();

            bugInfo.id = Request.Query["id"];
            bugInfo.title = Request.Form["title"];
            bugInfo.description = Request.Form["description"];
            bugInfo.assigned = Request.Form["assigned"];

            if (bugInfo.id.Length == 0 || bugInfo.title.Length == 0 || bugInfo.description.Length == 0 || bugInfo.assigned == null)
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
                    String sql = "UPDATE bugs " +
                        "SET title=@title, description=@description, assigned=@assigned " +
                        "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@title", bugInfo.title);
                        command.Parameters.AddWithValue("@description", bugInfo.description);
                        command.Parameters.AddWithValue("@assigned", bugInfo.assigned);
                        command.Parameters.AddWithValue("@id", bugInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Bugs/Index");
        }
    }
}