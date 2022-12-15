using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.App_Start;
using WebCafe.Models;

namespace WebCafe.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class QLKhachHangController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/QLKhachHang
        public ActionResult Index()
        {
            List<WebCafe.Models.ThanhVien> thanhViens = db.ThanhViens.ToList();
            return View(thanhViens);
        }

        [HttpPost]
        public ActionResult ThemThanhVien(string TaiKhoan, string MatKhau, string HoTen, string DiaChi, string Email, string DienThoai)
        {
            //Kiểm tra mã tK
            WebCafe.Models.ThanhVien tvm = db.ThanhViens.Where(m => m.TaiKhoan == TaiKhoan).FirstOrDefault();
            if (tvm == null)
            {
                WebCafe.Models.ThanhVien tv = new Models.ThanhVien();
                tv.TaiKhoan = TaiKhoan;
                tv.MatKhau = GetMD5(MatKhau);
                tv.HoTen = HoTen;
                tv.DiaChi = DiaChi;
                tv.Email = Email;
                tv.DienThoai = DienThoai;
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                TempData["Success"] = "Thêm tài khoản khách hàng thành công!!";
                return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "QLKhachHang",
                            action = "Index",
                            area = "Admin"
                        }));
            }

            TempData["Error"] = "Tài khoản khách hàng thêm không hợp lệ!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "QLKhachHang",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult XoaThanhVien(string id)
        {
            WebCafe.Models.ThanhVien tv = db.ThanhViens.Where(m => m.TaiKhoan == id).FirstOrDefault();
            db.ThanhViens.Remove(tv);
            db.SaveChanges();
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "QLKhachHang",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public ActionResult SuaThanhVien(string id)
        {
            WebCafe.Models.ThanhVien tv = db.ThanhViens.Where(m => m.TaiKhoan == id).FirstOrDefault();
            return View(tv);
        }

        [HttpPost]
        public ActionResult SuaThanhVien(string TaiKhoan, string MatKhau, string HoTen, string DiaChi, string Email, string DienThoai)
        {
            WebCafe.Models.ThanhVien tv = db.ThanhViens.Where(m => m.TaiKhoan == TaiKhoan).FirstOrDefault();
            tv.MatKhau = GetMD5(MatKhau);
            tv.HoTen = HoTen;
            tv.Email = Email;
            tv.DiaChi = DiaChi;
            tv.DienThoai = DienThoai;
            db.SaveChanges();
            TempData["Success"] = "Sửa khách hàng thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "QLKhachHang",
                        action = "Index",
                        area = "Admin"
                    }));
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}