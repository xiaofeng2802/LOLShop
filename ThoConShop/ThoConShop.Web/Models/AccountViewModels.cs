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

    public class CreateAccountViewModel
    {
        public CreateAccountViewModel()
        {
                RankList = new List<SelectListItem>();
        }
        public List<SelectListItem> RankList { get; set; }

        public string AccountName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int RankId { get; set; }

        public string RankName { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsHot { get; set; }

        public bool IsDelete { get; set; }

        [FileExtensions(ErrorMessage = "Your error message.", Extensions = "jpg,jpeg,png")]
        public HttpPostedFileBase Avatar { get; set; }
    }
}