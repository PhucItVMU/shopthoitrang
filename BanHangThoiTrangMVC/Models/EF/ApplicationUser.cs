using Microsoft.AspNetCore.Identity;

namespace BanHangThoiTrangMVC.Models.EF
{
    public class ApplicationUser : IdentityUser
    {
        public string? Fullname { get; set; }
        public string? Phone { get; set; }
        public List<string>? RoleNames { get; set; }
    }
}
