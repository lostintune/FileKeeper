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
        
        builder.Entity<FileAccessEntity>(entity =>
        {
            entity.HasIndex(fa => new { fa.UserId, fa.FileId })
                .IsUnique();

            entity.HasOne<FileEntity>()
                .WithMany()
                .HasForeignKey(fa => fa.FileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<AppIdentityUser>()
                .WithMany()
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<FileEntity>(entity =>
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(f => f.Path)
                .IsRequired();
                
            entity.HasOne<AppIdentityUser>()
                .WithMany()
                .HasForeignKey(f => f.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasQueryFilter(f => !f.IsDeleted);
        });
        
        builder.Entity<AppIdentityUser>(entity =>
        {
            entity.HasQueryFilter(u => !u.IsDeleted);
        });
        
    }
}