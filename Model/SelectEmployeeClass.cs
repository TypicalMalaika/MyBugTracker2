using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBugTracker.Model
{
    public class SelectEmployeeClass
    {
        [Key]
        public string name { get; set; }
    }
}
