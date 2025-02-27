using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;

namespace CalenderDatabase
{
    public partial class CalenderContext : DbContext
    {
        const string DefaultConnectStr = "Host=localhost;Port=5433;Database=Calender;Username=postgres;Password=user";
        const string ConnectFile = "CalenderConnectionString.cfg";

        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<FilmProfileCluster> FilmProfileClusters { get; set; }
        public virtual DbSet<FilmProfile> FilmProfiles { get; set; }
        public virtual DbSet<RollSetting> RollSettings { get; set; }
        public virtual DbSet<Scenario> Scenarios { get; set; }

        public CalenderContext()
        {
            Database.EnsureCreated();
        }

        public CalenderContext(DbContextOptions<CalenderContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                System.IO.FileInfo fileInfo = new FileInfo(ConnectFile);
                if (!fileInfo.Exists)
                {
                    File.WriteAllText(ConnectFile, DefaultConnectStr, Encoding.UTF8);
                }

                string connectStr = File.ReadAllText(ConnectFile, Encoding.UTF8);
                optionsBuilder.UseNpgsql(connectStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("Film");
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<FilmProfileCluster>(entity =>
            {
                entity.ToTable("FilmProfileCluster");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Width)
                    .IsRequired()
                    .HasColumnName("width");

                entity.Property(e => e.CrossStart)
                    .IsRequired()
                    .HasColumnName("cross_start");
                entity.Property(e => e.CurveStart)
                    .IsRequired()
                    .HasColumnName("curve_start");

                entity.HasIndex(e => e.FilmId, "IX_Relationship1");
                entity.Property(e => e.FilmId).HasColumnName("film_id");
                entity.HasOne(d => d.Film)
                    .WithMany(p => p.FilmProfileClusters)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FilmCluster");
            });

            modelBuilder.Entity<FilmProfile>(entity =>
            {
                entity.ToTable("FilmProfile");

                entity.HasKey(x => new { x.Id, x.FilmProfileClusterId });
                entity.HasIndex(e => e.FilmProfileClusterId, "IX_Relationship5");

                entity.Property(e => e.Profile)
                    .HasColumnType("jsonb[]")
                    .IsRequired()
                    .HasColumnName("profile_array");
            });

            modelBuilder.Entity<RollSetting>(entity =>
            {
                entity.ToTable("RollSetting");
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Elasticity)
                    .IsRequired()
                    .HasColumnName("elasticity");
                entity.Property(e => e.FirstDistance)
                    .IsRequired()
                    .HasColumnName("first_distance");
                entity.Property(e => e.SecondDistance)
                    .IsRequired()
                    .HasColumnName("second_distance");
                entity.Property(e => e.BarrelDiameter)
                    .IsRequired()
                    .HasColumnName("barrel_diameter");
                entity.Property(e => e.NeckDiameter)
                    .IsRequired()
                    .HasColumnName("neck_diameter");
                entity.Property(e => e.HoleDiameter)
                    .IsRequired()
                    .HasColumnName("hole_diameter");
                entity.Property(e => e.Width)
                    .IsRequired()
                    .HasColumnName("width");
            });

            modelBuilder.Entity<Scenario>(entity =>
            {
                entity.ToTable("Scenario");
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.HasIndex(e => e.RollSettingsId, "IX_Relationship3");
                entity.Property(e => e.RollSettingsId).HasColumnName("roll_settings_id");
                entity.HasOne(d => d.RollSettings)
                    .WithMany(p => p.Scenarios)
                    .HasForeignKey(d => d.RollSettingsId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("RollSettings");

                entity.HasIndex(e => e.FilmProfileClusterId, "IX_Relationship2");
                entity.Property(e => e.FilmProfileClusterId).HasColumnName("film_profile_cluster_id");
                entity.HasOne(d => d.FilmProfileCluster)
                    .WithMany(p => p.Scenarios)
                    .HasForeignKey(d => d.FilmProfileClusterId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FilmProfileCluster");

                entity.Property(e => e.CrossMin)
                    .IsRequired()
                    .HasColumnName("cross_min");
                entity.Property(e => e.CrossMax)
                    .IsRequired()
                    .HasColumnName("cross_max");
                entity.Property(e => e.CrossDelta)
                    .IsRequired()
                    .HasColumnName("cross_delta");

                entity.Property(e => e.CurveMin)
                    .IsRequired()
                    .HasColumnName("curve_min");
                entity.Property(e => e.CurveMax)
                    .IsRequired()
                    .HasColumnName("curve_max");
                entity.Property(e => e.CurveDelta)
                    .IsRequired()
                    .HasColumnName("curve_delta");

                entity.Property(e => e.IsRange)
                    .IsRequired()
                    .HasColumnName("is_range");
                entity.Property(e => e.ThicknessMax)
                    .IsRequired(false)
                    .HasColumnName("thickness_max");

                entity.Property(e => e.Minutes)
                    .IsRequired()
                    .HasColumnName("minutes_learning");

                entity.Property(e => e.AveragedProfilesCount)
                    .IsRequired()
                    .HasColumnName("averaged_profiles_count");
                entity.Property(e => e.AveragedProfileWeight)
                    .IsRequired()
                    .HasColumnName("averaged_profile_weight");
                entity.Property(e => e.LastProfileWeight)
                    .IsRequired()
                    .HasColumnName("last_profile_weight");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
