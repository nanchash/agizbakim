using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AgizDisSagligi.Business;

public class MailServisi
{
    private readonly MailAyarlari _ayarlar;

    public MailServisi(IOptions<MailAyarlari> ayarlar)
    {
        _ayarlar = ayarlar.Value;
    }

    public void GonderKayitMaili(string aliciMail, string adSoyad)
    {
        var mesaj = new MimeMessage();
        mesaj.From.Add(new MailboxAddress(_ayarlar.GondericiAdi, _ayarlar.GondericiMail));
        mesaj.To.Add(new MailboxAddress(adSoyad, aliciMail));
        mesaj.Subject = "Kaydınız Tamamlandı";
        mesaj.Body = new TextPart("html") { Text = $"<h2>Merhaba {adSoyad},</h2><p>Kaydınız başarıyla oluşturuldu.</p>" };

        using var client = new SmtpClient();
        client.Connect(_ayarlar.SmtpSunucu, _ayarlar.SmtpPort, false);
        client.Authenticate(_ayarlar.GondericiMail, _ayarlar.UygulamaSifresi);
        client.Send(mesaj);
        client.Disconnect(true);
    }

    public void GonderParolaSifirlamaKodu(string aliciMail, string adSoyad, string kod)
    {
        var mesaj = new MimeMessage();
        mesaj.From.Add(new MailboxAddress(_ayarlar.GondericiAdi, _ayarlar.GondericiMail));
        mesaj.To.Add(new MailboxAddress(adSoyad, aliciMail));
        mesaj.Subject = "Parola Sıfırlama Kodu";
        mesaj.Body = new TextPart("html") { Text = $"<h2>Merhaba {adSoyad},</h2><p>Parolanızı sıfırlamak için doğrulama kodunuz:</p><h1>{kod}</h1><p>Bu kod 15 dakika geçerlidir. Bu isteği siz yapmadıysanız bu maili yok sayabilirsiniz.</p>" };

        using var client = new SmtpClient();
        client.Connect(_ayarlar.SmtpSunucu, _ayarlar.SmtpPort, false);
        client.Authenticate(_ayarlar.GondericiMail, _ayarlar.UygulamaSifresi);
        client.Send(mesaj);
        client.Disconnect(true);
    }
}
