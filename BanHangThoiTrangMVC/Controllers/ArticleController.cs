using BanHangThoiTrangMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BanHangThoiTrangMVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ArticleController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Article
        public IActionResult Index(string alias)
        {
            var item = _db.Posts.FirstOrDefault(x => x.Alias == alias);
            return View(item);
        }
    }
}