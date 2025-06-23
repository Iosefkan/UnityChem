using CalenderDatabase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Text;

public partial class UsersContext : DbContext
{
    const string DefaultConnectStr = "Host=localhost;Port=5433;Database=Users;Username=postgres;Password=user";
    const string ConnectFile = "UsersConnectionString.cfg";

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

    public UsersContext()
    {
        Database.EnsureCreated();
    }

    public UsersContext(DbContextOptions<CalenderContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            FileInfo fileInfo = new FileInfo(ConnectFile);
            if (!fileInfo.Exists)
            {
                File.WriteAllText(ConnectFile, DefaultConnectStr, Encoding.UTF8);
            }

            string connectStr = File.ReadAllText(ConnectFile, Encoding.UTF8);
            optionsBuilder.UseNpgsql(connectStr);
            //optionsBuilder.UseSqlite("Data Source=Users.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Login)
                .IsRequired()
                .HasColumnName("login");

            entity.Property(e => e.Password)
                .IsRequired()
                .HasColumnName("password_hash");

            entity.HasIndex(e => e.RoleId, "IX_Relationship100");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("UserRole");

            entity.HasData(new List<User>
            {
                new User { Id = 1, Login = "admin", Password = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", RoleId = 1 },
                new User { Id = 2, Login = "instr_calen", Password = "j8YPfn7+YJ4WI6uz1j0HODUA6UV9tfzwSG4NtXQFKHk=", RoleId = 2 },
                new User { Id = 3, Login = "instr_extr", Password = "1icbjk10mwZQAbi+bBmNNjIJXLQ1RFqJgxKusz6wWSA=", RoleId = 3 },
            });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("r_name");

            entity.HasData(new List<Role>
            {
                new Role { Id = 1, Name = "Администратор (каландр)" },
                new Role { Id = 2, Name = "Инструктор (каландр)" },
                new Role { Id = 3, Name = "Инструктор (экструзия)" },
            });
        });
    }
}
