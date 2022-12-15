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
    public class LoaiSanPhamController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();

        // GET: Admin/LoaiSanPham
        public ActionResult Index()
        {
            List<WebCafe.Models.LoaiSanPham> loaiSanPhams = db.LoaiSanPhams.ToList();
            ViewBag.TitleAction = "Danh sách loại sản phẩm";
            return View(loaiSanPhams);
        }

        [HttpPost]
        public ActionResult ThemLoaiSanPham(LoaiSanPham loai, HttpPostedFileBase hinhAnh)
        {
            db.LoaiSanPhams.Add(loai);
            db.SaveChanges();
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
            {
                int id =loai.MaLoai;

                string _FileName = "";
                int index = hinhAnh.FileName.IndexOf('.');
                _FileName = "loai" + id.ToString() + "." + hinhAnh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Upload/loaisp"), _FileName);
                hinhAnh.SaveAs(_path);

                LoaiSanPham unv = db.LoaiSanPhams.FirstOrDefault(x => x.MaLoai == id);
                unv.HinhAnh = _FileName;
                db.SaveChanges();
            }
            TempData["Success"] = "Thêm loại sản phẩm thành công!!";

            return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "LoaiSanPham",
                            action = "Index",
                            area = "Admin"
                        }));
        }

        public ActionResult SuaLoai(int id)
        {
            LoaiSanPham loai = db.LoaiSanPhams.FirstOrDefault(x => x.MaLoai == id);
            return View(loai);
        }

        [HttpPost]
        public ActionResult SuaLoai(LoaiSanPham loai, HttpPostedFileBase hinhAnh)
        {
            LoaiSanPham ul = db.LoaiSanPhams.FirstOrDefault(x => x.MaLoai == loai.MaLoai);
            ul.TenLoai = loai.TenLoai;
            ul.Url_friendly = loai.Url_friendly;
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
            {
                int id = loai.MaLoai;

                string _FileName = "";
                int index = hinhAnh.FileName.IndexOf('.');
                _FileName = "loai" + id.ToString() + "." + hinhAnh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Upload/loaisp"), _FileName);
                hinhAnh.SaveAs(_path);

                LoaiSanPham unv = db.LoaiSanPhams.FirstOrDefault(x => x.MaLoai == id);
                ul.HinhAnh = _FileName;
            }
            db.SaveChanges();
            TempData["Success"] = "Sửa loại sản phẩm thành công!!";
            return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "LoaiSanPham",
                            action = "Index",
                            area = "Admin"
                        }));
        }

        public ActionResult XoaLoai(int id)
        {
            WebCafe.Models.LoaiSanPham ncc = db.LoaiSanPhams.Where(m => m.MaLoai == id).FirstOrDefault();
            db.LoaiSanPhams.Remove(ncc);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "LoaiSanPham",
                        action = "Index",
                        area = "Admin"
                    }));
        }
    }
}