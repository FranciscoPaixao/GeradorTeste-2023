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
        public DbSet<Questao> Questoes { get; set; }
        public DbSet<Teste> Testes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=GeradorTesteOrm;Integrated Security=True";
            
            optionsBuilder.UseSqlServer(connectionString);

            //optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);

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
            
            modelBuilder.Entity<Questao>(questao => {

                questao.ToTable("TBQuestao");
                questao.Property(q => q.Id).IsRequired(true).ValueGeneratedOnAdd();
                questao.Property(q => q.Enunciado).HasColumnType("varchar(500)").IsRequired();
                questao.Property(q => q.JaUtilizada).IsRequired();

                questao.HasOne(q => q.Materia)
                    .WithMany(m => m.Questoes)
                    .IsRequired()
                    .HasConstraintName("FK_TBQuestao_TBMateria")
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Alternativa>(alternativa =>
            {
                alternativa.ToTable("TBAlternativa");
                alternativa.Property(a => a.Id).IsRequired(true).ValueGeneratedOnAdd();
                alternativa.Property(a => a.Letra).IsRequired();
                alternativa.Property(a => a.Resposta).HasColumnType("varchar(100)").IsRequired();
                alternativa.Property(a => a.Correta).IsRequired();

                alternativa.HasOne(a => a.Questao)
                    .WithMany(q => q.Alternativas)
                    .IsRequired()
                    .HasConstraintName("FK_TBAlternativa_TBQuestao")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teste>(teste => {

                teste.ToTable("TBTeste");
                teste.Property(t => t.Id).IsRequired(true).ValueGeneratedOnAdd();
                teste.Property(t => t.Titulo).HasColumnType("varchar(250)").IsRequired();
                teste.Property(t => t.Provao).IsRequired();
                teste.Property(t => t.DataGeracao).IsRequired();
                teste.Property(t => t.QuantidadeQuestoes).IsRequired();

                teste.HasOne(t => t.Disciplina)
                    .WithMany()
                    .IsRequired()
                    .HasConstraintName("FK_TBTeste_TBDisciplina")
                    .OnDelete(DeleteBehavior.NoAction);

                teste.HasOne(t => t.Materia)
                    .WithMany()
                    .IsRequired(false)
                    .HasConstraintName("FK_TBTeste_TBMateria")
                    .OnDelete(DeleteBehavior.NoAction);

                teste.HasMany(t => t.Questoes)
                    .WithMany()
                    .UsingEntity(x => x.ToTable("TBTeste_TBQuestao"));

                teste.Ignore(t => t.QuestoesSorteadas);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}