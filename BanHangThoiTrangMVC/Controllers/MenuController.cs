using BanHangThoiTrangMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BanHangThoiTrangMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MenuController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Menu
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MenuTop()
        {
            var items = _db.Categories.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", items);
        }

        public IActionResult MenuProductCategory()
        {
            var items = _db.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
        }
        
        public IActionResult MenuLeft(int? id)
        {
            var items = _db.ProductCategories.ToList();
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            return PartialView("_MenuLeft", items);
        }

        public IActionResult MenuArrivals()
        {
            var items = _db.ProductCategories.ToList();
            return PartialView("_MenuArrivals", items);
        }
    }
}