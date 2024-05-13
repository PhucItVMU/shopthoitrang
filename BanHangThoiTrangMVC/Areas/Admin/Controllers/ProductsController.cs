using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("ManageProduct/[action]")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IWebHostEnvironment _webHostEnvironment;
        public ProductsController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ManageProduct
        public IActionResult Index(string? Searchtext, int? page)
        {
            IQueryable<Product> items = _db.Products.Include(p=>p.ProductCategory).OrderByDescending(x => x.Id); 

            var pageSize = 5;
            var pageIndex = page ?? 1;

            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Alias.Contains(Searchtext) || x.Title.Contains(Searchtext));
            }

            var pagedItems = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(pagedItems);
        }


        public IActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> AddAsync(Product model, IFormFileCollection files, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                // Upload images
                var imageUrls = await App.Helper.Utilities.UploadFiles(files, "products\\" + model.Id.ToString(), _webHostEnvironment);

                if (imageUrls != null && imageUrls.Any())
                {
                    model.Image = imageUrls;
                }
                model.CreateDate = DateTime.UtcNow;
                model.ModifiedDate = DateTime.UtcNow;
                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Title;
                }
                if (string.IsNullOrEmpty(model.Alias))
                {
                    model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                }
                _db.Products.Add(model);
                await _db.SaveChangesAsync();

                return Redirect("/ManageProduct/Index");
            }
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");

            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "Id", "Title");
            var item = _db.Products.Find(id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, Product model, IFormFileCollection files)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var productOld = await _db.Products.FindAsync(id);
                if (productOld == null)
                {
                    return NotFound();
                }
                List<string> oldImageUrls = new List<string>();

                if (files.Count() > 0)
                {
                    // Delete old images if there are any
                    oldImageUrls = productOld.Image;
                    if (oldImageUrls != null)
                    {
                        foreach (var oldImageUrl in oldImageUrls)
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldImageUrl);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }

                    // Upload new images
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", model.Id.ToString());
                    var newImageUrls = await App.Helper.Utilities.UploadFiles(files, "products\\" + model.Id.ToString(), _webHostEnvironment);

                    if (newImageUrls != null && newImageUrls.Any())
                    {
                        productOld.Image = newImageUrls;
                    }
                }
                productOld.Title = model.Title;
                productOld.Description = model.Description;
                productOld.Price = model.Price;
                productOld.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);

                productOld.CreateDate = DateTime.Now;
                productOld.ModifiedDate = DateTime.Now;
                _db.Update(productOld);
                await _db.SaveChangesAsync();
                return Redirect("/ManageProduct/Index");
            }
            return View(model);
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                _db.Products.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
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
                        var obj = _db.Products.Find(Convert.ToInt32(item));
                        _db.Products.Remove(obj);
                        _db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsActive(int id)
        {
            var item = _db.Products.Find(id);
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
        public IActionResult IsHome(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult IsSale(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });
            }
            return Json(new { success = false });
        }
    }
}