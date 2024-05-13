using BanHangThoiTrangMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BanHangThoiTrangMVC.Common
{
    public static class SettingHelper
    {
        private static ApplicationDbContext _db;

        static SettingHelper()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=BanHangThoiTrangMVC;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=true;");

            // Khởi tạo đối tượng _db
            _db = new ApplicationDbContext(optionsBuilder.Options);
        }

        public static string GetValue(string key)
        {
            var item = _db.SystemSettings.SingleOrDefault(x => x.SettingKey == key);
            if (item != null)
            {
                return item.SettingValue;
            }
            return "";
        }
    }
}
