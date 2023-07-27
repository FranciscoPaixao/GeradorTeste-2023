using GeradorTestes.Dominio.ModuloQuestao;

namespace GeradorTestes.Infra.Orm.ModuloQuestao
{
    public class MapeadorAlternativaOrm : IEntityTypeConfiguration<Alternativa>
    {
        public void Configure(EntityTypeBuilder<Alternativa> alternativa)
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
        }
    }
}
