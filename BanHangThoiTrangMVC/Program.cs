using App.Services;
using BanHangThoiTrangMVC.Models;
using BanHangThoiTrangMVC.Models.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

// Đăng ký AppDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectString = builder.Configuration.GetConnectionString("AppMVCConnectionStrings");
    if (connectString != null)
    {
        options.UseSqlServer(connectString);
    }
    else
    {
        throw new Exception("Chuỗi kết nối không được tìm thấy");
    }
});

//Đăng kí mail
builder.Services.AddOptions();
var mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

//Đăng kí identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cấu hình Cookie
builder.Services.ConfigureApplicationCookie(options => {
    // options.Cookie.HttpOnly = true;  
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role)
    // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
    options.ValidationInterval = TimeSpan.FromSeconds(5);
});
//Xác thực GG
/*builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var gConfig = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = gConfig["ClientId"];
        options.ClientSecret = gConfig["ClientSecret"];
        options.CallbackPath = "/dang-nhap-tu-Google";
    })
//Add fb 
  .AddFacebook(options =>
  {
      var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
      options.AppId = fconfig["AppId"];
      options.AppSecret = fconfig["AppSecret"];
      options.CallbackPath = "/dang-nhap-tu-facebook";
  });*/

//Thay thế error identity 
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

//app.AddStatusCodePage(); // tuy bien cac loi 400 - 599

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
