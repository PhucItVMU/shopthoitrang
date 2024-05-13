using Microsoft.AspNetCore.Mvc;
using BanHangThoiTrangMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BanHangThoiTrangMVC.Models.EF;
using Microsoft.AspNetCore.Identity.UI.Services;
using App.ExtendMethods;

namespace BanHangThoiTrangMVC.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ManageController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user)
            };
            return View(model);
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        public async Task<IActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var user = await _signInManager.UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Index), new { message = ManageMessageId.Error });
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.RemoveLoginSuccess });
            }
            return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
        }

        // GET: /Manage/AddPhoneNumber
        public IActionResult AddPhoneNumber()
        {
            return View();
        }

        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.Number);
            await _emailSender.SendSmsAsync(model.Number, "Mã xác thực là: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]

        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(await GetCurrentUserAsync(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]

        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.AddPhoneSuccess });
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Lỗi thêm số điện thoại");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]

        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ChangePassword
        public IActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]

        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                ModelState.AddModelError(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/SetPassword
        public IActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]

        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }
                ModelState.AddModelError(result);
                return View(model);
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<IActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewData["StatusMessage"] =
                message == ManageMessageId.RemoveLoginSuccess ? "Đã loại bỏ liên kết tài khoản."
                : message == ManageMessageId.AddLoginSuccess ? "Đã thêm liên kết tài khoản"
                : message == ManageMessageId.Error ? "Có lỗi."
                : "";
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]

        public IActionResult LinkLogin(string provider)
        {
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            if (info == null)
            {
                return RedirectToAction(nameof(ManageLogins), new { Message = ManageMessageId.Error });
            }
            var result = await _userManager.AddLoginAsync(user, info);
            var message = result.Succeeded ? ManageMessageId.AddLoginSuccess : ManageMessageId.Error;
            return RedirectToAction(nameof(ManageLogins), new { Message = message });
        }
    }
}