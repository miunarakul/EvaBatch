using EvaBatch.Web.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace EvaBatch.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Title = "Login";

            return View();

        } // end ActionResult: Index

        public JsonResult Login(string USER, string PASS)
        {
            ACTIVE_DIRECTION_Models result = CheckLogin(USER, PASS);

            if (result.USER_NAME != string.Empty)
            {
                Session["USER_NAME"] = result.USER_NAME;
                Session["FULL_NAME"] = result.FULL_NAME;

                return Json(new { Result = "OK", Message = "Login Success!", USER_NAME = result.USER_NAME, FULL_NAME = result.FULL_NAME });

            }
            else
            {
                Session["USER_NAME"] = string.Empty;
                Session["FULL_NAME"] = string.Empty;

                return Json(new { Result = "ERROR", Message = "Username, Password is incorrect!", EN = string.Empty, FULL_NAME = string.Empty });

            } // end if: result is not null

        } // end JsonResult: Login

        public ACTIVE_DIRECTION_Models CheckLogin(string USER, string PASS)
        {
            ACTIVE_DIRECTION_Models result = new ACTIVE_DIRECTION_Models();

            string domain = "asia.ad.celestica.com";

            try
            {
                string tmpUSER = USER.ToUpper();

                if (tmpUSER.Contains(@"ASIA\"))
                {
                    USER = USER.ToUpper().Replace(@"ASIA\", "");

                } // end if: USER have back slash

                tmpUSER = domain + @"\" + USER;

                DirectoryEntry adsRoot = new DirectoryEntry();

                adsRoot.AuthenticationType = AuthenticationTypes.Secure;
                adsRoot.Path = "LDAP://" + domain;
                adsRoot.Username = domain + @"\" + USER;
                adsRoot.Password = PASS;

                DirectorySearcher adsSearch = new DirectorySearcher(adsRoot);
                DirectoryEntry userDE;
                adsSearch.PropertiesToLoad.Add("sAMAccountName");
                //adsSearch.PropertiesToLoad.Add("memberof");
                //adsSearch.PropertiesToLoad.Add("cn");
                adsSearch.PropertiesToLoad.Add("FullName");
                //adsSearch.PropertiesToLoad.Add("email");

                adsSearch.Filter = "(&(sAMAccountName=" + USER + ")(!userAccountControl:1.2.840.113556.1.4.803:=2))";

                SearchResult oResult;
                oResult = adsSearch.FindOne();

                if (oResult != null)
                {
                    userDE = oResult.GetDirectoryEntry();
                    if (userDE == null) { throw new Exception("Username, Password is incorrect!"); }
                    if (!string.IsNullOrEmpty(userDE.Name) && userDE.Name.IndexOf(":") >= 0)
                    {
                        string[] ArrStr = userDE.Name.Split(':');

                        if (ArrStr.Length >= 2)
                        {
                            string USER_NAME = ArrStr[1].ToString().Substring(2);
                            string FULL_NAME = ArrStr[0].ToString().Substring(3);

                            result.USER_NAME = USER_NAME;
                            result.FULL_NAME = FULL_NAME;

                        }
                        else
                        {
                            throw new Exception("Username, Password is incorrect!");

                        } // end if: ArrStr more than 1

                    } // end if: 

                } // end if: oResult is not null

            } // end try:

            catch (Exception)
            {
                result.USER_NAME = string.Empty;
                result.FULL_NAME = string.Empty;

            } // end catch: Exception

            return result;

        } // end ACTIVE_DIRECTION_Models: CheckLogin

        public ActionResult Logout()
        {
            Session["USER_NAME"] = null;
            Session["FULL_NAME"] = null;

            return RedirectToAction("Index", "Login");

        } // end ActionResult: Logout

    } // end class: LoginController

} // end namespace: ScrapDSBMaster