using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.EF
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
               Environment.GetEnvironmentVariable("Account"),
                Environment.GetEnvironmentVariable("COSMOSDBKEY"),
                databaseName: Environment.GetEnvironmentVariable("COSMOSDB"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToContainer(Environment.GetEnvironmentVariable("COSMOSUSERCONTAINER"));
                e.HasKey(u => u.id);
                e.HasNoDiscriminator();
                e.HasPartitionKey(u => u.ZipCode);
                e.UseETagConcurrency();
            }
            );
        }
    }
}
