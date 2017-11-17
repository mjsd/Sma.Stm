using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sma.Stm.Services.NotificationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationService.DataAccess.EntityConfigurations
{
    public class SubscriberEntityTypeConfiguration
        : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToTable("Subscriber");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForNpgsqlUseSequenceHiLo("Subscriber_hilo")
               .IsRequired();

            builder.Property(c => c.NotificationEndpointUrl)
                .HasMaxLength(250);
        }
    }
}