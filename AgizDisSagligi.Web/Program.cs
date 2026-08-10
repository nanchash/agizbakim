using AgizDisSagligi.Business;
using AgizDisSagligi.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VarsayilanBaglanti")));

builder.Services.Configure<MailAyarlari>(builder.Configuration.GetSection("MailAyarlari"));

builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IHedefRepository, HedefRepository>();
builder.Services.AddScoped<IDurumKaydiRepository, DurumKaydiRepository>();
builder.Services.AddScoped<INotRepository, NotRepository>();
builder.Services.AddScoped<IOneriRepository, OneriRepository>();

builder.Services.AddScoped<SifrelemeServisi>();
builder.Services.AddScoped<MailServisi>();
builder.Services.AddScoped<KullaniciServisi>();
builder.Services.AddScoped<HedefServisi>();
builder.Services.AddScoped<DurumKaydiServisi>();
builder.Services.AddScoped<NotServisi>();
builder.Services.AddScoped<OneriServisi>();
builder.Services.AddScoped<RozetServisi>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Hesap/Giris";
        options.AccessDeniedPath = "/Hesap/Giris";
    });

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
