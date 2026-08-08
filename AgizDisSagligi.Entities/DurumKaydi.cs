namespace AgizDisSagligi.Entities;

public class DurumKaydi
{
    public int Id { get; set; }
    public int HedefId { get; set; }
    public Hedef Hedef { get; set; }
    public DateTime Tarih { get; set; }
    public TimeSpan Saat { get; set; }
    public int Sure { get; set; }
    public bool Uygulandi { get; set; }
}
