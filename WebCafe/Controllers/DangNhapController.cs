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
    public class DangNhapController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();

        // GET: DangNhap
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhapThanhVien(string TaiKhoan,string MatKhau)
        {
            var f_matkhau = GetMD5(MatKhau);
            var data = db.ThanhViens.Where(s => s.TaiKhoan.Equals(TaiKhoan) && s.MatKhau.Equals(f_matkhau)).ToList();
            if (data.Count() > 0)
            {
                //add session
                Session["HoTenTV"] = data.FirstOrDefault().HoTen;
                Session["TaiKhoanTV"] = data.FirstOrDefault().TaiKhoan;
                return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "TrangChu",
                        action = "Index",
                    }));
            }
            else
            {
                TempData["Error"] = "Thông tin tài khoản hoặc mật khẩu không đúng!";
                return RedirectToAction("Index");
            }
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

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
    }
}