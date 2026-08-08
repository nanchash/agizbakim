namespace AgizDisSagligi.Entities;

public class Kullanici
{
    public int Id { get; set; }
    public string Mail { get; set; }
    public string ParolaSifreli { get; set; }
    public string AdSoyad { get; set; }
    public DateTime DogumTarihi { get; set; }
    public DateTime KayitTarihi { get; set; }

    public List<Hedef> Hedefler { get; set; }
    public List<Not> Notlar { get; set; }
}
