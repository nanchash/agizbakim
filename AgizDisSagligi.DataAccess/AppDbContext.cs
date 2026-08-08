using AgizDisSagligi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgizDisSagligi.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Kullanici> Kullanicilar { get; set; }
    public DbSet<Hedef> Hedefler { get; set; }
    public DbSet<DurumKaydi> DurumKayitlari { get; set; }
    public DbSet<Not> Notlar { get; set; }
    public DbSet<Oneri> Oneriler { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kullanici>()
            .HasIndex(k => k.Mail)
            .IsUnique();

        modelBuilder.Entity<Oneri>().HasData(
            new Oneri { Id = 1, Metin = "Dişlerinizi günde en az iki kez, 2 dakika süreyle fırçalayın." },
            new Oneri { Id = 2, Metin = "Diş ipini her gün kullanmayı unutmayın." },
            new Oneri { Id = 3, Metin = "Fırçanızı her 3 ayda bir değiştirin." },
            new Oneri { Id = 4, Metin = "Şekerli ve asitli içeceklerin tüketimini azaltın." },
            new Oneri { Id = 5, Metin = "Yılda en az bir kez diş hekimi kontrolüne gidin." },
            new Oneri { Id = 6, Metin = "Fırçalamadan sonra ağız gargarası kullanmayı düşünün." },
            new Oneri { Id = 7, Metin = "Sert kıllı yerine yumuşak kıllı diş fırçası tercih edin." }
        );
    }
}
