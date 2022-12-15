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
    public class NhaCungCapController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/NhaCungCap
        public ActionResult Index()
        {
            List<WebCafe.Models.NhaCungCap> nhacungcaps = db.NhaCungCaps.ToList();
            return View(nhacungcaps);
        }

        [HttpPost]
        public ActionResult ThemNhaCungCap( string TenNCC, string DiaChi, string Email, string DienThoai, string Fax)
        {
                WebCafe.Models.NhaCungCap ncc = new Models.NhaCungCap();
                ncc.TenNCC = TenNCC;
                ncc.DiaChi = DiaChi;
                ncc.Email = Email;
                ncc.DienThoai = DienThoai;
                ncc.Fax = Fax;
                db.NhaCungCaps.Add(ncc);
                db.SaveChanges();
                TempData["Success"] = "Thêm nhà cung cấp thành công!!";
                return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "NhaCungCap",
                            action = "Index",
                            area = "Admin"
                        }));
            
        }


        public ActionResult XoaNhaCungCap(int id)
        {
            WebCafe.Models.NhaCungCap ncc = db.NhaCungCaps.Where(m => m.MaNCC == id).FirstOrDefault();
            db.NhaCungCaps.Remove(ncc);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "NhaCungCap",
                        action = "Index",
                        area = "Admin"
                    }));
        }



        public ActionResult SuaNhaCungCap(int id)
        {
            WebCafe.Models.NhaCungCap ncc = db.NhaCungCaps.Where(m => m.MaNCC == id).FirstOrDefault();
            return View(ncc);
        }

        [HttpPost]
        public ActionResult SuaNhaCungCap(int MaNCC, string TenNCC, string DiaChi, string Email, string DienThoai, string Fax)
        {
            WebCafe.Models.NhaCungCap ncc = db.NhaCungCaps.Where(m => m.MaNCC == MaNCC).FirstOrDefault();
            ncc.TenNCC = TenNCC;
            ncc.DiaChi = DiaChi;
            ncc.Email = Email;
            ncc.DienThoai = DienThoai;
            ncc.Fax = Fax;
            db.SaveChanges();
            TempData["Success"] = "Sửa nhà cung cấp thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "NhaCungCap",
                        action = "Index",
                        area = "Admin"
                    }));
        }

    }
}