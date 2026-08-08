using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public interface IHedefRepository
{
    Hedef GetirId(int id);
    List<Hedef> ListeleKullaniciIle(int kullaniciId);
    void Ekle(Hedef hedef);
    void Guncelle(Hedef hedef);
    void Sil(int id);
}
