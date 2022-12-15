using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.Models;

namespace WebCafe.Areas.Admin.Controllers
{
    public class QLNhapHangController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        // GET: Admin/QLNhapHang
        public ActionResult Index()
        {
            List<WebCafe.Models.PhieuNhap> phieuNhaps = db.PhieuNhaps.ToList();
            ViewBag.TitleAction = "Danh sách đơn nhập";
            return View(phieuNhaps);
        }

        public ActionResult TaoPhieuNhap()
        {
            ViewBag.TitleAction = "Tạo mới phiếu nhập";
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }

        [HttpPost]
        public ActionResult TaoPhieuNhap(PhieuNhap model ,IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.TitleAction = "Tạo mới phiếu nhập";
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            model.DaXoa = "Chưa";
            model.NgayNhap = DateTime.Now;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            SanPham sp;
            foreach(var item in lstModel)
            {
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XoaPhieuNhap(int id)
        {
            WebCafe.Models.PhieuNhap pn = db.PhieuNhaps.Where(m => m.MaPN == id).FirstOrDefault();
            DateTime ngayNhap = pn.NgayNhap??DateTime.Now;
            System.TimeSpan d = DateTime.Now.Subtract(ngayNhap);
            double days = d.TotalDays;
            if(days > 3)
            {
                TempData["Error"] = "Phiếu nhập quá 3 ngày không thể xóa!!";
                return new RedirectToRouteResult(new
                        RouteValueDictionary(
                        new
                        {
                            controller = "QLNhapHang",
                            action = "Index",
                            area = "Admin"
                        }));
            }
            SanPham sp;
            foreach (var item in pn.ChiTietPhieuNhaps)
            {
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon -= item.SoLuongNhap;
            }
            db.ChiTietPhieuNhaps.RemoveRange(pn.ChiTietPhieuNhaps);
            db.PhieuNhaps.Remove(pn);
            db.SaveChanges();
            TempData["Success"] = "Xóa thành công!!";
            return new RedirectToRouteResult(new
                    RouteValueDictionary(
                    new
                    {
                        controller = "QLNhapHang",
                        action = "Index",
                        area = "Admin"
                    }));
        }
    }
}