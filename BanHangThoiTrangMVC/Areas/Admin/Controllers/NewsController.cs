using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [Route("ManageNews/[action]")]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Admin/News

        //pagelist cách 2
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = _db.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }


        /*public IActionResult Index(string Searchtext, int page = 1, int pagesize = 5)
        {
            var items = _db.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
               items = (IOrderedQueryable<News>)items.Where(x => x.Alias.Equals(Searchtext) || x.Title.Contains(Searchtext));
            }
            items = items.ToPagedList(page, pagesize);
            //Chỉnh cho STT tăng dần theo trang
            ViewBag.PageSize = pagesize;
            ViewBag.Page = page;
            return View(items);
        }*/

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
       
        public IActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.CategoryId = 5;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.News.Add(model);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }
        public IActionResult Edit(int Id)
        {
            var item = _db.News.Find(Id);
            return View(item);
        }

        [HttpPost]
       
        public IActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                _db.News.Attach(model);
                model.CreateDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.News.Find(id);
            if (item != null)
            {
                _db.News.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.News.Find(id);
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
                        var obj = _db.News.Find(Convert.ToInt32(item));
                        _db.News.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}