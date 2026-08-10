# Ağız ve Diş Sağlığı Takip Uygulaması

Diş fırçalama alışkanlıklarını, hedefleri ve ağız sağlığıyla ilgili notları takip etmeye yarayan bir ASP.NET Core MVC uygulaması.

## Özellikler

- **Hedef belirleme**: Fırçalama, diş ipi gibi alışkanlıklar için periyot (günde/haftada/ayda/yılda) ve sıklık bazlı kişisel hedefler oluşturma
- **Durum kaydı**: Hedeflerin günlük/periyodik olarak tamamlanma durumunu işaretleme, geciken veya bugün zamanı gelen hedefleri görme
- **Notlar**: Ağız sağlığıyla ilgili gözlemleri metin ve isteğe bağlı görselle kaydetme
- **Rozetler ve günlük seri (streak)**: Düzenli takip alışkanlığını ödüllendiren rozet ve seri sistemi
- **Günlük öneriler**: Rastgele ağız/diş sağlığı önerileri
- **Hesap yönetimi**: Kayıt, giriş, parola hatırlatma/sıfırlama (cookie tabanlı kimlik doğrulama, AES ile şifrelenmiş parola saklama)
- **Profil**: Kullanıcı bilgileri ve kazanılan rozetlerin görüntülenmesi

## Teknoloji Yığını

- .NET 8 / ASP.NET Core MVC
- Entity Framework Core (SQL Server)
- Bootstrap 5 + özgün neon/cam (glassmorphism) temalı CSS

## Proje Yapısı

| Proje | Sorumluluk |
|---|---|
| `AgizDisSagligi.Entities` | Veritabanı varlıkları (Kullanıcı, Hedef, DurumKaydı, Not, Öneri...) |
| `AgizDisSagligi.DataAccess` | EF Core `DbContext`, repository'ler ve migration'lar |
| `AgizDisSagligi.Business` | İş mantığı servisleri (hedef durumu hesaplama, şifreleme, mail, rozet vb.) |
| `AgizDisSagligi.Web` | MVC controller'ları, view'lar ve statik dosyalar |

## Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Erişilebilir bir SQL Server örneği (yerel veya uzak)

### Adımlar

1. `AgizDisSagligi.Web/appsettings.json` içindeki `ConnectionStrings:VarsayilanBaglanti` değerini kendi SQL Server bağlantı bilgilerinle güncelle.
2. Veritabanı migration'larını uygula:

   ```bash
   dotnet ef database update --project AgizDisSagligi.DataAccess --startup-project AgizDisSagligi.Web
   ```

3. Uygulamayı başlat:

   ```bash
   dotnet run --project AgizDisSagligi.Web
   ```

4. Tarayıcıdan `http://localhost:5215` adresine git.

### Mail Ayarları (isteğe bağlı)

Kayıt sonrası hoş geldin maili ve parola hatırlatma özellikleri için `appsettings.json` içindeki `MailAyarlari` bölümünü (SMTP sunucu, gönderici mail/adı, uygulama şifresi) doldurman gerekir. Boş bırakılırsa mail gönderimi sessizce atlanır, kayıt/giriş akışı etkilenmez.
