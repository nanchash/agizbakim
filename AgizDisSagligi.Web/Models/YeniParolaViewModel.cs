using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligi.Web.Models;

public class YeniParolaViewModel
{
    [Required]
    public string Mail { get; set; }

    [Required(ErrorMessage = "Doğrulama kodu zorunludur.")]
    public string Kod { get; set; }

    [Required(ErrorMessage = "Yeni parola zorunludur.")]
    [DataType(DataType.Password)]
    public string YeniParola { get; set; }

    [Required(ErrorMessage = "Parola tekrarı zorunludur.")]
    [DataType(DataType.Password)]
    [Compare("YeniParola", ErrorMessage = "Parolalar eşleşmiyor.")]
    public string YeniParolaTekrar { get; set; }
}
