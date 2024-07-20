using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Customer.API.Persistence
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
        }
        public DbSet<Entities.Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.UserName)
                .IsUnique();
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.EmailAddress)
                .IsUnique();
        }
    }
    //public class CustomerContextFactory : IDesignTimeDbContextFactory<CustomerContext>
    //{
    //    public CustomerContext CreateDbContext(string[] args)
    //    {
    //        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    //        var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
    //        var connectionString = configuration.GetConnectionString("DefaultConnection");
    //        optionsBuilder.UseNpgsql(connectionString);

    //        return new CustomerContext(optionsBuilder.Options);
    //    }
    //}
}
