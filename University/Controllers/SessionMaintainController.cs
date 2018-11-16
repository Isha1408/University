using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace University.Controllers
{
    public class SessionMaintainController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // if (HttpContext.Current.Session["userInfo"] == null)
            if (HttpContext.Current.Session["User"]==null
                && HttpContext.Current.Session["UserId"]==null
                && HttpContext.Current.Session["UserName"]==null)
            {
               
                filterContext.Result = new RedirectResult("~/UserView/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}