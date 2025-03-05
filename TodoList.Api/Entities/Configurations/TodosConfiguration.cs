using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Api.Entites;

namespace TodoList.Api.Entities.Configurations;

public class TodosConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.Title)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x=>x.IsCompleted).HasDefaultValue(false);
        builder.Property(x=>x.Description)
            .HasMaxLength(1000);
    }
}
