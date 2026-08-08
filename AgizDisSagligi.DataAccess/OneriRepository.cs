using AgizDisSagligi.Entities;

namespace AgizDisSagligi.DataAccess;

public class OneriRepository : IOneriRepository
{
    private readonly AppDbContext _context;
    public OneriRepository(AppDbContext context) { _context = context; }

    public List<Oneri> ListeleTumu() => _context.Oneriler.ToList();
}
