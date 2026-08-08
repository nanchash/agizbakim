using AgizDisSagligi.DataAccess;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Business;

public class NotServisi
{
    private readonly INotRepository _repo;

    public NotServisi(INotRepository repo)
    {
        _repo = repo;
    }

    public List<Not> ListeleKullaniciIle(int kullaniciId) => _repo.ListeleKullaniciIle(kullaniciId);

    public (bool basarili, string mesaj) Ekle(int kullaniciId, string aciklama, string gorselYolu)
    {
        if (string.IsNullOrWhiteSpace(aciklama))
            return (false, "Açıklama zorunludur.");

        var not = new Not
        {
            KullaniciId = kullaniciId,
            Aciklama = aciklama,
            GorselYolu = gorselYolu,
            EklenmeTarihi = DateTime.Now
        };
        _repo.Ekle(not);
        return (true, "Not eklendi.");
    }

    public void Sil(int id) => _repo.Sil(id);
}
