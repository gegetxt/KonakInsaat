using KonakInsaat.Areas.Admin.Models;
using KonakInsaat.Models;
using Microsoft.EntityFrameworkCore;

namespace KonakInsaat.Data
{
    public class KonakInsaatContext : DbContext
    {
        public KonakInsaatContext(DbContextOptions<KonakInsaatContext> options) : base(options) { }

        // Ana Modeller
        public DbSet<Login> Logins { get; set; } = null!;
        public DbSet<ContactFormModel> Contacts { get; set; } = null!;
        public DbSet<PageContent> PageContents { get; set; } = null!;

        // Admin Modelleri
        public DbSet<Hizmet> Hizmetler { get; set; } = null!;
        public DbSet<Iletisim> Iletisim { get; set; } = null!;
        public DbSet<Katalog> Kataloglar { get; set; } = null!;
        public DbSet<Blog> Bloglar { get; set; } = null!;
        public DbSet<Slider> Sliderlar { get; set; } = null!;
        public DbSet<Proje> Projeler { get; set; } = null!;
        public DbSet<ProjeResim> ProjeResimleri { get; set; } = null!;
        public DbSet<Marka> Markalar { get; set; } = null!;
        public DbSet<Sayi> Sayilar { get; set; } = null!;
        public DbSet<Sosyal> SosyalMedya { get; set; } = null!;
        public DbSet<Keyword> Keywords { get; set; } = null!;
        public DbSet<Mail> MailAyarlari { get; set; } = null!;
        public DbSet<Kullanici> Kullanicilar { get; set; } = null!;
        public DbSet<Galeri> Galeriler { get; set; } = null!;
        public DbSet<Mesaj> Mesajlar { get; set; } = null!;
        public DbSet<Kurumsal> Kurumsal { get; set; } = null!;
        public DbSet<TeamMember> TeamMembers { get; set; } = null!;
        public DbSet<OfficeLocation> OfficeLocations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Hizmet Configuration
            modelBuilder.Entity<Hizmet>(entity =>
            {
                entity.ToTable("Hizmetler");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Baslik).IsRequired().HasMaxLength(200);
                entity.Property(e => e.KisaAciklama).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Aciklama).IsRequired();
            });

            // Proje Configuration
            modelBuilder.Entity<Proje>(entity =>
            {
                entity.ToTable("Projeler");
                entity.HasKey(e => e.ProjeID);
                entity.Property(e => e.ProjeAdi).IsRequired().HasMaxLength(200);
                entity.HasMany(e => e.ProjeResimleri)
                      .WithOne(e => e.Proje)
                      .HasForeignKey(e => e.ProjeID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ProjeResim Configuration
            modelBuilder.Entity<ProjeResim>(entity =>
            {
                entity.ToTable("ProjeResimleri");
                entity.HasKey(e => e.ResimID);
            });

            // Blog Configuration
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Bloglar");
                entity.HasKey(e => e.BlogID);
            });

            // Slider Configuration
            modelBuilder.Entity<Slider>(entity =>
            {
                entity.ToTable("Sliderlar");
                entity.HasKey(e => e.SliderID);
            });

            // Marka Configuration
            modelBuilder.Entity<Marka>(entity =>
            {
                entity.ToTable("Markalar");
                entity.HasKey(e => e.MarkaID);
            });

            // Sayi Configuration
            modelBuilder.Entity<Sayi>(entity =>
            {
                entity.ToTable("Sayilar");
                entity.HasKey(e => e.SayiID);
            });

            // Sosyal Configuration
            modelBuilder.Entity<Sosyal>(entity =>
            {
                entity.ToTable("SosyalMedya");
                entity.HasKey(e => e.SosyalID);
            });

            // Keyword Configuration
            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("Keywords");
                entity.HasKey(e => e.KeywordID);
            });

            // Mail Configuration
            modelBuilder.Entity<Mail>(entity =>
            {
                entity.ToTable("MailAyarlari");
                entity.HasKey(e => e.MailID);
            });

            // Kullanici Configuration
            modelBuilder.Entity<Kullanici>(entity =>
            {
                entity.ToTable("Kullanicilar");
                entity.HasKey(e => e.KullaniciID);
            });

            // Galeri Configuration
            modelBuilder.Entity<Galeri>(entity =>
            {
                entity.ToTable("Galeriler");
                entity.HasKey(e => e.GaleriID);
            });

            // Mesaj Configuration
            modelBuilder.Entity<Mesaj>(entity =>
            {
                entity.ToTable("Mesajlar");
                entity.HasKey(e => e.MesajID);
            });

            // Kurumsal Configuration
            modelBuilder.Entity<Kurumsal>(entity =>
            {
                entity.ToTable("Kurumsal");
                entity.HasKey(e => e.KurumsalID);
            });

            // TeamMember Configuration
            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.ToTable("TeamMembers");
                entity.HasKey(e => e.TeamMemberID);
            });

            // OfficeLocation Configuration
            modelBuilder.Entity<OfficeLocation>(entity =>
            {
                entity.ToTable("OfficeLocations");
                entity.HasKey(e => e.OfficeLocationID);
            });

            // Katalog Configuration
            modelBuilder.Entity<Katalog>(entity =>
            {
                entity.ToTable("Kataloglar");
                entity.HasKey(e => e.KatalogID);
            });

            // Iletisim Configuration
            modelBuilder.Entity<Iletisim>(entity =>
            {
                entity.ToTable("Iletisim");
                entity.HasKey(e => e.IletisimID);
            });
        }
    }
}