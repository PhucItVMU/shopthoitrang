using BanHangThoiTrangMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BanHangThoiTrangMVC.Models.EF;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [Route("ManageHome/[action]")]
    public class ManageHomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManage;
        private ApplicationDbContext _db;

        public ManageHomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManage)
        {
            _db = db;
            _userManage = userManage;
        }

        // GET: Admin/Home
        public IActionResult Index()
        {
            return View();
        }
    }
}