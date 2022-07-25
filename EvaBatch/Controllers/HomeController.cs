using EvaBatch.Filter;
using EvaBatch.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EVA_batch_Controlled;
using EvaBatch.Web.Helper;
using System.Configuration;
using EvaBatch.Web.Models;
using EVA_batch_Controlled.Models;


namespace EvaBatch.Filter
{
    [Auth]
    public class HomeController : Controller
    {
        LoginController loginCT = new LoginController();
        public EVAForm evaForm = new EVAForm();
        SearchController Search = new SearchController();
        public ActionResult Index()
        {
        
            return View();
        }


        [HttpGet]
        public ActionResult GetAllRecord(EVAForm evaForm)
        {
            List<view_EvaBatch> data = new List<view_EvaBatch>();
            string QTY = evaForm.QTY;
            string MFGNAME = evaForm.MFGNAME;
            string Cus = evaForm.CUSTOMER;
            string REQ_TYPE = evaForm.REQ_TYPE;
            string DOCNUMBER = evaForm.DOCNUMBER;
            DateTime startDate = evaForm.CREATE_DATE;
            DateTime toDate = evaForm.TO_DATE;
            List<String> ddlCus = new List<String>();
            string connStr = ConfigurationManager.AppSettings.Get("DB_Robin");
            string schema = ConfigurationManager.AppSettings.Get("DB_Schema");
            try
            {
                using (var session = new SessionFactory(connStr))
                {
                    string sql = @"select ID,DOCNUMBER AS Batch_NO, CUSTOMER,REQ_TYPE,MFGNAME,QTY, CREATE_DATE
                    from " + schema+ ".EVA_FORM ";

                    if (startDate < toDate) {
                        sql += " WHERE  (CREATE_DATE BETWEEN TO_DATE(SUBSTR('" + startDate + "',1,10),'mm-dd-yyyy') AND TO_DATE(SUBSTR('" + toDate + "',1,10),'mm-dd-yyyy') )";
                    }
                    if (!string.IsNullOrEmpty(Cus))
                    {
                        sql += " AND CUSTOMER='"+ Cus + "'";
                    }
                    if (!string.IsNullOrEmpty(DOCNUMBER))
                    {
                        sql += " AND DOCNUMBER='" + DOCNUMBER + "'";
                    }
                    if (!string.IsNullOrEmpty(REQ_TYPE))
                    {
                        sql += " AND REQ_TYPE='" + REQ_TYPE + "'";
                    }
                    if (!string.IsNullOrEmpty(MFGNAME))
                    {
                        sql += " AND MFGNAME='" + MFGNAME + "'";
                    }
                    if (!string.IsNullOrEmpty(QTY))
                    {
                        sql += " AND QTY='" + QTY + "'";
                    }

                    data = session.Exec<view_EvaBatch>(sql, null).ToList();

                    ddlCus = Search.GetCustomer();
                }
                var jsonResult = Json(new { status = true, data, ddlCus }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}