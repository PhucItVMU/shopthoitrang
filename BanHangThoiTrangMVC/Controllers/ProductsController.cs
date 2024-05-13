using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BanHangThoiTrangMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Products
        public IActionResult Index(string Searchtext, int? id)
        {
            /*var items = _db.Products.Where(x => x.IsActive).Take(12).ToList();*/
            IEnumerable<Product> items = _db.Products.OrderByDescending(x => x.Id);
            items = items.Where(x => x.IsActive).Take(12).ToList();
            /*if (id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }*/
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            return View(items);
        }
        [Route("danh-muc-san-pham")]
        public IActionResult ProductCategory(string alias, int id)
        {
            var items = _db.Products.Where(x => x.IsActive).Take(12).ToList();
            if (id > 0)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = _db.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }

        public IActionResult Partial_ItemsByCateId()
        {
            var items = _db.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }

        public IActionResult Partial_ProductSale()
        {
            var items = _db.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        [Route("/chi-tiet")]
        public IActionResult Detail(string alias, int id)
        {
            var item = _db.Products.FirstOrDefault(p => p.Id == id && p.Alias == alias);
            if (item != null)
            {
                item.ViewCount += 1;
                if (item.ViewCount == 1000)
                {
                    item.ViewCount = 0;
                }
                _db.SaveChanges();
                return View(item);
            }
            return NotFound(); // Trả về mã lỗi 404 nếu không tìm thấy sản phẩm
        }
    }
}