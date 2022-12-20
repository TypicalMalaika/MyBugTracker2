using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBugTracker.Pages.Bugs
{
    public class IndexModel : PageModel
    {
        public List<BugInfo> listBugs = new List<BugInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mybugtracker;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM bugs";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BugInfo bugInfo = new BugInfo();
                                bugInfo.id = "" + reader.GetInt32(0);
                                bugInfo.title = reader.GetString(1);
                                bugInfo.description = reader.GetString(2);
                                bugInfo.assigned = reader.GetString(3); //need to replace this
                                bugInfo.created_at = reader.GetDateTime(4).ToString(); //need to replace this

                                listBugs.Add(bugInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public class BugInfo
        {
            public string id;
            public string title;
            public string description;
            public string assigned;
            public string created_at;
        }
    }
}
