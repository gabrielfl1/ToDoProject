using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Models;

namespace TodoList.Data.Mappings {
    public class ToDoMap : IEntityTypeConfiguration<ToDo> {
        public void Configure(EntityTypeBuilder<ToDo> builder) {
            
            builder.ToTable("ToDos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.IsCompleted).IsRequired();
            builder.Property(x => x.Priority).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.DueDate);

        }
    }
}
