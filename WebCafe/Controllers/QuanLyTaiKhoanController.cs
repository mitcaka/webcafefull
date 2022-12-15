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
    public class QuanLyTaiKhoanController : Controller
    {
        // GET: QuanLyTaiKhoan
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CaiDat()
        {
            if(Session["TaiKhoanTV"]!= null){
                string ma = (string)Session["TaiKhoanTV"];
                WebCafe.Models.ThanhVien tv = db.ThanhViens.Where(m => m.TaiKhoan ==ma) .FirstOrDefault();
                return View(tv);
            }
            return new RedirectToRouteResult(new
                         RouteValueDictionary(
                         new
                         {
                             controller = "Warning",
                             action = "Index"
                         }));
        }

        public ActionResult DonHang()
        {
            string ma = (string)Session["TaiKhoanTV"];
            List<WebCafe.Models.KhachHang> khachHangs = db.KhachHangs.Where(x => x.TaiKhoan == ma).ToList();

            List<WebCafe.Models.DonDatHang> DonHangXuLy = new List<WebCafe.Models.DonDatHang>();
            List<WebCafe.Models.DonDatHang> DonHangVanChuyen = new List<WebCafe.Models.DonDatHang>();

            foreach (KhachHang item in khachHangs)
            {
                WebCafe.Models.DonDatHang ddh = db.DonDatHangs.Where(x => x.MaKH == item.MaKH).FirstOrDefault();
                if(ddh.TinhTrangGiao == "Đang xử lý đơn hàng")
                {
                    DonHangXuLy.Add(ddh);
                }
                else
                {
                    DonHangVanChuyen.Add(ddh);
                }
                
            }

            ViewBag.DonHangXuLy = DonHangXuLy;
            ViewBag.DonHangVanChuyen = DonHangVanChuyen;
            return View();
        }

        public ActionResult SuaTaiKhoanTV()
        {
            if (Session["TaiKhoanTV"] != null)
            {
                string ma = (string)Session["TaiKhoanTV"];
                WebCafe.Models.ThanhVien tv = db.ThanhViens.Where(m => m.TaiKhoan == ma).FirstOrDefault();
                return View(tv);
            }
            return new RedirectToRouteResult(new
                         RouteValueDictionary(
                         new
                         {
                             controller = "Warning",
                             action = "Index"
                         }));
        }

        [HttpPost]
        public ActionResult SuaTaiKhoanTV(string TaiKhoan ,string HoTen,string DiaChi,string Email,string DienThoai, string isChangePass,string MatKhauCu, string MatKhauMoi)
        {
            
                string ma = (string)Session["TaiKhoanTV"];
                WebCafe.Models.ThanhVien tv = db.ThanhViens.Where(m => m.TaiKhoan == ma).FirstOrDefault();
                tv.DiaChi = DiaChi;
                tv.DienThoai = DienThoai;
                tv.Email = Email;
                tv.HoTen = HoTen;
            if (isChangePass == "DoiMK")
            {
                string mkcu = GetMD5(MatKhauCu);
                if (GetMD5(tv.MatKhau).Equals(mkcu))
                {
                    TempData["Error"] = "Mật khẩu cũ không đúng!!";
                    return new RedirectToRouteResult(new
                                 RouteValueDictionary(
                                 new
                                 {
                                     controller = "QuanLyTaiKhoan",
                                     action = "SuaTaiKhoanTV"
                                 }));
                }
                else
                {
                    tv.MatKhau = GetMD5(MatKhauMoi);
                    db.SaveChanges();
                    TempData["Success"] = "Sửa tài khoản thành công!!";
                    return new RedirectToRouteResult(new
                                 RouteValueDictionary(
                                 new
                                 {
                                     controller = "QuanLyTaiKhoan",
                                     action = "CaiDat"
                                 }));
                }
            }
            else
            {
                db.SaveChanges();
                TempData["Success"] = "Sửa tài khoản thành công!!";
                return new RedirectToRouteResult(new
                             RouteValueDictionary(
                             new
                             {
                                 controller = "QuanLyTaiKhoan",
                                 action = "CaiDat"
                             }));
            }

            TempData["Error"] = "Sửa tài khoản thất bại!!";
            return new RedirectToRouteResult(new
                         RouteValueDictionary(
                         new
                         {
                             controller = "QuanLyTaiKhoan",
                             action = "SuaTaiKhoanTV"
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