using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BanHangThoiTrangMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("ManageAccount/[action]")]
    public class ManageAccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageAccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
        }


        // GET: Admin/Account
        public async Task<IActionResult> Index(int? page)
        {
            var pageSize = 5;
            var pageIndex = page ?? 1;

            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var userViewModel = new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.Fullname,
                    Phone = u.Phone,
                    Email = u.Email,
                    Roles = roles.ToList()
                };
                userViewModels.Add(userViewModel);
            }

            var sortedUserViewModels = userViewModels.OrderByDescending(x => x.Id).ToPagedList(pageIndex, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(sortedUserViewModels);
        }


        [AllowAnonymous]
        public IActionResult Create()
        {
            ViewBag.Role = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
       
        public async Task<IActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Fullname = model.FullName,
                    Phone = model.Phone
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }
            ViewBag.Role = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
       
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
       
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();


            if (roles != null && roles.Any())
            {
                var selecList = new SelectList(roles, "Name", "Name");
                ViewBag.Role = selecList;
            }
            else
            {
                ViewBag.Role = ""; 
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.Fullname = model.Fullname;
                user.Phone = model.Phone;
                user.Email = model.Email;

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}