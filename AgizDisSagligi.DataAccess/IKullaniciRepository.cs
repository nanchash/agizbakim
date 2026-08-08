using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public interface IKullaniciRepository
{
    Kullanici GetirMailIle(string mail);
    Kullanici GetirId(int id);
    void Ekle(Kullanici kullanici);
    void Guncelle(Kullanici kullanici);
}
