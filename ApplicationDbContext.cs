using Microsoft.EntityFrameworkCore;
using Navigation_System.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Navigation_System.Data
{
    public class ApplicationDbContext : DBContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<NavigationItem> NavigationItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NavigationItem>()
                .HasOne(n => n.Parent)
                .WithMany(n => n.Children)
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
