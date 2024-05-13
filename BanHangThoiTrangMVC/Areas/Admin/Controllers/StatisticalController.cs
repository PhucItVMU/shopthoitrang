using BanHangThoiTrangMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("ManageStatistical/[action]")]
    public class StatisticalController : Controller
    {
        private readonly ApplicationDbContext _db;

        public StatisticalController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Admin/Statistical
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in _db.Orders join od in _db.OrderDetails
                        on o.Id equals od.OrderId
                        join p in _db.Products
                        on od.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreateDate,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => new { Date = x.CreatedDate.Date })
             .Select(x => new
             {
                 Date = x.Key.Date,
                 TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                 TotalSell = x.Sum(y => y.Quantity * y.Price),
             })
             .Select(x => new
             {
                 Date = x.Date,
                 DoanhThu = x.TotalSell,
                 LoiNhuan = x.TotalSell - x.TotalBuy
             });

                    return Json(new { Data = result });
        }
    }
}