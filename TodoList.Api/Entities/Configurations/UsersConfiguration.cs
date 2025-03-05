using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Api.Entites;

namespace TodoList.Api.Entities.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => x.Username)
            .IsUnique();
        builder.Property(x => x.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasMany(x => x.Todos)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}
