using Microsoft.EntityFrameworkCore;
using RPP.Database.Models;

namespace RPP.Database;

public class CatHasPawsDbContext : DbContext
{
    public CatHasPawsDbContext(DbContextOptions<CatHasPawsDbContext> options)
        : base(options)
    {
    }

    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<WorkerEntity> Workers { get; set; }
    public DbSet<HomeEntity> Homes { get; set; }
    public DbSet<ToolEntity> Tools { get; set; }
    public DbSet<WorkTypeEntity> WorkTypes { get; set; }
    public DbSet<ReportEntity> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Уникальные индексы
        modelBuilder.Entity<ClientEntity>().HasIndex(x => x.PhoneNumber).IsUnique();
        modelBuilder.Entity<WorkerEntity>().HasIndex(x => x.PhoneNumber).IsUnique();
        modelBuilder.Entity<WorkerEntity>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<HomeEntity>().HasIndex(x => x.Address).IsUnique();
        modelBuilder.Entity<ToolEntity>().HasIndex(x => x.ToolName).IsUnique();
        modelBuilder.Entity<WorkTypeEntity>().HasIndex(x => x.WorkName).IsUnique();

        // Каскадное удаление
        modelBuilder.Entity<HomeEntity>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Homes)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReportEntity>()
            .HasOne(x => x.Home)
            .WithMany(x => x.Reports)
            .HasForeignKey(x => x.HomeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReportEntity>()
            .HasOne(x => x.Worker)
            .WithMany(x => x.Reports)
            .HasForeignKey(x => x.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}   