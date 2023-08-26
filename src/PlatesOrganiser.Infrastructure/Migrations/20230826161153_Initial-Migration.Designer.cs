﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PlatesOrganiser.Infrastructure.Context;

#nullable disable

namespace PlatesOrganiser.Infrastructure.Migrations
{
    [DbContext(typeof(PlatesContext))]
    [Migration("20230826161153_Initial-Migration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PlatePlateUser", b =>
                {
                    b.Property<Guid>("PlatesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("PlatesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("PlatePlateUser");
                });

            modelBuilder.Entity("PlatesOrganiser.Domain.Entities.Label", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("PlatesOrganiser.Domain.Entities.Plate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("DiscogsMasterReleaseId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PrimaryLabelId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryLabelId");

                    b.ToTable("Plates");
                });

            modelBuilder.Entity("PlatesOrganiser.Domain.Entities.PlateUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PlatePlateUser", b =>
                {
                    b.HasOne("PlatesOrganiser.Domain.Entities.Plate", null)
                        .WithMany()
                        .HasForeignKey("PlatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlatesOrganiser.Domain.Entities.PlateUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlatesOrganiser.Domain.Entities.Plate", b =>
                {
                    b.HasOne("PlatesOrganiser.Domain.Entities.Label", "PrimaryLabel")
                        .WithMany()
                        .HasForeignKey("PrimaryLabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PrimaryLabel");
                });
#pragma warning restore 612, 618
        }
    }
}