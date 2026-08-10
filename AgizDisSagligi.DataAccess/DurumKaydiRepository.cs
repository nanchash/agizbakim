using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public class DurumKaydiRepository : IDurumKaydiRepository
{
    private readonly AppDbContext _context;
    public DurumKaydiRepository(AppDbContext context) { _context = context; }

    public List<DurumKaydi> ListeleHedefIle(int hedefId) =>
        _context.DurumKayitlari.Where(d => d.HedefId == hedefId).ToList();

    public List<DurumKaydi> ListeleKullaniciIle(int kullaniciId) =>
        _context.DurumKayitlari.Where(d => d.Hedef.KullaniciId == kullaniciId).ToList();

    public bool HedefinKaydiVarMi(int hedefId) =>
        _context.DurumKayitlari.Any(d => d.HedefId == hedefId);

    public void Ekle(DurumKaydi durumKaydi)
    {
        _context.DurumKayitlari.Add(durumKaydi);
        _context.SaveChanges();
    }
}
