using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaBatch.Filter
{
    public class Auth : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["USER_NAME"] == null)
            {
                if (HttpContext.Current.Request.Url != null)
                {
                    HttpContext.Current.Session["Redirect"] = HttpContext.Current.Request.Url;
                }
                string loginpath = "~/Login/Index";
                filterContext.Result = new RedirectResult(loginpath);
            }
        }

    } // end class: Auth

} // end namespace: ScrapDSBMaster.Filter