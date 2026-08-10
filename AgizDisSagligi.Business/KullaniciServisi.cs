using System.Text.RegularExpressions;
using AgizDisSagligi.DataAccess;
using AgizDisSagligi.Entities;

namespace AgizDisSagligi.Business;

public class KullaniciServisi
{
    private readonly IKullaniciRepository _repo;
    private readonly SifrelemeServisi _sifreleme;
    private readonly MailServisi _mailServisi;

    public KullaniciServisi(IKullaniciRepository repo, SifrelemeServisi sifreleme, MailServisi mailServisi)
    {
        _repo = repo;
        _sifreleme = sifreleme;
        _mailServisi = mailServisi;
    }

    public (bool basarili, string mesaj) KayitOl(string mail, string parola, string parolaTekrar, string adSoyad, DateTime dogumTarihi)
    {
        if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return (false, "Geçerli bir mail adresi giriniz.");

        if (parola.Length < 8 || !Regex.IsMatch(parola, @"[A-Z]") || !Regex.IsMatch(parola, @"[a-z]") || !Regex.IsMatch(parola, @"[0-9]"))
            return (false, "Parola en az 8 karakter olmalı, büyük-küçük harf ve rakam içermelidir.");

        if (parola != parolaTekrar)
            return (false, "Parolalar eşleşmiyor.");

        if (dogumTarihi >= DateTime.Now)
            return (false, "Geçerli bir doğum tarihi giriniz.");

        if (_repo.GetirMailIle(mail) != null)
            return (false, "Bu mail adresi zaten kayıtlı.");

        var kullanici = new Kullanici
        {
            Mail = mail,
            ParolaSifreli = _sifreleme.Sifrele(parola),
            AdSoyad = adSoyad,
            DogumTarihi = dogumTarihi,
            KayitTarihi = DateTime.Now
        };
        _repo.Ekle(kullanici);

        try
        {
            _mailServisi.GonderKayitMaili(kullanici.Mail, kullanici.AdSoyad);
        }
        catch
        {
            // Mail gönderimi başarısız olsa bile kayıt işlemi tamamlanmış sayılır.
        }

        return (true, "Kayıt başarılı.");
    }

    public (bool basarili, string mesaj, Kullanici kullanici) GirisYap(string mail, string parola)
    {
        var kullanici = _repo.GetirMailIle(mail);
        if (kullanici == null)
            return (false, "Bu mail adresine kayıtlı kullanıcı bulunamadı.", null);

        var cozulmusParola = _sifreleme.SifreCoz(kullanici.ParolaSifreli);
        if (cozulmusParola != parola)
            return (false, "Parola hatalı.", null);

        return (true, "Giriş başarılı.", kullanici);
    }

    public Kullanici MailIleBul(string mail) => _repo.GetirMailIle(mail);

    public Kullanici IdIleBul(int id) => _repo.GetirId(id);

    public (bool basarili, string mesaj) ParolaSifirla(string mail, string yeniParola, string yeniParolaTekrar)
    {
        var kullanici = _repo.GetirMailIle(mail);
        if (kullanici == null)
            return (false, "Bu mail adresine kayıtlı kullanıcı bulunamadı.");

        if (yeniParola.Length < 8 || !Regex.IsMatch(yeniParola, @"[A-Z]") || !Regex.IsMatch(yeniParola, @"[a-z]") || !Regex.IsMatch(yeniParola, @"[0-9]"))
            return (false, "Parola en az 8 karakter olmalı, büyük-küçük harf ve rakam içermelidir.");

        if (yeniParola != yeniParolaTekrar)
            return (false, "Parolalar eşleşmiyor.");

        kullanici.ParolaSifreli = _sifreleme.Sifrele(yeniParola);
        _repo.Guncelle(kullanici);
        return (true, "Parolanız güncellendi.");
    }

    public (bool basarili, string mesaj) ProfilGuncelle(int kullaniciId, string mail, string adSoyad, DateTime dogumTarihi, string? yeniParola, string? yeniParolaTekrar)
    {
        if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return (false, "Geçerli bir mail adresi giriniz.");

        if (dogumTarihi >= DateTime.Now)
            return (false, "Geçerli bir doğum tarihi giriniz.");

        var mailliKullanici = _repo.GetirMailIle(mail);
        if (mailliKullanici != null && mailliKullanici.Id != kullaniciId)
            return (false, "Bu mail adresi başka bir kullanıcıya kayıtlı.");

        var kullanici = _repo.GetirId(kullaniciId);
        if (kullanici == null)
            return (false, "Kullanıcı bulunamadı.");

        if (!string.IsNullOrEmpty(yeniParola) || !string.IsNullOrEmpty(yeniParolaTekrar))
        {
            if (yeniParola == null || yeniParola.Length < 8 || !Regex.IsMatch(yeniParola, @"[A-Z]") || !Regex.IsMatch(yeniParola, @"[a-z]") || !Regex.IsMatch(yeniParola, @"[0-9]"))
                return (false, "Parola en az 8 karakter olmalı, büyük-küçük harf ve rakam içermelidir.");

            if (yeniParola != yeniParolaTekrar)
                return (false, "Parolalar eşleşmiyor.");

            kullanici.ParolaSifreli = _sifreleme.Sifrele(yeniParola);
        }

        kullanici.Mail = mail;
        kullanici.AdSoyad = adSoyad;
        kullanici.DogumTarihi = dogumTarihi;
        _repo.Guncelle(kullanici);
        return (true, "Profiliniz güncellendi.");
    }
}
