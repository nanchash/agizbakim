using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public interface INotRepository
{
    Not GetirId(int id);
    List<Not> ListeleKullaniciIle(int kullaniciId);
    void Ekle(Not not);
    void Sil(int id);
}
