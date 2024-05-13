using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("ManageProductCategory/[action]")]
    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IWebHostEnvironment _webHostEnvironment;
        public ProductCategoryController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ManageProductCategory
        public IActionResult Index()
        {
            var items = _db.ProductCategories;
            return View(items);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> AddAsync(ProductCategory model, IFormFileCollection files)
        {
            model.CreateDate = DateTime.Now;
            model.ModifiedBy = "Admin";
            model.CreateBy = "Admin";
            if (ModelState.IsValid)
            {// Upload images
                var imageUrls = await App.Helper.Utilities.UploadFiles(files, "productCate\\" + model.Id.ToString(), _webHostEnvironment);

                if (imageUrls != null && imageUrls.Any())
                {
                    model.Icon = imageUrls;
                }
                model.Description = "";
                model.ModifiedDate = DateTime.Now;
                model.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.ProductCategories.Add(model);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var item = _db.ProductCategories.Find(id);
            return View(item);
        }
        [HttpPost]
       
        public async Task<IActionResult> EditAsync(ProductCategory model, int id, IFormFileCollection files)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var productOld = await _db.ProductCategories.FindAsync(id);
                if (productOld == null)
                {
                    return NotFound();
                }
                List<string> oldImageUrls = new List<string>();

                if (files.Count() > 0)
                {
                    // Delete old images if there are any
                    oldImageUrls = productOld.Icon;
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
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "productCate", model.Id.ToString());
                    var newImageUrls = await App.Helper.Utilities.UploadFiles(files, "productCate\\" + model.Id.ToString(), _webHostEnvironment);

                    if (newImageUrls != null && newImageUrls.Any())
                    {
                        productOld.Icon = newImageUrls;
                    }
                }
                model.ModifiedDate = DateTime.Now;
                productOld.Alias = BanHangThoiTrangMVC.Models.Common.Filter.FilterChar(model.Title);
                _db.Update(productOld);
                await _db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _db.ProductCategories.Find(id);
            if (item != null)
            {
                _db.ProductCategories.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}