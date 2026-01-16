using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonakInsaat.Migrations
{
    /// <inheritdoc />
    public partial class AddAllAdminModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.CreateTable(
                name: "Bloglar",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Baslik_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icerik_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icerik_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KisaAciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KisaAciklama_EN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KisaAciklama_RU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YayinTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloglar", x => x.BlogID);
                });

            migrationBuilder.CreateTable(
                name: "Galeriler",
                columns: table => new
                {
                    GaleriID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kategori = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galeriler", x => x.GaleriID);
                });

            migrationBuilder.CreateTable(
                name: "Hizmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Baslik_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aciklama_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KisaAciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KisaAciklama_EN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KisaAciklama_RU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hizmetler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Iletisim",
                columns: table => new
                {
                    IletisimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adres_1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gsm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Whatsapp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harita = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Resim = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iletisim", x => x.IletisimID);
                });

            migrationBuilder.CreateTable(
                name: "Kataloglar",
                columns: table => new
                {
                    KatalogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kataloglar", x => x.KatalogID);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    KeywordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SayfaAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Title_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Title_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description_EN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description_RU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Keywords_EN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Keywords_RU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.KeywordID);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SonGirisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciID);
                });

            migrationBuilder.CreateTable(
                name: "Kurumsal",
                columns: table => new
                {
                    KurumsalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HakkimizdaBaslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HakkimizdaBaslik_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HakkimizdaBaslik_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HakkimizdaIcerik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HakkimizdaIcerik_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HakkimizdaIcerik_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Misyon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Misyon_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Misyon_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vizyon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vizyon_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vizyon_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HakkimizdaResim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kurumsal", x => x.KurumsalID);
                });

            migrationBuilder.CreateTable(
                name: "MailAyarlari",
                columns: table => new
                {
                    MailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmtpServer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SmtpPort = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GonderenAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SslKullan = table.Column<bool>(type: "bit", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailAyarlari", x => x.MailID);
                });

            migrationBuilder.CreateTable(
                name: "Markalar",
                columns: table => new
                {
                    MarkaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkaAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markalar", x => x.MarkaID);
                });

            migrationBuilder.CreateTable(
                name: "Mesajlar",
                columns: table => new
                {
                    MesajID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Konu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MesajIcerigi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GonderimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Okundu = table.Column<bool>(type: "bit", nullable: false),
                    Yanitlandi = table.Column<bool>(type: "bit", nullable: false),
                    IpAdresi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesajlar", x => x.MesajID);
                });

            migrationBuilder.CreateTable(
                name: "Projeler",
                columns: table => new
                {
                    ProjeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjeAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProjeAdi_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProjeAdi_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aciklama_EN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aciklama_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KisaAciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KisaAciklama_EN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    KisaAciklama_RU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProjeDurumu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AnaResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Konum = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Metrekare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OdaSayisi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeslimTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeler", x => x.ProjeID);
                });

            migrationBuilder.CreateTable(
                name: "Sayilar",
                columns: table => new
                {
                    SayiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Baslik_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SayiDegeri = table.Column<int>(type: "int", nullable: false),
                    Ikon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sayilar", x => x.SayiID);
                });

            migrationBuilder.CreateTable(
                name: "Sliderlar",
                columns: table => new
                {
                    SliderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_EN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Baslik_RU = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AltBaslik = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AltBaslik_EN = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AltBaslik_RU = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliderlar", x => x.SliderID);
                });

            migrationBuilder.CreateTable(
                name: "SosyalMedya",
                columns: table => new
                {
                    SosyalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IkonSinifi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Aktif = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SosyalMedya", x => x.SosyalID);
                });

            migrationBuilder.CreateTable(
                name: "ProjeResimleri",
                columns: table => new
                {
                    ResimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjeID = table.Column<int>(type: "int", nullable: false),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    EklenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjeResimleri", x => x.ResimID);
                    table.ForeignKey(
                        name: "FK_ProjeResimleri_Projeler_ProjeID",
                        column: x => x.ProjeID,
                        principalTable: "Projeler",
                        principalColumn: "ProjeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjeResimleri_ProjeID",
                table: "ProjeResimleri",
                column: "ProjeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bloglar");

            migrationBuilder.DropTable(
                name: "Galeriler");

            migrationBuilder.DropTable(
                name: "Hizmetler");

            migrationBuilder.DropTable(
                name: "Iletisim");

            migrationBuilder.DropTable(
                name: "Kataloglar");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Kurumsal");

            migrationBuilder.DropTable(
                name: "MailAyarlari");

            migrationBuilder.DropTable(
                name: "Markalar");

            migrationBuilder.DropTable(
                name: "Mesajlar");

            migrationBuilder.DropTable(
                name: "ProjeResimleri");

            migrationBuilder.DropTable(
                name: "Sayilar");

            migrationBuilder.DropTable(
                name: "Sliderlar");

            migrationBuilder.DropTable(
                name: "SosyalMedya");

            migrationBuilder.DropTable(
                name: "Projeler");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    About = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });
        }
    }
}
