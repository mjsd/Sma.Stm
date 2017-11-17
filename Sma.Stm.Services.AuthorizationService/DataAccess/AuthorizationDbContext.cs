using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sma.Stm.Services.AuthorizationService.Models;
using Sma.Stm.Services.AuthorizationService.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Design;

namespace Sma.Stm.Services.AuthorizationService.DataAccess
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options) : base(options)
        {
        }

        public DbSet<AuthorizationItem> Authorizations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AuthorizationItemEntityTypeConfiguration());
        }
    }

    public class AuthorizationDbContextDesignFactory : IDesignTimeDbContextFactory<AuthorizationDbContext>
    {
        public AuthorizationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthorizationDbContext>()
                .UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=Sma.Stm.Services.AuthorizationService;Pooling=true;");

            return new AuthorizationDbContext(optionsBuilder.Options);
        }
    }
}