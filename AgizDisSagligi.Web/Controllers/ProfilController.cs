using System.Security.Claims;
using AgizDisSagligi.Business;
using AgizDisSagligi.Web.Models;
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

    public IActionResult Index()
    {
        var kullaniciId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var kullanici = _kullaniciServisi.IdIleBul(kullaniciId);

        var model = new ProfilViewModel
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

        return View(model);
    }
}
