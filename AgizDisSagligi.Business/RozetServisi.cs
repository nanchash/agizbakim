namespace AgizDisSagligi.Business;

public class RozetServisi
{
    private readonly HedefServisi _hedefServisi;
    private readonly NotServisi _notServisi;
    private readonly DurumKaydiServisi _durumKaydiServisi;

    public RozetServisi(HedefServisi hedefServisi, NotServisi notServisi, DurumKaydiServisi durumKaydiServisi)
    {
        _hedefServisi = hedefServisi;
        _notServisi = notServisi;
        _durumKaydiServisi = durumKaydiServisi;
    }

    public List<Rozet> Hesapla(int kullaniciId)
    {
        var hedefSayisi = _hedefServisi.ListeleKullaniciIle(kullaniciId).Count;
        var notSayisi = _notServisi.ListeleKullaniciIle(kullaniciId).Count;
        var tamamlananDurumSayisi = _durumKaydiServisi.ListeleKullaniciIle(kullaniciId).Count(d => d.Uygulandi);
        var seri = _durumKaydiServisi.GunlukSeriHesapla(kullaniciId);

        return new List<Rozet>
        {
            new Rozet { Ad = "İlk Adım", Simge = "🎯", Aciklama = "İlk hedefini oluştur", Kazanildi = hedefSayisi >= 1 },
            new Rozet { Ad = "İlk Tamamlama", Simge = "✅", Aciklama = "İlk durum kaydını tamamla", Kazanildi = tamamlananDurumSayisi >= 1 },
            new Rozet { Ad = "Not Tutkunu", Simge = "📝", Aciklama = "5 not ekle", Kazanildi = notSayisi >= 5 },
            new Rozet { Ad = "Kararlı", Simge = "💪", Aciklama = "10 durum kaydı tamamla", Kazanildi = tamamlananDurumSayisi >= 10 },
            new Rozet { Ad = "Azimli", Simge = "🎖️", Aciklama = "50 durum kaydı tamamla", Kazanildi = tamamlananDurumSayisi >= 50 },
            new Rozet { Ad = "3 Günlük Seri", Simge = "🔥", Aciklama = "3 gün üst üste hedeflerini tamamla", Kazanildi = seri >= 3 },
            new Rozet { Ad = "7 Günlük Seri", Simge = "🔥", Aciklama = "7 gün üst üste hedeflerini tamamla", Kazanildi = seri >= 7 },
            new Rozet { Ad = "30 Günlük Seri", Simge = "🏆", Aciklama = "30 gün üst üste hedeflerini tamamla", Kazanildi = seri >= 30 },
        };
    }
}
