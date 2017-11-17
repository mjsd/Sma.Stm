﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using System;

namespace Sma.Stm.Services.AuthorizationService.Migrations
{
    [DbContext(typeof(AuthorizationDbContext))]
    [Migration("20171113083147_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("Relational:Sequence:.AuthorizationItem_hilo", "'AuthorizationItem_hilo', '', '1', '10', '', '', 'Int64', 'False'");

            modelBuilder.Entity("Sma.Stm.Services.AuthorizationService.Models.AuthorizationItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:HiLoSequenceName", "AuthorizationItem_hilo")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("DataId")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("OrgId")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("AuthorizationItem");
                });
#pragma warning restore 612, 618
        }
    }
}
