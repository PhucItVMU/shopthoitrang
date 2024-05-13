using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using BanHangThoiTrangMVC.Models.Payments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using BanHangThoiTrangMVC.ExtendMethods;
using Microsoft.EntityFrameworkCore;

namespace BanHangThoiTrangMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _accessor;

        public ShoppingCartController(ApplicationDbContext db, IConfiguration configuration, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {
            _db = db;
            _configuration = configuration;
            _env = env;
            _accessor = accessor;
        }

        public IActionResult Index()
        {

            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return View(cart.Items);
                ViewBag.CheckCart = cart;
            }
            return View();
        }


        public IActionResult CheckOut()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public IActionResult CheckOutSuccess()
        {
            return View();
        }

        public IActionResult Partial_CheckOut()
        {
            return PartialView();
        }

        [HttpPost]
       

        public IActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
                ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    order.Status = 1; // Chưa thanh toán / 2/đã thanh toán, 3/Hoàn thành, 4/hủy

                    // Add order details
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity ?? 0, // Use null-coalescing operator to provide a default value of 0
                        Price = x.Price ?? 0 // Use null-coalescing operator to provide a default value of 0
                    }));

                    // Calculate total amount
                    order.TotalAmount = cart.Items.Sum(x => (x.Price ?? 0) * (x.Quantity ?? 0)); // Use null-coalescing operator to provide default values of 0

                    order.TypePayment = req.TypePayment;
                    order.CreateDate = DateTime.Now;
                    order.ModifiedDate = DateTime.Now;
                    order.CreateBy = req.Phone;

                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

                    // Add order to database
                    _db.Orders.Add(order);
                    _db.SaveChanges();

                    // Prepare email content
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var TongTien = decimal.Zero;

                    foreach (var sp in cart.Items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>" + sp.ProductName + "</td>";
                        strSanPham += "<td>" + (sp.Quantity ?? 0) + "</td>"; // Use null-coalescing operator to provide a default value of 0
                        strSanPham += "<td>" + BanHangThoiTrangMVC.Common.Common.FormatNumber(sp.TotalPrice ?? 0, 0) + "</td>"; // Use null-coalescing operator to provide a default value of 0
                        strSanPham += "</tr>";

                        // Calculate total price
                        thanhtien += (sp.Price ?? 0) * (sp.Quantity ?? 0); // Use null-coalescing operator to provide default values of 0
                    }

                    TongTien = thanhtien;
                    string contentPath = Path.Combine(_env.WebRootPath, "Content/templates/send1.html");
                    string contentCustomer = System.IO.File.ReadAllText(contentPath);
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", BanHangThoiTrangMVC.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", BanHangThoiTrangMVC.Common.Common.FormatNumber(TongTien, 0));
                    BanHangThoiTrangMVC.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email, _configuration);

                    string contentAdmin = System.IO.File.ReadAllText(contentPath);
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", BanHangThoiTrangMVC.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", BanHangThoiTrangMVC.Common.Common.FormatNumber(TongTien, 0));
                    BanHangThoiTrangMVC.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), _configuration["EmailAdmin"], _configuration);
                    cart.ClearCart();
                    code = new { Success = true, Code = req.TypePayment, Url = "" };
                    //var url = "";
                    if (req.TypePayment == 2)
                    {
                        var url = UrlPayment(req.TypePaymentVN, order.Code);
                        code = new { Success = true, Code = req.TypePayment, Url = url };
                    }

                    //code = new { Success = true, Code = 1, Url = url };
                    //return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
        }


        public IActionResult ShowCount() //hien thi co bao nhieu san pham trong gio hang
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count });
            }
            return Json(new { Count = 0 });
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            if (User.Identity.IsAuthenticated)
            {
                var code = new { Success = false, msg = "", code = -1, Count = 0 };
                var checkProduct = _db.Products
                    .Where(x => x.Id == id)
                    .Select(p => new
                    {
                        Product = p,
                        Category = p.ProductCategory
                    })
                    .FirstOrDefault();

                if (checkProduct != null)
                {
                    var product = checkProduct.Product;
                    var category = checkProduct.Category;

                    ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
                    if (cart == null)
                    {
                        cart = new ShoppingCart();
                    }

                    ShoppingCartItem item = new ShoppingCartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Title,
                        ProductImg = product.Image, // Kiểm tra xem cách lưu trữ hình ảnh trong cơ sở dữ liệu
                        CategoryName = category.Title,
                        Price = product.Price,
                        Quantity = quantity
                    };
                    item.TotalPrice = quantity * product.Price;
                    cart.AddToCart(item, quantity);
                    HttpContext.Session.Set<ShoppingCart>("Cart", cart);
                    code = new { Success = true, msg = "Thêm Sản Phẩm Vào Giỏ Hàng Thành Công!", code = 1, Count = cart.Items.Count };
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true, msg = "Cập Nhật Thành Công" });
            }
            return Json(new { Success = false });
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public IActionResult DeleteAll()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(false);
        }


        public IActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        public IActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = HttpContext.Session.Get<ShoppingCart>("Cart");
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }


        public IActionResult VnpayReturn()
        {
            if (Request.Query.Count > 0)
            {
                string vnp_HashSecret = _configuration["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData.Keys)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                String TerminalID = Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = _db.Orders.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 2;//đã thanh toán
                            _db.Orders.Update(itemOrder);
                            _db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                    }
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                }
            }
            return View();
        }

        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = _db.Orders.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = _configuration["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = _configuration["vnp_HashSecret"]; //Secret Key
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreateDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            Utils utils = new Utils(_accessor);
            vnpay.AddRequestData("vnp_IpAddr", utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
    }
}