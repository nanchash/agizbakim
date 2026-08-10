using AgizDisSagligi.Business;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Web.Models;

public class AgizDisSagligiViewModel
{
    public List<Hedef> Hedefler { get; set; }
    public List<Not> Notlar { get; set; }
    public Oneri RastgeleOneri { get; set; }
    public Dictionary<int, HedefDurumu> HedefDurumlari { get; set; }
    public List<GunlukOzet> SonYediGun { get; set; }
}

public class HedefEkleViewModel
{
    public string Baslik { get; set; }
    public string Aciklama { get; set; }
    public string PeriyotZaman { get; set; }
    public int PeriyotSiklik { get; set; }
    public int OnemDerecesi { get; set; }
}

public class DurumKaydiEkleViewModel
{
    public int HedefId { get; set; }
    public DateTime Tarih { get; set; }
    public TimeSpan Saat { get; set; }
    public int Sure { get; set; }
    public bool Uygulandi { get; set; }
    public string? FircalamaTuru { get; set; }
}

public class NotEkleViewModel
{
    public string Aciklama { get; set; }
    public IFormFile Gorsel { get; set; }
}
