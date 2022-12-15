using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebCafe.Models
{
    public class AdminViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tài khoản")]
        public string TaiKhoan { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string MatKhau { get; set; }
    }
}