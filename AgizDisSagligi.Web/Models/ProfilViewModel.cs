using AgizDisSagligi.Business;

namespace AgizDisSagligi.Web.Models;

public class ProfilViewModel
{
    public string Mail { get; set; }
    public string AdSoyad { get; set; }
    public DateTime DogumTarihi { get; set; }
    public DateTime KayitTarihi { get; set; }
    public int GunlukSeri { get; set; }
    public int ToplamHedefSayisi { get; set; }
    public int ToplamTamamlananSayisi { get; set; }
    public int ToplamNotSayisi { get; set; }
    public List<Rozet> Rozetler { get; set; }
}
