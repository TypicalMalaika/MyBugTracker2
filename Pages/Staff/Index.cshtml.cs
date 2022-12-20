using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyBugTracker.Pages.Staff
{
    public class IndexModel : PageModel
    {
        public List<StaffInfo> listStaff = new List<StaffInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mybugtracker;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM staff";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StaffInfo staffInfo = new StaffInfo();
                                staffInfo.id = "" + reader.GetInt32(0);
                                staffInfo.name = reader.GetString(1);
                                staffInfo.email = reader.GetString(2);
                                staffInfo.phone = reader.GetString(3);
                                staffInfo.address = reader.GetString(4);
                                staffInfo.created_at = reader.GetDateTime(5).ToString();

                                listStaff.Add(staffInfo);

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

        public class StaffInfo
        {
            public string id;
            public string name;
            public string email;
            public string phone;
            public string address;
            public string created_at;
        }
    }
}
