using System.ComponentModel.DataAnnotations;

namespace AgizDisSagligi.Web.Models;

public class ParolaHatirlatViewModel
{
    [Required(ErrorMessage = "Mail adresi zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir mail giriniz.")]
    public string Mail { get; set; }
}
