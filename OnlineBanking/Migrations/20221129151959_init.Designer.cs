﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OnlineBanking;

#nullable disable

namespace OnlineBanking.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221129151959_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OnlineBanking.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .HasColumnType("integer");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("ID");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("OnlineBanking.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.Property<string>("UserFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UserLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UserUsername")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("roleID")
                        .HasColumnType("integer");

                    b.HasKey("UserID");

                    b.HasIndex("roleID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("OnlineBanking.Models.User", b =>
                {
                    b.HasOne("OnlineBanking.Models.Role", "role")
                        .WithMany()
                        .HasForeignKey("roleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("role");
                });
#pragma warning restore 612, 618
        }
    }
}
