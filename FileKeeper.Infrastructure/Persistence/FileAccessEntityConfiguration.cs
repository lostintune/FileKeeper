using FileKeeper.Domain.Entities;
using FileKeeper.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileKeeper.Infrastructure.Persistence;

public class FileAccessEntityConfiguration : IEntityTypeConfiguration<FileAccessEntity>
{
    public void Configure(EntityTypeBuilder<FileAccessEntity> builder)
    {
        builder.HasIndex(fa => new { fa.UserId, fa.FileId })
            .IsUnique();

        builder.HasOne<FileEntity>()
            .WithMany()
            .HasForeignKey(fa => fa.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<AppIdentityUser>()
            .WithMany()
            .HasForeignKey(fa => fa.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}