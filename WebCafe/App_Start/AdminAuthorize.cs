using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.Models;

namespace WebCafe.App_Start
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //check session
            var adSession = HttpContext.Current.Session["TaiKhoan"];
            if(adSession != null)
            {
                return;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "Home",
                        action = "Login",
                        area = "Admin"
                    }));
            }
        }
    }
}