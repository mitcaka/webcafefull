using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCafe.Models
{
    public class ShopViewModel
    {
        public List<SanPham> listSanPham { get; set; }

        public List<LoaiSanPham> listLoaiSP { get; set; }
        public List<LoaiSanPham> rd3loai { get; set; }

        public List<LoaiVaSoLuong> listLoaiSoLuong { get; set; }
    }
}