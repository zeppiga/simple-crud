using Microsoft.EntityFrameworkCore;
using simple_crud.Data.Entities;

namespace simple_crud.Data.Contexts
{
    public class NoveltyContext : DbContext
    {
        public NoveltyContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Novelty> Novelties { get; set; }

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Novelty>().ToTable("Novelty");
            modelBuilder.Entity<File>().ToTable("File");
        }
    }
}
