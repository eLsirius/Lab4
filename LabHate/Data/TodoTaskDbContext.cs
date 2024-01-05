using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data
{
    public class TodoTaskDbContext : DbContext
    {
        public TodoTaskDbContext(DbContextOptions<TodoTaskDbContext> options) : base(options)
        {
        }

        public DbSet<TodoTask> TodoTasks { get; set; }
    }
}
