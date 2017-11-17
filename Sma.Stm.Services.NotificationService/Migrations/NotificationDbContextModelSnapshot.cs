﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Sma.Stm.Services.NotificationService.Models;
using System;

namespace Sma.Stm.Services.NotificationService.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    partial class NotificationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("Relational:Sequence:.Notification_hilo", "'Notification_hilo', '', '1', '10', '', '', 'Int64', 'False'");

            modelBuilder.Entity("Sma.Stm.Services.NotificationService.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:HiLoSequenceName", "Notification_hilo")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo);

                    b.Property<string>("Body");

                    b.Property<bool>("Fetched");

                    b.Property<string>("FromOrgId")
                        .HasMaxLength(250);

                    b.Property<string>("FromOrgName")
                        .HasMaxLength(250);

                    b.Property<string>("FromServiceId")
                        .HasMaxLength(250);

                    b.Property<DateTime>("NotificationCreatedAt");

                    b.Property<int>("NotificationSource")
                        .HasMaxLength(10);

                    b.Property<int>("NotificationType")
                        .HasMaxLength(100);

                    b.Property<DateTime>("ReceivedAt");

                    b.Property<string>("Subject")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Sma.Stm.Services.NotificationService.Models.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NotificationEndpointUrl");

                    b.HasKey("Id");

                    b.ToTable("Subscribers");
                });
#pragma warning restore 612, 618
        }
    }
}
