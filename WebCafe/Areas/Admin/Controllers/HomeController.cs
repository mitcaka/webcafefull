using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebCafe.App_Start;
using WebCafe.Models;


namespace WebCafe.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/Home

        //[AdminAuthorize]
        public ActionResult Index()
        {
            //List new order
            List<WebCafe.Models.DonDatHang> donDatHangs = db.DonDatHangs.OrderByDescending(row => row.NgayDat).Take(5).ToList();
            //Đơn hàng chưa xử lý
            List<WebCafe.Models.DonDatHang> donDatHangChuaXuLy = db.DonDatHangs.Where(row => row.TinhTrangGiao == "Đang xử lý đơn hàng").ToList();
            ViewBag.ChuaXuLy = donDatHangChuaXuLy.Count();
            //Tổng số đơn hàng
            List<WebCafe.Models.DonDatHang> tongDonDatHangs = db.DonDatHangs.ToList();
            ViewBag.SoDonHang = tongDonDatHangs.Count();
            //Tính doanh thu
            List<WebCafe.Models.DonDatHang> donDatHangThanhCongs = db.DonDatHangs.Where(row => row.NgayGiao != null).ToList();
            double? DoanhThu = 0;
            foreach(DonDatHang ddh in donDatHangThanhCongs)
            {
                foreach(ChiTietDonHang ct in ddh.ChiTietDonHangs)
                {
                    DoanhThu += ct.SoLuong * ct.DonGia;
                }
            }
            ViewBag.DoanhThu = DoanhThu;

            //Xử lý biểu đồ

            List<String> thang = new List<String>();
            List<double> tienNhap = new List<double>();
            List<double> tienLai = new List<double>();

            List<DateTime> last12Months = new List<DateTime>();
            DateTime currentDate = DateTime.Now;

            for (int i = 0; i < 12; i++)
            {
                last12Months.Add(currentDate);
                currentDate = currentDate.AddMonths(-1);
            }
            double? DoanhThuThangi = 0;
            double? TienNhapThangi = 0;
            for (int i = 10; i >= 0; i--)
            {
                DateTime startDate = last12Months[i+1];
                DateTime endDate = last12Months[i];
                List<WebCafe.Models.DonDatHang> doanhThuThangs = db.DonDatHangs.Where(row => row.NgayGiao != null &&
                     row.NgayGiao >= startDate && row.NgayGiao <= endDate).ToList();
                
                foreach (DonDatHang ddh in doanhThuThangs)
                {
                    foreach (ChiTietDonHang ct in ddh.ChiTietDonHangs)
                    {
                        DoanhThuThangi += ct.SoLuong * ct.DonGia;
                    }
                }

                List<WebCafe.Models.PhieuNhap> tienNhaps = db.PhieuNhaps.Where(row => row.NgayNhap.HasValue &&
                     row.NgayNhap >= startDate && row.NgayNhap <= endDate).ToList();
                
                foreach (PhieuNhap pn in tienNhaps)
                {
                    foreach (ChiTietPhieuNhap ct in pn.ChiTietPhieuNhaps)
                    {
                        TienNhapThangi += ct.SoLuongNhap * ct.DonGiaNhap;
                    }
                }
                thang.Add(last12Months[i].ToString("MM/yyyy"));
                tienNhap.Add((double)TienNhapThangi);
                tienLai.Add((double)DoanhThuThangi - (double)TienNhapThangi);
                DoanhThuThangi = 0;
                TienNhapThangi = 0;
            }

            ViewBag.Thang = thang;
            ViewBag.TienNhap = tienNhap;
            ViewBag.TienLai= tienLai;

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