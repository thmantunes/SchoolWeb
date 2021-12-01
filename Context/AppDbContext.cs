using School.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using School.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Diagnostics;

namespace School.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<HorarioTurma> HorariosTurmas { get; set; }
        public DbSet<Historico> Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HorarioTurma>()
            .HasKey(o => new { o.HorarioId, o.TurmaId });

            /*
            modelBuilder.Entity<DisciplinaDependencia>()
            .HasKey(e => new { e.PrecursoraId, e.SucessoraId });

            modelBuilder.Entity<DisciplinaDependencia>()
                .HasOne(e => e.Precursora)
                .WithMany(e => e.Sucessoras)
                .HasForeignKey(e => e.PrecursoraId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<DisciplinaDependencia>()
            .HasOne(e => e.Sucessora)
            .WithMany(e => e.Precursoras)
            .HasForeignKey(e => e.SucessoraId)
            .OnDelete(DeleteBehavior.Restrict); 
            */
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
                .LogTo(message=>Debug.WriteLine(message), new[] { RelationalEventId.CommandExecuted })
                //.LogTo(Debug.Write, new[] { RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging();

    }

}

