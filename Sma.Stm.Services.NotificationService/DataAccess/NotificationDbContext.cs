using Microsoft.EntityFrameworkCore;
using Sma.Stm.Services.NotificationService.Models;
using Sma.Stm.Services.AuthorizationService.DataAccess.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Design;

namespace Sma.Stm.Services.AuthorizationService.DataAccess
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
        }
    }

    public class NotificationDbContextDesignFactory : IDesignTimeDbContextFactory<NotificationDbContext>
    {
        public NotificationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>()
                .UseNpgsql("User ID=postgres;Password=1234;Host=localhost;Port=5432;Database=Sma.Stm.Services.NotificationService;Pooling=true;");

            return new NotificationDbContext(optionsBuilder.Options);
        }
    }
}