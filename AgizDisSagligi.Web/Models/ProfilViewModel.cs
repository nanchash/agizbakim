using System.ComponentModel.DataAnnotations;
using AgizDisSagligi.Business;

namespace AgizDisSagligi.Web.Models;

public class ProfilViewModel
{
    [Required(ErrorMessage = "Mail adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir mail giriniz.")]
    public string Mail { get; set; }

    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    public string AdSoyad { get; set; }

    [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime DogumTarihi { get; set; }

    [DataType(DataType.Password)]
    public string? YeniParola { get; set; }

    [DataType(DataType.Password)]
    [Compare("YeniParola", ErrorMessage = "Parolalar eşleşmiyor.")]
    public string? YeniParolaTekrar { get; set; }

    public DateTime KayitTarihi { get; set; }
    public int GunlukSeri { get; set; }
    public int ToplamHedefSayisi { get; set; }
    public int ToplamTamamlananSayisi { get; set; }
    public int ToplamNotSayisi { get; set; }
    public List<Rozet>? Rozetler { get; set; }
}
