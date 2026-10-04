using FileKeeper.Domain.Entities;
using FileKeeper.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileKeeper.Infrastructure.Persistence;

public class FileEntityConfiguration : IEntityTypeConfiguration<FileEntity>
{
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(f => f.Path)
            .IsRequired();

        builder.HasOne<AppIdentityUser>()
            .WithMany()
            .HasForeignKey(f => f.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(f => !f.IsDeleted);
    }
}