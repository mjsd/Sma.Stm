using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sma.Stm.Services.GenericMessageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.GenericMessageService.DataAccess.EntityConfigurations
{
    public class UploadedMessageEntityTypeConfiguration
        : IEntityTypeConfiguration<UploadedMessage>
    {
        public void Configure(EntityTypeBuilder<UploadedMessage> builder)
        {
            builder.ToTable("UploadedMessage");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForNpgsqlUseSequenceHiLo("UploadedMessage_hilo")
               .IsRequired();

            builder.Property(c => c.DataId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.FromOrgId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.FromServiceId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.ReceiveTime)
                .IsRequired();

            builder.Property(c => c.Fetched)
                .HasDefaultValue(false);

            builder.Property(c => c.SendAcknowledgement)
                .HasDefaultValue(false);

            builder.Property(c => c.Content)
                .HasColumnType("text");
        }
    }
}