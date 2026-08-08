using AgizDisSagligi.DataAccess;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Business;

public class DurumKaydiServisi
{
    private readonly IDurumKaydiRepository _repo;

    public DurumKaydiServisi(IDurumKaydiRepository repo)
    {
        _repo = repo;
    }

    public List<DurumKaydi> ListeleHedefIle(int hedefId) => _repo.ListeleHedefIle(hedefId);

    public (bool basarili, string mesaj) Ekle(int hedefId, DateTime tarih, TimeSpan saat, int sure, bool uygulandi)
    {
        if (tarih > DateTime.Now.Date)
            return (false, "Gelecek bir tarih için durum kaydı girilemez.");

        var durumKaydi = new DurumKaydi
        {
            HedefId = hedefId,
            Tarih = tarih,
            Saat = saat,
            Sure = sure,
            Uygulandi = uygulandi
        };
        _repo.Ekle(durumKaydi);
        return (true, "Durum kaydı eklendi.");
    }
}
