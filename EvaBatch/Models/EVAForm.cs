using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVA_batch_Controlled.Models
{
    public class EVAForm
    {

        public int ID { get; set; }
        public string DOCNUMBER { get; set; }
        public string FORM_TYPE { get; set; }


        public DateTime TO_DATE { get; set; }
        public DateTime CREATE_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public DateTime LAST_UPDATE { get; set; }
        public string UPDATE_BY { get; set; }
        public List<Models.Log> Logs { get; set; }
        public List<string> CUSTOMER_LIST { get; set; }
        public string CUSTOMER { get; set; }
        public string REQ_TYPE { get; set; }
        public string DEVIATION_NUMBER { get; set; }
        public string INPUT_COMMENT { get; set; }
        public string CLSPN { get; set; }
        public string MFGNAME { get; set; }
        public string MPN { get; set; }
        public string QTY { get; set; }
        public DateTime? EXP_DATE { get; set; }
        public string REMARK { get; set; }
        public string FULL_NAME { get; set; }
        //03-08-2015 : Enhancement Add new Documentype "Solar EVA" items - joell
        public string Color { get; set; }
        public string EfficiencyPower { get; set; }

        public void SetNewDocNumber(string lastDocNumber, string formType, string color, string efficiencypowr)
        {
            if (string.IsNullOrEmpty(lastDocNumber))
            {
                //03-08-2015 : Enhancement Add new Documentype "Solar EVA" items - joell
                //Ex docunumber format 'SOxxxxL140'
                if (formType.ToUpper() == "SOLAR EVA")
                    DOCNUMBER = "SO" + "0001" + color + efficiencypowr;
                else
                    DOCNUMBER = formType.Trim().ToUpper() + "0000001";
            }
            else
            {
                int lnum = Convert.ToInt32(lastDocNumber.Substring(formType.Trim().Length));

                //03-08-2015 : Enhancement Add new Documentype "Solar EVA" items - joell
                //Ex docunumber format 'SOxxxxL140'
                if (formType.ToUpper() == "SOLAR EVA")
                    DOCNUMBER = "SO" + (lnum + 1).ToString("D4") + color + efficiencypowr;
                else
                    DOCNUMBER = formType.Trim().ToUpper() + (lnum + 1).ToString("D7");
            }
        }

        public string ToText()
        {
            string res = "";

            res += "[DOCNUMBER];[CUSTOMER];[REQ_TYPE];[DEVIATION_NUMBER];[INPUT_COMMENT];[CLSPN];[MFGNAME];[MPN];[QTY];[EXP_DATE];[REMARK]|";
            res += DOCNUMBER + ";";
            res += CUSTOMER + ";";
            res += REQ_TYPE + ";";
            res += DEVIATION_NUMBER + ";";
            res += INPUT_COMMENT + ";";
            res += CLSPN + ";";
            res += MFGNAME + ";";
            res += MPN + ";";
            res += QTY + ";";

            if (EXP_DATE.HasValue)
                res += EXP_DATE.Value.ToLongDateString() + ";";
            else
                res += ";";

            res += REMARK;
            return res;
        }

        public string ToTextSr()
        {
            string res = "";

            res += "[DOCNUMBER];[CUSTOMER];[REQ_TYPE];[DEVIATION_NUMBER];[INPUT_COMMENT];[CLSPN];[MFGNAME];[MPN];[QTY];[COLOR];[EFFICIENCY_POWER];[EXP_DATE];[REMARK]|";
            res += DOCNUMBER + ";";
            res += CUSTOMER + ";";
            res += REQ_TYPE + ";";
            res += DEVIATION_NUMBER + ";";
            res += INPUT_COMMENT + ";";
            res += CLSPN + ";";
            res += MFGNAME + ";";
            res += MPN + ";";
            res += QTY + ";";
            res += Color + ";";
            res += EfficiencyPower + ";";

            if (EXP_DATE.HasValue)
                res += EXP_DATE.Value.ToLongDateString() + ";";
            else
                res += ";";

            res += REMARK;
            return res;
        }

     
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Text { get; set; }

       
    }

}