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
using Microsoft.AspNetCore.Mvc;

namespace EvaBatch.Controllers
{
    [Auth]
    public class SearchController : Controller
    {
        // GET: Search
        LoginController loginCT = new LoginController();
        public string connStr = ConfigurationManager.AppSettings.Get("DB_Robin");
        public string schema = ConfigurationManager.AppSettings.Get("DB_Schema");
     
        public ActionResult Index()
        {

            ViewBag.Title = "Search";

            return View();
        }
        public ActionResult GetDataById(int id) 
        {
            if (id == ' ' || id == null) {
                ViewBag.Title = "Home";
                return View();
            }

            List<EVAForm> data = new List<EVAForm>();
            List<String> ddlCus = new List<String>();
            List<EVA_batch_Controlled.Models.Log> Log = new List<EVA_batch_Controlled.Models.Log>();
            try
            {
                using (var session = new SessionFactory(connStr))
                {
                    string sql = @"select ID
                                            ,DOCNUMBER
                                            ,FORM_TYPE
                                            ,CREATE_DATE
                                            ,CREATE_BY
                                            ,LAST_UPDATE
                                            ,UPDATE_BY
                                            ,CUSTOMER
                                            ,REQ_TYPE
                                            ,DEVIATION_NUMBER
                                            ,INPUT_COMMENT
                                            ,CLSPN
                                            ,MFGNAME
                                            ,MPN
                                            ,QTY
                                            ,EXP_DATE
                                            ,REMARK
                                            ,COLOR
                                            ,EFFICIENCY_POWER
                    from " + schema + ".EVA_FORM   WHERE ID =" + id;
                    data = session.Exec<EVAForm>(sql, null).ToList();
                    Log = GetLog(id);
                    ddlCus =  GetCustomer();
                }
                var jsonResult = Json(new { status = true, data, Log, ddlCus }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

      
        public List<EVA_batch_Controlled.Models.Log> GetLog(int id)
        {
            List<EVA_batch_Controlled.Models.Log> res = new List<EVA_batch_Controlled.Models.Log>();
            string Master_schema = "master";
         
            using (var session = new SessionFactory(connStr))
            {
                string sql = @"select n.action,n.description,m.full_name username,n.datetime,m.user_email email,n.DOCNUMBER,n.username display_name  " +
                "from "+ schema + ".EVA_LOG n " +
                "left outer join "+ Master_schema + ".SYSTEM_USER_INFO m on n.username=m.user_name " +
                "where n.ID='"+id+"' order by n.datetime";
               
                res = session.Exec<EVA_batch_Controlled.Models.Log>(sql, null).ToList();
            
            }
            return res;
        }

        [HttpGet]
        public List<String>  GetCustomer()
        {
            List<String> Customers = new List<String>();
            try
            {
                using (var session = new SessionFactory(connStr))
                {
                    
                    string sql = @"select distinct CUSTOMER Text  FROM  " + schema + ".EVA_FORM order by CUSTOMER asc";
                    Customers = session.Exec<String>(sql, null).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Customers;
        }

        [HttpGet]
        public ActionResult UpdateEVAForm(EVAForm data)
        {

            //string UserStr = "";
            //if (Session["USER"] != null)
            //{
            //    UserStr = Session["USER"].ToString();
            //}

            try
            {
                using (var session = new SessionFactory(connStr))
                {
                    string sql_update = "UPDATE quality.EVA_FORM set DOCNUMBER='" + data.DOCNUMBER + "',FORM_TYPE='" + data.FORM_TYPE + "',LAST_UPDATE=sysdate," +
                        "UPDATE_BY=(select USER_NAME FROM  MASTER.SYSTEM_USER_INFO where USER_EMPNO = '" + data.UPDATE_BY + "')," +
                        "CUSTOMER='" + data.CUSTOMER + "',REQ_TYPE='" + data.REQ_TYPE + "',DEVIATION_NUMBER ='" + data.DEVIATION_NUMBER + "'," +
                        "INPUT_COMMENT='" + data.INPUT_COMMENT + "',CLSPN='" + data.CLSPN + "',MFGNAME='" + data.MFGNAME + "',MPN='" + data.MPN + "',QTY='" + data.QTY + "',";
                    if (data.EXP_DATE.HasValue)
                        sql_update += "EXP_DATE= TO_DATE(SUBSTR('" + data.EXP_DATE + "',1,10),'mm-dd-yyyy'),";
                    else
                        sql_update += "EXP_DATE= null ,";

                    sql_update        += "REMARK   ='"+ data.REMARK + "'";
                    sql_update        += "where ID ='"+ data.ID + "'";
                    session.Exec<EVAForm>(sql_update, null).SingleOrDefault();

                    string sql = "insert into quality.EVA_LOG(ID,action,description,username,datetime,DOCNUMBER) ";
                    sql += "values(" +data.ID+ ",'UPDATE','" + data.ToText() + "',(select USER_NAME FROM  MASTER.SYSTEM_USER_INFO where USER_EMPNO = '" + data.UPDATE_BY + "'),sysdate, '" + data.DOCNUMBER + "')";
                    session.Exec<Log>(sql, null).SingleOrDefault();

                    return GetDataById(data.ID); 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
        }

    }
}