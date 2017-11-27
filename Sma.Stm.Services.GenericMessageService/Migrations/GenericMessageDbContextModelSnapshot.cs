﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Sma.Stm.Services.GenericMessageService.DataAccess;
using System;

namespace Sma.Stm.Services.GenericMessageService.Migrations
{
    [DbContext(typeof(GenericMessageDbContext))]
    partial class GenericMessageDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("Relational:Sequence:.PublishedMessage_hilo", "'PublishedMessage_hilo', '', '1', '10', '', '', 'Int64', 'False'")
                .HasAnnotation("Relational:Sequence:.UploadedMessage_hilo", "'UploadedMessage_hilo', '', '1', '10', '', '', 'Int64', 'False'");

            modelBuilder.Entity("Sma.Stm.Services.GenericMessageService.Models.PublishedMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:HiLoSequenceName", "PublishedMessage_hilo")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("DataId")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("PublishTime");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("PublishedMessage");
                });

            modelBuilder.Entity("Sma.Stm.Services.GenericMessageService.Models.UploadedMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:HiLoSequenceName", "UploadedMessage_hilo")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("DataId")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<bool>("Fetched")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("FromOrgId")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<string>("FromServiceId")
                        .IsRequired()
                        .HasMaxLength(250);

                    b.Property<DateTime>("ReceiveTime");

                    b.Property<bool>("SendAcknowledgement")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("UploadedMessage");
                });
#pragma warning restore 612, 618
        }
    }
}
