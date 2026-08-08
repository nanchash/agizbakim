using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Web.Models;

public class AnaSayfaViewModel
{
    public string AdSoyad { get; set; }
    public int ToplamHedefSayisi { get; set; }
    public int BugunTamamlananSayisi { get; set; }
    public Oneri GunlukOneri { get; set; }
}
