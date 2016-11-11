using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThoConShop.Web.Models
{
    public class AccountSoldViewModel
    {
        public string AccountName { get; set; }

        public string Password { get; set; }
    }

    public class CreateOrUpdateAccountViewModel
    {
        public int AccounId { get; set; }

        public CreateOrUpdateAccountViewModel()
        {
                RankList = new List<SelectListItem>();
        }

        public List<SelectListItem> RankList { get; set; }

        [Required( ErrorMessage = "Tài khoản không được trống.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được trống.")]
        public string Password { get; set; }

        //public string Title { get; set; }

        [Required,
         Range(1000, int.MaxValue, ErrorMessage = "Đơn giá không được nhỏ hơn 1000.")]
        public double Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Xin hãy chọn thông tin tồn tại.")]
        public int RankId { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public bool IsHot { get; set; } = true;

        public bool IsDelete { get; set; }

        [Required(ErrorMessage = "Hình đại diện không được trống.")]
        //[FileExtensions(ErrorMessage = "Your error message.", Extensions = "jpg,jpeg,png")]
        public HttpPostedFileBase Avatar { get; set; }


        //[FileExtensions(ErrorMessage = "Your error message.", Extensions = "jpg,jpeg,png")]
        public HttpPostedFileBase[] PageGem { get; set; }
    }
}