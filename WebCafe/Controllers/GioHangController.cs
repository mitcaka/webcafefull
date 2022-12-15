using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebCafe.Models;

namespace WebCafe.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyCaPheEntities db = new QuanLyCaPheEntities();
        private double giaTri;
        int i = 0;
        // GET: GioHang
        public ActionResult Index()
        {
            if(giaTri != null)
            {
                ViewBag.GiaTri = giaTri;
            }
            return View((List<CartModel>)Session["cart"]);
        }

        public ActionResult AddToCart(int id, int quantity)
        {
            if (Session["cart"] == null)
            {
                List<CartModel> cart = new List<CartModel>();
                cart.Add(new CartModel { SanPham = db.SanPhams.Find(id), Quantity = quantity });
                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                List<CartModel> cart = (List<CartModel>)Session["cart"];
                //kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa???
                int index = isExist(id);
                if (index != -1)
                {
                    //nếu sp tồn tại trong giỏ hàng thì cộng thêm số lượng
                    cart[index].Quantity += quantity;
                }
                else
                {
                    //nếu không tồn tại thì thêm sản phẩm vào giỏ hàng
                    cart.Add(new CartModel { SanPham = db.SanPhams.Find(id), Quantity = quantity });
                    //Tính lại số sản phẩm trong giỏ hàng
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        public ActionResult UpdateCart(List<int> id,List<int> quantity)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for(int i = 0; i < id.Count; i ++)
            {
                if(id[i] == cart[i].SanPham.MaSP)
                {
                    cart[i].Quantity = quantity[i];
                }
            }
            Session["cart"] = cart;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        private int isExist(int id)
        {
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].SanPham.MaSP.Equals(id))
                    return i;
            return -1;
        }

        //xóa sản phẩm khỏi giỏ hàng theo id
        public ActionResult Remove(int Id)
        {
            List<CartModel> li = (List<CartModel>)Session["cart"];
            li.RemoveAll(x => x.SanPham.MaSP == Id);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        public ActionResult AddCoupon(string Ma)
        {
            List<WebCafe.Models.GiamGia> giamGias = db.GiamGias.Where(x => x.active == true).ToList();
            if(Session["TaiKhoanTV"] != null)
            {
                foreach (GiamGia item in giamGias)
                {
                    if(item.MaGiamGia == Ma)
                    {
                        giaTri = (double)item.GiaTri;
                        Session["MaGiamGia"] = (string)item.MaGiamGia;
                        Session["GiaTri"] = (double)item.GiaTri;

                        return Json(new {GiaTri = giaTri , Message = "Thêm mã giảm giá hành công", JsonRequestBehavior.AllowGet });
                    }
                    else
                    {
                        return Json(new { GiaTri = 0, Message = "Mã giảm giá không hợp lệ", JsonRequestBehavior.DenyGet });
                    }
                }
            }
            else
            {
                return Json(new { GiaTri = 0, Message = "Bạn chưa đăng nhập", JsonRequestBehavior.DenyGet });
            }
            return Json(new { Message = "Thành công", JsonRequestBehavior.AllowGet });
        }

        public ActionResult ThanhToan()
        {
            return View((List<CartModel>)Session["cart"]);
        }

        public ActionResult InfoAccount(string MaTV)
        {
            WebCafe.Models.ThanhVien thanhViens = db.ThanhViens.Where(x => x.TaiKhoan == MaTV).FirstOrDefault();
            if(thanhViens != null)
            {
                return Json(new { TaiKhoan = thanhViens.TaiKhoan,HoTen = thanhViens.HoTen,DiaChi = thanhViens.DiaChi,Email = thanhViens.Email,DienThoai = thanhViens.DienThoai, JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { Message = "Bạn chưa đăng nhập!!!!", JsonRequestBehavior.AllowGet });
            }
        }
    }
}