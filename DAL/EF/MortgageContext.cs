using Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.EF
{
    public class MortgageContext : DbContext
    {
        public DbSet<Mortgage> Mortgages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                Environment.GetEnvironmentVariable("Account"),
                
                Environment.GetEnvironmentVariable("COSMOSDBKEY"),
                databaseName: Environment.GetEnvironmentVariable("COSMOSDB"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mortgage>()
                .ToContainer(Environment.GetEnvironmentVariable("COSMOSUSERCONTAINER"));

            modelBuilder.Entity<Mortgage>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Mortgage>()
                .HasPartitionKey(o => o.id);

            modelBuilder.Entity<Mortgage>()
                .UseETagConcurrency();
        }
    }
}
