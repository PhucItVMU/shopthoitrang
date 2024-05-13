using BanHangThoiTrangMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("ManageOrder/[action]")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Admin/Order
        public IActionResult Index(int? page)
        {
            var items = _db.Orders.OrderByDescending(x => x.CreateDate);
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(PagedListExtensions.ToPagedList(items, pageNumber, pageSize));
        }


        public IActionResult ViewOrder(int id)
        {
            var item = _db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == id);
            return View(item);
        }

        public IActionResult Partial_SanPham(int id)
        {
            var items = _db.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }

        [HttpPost]
        public IActionResult UpdateTT(int id,int trangthai)
        {
            var item = _db.Orders.Find(id);
            if (item != null)
            {
                _db.Orders.Attach(item);
                item.Status = trangthai;
                _db.Entry(item).Property(x => x.Status).IsModified = true;
                _db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "UnSuccess", Success = false });
        }
    }
}