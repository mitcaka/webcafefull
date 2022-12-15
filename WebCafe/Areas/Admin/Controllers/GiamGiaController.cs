using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.App_Start;
using WebCafe.Models;

namespace WebCafe.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class GiamGiaController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/GiamGia
        public ActionResult Index()
        {
            List<WebCafe.Models.GiamGia> giamgias = db.GiamGias.ToList();
            return View(giamgias);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemGiamGia(GiamGia gg)
        {
            WebCafe.Models.GiamGia giamGia = new Models.GiamGia();
            giamGia.MaGiamGia = gg.MaGiamGia;
            giamGia.TenMa = gg.TenMa;
            giamGia.GiaTri = gg.GiaTri;
            giamGia.active = gg.active;
            db.GiamGias.Add(giamGia);
            db.SaveChanges();
            TempData["Success"] = "Thêm mã giảm giá thành công!!";

            return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "GiamGia",
                            action = "Index",
                            area = "Admin"
                        }));
        }


        public ActionResult XoaGiamGia(string id)
        {
            WebCafe.Models.GiamGia gg = db.GiamGias.Where(m => m.MaGiamGia == id).FirstOrDefault();
            db.GiamGias.Remove(gg);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "GiamGia",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult SuaGiamGia(string id)
        {
            GiamGia gg = db.GiamGias.FirstOrDefault(x => x.MaGiamGia == id);
            return View(gg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaGiamGia(GiamGia gg)
        {
            GiamGia giamGia = db.GiamGias.FirstOrDefault(x => x.MaGiamGia == gg.MaGiamGia);
            giamGia.MaGiamGia = gg.MaGiamGia;
            giamGia.TenMa = gg.TenMa;
            giamGia.GiaTri = gg.GiaTri;
            giamGia.active = gg.active;
            db.SaveChanges();
            TempData["Success"] = "Sửa mã giảm giá thành công!!";

            return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "GiamGia",
                            action = "Index",
                            area = "Admin"
                        }));
        }
    }
}