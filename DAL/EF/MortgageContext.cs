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
            modelBuilder.Entity<Mortgage>(e =>
            {
                e.ToContainer(Environment.GetEnvironmentVariable("COSMOSMORTGAGECONTAINER"));
                e.HasKey(u => u.id);
                e.HasNoDiscriminator();
                e.HasPartitionKey(u => u.ZipCode);
                e.UseETagConcurrency();
            });
        }
    }
}
