using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCafe.App_Start;
using WebCafe.Models;

namespace WebCafe.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class QLDonHangController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/QLDonHang
        public ActionResult Index()
        {
            List<WebCafe.Models.DonDatHang> donDatHangs = db.DonDatHangs.ToList();
            ViewBag.TitleAction = "Danh sách đơn hàng";
            return View(donDatHangs);
        }

        [HttpPost]
        public ActionResult XuLyDonHang(int MaDH,string TinhTrang)
        {
            WebCafe.Models.DonDatHang dh = db.DonDatHangs.Where(m => m.MaDDH == MaDH).FirstOrDefault();
            dh.TinhTrangGiao = TinhTrang;
            db.SaveChanges();

            foreach(ChiTietDonHang ct in dh.ChiTietDonHangs)
            {
                SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSP == ct.MaSP);
                if(sp.SoLuongTon >= ct.SoLuong)
                {
                    sp.SoLuongTon -= ct.SoLuong;
                    db.SaveChanges();
                }
                else
                {
                    TempData["Error"] = "Hết hàng trong kho!!!";
                }
               
            }
            TempData["Success"] = "Xử lý đơn hàng thành công";
            return RedirectToAction("Index");
        }

        
    }
}