using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.Models;

namespace WebCafe.App_Start
{
    public class RootAdminAuthorize : AdminAuthorize
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //check session
            var adSession = HttpContext.Current.Session["TaiKhoan"];
            var data = db.Admins.Count(s => s.TaiKhoan == adSession & s.Role == "RootAdmin");
            if (data != 0)
            {
                return;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "Error",
                        action = "Index",
                        area = "Admin"
                    }));
            }
        }
    }
}