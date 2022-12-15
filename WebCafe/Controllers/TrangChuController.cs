using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class TrangChuController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();

        // GET: TrangChu
        public ActionResult Index()
        {
            List<SanPham> sanPhamNoiBat = db.SanPhams.OrderByDescending(row => row.LuotMua).Where(x => x.DaXoa == true).Take(5).ToList();
            ViewBag.SanPhamNoiBat = sanPhamNoiBat;
            List<SanPham> sanPhamMoi = db.SanPhams.OrderByDescending(row => row.NgayCapNhat).Where(x => x.DaXoa == true).Take(5).ToList();
            ViewBag.SanPhamMoi = sanPhamMoi;
            return View();
        }
    }
}