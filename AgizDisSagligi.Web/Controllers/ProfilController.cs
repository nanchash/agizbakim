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

    public ProfilController(KullaniciServisi kullaniciServisi)
    {
        _kullaniciServisi = kullaniciServisi;
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
            KayitTarihi = kullanici.KayitTarihi
        };

        return View(model);
    }
}
