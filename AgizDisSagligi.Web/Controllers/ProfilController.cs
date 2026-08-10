using System.Security.Claims;
using AgizDisSagligi.Business;
using AgizDisSagligi.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgizDisSagligi.Web.Controllers;

[Authorize]
public class ProfilController : Controller
{
    private readonly KullaniciServisi _kullaniciServisi;
    private readonly HedefServisi _hedefServisi;
    private readonly NotServisi _notServisi;
    private readonly DurumKaydiServisi _durumKaydiServisi;
    private readonly RozetServisi _rozetServisi;

    public ProfilController(
        KullaniciServisi kullaniciServisi,
        HedefServisi hedefServisi,
        NotServisi notServisi,
        DurumKaydiServisi durumKaydiServisi,
        RozetServisi rozetServisi)
    {
        _kullaniciServisi = kullaniciServisi;
        _hedefServisi = hedefServisi;
        _notServisi = notServisi;
        _durumKaydiServisi = durumKaydiServisi;
        _rozetServisi = rozetServisi;
    }

    private int GetirKullaniciId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    private ProfilViewModel ModelOlustur(int kullaniciId)
    {
        var kullanici = _kullaniciServisi.IdIleBul(kullaniciId);
        return new ProfilViewModel
        {
            Mail = kullanici.Mail,
            AdSoyad = kullanici.AdSoyad,
            DogumTarihi = kullanici.DogumTarihi,
            KayitTarihi = kullanici.KayitTarihi,
            GunlukSeri = _durumKaydiServisi.GunlukSeriHesapla(kullaniciId),
            ToplamHedefSayisi = _hedefServisi.ListeleKullaniciIle(kullaniciId).Count,
            ToplamTamamlananSayisi = _durumKaydiServisi.ListeleKullaniciIle(kullaniciId).Count(d => d.Uygulandi),
            ToplamNotSayisi = _notServisi.ListeleKullaniciIle(kullaniciId).Count,
            Rozetler = _rozetServisi.Hesapla(kullaniciId)
        };
    }

    public IActionResult Index()
    {
        return View(ModelOlustur(GetirKullaniciId()));
    }

    [HttpPost]
    public async Task<IActionResult> Guncelle(ProfilViewModel model)
    {
        var kullaniciId = GetirKullaniciId();

        if (!ModelState.IsValid)
        {
            var yeniden = ModelOlustur(kullaniciId);
            yeniden.Mail = model.Mail;
            yeniden.AdSoyad = model.AdSoyad;
            yeniden.DogumTarihi = model.DogumTarihi;
            return View("Index", yeniden);
        }

        var (basarili, mesaj) = _kullaniciServisi.ProfilGuncelle(kullaniciId, model.Mail, model.AdSoyad, model.DogumTarihi, model.YeniParola, model.YeniParolaTekrar);
        if (!basarili)
        {
            ModelState.AddModelError("", mesaj);
            var yeniden = ModelOlustur(kullaniciId);
            yeniden.Mail = model.Mail;
            yeniden.AdSoyad = model.AdSoyad;
            yeniden.DogumTarihi = model.DogumTarihi;
            return View("Index", yeniden);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, kullaniciId.ToString()),
            new Claim(ClaimTypes.Name, model.AdSoyad),
            new Claim(ClaimTypes.Email, model.Mail)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        TempData["Mesaj"] = mesaj;
        return RedirectToAction("Index");
    }
}
