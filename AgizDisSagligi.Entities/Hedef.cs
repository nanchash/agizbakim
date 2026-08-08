namespace AgizDisSagligi.Entities;

public class Hedef
{
    public int Id { get; set; }
    public int KullaniciId { get; set; }
    public Kullanici Kullanici { get; set; }
    public string Baslik { get; set; }
    public string Aciklama { get; set; }
    public string PeriyotZaman { get; set; }
    public int PeriyotSiklik { get; set; }
    public int OnemDerecesi { get; set; }

    public List<DurumKaydi> DurumKayitlari { get; set; }
}
