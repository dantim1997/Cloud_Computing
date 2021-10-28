using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.EF
{
    public class HouseContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
               Environment.GetEnvironmentVariable("Account"),
                Environment.GetEnvironmentVariable("COSMOSDBKEY"),
                databaseName: Environment.GetEnvironmentVariable("COSMOSDB"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>(e =>
            {
                e.ToContainer(Environment.GetEnvironmentVariable("COSMOSHOUSECONTAINER"));
                e.HasKey(u => u.id);
                e.HasNoDiscriminator();
                e.HasPartitionKey(u => u.ZipCode);
                e.UseETagConcurrency();
            }
            );
        }
    }
}
