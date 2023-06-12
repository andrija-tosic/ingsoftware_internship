﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VacaYAY.Data;

#nullable disable

namespace VacaYAY.Data.Migrations
{
    [DbContext(typeof(VacayayDbContext))]
    [Migration("20230610160707_SeedInitialData")]
    partial class SeedInitialData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
                            Name = "Administrator",
                            NormalizedName = "Administrator"
                        },
                        new
                        {
                            Id = "dddddddd-dddd-dddd-dddd-dddddddddddd",
                            Name = "Default",
                            NormalizedName = "Default"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "923316b1-55f2-4839-96c5-679841e02aff",
                            RoleId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
                        },
                        new
                        {
                            UserId = "11111111-1111-1111-1111-111111111111",
                            RoleId = "dddddddd-dddd-dddd-dddd-dddddddddddd"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VacaYAY.Data.Models.Employee", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DaysOffNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeleteDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EmploymentEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EmploymentStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IdNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("IdNumber")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PositionId");

                    b.HasIndex("FirstName", "LastName", "EmploymentStartDate", "EmploymentEndDate");

                    b.ToTable("Employees", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "923316b1-55f2-4839-96c5-679841e02aff",
                            AccessFailedCount = 0,
                            Address = "Address",
                            DaysOffNumber = 20,
                            Email = "admin@outlook.com",
                            EmailConfirmed = true,
                            EmploymentEndDate = new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Local),
                            EmploymentStartDate = new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Local),
                            FirstName = "Administrator",
                            IdNumber = "12345",
                            InsertDate = new DateTime(2023, 6, 10, 18, 7, 7, 570, DateTimeKind.Local).AddTicks(3950),
                            LastName = "Outlook",
                            LockoutEnabled = true,
                            NormalizedEmail = "admin@outlook.com",
                            NormalizedUserName = "admin@outlook.com",
                            PasswordHash = "AQAAAAIAAYagAAAAEOfXLvYbjsZzmjan4egorJo39cNegXd6/GLYcVgMVSEWDi6gZXgqIfx+bs1qOkG5kA==",
                            PhoneNumberConfirmed = false,
                            PositionId = 1,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "admin@outlook.com"
                        },
                        new
                        {
                            Id = "11111111-1111-1111-1111-111111111111",
                            AccessFailedCount = 0,
                            Address = "Address",
                            DaysOffNumber = 20,
                            Email = "andrija@gmail.com",
                            EmailConfirmed = true,
                            EmploymentEndDate = new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Local),
                            EmploymentStartDate = new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Local),
                            FirstName = "Andrija",
                            IdNumber = "10000",
                            InsertDate = new DateTime(2023, 6, 10, 18, 7, 7, 646, DateTimeKind.Local).AddTicks(2605),
                            LastName = "Tošić",
                            LockoutEnabled = true,
                            NormalizedEmail = "andrija@gmail.com",
                            NormalizedUserName = "andrija@gmail.com",
                            PasswordHash = "AQAAAAIAAYagAAAAEHDpunlJsypwUaV2CQnD749OGkZ36sYLLJpLvXCfKM+bhRiHwuydY7syegptzravlg==",
                            PhoneNumberConfirmed = false,
                            PositionId = 4,
                            SecurityStamp = "",
                            TwoFactorEnabled = false,
                            UserName = "andrija@gmail.com"
                        });
                });

            modelBuilder.Entity("VacaYAY.Data.Models.LeaveType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("LeaveTypes");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.ToTable("Positions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Caption = "HR",
                            Description = "Human Resources"
                        },
                        new
                        {
                            Id = 2,
                            Caption = "iOS Developer",
                            Description = "Apple user"
                        },
                        new
                        {
                            Id = 3,
                            Caption = "Android Developer",
                            Description = "Android user"
                        },
                        new
                        {
                            Id = 4,
                            Caption = "MVC Intern",
                            Description = "Lizard"
                        },
                        new
                        {
                            Id = 5,
                            Caption = "Java Intern",
                            Description = "Also lizard"
                        });
                });

            modelBuilder.Entity("VacaYAY.Data.Models.VacationRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeaveTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LeaveTypeId");

                    b.ToTable("VacationRequests");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.VacationReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Approved")
                        .HasColumnType("bit");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("EmployeeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReviewerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("VacationRequestRefId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ReviewerId");

                    b.HasIndex("VacationRequestRefId")
                        .IsUnique();

                    b.ToTable("VacationReviews");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("VacaYAY.Data.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("VacaYAY.Data.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VacaYAY.Data.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("VacaYAY.Data.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VacaYAY.Data.Models.Employee", b =>
                {
                    b.HasOne("VacaYAY.Data.Models.Position", "Position")
                        .WithMany("Employees")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.VacationRequest", b =>
                {
                    b.HasOne("VacaYAY.Data.Models.Employee", "Employee")
                        .WithMany("VacationRequests")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VacaYAY.Data.Models.LeaveType", "LeaveType")
                        .WithMany("VacationRequests")
                        .HasForeignKey("LeaveTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("LeaveType");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.VacationReview", b =>
                {
                    b.HasOne("VacaYAY.Data.Models.Employee", null)
                        .WithMany("VacationReviews")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("VacaYAY.Data.Models.Employee", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("VacaYAY.Data.Models.VacationRequest", "VacationRequest")
                        .WithOne("VacationReview")
                        .HasForeignKey("VacaYAY.Data.Models.VacationReview", "VacationRequestRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reviewer");

                    b.Navigation("VacationRequest");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.Employee", b =>
                {
                    b.Navigation("VacationRequests");

                    b.Navigation("VacationReviews");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.LeaveType", b =>
                {
                    b.Navigation("VacationRequests");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.Position", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("VacaYAY.Data.Models.VacationRequest", b =>
                {
                    b.Navigation("VacationReview");
                });
#pragma warning restore 612, 618
        }
    }
}
