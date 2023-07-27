using GeradorTestes.Dominio.ModuloDisciplina;

namespace GeradorTestes.Infra.Orm.ModuloDisciplina
{
    public class MapeadorDisciplinaOrm : IEntityTypeConfiguration<Disciplina>
    {
        public void Configure(EntityTypeBuilder<Disciplina> disciplinaBuilder)
        {
            disciplinaBuilder.ToTable("TBDisciplina");

            disciplinaBuilder.Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();

            disciplinaBuilder.Property(d => d.Nome).HasColumnType("varchar(100)").IsRequired();
        }
    }
}
