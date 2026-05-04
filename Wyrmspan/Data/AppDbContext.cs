using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Dragon> Dragons { get; set; }
    public DbSet<Cave> Caves { get; set; }
    public DbSet<WyrmAction> Actions { get; set; }
    public DbSet<User> Users { get; set; }

    /*
    This function is a vessel for Entity Framework Core to create the database context for the application. 
    
    Parameters:
    options: the DbContextOptions containing the configuration for the database context, such as the connection string and the database provider.
    */
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /*
    This function is responsible for configuring the model and the relationships between the entities in the database.
    
    Parameters:
    modelBuilder: the ModelBuilder object that is used to configure the model and the relationships between the entities in the database.
    */
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