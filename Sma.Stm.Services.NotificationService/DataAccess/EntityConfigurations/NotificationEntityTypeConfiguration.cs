using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sma.Stm.Services.NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationService.DataAccess.EntityConfigurations
{
    public class NotificationEntityTypeConfiguration
        : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForNpgsqlUseSequenceHiLo("Notification_hilo")
               .IsRequired();

            builder.Property(c => c.FromOrgId)
                .HasMaxLength(250);

            builder.Property(c => c.FromOrgName)
                .HasMaxLength(250);

            builder.Property(c => c.FromServiceId)
                .HasMaxLength(250);

            builder.Property(c => c.NotificationCreatedAt);

            builder.Property(c => c.NotificationSource)
                .HasMaxLength(10);

            builder.Property(c => c.NotificationType)
                .HasMaxLength(100);

            builder.Property(c => c.ReceivedAt);

            builder.Property(c => c.Subject)
                .HasMaxLength(250);
        }
    }
}