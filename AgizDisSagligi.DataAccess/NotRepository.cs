using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public class NotRepository : INotRepository
{
    private readonly AppDbContext _context;
    public NotRepository(AppDbContext context) { _context = context; }

    public List<Not> ListeleKullaniciIle(int kullaniciId) =>
        _context.Notlar.Where(n => n.KullaniciId == kullaniciId).ToList();

    public void Ekle(Not not)
    {
        _context.Notlar.Add(not);
        _context.SaveChanges();
    }

    public void Sil(int id)
    {
        var not = _context.Notlar.FirstOrDefault(n => n.Id == id);
        if (not != null)
        {
            _context.Notlar.Remove(not);
            _context.SaveChanges();
        }
    }
}
