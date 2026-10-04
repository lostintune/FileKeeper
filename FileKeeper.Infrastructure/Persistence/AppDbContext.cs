using FileKeeper.Domain.Entities;
using FileKeeper.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FileKeeper.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>
{
    public DbSet<FileEntity> Files => Set<FileEntity>();
    public DbSet<FileAccessEntity> FileAccesses => Set<FileAccessEntity>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}