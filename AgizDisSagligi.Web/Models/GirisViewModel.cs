using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligi.Web.Models;

public class GirisViewModel
{
    [Required(ErrorMessage = "Mail adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir mail giriniz.")]
    public string Mail { get; set; }

    [Required(ErrorMessage = "Parola zorunludur.")]
    [DataType(DataType.Password)]
    public string Parola { get; set; }
}
