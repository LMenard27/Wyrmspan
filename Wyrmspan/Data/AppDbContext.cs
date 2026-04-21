using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Dragon> Dragons { get; set; }
    public DbSet<Cave> Caves { get; set; }
    public DbSet<WyrmAction> Actions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WyrmAction>()
            .ToTable("wyrm_actions");

        modelBuilder.Entity<Cave>()
            .HasOne(c => c.Action)
            .WithMany()
            .HasForeignKey(c => c.WyrmActionId)
            .HasPrincipalKey(w => w.Id);

        modelBuilder.Entity<Cave>()
            .Property(c => c.WyrmActionId)
            .HasColumnName("wyrm_actions");
    }
}