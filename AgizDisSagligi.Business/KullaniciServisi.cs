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

    public (bool basarili, string mesaj) ParolaSifirlamaKoduGonder(string mail)
    {
        var kullanici = _repo.GetirMailIle(mail);
        if (kullanici == null)
            return (false, "Bu mail adresine kayıtlı kullanıcı bulunamadı.");

        var kod = Random.Shared.Next(100000, 999999).ToString();
        kullanici.ParolaSifirlamaKodu = kod;
        kullanici.ParolaSifirlamaKoduGecerlilik = DateTime.Now.AddMinutes(15);
        _repo.Guncelle(kullanici);

        try
        {
            _mailServisi.GonderParolaSifirlamaKodu(kullanici.Mail, kullanici.AdSoyad, kod);
        }
        catch
        {
            return (false, "Doğrulama kodu gönderilemedi. Lütfen daha sonra tekrar deneyin.");
        }

        return (true, "Doğrulama kodu mail adresinize gönderildi.");
    }

    public (bool basarili, string mesaj) ParolaSifirlamaKoduDogrula(string mail, string kod)
    {
        var kullanici = _repo.GetirMailIle(mail);
        if (kullanici == null || kullanici.ParolaSifirlamaKodu == null)
            return (false, "Önce doğrulama kodu isteyin.");

        if (kullanici.ParolaSifirlamaKoduGecerlilik < DateTime.Now)
            return (false, "Doğrulama kodunun süresi doldu. Lütfen yeni bir kod isteyin.");

        if (kullanici.ParolaSifirlamaKodu != kod)
            return (false, "Doğrulama kodu hatalı.");

        return (true, "Kod doğrulandı.");
    }

    public (bool basarili, string mesaj) ParolaSifirla(string mail, string kod, string yeniParola, string yeniParolaTekrar)
    {
        var kullanici = _repo.GetirMailIle(mail);
        if (kullanici == null)
            return (false, "Bu mail adresine kayıtlı kullanıcı bulunamadı.");

        if (kullanici.ParolaSifirlamaKodu == null || kullanici.ParolaSifirlamaKoduGecerlilik < DateTime.Now || kullanici.ParolaSifirlamaKodu != kod)
            return (false, "Doğrulama kodu hatalı ya da süresi dolmuş. Lütfen yeniden kod isteyin.");

        if (yeniParola.Length < 8 || !Regex.IsMatch(yeniParola, @"[A-Z]") || !Regex.IsMatch(yeniParola, @"[a-z]") || !Regex.IsMatch(yeniParola, @"[0-9]"))
            return (false, "Parola en az 8 karakter olmalı, büyük-küçük harf ve rakam içermelidir.");

        if (yeniParola != yeniParolaTekrar)
            return (false, "Parolalar eşleşmiyor.");

        kullanici.ParolaSifreli = _sifreleme.Sifrele(yeniParola);
        kullanici.ParolaSifirlamaKodu = null;
        kullanici.ParolaSifirlamaKoduGecerlilik = null;
        _repo.Guncelle(kullanici);
        return (true, "Parolanız güncellendi.");
    }

    public (bool basarili, string mesaj) ProfilGuncelle(int kullaniciId, string mail, string adSoyad, DateTime dogumTarihi, string? mevcutParola, string? yeniParola, string? yeniParolaTekrar)
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
            if (string.IsNullOrEmpty(mevcutParola) || _sifreleme.SifreCoz(kullanici.ParolaSifreli) != mevcutParola)
                return (false, "Mevcut parolanız hatalı.");

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
