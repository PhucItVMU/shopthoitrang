using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("ManageProductImage/[action]")]
    public class ProductImageController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductImageController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Admin/ProductImage
        public IActionResult Index(int Id)
        {
            ViewBag.ProductId = Id;
            var items = _db.ProductImages.Where(x => x.ProductId == Id).ToList();
            return View(items);
        }

        public IActionResult AddImage(int productId, string url)
        {
            /*  _db.ProductImages.Add(new ProductImage
              {
                  ProductId = productId,
                  Image = url,
                  IsDefault = false
              });
              _db.SaveChanges();
              return Json(new { Success = true });*/
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.ProductImages.Find(id);
            _db.ProductImages.Remove(item);
            _db.SaveChanges();
            return Json(new { success = true });
        }

    }
}