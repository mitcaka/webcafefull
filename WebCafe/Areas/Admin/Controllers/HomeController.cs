using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebCafe.Models;
using WebCafe.App_Start;

namespace WebCafe.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/Home

        [AdminAuthorize]
        public ActionResult Index()
        {
            List<WebCafe.Models.DonDatHang> donDatHangs = db.DonDatHangs.OrderByDescending(row => row.NgayDat).Take(5).ToList();
            List<WebCafe.Models.DonDatHang> donDatHangChuaXuLy = db.DonDatHangs.Where(row => row.TinhTrangGiao == "Đang xử lý đơn hàng").ToList();
            ViewBag.ChuaXuLy = donDatHangChuaXuLy.Count();
            return View(donDatHangs);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(AdminViewModel avm)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(avm.MatKhau);
                var data = db.Admins.Where(s => s.TaiKhoan.Equals(avm.TaiKhoan) && s.MatKhau.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["HoTen"] = data.FirstOrDefault().HoTen;
                    Session["TaiKhoan"] = data.FirstOrDefault().TaiKhoan;
                    Session["Role"] = data.FirstOrDefault().Role;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Thông tin tài khoản hoặc mật khẩu không đúng!";
                    return RedirectToAction("Login");
                }
            }
            TempData["Error"] = "Đăng nhập thất bại!!!";
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
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