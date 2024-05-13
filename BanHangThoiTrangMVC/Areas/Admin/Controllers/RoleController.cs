using App.Areas.Identity.Models.RoleViewModels;
using App.ExtendMethods;
using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Security.Claims;


namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("ManageRole/[action]")]
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(ILogger<RoleController> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        //
        // GET: /Role/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var r = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            var roles = new List<RoleModel>();
            foreach (var _r in r)
            {
                var claims = await _roleManager.GetClaimsAsync(_r);
                var claimsString = claims.Select(c => c.Type + "=" + c.Value);

                var rm = new RoleModel()
                {
                    Name = _r.Name,
                    Id = _r.Id,
                    Claims = claimsString.ToArray()
                };
                roles.Add(rm);
            }

            return View(roles);
        }

        // GET: /Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Role/Create
        [HttpPost, ActionName(nameof(Create))]
       
        public async Task<IActionResult> CreateAsync(CreateRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var newRole = new IdentityRole(model.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa tạo role mới: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }
            return View();
        }

        // GET: /Role/Delete/roleid
        [HttpGet("{roleid}")]
        public async Task<IActionResult> DeleteAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            return View(role);
        }

        // POST: /Role/Edit/1
        [HttpPost("{roleid}"), ActionName("Delete")]
       
        public async Task<IActionResult> DeleteConfirmAsync(string roleid)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null) return NotFound("Không tìm thấy role");

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa xóa: {role.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }
            return View(role);
        }

        // GET: /Role/Edit/roleid
        [HttpGet]
        public async Task<IActionResult> Edit(string roleid, [Bind("Name")] EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            model.Name = role.Name;
            model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            model.role = role;
            ModelState.Clear();
            return View(model);

        }

        // POST: /Role/Edit/1
        [HttpPost("{roleid}"), ActionName("Edit")]
       
        public async Task<IActionResult> EditConfirmAsync(string roleid, [Bind("Name")] EditRoleModel model)
        {
            if (roleid == null) return NotFound("Không tìm thấy role");
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role == null)
            {
                return NotFound("Không tìm thấy role");
            }
            model.Claims = await _context.RoleClaims.Where(rc => rc.RoleId == role.Id).ToListAsync();
            model.role = role;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            role.Name = model.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên: {model.Name}";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError(result);
            }

            return View(model);
        }
    }
}