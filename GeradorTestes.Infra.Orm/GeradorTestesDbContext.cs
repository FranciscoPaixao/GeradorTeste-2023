using GeradorTestes.Dominio.ModuloDisciplina;
using GeradorTestes.Dominio.ModuloMateria;
using GeradorTestes.Dominio.ModuloQuestao;
using GeradorTestes.Dominio.ModuloTeste;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace GeradorTestes.Infra.Orm
{
    public class GeradorTestesDbContext : DbContext
    {
        public GeradorTestesDbContext()
        {            
        }

        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Materia> Materias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=GeradorTesteOrm;Integrated Security=True";
            
            optionsBuilder.UseSqlServer(connectionString);

            optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Disciplina>(disciplina =>
            {
                disciplina.ToTable("TBDisciplina");
                disciplina.Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
                disciplina.Property(d => d.Nome).HasColumnType("varchar(100)").IsRequired();
            });

            modelBuilder.Entity<Materia>(materia => {

                materia.ToTable("TBMateria");
                materia.Property(m => m.Id).IsRequired(true).ValueGeneratedOnAdd();
                materia.Property(m => m.Nome).HasColumnType("varchar(100)").IsRequired();
                materia.Property(m => m.Serie).HasConversion<int>().IsRequired();

                materia.HasOne(m => m.Disciplina)
                    .WithMany(d => d.Materias)
                    .IsRequired()
                    .HasConstraintName("FK_TBMateria_TBDisciplina")
                    .OnDelete(DeleteBehavior.NoAction);

            });

            
            modelBuilder.Ignore<Questao>();
            modelBuilder.Ignore<Alternativa>();
            modelBuilder.Ignore<Teste>();

            base.OnModelCreating(modelBuilder);
        }
    }
}