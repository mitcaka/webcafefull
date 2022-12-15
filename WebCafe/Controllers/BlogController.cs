using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.App_Start;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        public ActionResult Index()
        {
            List<WebCafe.Models.TinTuc> tintucs = db.TinTucs.ToList();
            ViewBag.DanhMuc = db.DanhMucs.ToList();
            return View(tintucs);
        }

        public ActionResult BlogDetail(int id)
        {
            List<TinTuc> tintucss = db.TinTucs.OrderByDescending(row => row.NgayViet).Take(3).ToList();
            ViewBag.BaiVietMoi = tintucss;
            TinTuc tt = db.TinTucs.FirstOrDefault(x => x.MaTin == id);
            return View(tt);
        }
    }
}