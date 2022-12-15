using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class DangKyController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();

        // GET: DangKy
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKyThanhVien(string TaiKhoan, string MatKhau, string HoTen, string DiaChi, string Email, string DienThoai)
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
                TempData["Success"] = "Đăng Kí Thành Công!!";
                return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "DangNhap",
                            action = "Index",
                        }));
            }

            TempData["Error"] = "Tài Khoản Đã Tồn Tại!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "DangKy",
                        action = "Index",
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