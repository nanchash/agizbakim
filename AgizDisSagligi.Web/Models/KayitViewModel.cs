using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligi.Web.Models;

public class KayitViewModel
{
    [Required(ErrorMessage = "Mail adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir mail giriniz.")]
    public string Mail { get; set; }

    [Required(ErrorMessage = "Parola zorunludur.")]
    [DataType(DataType.Password)]
    public string Parola { get; set; }

    [Required(ErrorMessage = "Parola tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Compare("Parola", ErrorMessage = "Parolalar eşleşmiyor.")]
    public string ParolaTekrar { get; set; }

    [Required(ErrorMessage = "Ad soyad zorunludur.")]
    public string AdSoyad { get; set; }

    [Required(ErrorMessage = "Doğum tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime DogumTarihi { get; set; }
}
