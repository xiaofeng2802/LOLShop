using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PagedList;
using ThoConShop.Business.Dtos;

namespace ThoConShop.Web.Models
{

    public class Feed
    {
        public string Text { get; set; }
    }

    public class ChampCreationViewModel
    {
        [Required(ErrorMessage = "Tên Tướng không được trống.")]
        public string ChampName { get; set; }

        [Required(ErrorMessage = "Hình đại diện không được trống.")]
        public HttpPostedFileBase Avatar { get; set; }
    }

    public class SkinCreationViewModel
    {

        [Required(ErrorMessage = "Tên trang phục không được trống.")]
        public string SkinName { get; set; }


        public HttpPostedFileBase Avatar { get; set; }

        public string IsOnFilter { get; set; }

        public int? GroupId { get; set; }

        public List<SelectListItem> ParentData { get; set; }
    }

    public class UserManagementViewModel
    {
        public IPagedList<AccountDto> Datasource { get; set; }

        public string SearchString { get; set; }

    }
    public class CreateRankViewModel
    {
        [Required(ErrorMessage = "Tên Rank không được trống.")]
        public string RankName { get; set; }

        [Required(ErrorMessage = "Hình rank không được trống.")]
        public HttpPostedFileBase RankImage { get; set; }
    }

    public class UserRechargeViewModel
    {
        public IPagedList<UserRechargeHistoryDto> DataSource { get; set; }

        public int ReportMonth { get; set; } = DateTime.Now.Month;

        public IList<SelectListItem> FilterList
        {
            get
            {
                var data =
                    new List<SelectListItem>();

                for (int i = 1; i <= 12; i++)
                {
                    data.Add(new SelectListItem()
                    {
                        Selected = (i == ReportMonth),
                        Text = "Tháng " + i,
                        Value = i.ToString()
                    });
                }
                return data;
            }
        }
    }
}