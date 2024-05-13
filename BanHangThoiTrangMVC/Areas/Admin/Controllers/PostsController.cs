using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [Route("ManagePost/[action]")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: ManagePost
        public IActionResult Index()
        {
            var items = _db.Posts.ToList();
            return View(items);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
       
        public IActionResult Add(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.CategoryId = 5;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.Posts.Add(model);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }
        public IActionResult Edit(int Id)
        {
            var item = _db.Posts.Find(Id);
            return View(item);
        }

        [HttpPost]
       
        public IActionResult Edit(Posts model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.Posts.Attach(model);
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Posts.Find(id);
            if (item != null)
            {
                _db.Posts.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, IsActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _db.Posts.Find(Convert.ToInt32(item));
                        _db.Posts.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}