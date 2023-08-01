using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Infra.Orm.ModuloQuestao
{
    internal class MapeadorQuestaoOrm : IEntityTypeConfiguration<Questao>
    {
        public void Configure(EntityTypeBuilder<Questao> questao)
        {
            questao.ToTable("TBQuestao");
            questao.Property(q => q.Id).IsRequired(true).ValueGeneratedNever();
            questao.Property(q => q.Enunciado).HasColumnType("varchar(500)").IsRequired();
            questao.Property(q => q.JaUtilizada).IsRequired();

            questao.HasOne(q => q.Materia)
                .WithMany(m => m.Questoes)
                .IsRequired()
                .HasConstraintName("FK_TBQuestao_TBMateria")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
