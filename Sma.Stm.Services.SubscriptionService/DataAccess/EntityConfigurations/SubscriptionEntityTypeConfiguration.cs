using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sma.Stm.Services.SubscriptionService.Models;

namespace Sma.Stm.Services.SubscriptionService.DataAccess.EntityConfigurations
{
    public class SubscriptionEntityTypeConfiguration
        : IEntityTypeConfiguration<SubscriptionItem>
    {
        public void Configure(EntityTypeBuilder<SubscriptionItem> builder)
        {
            builder.ToTable("SubscriptionItem");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForNpgsqlUseSequenceHiLo("SubscriptionItem_hilo")
               .IsRequired();

            builder.Property(c => c.OrgId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.ServiceId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.DataId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.CallbackEndpoint)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}