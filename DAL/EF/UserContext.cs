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
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                databaseName: "BuildMyHouseDB");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToContainer(Environment.GetEnvironmentVariable("COSMOSMORTGAGECONTAINER"));
                e.HasKey(u => u.Id);
                e.HasNoDiscriminator();
                e.HasPartitionKey(u => u.Id);
                e.UseETagConcurrency();
            }
            );
        }
    }
}
