using AgizDisSagligi.DataAccess;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Business;

public class OneriServisi
{
    private readonly IOneriRepository _repo;

    public OneriServisi(IOneriRepository repo)
    {
        _repo = repo;
    }

    public Oneri RastgeleGetir()
    {
        var tumOneriler = _repo.ListeleTumu();
        if (tumOneriler.Count == 0)
            return null;

        var rastgele = new Random();
        return tumOneriler[rastgele.Next(tumOneriler.Count)];
    }
}
