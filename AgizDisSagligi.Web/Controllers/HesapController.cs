using System.Security.Claims;
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
        if (!ModelState.IsValid) return View("ParolaHatirlat", model);

        var kullanici = _kullaniciServisi.MailIleBul(model.Mail);
        if (kullanici == null)
        {
            ViewBag.Hata = "Bu mail adresine kayıtlı kullanıcı bulunamadı.";
            return View("ParolaHatirlat", model);
        }

        return View("YeniParolaBelirle", new YeniParolaViewModel { Mail = model.Mail });
    }

    [HttpPost]
    public IActionResult YeniParolaBelirle(YeniParolaViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (basarili, mesaj) = _kullaniciServisi.ParolaSifirla(model.Mail, model.YeniParola, model.YeniParolaTekrar);
        if (!basarili)
        {
            ModelState.AddModelError("", mesaj);
            return View(model);
        }

        TempData["Mesaj"] = "Parolanız güncellendi, giriş yapabilirsiniz.";
        return RedirectToAction("Giris");
    }
}
