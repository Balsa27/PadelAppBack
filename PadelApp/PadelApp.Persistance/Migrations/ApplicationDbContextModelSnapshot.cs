﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PadelApp.Persistance.EFC;

#nullable disable

namespace PadelApp.Persistance.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PadelApp.Domain.Aggregates.Court", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("CourtImages")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ProfileImage")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<TimeSpan>("WorkingEndTime")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("WorkingStartTime")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("Courts", (string)null);
                });

            modelBuilder.Entity("PadelApp.Domain.Aggregates.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AppleId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("GoogleId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasMaxLength(100)
                        .HasColumnType("tinyint(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Players", (string)null);
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("BookerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CourtId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("BookerId");

                    b.HasIndex("CourtId");

                    b.ToTable("Bookings", (string)null);
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("AppleId")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("GoogleId")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasMaxLength(100)
                        .HasColumnType("tinyint(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<TimeSpan>("WorkingEndingHours")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("WorkingStartHours")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.ToTable("Organizations", (string)null);
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.OrganizationCourt", b =>
                {
                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CourtId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("OrganizationId1")
                        .HasColumnType("char(36)");

                    b.HasKey("OrganizationId", "CourtId");

                    b.HasIndex("CourtId");

                    b.HasIndex("OrganizationId1");

                    b.ToTable("OrganizationCourt");
                });

            modelBuilder.Entity("PadelApp.Domain.ValueObjects.Price", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("CourtId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Days")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("DaysOfWeek");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("TimeEnd")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("TimeStart")
                        .HasColumnType("time(6)");

                    b.HasKey("Id");

                    b.HasIndex("CourtId");

                    b.ToTable("CourtPrices", (string)null);
                });

            modelBuilder.Entity("PadelApp.Persistance.Dbos.BookingAttendee", b =>
                {
                    b.Property<Guid>("BookingId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.HasKey("BookingId", "PlayerId");

                    b.ToTable("BookingAttendees", (string)null);
                });

            modelBuilder.Entity("PadelApp.Persistance.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Error")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("PadelApp.Domain.Aggregates.Court", b =>
                {
                    b.OwnsOne("PadelApp.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CourtId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("CourtId");

                            b1.ToTable("CourtAddress", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("CourtId");
                        });

                    b.Navigation("Address")
                        .IsRequired();
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.Booking", b =>
                {
                    b.HasOne("PadelApp.Domain.Aggregates.Player", null)
                        .WithMany()
                        .HasForeignKey("BookerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PadelApp.Domain.Aggregates.Court", null)
                        .WithMany("Bookings")
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.Organization", b =>
                {
                    b.OwnsOne("PadelApp.Domain.ValueObjects.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("OrganizationId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.HasKey("OrganizationId");

                            b1.ToTable("OrganizationAddress", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.OwnsOne("PadelApp.Domain.ValueObjects.ContactInfo", "ContactInfo", b1 =>
                        {
                            b1.Property<Guid>("OrganizationId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasColumnType("longtext");

                            b1.Property<string>("WebsiteUrl")
                                .HasColumnType("longtext");

                            b1.HasKey("OrganizationId");

                            b1.ToTable("OrganizationContactInfo", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("OrganizationId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("ContactInfo");
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.OrganizationCourt", b =>
                {
                    b.HasOne("PadelApp.Domain.Aggregates.Court", null)
                        .WithMany()
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PadelApp.Domain.Entities.Organization", null)
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PadelApp.Domain.Entities.Organization", null)
                        .WithMany("Courts")
                        .HasForeignKey("OrganizationId1");
                });

            modelBuilder.Entity("PadelApp.Domain.ValueObjects.Price", b =>
                {
                    b.HasOne("PadelApp.Domain.Aggregates.Court", null)
                        .WithMany("Prices")
                        .HasForeignKey("CourtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PadelApp.Domain.Aggregates.Court", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Prices");
                });

            modelBuilder.Entity("PadelApp.Domain.Entities.Organization", b =>
                {
                    b.Navigation("Courts");
                });
#pragma warning restore 612, 618
        }
    }
}
