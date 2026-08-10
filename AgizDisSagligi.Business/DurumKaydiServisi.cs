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

    public List<DurumKaydi> ListeleKullaniciIle(int kullaniciId) => _repo.ListeleKullaniciIle(kullaniciId);

    public int GunlukSeriHesapla(int kullaniciId)
    {
        var tamamlananGunler = _repo.ListeleKullaniciIle(kullaniciId)
            .Where(d => d.Uygulandi)
            .Select(d => d.Tarih.Date)
            .Distinct()
            .ToHashSet();

        if (tamamlananGunler.Count == 0)
            return 0;

        var bugun = DateTime.Now.Date;
        var baslangic = tamamlananGunler.Contains(bugun) ? bugun : bugun.AddDays(-1);

        if (!tamamlananGunler.Contains(baslangic))
            return 0;

        var seri = 0;
        var gun = baslangic;
        while (tamamlananGunler.Contains(gun))
        {
            seri++;
            gun = gun.AddDays(-1);
        }
        return seri;
    }

    public List<GunlukOzet> SonYediGunOzeti(int kullaniciId)
    {
        var kayitlar = _repo.ListeleKullaniciIle(kullaniciId);
        var bugun = DateTime.Now.Date;

        return Enumerable.Range(0, 7)
            .Select(i => bugun.AddDays(-i))
            .OrderBy(tarih => tarih)
            .Select(tarih => new GunlukOzet
            {
                Tarih = tarih,
                KayitSayisi = kayitlar.Count(d => d.Tarih.Date == tarih),
                TamamlananSayisi = kayitlar.Count(d => d.Tarih.Date == tarih && d.Uygulandi)
            })
            .ToList();
    }

    public (bool basarili, string mesaj) Ekle(int hedefId, DateTime tarih, TimeSpan saat, int sure, bool uygulandi, string? fircalamaTuru)
    {
        if (tarih > DateTime.Now.Date)
            return (false, "Gelecek bir tarih için durum kaydı girilemez.");

        var durumKaydi = new DurumKaydi
        {
            HedefId = hedefId,
            Tarih = tarih,
            Saat = saat,
            Sure = sure,
            Uygulandi = uygulandi,
            FircalamaTuru = fircalamaTuru
        };
        _repo.Ekle(durumKaydi);
        return (true, "Durum kaydı eklendi.");
    }
}
