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
    public class QLSanPhamController : Controller
    {
        // GET: Admin/SanPham
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        public ActionResult Index()
        {
            List<WebCafe.Models.SanPham> SanPhams = db.SanPhams.ToList();
            ViewBag.TitleAction = "Danh sách sản phẩm";
            return View(SanPhams);
        }
        [RootAdminAuthorize]
        public ActionResult ThemMoi()
        {
            ViewBag.LoaiSanPham = db.LoaiSanPhams.ToList();
            int i = 0;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(SanPham sp ,HttpPostedFileBase hinhAnh)
        {
            DateTime now = DateTime.Now;
            WebCafe.Models.SanPham sanPham = new Models.SanPham();
            sanPham.TenSP = sp.TenSP;
            sanPham.DonGia = sp.DonGia;
            sanPham.NgayCapNhat = now;
            sanPham.Mota = sp.Mota;
            sanPham.SoLuongTon = sp.SoLuongTon;
            sanPham.LuotMua = 0;
            sanPham.LuotXem = 0;
            sanPham.NguonGoc = sp.NguonGoc;
            sanPham.KhoiLuong = sp.KhoiLuong;
            sanPham.MaLoai = sp.MaLoai;
            
            if (sp.DaXoa == null)
            {
                sanPham.DaXoa = false;
            }
            else
            {
                sanPham.DaXoa = sp.DaXoa;
            }
            sanPham.Url_friendly = sp.Url_friendly;
            sanPham.HinhAnh = "";
            db.SanPhams.Add(sanPham);
            db.SaveChanges();
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
            {
                int id = sanPham.MaSP;

                string _FileName = "";
                int index = hinhAnh.FileName.IndexOf('.');
                _FileName = "sanpham" + id.ToString() + "." + hinhAnh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Upload/sanpham"), _FileName);
                hinhAnh.SaveAs(_path);

                SanPham unv = db.SanPhams.FirstOrDefault(x => x.MaSP == id);
                unv.HinhAnh = _FileName;
                db.SaveChanges();
            }
            TempData["Success"] = "Thêm sản phẩm thành công!!";

            return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "QLSanPham",
                            action = "Index",
                            area = "Admin"
                        }));
        }

        public ActionResult SuaSanPham(int id)
        {
            SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSP == id);
            ViewBag.LoaiSanPham = db.LoaiSanPhams.ToList();

            return View(sp);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSanPham(SanPham sp, HttpPostedFileBase hinhAnh)
        {
            DateTime now = DateTime.Now;
            SanPham sanPham = db.SanPhams.FirstOrDefault(x => x.MaSP == sp.MaSP);
            sanPham.TenSP = sp.TenSP;
            sanPham.DonGia = sp.DonGia;
            sanPham.NgayCapNhat = now;
            sanPham.Mota = sp.Mota;
            sanPham.SoLuongTon = sp.SoLuongTon;
            sanPham.LuotMua = sp.LuotMua;
            sanPham.LuotXem = sp.LuotXem;
            sanPham.NguonGoc = sp.NguonGoc;
            sanPham.KhoiLuong = sp.KhoiLuong;
            sanPham.MaLoai = sp.MaLoai;
            if (sp.DaXoa == null)
            {
                sanPham.DaXoa = false;
            }
            else
            {
                sanPham.DaXoa = sp.DaXoa;
            }
            sanPham.Url_friendly = sp.Url_friendly;
            sanPham.HinhAnh = "";
            if (hinhAnh != null && hinhAnh.ContentLength > 0)
            {
                int id = sanPham.MaSP;

                string _FileName = "";
                int index = hinhAnh.FileName.IndexOf('.');
                _FileName = "sanpham" + id.ToString() + "." + hinhAnh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/Upload/sanpham"), _FileName);
                hinhAnh.SaveAs(_path);

                SanPham unv = db.SanPhams.FirstOrDefault(x => x.MaSP == id);
                unv.HinhAnh = _FileName;
            }
            db.SaveChanges();

            TempData["Success"] = "Sửa sản phẩm thành công!!";

            return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "QLSanPham",
                            action = "Index",
                            area = "Admin"
                        }));
        }

        public ActionResult XoaSanPham(int id)
        {
            WebCafe.Models.SanPham ncc = db.SanPhams.Where(m => m.MaSP == id).FirstOrDefault();
            db.SanPhams.Remove(ncc);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "QLSanPham",
                        action = "Index",
                        area = "Admin"
                    }));
        }
    }
}