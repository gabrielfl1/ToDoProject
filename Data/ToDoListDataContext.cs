using Microsoft.EntityFrameworkCore;
using TodoList.Data.Mappings;
using TodoList.Models;

namespace TodoList.Data {
    public class ToDoListDataContext : DbContext{
        public ToDoListDataContext(DbContextOptions<ToDoListDataContext> options) : base(options) {
        }
        
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new ToDoMap());
        }
    }
}
