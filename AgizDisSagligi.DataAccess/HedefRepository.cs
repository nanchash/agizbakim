using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public class HedefRepository : IHedefRepository
{
    private readonly AppDbContext _context;
    public HedefRepository(AppDbContext context) { _context = context; }

    public Hedef GetirId(int id) =>
        _context.Hedefler.FirstOrDefault(h => h.Id == id);

    public List<Hedef> ListeleKullaniciIle(int kullaniciId) =>
        _context.Hedefler.Where(h => h.KullaniciId == kullaniciId).ToList();

    public void Ekle(Hedef hedef)
    {
        _context.Hedefler.Add(hedef);
        _context.SaveChanges();
    }

    public void Guncelle(Hedef hedef)
    {
        _context.Hedefler.Update(hedef);
        _context.SaveChanges();
    }

    public void Sil(int id)
    {
        var hedef = GetirId(id);
        if (hedef != null)
        {
            _context.Hedefler.Remove(hedef);
            _context.SaveChanges();
        }
    }
}
