using System.Security.Cryptography;
using System.Text;

namespace AgizDisSagligi.Business;

public class SifrelemeServisi
{
    private static readonly string AnahtarMetin = "Ta0dkBzxgMGWs1IQvlYnyDZiF8PuqS2h"; // 32 karakter, AES-256 için

    public string Sifrele(string duzMetin)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(AnahtarMetin);
        aes.GenerateIV();
        using var encryptor = aes.CreateEncryptor();
        var duzBytes = Encoding.UTF8.GetBytes(duzMetin);
        var sifreliBytes = encryptor.TransformFinalBlock(duzBytes, 0, duzBytes.Length);
        var sonuc = new byte[aes.IV.Length + sifreliBytes.Length];
        Buffer.BlockCopy(aes.IV, 0, sonuc, 0, aes.IV.Length);
        Buffer.BlockCopy(sifreliBytes, 0, sonuc, aes.IV.Length, sifreliBytes.Length);
        return Convert.ToBase64String(sonuc);
    }

    public string SifreCoz(string sifreliMetin)
    {
        var tumBytes = Convert.FromBase64String(sifreliMetin);
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(AnahtarMetin);
        var iv = new byte[16];
        Buffer.BlockCopy(tumBytes, 0, iv, 0, iv.Length);
        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor();
        var sifreliVeri = new byte[tumBytes.Length - iv.Length];
        Buffer.BlockCopy(tumBytes, iv.Length, sifreliVeri, 0, sifreliVeri.Length);
        var duzBytes = decryptor.TransformFinalBlock(sifreliVeri, 0, sifreliVeri.Length);
        return Encoding.UTF8.GetString(duzBytes);
    }
}
