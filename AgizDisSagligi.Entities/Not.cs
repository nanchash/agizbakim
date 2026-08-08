namespace AgizDisSagligi.Entities;

public class Not
{
    public int Id { get; set; }
    public int KullaniciId { get; set; }
    public Kullanici Kullanici { get; set; }
    public string Aciklama { get; set; }
    public string? GorselYolu { get; set; }
    public DateTime EklenmeTarihi { get; set; }
}
