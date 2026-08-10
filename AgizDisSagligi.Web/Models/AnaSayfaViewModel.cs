using AgizDisSagligi.Business;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Web.Models;

public class AnaSayfaViewModel
{
    public string AdSoyad { get; set; }
    public int ToplamHedefSayisi { get; set; }
    public int BugunTamamlananSayisi { get; set; }
    public Oneri GunlukOneri { get; set; }
    public bool HaftalikKontrolHatirlatmasi { get; set; }
    public int? SonKontrolGunSayisi { get; set; }
    public List<GunlukOzet> SonYediGun { get; set; }
}
