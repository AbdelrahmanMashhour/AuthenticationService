﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReposatoryPatternWithUOW.EF;

#nullable disable

namespace ReposatoryPatternWithUOW.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240606181219_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ReposatoryPatternWithUOW.Core.Models.EmailVerificationCode", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "Code");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("EmailVerificationCodes");
                });

            modelBuilder.Entity("ReposatoryPatternWithUOW.Core.Models.RefreshToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasMaxLength(44)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Revoked")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "Token");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("ReposatoryPatternWithUOW.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ReposatoryPatternWithUOW.Core.Models.EmailVerificationCode", b =>
                {
                    b.HasOne("ReposatoryPatternWithUOW.Core.Models.User", "User")
                        .WithOne("EmailVerificationCode")
                        .HasForeignKey("ReposatoryPatternWithUOW.Core.Models.EmailVerificationCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReposatoryPatternWithUOW.Core.Models.RefreshToken", b =>
                {
                    b.HasOne("ReposatoryPatternWithUOW.Core.Models.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReposatoryPatternWithUOW.Core.Models.User", b =>
                {
                    b.Navigation("EmailVerificationCode")
                        .IsRequired();

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
