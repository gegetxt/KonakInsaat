using KonakInsaat.Data;
using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Data.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<KonakInsaatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// Services
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IContentService, ContentService>();

// Session (Admin panel için yararlı)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Desteklenen diller
var supportedCultures = new[]
{
    new CultureInfo("tr-TR"),
    new CultureInfo("en-US"),
    new CultureInfo("de-DE"),
    new CultureInfo("ru-RU")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Provider sırası önemli!
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider { QueryStringKey = "dil", UIQueryStringKey = "dil" });
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
});

// MVC + Localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/admin/Login";  // Area'ya yönlendir
        options.LogoutPath = "/Admin/admin/Logout";
        options.AccessDeniedPath = "/Admin/admin/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// Authorization Policies (İsteğe bağlı)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorOrAdmin", policy => policy.RequireRole("Admin", "Editor"));
});

var app = builder.Build();

// Default Admin Kullanıcısı Oluştur (Yoksa)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KonakInsaatContext>();
        
        // Veritabanının oluşturulduğundan emin ol
        context.Database.EnsureCreated();
        
        // Admin kullanıcısı var mı kontrol et ve şifresini güncelle
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedPassword = Convert.ToHexString(
                sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("T1"))
            ).ToLower();
            
            var adminUser = context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == "admin");
            
            if (adminUser == null)
            {
                // Yeni admin kullanıcısı oluştur
                adminUser = new Kullanici
                {
                    KullaniciAdi = "admin",
                    Sifre = hashedPassword,
                    Email = "admin@konakconstruction.net",
                    AdSoyad = "Yönetici",
                    Rol = "Admin",
                    Aktif = true,
                    SonGirisTarihi = null
                };
                
                context.Kullanicilar.Add(adminUser);
                Console.WriteLine("✅ Default admin kullanıcısı oluşturuldu!");
            }
            else
            {
                // Mevcut admin kullanıcısının şifresini güncelle
                adminUser.Sifre = hashedPassword;
                adminUser.Aktif = true; // Aktif olduğundan emin ol
                context.Kullanicilar.Update(adminUser);
                Console.WriteLine("✅ Admin kullanıcısının şifresi güncellendi!");
            }
            
            context.SaveChanges();
            Console.WriteLine("   Kullanıcı Adı: admin");
            Console.WriteLine("   Şifre: admin123");
        }

        // Ana sayfa slider'ı için başlangıç verisi
        if (!context.Sliderlar.Any())
        {
            var sliders = new List<Slider>
            {
                new Slider
                {
                    Baslik = "KONAK CONSTRUCTION",
                    AltBaslik = "Hayalini kurduğunuz evi gerçeğe dönüştürüyoruz.",
                    ResimYolu = "/upload/slider/thump_1652532067.jpeg",
                    Sira = 1,
                    Aktif = true
                },
                new Slider
                {
                    Baslik = "Konak Construction",
                    AltBaslik = "Twin Towers projemizle modern yaşamı yeniden tanımlıyoruz.",
                    ResimYolu = "/upload/slider/thump_1648078077.jpeg",
                    Sira = 2,
                    Aktif = true
                },
                new Slider
                {
                    Baslik = "KONAK CONSTRUCTION",
                    AltBaslik = "Modern yaşam alanlarıyla yarının şehirlerini bugünden kuruyoruz.",
                    ResimYolu = "/upload/slider/thump_1649859800.jpg",
                    Sira = 3,
                    Aktif = true
                }
            };

            context.Sliderlar.AddRange(sliders);
            context.SaveChanges();

            Console.WriteLine("✅ Varsayılan ana sayfa slider kayıtları eklendi.");
        }

        // Varsayılan projeleri ekle (Coming Soon / Completed / Ongoing)
        var defaultProjects = new List<Proje>
        {
            new Proje
            {
                ProjeAdi = "Twin Tower 2",
                ProjeAdi_EN = "Twin Tower 2",
                ProjeAdi_RU = "Twin Tower 2",
                ProjeDurumu = "Tamamlandı",
                KisaAciklama = "KONAK CONSTRUCTION",
                KisaAciklama_EN = "KONAK CONSTRUCTION",
                KisaAciklama_RU = "KONAK CONSTRUCTION",
                AnaResimYolu = "/upload/proje/thump_1726913669.jpg",
                Konum = "Alanya / Antalya",
                Aktif = true
            },
            new Proje
            {
                ProjeAdi = "Twin Towers 1",
                ProjeAdi_EN = "Twin Towers 1",
                ProjeAdi_RU = "Twin Towers 1",
                ProjeDurumu = "Tamamlandı",
                KisaAciklama = "KONAK CONSTRUCTION",
                KisaAciklama_EN = "KONAK CONSTRUCTION",
                KisaAciklama_RU = "KONAK CONSTRUCTION",
                AnaResimYolu = "/upload/proje/thump_1648175613.jpg",
                Konum = "Alanya / Antalya",
                Aktif = true
            },
            new Proje
            {
                ProjeAdi = "Blue By Residence",
                ProjeAdi_EN = "Blue By Residence",
                ProjeAdi_RU = "Blue By Residence",
                ProjeDurumu = "Tamamlandı",
                KisaAciklama = "KONAK CONSTRUCTION",
                KisaAciklama_EN = "KONAK CONSTRUCTION",
                KisaAciklama_RU = "KONAK CONSTRUCTION",
                AnaResimYolu = "/upload/proje/thump_1648178545.jpg",
                Konum = "Alanya / Antalya",
                Aktif = true
            },
            new Proje
            {
                ProjeAdi = "Uğurlu Apartments",
                ProjeAdi_EN = "Uğurlu Apartments",
                ProjeAdi_RU = "Uğurlu Apartments",
                ProjeDurumu = "Tamamlandı",
                KisaAciklama = "KONAK CONSTRUCTION",
                KisaAciklama_EN = "KONAK CONSTRUCTION",
                KisaAciklama_RU = "KONAK CONSTRUCTION",
                AnaResimYolu = "/upload/proje/thump_1648179482.jpg",
                Konum = "Alanya / Antalya",
                Aktif = true
            },
            new Proje
            {
                ProjeAdi = "Twin Tower 3",
                ProjeAdi_EN = "Twin Tower 3",
                ProjeAdi_RU = "Twin Tower 3",
                ProjeDurumu = "Devam Ediyor",
                KisaAciklama = "KONAK CONSTRUCTION",
                KisaAciklama_EN = "KONAK CONSTRUCTION",
                KisaAciklama_RU = "KONAK CONSTRUCTION",
                AnaResimYolu = "/upload/proje/thump_1652533338.jpg",
                Konum = "Alanya / Antalya",
                Aktif = true
            },
            new Proje
            {
                ProjeAdi = "Twin Tower 4",
                ProjeAdi_EN = "Twin Tower 4",
                ProjeAdi_RU = "Twin Tower 4",
                ProjeDurumu = "Yakında",
                KisaAciklama = "KONAK CONSTRUCTION",
                KisaAciklama_EN = "KONAK CONSTRUCTION",
                KisaAciklama_RU = "KONAK CONSTRUCTION",
                AnaResimYolu = "/upload/proje/thump_1649858670.jpg",
                Konum = "Alanya / Antalya",
                Aktif = true
            }
        };

        var addedProjectCount = 0;
        foreach (var defaultProject in defaultProjects)
        {
            if (!context.Projeler.Any(p => p.ProjeAdi == defaultProject.ProjeAdi))
            {
                defaultProject.OlusturmaTarihi = DateTime.Now;
                context.Projeler.Add(defaultProject);
                addedProjectCount++;
            }
        }

        if (addedProjectCount > 0)
        {
            context.SaveChanges();
            Console.WriteLine($"✅ {addedProjectCount} varsayılan proje kaydı eklendi.");
        }

        // Proje detay resimleri (varsa tekrar ekleme)
        var projectImageSeedData = new Dictionary<string, List<string>>
        {
            ["Twin Tower 2"] = new List<string>
            {
                "/upload/proje/thump_1648165636.jpg",
                "/upload/proje/thump_1648165643.jpg",
                "/upload/proje/thump_1648165652.jpg",
                "/upload/proje/thump_1648165658.jpg",
                "/upload/proje/thump_1648165683.jpg",
                "/upload/proje/thump_1648165691.jpg",
                "/upload/proje/thump_1648165702.jpg"
            },
            ["Twin Towers 1"] = new List<string>
            {
                "/upload/proje/thump_1648485100.jpg",
                "/upload/proje/thump_1648485118.jpg",
                "/upload/proje/thump_1648485124.jpg",
                "/upload/proje/thump_1648485140.jpg",
                "/upload/proje/thump_1648485146.jpg",
                "/upload/proje/thump_1648485155.jpg",
                "/upload/proje/thump_1648485163.jpg",
                "/upload/proje/thump_1648485169.jpg",
                "/upload/proje/thump_1648485197.jpg",
                "/upload/proje/thump_1648485206.jpg",
                "/upload/proje/thump_1648486101.jpg",
                "/upload/proje/thump_1648486108.jpg",
                "/upload/proje/thump_1648486115.jpg"
            },
            ["Blue By Residence"] = new List<string>
            {
                "/upload/proje/thump_1648481341.jpg",
                "/upload/proje/thump_1648481349.jpg",
                "/upload/proje/thump_1648481357.jpg",
                "/upload/proje/thump_1648481363.jpg",
                "/upload/proje/thump_1648481370.jpg",
                "/upload/proje/thump_1648481377.jpg",
                "/upload/proje/thump_1648481383.jpg",
                "/upload/proje/thump_1648481389.jpg",
                "/upload/proje/thump_1648481396.jpg",
                "/upload/proje/thump_1648481401.jpg"
            },
            ["Uğurlu Apartments"] = new List<string>
            {
                "/upload/proje/thump_1648179492.jpg",
                "/upload/proje/thump_1648179502.jpg",
                "/upload/proje/thump_1648179514.jpg"
            },
            ["Twin Tower 3"] = new List<string>
            {
                "/upload/proje/thump_1648087014.jpg",
                "/upload/proje/thump_1648087042.jpg",
                "/upload/proje/thump_1648087055.jpg",
                "/upload/proje/thump_1648177488.jpg",
                "/upload/proje/thump_1648177515.jpg",
                "/upload/proje/thump_1648177524.jpg",
                "/upload/proje/thump_1648177530.jpg",
                "/upload/proje/thump_1648177536.jpg",
                "/upload/proje/thump_1648177549.jpg",
                "/upload/proje/thump_1648177560.jpg",
                "/upload/proje/thump_1648177576.jpg",
                "/upload/proje/thump_1649032701.jpg",
                "/upload/proje/thump_1649032760.jpg",
                "/upload/proje/thump_1649032771.jpg",
                "/upload/proje/thump_1649032782.jpg",
                "/upload/proje/thump_1649387191.jpg",
                "/upload/proje/thump_1649387238.jpg",
                "/upload/proje/thump_1649387245.jpg",
                "/upload/proje/thump_1649387269.jpg",
                "/upload/proje/thump_1649387277.jpg",
                "/upload/proje/thump_1649387286.jpg",
                "/upload/proje/thump_1649387302.jpg",
                "/upload/proje/thump_1649387314.jpg",
                "/upload/proje/thump_1649387323.jpg",
                "/upload/proje/thump_1649387366.jpg",
                "/upload/proje/thump_1649387404.jpg",
                "/upload/proje/thump_1649387418.jpg"
            },
            ["Twin Tower 4"] = new List<string>
            {
                "/upload/proje/thump_1649858750.jpg",
                "/upload/proje/thump_1649858758.jpg",
                "/upload/proje/thump_1649858766.jpg",
                "/upload/proje/thump_1649858780.jpg",
                "/upload/proje/thump_1649858787.jpg",
                "/upload/proje/thump_1649858794.jpg",
                "/upload/proje/thump_1651217712.jpg",
                "/upload/proje/thump_1651217770.jpg",
                "/upload/proje/thump_1651218053.jpg"
            }
        };

        var addedProjectImageCount = 0;
        foreach (var kvp in projectImageSeedData)
        {
            var proje = context.Projeler.FirstOrDefault(p => p.ProjeAdi == kvp.Key);
            if (proje == null)
            {
                continue;
            }

            if (context.ProjeResimleri.Any(r => r.ProjeID == proje.ProjeID))
            {
                continue;
            }

            var yeniResimler = kvp.Value.Select((path, index) => new ProjeResim
            {
                ProjeID = proje.ProjeID,
                ResimYolu = path,
                Baslik = kvp.Key,
                Sira = index + 1
            }).ToList();

            if (yeniResimler.Count > 0)
            {
                context.ProjeResimleri.AddRange(yeniResimler);
                addedProjectImageCount += yeniResimler.Count;
            }
        }

        if (addedProjectImageCount > 0)
        {
            context.SaveChanges();
            Console.WriteLine($"✅ {addedProjectImageCount} adet proje detay görseli eklendi.");
        }

        // Kurumsal / Hakkımızda için başlangıç verisi
        if (!context.Kurumsal.Any())
        {
            var kurumsal = new Kurumsal
            {
                HakkimizdaBaslik = "Hakkımızda",
                HakkimizdaIcerik = @"<p>Konak İnşaat olarak konut projeleri, ticari merkezler vs. alanlarda inşaat sektöründe çalışmalar yapan bir inşaat firmasıdır.</p>
<p>Konak inşaat, 1997 yılından bu yana Alanya'da faaliyet gösteren bir inşaat firmasıdır. Konak inşaatin yöneticisi ve sahibi olan Kemal Hacıfazlıoğlu bir çok inşaat projesinin hayata geçirilmesinde ve tamamlanmasında önemli roller almıştır. Kendisi deniz kıyısı konut projelerinde ve ticari alanlar üretiminde yoğunlaşarak ekibiyle birlikte birçok prestijli işlere dâhil olmaktadır.</p>",
                HakkimizdaResim = "/upload/thump_1653662938.jpg",
                GuncellemeTarihi = DateTime.Now
            };

            context.Kurumsal.Add(kurumsal);
            context.SaveChanges();

            Console.WriteLine("✅ Varsayılan Kurumsal / Hakkımızda bilgisi eklendi.");
        }
        else
        {
            var mevcutKurumsal = context.Kurumsal.FirstOrDefault();
            if (mevcutKurumsal != null &&
                string.IsNullOrEmpty(mevcutKurumsal.HakkimizdaBaslik) &&
                string.IsNullOrEmpty(mevcutKurumsal.HakkimizdaIcerik))
            {
                mevcutKurumsal.HakkimizdaBaslik = "Hakkımızda";
                mevcutKurumsal.HakkimizdaIcerik = @"<p>Konak İnşaat olarak konut projeleri, ticari merkezler vs. alanlarda inşaat sektöründe çalışmalar yapan bir inşaat firmasıdır.</p>
<p>Konak inşaat, 1997 yılından bu yana Alanya'da faaliyet gösteren bir inşaat firmasıdır. Konak inşaatin yöneticisi ve sahibi olan Kemal Hacıfazlıoğlu bir çok inşaat projesinin hayata geçirilmesinde ve tamamlanmasında önemli roller almıştır. Kendisi deniz kıyısı konut projelerinde ve ticari alanlar üretiminde yoğunlaşarak ekibiyle birlikte birçok prestijli işlere dâhil olmaktadır.</p>";
                if (string.IsNullOrEmpty(mevcutKurumsal.HakkimizdaResim))
                {
                    mevcutKurumsal.HakkimizdaResim = "/upload/thump_1653662938.jpg";
                }
                mevcutKurumsal.GuncellemeTarihi = DateTime.Now;
                context.SaveChanges();
            }
        }

        // Ana Logo için başlangıç verisi
        var anaLogo = context.Markalar
            .FirstOrDefault(m => m.MarkaAdi != null && m.MarkaAdi.ToLower().Contains("ana logo"));
        
        if (anaLogo == null)
        {
            anaLogo = new Marka
            {
                MarkaAdi = "Ana Logo",
                ResimYolu = "/img/logo.png",
                Aktif = true,
                Sira = 0,
                OlusturmaTarihi = DateTime.Now
            };
            context.Markalar.Add(anaLogo);
            context.SaveChanges();
            Console.WriteLine("✅ Ana Logo kaydı eklendi.");
        }
        else
        {
            // Mevcut ana logo kaydının resim yolunu güncelle (eğer boşsa)
            if (string.IsNullOrEmpty(anaLogo.ResimYolu))
            {
                anaLogo.ResimYolu = "/img/logo.png";
                context.Markalar.Update(anaLogo);
                context.SaveChanges();
                Console.WriteLine("✅ Ana Logo kaydının resim yolu güncellendi.");
            }
        }

        // Sosyal Medya için başlangıç verisi
        // Instagram
        var instagramHesabi = context.SosyalMedya
            .FirstOrDefault(s => s.Link != null && s.Link.ToLower().Contains("instagram.com"));
        
        if (instagramHesabi == null)
        {
            instagramHesabi = new Sosyal
            {
                PlatformAdi = "Instagram",
                IkonSinifi = "fa-brands fa-instagram",
                Link = "https://www.instagram.com/konakconstructiongroup",
                Aktif = true,
                Sira = 1
            };
            context.SosyalMedya.Add(instagramHesabi);
            context.SaveChanges();
            Console.WriteLine("✅ Instagram sosyal medya hesabı eklendi.");
        }
        else
        {
            // Mevcut Instagram hesabının linkini güncelle
            if (instagramHesabi.Link != "https://www.instagram.com/konakconstructiongroup")
            {
                instagramHesabi.Link = "https://www.instagram.com/konakconstructiongroup";
                if (string.IsNullOrEmpty(instagramHesabi.PlatformAdi))
                    instagramHesabi.PlatformAdi = "Instagram";
                if (string.IsNullOrEmpty(instagramHesabi.IkonSinifi))
                    instagramHesabi.IkonSinifi = "fa-brands fa-instagram";
                context.SosyalMedya.Update(instagramHesabi);
                context.SaveChanges();
                Console.WriteLine("✅ Instagram sosyal medya hesabı güncellendi.");
            }
        }

        // Facebook
        var facebookHesabi = context.SosyalMedya
            .FirstOrDefault(s => s.Link != null && s.Link.ToLower().Contains("facebook.com"));
        
        if (facebookHesabi == null)
        {
            facebookHesabi = new Sosyal
            {
                PlatformAdi = "Facebook",
                IkonSinifi = "fa-brands fa-facebook",
                Link = "https://www.facebook.com/konakconstructiongroup/",
                Aktif = true,
                Sira = 2
            };
            context.SosyalMedya.Add(facebookHesabi);
            context.SaveChanges();
            Console.WriteLine("✅ Facebook sosyal medya hesabı eklendi.");
        }
        else
        {
            // Mevcut Facebook hesabının linkini güncelle
            if (facebookHesabi.Link != "https://www.facebook.com/konakconstructiongroup/")
            {
                facebookHesabi.Link = "https://www.facebook.com/konakconstructiongroup/";
                if (string.IsNullOrEmpty(facebookHesabi.PlatformAdi))
                    facebookHesabi.PlatformAdi = "Facebook";
                if (string.IsNullOrEmpty(facebookHesabi.IkonSinifi))
                    facebookHesabi.IkonSinifi = "fa-brands fa-facebook";
                context.SosyalMedya.Update(facebookHesabi);
                context.SaveChanges();
                Console.WriteLine("✅ Facebook sosyal medya hesabı güncellendi.");
            }
        }

        // YouTube
        var youtubeHesabi = context.SosyalMedya
            .FirstOrDefault(s => s.Link != null && s.Link.ToLower().Contains("youtube.com"));
        
        if (youtubeHesabi == null)
        {
            youtubeHesabi = new Sosyal
            {
                PlatformAdi = "YouTube",
                IkonSinifi = "fa-brands fa-youtube",
                Link = "https://www.youtube.com/channel/UCQxyH5knk3JmTfyZSpS1QiA",
                Aktif = true,
                Sira = 3
            };
            context.SosyalMedya.Add(youtubeHesabi);
            context.SaveChanges();
            Console.WriteLine("✅ YouTube sosyal medya hesabı eklendi.");
        }
        else
        {
            // Mevcut YouTube hesabının linkini güncelle
            if (youtubeHesabi.Link != "https://www.youtube.com/channel/UCQxyH5knk3JmTfyZSpS1QiA")
            {
                youtubeHesabi.Link = "https://www.youtube.com/channel/UCQxyH5knk3JmTfyZSpS1QiA";
                if (string.IsNullOrEmpty(youtubeHesabi.PlatformAdi))
                    youtubeHesabi.PlatformAdi = "YouTube";
                if (string.IsNullOrEmpty(youtubeHesabi.IkonSinifi))
                    youtubeHesabi.IkonSinifi = "fa-brands fa-youtube";
                context.SosyalMedya.Update(youtubeHesabi);
                context.SaveChanges();
                Console.WriteLine("✅ YouTube sosyal medya hesabı güncellendi.");
            }
        }

        // Hizmetler için başlangıç verisi
        var kentselDonusumHizmeti = context.Hizmetler
            .FirstOrDefault(h => h.Baslik != null && h.Baslik.ToLower().Contains("kentsel"));
        
        if (kentselDonusumHizmeti == null)
        {
            // Önce ID=3 olan bir kayıt var mı kontrol et
            var mevcutId3 = context.Hizmetler.FirstOrDefault(h => h.Id == 3);
            if (mevcutId3 != null)
            {
                // ID=3 zaten başka bir hizmet tarafından kullanılıyor, onu güncelle
                mevcutId3.Baslik = "Kentsel Dönüşüm";
                mevcutId3.Baslik_EN = "Urban Transformation";
                mevcutId3.Baslik_RU = "Городская трансформация";
                mevcutId3.KisaAciklama = "Kentsel dönüşüm projelerimiz ile şehirlerimizi modernleştiriyoruz.";
                mevcutId3.KisaAciklama_EN = "We modernize our cities with our urban transformation projects.";
                mevcutId3.KisaAciklama_RU = "Мы модернизируем наши города с помощью наших проектов городской трансформации.";
                mevcutId3.Aciklama = "Kentsel dönüşüm hizmetlerimiz hakkında detaylı bilgi için lütfen bizimle iletişime geçin.";
                mevcutId3.Aciklama_EN = "For detailed information about our urban transformation services, please contact us.";
                mevcutId3.Aciklama_RU = "Для получения подробной информации о наших услугах по городской трансформации, пожалуйста, свяжитесь с нами.";
                if (string.IsNullOrEmpty(mevcutId3.ResimYolu))
                {
                    mevcutId3.ResimYolu = "/upload/background.jpg";
                }
                context.Hizmetler.Update(mevcutId3);
                context.SaveChanges();
                Console.WriteLine("✅ Kentsel Dönüşüm hizmeti (ID=3) güncellendi.");
            }
            else
            {
                // ID=3 boş, yeni hizmet ekle
                kentselDonusumHizmeti = new Hizmet
                {
                    Baslik = "Kentsel Dönüşüm",
                    Baslik_EN = "Urban Transformation",
                    Baslik_RU = "Городская трансформация",
                    KisaAciklama = "Kentsel dönüşüm projelerimiz ile şehirlerimizi modernleştiriyoruz.",
                    KisaAciklama_EN = "We modernize our cities with our urban transformation projects.",
                    KisaAciklama_RU = "Мы модернизируем наши города с помощью наших проектов городской трансформации.",
                    Aciklama = "Kentsel dönüşüm hizmetlerimiz hakkında detaylı bilgi için lütfen bizimle iletişime geçin.",
                    Aciklama_EN = "For detailed information about our urban transformation services, please contact us.",
                    Aciklama_RU = "Для получения подробной информации о наших услугах по городской трансформации, пожалуйста, свяжитесь с нами.",
                    ResimYolu = "/upload/background.jpg",
                    OlusturmaTarihi = DateTime.Now
                };
                context.Hizmetler.Add(kentselDonusumHizmeti);
                context.SaveChanges();
                
                // Eğer eklenen hizmetin ID'si 3 değilse, ID'yi 3 yapmak için güncelle
                if (kentselDonusumHizmeti.Id != 3)
                {
                    // SQL ile ID'yi güncelle (Entity Framework'te ID'yi direkt değiştiremeyiz)
                    // Bu durumda navigation menüsünü dinamik ID'ye göre güncellemek daha iyi
                    Console.WriteLine($"✅ Kentsel Dönüşüm hizmeti eklendi (ID: {kentselDonusumHizmeti.Id}).");
                }
                else
                {
                    Console.WriteLine("✅ Kentsel Dönüşüm hizmeti eklendi (ID: 3).");
                }
            }
        }
        else
        {
            // Mevcut hizmetin bilgilerini güncelle
            if (string.IsNullOrEmpty(kentselDonusumHizmeti.Baslik))
            {
                kentselDonusumHizmeti.Baslik = "Kentsel Dönüşüm";
                kentselDonusumHizmeti.Baslik_EN = "Urban Transformation";
                kentselDonusumHizmeti.Baslik_RU = "Городская трансформация";
            }
            if (string.IsNullOrEmpty(kentselDonusumHizmeti.KisaAciklama))
            {
                kentselDonusumHizmeti.KisaAciklama = "Kentsel dönüşüm projelerimiz ile şehirlerimizi modernleştiriyoruz.";
            }
            if (string.IsNullOrEmpty(kentselDonusumHizmeti.Aciklama))
            {
                kentselDonusumHizmeti.Aciklama = "Kentsel dönüşüm hizmetlerimiz hakkında detaylı bilgi için lütfen bizimle iletişime geçin.";
            }
            if (string.IsNullOrEmpty(kentselDonusumHizmeti.ResimYolu))
            {
                kentselDonusumHizmeti.ResimYolu = "/upload/background.jpg";
            }
            context.Hizmetler.Update(kentselDonusumHizmeti);
            context.SaveChanges();
            Console.WriteLine($"✅ Kentsel Dönüşüm hizmeti güncellendi (ID: {kentselDonusumHizmeti.Id}).");
        }

        // Ekip üyeleri için başlangıç verisi
        if (!context.TeamMembers.Any())
        {
            var team = new List<TeamMember>
            {
                new TeamMember
                {
                    AdSoyad = "Kemal Hacıfazlıoğlu",
                    Pozisyon = "Kurucu",
                    FotoYolu = "/upload/ekip/thump_1648177909.jpg",
                    Sira = 1,
                    Aktif = true
                },
                new TeamMember
                {
                    AdSoyad = "UMIT RAD",
                    Pozisyon = "Satış Temsilcisi",
                    FotoYolu = "/upload/ekip/thump_1652451521.jpg",
                    Sira = 2,
                    Aktif = true
                }
            };

            context.TeamMembers.AddRange(team);
            context.SaveChanges();

            Console.WriteLine("✅ Varsayılan ekip üyeleri eklendi.");
        }

        // Ofisler için başlangıç verisi
        if (!context.OfficeLocations.Any())
        {
            var ofisler = new List<OfficeLocation>
            {
                new OfficeLocation
                {
                    Ad = "Merkez Ofis",
                    Tip = "Merkez",
                    Adres = "Alanya, Türkiye",
                    Telefon = "+90 (000) 000 00 00",
                    Email = "info@konakconstruction.net",
                    Sira = 1,
                    Aktif = true
                },
                new OfficeLocation
                {
                    Ad = "Satış Ofisi",
                    Tip = "Satış",
                    Adres = "Alanya, Türkiye",
                    Telefon = "+90 (000) 000 00 01",
                    Email = "sales@konakconstruction.net",
                    Sira = 2,
                    Aktif = true
                }
            };

            context.OfficeLocations.AddRange(ofisler);
            context.SaveChanges();

            Console.WriteLine("✅ Varsayılan ofis kayıtları eklendi.");
        }

        // İletişim bilgileri için başlangıç verisi (Merkez Ofis ve Satış Ofisi)
        // Mevcut kayıtları kontrol et ve güncelle/ekle
        var tumKayitlar = context.Iletisim.ToList();
        
        // Merkez Ofis kayıtlarını bul (Baslik "Merkez" içeriyorsa)
        var merkezKayitlari = tumKayitlar.Where(i => 
            !string.IsNullOrEmpty(i.Baslik) && 
            i.Baslik.Contains("Merkez", StringComparison.OrdinalIgnoreCase)).ToList();
        
        // Satış Ofisi kayıtlarını bul (Baslik "Satış" içeriyorsa)
        var satisKayitlari = tumKayitlar.Where(i => 
            !string.IsNullOrEmpty(i.Baslik) && 
            i.Baslik.Contains("Satış", StringComparison.OrdinalIgnoreCase)).ToList();
        
        // Eğer birden fazla Merkez kaydı varsa, ilkini tut, diğerlerini sil
        var mevcutMerkez = merkezKayitlari.OrderBy(i => i.IletisimID).FirstOrDefault();
        if (merkezKayitlari.Count > 1)
        {
            var silinecekMerkezler = merkezKayitlari.Skip(1).ToList();
            context.Iletisim.RemoveRange(silinecekMerkezler);
            context.SaveChanges();
            Console.WriteLine($"⚠️ {silinecekMerkezler.Count} adet yinelenen Merkez Ofis kaydı silindi.");
        }
        
        // Eğer birden fazla Satış kaydı varsa, ilkini tut, diğerlerini sil
        var mevcutSatis = satisKayitlari.OrderBy(i => i.IletisimID).FirstOrDefault();
        if (satisKayitlari.Count > 1)
        {
            var silinecekSatislar = satisKayitlari.Skip(1).ToList();
            context.Iletisim.RemoveRange(silinecekSatislar);
            context.SaveChanges();
            Console.WriteLine($"⚠️ {silinecekSatislar.Count} adet yinelenen Satış Ofisi kaydı silindi.");
        }
        
        // Eğer Merkez kaydı yoksa, ilk kaydı kullan
        if (mevcutMerkez == null)
        {
            mevcutMerkez = tumKayitlar.OrderBy(i => i.IletisimID).FirstOrDefault();
        }
        
        // Eğer Satış kaydı yoksa, ikinci kaydı kullan (Merkez'den farklı olmalı)
        if (mevcutSatis == null)
        {
            mevcutSatis = tumKayitlar.Where(i => i.IletisimID != mevcutMerkez?.IletisimID)
                .OrderBy(i => i.IletisimID)
                .FirstOrDefault();
        }
        
        // Eğer Baslik null olan kayıtlar varsa, onları da kontrol et
        var basliksizKayitlar = tumKayitlar.Where(i => string.IsNullOrEmpty(i.Baslik)).ToList();

        if (mevcutMerkez == null)
        {
            // Merkez Ofis yoksa, basliksiz bir kayıt varsa onu kullan, yoksa yeni ekle
            var kullanilacakMerkez = basliksizKayitlar.FirstOrDefault();
            if (kullanilacakMerkez != null)
            {
                if (string.IsNullOrEmpty(kullanilacakMerkez.Baslik))
                    kullanilacakMerkez.Baslik = "Merkez Ofis";
                if (string.IsNullOrEmpty(kullanilacakMerkez.Adres_1))
                    kullanilacakMerkez.Adres_1 = "Kızlarpınarı Mah. Uğurlular Sk. Uğurlular Sitesi B Blok NO:2 Alanya/Antalya";
                if (string.IsNullOrEmpty(kullanilacakMerkez.Telefon))
                    kullanilacakMerkez.Telefon = "+90 242 519 0934";
                if (string.IsNullOrEmpty(kullanilacakMerkez.Mail))
                    kullanilacakMerkez.Mail = "konakconstruction07@gmail.com";
                if (string.IsNullOrEmpty(kullanilacakMerkez.Gsm))
                    kullanilacakMerkez.Gsm = "+90 552 601 16 64";
                if (string.IsNullOrEmpty(kullanilacakMerkez.Whatsapp))
                    kullanilacakMerkez.Whatsapp = "+90 552 601 16 64";
                context.Update(kullanilacakMerkez);
                basliksizKayitlar.Remove(kullanilacakMerkez);
                Console.WriteLine("✅ Mevcut kayıt Merkez Ofis olarak güncellendi.");
            }
            else
            {
                var merkezOfis = new Iletisim
                {
                    Baslik = "Merkez Ofis",
                    Adres_1 = "Kızlarpınarı Mah. Uğurlular Sk. Uğurlular Sitesi B Blok NO:2 Alanya/Antalya",
                    Telefon = "+90 242 519 0934",
                    Mail = "konakconstruction07@gmail.com",
                    Gsm = "+90 552 601 16 64",
                    Whatsapp = "+90 552 601 16 64"
                };
                context.Iletisim.Add(merkezOfis);
                Console.WriteLine("✅ Merkez Ofis iletişim bilgisi eklendi.");
            }
        }
        else
        {
            // Mevcut Merkez Ofis kaydını güncelle (Baslik'i değiştirme, sadece diğer alanları varsayılan değerlerle doldur)
            // Baslik kullanıcı tarafından değiştirilmiş olabilir, bu yüzden koruyoruz
            if (string.IsNullOrEmpty(mevcutMerkez.Adres_1))
                mevcutMerkez.Adres_1 = "Kızlarpınarı Mah. Uğurlular Sk. Uğurlular Sitesi B Blok NO:2 Alanya/Antalya";
            if (string.IsNullOrEmpty(mevcutMerkez.Telefon))
                mevcutMerkez.Telefon = "+90 242 519 0934";
            if (string.IsNullOrEmpty(mevcutMerkez.Mail))
                mevcutMerkez.Mail = "konakconstruction07@gmail.com";
            if (string.IsNullOrEmpty(mevcutMerkez.Gsm))
                mevcutMerkez.Gsm = "+90 552 601 16 64";
            if (string.IsNullOrEmpty(mevcutMerkez.Whatsapp))
                mevcutMerkez.Whatsapp = "+90 552 601 16 64";
            // Baslik'i değiştirmiyoruz - kullanıcı tarafından özelleştirilmiş olabilir
            context.Update(mevcutMerkez);
            Console.WriteLine("✅ Merkez Ofis iletişim bilgisi kontrol edildi (Baslik korundu).");
        }

        if (mevcutSatis == null)
        {
            // Satış Ofisi yoksa, basliksiz bir kayıt varsa onu kullan, yoksa yeni ekle
            var kullanilacakSatis = basliksizKayitlar.FirstOrDefault();
            if (kullanilacakSatis != null)
            {
                if (string.IsNullOrEmpty(kullanilacakSatis.Baslik))
                    kullanilacakSatis.Baslik = "Satış Ofisi";
                if (string.IsNullOrEmpty(kullanilacakSatis.Adres_1))
                    kullanilacakSatis.Adres_1 = "Mahmutlar Mah. D-400 Karayolu Üstü Bulv. NO: 23 A-B Alanya/Antalya";
                if (string.IsNullOrEmpty(kullanilacakSatis.Telefon))
                    kullanilacakSatis.Telefon = "+90 552 611 1662 EN,TR,FARSI";
                if (string.IsNullOrEmpty(kullanilacakSatis.Mail))
                    kullanilacakSatis.Mail = "info@konakconstruction.com";
                if (string.IsNullOrEmpty(kullanilacakSatis.Gsm))
                    kullanilacakSatis.Gsm = "+90 552 611 1663 RU,UA,EN,TR";
                if (string.IsNullOrEmpty(kullanilacakSatis.Whatsapp))
                    kullanilacakSatis.Whatsapp = "+90 552 611 1663 RU,UA,EN,TR";
                context.Update(kullanilacakSatis);
                Console.WriteLine("✅ Mevcut kayıt Satış Ofisi olarak güncellendi.");
            }
            else
            {
                var satisOfisi = new Iletisim
                {
                    Baslik = "Satış Ofisi",
                    Adres_1 = "Mahmutlar Mah. D-400 Karayolu Üstü Bulv. NO: 23 A-B Alanya/Antalya",
                    Telefon = "+90 552 611 1662 EN,TR,FARSI",
                    Mail = "info@konakconstruction.com",
                    Gsm = "+90 552 611 1663 RU,UA,EN,TR",
                    Whatsapp = "+90 552 611 1663 RU,UA,EN,TR"
                };
                context.Iletisim.Add(satisOfisi);
                Console.WriteLine("✅ Satış Ofisi iletişim bilgisi eklendi.");
            }
        }
        else
        {
            // Mevcut Satış Ofisi kaydını güncelle (Baslik'i değiştirme, sadece diğer alanları varsayılan değerlerle doldur)
            // Baslik kullanıcı tarafından değiştirilmiş olabilir, bu yüzden koruyoruz
            if (string.IsNullOrEmpty(mevcutSatis.Adres_1))
                mevcutSatis.Adres_1 = "Mahmutlar Mah. D-400 Karayolu Üstü Bulv. NO: 23 A-B Alanya/Antalya";
            if (string.IsNullOrEmpty(mevcutSatis.Telefon))
                mevcutSatis.Telefon = "+90 552 611 1662 EN,TR,FARSI";
            if (string.IsNullOrEmpty(mevcutSatis.Mail))
                mevcutSatis.Mail = "info@konakconstruction.com";
            if (string.IsNullOrEmpty(mevcutSatis.Gsm))
                mevcutSatis.Gsm = "+90 552 611 1663 RU,UA,EN,TR";
            if (string.IsNullOrEmpty(mevcutSatis.Whatsapp))
                mevcutSatis.Whatsapp = "+90 552 611 1663 RU,UA,EN,TR";
            // Baslik'i değiştirmiyoruz - kullanıcı tarafından özelleştirilmiş olabilir
            context.Update(mevcutSatis);
            Console.WriteLine("✅ Satış Ofisi iletişim bilgisi kontrol edildi (Baslik korundu).");
        }

        context.SaveChanges();

        // Anahtar Kelimeler (Keywords) için başlangıç verisi
        if (!context.Keywords.Any())
        {
            var keyword = new Keyword
            {
                SayfaAdi = "Anasayfa",
                Title = "Konak Construction - Hayallerinizin Ötesinde...",
                Title_EN = "Konak Construction - Beyond Your Dreams...",
                Title_RU = "Konak Construction - За пределами ваших мечтаний...",
                Description = "Alanya ve Antalya'da deniz kenarında lüks daireler inşa eden Konak İnşaat'ın en yeni konut projeleri hakkında bilgi alın.",
                Description_EN = "Learn about the newest residential projects of Konak Construction, building luxury apartments by the sea in Alanya and Antalya.",
                Description_RU = "Узнайте о новейших жилых проектах Konak Construction, строящих роскошные апартаменты у моря в Аланье и Анталье.",
                Keywords = "Alanya, Emlak, Yatırım, Lüks Daireler, konak inşaat",
                Keywords_EN = "Alanya, Real Estate, Investment, Luxury Apartments, konak construction",
                Keywords_RU = "Аланья, Недвижимость, Инвестиции, Роскошные апартаменты, konak construction"
            };

            context.Keywords.Add(keyword);
            context.SaveChanges();

            Console.WriteLine("✅ Varsayılan anahtar kelime kaydı eklendi.");
        }

        // Mail Ayarları için başlangıç verisi
        if (!context.MailAyarlari.Any())
        {
            var mailAyar = new Mail
            {
                SmtpServer = "mail.konakconstruction.net",
                SmtpPort = 587,
                Email = "info@konakconstruction.net",
                Password = "Konak*07*",
                GonderenAdi = "Konak Construction",
                SslKullan = true,
                Aktif = true
            };

            context.MailAyarlari.Add(mailAyar);
            context.SaveChanges();

            Console.WriteLine("✅ Varsayılan mail ayarları kaydı eklendi.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Admin kullanıcısı oluşturulurken hata oluştu.");
    }
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Localization - UseRouting'den SONRA, UseEndpoints'den ÖNCE
var localizationOptions = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

app.UseSession(); // Session ekledik

app.UseAuthentication();
app.UseAuthorization();

// Routes - SIRALAMA ÖNEMLİ!
// Admin/AdminHome için özel route
app.MapControllerRoute(
    name: "adminHome",
    pattern: "Admin/AdminHome",
    defaults: new { area = "Admin", controller = "Home", action = "AdminHome" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=AdminHome}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();