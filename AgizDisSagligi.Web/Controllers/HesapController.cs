using System.Security.Claims;
using System.Linq;
using AgizDisSagligi.Business;
using AgizDisSagligi.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AgizDisSagligi.Web.Controllers;

public class HesapController : Controller
{
    private readonly KullaniciServisi _kullaniciServisi;

    public HesapController(KullaniciServisi kullaniciServisi)
    {
        _kullaniciServisi = kullaniciServisi;
    }

    [HttpGet]
    public IActionResult Kayit() => View();

    [HttpPost]
    public IActionResult Kayit(KayitViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (basarili, mesaj) = _kullaniciServisi.KayitOl(model.Mail, model.Parola, model.ParolaTekrar, model.AdSoyad, model.DogumTarihi);
        if (!basarili)
        {
            ModelState.AddModelError("", mesaj);
            return View(model);
        }

        TempData["Mesaj"] = "Kayıt başarılı, giriş yapabilirsiniz.";
        return RedirectToAction("Giris");
    }

    [HttpGet]
    public IActionResult Giris() => View();

    [HttpPost]
    public async Task<IActionResult> Giris(GirisViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (basarili, mesaj, kullanici) = _kullaniciServisi.GirisYap(model.Mail, model.Parola);
        if (!basarili)
        {
            ModelState.AddModelError("", mesaj);
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
            new Claim(ClaimTypes.Name, kullanici.AdSoyad),
            new Claim(ClaimTypes.Email, kullanici.Mail)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "AnaSayfa");
    }

    public async Task<IActionResult> Cikis()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Giris");
    }

    [HttpGet]
    public IActionResult ParolaHatirlat() => View();

    [HttpPost]
    public IActionResult ParolaHatirlatDogrula(ParolaHatirlatViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var hata = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage ?? "Geçerli bir mail adresi giriniz.";
            return Json(new { basarili = false, mesaj = hata });
        }

        var kullanici = _kullaniciServisi.MailIleBul(model.Mail);
        if (kullanici == null)
            return Json(new { basarili = false, mesaj = "Bu mail adresine kayıtlı kullanıcı bulunamadı." });

        return Json(new { basarili = true });
    }

    [HttpPost]
    public IActionResult YeniParolaBelirle(YeniParolaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var hata = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage ?? "Geçersiz bilgi.";
            return Json(new { basarili = false, mesaj = hata });
        }

        var (basarili, mesaj) = _kullaniciServisi.ParolaSifirla(model.Mail, model.YeniParola, model.YeniParolaTekrar);
        return Json(new { basarili, mesaj });
    }
}
