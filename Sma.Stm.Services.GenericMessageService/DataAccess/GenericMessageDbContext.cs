using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sma.Stm.Services.GenericMessageService.Models;
using Sma.Stm.Services.GenericMessageService.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading;

namespace Sma.Stm.Services.GenericMessageService.DataAccess
{
    public class GenericMessageDbContext : DbContext
    {
        public GenericMessageDbContext(DbContextOptions<GenericMessageDbContext> options) : base(options)
        {
        }

        public DbSet<PublishedMessage> PublishedMessages { get; set; }
        public DbSet<UploadedMessage> UploadedMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UploadedMessageEntityTypeConfiguration());
            builder.ApplyConfiguration(new PublishedMessageEntityTypeConfiguration());
        }
    }

    public class GenericMessageDbContextDesignFactory : IDesignTimeDbContextFactory<GenericMessageDbContext>
    {
        public GenericMessageDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GenericMessageDbContext>()
                .UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=Sma.Stm.Services.GenericMessageService;Pooling=true;");

            return new GenericMessageDbContext(optionsBuilder.Options);
        }
    }
}