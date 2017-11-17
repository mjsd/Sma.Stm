using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sma.Stm.Services.AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Services.AuthorizationService.DataAccess.EntityConfigurations
{
    public class AuthorizationItemEntityTypeConfiguration
        : IEntityTypeConfiguration<AuthorizationItem>
    {
        public void Configure(EntityTypeBuilder<AuthorizationItem> builder)
        {
            builder.ToTable("AuthorizationItem");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForNpgsqlUseSequenceHiLo("AuthorizationItem_hilo")
               .IsRequired();

            builder.Property(c => c.OrgId)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.DataId)
                .IsRequired()
                .HasMaxLength(250);
        }
    }
}