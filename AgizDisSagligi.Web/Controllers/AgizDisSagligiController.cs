using System.Security.Claims;
using AgizDisSagligi.Business;
using AgizDisSagligi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgizDisSagligi.Web.Controllers;

[Authorize]
public class AgizDisSagligiController : Controller
{
    private readonly HedefServisi _hedefServisi;
    private readonly DurumKaydiServisi _durumKaydiServisi;
    private readonly NotServisi _notServisi;
    private readonly OneriServisi _oneriServisi;
    private readonly IWebHostEnvironment _ortam;

    public AgizDisSagligiController(
        HedefServisi hedefServisi,
        DurumKaydiServisi durumKaydiServisi,
        NotServisi notServisi,
        OneriServisi oneriServisi,
        IWebHostEnvironment ortam)
    {
        _hedefServisi = hedefServisi;
        _durumKaydiServisi = durumKaydiServisi;
        _notServisi = notServisi;
        _oneriServisi = oneriServisi;
        _ortam = ortam;
    }

    private int GetirKullaniciId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    public IActionResult Index()
    {
        var kullaniciId = GetirKullaniciId();
        var model = new AgizDisSagligiViewModel
        {
            Hedefler = _hedefServisi.ListeleKullaniciIle(kullaniciId),
            Notlar = _notServisi.ListeleKullaniciIle(kullaniciId),
            RastgeleOneri = _oneriServisi.RastgeleGetir()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult HedefEkle(HedefEkleViewModel model)
    {
        var (basarili, mesaj) = _hedefServisi.Ekle(GetirKullaniciId(), model.Baslik, model.Aciklama, model.PeriyotZaman, model.PeriyotSiklik, model.OnemDerecesi);
        return Json(new { basarili, mesaj });
    }

    [HttpPost]
    public IActionResult HedefinDurumKaydiVarMi(int id)
    {
        return Json(new { varMi = _hedefServisi.DurumKaydiVarMi(id) });
    }

    [HttpPost]
    public IActionResult HedefSil(int id)
    {
        var (basarili, mesaj) = _hedefServisi.Sil(id);
        return Json(new { basarili, mesaj });
    }

    [HttpPost]
    public IActionResult DurumKaydiEkle(DurumKaydiEkleViewModel model)
    {
        var (basarili, mesaj) = _durumKaydiServisi.Ekle(model.HedefId, model.Tarih, model.Saat, model.Sure, model.Uygulandi);
        return Json(new { basarili, mesaj });
    }

    [HttpPost]
    public async Task<IActionResult> NotEkle(NotEkleViewModel model)
    {
        string gorselYolu = null;
        if (model.Gorsel != null && model.Gorsel.Length > 0)
        {
            var uploadsKlasoru = Path.Combine(_ortam.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsKlasoru);

            var dosyaAdi = $"{Guid.NewGuid()}{Path.GetExtension(model.Gorsel.FileName)}";
            var tamYol = Path.Combine(uploadsKlasoru, dosyaAdi);

            using (var stream = new FileStream(tamYol, FileMode.Create))
            {
                await model.Gorsel.CopyToAsync(stream);
            }

            gorselYolu = $"/uploads/{dosyaAdi}";
        }

        var (basarili, mesaj) = _notServisi.Ekle(GetirKullaniciId(), model.Aciklama, gorselYolu);
        return Json(new { basarili, mesaj });
    }

    [HttpPost]
    public IActionResult NotSil(int id)
    {
        _notServisi.Sil(id);
        return Json(new { basarili = true });
    }
}
