using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public interface IDurumKaydiRepository
{
    List<DurumKaydi> ListeleHedefIle(int hedefId);
    bool HedefinKaydiVarMi(int hedefId);
    void Ekle(DurumKaydi durumKaydi);
}
