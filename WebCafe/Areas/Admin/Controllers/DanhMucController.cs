using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.App_Start;
using WebCafe.Models;

namespace WebCafe.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DanhMucController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/DanhMuc
        public ActionResult Index()
        {
            List<WebCafe.Models.DanhMuc> danhmucs = db.DanhMucs.ToList();
            return View(danhmucs);
        }

        public ActionResult ThemDanhMuc(string TenDanhMuc, string Url_friendly)
        {
            WebCafe.Models.DanhMuc dm = new Models.DanhMuc();
            dm.TenDanhMuc = TenDanhMuc;
            dm.Url_friendly = Url_friendly;
            db.DanhMucs.Add(dm);
            db.SaveChanges();
            TempData["Success"] = "Thêm danh mục thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "DanhMuc",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult XoaDanhMuc(int id)
        {
            WebCafe.Models.DanhMuc dm = db.DanhMucs.Where(m => m.MaDanhMuc == id).FirstOrDefault();
            db.DanhMucs.Remove(dm);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "DanhMuc",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult SuaDanhMuc(int id)
        {
            WebCafe.Models.DanhMuc dm = db.DanhMucs.Where(m => m.MaDanhMuc == id).FirstOrDefault();
            return View(dm);
        }

        [HttpPost]
        public ActionResult SuaDanhMuc(int MaDanhMuc, string TenDanhMuc, string Url_friendly)
        {
            WebCafe.Models.DanhMuc dm = db.DanhMucs.Where(m => m.MaDanhMuc == MaDanhMuc).FirstOrDefault();
            dm.TenDanhMuc = TenDanhMuc;
            dm.Url_friendly = Url_friendly;
            db.SaveChanges();
            TempData["Success"] = "Sửa danh mục thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "DanhMuc",
                        action = "Index",
                        area = "Admin"
                    }));
        }
    }
}