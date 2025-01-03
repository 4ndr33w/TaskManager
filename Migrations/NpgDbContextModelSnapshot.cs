﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TaskManager.Api.DbContexts;

#nullable disable

namespace TaskManager.Api.Migrations
{
    [DbContext(typeof(NpgDbContext))]
    partial class NpgDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("TaskManager")
                .UseCollation("utf8_general_ci")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TaskManager.Models.CommonModels.ProjectUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUser", "TaskManager");
                });

            modelBuilder.Entity("TaskManager.Models.Desk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string[]>("Columns")
                        .HasColumnType("text[]");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("TaskIds")
                        .HasColumnType("uuid[]");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("ProjectId");

                    b.ToTable("DesksTable", "TaskManager");
                });

            modelBuilder.Entity("TaskManager.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<List<Guid>>("DesksIds")
                        .HasColumnType("uuid[]");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ProjectStatus")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<Guid>>("UsersIds")
                        .HasColumnType("uuid[]");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("ProjectsTable", "TaskManager");
                });

            modelBuilder.Entity("TaskManager.Models.TaskModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Column")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("DeskId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("ExecutorId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("DeskId");

                    b.HasIndex("ExecutorId");

                    b.ToTable("TasksTable", "TaskManager");
                });

            modelBuilder.Entity("TaskManager.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<List<Guid>>("DesksIds")
                        .HasColumnType("uuid[]");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<DateTime>("LastLoginDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("ProjectsIds")
                        .HasColumnType("uuid[]");

                    b.Property<List<Guid>>("TasksIds")
                        .HasColumnType("uuid[]");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("UsersTable", "TaskManager");
                });

            modelBuilder.Entity("TaskManager.Models.CommonModels.ProjectUser", b =>
                {
                    b.HasOne("TaskManager.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskManager.Models.Desk", b =>
                {
                    b.HasOne("TaskManager.Models.User", "Admin")
                        .WithMany("Desks")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.Project", "Project")
                        .WithMany("Desks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TaskManager.Models.Project", b =>
                {
                    b.HasOne("TaskManager.Models.User", "Admin")
                        .WithMany("Projects")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("TaskManager.Models.TaskModel", b =>
                {
                    b.HasOne("TaskManager.Models.User", "Creator")
                        .WithMany("Tasks")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.Desk", "Desk")
                        .WithMany("Tasks")
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Models.User", "Executor")
                        .WithMany()
                        .HasForeignKey("ExecutorId");

                    b.Navigation("Creator");

                    b.Navigation("Desk");

                    b.Navigation("Executor");
                });

            modelBuilder.Entity("TaskManager.Models.User", b =>
                {
                    b.HasOne("TaskManager.Models.Project", null)
                        .WithMany("Users")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("TaskManager.Models.Desk", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("TaskManager.Models.Project", b =>
                {
                    b.Navigation("Desks");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TaskManager.Models.User", b =>
                {
                    b.Navigation("Desks");

                    b.Navigation("Projects");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
