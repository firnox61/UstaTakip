# 🚗 UstaTakip

UstaTakip, araç takibi, sigorta, bakım ve kullanıcı yönetimi gibi operasyonları merkezi bir noktadan yönetmenizi sağlayan, modüler ve ölçeklenebilir bir ASP.NET Core Web API ve MVC projesidir. Proje Clean Architecture prensiplerine göre tasarlanmış olup, katmanlı yapı ile sürdürülebilir yazılım geliştirme imkanı sunar.

---

## 📌 Projenin Genel Özellikleri

* Araç kayıt, güncelleme, listeleme
* Araç resim yükleme ve yönetimi
* Sigorta poliçesi ve ödeme takibi
* Tamir/bakım geçmişi yönetimi
* Kullanıcı ve rol yönetimi (JWT tabanlı authentication)
* Loglama, cache ve validasyon sistemleri (Aspect Oriented Programming ile)
* Sağlam DTO ve servis mimarisi
* MVC arayüzü ile kullanıcı dostu web ekranları

---

## 🧱 Mimari Yapı

Proje Clean Architecture prensibine göre aşağıdaki katmanlardan oluşur:

```
UstaTakip
🔼
├── Domain         # Temel varlıklar (Entity) ve interface'ler
├── Application    # DTO'lar, servis arayüzleri ve iş mantığı
├── Infrastructure # EF Core context, repository implementasyonları, JWT ve Hash servisleri
├── Core           # Aspectler, cache, logging, validation, result modelleri
├── WebAPI         # RESTful servis uç noktaları
└── Mvc.Web        # ASP.NET Core MVC kullanıcı arayüzü (giriş, listeleme, işlem sayfaları)
```

---

## 🚀 Kurulum ve Çalıştırma

### 1. Depoyu klonlayın

```bash
git clone https://github.com/kullaniciadi/UstaTakip.git
cd UstaTakip
```

### 2. Veritabanı bağlantı ayarlarını yapın

`appsettings.json` ya da `appsettings.Development.json` dosyasında `ConnectionStrings` bölümünü kendi SQL Server bağlantınıza göre düzenleyin.

### 3. Migration ve Veritabanı Oluşturma

```bash
cd UstaTakip.WebAPI
dotnet ef database update --project ../UstaTakip.Infrastructure
```

### 4. Uygulamaları çalıştırın

```bash
# WebAPI başlatmak için:
dotnet run --project UstaTakip.WebAPI

# MVC arayüzünü başlatmak için:
dotnet run --project UstaTakipMvc.Web
```

### 5. Giriş Noktaları

* Swagger: `https://localhost:<port>/swagger`
* MVC UI: `https://localhost:<port>` (Login, araç işlemleri vb.)

---

## 🔐 Kullanılan Teknolojiler ve Paketler

* **.NET 8**
* **Entity Framework Core**
* **AutoMapper**
* **FluentValidation**
* **Autofac (Dependency Injection)**
* **JWT Authentication**
* **AOP (Aspect-Oriented Programming)**
* **Memory Cache**
* **SQL Server**
* **ASP.NET Core MVC**
* **Razor View Engine**

---

## 📂 Önemli Dizinler ve Dosyalar

| Dizin/Yol                           | Açıklama                                                             |
| ----------------------------------- | -------------------------------------------------------------------- |
| `Entities/`                         | Domain katmanındaki tüm Entity modelleri                             |
| `DTOs/`                             | Application katmanındaki Create/Update/List veri transfer modelleri  |
| `Services/Managers`                 | Business logic servisleri                                            |
| `Repositories/`                     | Veri erişim interface'leri ve EF implementasyonları                  |
| `MappingProfiles/GeneralMapping.cs` | AutoMapper konfigürasyonu                                            |
| `Validators/`                       | FluentValidation sınıfları                                           |
| `Aspects/`                          | AOP ile çalışan log, cache, validation ve security yapılarını içerir |
| `Middlewares/`                      | Hata yönetimi ve özel exception handling                             |
| `DependencyInjection/`              | Autofac bağımlılık çözümleyici yapılandırmaları                      |
| `Views/`                            | Razor View dosyaları (MVC tarafı için)                               |
| `Controllers/`                      | MVC controller'ları                                                  |

---

## ✅ Geliştirme Durumu

📌 Proje aktif geliştirme sürecindedir. Yeni modül fikirleri:

* Bildirim sistemi
* PDF fatura oluşturma
* Yetkilendirme paneli (UI tarafı)

---

## 🤝 Katkı Sağlamak

1. Fork yap
2. Yeni bir branch oluştur (`git checkout -b feature/yeniozellik`)
3. Değişikliklerini commit et (`git commit -m 'Yeni özellik eklendi'`)
4. Push et (`git push origin feature/yeniozellik`)
5. Pull Request aç 🎉

---

## 📜 Lisans

Bu proje MIT lisansı ile lisanslanmıştır. Detaylar için `LICENSE` dosyasını inceleyebilirsiniz.

---

💡 Projeyle ilgili soruların veya katkı fikirlerin varsa [issue](https://github.com/kullaniciadi/UstaTakip/issues) oluşturabilirsin.
