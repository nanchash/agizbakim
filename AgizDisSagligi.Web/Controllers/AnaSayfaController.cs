using System.Security.Claims;
using AgizDisSagligi.Business;
using AgizDisSagligi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgizDisSagligi.Web.Controllers;

[Authorize]
public class AnaSayfaController : Controller
{
    private readonly HedefServisi _hedefServisi;
    private readonly DurumKaydiServisi _durumKaydiServisi;
    private readonly OneriServisi _oneriServisi;
    private readonly NotServisi _notServisi;

    public AnaSayfaController(HedefServisi hedefServisi, DurumKaydiServisi durumKaydiServisi, OneriServisi oneriServisi, NotServisi notServisi)
    {
        _hedefServisi = hedefServisi;
        _durumKaydiServisi = durumKaydiServisi;
        _oneriServisi = oneriServisi;
        _notServisi = notServisi;
    }

    public IActionResult Index()
    {
        var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var hedefler = _hedefServisi.ListeleKullaniciIle(kullaniciId);

        var bugunTamamlanan = hedefler
            .SelectMany(h => _durumKaydiServisi.ListeleHedefIle(h.Id))
            .Count(d => d.Tarih.Date == DateTime.Now.Date && d.Uygulandi);

        var sonNot = _notServisi.ListeleKullaniciIle(kullaniciId)
            .OrderByDescending(n => n.EklenmeTarihi)
            .FirstOrDefault();
        var sonKontrolGunSayisi = sonNot == null ? (int?)null : (DateTime.Now.Date - sonNot.EklenmeTarihi.Date).Days;

        var model = new AnaSayfaViewModel
        {
            AdSoyad = User.Identity.Name,
            ToplamHedefSayisi = hedefler.Count,
            BugunTamamlananSayisi = bugunTamamlanan,
            GunlukOneri = _oneriServisi.RastgeleGetir(),
            HaftalikKontrolHatirlatmasi = sonKontrolGunSayisi is null or >= 7,
            SonKontrolGunSayisi = sonKontrolGunSayisi
        };

        return View(model);
    }
}
