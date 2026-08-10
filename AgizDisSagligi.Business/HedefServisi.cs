using AgizDisSagligi.DataAccess;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Business;

public class HedefServisi
{
    private readonly IHedefRepository _repo;
    private readonly IDurumKaydiRepository _durumRepo;

    public HedefServisi(IHedefRepository repo, IDurumKaydiRepository durumRepo)
    {
        _repo = repo;
        _durumRepo = durumRepo;
    }

    public List<Hedef> ListeleKullaniciIle(int kullaniciId) => _repo.ListeleKullaniciIle(kullaniciId);

    public Hedef GetirId(int id) => _repo.GetirId(id);

    public (bool basarili, string mesaj) Ekle(int kullaniciId, string baslik, string? aciklama, string periyotZaman, int periyotSiklik, int onemDerecesi)
    {
        if (string.IsNullOrWhiteSpace(baslik))
            return (false, "Başlık zorunludur.");

        if (periyotSiklik <= 0)
            return (false, "Periyot sıklığı sıfırdan büyük olmalıdır.");

        var hedef = new Hedef
        {
            KullaniciId = kullaniciId,
            Baslik = baslik,
            Aciklama = aciklama,
            PeriyotZaman = periyotZaman,
            PeriyotSiklik = periyotSiklik,
            OnemDerecesi = onemDerecesi
        };
        _repo.Ekle(hedef);
        return (true, "Hedef eklendi.");
    }

    public (bool basarili, string mesaj) Sil(int id, int kullaniciId)
    {
        var hedef = _repo.GetirId(id);
        if (hedef == null || hedef.KullaniciId != kullaniciId)
            return (false, "Hedef bulunamadı.");

        _repo.Sil(id);
        return (true, "Hedef silindi.");
    }

    public bool DurumKaydiVarMi(int hedefId) => _durumRepo.HedefinKaydiVarMi(hedefId);

    public HedefDurumu DurumuHesapla(Hedef hedef)
    {
        var tamamlananKayitlar = _durumRepo.ListeleHedefIle(hedef.Id).Where(d => d.Uygulandi).ToList();

        var cevrimGunSayisi = hedef.PeriyotZaman switch
        {
            "Günde" => 1,
            "Haftada" => 7,
            "Ayda" => 30,
            "Yılda" => 365,
            _ => 30
        };
        var araGun = hedef.PeriyotSiklik > 0 ? cevrimGunSayisi / (double)hedef.PeriyotSiklik : cevrimGunSayisi;

        if (hedef.PeriyotZaman == "Günde" || araGun <= 2)
        {
            var gunlukHedefSayisi = hedef.PeriyotZaman == "Günde"
                ? hedef.PeriyotSiklik
                : Math.Max(1, (int)Math.Round(hedef.PeriyotSiklik / (double)cevrimGunSayisi));

            var bugun = DateTime.Now.Date;
            var bugunSayisi = tamamlananKayitlar.Count(d => d.Tarih.Date == bugun);
            return new HedefDurumu
            {
                GunlukMu = true,
                BugunSayisi = bugunSayisi,
                HedefSayisi = gunlukHedefSayisi,
                TamamlandiMi = bugunSayisi >= gunlukHedefSayisi
            };
        }

        var sonKayit = tamamlananKayitlar.OrderByDescending(d => d.Tarih).FirstOrDefault();
        if (sonKayit == null)
        {
            return new HedefDurumu { GunlukMu = false, HicTamamlanmadi = true };
        }

        var sonrakiTarih = sonKayit.Tarih.Date.AddDays(Math.Round(araGun));
        var kalanGun = (sonrakiTarih - DateTime.Now.Date).Days;

        return new HedefDurumu
        {
            GunlukMu = false,
            KalanGun = kalanGun,
            TamamlandiMi = kalanGun > 0
        };
    }
}
