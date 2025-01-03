using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Models.CommonModels;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;

namespace TaskManager.Api.DbContexts
{
    public class NpgDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public NpgDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        //public DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
               .UseCollation(_configuration.GetSection("PostgreDB")["collation"])
               .HasDefaultSchema(_configuration.GetSection("PostgreDB")["NpgDefaultSchema"]);

            modelBuilder
              .Entity<User>(e =>
              {
                  e.ToTable(_configuration.GetSection("PostgreDB")["UserTable"]);
                  e.HasKey(u => u.Id);
                  e.HasMany(p => p.Projects)
                  .WithMany(p => p.Users);
                  e.HasMany(u => u.Desks);
                  e.HasMany(u => u.Tasks);
                  
              });

            modelBuilder
            .Entity<Project>(e =>
            {
                e.ToTable(_configuration.GetSection("PostgreDB")["ProjectTable"]);
                e.HasMany(p => p.Users);
                e.HasKey(p => p.Id);

                e.HasOne(p => p.Admin);
                e.HasMany(p => p.Desks);
            });

            modelBuilder
             .Entity<Desk>(e =>
             {
                 e.ToTable(_configuration.GetSection("PostgreDB")["DeskTable"]);
                 e.HasKey(d => d.Id);

                 e.HasOne(d => d.Admin)
                 .WithMany(d => d.Desks)
                 .HasForeignKey(u => u.AdminId)
                 .HasPrincipalKey(u => u.Id)
                 .OnDelete(DeleteBehavior.Cascade);

                 e.HasOne(d => d.Project)
                 .WithMany(d => d.Desks)
                 .HasForeignKey(u => u.ProjectId)
                 .HasPrincipalKey(p => p.Id)
                 .OnDelete(DeleteBehavior.Cascade);

                 e.HasMany(d => d.Tasks);
             });

            modelBuilder
            .Entity<TaskModel>(e =>
            {
                e.ToTable(_configuration.GetSection("PostgreDB")["TaskTable"]);
                e.HasKey(d => d.Id);
                e.HasOne(t => t.Creator);
                e.HasOne(t => t.Executor);

                e.HasOne(t => t.Desk)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.DeskId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);
            });

            //modelBuilder.Entity<ProjectUser>(e =>
            //{
            //    e.ToTable(_configuration.GetSection("PostgreDB")["ProjectUserTable"]);
            //    e.HasKey(k => k.Id);
            //});
        }
    }
}
