using Microsoft.EntityFrameworkCore;

namespace ContractsAPI.Models
{
    public partial class ContractContext : DbContext
    {
        //Install the following Packages
        //Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
        //Install-Package Microsoft.EntityFrameworkCore.SqlServer
        //dotnet add package Microsoft.EntityFrameworkCore.Tools
        public ContractContext(DbContextOptions<ContractContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        //To prevent Polarized table names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Service>().ToTable("Service");
            modelBuilder.Entity<Contract>().ToTable("Contract");
        }
    }
}
