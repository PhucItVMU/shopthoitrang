using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangThoiTrangMVC.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var products = _db.Products
                .Include(p=>p.ProductCategory).ToList();    
            var categoriesModel = _db.Categories.ToList();
            var subscribe = _db.Subscribes.FirstOrDefault();
            var news = _db.News.ToList();

            ViewData["Products"] = products;
            ViewData["CategoryModel"] = categoriesModel;
            ViewData["Subscribe"] = subscribe;
            ViewData["News"] = news;

            return View();
        }


        public IActionResult Partial_Subcribe()
        {
            return PartialView();
        }
        [HttpPost]
       
        public IActionResult Subscribe(Subscribe req)
        {
            if (ModelState.IsValid)
            {
                _db.Subscribes.Add(new Subscribe { Email = req.Email, CreateDate = DateTime.Now });
                _db.SaveChanges();
                return Json(new { Success = true });
            }
            return View("Partial_Subcribe", req);
        }
        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Refresh()
        {
            var item = new ThongKeModel();
            ViewBag["Visitors_online"] = HttpContext.Items["visitors_online"];
            var hn = HttpContext.Items["HomNay"];
            item.HomNay = HttpContext.Items["HomNay"].ToString();
            item.HomQua = HttpContext.Items["HomQua"].ToString();
            item.TuanNay = HttpContext.Items["TuanNay"].ToString();
            item.TuanTruoc = HttpContext.Items["TuanTruoc"].ToString();
            item.ThangNay = HttpContext.Items["ThangNay"].ToString();
            item.ThangTruoc = HttpContext.Items["ThangTruoc"].ToString();
            item.TatCa = HttpContext.Items["TatCa"].ToString();
            return PartialView(item);
        }
    }
}