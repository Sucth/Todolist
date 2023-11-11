using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ToDolistContext : DbContext
{
    public DbSet<Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<TaskUser> TaskUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskUser>()
            .HasKey(tu => new { tu.TaskId, tu.UserId });

        modelBuilder.Entity<TaskUser>()
            .HasOne(tu => tu.Task)
            .WithMany(t => t.TaskUsers)
            .HasForeignKey(tu => tu.TaskId);

        modelBuilder.Entity<TaskUser>()
            .HasOne(tu => tu.User)
            .WithMany(u => u.TaskUsers)
            .HasForeignKey(tu => tu.UserId);
    }

    public List<Task> GetTasksWithUsers()
    {
        return Tasks.Include(t => t.TaskUsers).ThenInclude(tu => tu.User).ToList();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BddToDolistref;Trusted_Connection=True;");
    }
}
