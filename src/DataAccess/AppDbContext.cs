using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser, ApplicationRole, int>(options)
{
    public DbSet<FeedSource> FeedSources { get; set; }
    public DbSet<FeedItem> FeedItems { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<WriterApplicationRequest> WriterApplicationRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}