using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sma.Stm.Services.GenericMessageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.GenericMessageService.DataAccess.EntityConfigurations
{
    public class PublishedMessageEntityTypeConfiguration
        : IEntityTypeConfiguration<PublishedMessage>
    {
        public void Configure(EntityTypeBuilder<PublishedMessage> builder)
        {
            builder.ToTable("PublishedMessage");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForNpgsqlUseSequenceHiLo("PublishedMessage_hilo")
               .IsRequired();

            builder.Property(c => c.DataId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.PublishTime)
                .IsRequired();

            builder.Property(c => c.Content)
                .HasColumnType("text");
        }
    }
}