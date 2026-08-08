using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public class KullaniciRepository : IKullaniciRepository
{
    private readonly AppDbContext _context;
    public KullaniciRepository(AppDbContext context) { _context = context; }

    public Kullanici GetirMailIle(string mail) =>
        _context.Kullanicilar.FirstOrDefault(k => k.Mail == mail);

    public Kullanici GetirId(int id) =>
        _context.Kullanicilar.FirstOrDefault(k => k.Id == id);

    public void Ekle(Kullanici kullanici)
    {
        _context.Kullanicilar.Add(kullanici);
        _context.SaveChanges();
    }

    public void Guncelle(Kullanici kullanici)
    {
        _context.Kullanicilar.Update(kullanici);
        _context.SaveChanges();
    }
}
