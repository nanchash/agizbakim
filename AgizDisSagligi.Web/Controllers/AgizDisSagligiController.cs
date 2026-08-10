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
        var hedefler = _hedefServisi.ListeleKullaniciIle(kullaniciId);

        var hedefDurumlari = hedefler.ToDictionary(h => h.Id, h => _hedefServisi.DurumuHesapla(h));

        var model = new AgizDisSagligiViewModel
        {
            Hedefler = hedefler,
            Notlar = _notServisi.ListeleKullaniciIle(kullaniciId),
            RastgeleOneri = _oneriServisi.RastgeleGetir(),
            HedefDurumlari = hedefDurumlari
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
        var (basarili, mesaj) = _hedefServisi.Sil(id, GetirKullaniciId());
        return Json(new { basarili, mesaj });
    }

    [HttpPost]
    public IActionResult DurumKaydiEkle(DurumKaydiEkleViewModel model)
    {
        var kullaniciId = GetirKullaniciId();
        var hedef = _hedefServisi.GetirId(model.HedefId);
        if (hedef == null || hedef.KullaniciId != kullaniciId)
            return Json(new { basarili = false, mesaj = "Hedef bulunamadı." });

        var (basarili, mesaj) = _durumKaydiServisi.Ekle(model.HedefId, model.Tarih, model.Saat, model.Sure, model.Uygulandi, model.FircalamaTuru);

        if (basarili && model.Uygulandi && model.FircalamaTuru != null && model.FircalamaTuru.Contains("Yumuşak", StringComparison.OrdinalIgnoreCase))
        {
            var hafifHedef = _hedefServisi.ListeleKullaniciIle(kullaniciId)
                .FirstOrDefault(h => h.Id != model.HedefId && h.Baslik.Contains("Hafif", StringComparison.OrdinalIgnoreCase) && h.Baslik.Contains("Fırçala", StringComparison.OrdinalIgnoreCase));

            if (hafifHedef != null)
                _durumKaydiServisi.Ekle(hafifHedef.Id, model.Tarih, model.Saat, model.Sure, true, model.FircalamaTuru);
        }

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
        var (basarili, mesaj) = _notServisi.Sil(id, GetirKullaniciId());
        return Json(new { basarili, mesaj });
    }
}
