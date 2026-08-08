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

    public (bool basarili, string mesaj) Ekle(int kullaniciId, string baslik, string aciklama, string periyotZaman, int periyotSiklik, int onemDerecesi)
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

    public (bool basarili, string mesaj) Sil(int id)
    {
        var hedef = _repo.GetirId(id);
        if (hedef == null)
            return (false, "Hedef bulunamadı.");

        _repo.Sil(id);
        return (true, "Hedef silindi.");
    }

    public bool DurumKaydiVarMi(int hedefId) => _durumRepo.HedefinKaydiVarMi(hedefId);
}
