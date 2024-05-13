using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BanHangThoiTrangMVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public IActionResult Index()
        {
            return View();
        }
    }
}