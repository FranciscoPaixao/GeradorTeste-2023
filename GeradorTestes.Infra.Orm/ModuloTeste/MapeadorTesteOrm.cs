using GeradorTestes.Dominio.ModuloTeste;

namespace GeradorTestes.Infra.Orm.ModuloTeste
{
    public class MapeadorTesteOrm : IEntityTypeConfiguration<Teste>
    {
        public void Configure(EntityTypeBuilder<Teste> teste)
        {
            teste.ToTable("TBTeste");
            teste.Property(t => t.Id).IsRequired(true).ValueGeneratedNever();
            teste.Property(t => t.Titulo).HasColumnType("varchar(250)").IsRequired();
            teste.Property(t => t.Provao).IsRequired();
            teste.Property(t => t.DataGeracao).IsRequired();
            teste.Property(t => t.QuantidadeQuestoes).IsRequired();

            teste.Property(t => t.QuestoesSorteadas);

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

            
        }
    }
}
