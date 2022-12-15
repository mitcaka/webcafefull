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
    public class TinTucController : Controller
    {
        // GET: Admin/TinTuc
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        public ActionResult Index()
        {
            List<WebCafe.Models.TinTuc> tintucs = db.TinTucs.ToList();
            ViewBag.DanhMuc = db.DanhMucs.ToList();
            return View(tintucs);
        }

        public ActionResult ThemTinTuc()
        {
            ViewBag.DanhMuc = db.DanhMucs.ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemTinTuc(int MaDanhMuc,string TieuDe, string NoiDung,string slug, HttpPostedFileBase hinhAnh)
        {
            DateTime now = DateTime.Now;
            WebCafe.Models.TinTuc tinTuc = new Models.TinTuc();
            tinTuc.MaDanhMuc = MaDanhMuc;
            tinTuc.TieuDe = TieuDe;
            tinTuc.NoiDung = NoiDung;
            tinTuc.Url_friendly = slug;
            tinTuc.NgayViet = now;
            db.TinTucs.Add(tinTuc);
            db.SaveChanges();
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
            {
                int id = tinTuc.MaTin;

                string _FileName = "";
                int index = hinhAnh.FileName.IndexOf('.');
                _FileName = "tintuc" + id.ToString() + "." + hinhAnh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Upload/tintuc"), _FileName);
                hinhAnh.SaveAs(_path);

                TinTuc unv = db.TinTucs.FirstOrDefault(x => x.MaTin == id);
                unv.HinhAnh = _FileName;
                db.SaveChanges();
            }
            TempData["Success"] = "Thêm bài viết thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "TinTuc",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult SuaTinTuc(int id)
        {
            WebCafe.Models.TinTuc tt = db.TinTucs.Where(m => m.MaTin == id).FirstOrDefault();
            ViewBag.DanhMuc = db.DanhMucs.ToList();
            return View(tt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaTinTuc(int MaDanhMuc, string TieuDe, string NoiDung, string slug)
        {
            DateTime now = DateTime.Now;
            WebCafe.Models.TinTuc tt = db.TinTucs.Where(m => m.MaTin == MaDanhMuc).FirstOrDefault();
            tt.MaDanhMuc = MaDanhMuc;
            tt.TieuDe = TieuDe;
            //tt.NgayViet = now;
            tt.NoiDung = NoiDung;
            tt.Url_friendly = slug;
            db.SaveChanges();
            TempData["Success"] = "Sửa bài đăng thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "TinTuc",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult XoaTinTuc(int id)
        {
            WebCafe.Models.TinTuc tt = db.TinTucs.Where(m => m.MaTin == id).FirstOrDefault();
            db.TinTucs.Remove(tt);
            db.SaveChanges();
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "TinTuc",
                        action = "Index",
                        area = "Admin"
                    }));
        }
    }
}