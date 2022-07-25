using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVA_batch_Controlled.Models
{
    public class User
    {
        public string USERNAME { get; set; }
        public string DISPLAY_NAME { get; set; }
        public string EMAIL { get; set; }

        public User GetLogonUser()
        {
            string res = "";
            if (HttpContext.Current.Session["USER"] != null)
            {
                res = HttpContext.Current.Session["USER"].ToString();
            }
            return new User { USERNAME = res };
        }
    }
}