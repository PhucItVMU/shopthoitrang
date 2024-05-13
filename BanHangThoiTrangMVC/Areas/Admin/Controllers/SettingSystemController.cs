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
    [Authorize(Roles = "Admin")]
    [Route("ManageSettingSystem/[action]")]
    public class SettingSystemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SettingSystemController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: SettingSystem
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Partial_Setting()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult AddSetting(SettingSystemViewModel req)
        {
            SystemSetting set = null;
            var checkTitle = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            if (checkTitle == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingTitle";
                set.SettingValue = req.SettingTitle;
                _db.SystemSettings.Add(set);
            }
            else
            {
                checkTitle.SettingValue = req.SettingTitle;
                _db.Entry(checkTitle).State = EntityState.Modified;
            }
            //logo
            var checkLogo = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingLogo";
                set.SettingValue = req.SettingLogo;
                _db.SystemSettings.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
                _db.Entry(checkLogo).State = EntityState.Modified;
            }
            //Email
            var email = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingEmail"));
            if (email == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingEmail";
                set.SettingValue = req.SettingEmail;
                _db.SystemSettings.Add(set);
            }
            else
            {
                email.SettingValue = req.SettingEmail;
                _db.Entry(email).State = EntityState.Modified;
            }
            //Hotline
            var Hotline = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingHotline"));
            if (Hotline == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingHotline";
                set.SettingValue = req.SettingHotline;
                _db.SystemSettings.Add(set);
            }
            else
            {
                Hotline.SettingValue = req.SettingHotline;
                _db.Entry(Hotline).State = EntityState.Modified;
            }
            //TitleSeo
            var TitleSeo = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingTitleSeo"));
            if (TitleSeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = req.SettingTitleSeo;
                _db.SystemSettings.Add(set);
            }
            else
            {
                TitleSeo.SettingValue = req.SettingTitleSeo;
                _db.Entry(TitleSeo).State = EntityState.Modified;
            }
            //DessSeo
            var DessSeo = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingDesSeo"));
            if (DessSeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = req.SettingDesSeo;
                _db.SystemSettings.Add(set);
            }
            else
            {
                DessSeo.SettingValue = req.SettingDesSeo;
                _db.Entry(DessSeo).State = EntityState.Modified;
            }
            //KeySeo
            var KeySeo = _db.SystemSettings.FirstOrDefault(x => x.SettingKey.Contains("SettingKeySeo"));
            if (KeySeo == null)
            {
                set = new SystemSetting();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = req.SettingKeySeo;
                _db.SystemSettings.Add(set);
            }
            else
            {
                KeySeo.SettingValue = req.SettingKeySeo;
                _db.Entry(KeySeo).State = EntityState.Modified;
            }
            _db.SaveChanges();

            return View("Partial_Setting");
        }
    }
}