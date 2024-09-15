using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository.Context
{
    public class GeladeiraContext : DbContext
    {
        public GeladeiraContext(DbContextOptions<GeladeiraContext> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<Item>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Item>()
                .Property(i => i.Classificacao)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
