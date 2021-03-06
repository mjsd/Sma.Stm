﻿using Microsoft.EntityFrameworkCore;
using Sma.Stm.Services.SubscriptionService.Models;
using Sma.Stm.Services.SubscriptionService.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Design;

namespace Sma.Stm.Services.SubscriptionService.DataAccess
{
    public class SubscriptionDbContext : DbContext
    {
        public SubscriptionDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SubscriptionItem> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new SubscriptionEntityTypeConfiguration());
        }
    }

    public class AuthorizationDbContextDesignFactory : IDesignTimeDbContextFactory<SubscriptionDbContext>
    {
        public SubscriptionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SubscriptionDbContext>()
                .UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=Sma.Stm.Services.SubscriptionService;Pooling=true;");

            return new SubscriptionDbContext(optionsBuilder.Options);
        }
    }
}