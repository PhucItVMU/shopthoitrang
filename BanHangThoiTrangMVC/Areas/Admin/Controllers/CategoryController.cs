using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("ManageCategory/[action]")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: ManageCategory
        public IActionResult Index()
        {
            var items = _db.Categories.ToList();
            return View(items);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
       
        public IActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.Categories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var item = _db.Categories.Find(id);
            return View(item);
        }
        [HttpPost]
       
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Attach(model);
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.Entry(model).Property(x => x.Title).IsModified = true;
                _db.Entry(model).Property(x => x.Link).IsModified = true;
                _db.Entry(model).Property(x => x.Description).IsModified = true;
                _db.Entry(model).Property(x => x.Alias).IsModified = true;
                _db.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _db.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                _db.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _db.Entry(model).Property(x => x.Position).IsModified = true;
                _db.Entry(model).Property(x => x.ModifiedDate).IsModified = true;
                _db.Entry(model).Property(x => x.ModifiedBy).IsModified = true;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var item = _db.Categories.Find(id);
            if (item != null)
            {
                /*var DeteleItem = _db.Categories.Attach(item);*/
                _db.Categories.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}