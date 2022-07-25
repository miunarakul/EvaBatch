using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaBatch.Web.Models
{
    public class ACTIVE_DIRECTION_Models
    {
        public string USER_NAME { get; set; }
        public string FULL_NAME { get; set; }
    }
    public class view_EvaBatch
    {
        public int ID { get; set; }
        public string Batch_NO { get; set; }
        public string CUSTOMER { get; set; }
        public string REQ_TYPE { get; set; }
        public string MFGNAME { get; set; }
        public string QTY { get; set; }
        public DateTime CREATE_DATE { get; set; }
        List<string> ddlCus { get; set; }
    }

}