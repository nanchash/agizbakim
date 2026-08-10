# 🦷 Ağız ve Diş Sağlığı Takip Uygulaması

Kullanıcıların diş fırçalama, diş ipi ve ağız bakımı alışkanlıklarını kişisel hedefler halinde tanımlayıp takip edebildiği, ilerlemelerini not ve rozetlerle görebildiği bir **ASP.NET Core MVC** uygulaması.

Kullanıcı yönetimi (kayıt, giriş, parola sıfırlama, oturum yönetimi) üçüncü parti bir kimlik kütüphanesi (Identity/IdentityServer vb.) kullanılmadan, sıfırdan tasarlanmıştır.

## İçindekiler

- [Özellikler](#özellikler)
- [Mimari ve Teknoloji Yığını](#mimari-ve-teknoloji-yığını)
- [Proje Yapısı](#proje-yapısı)
- [Veritabanı Şeması](#veritabanı-şeması)
- [Kurulum](#kurulum)
- [Yapılandırma](#yapılandırma)
- [Güvenlik Notları](#güvenlik-notları)

## Özellikler

### Hesap Yönetimi
- Mail formatı, parola karmaşıklığı (min. 8 karakter, büyük/küçük harf, rakam) ve mail tekilliği kontrolleriyle kayıt
- Cookie tabanlı oturum açma; hatalı mail ve hatalı parola için ayrı ayrı geri bildirim
- Aynı sayfa üzerinde AJAX ile ilerleyen parola sıfırlama akışı (mail doğrulama → yeni parola)
- AES-256 ile şifrelenmiş parola saklama

### Hedef ve Alışkanlık Takibi
- Başlık, açıklama, periyot (zaman + sıklık) ve önem derecesine göre kişisel hedef tanımlama
- Her hedef için tarih/saat/süre bilgisiyle durum kaydı girme, günlük veya periyodik ilerlemeyi otomatik hesaplama
- Son 7 günün özetini hem ana sayfada hem hedef ekranında listeleme
- İlişkili durum kaydı bulunan hedefler için silmeden önce onay isteme

### Not, Öneri, Rozet
- Açıklama ve isteğe bağlı görselle serbest not ekleme
- Rastgele seçilen günlük ağız/diş sağlığı önerileri
- Günlük seri (streak) ve ilerlemeye dayalı rozet sistemi
- Profil sayfasından hesap bilgilerini (mail, ad soyad, doğum tarihi, parola) güncelleme

## Mimari ve Teknoloji Yığını

| Katman | Teknoloji |
|---|---|
| Çalışma zamanı | .NET 8 |
| Web / MVC | ASP.NET Core MVC (Razor Views) |
| Veri erişimi | Entity Framework Core 8 (Code First + Migrations) |
| Veritabanı | Microsoft SQL Server |
| Kimlik doğrulama | ASP.NET Core Cookie Authentication (özel, Identity kullanılmadan) |
| Mail | MailKit / MimeKit (SMTP) |
| Ön yüz | Bootstrap 5, jQuery, özgün neon/cam (glassmorphism) temalı CSS |

Proje, sorumlulukların ayrıldığı **n-katmanlı mimari** ile geliştirilmiştir: sunum katmanı iş mantığına, iş mantığı veri erişimine bağımlıdır; bağımlılıklar arayüzler (`IKullaniciRepository`, `IHedefRepository` vb.) üzerinden enjekte edilir.

## Proje Yapısı

```
AgizDisSagligi/
├── AgizDisSagligi.Entities/     # POCO varlıklar: Kullanici, Hedef, DurumKaydi, Not, Oneri
├── AgizDisSagligi.DataAccess/   # AppDbContext, repository implementasyonları, EF Core migration'ları
├── AgizDisSagligi.Business/     # İş kuralları: KullaniciServisi, HedefServisi, SifrelemeServisi, RozetServisi...
├── AgizDisSagligi.Web/          # Controller'lar, Razor view'lar, wwwroot (statik dosyalar)
└── docs/                        # Ek dokümantasyon (veritabanı şeması)
```

| Proje | Sorumluluk |
|---|---|
| `AgizDisSagligi.Entities` | Veritabanı varlıkları ve aralarındaki navigasyon ilişkileri |
| `AgizDisSagligi.DataAccess` | EF Core `DbContext`, repository'ler, migration'lar |
| `AgizDisSagligi.Business` | İş mantığı servisleri: hedef durumu hesaplama, şifreleme, mail gönderimi, rozet hesaplama |
| `AgizDisSagligi.Web` | MVC controller'ları, view'lar, view model'lar, statik dosyalar |

## Veritabanı Şeması

Varlık-ilişki (ER) diyagramı ve tablo açıklamaları için [docs/veritabani-semasi.md](docs/veritabani-semasi.md) dosyasına bakın.

## Kurulum

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Erişilebilir bir SQL Server örneği (yerel veya uzak)

### Adımlar

1. Bağlantı dizesini kendi ortamına göre ayarla — `AgizDisSagligi.Web/appsettings.json` içindeki `ConnectionStrings:VarsayilanBaglanti`.

2. Veritabanını oluştur / migration'ları uygula:

   ```bash
   dotnet ef database update --project AgizDisSagligi.DataAccess --startup-project AgizDisSagligi.Web
   ```

3. Uygulamayı çalıştır:

   ```bash
   dotnet run --project AgizDisSagligi.Web
   ```

4. Tarayıcıdan aç: `http://localhost:5215`

## Yapılandırma

### Mail Ayarları (isteğe bağlı)

Kayıt sonrası hoş geldin maili göndermek için `MailAyarlari` bölümünün (SMTP sunucu, gönderici mail/adı, uygulama şifresi) doldurulması gerekir. Boş bırakılırsa mail gönderimi sessizce atlanır; kayıt/giriş akışı bundan etkilenmez.

Gerçek bir Gmail hesabıyla göndermek için bir **App Password (Uygulama Şifresi)** gerekir. Bu değer `appsettings.json`'a değil, repoya commit edilmeyen [.NET User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets)'a girilmelidir:

1. Google hesabında [2 Adımlı Doğrulama](https://myaccount.google.com/security)'yı etkinleştir.
2. [myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords) üzerinden bir uygulama şifresi oluştur.
3. `AgizDisSagligi.Web` projesine yerel gizli anahtar olarak ekle:

   ```bash
   dotnet user-secrets set "MailAyarlari:GondericiMail" "senin-mailin@gmail.com" --project AgizDisSagligi.Web
   dotnet user-secrets set "MailAyarlari:UygulamaSifresi" "16-haneli-uygulama-sifresi" --project AgizDisSagligi.Web
   ```

User Secrets yalnızca `Development` ortamında ve yalnızca bu makinede geçerlidir; `appsettings.json`'daki boş değerlerin üzerine yerel olarak yazar, git'e hiçbir zaman gitmez.

## Güvenlik Notları

- Parolalar veritabanında düz metin olarak değil, AES-256 ile şifrelenmiş şekilde saklanır (`SifrelemeServisi`).
- Kimlik doğrulama tamamen özel yazılmıştır; üçüncü parti bir Identity/IdentityServer kütüphanesi kullanılmaz.
- Gerçek kimlik bilgileri (SMTP şifresi vb.) yalnızca .NET User Secrets ile yerel olarak tutulur, hiçbir zaman kaynak koduna veya `appsettings.json`'a yazılmaz.
