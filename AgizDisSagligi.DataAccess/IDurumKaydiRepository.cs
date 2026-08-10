using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public interface IDurumKaydiRepository
{
    List<DurumKaydi> ListeleHedefIle(int hedefId);
    List<DurumKaydi> ListeleKullaniciIle(int kullaniciId);
    bool HedefinKaydiVarMi(int hedefId);
    void Ekle(DurumKaydi durumKaydi);
}
