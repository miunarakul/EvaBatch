using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVA_batch_Controlled.Models
{
    public class Log
    {
        public Log()
        {
            EVAForm = new EVAForm();
            User = new User();
        }
    
        public EVAForm EVAForm { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Display_name { get; set; }
        public User User { get; set; }
        public DateTime Datetime { get; set; }
        public string DOCNUMBER { get; set; }

    }
}
